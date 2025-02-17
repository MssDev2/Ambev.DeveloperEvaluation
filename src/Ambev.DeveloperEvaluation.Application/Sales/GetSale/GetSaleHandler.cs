﻿using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Net;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetSaleCommand</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleCommand request
    /// </summary>
    /// <param name="command">The GetSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<GetSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Sale? findSale = null;
        if (command.SaleNumber > 0)
        {
            findSale = await _saleRepository.GetBysaleNumberAsync(command.SaleNumber, cancellationToken);
            if (findSale == null)
                throw new HttpRequestException($"Sale with Number {command.SaleNumber} not found", null, HttpStatusCode.NotFound);
        }
        else if (command.Id != Guid.Empty)
        {
            findSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (findSale == null)
                throw new HttpRequestException($"Sale with ID {command.Id} not found", null, HttpStatusCode.NotFound);
        }

        return _mapper.Map<GetSaleResult>(findSale);
    }
}
