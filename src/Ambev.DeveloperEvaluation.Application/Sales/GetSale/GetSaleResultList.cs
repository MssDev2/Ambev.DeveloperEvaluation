using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Response model for GetSaleList operation with a list of sales
/// </summary>
public class GetSaleResultList
{
    /// <summary>
    /// List of GetSaleResult
    /// </summary>
    public List<GetSaleResult> SaleList { get; set; } = [];

    public int TotalCount { get; set; }

    public GetSaleResultList()
    {
        SaleList = [];
    }
}
