using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;
using System.Net;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class GetSaleHandlerList : IRequestHandler<GetSaleCommandList, GetSaleResultList>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of GetSaleHandlerList
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetSaleCommand</param>
    public GetSaleHandlerList(ISaleRepository saleRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the GetSaleCommand request
    /// </summary>
    /// <param name="command">The GetSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<GetSaleResultList> Handle(GetSaleCommandList command, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidatorList();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        Sale? findSale = null;
        List<Sale> sales = new List<Sale>();

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
        
        if (findSale == null)
            sales = await _saleRepository.GetListAsync(command.Page, command.PageSize, command.OrderField, command.OrderAscending, cancellationToken: cancellationToken);
        else
            sales.Add(findSale);

        return _mapper.Map<GetSaleResultList>(sales);
    }
}
