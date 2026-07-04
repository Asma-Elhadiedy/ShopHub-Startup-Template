namespace myshop.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
{
    public async Task<int> Complete()
    {
        return await _dbContext.SaveChangesAsync();
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}