using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommandList that defines validation rules for sale retrieval.
/// </summary>
public class GetSaleValidatorList : AbstractValidator<GetSaleCommandList>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleValidatorList with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Page must be greater than 0
    /// - PageSize must be greater than 0
    /// </remarks>
    public GetSaleValidatorList()
    {
        RuleFor(sale => sale.Page).GreaterThan(0);
        RuleFor(sale => sale.PageSize).GreaterThan(0);
    }
}
