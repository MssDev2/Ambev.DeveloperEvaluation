using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.PutSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.PutSale;

/// <summary>
/// Profile for mapping between Application and API PutSale responses
/// </summary>
public class PutSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for PutSale feature
    /// </summary>
    public PutSaleProfile()
    {
        CreateMap<PutSaleRequest, PutSaleCommand>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<Guid, PutSaleCommand>()
            .ConstructUsing(id => new PutSaleCommand(id));

        CreateMap<PutSaleItemRequest, PutSaleItemCommand>();

        CreateMap<PutSaleResult, PutSaleResponse>();
    }
}
