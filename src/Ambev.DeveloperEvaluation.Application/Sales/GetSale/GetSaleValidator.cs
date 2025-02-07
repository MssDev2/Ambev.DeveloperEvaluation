using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines validation rules for sale retrieval.
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Id is required
    /// </remarks>
    public GetSaleValidator()
    {
        RuleFor(sale => sale.Id).NotEmpty();
    }
}
