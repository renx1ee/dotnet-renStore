using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Application.Abstractions.Repository;

public interface IAnswerComplainRepository
{
    Task<Guid> CreateAsync(AnswerComplainEntity complain, CancellationToken cancellationToken);
    Task UpdateAsync(AnswerComplainEntity complain, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AnswerComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        AnswerComplainSortBy sortBy = AnswerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<AnswerComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<AnswerComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<AnswerComplainEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AnswerComplainSortBy sortBy = AnswerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    Task<IEnumerable<AnswerComplainEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AnswerComplainSortBy sortBy = AnswerComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}