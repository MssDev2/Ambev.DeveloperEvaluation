using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for sale creation.
/// </summary>
public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - Sale number: Required, must be greater than 0
    /// - IsCancelled: Cannot be Unknown
    /// - Customer: Required, length between 3 and 50 characters
    /// - Branch: Required, length between 3 and 50 characters
    /// - Products: Required
    /// - Products: Must have valid discount based on quantity
    /// </remarks>
    public CreateSaleRequestValidator()
    {
        RuleFor(sale => sale.SaleNumber).GreaterThan(0);
        RuleFor(sale => sale.IsCancelled).NotEqual(SaleStatus.Unknown);
        RuleFor(sale => sale.Customer).NotEmpty();
        RuleFor(sale => sale.Branch).NotEmpty();
        RuleFor(sale => sale.Products).NotEmpty();
    }
}