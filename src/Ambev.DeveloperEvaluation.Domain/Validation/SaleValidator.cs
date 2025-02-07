using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(sale => sale.SaleNumber)
            .GreaterThan(0)
            .LessThan(int.MaxValue)
            .WithMessage("Sale Number must be greater than 0 and less than " + int.MaxValue.ToString());
    }
}
