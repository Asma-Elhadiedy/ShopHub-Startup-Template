

namespace myshop.DAL.Repository;

public class GenereicRepository<T>(ApplicationDbContext _dbContext) : IGenereicRepository<T> where T : class
{

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
    public async Task<T?> GetItemAsync(Expression<Func<T, bool>> match)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(match);
    }


    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
    {
        return predicate is null
            ? await _dbContext.Set<T>().AsNoTracking().ToListAsync()
            : await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
    }
    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Set<T>().Where(predicate);
    }


    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

}
