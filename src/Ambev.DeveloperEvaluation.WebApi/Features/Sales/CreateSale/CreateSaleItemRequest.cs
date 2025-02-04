using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Represents a request to create a new sale item
/// </summary>
public class CreateSaleItemRequest
{
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