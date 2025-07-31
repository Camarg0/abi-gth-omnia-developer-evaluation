using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
    {
        [Fact]
        public void Given_NewSaleItem_When_AddItem_NoDiscount_Then_ItemAddedAndTotalCorrect()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var initialTotalAmount = sale.TotalAmount;
            
            var newItem = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Another",
                Quantity = 3,
                UnitPrice = 50m
            };

            // Act
            sale.AddItem(newItem);

            // Assert
            Assert.Contains(sale.Items, i => i.Id == newItem.Id);
            var expected = initialTotalAmount + (3 * 50m);
            Assert.Equal(expected, sale.TotalAmount);
        }

        [Fact]
        public void Given_ExistingSaleItem_When_AddItemAgain_Then_QuantityIncreased()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var existing = sale.Items.First();
            var originalQuantity = existing.Quantity;
            var originalTotal = sale.TotalAmount;

            // Act
            var duplicateItem = new SaleItem
            {
                Id = existing.Id,
                ProductId = existing.ProductId,
                ProductName = existing.ProductName,
                ProductDescription = existing.ProductDescription,
                Quantity = 2,
                UnitPrice = existing.UnitPrice
            };
            sale.AddItem(duplicateItem);

            // Assert
            var updated = sale.Items.Single(i => i.Id == existing.Id);
            Assert.Equal(originalQuantity + 2, updated.Quantity);
            Assert.True(sale.TotalAmount > originalTotal);
        }

        [Theory]
        [InlineData(3, 0)]
        [InlineData(5, 0.10)]
        [InlineData(15, 0.20)]
        public void Given_ItemQuantities_When_AddItem_Then_DiscountApplies(int quantity, double expectedRate)
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var price = 10m;
            var item = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = "teste",
                Quantity = quantity,
                UnitPrice = price
            };

            // Act
            sale.AddItem(item);

            // Assert
            var added = sale.Items.Single(i => i.ProductId == item.ProductId);
            var expectedDiscount = price * quantity * (decimal)expectedRate;
            Assert.Equal(expectedDiscount, added.Discount);
            var expectedTotalItem = price * quantity - expectedDiscount;
            Assert.Equal(expectedTotalItem, added.TotalAmountItem);
        }

        [Fact]
        public void Given_TooManyItems_When_AddItem_Then_Throws()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var wrongItem = new SaleItem
            {
                ProductId = Guid.NewGuid(),
                ProductName = "X",
                Quantity = 21,
                UnitPrice = 1m
            };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => sale.AddItem(wrongItem));
            Assert.Contains("The maximum limit is 20 items per product", ex.Message);
        }

        [Fact]
        public void Given_ExistingItem_When_UpdateItem_Then_ValuesChange()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var item = sale.Items.First();
            var newQuantity = item.Quantity + 3;
            var newPrice = item.UnitPrice + 20m;

            // Act
            sale.UpdateItem(item.Id, item.ProductName, item.ProductDescription, newQuantity, newPrice);

            // Assert
            var updated = sale.Items.Single(i => i.Id == item.Id);
            Assert.Equal(newQuantity, updated.Quantity);
            Assert.Equal(newPrice, updated.UnitPrice);
            Assert.Equal((newQuantity * newPrice) - updated.Discount, updated.TotalAmountItem);
            Assert.Equal(sale.TotalAmount, sale.Items.Sum(i => i.TotalAmountItem));
        }

        [Fact]
        public void Given_NonexistentItem_When_UpdateItem_Then_Throws()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var id = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                sale.UpdateItem(id, "Test", "Test", 1, 1m));
        }

        [Fact]
        public void Given_ExistingItem_When_RemoveItem_Then_StatusCancelledAndTotalZero()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            var item = sale.Items.First();

            // Act
            sale.RemoveItem(item.Id);

            // Assert
            var removed = sale.Items.Single(i => i.Id == item.Id);
            Assert.Equal(SaleItemStatus.Cancelled, removed.Status);
            Assert.Equal(0m, sale.TotalAmount);
        }

        [Fact]
        public void Given_NonexistentItem_When_RemoveItem_Then_Throws()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                sale.RemoveItem(Guid.NewGuid()));
        }

        [Fact]
        public void Given_ActiveSale_When_Cancel_Then_SaleAndItemsCancelled()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();

            // Act
            sale.Cancel();

            // Assert
            Assert.Equal(SaleStatus.Cancelled, sale.Status);
            Assert.All(sale.Items, i => Assert.Equal(SaleItemStatus.Cancelled, i.Status));
        }

        [Fact]
        public void Given_SaleWithItems_When_ClearItems_Then_ItemsCleared()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();

            // Act
            sale.ClearItems();

            // Assert
            Assert.Empty(sale.Items);
            Assert.Equal(0m, sale.TotalAmount);
        }

        [Fact]
        public void Given_CancelledSale_When_ClearItems_Then_Throws()
        {
            // Arrange
            var sale = SaleTestData.InstanceSale();
            sale.Cancel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(sale.ClearItems);
        }

        [Fact]
        public void Given_SaleData_When_Validate_Then_CorrectValidity()
        {
            // valid
            var sale = SaleTestData.InstanceSale();
            var result = sale.Validate();
            Assert.True(result.IsValid);

            var wrongSale = new Sale
            {
                SaleDate = DateTime.UtcNow,
                CustomerId = Guid.Empty,
                CustomerName = "",
                BranchId = Guid.Empty,
                BranchName = ""
            };
            var validationResult = wrongSale.Validate();
            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.Errors);
        }
    }
}
