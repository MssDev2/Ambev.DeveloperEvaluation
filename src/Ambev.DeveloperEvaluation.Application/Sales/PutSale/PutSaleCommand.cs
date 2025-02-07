using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.PutSale;

/// <summary>
/// Command to update a sale
/// </summary>
/// <remarks>
/// This command is used to capture the required data to update a existing sale 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="PutSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="PutSaleValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class PutSaleCommand : IRequest<PutSaleResult>
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
    public SaleStatus IsCancelled { get; set; } = SaleStatus.NotCancelled;

    /// <summary>
    /// List of products that belong to the sale
    /// </summary>
    public List<PutSaleItemCommand> Products { get; set; } = [];

    /// <summary>
    /// Initializes a new instance of PutSaleCommand
    /// </summary>
    /// <param name="id">The ID of the sale to retrieve</param>
    public PutSaleCommand(Guid id)
    {
        Id = id;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new PutSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class PutSaleItemCommand
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

    /// <summary>
    /// Discount applied to the product (readonly)
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for the product (readonly)
    /// </summary>
    public decimal TotalAmount => Quantity * UnitPrice - Discount;
}
