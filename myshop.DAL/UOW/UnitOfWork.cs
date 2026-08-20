
using System.Transactions;

namespace myshop.DAL.UOW;

public class UnitOfWork(ApplicationDbContext _context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IDomainModelMarker
    {
        return (IGenericRepository<TEntity>)_repositories.GetOrAdd(
            typeof(TEntity),
            _ => new GenereicRepository<TEntity>(_context)
        );
    }

    public async Task<bool> ExecuteInTransactionAsync(Func<Task<bool>> action, CancellationToken ct = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            if (result)
                await transaction.CommitAsync(ct);
            else
                await transaction.RollbackAsync(ct);

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<(bool isSucess, T1 resultInfo)> ExecuteInTransactionAsync<T1>(Func<Task<(bool, T1)>> action, CancellationToken ct = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            if (result.Item1)
                await transaction.CommitAsync(ct);
            else
                await transaction.RollbackAsync(ct);

            return result;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<bool> ExecuteInTransactionAllContextAsync(Func<Task<bool>> action, CancellationToken ct = default)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var result = await action();
        if (result)
            scope.Complete();

        return result;

    }

    public async Task<int> CompleteAsync(CancellationToken ct = default)
    {
        var entries = _context.ChangeTracker.Entries();
        var changedEntries = entries.Where(e => e.State == EntityState.Modified && e.Entity is DomainModelBase);
        var deletedEntries = entries.Where(e => e.State == EntityState.Deleted && e.Entity is DomainModelBase);

        var dateTime = DateTime.UtcNow;
        foreach (var entry in changedEntries)
        {
            ((DomainModelBase)entry.Entity).UpdatedAt = dateTime;
        }
        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Modified;
            ((DomainModelBase)entry.Entity).IsDeleted = true;
            ((DomainModelBase)entry.Entity).DeletedAt = dateTime;
        }

        return await _context.SaveChangesAsync(ct);
    }


    public void Dispose() => _context.Dispose();
}
