
namespace myshop.DAL.UOW;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IDomainModelMarker;
    Task<bool> ExecuteInTransactionAsync(Func<Task<bool>> action, CancellationToken ct = default);
    Task<(bool isSucess, T1 resultInfo)> ExecuteInTransactionAsync<T1>(Func<Task<(bool, T1)>> action, CancellationToken ct = default);
    Task<bool> ExecuteInTransactionAllContextAsync(Func<Task<bool>> action, CancellationToken ct = default);
    Task<int> CompleteAsync(CancellationToken ct = default);
}

