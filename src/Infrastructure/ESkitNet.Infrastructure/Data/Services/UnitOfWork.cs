using ESkitNet.Core.Interfaces;
using System.Collections.Concurrent;

namespace ESkitNet.Infrastructure.Data.Services;

/* look into https://www.infoworld.com/article/2338265/how-to-use-the-unit-of-work-pattern-in-aspnet-core.html later */
public class UnitOfWork(StoreDbContext context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();
    public async Task<bool> Complete(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>()
        where TEntity : Entity<TKey>
        where TKey : class
    {
        var type = typeof(TEntity).Name;

        var repo = (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(type, t =>
        {
            var repositoryType = typeof(GenericRepository<,>).MakeGenericType(typeof(TEntity), typeof(TKey));

            return Activator.CreateInstance(repositoryType, context) 
                ?? throw new InvalidOperationException($"Could not create repo instance of {t}");
        });

        return repo;
    }
}
