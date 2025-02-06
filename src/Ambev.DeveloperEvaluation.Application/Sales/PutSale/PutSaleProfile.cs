using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.PutSale;

/// <summary>
/// Profile for mapping between Sale entity and PutSaleResponse
/// </summary>
public class PutSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for PutSale operation
    /// </summary>
    public PutSaleProfile()
    {
        CreateMap<PutSaleCommand, Sale>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        CreateMap<PutSaleItemCommand, SaleItem>();

        CreateMap<Sale, PutSaleResult>();
    }
}
