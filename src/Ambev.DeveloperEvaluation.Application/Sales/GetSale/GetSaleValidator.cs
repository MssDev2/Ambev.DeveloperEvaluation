﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for GetSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the GetSaleValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: Required, must be greater than 0
    /// </remarks>
    public GetSaleValidator()
    {
        //RuleFor(sale => sale.SaleNumber).GreaterThan(0);
        RuleFor(sale => sale.Id).NotEmpty();
    }
}
