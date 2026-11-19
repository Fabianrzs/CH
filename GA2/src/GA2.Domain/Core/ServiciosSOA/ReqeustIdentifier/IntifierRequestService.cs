using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace GA2.Domain.Core.Servicios
{
    /// <summary>
    /// Identificador de peticiones
    /// </summary>
    //public class IntifierRequestService : IIntifierRequestService
    //{
    //    private readonly IRequestIdentifierRepository _requestIdentifierRepository;

    //    /// <summary>
    //    /// Ctro servicio
    //    /// </summary>
    //    /// <param name="asyncRepository"></param>
    //    public IntifierRequestService(IRequestIdentifierRepository requestIdentifierRepository)
    //    {
    //        _requestIdentifierRepository = requestIdentifierRepository;
    //    }

    //    /// <summary>
    //    /// Intificador de peticiones
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task IntifierRequestSaveAsync(IEnumerable<Claim> claims, HttpRequest request)
    //    {
    //        var userId = claims.Where(x => x.Type == "UserId").FirstOrDefault();
    //        await _requestIdentifierRepository.CreateError(
    //            new RequestIdentity { Endpoint = $"{request.Method} - {request.Path}",IpRequest = request.HttpContext.Connection.RemoteIpAddress?.ToString(), UserId = Guid.Parse(userId.Value), HoraPeticion = DateTime.Now });
    //    }
    //}
}
