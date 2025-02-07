namespace Ambev.DeveloperEvaluation.Application.Sales.PutSale;

/// <summary>
/// Represents the response returned after successfully updating a sale.
/// </summary>
/// <remarks>
/// This response contains the unique identifier of the updated sale,
/// which can be used for subsequent operations or reference.
/// </remarks>
public class PutSaleResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale.
    /// </summary>
    /// <value>A GUID that uniquely identifies the sale in the system.</value>
    public Guid Id { get; set; }
}
