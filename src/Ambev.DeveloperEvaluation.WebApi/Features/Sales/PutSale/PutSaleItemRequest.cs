using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.PutSale;

/// <summary>
/// Represents a request to create a new sale item
/// </summary>
public class PutSaleItemRequest
{
    /// <summary>
    /// The unique identifier of item sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Numeric identifier for the sale item within the sale
    /// </summary>
    public int SaleItemId { get; set; }

    /// <summary>
    /// Sale internal identifier
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Product name or identifier
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// Product quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Product unit price
    /// </summary>
    public decimal UnitPrice { get; set; }
}