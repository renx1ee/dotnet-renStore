using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;

public interface IQuestionComplainRepository
{
    Task<Guid> CreateAsync(QuestionComplainEntity complain, CancellationToken cancellationToken);
    Task UpdateAsync(QuestionComplainEntity complain, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<QuestionComplainEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<QuestionComplainEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<QuestionComplainEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<QuestionComplainEntity?>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);

    Task<IEnumerable<QuestionComplainEntity?>> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        QuestionComplainSortBy sortBy = QuestionComplainSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}