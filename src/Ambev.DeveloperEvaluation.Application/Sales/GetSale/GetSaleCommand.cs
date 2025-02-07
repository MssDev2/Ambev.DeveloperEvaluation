using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for retrieving a sale
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="GetSaleResult"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="GetSaleValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class GetSaleCommand : IRequest<GetSaleResult>
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
    /// Initializes a new instance of GetUSaleCommand
    /// </summary>
    /// <param name="id">The ID of the sale to retrieve</param>
    public GetSaleCommand(Guid id)
    {
        Id = id;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new GetSaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}