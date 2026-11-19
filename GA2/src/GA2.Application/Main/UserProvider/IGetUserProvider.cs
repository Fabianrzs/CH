using Microsoft.AspNetCore.Http;

namespace GA2.Application.Main.UserProvider
{
    public interface IGetUserProvider
    {
        string GetUserId(HttpContext context);
    }
}