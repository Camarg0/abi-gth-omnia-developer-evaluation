using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsResultAndCallsRepository()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.InstanceCommand();

            _ = _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
                .Returns((Sale)null);

            var domainSale = new Sale
            {
                SaleNumber = command.SaleNumber,
                SaleDate = command.SaleDate,
                CustomerId = command.CustomerId,
                CustomerName = command.CustomerName,
                CustomerEmail = command.CustomerEmail,
                BranchId = command.BranchId,
                BranchName = command.BranchName
            };
            _mapper.Map<Sale>(command).Returns(domainSale);

            foreach (var itemCommand in command.Items)
            {
                var domainItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = itemCommand.ProductId,
                    ProductName = itemCommand.ProductName,
                    ProductDescription = itemCommand.ProductDescription,
                    Quantity = itemCommand.Quantity,
                    UnitPrice = itemCommand.UnitPrice
                };
                _mapper.Map<SaleItem>(itemCommand).Returns(domainItem);
            }

            _saleRepository.CreateAsync(domainSale, Arg.Any<CancellationToken>())
                .Returns(domainSale);

            var expectedResult = new CreateSaleResult { Id = domainSale.Id };
            _mapper.Map<CreateSaleResult>(domainSale).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeSameAs(expectedResult);
            await _saleRepository.Received(1).GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<Sale>(command);

            foreach (var itemCommand in command.Items)
            {
                _mapper.Received(1).Map<SaleItem>(itemCommand);
            }

            await _saleRepository.Received(1).CreateAsync(domainSale, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<CreateSaleResult>(domainSale);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand();

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_DuplicateSaleNumber_ThrowsInvalidOperationException()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.InstanceCommand();

            _saleRepository.GetBySaleNumberAsync(command.SaleNumber, Arg.Any<CancellationToken>())
                .Returns(new Sale { Id = Guid.NewGuid(), SaleNumber = command.SaleNumber });

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Sale with number {command.SaleNumber} already exists");
        }
    }
}
