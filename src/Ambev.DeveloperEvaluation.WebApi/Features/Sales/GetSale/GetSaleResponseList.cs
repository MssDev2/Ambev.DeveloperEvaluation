using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Response model for GetSaleList operation with a list of sales
/// </summary>
public class GetSaleResponseList
{
    /// <summary>
    /// List of sales
    /// </summary>
    public List<Sale>? SaleList { get; set; }

    public GetSaleResponseList(List<Sale>? saleList)
    {
        SaleList = saleList;
    }
}
