using GA2.Application.Dto;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
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
    /// Interfaz para solicitud de cesantias
    /// </summary>
    public interface ICesantiasBusinessLogic
    {
        Task<Response<ObtenerTramiteCesantiasDto>> ObtenerTramiteCesantias(ParametrosObtenerCesantiasDto solicitud);
        Task<Response<InformacionBasicaCesantiasDto>> InformacionBasicaCesantias(int numeroIdentificacion);
        Task<Response<InsertarGnTerceResponse>> InsertTerceroSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<CrearCDPResponse>> CrearCDPSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<SyncPoPvdorResponse>> CrearProveedorSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<PagosSevenDto>> PagosCesantiasSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> MovContableCesantiasSeven(ParametrosMovContableDto solicitud, CancellationToken cancellationToken);
    }
}