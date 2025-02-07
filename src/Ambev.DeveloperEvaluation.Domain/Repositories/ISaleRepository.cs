using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>  
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update sale in the database
    /// </summary>
    /// <param name="saleOld">The original sale</param>
    /// <param name="saleNew">The sale with new values</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale</returns>  
    Task<Sale> UpdateAsync(Sale saleOld, Sale saleNew, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sale by their sale number
    /// </summary>
    /// <param name="saleNumber">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    Task<Sale?> GetBysaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a sales list by page and size order by a specific field
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <param name="orderField">The field to order by</param>
    /// <param name="orderAscending">True to order ascending, false to order descending</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of sales</returns>
    Task<(List<Sale> SalesList, int TotalCount)> GetListAsync(int page, int pageSize, string orderField = "", bool orderAscending = true, CancellationToken cancellationToken = default);
}