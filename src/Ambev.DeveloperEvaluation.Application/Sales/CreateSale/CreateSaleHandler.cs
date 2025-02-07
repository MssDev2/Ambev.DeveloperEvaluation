using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Net;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for CreateSaleCommand</param>
    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Products)
        {
            if (item.Quantity > 20)
            {
                throw new ValidationException("Cannot sell more than 20 identical items.");
            }
            if (item.Quantity < 4)
            {
                item.Discount = 0.00m;
            }
            else if (item.Quantity >= 4 && item.Quantity < 10)
            {
                item.Discount = item.Quantity * item.UnitPrice * 0.10m;
            }
            else if (item.Quantity >= 10 && item.Quantity <= 20)
            {
                item.Discount = item.Quantity * item.UnitPrice * 0.20m;
            }
        }

        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetBysaleNumberAsync(command.SaleNumber, cancellationToken);
        if (existingSale != null)
            throw new HttpRequestException($"Sale with Number {command.SaleNumber} already exists", null, HttpStatusCode.Conflict);

        var sale = _mapper.Map<Sale>(command);

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}
