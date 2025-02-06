using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Response model for GetSale operation with a list of sales
/// </summary>
public class GetSaleResultList
{
    /// <summary>
    /// List of sales
    /// </summary>
    public List<Sale>? SaleList { get; set; }

    public GetSaleResultList(List<Sale>? saleList)
    {
        SaleList = saleList;
    }
}
