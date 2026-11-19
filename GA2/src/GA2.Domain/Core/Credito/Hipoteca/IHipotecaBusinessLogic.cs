using GA2.Application.Dto;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Interfaz para giro de recursos de creditos hipotecarios
    /// </summary>
    public interface IHipotecaBusinessLogic
    {
        Task<Response<InsertarGnTerceResponse>> InsertTerceroSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<CrearCDPResponse>> CrearCDPSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<SyncPoPvdorResponse>> CrearProveedorSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<PagosSevenDto>> PagoCuotaInicalSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
    }
}