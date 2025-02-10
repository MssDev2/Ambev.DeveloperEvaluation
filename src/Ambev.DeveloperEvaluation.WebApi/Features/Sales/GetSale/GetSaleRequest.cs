using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Represents a request to create a new sale in the system.
/// </summary>
public class GetSaleRequest
{
    /// <summary>
    /// The unique identifier of the sale to retrieve
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Order fields (Field1 asc, Field2 desc)
    /// </summary>
    public string OrderFields { get; set; } = string.Empty;

    /// <summary>
    /// Filters (Key: Field, Value: Value)
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }
}