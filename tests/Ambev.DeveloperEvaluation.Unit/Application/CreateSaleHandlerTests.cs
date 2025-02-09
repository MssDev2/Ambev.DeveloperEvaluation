using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Customer = command.Customer,
            IsCancelled = command.IsCancelled,
            Products = command.Products.ConvertAll(p => new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = p.Product,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            })
        };

        foreach (var product in sale.Products)
        {
            product.SaleId = sale.Id;
        }

        var result = new CreateSaleResult
        {
            Id = sale.Id,
        };

        // Mock
        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);

        // Assert
        foreach (var item in command.Products)
        {
            var expectedDiscount = item.Quantity switch
            {
                < 4 => 0.00m,
                >= 4 and < 10 => item.Quantity * item.UnitPrice * 0.10m,
                >= 10 and <= 20 => item.Quantity * item.UnitPrice * 0.20m,
                _ => throw new ValidationException("Cannot sell more than 20 identical items.")
            };

            item.Discount.Should().Be(expectedDiscount);
        }

        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Customer = command.Customer,
            IsCancelled = command.IsCancelled,
            Products = command.Products.ConvertAll(p => new SaleItem
            {
                Id = Guid.NewGuid(),
                Product = p.Product,
                Quantity = p.Quantity,
                UnitPrice = p.UnitPrice
            })
        };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c =>
            c.SaleNumber == command.SaleNumber &&
            c.SaleDate == command.SaleDate &&
            c.Branch == command.Branch &&
            c.Customer == command.Customer &&
            c.IsCancelled == command.IsCancelled &&
            c.Products.Count == command.Products.Count));
    }

    [Fact(DisplayName = "Given valid sale data When Quantity is less than 4 Then no discount is applied")]
    public async Task Handle_ShouldApplyNoDiscount_WhenQuantityIsLessThan4()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 1,
            Products =
                [
                    new CreateSaleItemCommand { Quantity = 3, UnitPrice = 10.00m }
                ]
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        command.Products[0].Discount.Should().Be(0.00m);
    }

    [Fact(DisplayName = "Given valid sale data When Quantity is between 4 and 9 Then 10% discount is applied")]
    public async Task Handle_ShouldApply10PercentDiscount_WhenQuantityIsBetween4And9()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 1,
            Products =
                [
                    new() { Quantity = 5, UnitPrice = 10.00m }
                ]
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        command.Products[0].Discount.Should().Be(5 * 10.00m * 0.10m);
    }

    [Fact(DisplayName = "Given valid sale data When Quantity is between 10 and 20 Then 20% discount is applied")]
    public async Task Handle_ShouldApply20PercentDiscount_WhenQuantityIsBetween10And20()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 1,
            Products =
                [
                    new CreateSaleItemCommand { Quantity = 15, UnitPrice = 10.00m }
                ]
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        command.Products[0].Discount.Should().Be(15 * 10.00m * 0.20m);
    }

    [Fact(DisplayName = "Given valid sale data When Quantity is greater than 20 Then throws validation exception")]
    public async Task Handle_ShouldThrowValidationException_WhenQuantityIsGreaterThan20()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            SaleNumber = 1,
            Products =
                [
                    new CreateSaleItemCommand { Quantity = 21, UnitPrice = 10.00m }
                ]
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
