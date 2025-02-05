using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class GetSaleCommandValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, must be greater than 0
    /// </remarks>
    public GetSaleCommandValidator()
    {
        //RuleFor(sale => sale.SaleNumber).GreaterThan(0);
        RuleFor(sale => sale.Id).NotEmpty();
    }
}
