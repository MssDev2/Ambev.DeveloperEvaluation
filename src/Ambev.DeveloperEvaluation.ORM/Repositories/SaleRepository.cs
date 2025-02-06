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
                throw new KeyNotFoundException($"SaleId {saleItemNew.SaleId} does not match the current Sold");

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
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool trackChanges = true)
    {
        IQueryable<Sale> query = _context.Sales.Include(s => s.Products);

        if (!trackChanges)
            query = query.AsNoTracking();

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

    /// <summary>
    /// Retrieves a sales list by page and size order by a specific field
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="orderField">The field to order by</param>
    /// <param name="orderAscending">True to order ascending, false to order descending</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of sales</returns>
    public async Task<List<Sale>> GetListAsync(int page, int pageSize, string orderField = "", bool orderAscending = true, CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Sale>().AsQueryable();
        if (page > 0 && pageSize > 0)
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
        
        if (!string.IsNullOrEmpty(orderField))
            query = orderAscending ? query.OrderBy(e => EF.Property<object>(e, orderField)) : query.OrderByDescending(e => EF.Property<object>(e, orderField));

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }
}
