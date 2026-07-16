
namespace myshop.BLL.UOW;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IDomainModelMarker;
    Task<int> CompleteAsync();
}

