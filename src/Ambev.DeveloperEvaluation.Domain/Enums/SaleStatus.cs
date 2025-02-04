namespace Ambev.DeveloperEvaluation.Domain.Enums;

/// <summary>
/// Indicates whether the sale was cancelled
/// Must be Cancelled or Not Cancelled
/// </summary>
public enum SaleStatus
{
    Unknown = 0,
    Cancelled,
    NotCancelled
}
