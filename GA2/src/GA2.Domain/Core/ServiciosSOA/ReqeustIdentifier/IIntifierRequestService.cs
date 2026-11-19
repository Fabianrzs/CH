using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Servicios
{
    public interface IIntifierRequestService
    {
        Task IntifierRequestSaveAsync(IEnumerable<Claim> claims, HttpRequest request);
    }
}