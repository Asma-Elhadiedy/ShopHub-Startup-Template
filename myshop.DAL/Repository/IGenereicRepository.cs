
namespace myshop.DAL.Repository;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes);
    Task<T1?> GetItemSelectedAsync<T1>(Expression<Func<T, bool>> predicate, Expression<Func<T, T1>> selector, CancellationToken ct = default);
    Task<bool> IsExistAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);


    IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);
    Task<IEnumerable<T1>> GetAllSelectedAsync<T1>(Expression<Func<T, bool>>? predicate, Expression<Func<T, T1>> selector, CancellationToken ct = default);
    Task<int> GetCountAsync(Expression<Func<T, bool>>? predicate, CancellationToken ct = default);
    Task<int> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, int>> selector, CancellationToken ct = default);
        

    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    
    bool IsItemChanged();

    Task<int> BulkInsertAsync(IEnumerable<T> entities, CancellationToken ct = default);
    Task<int> BulkUpdateAsync(Expression<Func<T, bool>> predicate, Action<UpdateSettersBuilder<T>> setters, bool ignoreQueryFilters = false, CancellationToken ct = default);
    Task<int> BulkDeleteAsync(Expression<Func<T, bool>> predicate, bool ignoreQueryFilters = false, CancellationToken ct = default);
}
