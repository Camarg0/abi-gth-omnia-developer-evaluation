using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public class ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<Sale> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Sale?> GetByCustomerIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Sale?> GetByBranchIdAsync(Guid id, CancellationToken cancellationToken = default);
}
