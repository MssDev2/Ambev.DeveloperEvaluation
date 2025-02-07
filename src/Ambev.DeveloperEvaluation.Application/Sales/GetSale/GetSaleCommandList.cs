using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Command for getting a list of sales
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale, 
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
/// that returns a <see cref="GetSaleResultList"/>.
/// 
/// The data provided in this command is validated using the 
/// <see cref="GetSaleValidator"/> which extends 
/// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
/// populated and follow the required rules.
/// </remarks>
public class GetSaleCommandList : IRequest<GetSaleResultList>
{
    /// <summary>
    /// The unique identifier of the created sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Sale number
    /// </summary>
    public int SaleNumber { get; set; }

    /// <summary>
    /// Page number
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Order field
    /// </summary>
    public string OrderField { get; set; } = string.Empty;

    /// <summary>
    /// Order ascending or descending
    /// </summary>
    public bool OrderAscending { get; set; }

    /// <summary>
    /// Initializes a new instance of GetUSaleCommand
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="orderField">The order field</param>
    /// <param name="orderAscending">True to order ascending, false to order descending</param>
    public GetSaleCommandList(int page, int pageSize, string orderField, bool orderAscending)
    {
        Page = page;
        PageSize = pageSize;
        OrderField = orderField;
        OrderAscending = orderAscending;
    }

    public ValidationResultDetail Validate()
    {
        var validator = new GetSaleValidatorList();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}