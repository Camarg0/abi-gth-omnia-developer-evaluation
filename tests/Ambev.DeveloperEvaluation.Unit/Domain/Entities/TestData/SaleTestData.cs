using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData
{
    public static class SaleTestData
    {
        public static Sale InstanceSale()
        {
            var sale = new Sale
            {
                SaleNumber = "S-1000",
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                CustomerName = "Test Customer",
                CustomerEmail = "test@example.com",
                BranchId = Guid.NewGuid(),
                BranchName = "Main Branch"
            };

            var item = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Test Product",
                ProductDescription = "Description",
                Quantity = 5,
                UnitPrice = 100m
            };
            sale.AddItem(item);

            return sale;
        }
    }
}
