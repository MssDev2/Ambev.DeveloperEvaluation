using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Indicates whether the sale was cancelled
/// Must be Cancelled or Not Cancelled
/// </summary>
public enum SaleStatus
{
    [Display(Name = "Unknown", Description = "The sale status is unknown.")]
    Unknown = 0,
    [Display(Name = "Cancelled", Description = "The sale has been cancelled.")]
    Cancelled,
    [Display(Name = "NotCancelled", Description = "The sale has not been cancelled.")]
    NotCancelled
}
