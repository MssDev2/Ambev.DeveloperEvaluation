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
    /// Order fields (Field1 asc, Field2 desc) 
    /// </summary>
    public string OrderFields { get; set; } = string.Empty;

    /// <summary>
    /// Filters (Key: Field, Value: Value)
    /// </summary>
    public Dictionary<string, string>? Filters { get; set; }

    /// <summary>
    /// Initializes a new instance of GetUSaleCommand
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="orderFields">The order field</param>
    /// <param name="filters">The filters</param>

    public GetSaleCommandList(int page, int pageSize, string orderFields, Dictionary<string, string>? filters)
    {
        Page = page;
        PageSize = pageSize;
        OrderFields = orderFields;
        Filters = filters;
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