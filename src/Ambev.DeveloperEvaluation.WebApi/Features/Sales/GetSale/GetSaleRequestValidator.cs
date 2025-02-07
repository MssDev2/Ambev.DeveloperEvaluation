using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Validator for GetSaleRequest that defines validation rules for sale creation.
/// </summary>
public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleRequestValidator with defined validation rules.
    /// </summary>
    public GetSaleRequestValidator()
    {
        When(x => x.Id == Guid.Empty, () =>
        {
            RuleFor(x => x.Page)
                .NotEmpty()
                .WithMessage("Page is required");
            RuleFor(x => x.PageSize)
                .NotEmpty()
                .WithMessage("Page size is required");
        });
    }
}