using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing GetSaleCommandList requests
/// </summary>
public class GetSaleHandlerList : IRequestHandler<GetSaleCommandList, GetSaleResultList>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetSaleHandlerList
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetSaleCommand</param>
    public GetSaleHandlerList(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetSaleCommandList request
    /// </summary>
    /// <param name="command">The GetSaleList command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<GetSaleResultList> Handle(GetSaleCommandList command, CancellationToken cancellationToken)
    {
        var validator = new GetSaleValidatorList();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var (salesList, totalCount) = await _saleRepository.GetListAsync(command.Page, command.PageSize, command.OrderField, command.OrderAscending, cancellationToken: cancellationToken);

        var ret = _mapper.Map<GetSaleResultList>(salesList);
        ret.TotalCount = totalCount;
        return ret;
    }
}
