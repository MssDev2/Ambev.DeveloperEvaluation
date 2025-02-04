using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Enums;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="CreateSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="CreateSaleCommandValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
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
    /// Gets or sets the initial status of the sale account.
    /// </summary>
    public SaleStatus Status { get; set; }

    /// <summary>
    /// List of products that belong to the sale
    /// </summary>
    public List<CreateSaleItemCommand> Products { get; set; } = new List<CreateSaleItemCommand>();


    public ValidationResultDetail Validate()
    {
        var validator = new CreateSaleCommandValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}

public class CreateSaleItemCommand
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

    /// <summary>
    /// Discount applied to the product (readonly)
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total amount for the product (readonly)
    /// </summary>
    public decimal TotalAmount => Quantity * UnitPrice - Discount;
}
