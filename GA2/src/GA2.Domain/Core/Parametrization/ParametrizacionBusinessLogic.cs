using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Parametrization;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using GA2.Domain.Entities.Email;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace GA2.Domain.Core
{
    /// <summary>
    /// BusinessLogic de Parametrizacion 
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/2021</date>
    /// </summary>
    public class ParametrizacionBusinessLogic : IParametrizacionBusinessLogic
    {
        private readonly IParametrizacionRepository _parametrizationRepository;
        private readonly ILogger<ParametrizacionBusinessLogic> _logger;
        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;
        private readonly IMensajeRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IVerificacionDerechosRepository _verificacionDerechosRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailRepository _emailRepository;
        private readonly IOptions<EmailOptions> _emailOptions;
        private readonly ICreditoRepository _creditoRepository;

        public ParametrizacionBusinessLogic(IParametrizacionRepository parametrizationRepository, ILogger<ParametrizacionBusinessLogic> logger, IMapper mapper, IMensajeRepository messageRepository, IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic,
            IVerificacionDerechosRepository verificacionDerechosRepository, IWebHostEnvironment webHostEnvironment,IEmailRepository emailRepository,
            IOptions<EmailOptions> emailOptions, ICreditoRepository creditoRepository)
        {

            _parametrizationRepository = parametrizationRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
            _verificacionDerechosRepository = verificacionDerechosRepository;
            _webHostEnvironment = webHostEnvironment;   
            _emailRepository = emailRepository; 
            _emailOptions = emailOptions;
            _creditoRepository = creditoRepository;
        }
        /// <summary>
        /// Metodo Obtener seguro
        /// </summary>
        /// <author>Edgar Andrés Riaño Suarez</author>
        /// <date>2/03/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionSeguroDto>> ObtenerSeguro()
        {
            var responseData = this._mapper.Map<ParametrizacionSeguroDto>(
                await this._parametrizationRepository.ObtenerSeguro());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionSeguroDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar seguro mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionSeguro"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionSeguroDto>> CrearSeguro(ParametrizacionSeguroDto parametrizacionSeguro)
        {
            var responseData = this._mapper.Map<ParametrizacionSeguroDto>(await this._parametrizationRepository.CrearSeguro(this._mapper.Map<SeguroParametrizacion>(parametrizacionSeguro)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionSeguroDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener Tasa
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionTasaDto>> ObtenerTasa()
        {
            var responseData = this._mapper.Map<ParametrizacionTasaDto>(await this._parametrizationRepository.ObtenerTasa());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionTasaDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionTasa"></param>
        /// <returns></returns>

        public async Task<Response<ParametrizacionTasaDto>> CrearTasa(ParametrizacionTasaDto parametrizacionTasa)
        {
            var responseData = this._mapper.Map<ParametrizacionTasaDto>(
                await this._parametrizationRepository.CrearTasa(this._mapper.Map<TasaParametrizacion>(parametrizacionTasa)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionTasaDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener Plazo
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionPlazosDto>> ObtenerPlazo()
        {
            var responseData = this._mapper.Map<ParametrizacionPlazosDto>(
                await this._parametrizationRepository.ObtenerPlazo());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionPlazosDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        /// <summary>
        /// Metodo Obtener Parametro Transaccion
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ParametroTransaccionDto>>> ObtenerParametrosTransaccion()
        {
            IEnumerable<ParametroTransaccionDto> responseData = this._mapper.Map<IEnumerable<ParametroTransaccionDto>>(await this._parametrizationRepository.ObtenerParametrosTransaccion());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<ParametroTransaccionDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar plazos mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionDeadLine"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionPlazosDto>> CrearPlazo(ParametrizacionPlazosDto parametrizacionDeadLine)
        {
            var responseData = this._mapper.Map<ParametrizacionPlazosDto>(
                await this._parametrizationRepository.CrearPlazo(this._mapper.Map<PlazoParametrizacion>(parametrizacionDeadLine)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionPlazosDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener Legal Tasa
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ParametrizacionLegalTasaDto>>> ObtenerLegalTasa()
        {
            IEnumerable<ParametrizacionLegalTasaDto> responseData = this._mapper.Map<IEnumerable<LegalTasaParametrizacion>, IEnumerable<ParametrizacionLegalTasaDto>>(
                await this._parametrizationRepository.ObtenerLegalTasa());
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje11; }
            else { codigoMensaje = ResponseMessageConstants.CodigoMensaje12; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IEnumerable<ParametrizacionLegalTasaDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear legal tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionLegalTasa"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalTasaDto>> CrearLegalTasa(ParametrizacionLegalTasaDto parametrizacionLegalTasa)
        {
            var responseData = this._mapper.Map<ParametrizacionLegalTasaDto>(
                await this._parametrizationRepository.CrearLegalTasa(this._mapper.Map<LegalTasaParametrizacion>(parametrizacionLegalTasa)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalTasaDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        /// <summary>
        /// Metodo Actualizar legal tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametroTransaccion"></param>
        /// <returns></returns>
        public async Task<Response<ParametroTransaccionDto>> ActualizarParametroTransaccion(ParametroTransaccionDto parametroTransaccion)
        {
            var responseData = this._mapper.Map<ParametroTransaccionDto>(
                await this._parametrizationRepository.ActualizarParametroTransaccion(this._mapper.Map<ParametroTransaccion>(parametroTransaccion)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje16; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametroTransaccionDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Actualizar legal tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionlegaltasa"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalTasaDto>> ActualizarLegalTasa(ParametrizacionLegalTasaDto parametrizacionlegaltasa)
        {
            var responseData = this._mapper.Map<ParametrizacionLegalTasaDto>(
                await this._parametrizationRepository.ActualizarLegalTasa(this._mapper.Map<LegalTasaParametrizacion>(parametrizacionlegaltasa)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje16; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalTasaDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Eliminar legal tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionLegalTasa"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalTasaDto>> EliminarLegalTasa(ParametrizacionLegalTasaDto parametrizacionLegalTasa)
        {
            var responseData = this._mapper.Map<ParametrizacionLegalTasaDto>(
                await this._parametrizationRepository.EliminarLegalTasa(this._mapper.Map<LegalTasaParametrizacion>(parametrizacionLegalTasa)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje17; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalTasaDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener Legal Alivio
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalAlivioDto>> ObtenerLegalAlivio()
        {
            var responseData = this._mapper.Map<ParametrizacionLegalAlivioDto>(await this._parametrizationRepository.ObtenerLegalAlivio());
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje11; }
            else { codigoMensaje = ResponseMessageConstants.CodigoMensaje13; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalAlivioDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar legal alivio mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionLegalAlivio"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalAlivioDto>> CrearLegalAlivio(ParametrizacionLegalAlivioDto parametrizacionLegalAlivio)
        {
            var responseData = this._mapper.Map<ParametrizacionLegalAlivioDto>(
                await this._parametrizationRepository.CrearLegalAlivio(this._mapper.Map<LegalAlivioParametrizacion>(parametrizacionLegalAlivio)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalAlivioDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener Legal Gmf
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalGmfDto>> ObtenerLegalGmf()
        {
            var responseData = this._mapper.Map<ParametrizacionLegalGmfDto>(await this._parametrizationRepository.ObtenerLegalGmf());
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje11; }
            else { codigoMensaje = ResponseMessageConstants.CodigoMensaje14; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalGmfDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar legal gmf mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionLegalGmf"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLegalGmfDto>> CrearLegalGmf(ParametrizacionLegalGmfDto parametrizacionLegalGmf)
        {
            var responseData = this._mapper.Map<ParametrizacionLegalGmfDto>(
                await this._parametrizationRepository.CrearLegalGmf(this._mapper.Map<LegalGmfParametrizacion>(parametrizacionLegalGmf)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLegalGmfDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Obtener liquidacion
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLiquidacionDto>> ObtenerLiquidacion()
        {
            var responseData = this._mapper.Map<ParametrizacionLiquidacionDto>(await this._parametrizationRepository.ObtenerLiquidacion());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLiquidacionDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear y Actualizar liquidacion mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <param name="parametrizacionLiquidacion"></param>
        /// <returns></returns>
        public async Task<Response<ParametrizacionLiquidacionDto>> CrearLiquidacion(ParametrizacionLiquidacionDto parametrizacionLiquidacion)
        {
            var responseData = this._mapper.Map<ParametrizacionLiquidacionDto>(
                await this._parametrizationRepository.CrearLiquidacion(this._mapper.Map<LiquidacionParametrizacion>(parametrizacionLiquidacion)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametrizacionLiquidacionDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        /// <summary>
        /// Metodo Crear parametro transaccion mediante el Dto
        /// </summary>
        /// <author>Cristian Gonzalez</author>
        /// <date>7/05/202</date>
        /// <param name="parametroTransaccion"></param>
        /// <returns></returns>
        public async Task<Response<ParametroTransaccionDto>> CrearParametroTransaccion(ParametroTransaccionDto parametroTransaccion)
        {
            var responseData = this._mapper.Map<ParametroTransaccionDto>(
                    await this._parametrizationRepository.CrearParametroTransaccion(
                         this._mapper.Map<ParametroTransaccion>(parametroTransaccion)));
            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<ParametroTransaccionDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }
        private async Task<MensajeDto> ObtenerMensaje(string codigoMensaje)
        {
            return this._mapper.Map<MensajeDto>(await this._messageRepository.ObtenerMensaje(codigoMensaje));
        }


        /// <summary>
        /// Metodo para obtener los archivos parametrizados por modalidad
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>14/10/2021</date>
        /// <param name="tipoModalidadId"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ParametrizacionArchivoModalidadDto>>> ObtenerParametrizacionArchivosModalidad(int tipoModalidadId)
        {
            IEnumerable<ParametrizacionArchivoModalidadDto> responseData = this._mapper.Map<IEnumerable<ParametrizacionArchivoModalidadDto>>(
                await this._parametrizationRepository.ObtenerParametrizacionArchivosModalidad(tipoModalidadId));

            return new Response<IEnumerable<ParametrizacionArchivoModalidadDto>>
            {
                Data = responseData
            };
        }

        /// <summary>
        /// Metodo para obtener los archivos parametrizados por modalidad Task<Response<dynamic>> AgregarTablasParametros(dynamic data);
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>14/10/2021</date>
        /// <param name="tipoModalidadId"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TablaParametroDto>>> ObtenerTablasParametrosPorCategoria(int categoriaId)
        {
            IEnumerable<TablaParametroDto> responseData = this._mapper.Map<IEnumerable<TablaParametroDto>>(
                await this._parametrizationRepository.ObtenerTablasParametrosPorCategoria(categoriaId));

            return new Response<IEnumerable<TablaParametroDto>>
            {
                Data = responseData
            };
        }

        public async Task<Response<IEnumerable<ValorInmuebleResponseDto>>> ValorInmueble(ValorInmuebleDto requestd)
        {
            IEnumerable<ValorInmuebleResponseDto> responseData = 
                await this._parametrizationRepository.SP_CONSULTAR_VALOR_INMUEBLE(requestd);

            return new Response<IEnumerable<ValorInmuebleResponseDto>>
            {
                Data = responseData
            };
        }

        /// <summary>
        /// Metodo para agregar registro en tablas parametros 
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>23/08/2023</date>
        /// <param name="tipoModalidadId"></param>
        /// <returns></returns>
        public async Task<Response<dynamic>> AgregarTablasParametros(dynamic data)
        {
            dynamic responseData = await this._parametrizationRepository.AgregarTablasParametros(data);

            return new Response<dynamic>
            {
                Data = responseData
            };
        }


        /// <summary>
        /// Metodo para obtener los archivos parametrizados por entidad
        /// </summary>
        /// <author> Edwin Lopez</author>
        /// <date>29/03/2022</date>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ParametrizacionArchivoEntidadEducativaDto>>> ObtenerParametrizacionArchivosEntidadEducativa(string entidadId)
        {
            IEnumerable<ParametrizacionArchivoEntidadEducativaDto> responseData = this._mapper.Map<IEnumerable<ParametrizacionArchivoEntidadEducativaDto>>(
                await _parametrizationRepository.ObtenerParametrizacionArchivosEntidadEducativa(entidadId));

            return new Response<IEnumerable<ParametrizacionArchivoEntidadEducativaDto>>
            {
                Data = responseData
            };
        }

        /// <summary>
        /// Metodo crear  valores IPC mediante el Dto
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<Response<IPCDto>> CrearIPC(IPCDto parametroIPC)
        {

            string codigoMensaje = "";

            if (ExisteIPC(parametroIPC).Result == true)
            {
                codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
                MensajeDto mensaje1 = await ObtenerMensaje(codigoMensaje);
                return new Response<IPCDto>
                {
                    Data = null,
                    ReturnMessage = "Ya existe un IPC para esa fecha"
                };
            }


            var dat = DateTime.Now;
            var meses = ((dat.Month - parametroIPC.Mes) + 12 * (dat.Year - parametroIPC.Anio));
            
            
            //if (meses > 1 || meses <= 0)
            if (meses <= 0)
                if (meses <= 0)
                {
                    codigoMensaje = ResponseMessageConstants.CodigoMensaje2;
                    MensajeDto mensaje1 = await ObtenerMensaje(codigoMensaje);

                    return new Response<IPCDto>
                    {
                        Data = null,
                        ReturnMessage = "Solo se puede agregar el IPC del mes anterior"
                    };
                }





            var responseData = this._mapper.Map<IPCDto>(await this._parametrizationRepository.CrearIPC(this._mapper.Map<IPC>(parametroIPC)));

            

            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje15; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IPCDto>
            {
                Data = responseData,
                ReturnMessage = "IPC Creado con éxito"
            };
        }

        public async Task<Response<IPCDto>> ProcesosIPC(IPCDto parametroIPC)
        {

            
            var solicitudes = this._mapper.Map<IEnumerable<SolicitudConsecutivoDto>>(await _verificacionDerechosRepository.ObtenerTodoSolicitudConsecutivo());
            var consecutivo = solicitudes.Where(x => x.CnsId == "SC").FirstOrDefault();

            long nuevoConsecutivoVerificacionDerechos = consecutivo.CnsUltimoNumero + 1;
            long nuevoConsecutivoAboinoInteres = consecutivo.CnsUltimoNumero + 2;


            consecutivo.CnsUltimoNumero = nuevoConsecutivoAboinoInteres;

            this._mapper.Map<SolicitudConsecutivoDto>(await _verificacionDerechosRepository.ActualizarSolicitudConsecutivo(consecutivo));

            await _verificacionDerechosBusinessLogic.CrearPeriodoVerificacionDerecho(parametroIPC, nuevoConsecutivoVerificacionDerechos);
            await _verificacionDerechosBusinessLogic.CrearPeriodoValoresInteresAbonados(parametroIPC, nuevoConsecutivoAboinoInteres);

            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAVResult(parametroIPC, nuevoConsecutivoAboinoInteres);
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAdministracionCesantiasResult(parametroIPC, nuevoConsecutivoAboinoInteres);

            ///
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasSolucionViviendaResult(parametroIPC, nuevoConsecutivoAboinoInteres);

            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAVACResult(parametroIPC, nuevoConsecutivoAboinoInteres);
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAVAVResult(parametroIPC, nuevoConsecutivoAboinoInteres);
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAVCResult(parametroIPC, nuevoConsecutivoAboinoInteres);


           
            return new Response<IPCDto>
            {
                Data = null,
                ReturnMessage = "Procesos de IPC Creado con éxito"
            };
        }

        /// <summary>
        /// Metodo Revisar si ya existe un IPC para esa fecha
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>08/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<bool> ExisteIPC(IPCDto parametroIPC)
        {
            IEnumerable<IPCDto> responseData = this._mapper.Map<IEnumerable<IPCDto>>(await _parametrizationRepository.ExisteIPC(this._mapper.Map<IPC>(parametroIPC)));


            if (responseData.Count() >= 1) return true;


            return false;
        }


        /// <summary>
        /// Metodo Actualiar  valores IPC mediante el Dto
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>07/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<Response<IPCDto>> ActualizarIPC(IPCDto parametroIPC)
        {

            var aprobado = await _verificacionDerechosBusinessLogic.ObtenerTodoVerificacionDerechoByPeriodo(parametroIPC);

            var InteresAbonado = await _verificacionDerechosBusinessLogic.ObtenerTodoValoresInteresAbonadosByPeriodo(parametroIPC);


            if (aprobado.Data == null)
            {

                return new Response<IPCDto>
                {
                    Data = null,
                    ReturnMessage = "No se puede Actualizar por que no existe Verificacion de derechos"
                };

            }


            if (aprobado.Data.Aprobado != null)
            {

                return new Response<IPCDto>
                {
                    Data = null,
                    ReturnMessage = "No se puede Actualizar por que ya fue aprobado"
                };

            }


            if (InteresAbonado.Data.Aprobado != null)
            {

                return new Response<IPCDto>
                {
                    Data = null,
                    ReturnMessage = "No se puede Actualizar por que ya fue aprobado el abono de interes"
                };

            }

            var dat = DateTime.Now;
            var meses = ((dat.Month - parametroIPC.Mes) + 12 * (dat.Year - parametroIPC.Anio));

            //if (meses > 1 || meses <= 0)
            if (meses > 1 || meses <= 0)
            {               

                return new Response<IPCDto>
                {
                    Data = null,
                    ReturnMessage = "Solo se puede agregar el IPC del mes anterior"
                };
            }



            await _verificacionDerechosBusinessLogic.EliminarActualizarIpc(parametroIPC);

            var responseData = this._mapper.Map<IPCDto>(
                await this._parametrizationRepository.ActualizarIPC(this._mapper.Map<IPC>(parametroIPC)));

            var solicitudes = this._mapper.Map<IEnumerable<SolicitudConsecutivoDto>>(await _verificacionDerechosRepository.ObtenerTodoSolicitudConsecutivo());
            var consecutivo = solicitudes.Where(x => x.CnsId == "SC").FirstOrDefault();

            long nuevoConsecutivoVerificacionDerechos = consecutivo.CnsUltimoNumero + 1;
            long nuevoConsecutivoAbonoInteres = consecutivo.CnsUltimoNumero + 2;


            consecutivo.CnsUltimoNumero = nuevoConsecutivoAbonoInteres;

            this._mapper.Map<SolicitudConsecutivoDto>(await _verificacionDerechosRepository.ActualizarSolicitudConsecutivo(consecutivo));

            await _verificacionDerechosBusinessLogic.CrearPeriodoVerificacionDerecho(parametroIPC, nuevoConsecutivoVerificacionDerechos);
            await _verificacionDerechosBusinessLogic.CrearPeriodoValoresInteresAbonados(parametroIPC, nuevoConsecutivoAbonoInteres);

            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAVResult(parametroIPC, nuevoConsecutivoAbonoInteres);
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasAdministracionCesantiasResult(parametroIPC, nuevoConsecutivoAbonoInteres);
            await _verificacionDerechosBusinessLogic.CrearDatosCuentasSolucionViviendaResult(parametroIPC, nuevoConsecutivoAbonoInteres);




            string codigoMensaje = "";
            if (responseData != null) { codigoMensaje = ResponseMessageConstants.CodigoMensaje16; }
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            return new Response<IPCDto>
            {
                Data = responseData,
                ReturnMessage = "IPC Actualizado con éxito"
            };
        }

        /// <summary>
        /// Metodo crear  valores IPC mediante el Dto
        /// </summary>
        /// <author>Cesar Hernandez</author>
        /// <date>21/03/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<IPCDto>>> ObtenerReporteIPC(IPCDto parametroIPC)
        {
            IEnumerable<IPCDto> responseData = this._mapper.Map<IEnumerable<IPCDto>>(await _parametrizationRepository.ObtenerReporteIPC(this._mapper.Map<IPC>(parametroIPC)));

            return new Response<IEnumerable<IPCDto>>
            {
                Data = responseData
            };
        }

        /// <summary>
        /// Metodo Obtener todos reporte IPC
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>02/05/2023</date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<IPCDto>>> ObtenerTodoReporteIPC()
        {
            var responseData = this._mapper.Map<IEnumerable<IPCDto>>(await this._parametrizationRepository.ObtenerTodoReporteIPC());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);

            responseData.Count();


            return new Response<IEnumerable<IPCDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }


        public async Task SincronizacionCuentas(IPCDto request)
        {
           // await SendNotification(request);
            await this._parametrizationRepository.EliminacionParametrizacionCuentas();
            await _verificacionDerechosBusinessLogic.SincronizacionCuentasAV(request);
            await _verificacionDerechosBusinessLogic.SincronizacionCuentasAdmiCesantias(request);
            await _verificacionDerechosBusinessLogic.SincronizacionCuentaAdmiVivienda(request);
            await _verificacionDerechosBusinessLogic.SincronizacionCuentaAVAC(request);
            await _verificacionDerechosBusinessLogic.SincronizacionCuentaAVAV(request);
            await _verificacionDerechosBusinessLogic.SincronizacionCuentaAVC(request);
            
        }
        /// <summary>
        /// Metodo para obtener todos los estado de integracion del aplciativo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>02/05/2023</date>
        /// <returns></returns>
        public async Task<Response<IEnumerable<ParametroIntegracionDto>>> ObtenerParametrosIntegraciones()
        {
            var responseData = this._mapper.Map<IEnumerable<ParametroIntegracionDto>>(await _creditoRepository.ObtenerParametrosIntegraciones());

            return new Response<IEnumerable<ParametroIntegracionDto>>
            {
                Data = responseData
            };
        }
        private async Task SendNotification(IPCDto request)
        {

            try
            {
                var client = new TcpClient();
                try
                {
                    client.Connect("smtp-mail.outlook.com", 465);
                    Console.WriteLine("Conexión exitosa.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
                }

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Reportes", "HTML", "Templates", "HTML", "Liquidacion");

                //var html = new StringBuilder(await File.ReadAllTextAsync(Path.Join(path, "main-content.html")));
                var html = await File.ReadAllTextAsync(Path.Join(path, "SincronizacionLiquidacion.html"));
                var cultureEs = CultureInfo.GetCultureInfo("es-ES");


                html = html.Replace("mes", request.Mes.ToString());
                html = html.Replace("anio", request.Anio.ToString());
                html = html.Replace("ValorIPC", request.ValorIPC.ToString());



                MailMessage correo = new MailMessage();
                correo.From = new MailAddress("correo.cajahonor@cajahonor.gov.co", "CajaHonor", System.Text.Encoding.UTF8);
                correo.To.Add("desarrollok11@grupokriterion.com.co");
                correo.Subject = "Restablecer contraseña";
                correo.Body = html;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;

                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("correo.cajahonor@cajahonor.gov.co", "Ingreso.123");
               
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.EnableSsl = true;
                smtp.Timeout = 70000;
                smtp.Send(correo);

                //var data = new EmailModel() {
                //    Asunto = "Notificación: Sincronización de Cuentas Completada",
                //    Destinatarios = new  List<string>(){ "desarrollok11@grupokriterion.com.co", "proyectos.kriterion@cajahonor.gov.co" },
                //    IsHtmlBody = true,
                //    Mensaje = html,
                //    Prioridad = MailPriority.High
                //};
                //await _emailRepository.EnviarEmail(data);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
