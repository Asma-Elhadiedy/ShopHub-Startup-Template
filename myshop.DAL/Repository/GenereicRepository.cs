

namespace myshop.DAL.Repository;

public class GenereicRepository<T>(ApplicationDbContext _dbContext) : IGenericRepository<T> where T : class
{

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
    public async Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
    }
    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate)
    {
        //if (typeof(T) == typeof(Product))
        //    return await _dbContext.Set<T>().Include("Category").ToListAsync();

        return predicate is null
            ? await _dbContext.Set<T>().AsNoTracking().ToListAsync()
            : await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync();
    }
    public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate)
    {
        return predicate is null
            ? _dbContext.Set<T>().AsNoTracking()
            : _dbContext.Set<T>().Where(predicate);
    }


    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

    public void Add(T entity) => _dbContext.Set<T>().Add(entity);


}
