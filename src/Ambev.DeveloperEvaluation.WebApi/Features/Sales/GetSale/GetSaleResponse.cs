using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API response model for GetSale operation
/// </summary>
public class GetSaleResponse
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
    /// List of products that belong to the sale
    /// </summary>
    public List<SaleItem> Products { get; set; } = new List<SaleItem>();

    /// <summary>
    /// Indicates whether the sale was cancelled
    /// </summary>
    [JsonIgnore]
    public SaleStatus IsCancelled { get; set; }
    public string Cancelled => IsCancelled.ToString();

    /// <summary>
    /// Total sale amount
    /// Calculated by summing the total amount of each product in the sale
    /// </summary>
    public decimal TotalSaleAmount => Products.Sum(p => p.TotalAmount);
}
