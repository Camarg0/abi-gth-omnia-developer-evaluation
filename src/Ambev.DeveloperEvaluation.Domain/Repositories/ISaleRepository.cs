using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for User entity operations
/// </summary>
public interface ISaleRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<bool> CancelAsync(Sale sale, CancellationToken cancellationToken = default);

    Task<IEnumerable<Sale>> GetByCustomerIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Sale>> GetByBranchIdAsync(Guid id, CancellationToken cancellationToken = default);
}
