using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSaleByNumber;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByCustomerId;
using Ambev.DeveloperEvaluation.Application.Sales.GetSalesByBranchId;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSaleByNumber;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByCustomerId;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSalesByBranchId;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class SaleProfile : Profile
{
    public SaleProfile()
    {
        // CreateSale mappings
        // http -> application
        CreateMap<CreateSaleRequest, CreateSaleCommand>();
        CreateMap<CreateSaleItemRequest, CreateSaleItemCommandRequest>();

        // application -> http
        CreateMap<CreateSaleResult, CreateSaleResponse>();
        CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();

        // DeleteSales
        // http -> app
        CreateMap<DeleteSaleRequest, DeleteSaleCommand>();
        // app -> http
        CreateMap<DeleteSaleResult, DeleteSaleResponse>();

        // Cancel
        // http -> app
        CreateMap<CancelSaleRequest, CancelSaleCommand>();
        // app -> http
        CreateMap<CancelSaleResult, CancelSaleResponse>();
        
        // GetSale mappings
        // http -> application
        CreateMap<GetSaleRequest, GetSaleQuery>();

        // application -> http
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<GetSaleItemResult, GetSaleItemResponse>();

        // GetSaleByNumber mappings
        // http -> application
        CreateMap<GetSaleByNumberRequest, GetSaleByNumberQuery>();

        // application -> http
        CreateMap<GetSaleByNumberResult, GetSaleByNumberResponse>();
        CreateMap<GetSaleByNumberItemResult, GetSaleByNumberItemResponse>();

        // GetSalesByCustomerId mappings
        // http -> application
        CreateMap<GetSalesByCustomerIdRequest, GetSalesByCustomerIdQuery>();

        // application -> http
        CreateMap<GetSalesByCustomerIdResult, GetSalesByCustomerIdResponse>();
        CreateMap<CustomerSaleResult, CustomerSaleResponse>();
        CreateMap<CustomerSaleItemResult, CustomerSaleItemResponse>();

        // GetSalesByBranchId mappings
        // http -> application
        CreateMap<GetSalesByBranchIdRequest, GetSalesByBranchIdQuery>();

        // application -> http
        CreateMap<GetSalesByBranchIdResult, GetSalesByBranchIdResponse>();
        CreateMap<BranchSaleResult, BranchSaleResponse>();
        CreateMap<BranchSaleItemResult, BranchSaleItemResponse>();

        // Domain -> Application (for all Get operations)
        CreateMap<Sale, GetSaleResult>();
        CreateMap<SaleItem, GetSaleItemResult>();
        CreateMap<Sale, GetSaleByNumberResult>();
        CreateMap<SaleItem, GetSaleByNumberItemResult>();
        CreateMap<Sale, CustomerSaleResult>();
        CreateMap<SaleItem, CustomerSaleItemResult>();
        CreateMap<Sale, BranchSaleResult>();
        CreateMap<SaleItem, BranchSaleItemResult>();
    }
}
