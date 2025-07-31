using System;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class CreateSaleHandlerTestData
{
    public static CreateSaleCommand InstanceCommand()
    {
        return new CreateSaleCommand
        {
            SaleNumber = "1",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "Joaozinho",
            CustomerEmail = "joaozinho@teste.com",
            BranchId = Guid.NewGuid(),
            BranchName = "branch 1",
            Items = new List<CreateSaleItemCommand>
                    {
                        new CreateSaleItemCommand
                        {
                            ProductId = Guid.NewGuid(),
                            ProductName = "Prod A",
                            ProductDescription = "Desc A",
                            Quantity = 5,
                            UnitPrice = 10m
                        }
                    }
        };
    }
}
