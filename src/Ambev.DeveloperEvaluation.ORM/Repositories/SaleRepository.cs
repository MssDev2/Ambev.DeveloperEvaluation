using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of SaleRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Update sale in the database
    /// </summary>
    /// <param name="saleOld">The original sale</param>
    /// <param name="saleNew">The sale with new values</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale with tracking changes context</returns>
    public async Task<Sale> UpdateAsync(Sale saleOld, Sale saleNew, CancellationToken cancellationToken = default)
    {
        _context.Entry(saleOld).CurrentValues.SetValues(saleNew);

        var saleItemNewIds = saleNew.Products.Select(si => si.Id).ToList();
        foreach (var existingSaleItem in saleOld.Products.ToList())
        {
            if (!saleItemNewIds.Contains(existingSaleItem.Id))
                _context.Entry(existingSaleItem).State = EntityState.Deleted;
        }

        foreach (var saleItemNew in saleNew.Products)
        {
            if (saleOld.Id != saleItemNew.SaleId)
            {
                if (saleItemNew.SaleId == Guid.Empty)
                    saleItemNew.SaleId = saleOld.Id;
                else
                    throw new KeyNotFoundException($"SaleId {saleItemNew.SaleId} does not match the current Sold");
            }

            if (saleItemNew.Id == Guid.Empty)
                saleOld.Products.Add(saleItemNew);
            else
            {
                var existingSaleItem = saleOld.Products.FirstOrDefault(si => si.Id == saleItemNew.Id);
                if (existingSaleItem == null)
                    throw new KeyNotFoundException($"Id {saleItemNew.Id} does not found in SoldItem. To create a new SoldItem, the Id field must be null.");
                else
                {
                    if (saleItemNew.SaleItemId <= 0 && existingSaleItem.SaleItemId > 0)
                        saleItemNew.SaleItemId = existingSaleItem.SaleItemId;

                    _context.Entry(existingSaleItem).CurrentValues.SetValues(saleItemNew);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return saleOld;
    }

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        IQueryable<Sale> query = _context.Sales.Include(s => s.Products);
        return await query.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by their sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBysaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Products)
            .FirstOrDefaultAsync(u => u.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static string ToPascalCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        if (str.Length == 1)
            return char.ToUpper(str[0]).ToString();

        return char.ToUpper(str[0]) + str.Substring(1);
    }

    private static Dictionary<string, bool> ParseOrderField(string orderField)
    {
        var orderDict = new Dictionary<string, bool>();

        if (string.IsNullOrEmpty(orderField))
            return orderDict;

        var fields = orderField.Split(',');
        foreach (var field in fields)
        {
            var parts = field.Trim().Split(' ');
            var fieldName = parts[0];
            var orderAscending = parts.Length < 2 || parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase);

            orderDict.Add(fieldName, orderAscending);
        }

        return orderDict;
    }

    /// <summary>
    /// Retrieves a sales list by page and size order by a specific field
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="orderFields">The field to order by and direction (field1 asc, field2 desc)</param>
    /// <param name="filters">Dictionary with filters (key: field name, value: filter value)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of sales</returns>
    public async Task<(List<Sale> SalesList, int TotalCount)> GetListFilterAsync(int page, int pageSize, string orderFields = "", Dictionary<string, string>? filters = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Sale>()
            .Include(s => s.Products)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        // Apply filters
        if (filters != null)
        {
            foreach (var filter in filters)
            {
                if (filter.Key.StartsWith("_min") || filter.Key.StartsWith("_max"))
                {
                    var fieldName = filter.Key.Substring(4);
                    var isMin = filter.Key.StartsWith("_min");
                    fieldName = ToPascalCase(fieldName);

                    if (typeof(Sale).GetProperty(fieldName) == null)
                    {
                        var errorMessage = $"The field '{fieldName}' does not exist in the Sale entity.";
                        throw new Exception(errorMessage);
                    }

                    if (decimal.TryParse(filter.Value, out decimal decimalValue))
                    {
                        query = isMin
                            ? query.Where(e => EF.Property<decimal>(e, fieldName) >= decimalValue)
                            : query.Where(e => EF.Property<decimal>(e, fieldName) <= decimalValue);
                    }
                    else if (DateTime.TryParse(filter.Value, out DateTime dateValue))
                    {
                        var dateValueUTC = DateTime.SpecifyKind(dateValue, DateTimeKind.Utc);
                        query = isMin
                            ? query.Where(e => EF.Property<DateTime>(e, fieldName) >= dateValueUTC)
                            : query.Where(e => EF.Property<DateTime>(e, fieldName) <= dateValueUTC);
                    }
                }
                else
                {
                    string fieldName = ToPascalCase(filter.Key) ?? "";
                    if (typeof(Sale).GetProperty(fieldName) == null)
                    {
                        var errorMessage = $"The field '{fieldName}' does not exist in the Sale entity.";
                        throw new Exception(errorMessage);
                    }
                    if (filter.Value.Contains('*'))
                        query = query.Where(e => EF.Functions.Like(EF.Property<string>(e, fieldName), filter.Value.Replace("*", "%")));
                    else
                        query = query.Where(e => EF.Property<string>(e, fieldName) == filter.Value);
                }
            }
        }

        // Apply ordering
        if (!string.IsNullOrEmpty(orderFields))
        {
            orderFields = orderFields.Replace("\"", "");
            var orderDict = ParseOrderField(orderFields);
            foreach (var kvp in orderDict)
            {
                var fieldName = ToPascalCase(kvp.Key);
                if (typeof(Sale).GetProperty(fieldName) == null)
                {
                    var errorMessage = $"The field '{fieldName}' does not exist in the Sale entity.";
                    throw new Exception(errorMessage);
                }
                query = kvp.Value ? query.OrderBy(e => EF.Property<object>(e, fieldName)) : query.OrderByDescending(e => EF.Property<object>(e, fieldName));
            }
        }

        // Apply pagination
        if (page > 0 && pageSize > 0)
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return (await query.AsNoTracking().ToListAsync(cancellationToken), totalCount);
    }
}