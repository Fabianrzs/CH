using GA2.Application.Dto;
using GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion;
using GA2.Domain.Core.Servicios.Vigia.CargaProductoTercero;
using GA2.Domain.Core.Servicios.Vigia.CargaTercero;
using GA2.Domain.Core.Servicios.Vigia.CargaTransaccion;
using GA2.Domain.Core.Servicios.Vigia.VerificaTercero;
using GA2.Domain.Entities.Credito;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Options;
using ServiceVigiaVerificacionTercero;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Work Manager Core Negocio
    /// <author>Nicolas FLorez Sarrazola</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class VigiaBusinessLogic : IVigiaBusinessLogic
    {
        private readonly AppMainOptions _optionsMain;
        private readonly ICargaProductoTerceroService _cargarProductoTercero;
        private readonly ICargaTerceroService _cargarTercero;
        private readonly ICargaTransaccionService _cargarTransiccion;
        private readonly IVerificacionTerceroService _verificarTercero;
        private readonly ICreditoRepository _creditoRepository;
        private readonly IVerificacionTerceroVigia _verificarTerceroVigia; 

        public VigiaBusinessLogic(IOptions<AppMainOptions> optionsMain, ICargaProductoTerceroService cargarProductoTercero, ICargaTerceroService cargarTercero, ICargaTransaccionService cargarTransiccion, IVerificacionTerceroService verificarTercero, ICreditoRepository creditoRepository
            , IVerificacionTerceroVigia verificarTerceroVigia)
        {
            _optionsMain = optionsMain.Value ?? throw new ArgumentNullException(nameof(optionsMain));
            _cargarProductoTercero = cargarProductoTercero ?? throw new ArgumentNullException(nameof(cargarProductoTercero));
            _cargarTercero = cargarTercero ?? throw new ArgumentNullException(nameof(cargarTercero));
            _cargarTransiccion = cargarTransiccion ?? throw new ArgumentNullException(nameof(cargarTransiccion));
            _verificarTercero = verificarTercero ?? throw new ArgumentNullException(nameof(verificarTercero));
            _creditoRepository = creditoRepository;
            _verificarTerceroVigia = verificarTerceroVigia; 

        }

        public async Task<Response<ServiceVigiaVerificacionTercero.Resultado>> ServiceVertificacionTerceroAsync(VerificacionTerceroRequest request)
        {
            var response = new Response<ServiceVigiaVerificacionTercero.Resultado>();
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            request.Pe_Origen = _optionsMain.ApplicationId;
            if (string.IsNullOrEmpty(request.Pe_Porcentaje)) request.Pe_Porcentaje = string.Empty;

            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals("VIGIA") && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR))//vigia
            {
                try
                {
                    var resultado = await _verificarTerceroVigia.VertificacionTerceroSolicitud(request.Pe_Dato,request.Pe_TipoVerificacion,request.Pe_Porcentaje);

                    response.Data = new ServiceVigiaVerificacionTercero.Resultado
                    {
                        Encontrado = resultado.Data.Encontrado,
                        Listas = resultado.Data.Listas,
                        Metodo = resultado.Data.Metodo
                    };
                }
                catch (Exception ex)
                {
                    return new Response<ServiceVigiaVerificacionTercero.Resultado>() { IsSuccess = false, ReturnMessage = "Error en el servicio de vigía por favor intentar más tarde." };
                }
            }
            else
            {
                RespuestaVigiaParam vigiaParam = await _creditoRepository.ConsultarResponseVigiaDesactivado(); 
                response.Data = new ServiceVigiaVerificacionTercero.Resultado
                {
                    Encontrado = vigiaParam.VIG_ENCONTRADO,
                    Listas = vigiaParam.VIG_LISTAS,
                    Metodo = vigiaParam.VIG_METODO
                };
            }

            return response;
        }

        public async Task<Response<ReferenciasSvc.Vigia.CargaTercero.Resultado>> CargarTerceroAsync(CargaTerceroRequest request)
        {
            return await _cargarTercero.CargaTercero(request);
        }

        public async Task<Response<ReferenciasSvc.Vigia.CargaProductoTercero.Resultado>> CargaProductoTerceroAsync(CargaProductoTerceroRequest request)
        {
            return await _cargarProductoTercero.CargaProductoTercero(request);
        }

        Task<Response<ReferenciasSvc.Vigia.CargaTransaccion.Resultado>> IVigiaBusinessLogic.CargarTransanccionAsync(CargaTransaccionRequest request)
        {
            throw new NotImplementedException();
        }

        Task<Response<ResultadoVerificacionTercero>> IVigiaBusinessLogic.VertificacionTerceroAsync(VerificacionTerceroRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
