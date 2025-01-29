using ESkitNet.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace ESkitNet.Core.Services;

public class UserAccessor : IUserAccessor
{
    private readonly string defaultUserName = "system-user";

    public string UserName { get; }

    public UserAccessor(IHttpContextAccessor context)
    {
        UserName = context.HttpContext != null
            ? context.HttpContext!.User.GetEmail(false) ?? defaultUserName
            : defaultUserName;
    }
}
