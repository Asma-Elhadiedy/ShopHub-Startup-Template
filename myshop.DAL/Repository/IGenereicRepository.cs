namespace myshop.DAL.Repository;

public interface IGenereicRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetItemAsync(Expression<Func<T, bool>> predicate);

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate);
    IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate);

    void Update(T entity);
    void Remove(T entity);
    void Add(T entity);
}
