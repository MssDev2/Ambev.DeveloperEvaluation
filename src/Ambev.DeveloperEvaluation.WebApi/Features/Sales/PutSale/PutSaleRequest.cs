using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.PutSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class PutSaleRequest
{
    /// <summary>
    /// The unique identifier of the created sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// Sale date and time
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Customer name or identifier
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Branch where the sale was made, can be a name or identifier
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the initial status of the sale account.
    /// </summary>
    public SaleStatus IsCancelled { get; set; }

    /// <summary>
    /// List of products that belong to the sale
    /// </summary>
    public List<PutSaleItemRequest> Products { get; set; } = new List<PutSaleItemRequest>();

}