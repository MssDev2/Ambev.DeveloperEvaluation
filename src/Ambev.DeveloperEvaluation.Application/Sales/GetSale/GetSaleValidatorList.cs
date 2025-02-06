using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class GetSaleValidatorList : AbstractValidator<GetSaleCommandList>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, must be greater than 0
    /// </remarks>
    public GetSaleValidatorList()
    {
        RuleFor(sale => sale.Page).GreaterThan(0);
        RuleFor(sale => sale.PageSize).GreaterThan(0);
        RuleFor(sale => sale.OrderField).NotEmpty();
        RuleFor(sale => sale.OrderAscending).NotNull();
    }
}
