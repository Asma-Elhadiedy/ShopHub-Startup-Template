namespace myshop.BLL.UOF;

public interface IUnitOfWork : IDisposable
{
    IGenereicRepository<TEntity> Repository<TEntity>() where TEntity : class;
    Task<int> CompleteAsync();
}

