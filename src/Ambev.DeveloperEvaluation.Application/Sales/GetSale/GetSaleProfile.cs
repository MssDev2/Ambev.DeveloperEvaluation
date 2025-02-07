using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Profile for mapping between Sale entity and GetSaleResponse
/// </summary>
public class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetSale operation
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>();

        //CreateMap<List<Sale>, GetSaleResult>();
                
        CreateMap<List<Sale>, GetSaleResultList>()
            .ConstructUsing((src, context) => new GetSaleResultList { SaleList = context.Mapper.Map<List<GetSaleResult>>(src) });
    }
}
