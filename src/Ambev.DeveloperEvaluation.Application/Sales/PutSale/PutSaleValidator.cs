﻿using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.PutSale;

/// <summary>
/// Validator for PutSaleCommand that defines validation rules for sale update.
/// </summary>
public class PutSaleValidator : AbstractValidator<PutSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the PutSaleCommandValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, must be greater than 0
    /// - Status: Cannot be set to Unknown
    /// - Products: Must have valid discount based on quantity
    /// - Products: Cannot sell more than 20 identical items
    /// </remarks>
    public PutSaleValidator()
    {
        RuleFor(sale => sale.SaleNumber).GreaterThan(0);
        RuleFor(sale => sale.IsCancelled).NotEqual(SaleStatus.Unknown);

        RuleForEach(request => request.Products).ChildRules(items =>
        {
            items.RuleFor(x => x.Quantity).Must(BeWithinAllowedRange)
                .WithMessage("Cannot sell more than 20 identical items.");
            items.RuleFor(x => x).Must(HaveValidDiscount)
                .WithMessage("Invalid discount based on quantity.");
        });
    }
    
    private bool BeWithinAllowedRange(decimal quantity)
    {
        return quantity <= 20;
    }

    private bool HaveValidDiscount(PutSaleItemCommand item)
    {
        if (item.Quantity < 4 && item.Discount > 0)
        {
            return false;
        }
        if (item.Quantity >= 4 && item.Quantity < 10 && item.Discount <= 0)
        {
            return false;
        }
        if (item.Quantity >= 10 && item.Quantity <= 20 && item.Discount <= 0)
        {
            return false;
        }
        return true;
    }
}
