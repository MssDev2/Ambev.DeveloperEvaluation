using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale made in the system
/// This entity follows domain-driven design principles and includes business rules validation.
/// Using DDD principles, we reference entities from other domains by applying the External Identities pattern.
/// This involves denormalizing entity descriptions to maintain domain boundaries and improve performance.
/// </summary>
public class Sale : BaseEntity
{
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
    public List<SaleItem> Products { get; set; } = [];

    /// <summary>
    /// Indicates whether the sale was cancelled
    /// </summary>
    public SaleStatus IsCancelled { get; set; }

    /// <summary>
    /// Total sale amount
    /// Calculated by summing the total amount of each product in the sale
    /// </summary>
    public decimal TotalSaleAmount => Products.Sum(p => p.TotalAmount);


    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules.
    /// </summary>
    /// <returns>
    /// A <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    /// <remarks>
    /// <listheader>The validation includes checking:</listheader>
    /// <list type="bullet">SaleNumber is greater than 0</list>
    /// </remarks>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
