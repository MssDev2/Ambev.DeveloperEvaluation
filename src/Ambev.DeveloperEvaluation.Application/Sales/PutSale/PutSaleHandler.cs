using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Net;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.PutSale;

/// <summary>
/// Handler for processing PutSaleCommand requests
/// </summary>
public class PutSaleHandler : IRequestHandler<PutSaleCommand, PutSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of PutSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for PutSaleCommand</param>
    public PutSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the PutSaleCommand request
    /// </summary>
    /// <param name="command">The PutSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<PutSaleResult> Handle(PutSaleCommand command, CancellationToken cancellationToken)
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

        var validator = new PutSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale == null)
            throw new HttpRequestException($"Sale with ID {command.Id} not found", null, HttpStatusCode.NotFound);

        var sale = _mapper.Map<Sale>(command);

        var updatedSale = await _saleRepository.UpdateAsync(existingSale, sale, cancellationToken);
        var result = _mapper.Map<PutSaleResult>(updatedSale);
        return result;
    }
}
