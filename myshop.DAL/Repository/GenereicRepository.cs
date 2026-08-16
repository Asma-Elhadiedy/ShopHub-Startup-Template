
namespace myshop.DAL.Repository;

public class GenereicRepository<T>(ApplicationDbContext _dbContext) : IGenericRepository<T> where T : class
{

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().FindAsync(id, ct);
    }
    public async Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, ct);
    }
    public async Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(predicate, ct);
    }
    public async Task<T1?> GetItemSelectedAsync<T1>(Expression<Func<T, bool>> predicate, Expression<Func<T, T1>> selector, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().Where(predicate).Select(selector).FirstOrDefaultAsync(ct);
    }
    public async Task<bool> IsExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        return predicate is null
            ? await _dbContext.Set<T>().AnyAsync(ct)
            : await _dbContext.Set<T>().Where(predicate).AnyAsync(ct);
    }

    public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate)
    {
        return predicate is null
            ? _dbContext.Set<T>().AsNoTracking()
            : _dbContext.Set<T>().Where(predicate).AsNoTracking();
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        return predicate is null
            ? await _dbContext.Set<T>().AsNoTracking().ToListAsync(ct)
            : await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync(ct);
    }
    public async Task<IEnumerable<T1>> GetAllSelectedAsync<T1>(Expression<Func<T, bool>>? predicate, Expression<Func<T, T1>> selector, CancellationToken ct = default)
    {
        return predicate is null
            ? await _dbContext.Set<T>().AsNoTracking().Select(selector).ToListAsync(ct)
            : await _dbContext.Set<T>().Where(predicate).AsNoTracking().Select(selector).ToListAsync(ct);
    }

    public async Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default)
    {
        return predicate is null
            ? await _dbContext.Set<T>().CountAsync(ct)
            : await _dbContext.Set<T>().CountAsync(predicate, ct);
    }
    public async Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector, CancellationToken ct = default)
    {
        return await _dbContext.Set<T>().Where(predicate).SumAsync(selector, ct);
    }


    public void Add(T entity) => _dbContext.Set<T>().Add(entity);
    public void Update(T entity) => _dbContext.Set<T>().Update(entity);
    public void Remove(T entity) => _dbContext.Set<T>().Remove(entity);

    public bool IsItemChanged()
        => _dbContext.ChangeTracker.HasChanges();


    public async Task<int> BulkInsertAsync(IEnumerable<T> entities, CancellationToken ct = default)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities, ct);
        return await _dbContext.SaveChangesAsync(ct);
    }
    public async Task<int> BulkUpdateAsync(Expression<Func<T, bool>> predicate, Action<UpdateSettersBuilder<T>> setters, bool ignoreQueryFilters = false, CancellationToken ct = default)
    {
        return ignoreQueryFilters
            ? await _dbContext.Set<T>().IgnoreQueryFilters().Where(predicate).ExecuteUpdateAsync(setters, ct)
            : await _dbContext.Set<T>().Where(predicate).ExecuteUpdateAsync(setters, ct);
    }
    public async Task<int> BulkDeleteAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false, CancellationToken ct = default)
    {
        return ignoreQueryFilters
            ? await _dbContext.Set<T>().IgnoreQueryFilters().Where(predicate).ExecuteDeleteAsync(ct)
            : await _dbContext.Set<T>().Where(predicate).ExecuteDeleteAsync();
    }
}
