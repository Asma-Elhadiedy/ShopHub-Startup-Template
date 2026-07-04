

namespace myshop.BLL.UOF;

public class UnitOfWork(ApplicationDbContext _context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IGenereicRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        return (IGenereicRepository<TEntity>)_repositories.GetOrAdd(
            typeof(TEntity),
            _ => new GenereicRepository<TEntity>(_context)
        );
    }
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
    public void Dispose() => _context.Dispose();
}