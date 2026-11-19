using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GA2.Application.Main.UserProvider
{
    /// <summary>
    /// Configuracion de los claims para obtener el usuario por el claim Name
    /// y se le passa el id del usuario o cliente
    /// </summary>
    /// <author>Oscar Julian Rojas</author>
    /// <date>27/05/2021</date>
    public class GetUserProvider : IGetUserProvider
    {
        public string GetUserId(HttpContext context)
        {
            var claim = context.User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value;
            return claim;
        }
    }
}
