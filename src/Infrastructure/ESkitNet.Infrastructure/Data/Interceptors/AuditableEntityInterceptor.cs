namespace ESkitNet.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(IAppTimeProvider timeProvider, IUserAccessor userAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var entities = context.ChangeTracker.Entries<IEntity>();

        foreach (var entity in entities)
        {
            // TODO: use ITimeProvider later and figure out how to get the user from the context
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedBy = userAccessor.UserName;
                entity.Entity.CreatedAt = timeProvider.Now;
            }

            if (entity.State == EntityState.Added || entity.State == EntityState.Modified || entity.HasChangedOwnedEntities())
            {
                entity.Entity.LastModifiedBy = userAccessor.UserName;
                entity.Entity.LastModified = timeProvider.Now;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}