using Ambev.DeveloperEvaluation.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a product sold in a sale
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Numeric identifier for the sale item within the sale
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [Column(TypeName = "decimal(18,6)")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Product unit price
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Discount applied to the product
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for the product
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
}
