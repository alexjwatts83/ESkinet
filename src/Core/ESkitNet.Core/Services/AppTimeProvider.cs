namespace ESkitNet.Core.Services;

public class AppTimeProvider : IAppTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
