using src.Domain.Models;

namespace src.Application.Interfaces.Repositories
{
    public interface IDAPI : IDisposable
    {
        IRepository<User> Users { get; }

        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
