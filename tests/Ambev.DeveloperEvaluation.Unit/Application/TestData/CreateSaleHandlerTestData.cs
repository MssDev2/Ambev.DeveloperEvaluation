using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid SaleItem entities.
    /// The generated sale items will have valid:
    /// - Product (valid format)
    /// - Quantity (1-20)
    /// - UnitPrice (1.00-100.00)
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> createSaleItemHandlerFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.Product, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => Math.Round(f.Random.Decimal(1.00m, 100.00m), 2));

    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// The generated sales will have valid:
    /// - Customer (valid format)
    /// - Branch (Country - City)
    /// - SaleNumber (100-999)
    /// - SaleDate (Past date)
    /// - IsCancelled (Cancelled or NotCancelled)
    /// - Products (1-5 valid sale items)
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        .RuleFor(u => u.Customer, f => f.Internet.UserName())
        .RuleFor(u => u.Branch, f => f.Address.Country() + " - " + f.Address.City())
        .RuleFor(u => u.SaleNumber, f => f.Random.Number(100, 999))
        .RuleFor(u => u.SaleDate, f => f.Date.Past())
        .RuleFor(u => u.IsCancelled, f => f.PickRandom(SaleStatus.Cancelled, SaleStatus.NotCancelled))
        .RuleFor(u => u.Products, f => createSaleItemHandlerFaker.Generate(f.Random.Int(1, 5)));

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated sale will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }
}
