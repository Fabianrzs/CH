using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Application.Main;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static GA2.Domain.Core.ProspeccionBusinessLogic;

namespace GA2.Domain.Core
{
    public class VerificacionDerechosBusinessLogic : IVerificacionDerechosBusinessLogic
    {
        private readonly IVerificacionDerechosRepository _verificacionDerechosRepository;
        private readonly IMensajeRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly ICierreMensualRequestProvider _iCierreMensualRequestProvider;
        private readonly IClientRequestProvider _iClienteRequestProvider;
        private readonly IClienteRepository _clientsRepository;

        private readonly IProcesoRepository _procesoRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ILogger<ClientesBusinessLogic> _logger;
        private readonly IBuaService _servicioBua;
        public VerificacionDerechosBusinessLogic(IVerificacionDerechosRepository verificacionDerechosRepository, IMapper mapper, IMensajeRepository messageRepository,
            ICierreMensualRequestProvider iCierreMensualRequestProvider, IClientRequestProvider iClienteRequestProvider, IClienteRepository clientsRepository,
            IProcesoRepository procesoRepository, ISolicitudRepository solicitudRepository, ILogger<ClientesBusinessLogic> logger, IBuaService servicioBua)
        {
            _verificacionDerechosRepository = verificacionDerechosRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
            _iCierreMensualRequestProvider = iCierreMensualRequestProvider;
            _iClienteRequestProvider = iClienteRequestProvider;
            _clientsRepository = clientsRepository;
            _procesoRepository = procesoRepository;
            _solicitudRepository = solicitudRepository;
            _logger = logger;
            _servicioBua = servicioBua ?? throw new ArgumentNullException(nameof(servicioBua));

        }

        private async Task<MensajeDto> ObtenerMensaje(string codigoMensaje)
        {
            return this._mapper.Map<MensajeDto>(await this._messageRepository.ObtenerMensaje(codigoMensaje));
        }

        public async Task<Response<IEnumerable<ParametrosVerificacionDerechoDto>>> ObtenerTodoVerificacionDerecho()
        {


            var responseData = this._mapper.Map<IEnumerable<ParametrosVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerecho());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);

            responseData.Count();


            return new Response<IEnumerable<ParametrosVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        public async Task<Response<IEnumerable<InfoVerificacionDerechoDto>>> ObtenerTodoVerificacionDerechoInfoById(int ID)
        {
            var responseData = this._mapper.Map<IEnumerable<InfoVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechoInfoById(ID));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);


            return new Response<IEnumerable<InfoVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        public async Task<Response<IEnumerable<ResultVerificacionDerechoDto>>> ObtenerVerificacionDerechoResultById(int ID)
        {
            var responseData = this._mapper.Map<IEnumerable<ResultVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerVerificacionDerechoResultById(ID));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);


            return new Response<IEnumerable<ResultVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        public async Task<Response<IEnumerable<InfoVerificacionDerechoDto>>> ObtenerTodoVerificacionDerechoCuentas()
        {
            var lista = await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechoCuentas();


            var responseData = this._mapper.Map<IEnumerable<InfoVerificacionDerechoDto>>(lista);
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);


            return new Response<IEnumerable<InfoVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        public async Task<Response<IEnumerable<ParametrosVerificacionDerechoDto>>> CrearTodoVerificacionDerechoCuentasInfo(IPC IPCparametrizacion)
        {
            var lista = await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechoCuentas();
            var cuentasVerificacas = lista.Count();

            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            var responseData = this._mapper.Map<IEnumerable<ParametrosVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerecho());
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);

            responseData.Count();


            return new Response<IEnumerable<ParametrosVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        /// <summary>
        /// Metodo crear el periodo de verificacion de derechos al mismo tiempo que el ipc
        /// </summary>
        /// <author>Julian Nova</author>+
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<Response<ParametrosVerificacionDerechoDto>> CrearPeriodoVerificacionDerecho(IPCDto parametroIPC, long consecutivo)
        {

            var FLJ_ID = Guid.NewGuid();
            //ID frupo de usuario  "GCICE_L" = 60;
            //var CodigoPerfil = "USER_SUPER_ADMIN";
            var CodigoPerfil = "GCICE_L";


            var USR_ID = await _verificacionDerechosRepository.ObtenerUsuarioVerificacionDerechos(CodigoPerfil);


            var nuevo = new ParametrosVerificacionDerechoDto
            {
                NumeroSolicitud = consecutivo.ToString(),
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes,
                FechaOperacion = parametroIPC.FechaActualizacion,
                FechaAprobacion = null,
                TotalCuentasVerificadas = 0,
                TotalCambiosEstado = 0,
                UsuarioId = USR_ID.USR_ID,
                UsuarioNombre = USR_ID.USR_NOMBRE + " " + USR_ID.USR_APELLIDO,

                Estado = 1,
                NombreEstado = "Pendiente por procesar",

                SltId = FLJ_ID,

            };


            var verificacionDerechoSolicitudTarea = new VerificacionDerechoSolicitudTarea
            {
                SLT_ID = FLJ_ID,
                TRA_ID = Guid.Parse("1B016AD4-8D99-4E1A-9875-490C3C55D5F5"),
                SOL_ID = Guid.Parse("B04F5F6B-8D89-4F0E-BC5F-C1CFF5C24476"),
                USR_ID = (Guid)parametroIPC.UsuarioId,
                OBSERVACION = "Verificación de Derechos procesar"


            };
            // await _verificacionDerechosRepository.CrearVerificacionDerechoSolicitudTarea(verificacionDerechoSolicitudTarea);


            var tarea = new NotificacionTarea
            {
                Usuario = USR_ID.USR_ID,
                Notificacion = "solicitud Verificación de Derechos en la etapa de Pendiente por Procesar correspondiente al periodo " + parametroIPC.Anio + "/" + parametroIPC.Mes,
                Modulo = "Verificación de Derechos",
                TareaSiguiente = FLJ_ID,
                NUM_CEDULA = USR_ID.NUM_CEDULA,
                UsuarioEnvia = (Guid)parametroIPC.UsuarioId,


            };
            await _verificacionDerechosRepository.InsertarNotificacionTarea(tarea);

            await _verificacionDerechosRepository.ActualizarNotificacionTareaVerificacionYAbonos(FLJ_ID, "/verificacionDerechos");

            var responseData = this._mapper.Map<ParametrosVerificacionDerechoDto>(
                    await this._verificacionDerechosRepository.CrearPeriodoVerificacionDerecho(
                         this._mapper.Map<ParametrosVerificacionDerecho>(nuevo)));

            return new Response<ParametrosVerificacionDerechoDto>
            {
                Data = responseData,
                ReturnMessage = "Verificación Creado con éxito"
            };
        }



        public async Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerTodoDatosVerificacionDerecho()
        {
            DateTime primerDia = new DateTime();
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);

            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var DatosVerificacion = await _iCierreMensualRequestProvider.ObtenerTodoDatosVerificacionDerechos(filtro);

            return DatosVerificacion;

        }

        public Task<Response<ParametrosVerificacionDerechoDto>> CrearParametrosVerificacionDerecho(ParametrosVerificacionDerechoDto parametrizacionIPC)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Metodo para obtener la verificacion de derechos a partir del periodo y da el valor del cambio de estado y la unidad ejecutora segun su ID
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 
        public async Task<Response<IEnumerable<ResultVerificacionDerechoDto>>> ObtenerTodoDatosVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID)
        {
            var responseData = this._mapper.Map<IEnumerable<ResultVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoDatosVerificacionDerechosByPeriodo(ID));

            foreach (var item in responseData)
            {

                if (item.EctIdInicial == 1) { item.EstadoInicial = "VIGENTE"; }
                if (item.EctIdInicial == 2) { item.EstadoInicial = "INACTIVA"; }
                if (item.EctIdInicial == 5) { item.EstadoInicial = "PRECANCELADA"; }
                if (item.EctIdInicial == 6) { item.EstadoInicial = "SUSUPENDIDA"; }
                if (item.EctIdInicial == 7) { item.EstadoInicial = "CANCELADA"; }
                if (item.EctIdInicial == 8) { item.EstadoInicial = "ANULADA"; }
                if (item.EctIdInicial == 9) { item.EstadoInicial = "MOROSA"; }
                if (item.EctIdFinal == 1) { item.EstadoFinal = "VIGENTE"; }
                if (item.EctIdFinal == 2) { item.EstadoFinal = "INACTIVA"; }
                if (item.EctIdFinal == 5) { item.EstadoFinal = "PRECANCELADA"; }
                if (item.EctIdFinal == 6) { item.EstadoFinal = "SUSUPENDIDA"; }
                if (item.EctIdFinal == 7) { item.EstadoFinal = "CANCELADA"; }
                if (item.EctIdFinal == 8) { item.EstadoFinal = "ANULADA"; }
                if (item.EctIdFinal == 9) { item.EstadoFinal = "MOROSA"; }



                if (item.UnidadEjecutora == 1) { item.NombreUnidadEjecutora = "EJÉRCITO NACIONAL DE COLOMBIA"; }
                if (item.UnidadEjecutora == 2) { item.NombreUnidadEjecutora = "CAJA PROMOTORA DE VIVIENDA MILITAR Y DE POLICÍA"; }
                if (item.UnidadEjecutora == 3) { item.NombreUnidadEjecutora = "POLICÍA NACIONAL"; }
                if (item.UnidadEjecutora == 5) { item.NombreUnidadEjecutora = "PENSIONADOS POLICÍA NACIONAL"; }
                if (item.UnidadEjecutora == 6) { item.NombreUnidadEjecutora = "DIRECCIÓN SANIDAD POLICÍA NACIONAL"; }
                if (item.UnidadEjecutora == 7) { item.NombreUnidadEjecutora = "GABINETE"; }
                if (item.UnidadEjecutora == 8) { item.NombreUnidadEjecutora = "PENSIONADOS FFMM"; }
                if (item.UnidadEjecutora == 9) { item.NombreUnidadEjecutora = "COMANDO GENERAL FFMM"; }
                if (item.UnidadEjecutora == 10) { item.NombreUnidadEjecutora = "ARMADA NACIONAL"; }
                if (item.UnidadEjecutora == 11) { item.NombreUnidadEjecutora = "FUERZA ÁEREA DE COLOMBIA"; }
                if (item.UnidadEjecutora == 12) { item.NombreUnidadEjecutora = "CAJA SUELDOS DE RETIRO POLICÍA"; }
                if (item.UnidadEjecutora == 13) { item.NombreUnidadEjecutora = "DIRECCIÓN GENERAL SANIDAD MILITAR"; }
                if (item.UnidadEjecutora == 14) { item.NombreUnidadEjecutora = "DIRECCIÓN GENERAL SANIDAD EJÉRCITO"; }
                if (item.UnidadEjecutora == 15) { item.NombreUnidadEjecutora = "DIRECCIÓN GENERAL SANIDAD ARMADA"; }
                if (item.UnidadEjecutora == 16) { item.NombreUnidadEjecutora = "DIRECCIÓN GENERAL SANIDAD FUERZA AÉREA"; }
                if (item.UnidadEjecutora == 17) { item.NombreUnidadEjecutora = "CAJA DE RETIRO DE LAS FFMM"; }
                if (item.UnidadEjecutora == 18) { item.NombreUnidadEjecutora = "SOLDADOS EJÉRCITO NACIONAL DE COLOMBIA"; }
                if (item.UnidadEjecutora == 19) { item.NombreUnidadEjecutora = "SOLDADOS ARMADA NACIONAL"; }
                if (item.UnidadEjecutora == 20) { item.NombreUnidadEjecutora = "DIRECCIÓN GENERAL MARÍTIMA"; }
                if (item.UnidadEjecutora == 21) { item.NombreUnidadEjecutora = "DIRECCIÓN EJECUTIVA DE LA JUSTICIA PENAL MILITAR"; }
                if (item.UnidadEjecutora == 22) { item.NombreUnidadEjecutora = "POLICÍA BIENESTAR"; }
                if (item.UnidadEjecutora == 24) { item.NombreUnidadEjecutora = "DIRECCIÓN DE VETERANOS Y REHABILITACIÓN INCLUSIVA"; }
            }


            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);
            foreach (var item in responseData)
            {
                item.TotalCuenta = responseData.Count();
                break;
            }
            return new Response<IEnumerable<ResultVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }

        public async Task<IEnumerable<ResultVerificacionDerechoDto>> ObtenerTodoDatosVerificacionDerechosByPeriodo1(VerificacionDerechosPeriodo ID)
        {
            var responseData = this._mapper.Map<IEnumerable<ResultVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoDatosVerificacionDerechosByPeriodo(ID));

            return responseData;
        }

        public async Task<Response<ParametrosVerificacionDerechoDto>> ObtenerTodoVerificacionDerechosByPeriodo(VerificacionDerechosPeriodo ID)
        {
            var responseData = this._mapper.Map<ParametrosVerificacionDerechoDto>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechosByPeriodo(ID));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;



            return new Response<ParametrosVerificacionDerechoDto>
            {
                Data = responseData,
                ReturnMessage = "Verificacion Traida con exito"
            };
        }

        public async Task<Response<IEnumerable<ParametrosVerificacionDerechoDto>>> ObtenerTodoVerificacionDerechosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID)
        {

            var responseData = this._mapper.Map<IEnumerable<ParametrosVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechosFiltroByPeriodo(ID));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;



            return new Response<IEnumerable<ParametrosVerificacionDerechoDto>>
            {
                Data = responseData,
                ReturnMessage = "Verificaciones por rango traidas con exito"
            };

        }

        /// <summary>
        /// Metodo para obtener la verificacion de derechos a partir del periodo y da la cantidad de cambios de estados
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 

        public async Task<Response<InfoVerificacionDerechoDto>> ObtenerTodoVerificacionDerechosInfoByPeriodo(VerificacionDerechosPeriodo ID)
        {
            var responseData = this._mapper.Map<IEnumerable<ResultVerificacionDerechoDto>>(await _verificacionDerechosRepository.ObtenerTodoDatosVerificacionDerechosByPeriodo(ID));

            int VigenteMorosa = 0;
            int VigenteInactiva = 0;
            int VigentePrecancelada = 0;
            int VigenteCancelada = 0;
            int MorosaVigente = 0;
            int MorosaInactiva = 0;
            int MorosaPrecancelada = 0;
            int MorosaCancelada = 0;
            int InactivaVigente = 0;
            int InactivaMorosa = 0;
            int InactivaPrecancelada = 0;
            int InactivaCancelada = 0;
            int SuspendidaCancelada = 0;


            foreach (var item in responseData)
            {
                if (item.EctIdInicial == 1 && item.EctIdFinal == 9) { VigenteMorosa++; };
                if (item.EctIdInicial == 1 && item.EctIdFinal == 2) { VigenteInactiva++; };
                if (item.EctIdInicial == 1 && item.EctIdFinal == 5) { VigentePrecancelada++; };
                if (item.EctIdInicial == 1 && item.EctIdFinal == 7) { VigenteCancelada++; };
                if (item.EctIdInicial == 9 && item.EctIdFinal == 1) { MorosaVigente++; };
                if (item.EctIdInicial == 9 && item.EctIdFinal == 2) { MorosaInactiva++; };
                if (item.EctIdInicial == 9 && item.EctIdFinal == 5) { MorosaPrecancelada++; };
                if (item.EctIdInicial == 9 && item.EctIdFinal == 7) { MorosaCancelada++; };
                if (item.EctIdInicial == 2 && item.EctIdFinal == 1) { InactivaVigente++; };
                if (item.EctIdInicial == 2 && item.EctIdFinal == 9) { InactivaMorosa++; };
                if (item.EctIdInicial == 2 && item.EctIdFinal == 5) { InactivaPrecancelada++; };
                if (item.EctIdInicial == 2 && item.EctIdFinal == 7) { InactivaCancelada++; };
                if (item.EctIdInicial == 5 && item.EctIdFinal == 7) { SuspendidaCancelada++; };

            }

            var Info = new InfoVerificacionDerechoDto
            {
                VigenteMorosa = VigenteMorosa,
                VigenteInactiva = VigenteInactiva,
                VigentePrecancelada = VigentePrecancelada,
                VigenteCancelada = VigenteCancelada,
                MorosaVigente = MorosaVigente,
                MorosaInactiva = MorosaInactiva,
                MorosaPrecancelada = MorosaPrecancelada,
                MorosaCancelada = MorosaCancelada,
                InactivaVigente = InactivaVigente,
                InactivaMorosa = InactivaMorosa,
                InactivaPrecancelada = InactivaPrecancelada,
                InactivaCancelada = InactivaCancelada,
                SuspendidaCancelada = SuspendidaCancelada,

            };
            Info.TotalCuentasAfectadas = VigenteMorosa + VigenteInactiva + VigentePrecancelada + MorosaVigente + MorosaInactiva + MorosaPrecancelada + MorosaCancelada + InactivaVigente
                + InactivaMorosa + InactivaPrecancelada + InactivaCancelada + SuspendidaCancelada;
            return new Response<InfoVerificacionDerechoDto>
            {
                Data = Info,
                ReturnMessage = "Verificaciones por Info traidas con exito"
            };
        }

        public async Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request)
        {

            var lista = await _iCierreMensualRequestProvider.ActualizarECTCuentas(request);


            return lista;
        }

        /// <summary>
        /// Metodo para registrar los cambios de estados y actulalizar los valores de abonos de interes
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 

        public async Task<Response<ParametrosVerificacionDerechoDto>> ActualizarTodoECTCuentas(VerificacionDerechosPeriodo request)
        {
            var ipc = await ObtenerIPCByPeriodo(request);

            var ipcdatos = ipc.Data;


            var Existe = await ObtenerTodoVerificacionDerechosByPeriodo(request);



            if (Existe.Data.Aprobado != null)
            {
                return new Response<ParametrosVerificacionDerechoDto>
                {
                    Data = null,
                    ReturnMessage = "La verificación ya fue aprobada"
                };
            }

            if (string.IsNullOrEmpty(request.NumeroRadicado))
            {
                return new Response<ParametrosVerificacionDerechoDto>
                {
                    Data = null,
                    ReturnMessage = "EL número de radicado esta vacio"
                };
            }


            var ID = new VerificacionDerechosPeriodo
            {
                Anio = request.Anio,
                Mes = request.Mes,

            };

            if (Existe.Data.Estado == 1)
            {
                ActualizarSolicitudTarea actualizarSolicitudTarea = new ActualizarSolicitudTarea()
                {
                    SLT_ID = Existe.Data.SltId,
                    SLT_ESTADO_SOLICITUD = 328,
                    SLT_OBSERVACION = "Verificación de Derechos Procesado"
                };

                await _verificacionDerechosRepository.ActualizarSolicitudTareaVerificacionDerechos(actualizarSolicitudTarea);



                //ID frupo de usuario  "AOPER_J" = 88;
                //var CodigoPerfil = "USER_SUPER_ADMIN";
                var CodigoPerfil = "AOPER_J";


                var USR_ID = await _verificacionDerechosRepository.ObtenerUsuarioVerificacionDerechos(CodigoPerfil);

                Existe.Data.FechaAprobacion = null;
                Existe.Data.UsuarioId = USR_ID.USR_ID;
                Existe.Data.UsuarioNombre = USR_ID.USR_NOMBRE + " " + USR_ID.USR_APELLIDO;
                Existe.Data.NumeroRadicado = request.NumeroRadicado;
                Existe.Data.NombreEstado = "Pendiente por aprobar";
                Existe.Data.Estado = 2;
                Existe.Data.SltIdAprobar = Existe.Data.SltId;

                var actualizarParametrosVerificacionDerechosPorAsignacion = this._mapper.Map<ParametrosVerificacionDerechoDto>(
                 await this._verificacionDerechosRepository.ActualizarParametrosVerificacionDerechos(this._mapper.Map<ParametrosVerificacionDerecho>(Existe.Data)));

                var verificacionDerechoSolicitudTarea = new VerificacionDerechoSolicitudTarea
                {
                    SLT_ID = Existe.Data.SltId,
                    TRA_ID = Guid.Parse("FB03225B-2E1B-472E-A5AC-8D08E41AA1AD"),
                    SOL_ID = Guid.Parse("6A02A9B4-3E3C-4E9A-B4FC-325793F90B94"),
                    USR_ID = (Guid)request.UsuarioId,
                    OBSERVACION = "Verificación de Derechos pendiente por aprobar"
                };


                var tarea = new NotificacionTarea
                {
                    Usuario = USR_ID.USR_ID,
                    Notificacion = "Verificación de derechos en la etapa de Pendiente por aprobar, para su aprobación con Numero de radicado: " + request.NumeroRadicado + " correspondiente al periodo " + request.Anio + "/" + request.Mes,
                    Modulo = "Verificación de Derechos",
                    TareaSiguiente = Existe.Data.SltId,
                    NumRadicado = request.NumeroRadicado,
                    NUM_CEDULA = USR_ID.NUM_CEDULA,
                    UsuarioEnvia = (Guid)request.UsuarioId,
                };

                await _verificacionDerechosRepository.InsertarNotificacionTarea(tarea);

                await _verificacionDerechosRepository.ActualizarNotificacionTareaVerificacionYAbonos(Existe.Data.SltId, "/verificacionDerechos");


                return new Response<ParametrosVerificacionDerechoDto>
                {
                    Data = actualizarParametrosVerificacionDerechosPorAsignacion,
                    ReturnMessage = "Verificacion Procesada con exito"
                };
            }

            var CodigoPerfilTareaProcesar = "AOPER_J";
            var USR_IDVerificacionAprobada = await _verificacionDerechosRepository.ObtenerUsuarioVerificacionDerechos(CodigoPerfilTareaProcesar);

            Existe.Data.FechaAprobacion = DateTime.Now;
            Existe.Data.Aprobado = 1;
            Existe.Data.UsuarioId = request.UsuarioId;
            Existe.Data.UsuarioId = USR_IDVerificacionAprobada.USR_ID;
            Existe.Data.UsuarioNombre = USR_IDVerificacionAprobada.USR_NOMBRE + " " + USR_IDVerificacionAprobada.USR_APELLIDO;
            Existe.Data.NombreEstado = "Aprobado";
            Existe.Data.Estado = 3;


            var responseData = this._mapper.Map<ParametrosVerificacionDerechoDto>(
                 await this._verificacionDerechosRepository.ActualizarParametrosVerificacionDerechos(this._mapper.Map<ParametrosVerificacionDerecho>(Existe.Data)));

            var datos = await _verificacionDerechosRepository.ObtenerTodoDatosVerificacionDerechosByPeriodo(request);

            if (ipcdatos.Mes == 1)
            {
                foreach (var dato in datos)
                {
                    await ActualizarComienzoAnio(dato, ipc.Data);

                }
            }
            else
            {
                foreach (var dato in datos)
                {
                    await ActualizarPorMes(dato, ipc.Data);
                }
            }

            var dataFiler = datos.Where(x => x.CAMBIO_ESTADO.Trim().ToLower() == "si").ToList();
            if (dataFiler.Count > 0)
            {
               await ActualizarPorMesList(dataFiler);
            }
            var FLJ_ID_INTERES_PROCESAR = Guid.NewGuid();
            /**
            * Actualizar solicitud tarea
            */

            ActualizarSolicitudTarea actualizarSolicitudTareaAprobada = new ActualizarSolicitudTarea()
            {
                SLT_ID = Existe.Data.SltIdAprobar,
                SLT_ESTADO_SOLICITUD = 328,
                SLT_OBSERVACION = "Verificación de Derechos Aprobada"
            };

            await _verificacionDerechosRepository.ActualizarSolicitudTareaVerificacionDerechos(actualizarSolicitudTareaAprobada);


            /**
            * Crear tarea Abonos de Interes pendiente por procesar
            */

            //var CodigoPerfilTareaAbonosInteresProcesar = "USER_SUPER_ADMIN";
            var CodigoPerfilTareaAbonosInteresProcesar = "GCICE_L";
            var USR_IDAbonosInteresParaProcesar = await _verificacionDerechosRepository.ObtenerUsuarioVerificacionDerechos(CodigoPerfilTareaAbonosInteresProcesar);



            var verificacionInteresAbonadoTareaProcesar = new VerificacionDerechoSolicitudTarea
            {
                SLT_ID = FLJ_ID_INTERES_PROCESAR,
                TRA_ID = Guid.Parse("A4CDA34E-8EEA-49B3-8D44-A0F4E7A0F3CA"),
                SOL_ID = Guid.Parse("0207CA4B-542D-4F09-893B-D94068FC53D7"),
                USR_ID = (Guid)request.UsuarioId,
                OBSERVACION = "Abonos de Interes pendiente por procesar"


            };
            //await _verificacionDerechosRepository.CrearVerificacionDerechoSolicitudTarea(verificacionInteresAbonadoTareaProcesar);

            /**
            * Crear notificacion tarea Abonos de Interes pendiente por procesar
             */

            var tareaNotificacionProcesarInteresAbonados = new NotificacionTarea
            {
                Usuario = USR_IDAbonosInteresParaProcesar.USR_ID,
                Notificacion = "Abono de Intereses en la etapa de Pendiente por Procesar correspondiente al período " + request.Anio + "/" + request.Mes,
                Modulo = "Abono de Intereses",
                TareaSiguiente = FLJ_ID_INTERES_PROCESAR,
                NumRadicado = request.NumeroRadicado,
                NUM_CEDULA = USR_IDAbonosInteresParaProcesar.NUM_CEDULA,
                UsuarioEnvia = (Guid)request.UsuarioId,


            };

            await _verificacionDerechosRepository.InsertarNotificacionTarea(tareaNotificacionProcesarInteresAbonados);

            await _verificacionDerechosRepository.ActualizarNotificacionTareaVerificacionYAbonos(FLJ_ID_INTERES_PROCESAR, "/AbonoIntereses");

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterPeriodo(ID);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(ID);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterPeriodo(ID);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(ID);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(ID);


            decimal? interesesAbonadosAVAC = AVAC.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_AVAC);
            decimal? interesesAbonadosAVAV = AVAV.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? interesesAbonadosAVC = AVC.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? interesesAbonadosAdministracionCesantias = administracionCesantias.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? interesesAbonadosSolucionViviendaAportes = solucionVivienda.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_APORTES);
            decimal? interesesAbonadosSolucionViviendaCesantias = solucionVivienda.Where(a => a.PERIODO_ANIO == ID.Anio && a.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);

            decimal? totalInteresesAbonados = interesesAbonadosAVAC + interesesAbonadosAVAV + interesesAbonadosAVC + interesesAbonadosAdministracionCesantias +
                interesesAbonadosSolucionViviendaAportes + interesesAbonadosSolucionViviendaCesantias;

            var parametrosAbonoInteres = new ParametrosAbonoInteres
            {
                ANIO = request.Anio,
                MES = request.Mes,
                TOTAL_INTERESES_ABONADOS = totalInteresesAbonados ?? 0,
                NUMERO_RADICADO = request.NumeroRadicado,
                SLT_ID = FLJ_ID_INTERES_PROCESAR,
                USUARIO_ID = USR_IDAbonosInteresParaProcesar.USR_ID,
                USUARIO_NOMBRE = USR_IDAbonosInteresParaProcesar.USR_NOMBRE + " " + USR_IDAbonosInteresParaProcesar.USR_APELLIDO,

            };

            await _verificacionDerechosRepository.ActualizarParametrosInteresAbonadosTotal(parametrosAbonoInteres);



            return new Response<ParametrosVerificacionDerechoDto>
            {
                Data = responseData,
                ReturnMessage = "Verificacion Traida con exito"
            };

        }

        /// <summary>
        /// Metodo para actualrualizar los ect de la verificacion de derechos cuando el mes es el primero del año 
        /// y actualiza los valores de interes
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="ResultVerificacionDerecho" ></param>
        /// <returns></returns> 

        private async Task ActualizarComienzoAnio(ResultVerificacionDerecho dato, IPCDto ipc)
        {

            if (dato.CAMBIO_ESTADO == "si")
            {

                var request = new ActualizarEctCuentasDto
                {
                    ctaID = dato.CTA_ID,
                    EctId = dato.ECT_FINAL

                };

                //await _iCierreMensualRequestProvider.ActualizarECTCuentas(request);


                decimal InAportes = 0;
                decimal IAportes = 0;
                var VPAportes = (decimal)dato.SALDO_ANUAL_APORTES + (decimal)dato.MOVIMIENTOS_MENSUAL_APORTES;

                decimal? InCesantias = 0;
                decimal ICesantias = 0;
                var VPCesantias = (decimal)dato.SALDO_ANUAL_CESANTIAS + (decimal)dato.MOVIMIENTOS_MENSUAL_CESANTIAS;


                var r = (decimal)ipc.ValorIPC / 100;
                var Pa = (decimal)ipc.PorcentajeIPC / 100;

                InAportes = VPAportes * (decimal)r * (1 - (decimal)Pa);
                InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);


                IAportes = VPAportes * (decimal)r * (decimal)Pa;
                ICesantias = VPCesantias * (decimal)r * (decimal)Pa;



                if (dato.ECT_FINAL == 1 || dato.ECT_FINAL == 9)
                {
                    var ActualizacioResult = new ResultVerificacionDerecho
                    {
                        ID = dato.ID,

                        SALDO_ANTERIOR_APORTES = dato.SALDO_ANUAL_APORTES,
                        SALDO_ANTERIOR_CESANTIAS = dato.SALDO_ANUAL_CESANTIAS,

                        INTERESES_CAUSADO_APORTES = IAportes,
                        INTERESES_CAUSADO_CESANTIAS = ICesantias,

                        INTERESES_NO_CAUSADO_APORTES = InAportes,
                        INTERESES_NO_CAUSADO_CESANTIAS = InCesantias,

                        SALDO_FINAL_APORTES = dato.SALDO_FINAL_APORTES + IAportes,
                        SALDO_FINAL_CESANTIAS = dato.SALDO_FINAL_CESANTIAS + ICesantias,

                        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES = dato.INTERES_APORTES + IAportes,
                        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS = dato.INTERES_CESANTIAS + ICesantias,


                    };



                    await _verificacionDerechosRepository.ActualizarVerificacionDerechosResult(ActualizacioResult);
                }
                else
                {

                    var ActualizacioResult = new ResultVerificacionDerecho
                    {
                        ID = dato.ID,

                        SALDO_ANTERIOR_APORTES = dato.SALDO_ANUAL_APORTES,
                        SALDO_ANTERIOR_CESANTIAS = dato.SALDO_ANUAL_CESANTIAS,

                        INTERESES_CAUSADO_APORTES = 0,
                        INTERESES_CAUSADO_CESANTIAS = 0,

                        INTERESES_NO_CAUSADO_APORTES = 0,
                        INTERESES_NO_CAUSADO_CESANTIAS = 0,

                        SALDO_FINAL_APORTES = 0,
                        SALDO_FINAL_CESANTIAS = 0,

                        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES = dato.INTERES_APORTES + IAportes,
                        INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS = dato.INTERES_CESANTIAS + ICesantias,
                    };


                    await _verificacionDerechosRepository.ActualizarVerificacionDerechosResult(ActualizacioResult);

                }

            }
            else if (dato.ECT_FINAL == 1 || dato.ECT_FINAL == 9)
            {


                decimal InAportes = 0;
                decimal IAportes = 0;
                var VPAportes = (decimal)dato.SALDO_ANUAL_APORTES + (decimal)dato.MOVIMIENTOS_MENSUAL_APORTES;

                decimal InCesantias = 0;
                decimal ICesantias = 0;
                var VPCesantias = (decimal)dato.SALDO_ANUAL_CESANTIAS + (decimal)dato.MOVIMIENTOS_MENSUAL_CESANTIAS;


                var r = (decimal)ipc.ValorIPC / 100;
                var Pa = (decimal)ipc.PorcentajeIPC / 100;

                InAportes = VPAportes * (decimal)r * (1 - (decimal)Pa);
                InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);


                IAportes = VPAportes * (decimal)r * (decimal)Pa;
                ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                var ActualizacioResult = new ResultVerificacionDerecho
                {
                    ID = dato.ID,

                    SALDO_ANTERIOR_APORTES = dato.SALDO_ANUAL_APORTES,
                    SALDO_ANTERIOR_CESANTIAS = dato.SALDO_ANUAL_CESANTIAS,

                    INTERESES_CAUSADO_APORTES = IAportes,
                    INTERESES_CAUSADO_CESANTIAS = ICesantias,

                    INTERESES_NO_CAUSADO_APORTES = InAportes,
                    INTERESES_NO_CAUSADO_CESANTIAS = InCesantias,

                    SALDO_FINAL_APORTES = dato.SALDO_FINAL_APORTES + IAportes,
                    SALDO_FINAL_CESANTIAS = dato.SALDO_FINAL_CESANTIAS + ICesantias,

                    INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES = dato.INTERES_APORTES + IAportes,
                    INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS = dato.INTERES_CESANTIAS + ICesantias,
                };



                await _verificacionDerechosRepository.ActualizarVerificacionDerechosResult(ActualizacioResult);



            }

        }

        private async Task ActualizarPorMesList(List<ResultVerificacionDerecho> dato)
        {
            var request = new List<ActualizarEctCuentasDto>();

            foreach (var item in dato)
            {
                request.Add(new ActualizarEctCuentasDto
                {
                    ctaID = item.CTA_ID,
                    EctId = item.ECT_FINAL
                });
            }
            await _iCierreMensualRequestProvider.ActualizarECTCuentasList(request);
        }

        /// <summary>
        /// Metodo para actualrualizar los ect de la verificacion de derechos cuando el mes es diferente al primero del año y actualiza los valores de interes
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="ResultVerificacionDerecho" ></param>
        /// <returns></returns> 
        private async Task ActualizarPorMes(ResultVerificacionDerecho dato, IPCDto ipc)
        {
            var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorCuenta(dato.CTA_ID, ipc.Anio, ipc.Mes - 1);

            decimal saldoFinalCesantias = valores?.SALDO_FINAL_CESANTIAS ?? 0;
            decimal saldoFinalAportes = valores?.SALDO_FINAL_APORTES ?? 0;

            decimal movimientoAportes = dato.MOVIMIENTOS_MENSUAL_APORTES ?? 0;
            decimal movimientoCesantias = dato.MOVIMIENTOS_MENSUAL_CESANTIAS ?? 0;

            decimal VPAportes = saldoFinalAportes + movimientoAportes;
            decimal VPCesantias = saldoFinalCesantias + movimientoCesantias;

            decimal r = (decimal)ipc.ValorIPC / 100;
            decimal Pa = (decimal)ipc.PorcentajeIPC / 100;

            decimal InAportes = VPAportes * r * (1 - Pa);
            decimal InCesantias = VPCesantias * r * (1 - Pa);

            decimal IAportes = VPAportes * r * Pa;
            decimal ICesantias = VPCesantias * r * Pa;

            // Por defecto todos los intereses y saldos son cero
            decimal interesCausadoAportes = 0, interesCausadoCesantias = 0;
            decimal interesNoCausadoAportes = 0, interesNoCausadoCesantias = 0;
            decimal saldoFinalAportesResult = 0, saldoFinalCesantiasResult = 0;

            if (dato.CAMBIO_ESTADO.Trim().ToLower() == "si" || dato.ECT_FINAL == 1 || dato.ECT_FINAL == 9)
            {
                bool conInteres = dato.ECT_FINAL == 1 || dato.ECT_FINAL == 9;

                interesCausadoAportes = conInteres ? IAportes : 0;
                interesCausadoCesantias = conInteres ? ICesantias : 0;

                interesNoCausadoAportes = conInteres ? InAportes : 0;
                interesNoCausadoCesantias = conInteres ? InCesantias : 0;

                saldoFinalAportesResult = saldoFinalAportes + interesCausadoAportes;
                saldoFinalCesantiasResult = saldoFinalCesantias + interesCausadoCesantias;
            }

            var actualizacionResult = new ResultVerificacionDerecho
            {
                ID = dato.ID,

                SALDO_ANTERIOR_APORTES = saldoFinalAportes,
                SALDO_ANTERIOR_CESANTIAS = saldoFinalCesantias,

                INTERESES_CAUSADO_APORTES = interesCausadoAportes,
                INTERESES_CAUSADO_CESANTIAS = interesCausadoCesantias,

                INTERESES_NO_CAUSADO_APORTES = interesNoCausadoAportes,
                INTERESES_NO_CAUSADO_CESANTIAS = interesNoCausadoCesantias,

                SALDO_FINAL_APORTES = saldoFinalAportesResult,
                SALDO_FINAL_CESANTIAS = saldoFinalCesantiasResult,

                INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES = dato.INTERES_APORTES + interesCausadoAportes,
                INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS = dato.INTERES_CESANTIAS + interesCausadoCesantias
            };

            await _verificacionDerechosRepository.ActualizarVerificacionDerechosResult(actualizacionResult);
        }


        /// <summary>
        /// Obtiene el valor del ipc por periodo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 

        public async Task<Response<IPCDto>> ObtenerIPCByPeriodo(VerificacionDerechosPeriodo ID)
        {
            var responseData = this._mapper.Map<IPCDto>(await _verificacionDerechosRepository.ObtenerIPCByPeriodo(ID));



            return new Response<IPCDto>
            {
                Data = responseData,
                ReturnMessage = "Verificacion Traida con exito"
            };
        }


        /// <summary>
        /// Metodo crear el periodo de verificacion de derechos al mismo tiempo que el ipc
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task CrearDatosCuentasSolucionViviendaResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);


            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var lista = await _verificacionDerechosRepository.ConsultaDatosVerificacionDerechoCuenta();

            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {


                estadoFinal = 0;
                //var dat = DateTime.Now.AddMonths(-1);
                var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);
                var meses = ((dat.Month - item.UltimoMesPeriodo) + 12 * (dat.Year - item.UltimoAnioPeriodo));

                var saldo = item.CtaSaldo;
                int? cuotasPendientes = 0;
                var cambioEstado = "";




                if (item.EctId == 5)
                {
                    estadoFinal = 5;

                    if (item.CtaSaldo <= 0 && item.CtaCuotas < 168)
                    {
                        estadoFinal = 7;
                        contadorCambioEstado++;

                    }

                }
                else if (item.CtaCuotas >= 168)
                {
                    estadoFinal = 2;
                    if (item.EctId != estadoFinal)
                    {
                        contadorCambioEstado++;
                    }
                }
                else
                {
                    if (meses <= 0)
                    {
                        estadoFinal = 1;

                        if (item.EctId != estadoFinal)
                        {
                            contadorCambioEstado++;
                        }
                    }
                    else if (meses >= 1 && meses <= 2)
                    {
                        estadoFinal = 9;
                        if (item.EctId != estadoFinal)
                        {
                            contadorCambioEstado++;
                        }
                    }
                    else if ((meses >= 3 && meses < 12) || item.CtaCuotas >= 168)
                    {
                        estadoFinal = 2;
                        if (item.EctId != estadoFinal)
                        {
                            contadorCambioEstado++;
                        }
                    }
                    else if (meses > 12)
                    {
                        if (item.CtaSaldo >= 0)
                        {
                            estadoFinal = 5;
                            if (item.EctId != estadoFinal)
                            {
                                contadorCambioEstado++;
                            }
                        }
                        else if (item.CtaSaldo <= 0 && item.CtaCuotas <= 168)
                        {
                            estadoFinal = 7;
                            contadorCambioEstado++;
                        }
                    }
                    else
                    {
                        estadoFinal = item.EctId;
                    }
                }



                if (item.EctId == 1 && (estadoFinal == 2 || estadoFinal == 5 || estadoFinal == 9))
                {
                    cambioEstado = "si";
                }
                else if (item.EctId == 9 && (estadoFinal == 1 || estadoFinal == 2 || estadoFinal == 5))
                {
                    cambioEstado = "si";
                }
                else if (item.EctId == 2 && (estadoFinal == 1 || estadoFinal == 5 || estadoFinal == 9))
                {
                    cambioEstado = "si";
                }
                else if (item.EctId == 5 && estadoFinal == 7)
                {
                    cambioEstado = "si";
                }
                else
                {
                    cambioEstado = "no";
                }


                if (item.CtaCuotas >= 168)
                {
                    cuotasPendientes = 0;
                }
                else
                {
                    cuotasPendientes = 168 - item.CtaCuotas;
                }

                if (parametroIPC.Mes == 1)
                {



                    var reporte = new ResultVerificacionDerechoDto
                    {

                        Id = 0,
                        CtaId = item.CtaId,
                        CtaNumero = item.CtaNumero,
                        FechaUltimoMovimientoCredito = item.FechaUltimoMovimientoAporte,
                        CtaFechaPrimerAporte = item.CtaFechaPrimerAporte,
                        CtaCuotas = item.CtaCuotas,
                        CuotasPendientes = cuotasPendientes,
                        EctIdInicial = item.EctId,
                        EctIdFinal = estadoFinal,
                        CambiarEstado = cambioEstado,
                        PeriodoAnio = parametroIPC.Anio,
                        PeriodoMes = parametroIPC.Mes,
                        Flag = item.Flag,
                        Identificacion = item.Identificacion,
                        CtaSaldo = item.CtaSaldo,

                        SaldoAnualAportes = item.SaldoAnualAportes,
                        SaldoAnualCesantias = item.SaldoAnualCesantias,
                        MovimientosMensualAportes = item.MovimientosMensualAportes,
                        MovimientosMensualCesantias = item.MovimientosMensualCesantias,

                        SaldoAnteriorAportes = item.SaldoAnualAportes,
                        SaldoAnteriorCesantias = item.SaldoAnualCesantias,
                        SaldoFinalAportes = item.SaldoAnualAportes + item.MovimientosMensualAportes,
                        SaldoFinalCesantias = item.SaldoAnualCesantias + item.MovimientosMensualCesantias,

                        UnidadEjecutora = item.UnidadEjecutora,

                        NumeroSolicitud = consecutivo,
                        TipoSolicitud = "ABONOS INTERESES",

                        InteresesAportes = item.InteresesAportes,
                        InteresesCesantias = item.InteresesCesantias,

                        InteresNoAbonadosSaldoInsuficienteAportes = 0,
                        InteresNoAbonadosSaldoInsuficienteCesantias = 0,





                    };
                    var responseResult = this._mapper.Map<ResultVerificacionDerechoDto>(
                    await this._verificacionDerechosRepository.CrearVerificacionDerechoResult(
                         this._mapper.Map<ResultVerificacionDerecho>(reporte)));


                }
                else
                {
                    var reporte = new ResultVerificacionDerechoDto
                    {

                        Id = 0,
                        CtaId = item.CtaId,
                        CtaNumero = item.CtaNumero,
                        FechaUltimoMovimientoCredito = item.FechaUltimoMovimientoAporte,
                        CtaFechaPrimerAporte = item.CtaFechaPrimerAporte,
                        CtaCuotas = item.CtaCuotas,
                        CuotasPendientes = cuotasPendientes,
                        EctIdInicial = item.EctId,
                        EctIdFinal = estadoFinal,
                        CambiarEstado = cambioEstado,
                        PeriodoAnio = parametroIPC.Anio,
                        PeriodoMes = parametroIPC.Mes,
                        Flag = item.Flag,
                        Identificacion = item.Identificacion,
                        CtaSaldo = item.CtaSaldo,

                        SaldoAnualAportes = item.SaldoAnualAportes,
                        SaldoAnualCesantias = item.SaldoAnualCesantias,
                        MovimientosMensualAportes = item.MovimientosMensualAportes,
                        MovimientosMensualCesantias = item.MovimientosMensualCesantias,
                        UnidadEjecutora = item.UnidadEjecutora,

                        NumeroSolicitud = consecutivo,
                        TipoSolicitud = "ABONOS INTERESES",

                        InteresesAportes = item.InteresesAportes,
                        InteresesCesantias = item.InteresesCesantias,

                        InteresNoAbonadosSaldoInsuficienteAportes = 0,
                        InteresNoAbonadosSaldoInsuficienteCesantias = 0,

                    };

                    var responseResult = this._mapper.Map<ResultVerificacionDerechoDto>(
                        await this._verificacionDerechosRepository.CrearVerificacionDerechoResult(
                             this._mapper.Map<ResultVerificacionDerecho>(reporte)));

                }


            }

            var nuevo = new ParametrosVerificacionDerechoDto
            {
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes,
                FechaOperacion = parametroIPC.FechaActualizacion,
                FechaAprobacion = null,
                TotalCuentasVerificadas = cuentasVerificadaas,
                TotalCambiosEstado = contadorCambioEstado,
                UsuarioId = null,
                UsuarioNombre = null,

            };

            var responseData = this._mapper.Map<ParametrosVerificacionDerechoDto>(
                    await this._verificacionDerechosRepository.ActualizarPeriodoVerificacionDerecho(
                         this._mapper.Map<ParametrosVerificacionDerecho>(nuevo)));



        }

        /// <summary>
        /// Metodo para obtener los valores de interes no causado por año, tipo de cuenta y concepto
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="InteresNoCausadoDto" ></param>
        /// <returns></returns> 

        public async Task<Response<InteresNoCausadoDto>> ObtenerTodoVerificacionDerechosResultByAnioCuenta(InteresNoCausadoDto datos)
        {
            //var lista = await _iClienteRequestProvider.ObtenerCuentaIdCuenta(ID.CtaID.ToString());
            //var saldo = lista.Data.First().SaldoDisponible;

            DateTime primerDia = new DateTime(2023, 1, 1);
            DateTime ultimoDia = new DateTime(2023, 1, 31);

            //var filtro = new VerificacionDerechosByFechaDto
            //{
            //    FechaInicial = primerDia,
            //    FechaFinal = ultimoDia,

            //};


            ////var DatosVerificacion = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAC(filtro);
            ////var DatosVerificacionAVAV = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAV(filtro);
            ////var DatosVerificacionAVC = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVC(filtro);



            //DateTime primerDia1 = new DateTime(2022, 1, 1);
            //DateTime ultimoDia1 = new DateTime(2022, 1, 31);

            //var filtro1 = new VerificacionDerechosByFechaDto
            //{
            //    FechaInicial = primerDia1,
            //    FechaFinal = ultimoDia1,

            //};


            ////var DatosVerificacion1 = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAC(filtro1);
            ////var DatosVerificacionAVAV1 = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAV(filtro1);
            ////var DatosVerificacionAVC1 = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVC(filtro1);





            var ID = new VerificacionDerechosPeriodo
            {
                Anio = datos.Anio,
            };

            //var AV = await _verificacionDerechosRepository.ObtenerTodoVerificacionCuentasAVByAnio(ID);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoVerificacionCuentasAVACByAnio(ID);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoVerificacionCuentasAVAVByAnio(ID);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoVerificacionCuentasAVCByAnio(ID);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoVerificacionSolucionViviendaByAnio(ID);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoVerificacionAdministracionCesantiasByAnio(ID);

            if (datos.Concepto == 0)
            {

                var resultadoAportes = solucionVivienda.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_APORTES);
                var resultadoCesantias = solucionVivienda.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_CESANTIAS);
                var resultadoAVC = AVC.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_AVC);
                var resultadoAdminCesantias = administracionCesantias.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_CESANTIAS);
                var resultadoAVAC = AVAC.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_AVAC);
                var resultadoAVAV = AVAV.Where(a => a.PERIODO_ANIO == datos.Anio && a.IDENTIFICACION == datos.UsuarioId.ToString()).Sum(s => s.INTERESES_NO_CAUSADO_AVAV);

                decimal resultadoFinal = (decimal)resultadoAportes + (decimal)resultadoCesantias + (decimal)resultadoAdminCesantias + (decimal)resultadoAVC + (decimal)resultadoAVAC + (decimal)resultadoAVAV;

                var responseSolucionApotes = new InteresNoCausadoDto
                {

                    Anio = ID.Anio,
                    UsuarioId = datos.UsuarioId,
                    Concepto = datos.Concepto,
                    CtaId = datos.CtaId,
                    TipoCuentaId = datos.TipoCuentaId,

                    Saldo = datos.Saldo,
                    InteresesNoCausados = resultadoFinal,
                    SaldoTotalInteresNoCausado = resultadoFinal + datos.Saldo

                };

                return new Response<InteresNoCausadoDto>
                {
                    Data = responseSolucionApotes,
                    ReturnMessage = "Intereses no causados traidos con exito"
                };


            }

            if (datos.TipoCuentaId == 1)
            {
                if (datos.Concepto == 9)
                {
                    var resultado = solucionVivienda.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_APORTES);

                    var responseSolucionApotes = new InteresNoCausadoDto
                    {

                        Anio = ID.Anio,
                        UsuarioId = datos.UsuarioId,
                        Concepto = datos.Concepto,
                        CtaId = datos.CtaId,

                        Saldo = datos.Saldo,
                        InteresesNoCausados = resultado,
                        SaldoTotalInteresNoCausado = resultado + datos.Saldo

                    };

                    return new Response<InteresNoCausadoDto>
                    {
                        Data = responseSolucionApotes,
                        ReturnMessage = "Intereses no causados traidos con exito"
                    };

                }
                else if (datos.Concepto == 10)
                {
                    var resultado = solucionVivienda.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_CESANTIAS);


                    var responseSolucionApotes = new InteresNoCausadoDto
                    {

                        Anio = ID.Anio,
                        UsuarioId = datos.UsuarioId,
                        Concepto = datos.Concepto,
                        CtaId = datos.CtaId,

                        Saldo = datos.Saldo,
                        InteresesNoCausados = resultado,
                        SaldoTotalInteresNoCausado = resultado + datos.Saldo

                    };

                    return new Response<InteresNoCausadoDto>
                    {
                        Data = responseSolucionApotes,
                        ReturnMessage = "Intereses no causados traidos con exito"
                    };

                }
                else if (datos.Concepto == 29)
                {
                    var resultado = AVC.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_AVC);

                    var responseSolucionApotes = new InteresNoCausadoDto
                    {

                        Anio = ID.Anio,
                        UsuarioId = datos.UsuarioId,
                        Concepto = datos.Concepto,
                        CtaId = datos.CtaId,

                        Saldo = datos.Saldo,
                        InteresesNoCausados = resultado,
                        SaldoTotalInteresNoCausado = resultado + datos.Saldo

                    };

                    return new Response<InteresNoCausadoDto>
                    {
                        Data = responseSolucionApotes,
                        ReturnMessage = "Intereses no causados traidos con exito"
                    };
                }
                //var datos = await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechosResultByAnioCuenta(ID.CtaId, ID.Concepto ,ID.Anio);             

            }
            else if (datos.TipoCuentaId == 2)
            {
                var resultado = administracionCesantias.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_CESANTIAS);

                var responseSolucionApotes = new InteresNoCausadoDto
                {

                    Anio = ID.Anio,
                    UsuarioId = datos.UsuarioId,
                    Concepto = datos.Concepto,
                    CtaId = datos.CtaId,

                    Saldo = datos.Saldo,
                    InteresesNoCausados = resultado,
                    SaldoTotalInteresNoCausado = resultado + datos.Saldo

                };

                return new Response<InteresNoCausadoDto>
                {
                    Data = responseSolucionApotes,
                    ReturnMessage = "Intereses no causados traidos con exito"
                };

            }
            else
            {

                if (datos.Concepto == 24)
                {
                    var resultado = AVAC.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_AVAC);


                    var responseSolucionApotes = new InteresNoCausadoDto
                    {

                        Anio = ID.Anio,
                        UsuarioId = datos.UsuarioId,
                        Concepto = datos.Concepto,
                        CtaId = datos.CtaId,

                        Saldo = datos.Saldo,
                        InteresesNoCausados = resultado,
                        SaldoTotalInteresNoCausado = resultado + datos.Saldo

                    };

                    return new Response<InteresNoCausadoDto>
                    {
                        Data = responseSolucionApotes,
                        ReturnMessage = "Intereses no causados traidos con exito"
                    };
                }
                else if (datos.Concepto == 26)
                {
                    var resultado = AVAV.Where(a => a.PERIODO_ANIO == datos.Anio && a.CTA_ID == datos.CtaId).Sum(s => s.INTERESES_NO_CAUSADO_AVAV);

                    var responseSolucionApotes = new InteresNoCausadoDto
                    {

                        Anio = ID.Anio,
                        UsuarioId = datos.UsuarioId,
                        Concepto = datos.Concepto,
                        CtaId = datos.CtaId,

                        Saldo = datos.Saldo,
                        InteresesNoCausados = resultado,
                        SaldoTotalInteresNoCausado = resultado + datos.Saldo

                    };

                    return new Response<InteresNoCausadoDto>
                    {
                        Data = responseSolucionApotes,
                        ReturnMessage = "Intereses no causados traidos con exito"
                    };
                }




            }
            decimal? sumatoria = 0;

            var responseData = new InteresNoCausadoDto
            {

                Anio = ID.Anio,
                UsuarioId = datos.UsuarioId,
                Concepto = datos.Concepto,
                CtaId = datos.CtaId,
                Saldo = datos.Saldo,
                InteresesNoCausados = 0,
                SaldoTotalInteresNoCausado = datos.Saldo

            };

            return new Response<InteresNoCausadoDto>
            {
                Data = responseData,
                ReturnMessage = "Intereses no causados traidos con exito"
            };
        }

        public async Task<Response<IEnumerable<LiquidacionCuentasSolucionViviendaDTO>>> ObtenerInteresesCuentasSolucionVivienda(InteresNoCausadoDto request)
        {
            var datos = await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechosResultByAnioCuenta(request.CtaId, request.Anio);

            var list = new List<LiquidacionCuentasSolucionViviendaDTO>();

            foreach (var item in datos)
            {

                var periodo = new VerificacionDerechosPeriodo
                {

                    Anio = request.Anio,
                    Mes = item.PERIODO_MES

                };
                var ipc = await ObtenerIPCByPeriodo(periodo);

                var vivienda = new LiquidacionCuentasSolucionViviendaDTO
                {
                    Mes = item.PERIODO_MES,
                    VP = item.CTA_SALDO,
                    Ipc = (decimal)ipc.Data.ValorIPC,
                    Pa = (decimal)ipc.Data.PorcentajeIPC,


                    InteresNoCausado = item.INTERESES_NO_CAUSADO_APORTES,
                    InteresCausado = item.INTERESES_CAUSADO_APORTES,
                    Vf = item.CTA_SALDO + item.INTERESES_NO_CAUSADO_APORTES + item.INTERESES_CAUSADO_APORTES,
                };

                list.Add(vivienda);
            }




            return new Response<IEnumerable<LiquidacionCuentasSolucionViviendaDTO>>
            {
                Data = list,
                ReturnMessage = "Liquidación "
            };
        }






        /// <summary>
        /// Metodo crear el periodo de verificacion de derechos al mismo tiempo que el ipc
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns> 
        public async Task CrearDatosCuentasAdministracionCesantiasResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);


            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            // var lista = await _iCierreMensualRequestProvider.ObtenerCedulaCuentaCesantias();
            var data = await _verificacionDerechosRepository.ConsultarDatosCedulasCesantias(parametroIPC.Mes, parametroIPC.Anio);

            var lista = _mapper.Map<List<DatosCedulasCesantiasDto>>(data);
          //  lista.Data = lista.Data;

            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {

                var requet = new ObtenerInfoCedulaRequestDto
                {
                    CuentaId = item.CuentaId,
                    Periodo = parametroIPC.Anio
                };
                var dataResponse = await _verificacionDerechosRepository.ConsultarDatosInfoCesantia(requet);
               var dataCuentaResponse = _mapper.Map<DatosInfoCensatiaByCuentaPeriodoDto>(dataResponse);
                //var dataCuentaResponse = await _iCierreMensualRequestProvider.ObtenerInfoCuentaByPeriodo(requet);
                //var dataCuenta = dataCuentaResponse.Data;
                var dataCuenta = dataCuentaResponse;

                if (parametroIPC.Mes == 1)
                {

                    var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);


                    decimal InCesantias = 0;
                    decimal ICesantias = 0;
                    decimal VPCesantias = 0;

                    VPCesantias = (decimal)dataCuenta.SaldoAnual;


                    var r = (decimal)parametroIPC.ValorIPC / 100;
                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                    InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                    ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                    var reporte = new DatosCuentasAdministracionCesantiasResultDto
                    {

                        Id = 0,
                        CtaId = item.CuentaId,
                        CtaNumero = item.CuentaNumero,
                        CliId = item.IdCliente,
                        TctId = 0,
                        CtaSaldo = (decimal)dataCuenta.SaldoCuenta,
                        FechaUltimoMovimientosCredito = DateTime.Now,
                        Identificacion = item.Identificacion,

                        PeriodoAnio = parametroIPC.Anio,
                        PeriodoMes = parametroIPC.Mes,


                        SaldoAnualCesantias = (decimal)dataCuenta.SaldoAnual > 0 ? (decimal)dataCuenta.SaldoAnual : 0,
                        MovimientosMensualCesantias = 0,

                        SaldoAnteriorCesantias = 0,

                        InteseresCausadosCesantias = ICesantias,
                        InteseresNoCausadosCesantias = InCesantias,

                        SaldoFinalCesantias = VPCesantias + ICesantias,


                        UnidadEjecutora = 0,

                        NumeroSolicitud = consecutivo,
                        TipoSolicitud = "ABONOS INTERESES",

                        InteresesCesantias = 0,

                        InteresNoAbonadosSaldoInsuficienteCesantias = 0 + ICesantias,

                    };
                    var responseResult = this._mapper.Map<DatosCuentasAdministracionCesantiasResultDto>(
                    await this._verificacionDerechosRepository.CrearDatosCuentasAdministracionCesantiasResult(
                         this._mapper.Map<DatosCuentasAdministracionCesantiasResult>(reporte)));
                }
                else
                {
                    var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);
                    var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorCuentaCesantias(0, parametroIPC.Anio, parametroIPC.Mes - 1);

                    decimal InCesantias = 0;
                    decimal ICesantias = 0;
                    decimal VPCesantias = 0;
                    decimal saldoFinalCesantias = 0;


                    saldoFinalCesantias = (decimal)dataCuenta.SaldoAnual > 0 ? (decimal)dataCuenta.SaldoAnual : 0;
                    VPCesantias = (decimal)dataCuenta.SaldoAnual > 0 ? (decimal)dataCuenta.SaldoAnual : 0;


                    var r = (decimal)parametroIPC.ValorIPC / 100;
                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;

                    InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                    ICesantias = VPCesantias * (decimal)r * (decimal)Pa;

                    var reporte = new DatosCuentasAdministracionCesantiasResultDto
                    {
                        Id = 0,
                        CtaId = 0,
                        CtaNumero = 0,
                        CliId = 0,
                        TctId = 0,
                        CtaSaldo = (decimal)dataCuenta.SaldoCuenta,
                        FechaUltimoMovimientosCredito = DateTime.Now,
                        Identificacion = item.Identificacion,
                        PeriodoAnio = parametroIPC.Anio,
                        PeriodoMes = parametroIPC.Mes,
                        SaldoAnualCesantias = (decimal)dataCuenta.SaldoAnual > 0 ? (decimal)dataCuenta.SaldoAnual : 0,
                        MovimientosMensualCesantias = 0,
                        InteseresCausadosCesantias = ICesantias,
                        InteseresNoCausadosCesantias = InCesantias,
                        SaldoAnteriorCesantias = saldoFinalCesantias,
                        SaldoFinalCesantias = VPCesantias + ICesantias,
                        UnidadEjecutora = 0,
                        NumeroSolicitud = consecutivo,
                        TipoSolicitud = "ABONOS INTERESES",
                        InteresesCesantias = 0,
                        InteresNoAbonadosSaldoInsuficienteCesantias = 0 + ICesantias,
                    };

                    var responseResult = this._mapper.Map<DatosCuentasAdministracionCesantiasResultDto>(
                    await this._verificacionDerechosRepository.CrearDatosCuentasAdministracionCesantiasResult(
                         this._mapper.Map<DatosCuentasAdministracionCesantiasResult>(reporte)));

                }

            }

        }




        /// <summary>
        /// Metodo crear el periodo de abonos de interes para las cuentas AV
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task CrearDatosCuentasAVResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var lista = await _verificacionDerechosRepository.ICT_ConsultarCuentasTemporales();

            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {                //var dat = DateTime.Now.AddMonths(-1);

                if (parametroIPC.Mes == 1)
                {
                    if (item.AVAC != null)
                    {

                        decimal InAVAC = 0;
                        decimal IAVAC = 0;
                        decimal VPAVAC = 0;

                        VPAVAC = (decimal)(item.SaldoAnualAVAC) + (decimal)(item.MovimientosMensualAVAC);


                        var r = (decimal)parametroIPC.ValorIPC / 100;
                        var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                        InAVAC = VPAVAC * (decimal)r * (1 - (decimal)Pa);
                        IAVAC = VPAVAC * (decimal)r * (decimal)Pa;


                        var reporte = new DatosCuentasAVResultDto
                        {

                            Id = 0,
                            CtaId = item.CtaId,
                            CtaNumero = item.CtaNumero,
                            PeriodoAnio = parametroIPC.Anio,
                            PeriodoMes = parametroIPC.Mes,




                            ValorAVAC = item.ValorAVAC,
                            SaldoAnualAVAC = item.SaldoAnualAVC,
                            MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                            InteresesCausadosAVAC = IAVAC,
                            InteresesNoCausadosAVAC = InAVAC,
                            SaldoAnteriorAVAC = VPAVAC,
                            SaldoFinalAVAC = VPAVAC + IAVAC,

                            Identificacion = item.Identificacion,
                            UnidadEjecutora = item.UnidadEjecutora,

                            NumeroSolicitud = consecutivo,
                            TipoSolicitud = "ABONOS INTERESES",

                        };

                        var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                        await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                             this._mapper.Map<DatosCuentasAVResult>(reporte)));





                    }
                    if (item.AVAV != null)
                    {




                        decimal InCesantias = 0;
                        decimal ICesantias = 0;
                        decimal VPCesantias = 0;

                        VPCesantias = (decimal)(item.SaldoAnualAVAV) + (decimal)(item.MovimientosMensualAVAV);


                        var r = (decimal)parametroIPC.ValorIPC / 100;
                        var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                        InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                        ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                        var reporte = new DatosCuentasAVResultDto
                        {

                            Id = 0,
                            CtaId = item.CtaId,
                            CtaNumero = item.CtaNumero,
                            PeriodoAnio = parametroIPC.Anio,
                            PeriodoMes = parametroIPC.Mes,
                            ValorAVAV = item.ValorAVAV,

                            SaldoAnualAVAV = item.SaldoAnualAVAV,
                            MovimientosMensualAVAV = item.MovimientosMensualAVAV,

                            InteresesCausadosAVAV = ICesantias,
                            InteresesNoCausadosAVAV = InCesantias,

                            SaldoAnteriorAVAV = VPCesantias,
                            SaldoFinalAVAV = VPCesantias + ICesantias,
                            Identificacion = item.Identificacion,
                            UnidadEjecutora = item.UnidadEjecutora,

                            NumeroSolicitud = consecutivo,
                            TipoSolicitud = "ABONOS INTERESES",

                        };

                        var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                        await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                             this._mapper.Map<DatosCuentasAVResult>(reporte)));

                    }
                    if (item.AVC != null)
                    {



                        decimal InCesantias = 0;
                        decimal ICesantias = 0;
                        decimal VPCesantias = 0;

                        VPCesantias = (decimal)(item.SaldoAnualAVC) + (decimal)(item.MovimientosMensualAVC);


                        var r = (decimal)parametroIPC.ValorIPC / 100;
                        var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                        InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                        ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                        var reporte = new DatosCuentasAVResultDto
                        {

                            Id = 0,
                            CtaId = item.CtaId,
                            CtaNumero = item.CtaNumero,
                            PeriodoAnio = parametroIPC.Anio,
                            PeriodoMes = parametroIPC.Mes,


                            ValorAVC = item.ValorAVC ?? 0,
                            SaldoAnualAVC = item.SaldoAnualAVC ?? 0,
                            MovimientosMensualAVC = item.MovimientosMensualAVC ?? 0,
                            InteresesCausadosAVC = ICesantias,
                            InteresesNoCausadosAVC = InCesantias,
                            SaldoAnteriorAVC = VPCesantias,
                            SaldoFinalAVC = VPCesantias + ICesantias,

                            Identificacion = item.Identificacion,
                            UnidadEjecutora = item.UnidadEjecutora,

                            NumeroSolicitud = consecutivo,
                            TipoSolicitud = "ABONOS INTERESES",

                        };




                        var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                        await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                             this._mapper.Map<DatosCuentasAVResult>(reporte)));



                    }



                }
                else
                {
                    var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVResult(item.CtaId, parametroIPC.Anio, parametroIPC.Mes - 1);


                    if (valores == null)
                    {



                        if (item.AVAC != null)
                        {

                            decimal InAVAC = 0;
                            decimal IAVAC = 0;
                            decimal VPAVAC = 0;

                            VPAVAC = (decimal)(item.SaldoAnualAVAC) + (decimal)(item.MovimientosMensualAVAC);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InAVAC = VPAVAC * (decimal)r * (1 - (decimal)Pa);
                            IAVAC = VPAVAC * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,




                                ValorAVAC = item.ValorAVAC,
                                SaldoAnualAVAC = item.SaldoAnualAVC,
                                MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                                InteresesCausadosAVAC = IAVAC,
                                InteresesNoCausadosAVAC = InAVAC,
                                SaldoAnteriorAVAC = VPAVAC,
                                SaldoFinalAVAC = VPAVAC + IAVAC,

                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };

                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));





                        }
                        if (item.AVAV != null)
                        {




                            decimal InCesantias = 0;
                            decimal ICesantias = 0;
                            decimal VPCesantias = 0;

                            VPCesantias = (decimal)(item.SaldoAnualAVAV) + (decimal)(item.MovimientosMensualAVAV);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                            ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,
                                ValorAVAV = item.ValorAVAV,

                                SaldoAnualAVAV = item.SaldoAnualAVAV,
                                MovimientosMensualAVAV = item.MovimientosMensualAVAV,

                                InteresesCausadosAVAV = ICesantias,
                                InteresesNoCausadosAVAV = InCesantias,

                                SaldoAnteriorAVAV = VPCesantias,
                                SaldoFinalAVAV = VPCesantias + ICesantias,

                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };

                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));

                        }
                        if (item.AVC != null)
                        {



                            decimal InCesantias = 0;
                            decimal ICesantias = 0;
                            decimal VPCesantias = 0;

                            VPCesantias = (decimal)(item.SaldoAnualAVAC) + (decimal)(item.MovimientosMensualAVC);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                            ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,


                                ValorAVC = item.ValorAVC ?? 0,
                                SaldoAnualAVC = item.SaldoAnualAVC ?? 0,
                                MovimientosMensualAVC = item.MovimientosMensualAVC ?? 0,
                                InteresesCausadosAVC = ICesantias,
                                InteresesNoCausadosAVC = InCesantias,
                                SaldoAnteriorAVC = VPCesantias,
                                SaldoFinalAVC = VPCesantias + ICesantias,


                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };




                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));



                        }



                    }
                    else
                    {





                        if (valores.SALDO_FINAL_AVAC != null)
                        {
                            var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);




                            decimal InCesantias = 0;
                            decimal ICesantias = 0;
                            decimal VPCesantias = 0;

                            VPCesantias = (decimal)(valores.SALDO_FINAL_AVAC) + (decimal)(item.MovimientosMensualAVAC);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                            ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,


                                ValorAVAC = item.ValorAVAC,
                                SaldoAnualAVAC = item.SaldoAnualAVC,
                                MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                                InteresesCausadosAVAC = ICesantias,
                                InteresesNoCausadosAVAC = InCesantias,
                                SaldoAnteriorAVAC = valores.SALDO_FINAL_AVAC,
                                SaldoFinalAVAC = VPCesantias + ICesantias,

                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };

                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));





                        }
                        if (valores.SALDO_FINAL_AVAV != null)
                        {

                            var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);

                            // var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVResult(item.CtaId, parametroIPC.Anio, parametroIPC.Mes - 1);


                            decimal InCesantias = 0;
                            decimal ICesantias = 0;
                            decimal VPCesantias = 0;

                            VPCesantias = (decimal)(valores.SALDO_FINAL_AVAV) + (decimal)(item.MovimientosMensualAVAV);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                            ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,
                                ValorAVAV = item.ValorAVAV,

                                SaldoAnualAVAV = item.SaldoAnualAVAV,
                                MovimientosMensualAVAV = item.MovimientosMensualAVAV,

                                InteresesCausadosAVAV = ICesantias,
                                InteresesNoCausadosAVAV = InCesantias,

                                SaldoAnteriorAVAV = valores.SALDO_FINAL_AVAV,
                                SaldoFinalAVAV = VPCesantias + ICesantias,

                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };

                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));

                        }
                        if (valores.SALDO_FINAL_AVC != null)
                        {


                            var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);
                            //var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVResult(item.CtaId, parametroIPC.Anio, parametroIPC.Mes - 1);


                            decimal InCesantias = 0;
                            decimal ICesantias = 0;
                            decimal VPCesantias = 0;

                            VPCesantias = (decimal)(valores.SALDO_FINAL_AVC) + (decimal)(item.MovimientosMensualAVC);


                            var r = (decimal)parametroIPC.ValorIPC / 100;
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                            InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                            ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                            var reporte = new DatosCuentasAVResultDto
                            {

                                Id = 0,
                                CtaId = item.CtaId,
                                CtaNumero = item.CtaNumero,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,


                                ValorAVC = item.ValorAVC,
                                SaldoAnualAVC = item.SaldoAnualAVC,
                                MovimientosMensualAVC = item.MovimientosMensualAVC,
                                InteresesCausadosAVC = ICesantias,
                                InteresesNoCausadosAVC = InCesantias,
                                SaldoAnteriorAVC = valores.SALDO_FINAL_AVC,
                                SaldoFinalAVC = VPCesantias + ICesantias,

                                Identificacion = item.Identificacion,
                                UnidadEjecutora = item.UnidadEjecutora,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                            };

                            var responseResult = this._mapper.Map<DatosCuentasAVResult>(
                            await this._verificacionDerechosRepository.CrearDatosCuentasAVResult(
                                 this._mapper.Map<DatosCuentasAVResult>(reporte)));



                        }



                    }

                }

            }

        }

        /// <summary>
        /// Metodo para crear el periodo de abonos de interes para las cuentas AVAC
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task CrearDatosCuentasAVACResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var dataAvac = await _verificacionDerechosRepository.ConsultarCuentasAVAC();
            var lista = _mapper.Map<List<ObtenerCuentasAVACDto>>(dataAvac);
            //  var lista = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAC(filtro);

            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {
                //var dataClienteResponse = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.Identificacion);
                //var dataCliente = dataClienteResponse.Data;
                //var data = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(dataCliente.IdTipoIdentificacion, item.Identificacion);
                //var dataInformacionCliente = data.Data;

                //var dataCuentas = dataInformacionCliente.Cuentas;

                var dataCliente = await _verificacionDerechosRepository.ConsultarInformacionCliente(item.Identificacion);
                if (dataCliente != null)
                {
                    var dataCuentas = await _verificacionDerechosRepository.ConsultarInformacioCuentaCliente(dataCliente.IdCliente);


                    var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("06")).ToList();
                    var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                    if (dataInfoCuenta.Count > 0)
                    {

                        DateTime primerDiaMesAnterior = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
                        foreach (var cuenta in dataInfoCuenta)
                        {

                            cuenta.Conceptos = await _verificacionDerechosRepository.ConsultarInformacioConceptoCuentaCliente(cuenta.IdCuenta);
                            cuenta.Movimientos = await _verificacionDerechosRepository.ConsultarInformacioMovimientoCuentaCliente(cuenta.IdCuenta);
                            var saldoAnterior = new List<decimal>();
                            var saldoAnual = new double();
                            foreach (var concepto in cuenta.Conceptos)
                            {
                                if (concepto.ConceptoId == 23 || concepto.ConceptoId == 24)
                                {
                                    saldoAnual = concepto.Sva;
                                    var saldoAcumulado = new List<decimal>();
                                    foreach (var m in cuenta.Movimientos)
                                    {
                                        saldoAcumulado.Add(m.MVT_VALOR);
                                    }
                                    var saldo = saldoAcumulado.Sum(c => c);
                                    var movimiento = cuenta.Movimientos.Where(x => x.CNC_ID == concepto.ConceptoId).ToList();
                                    var movimientoOrden = movimiento.OrderByDescending(x => x.MVT_FECHA_MOVIMIENTO).ToList();
                                    var dataV = movimientoOrden.Where(mov => mov.MVT_FECHA_MOVIMIENTO > primerDiaMesAnterior).ToList();
                                    var valorMovimiento = dataV.Sum(x => x.MVT_VALOR);
                                    saldoAnterior.Add(dataV.Count == 0 ? (decimal)concepto.saldo : (decimal)concepto.saldo - (decimal)valorMovimiento);
                                }
                            }
                            var valorSaldoMesAnterior = saldoAnterior.Sum();
                            if (parametroIPC.Mes == 1)
                            {
                                decimal InAVAC = 0;
                                decimal IAVAC = 0;
                                decimal VPAVAC = 0;
                                VPAVAC = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                                var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                                var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                                InAVAC = VPAVAC * (decimal)r * (1 - (decimal)Pa);
                                IAVAC = VPAVAC * (decimal)r * (decimal)Pa;


                                var reporte = new ObtenerCuentasAVACResultDto
                                {

                                    Id = 0,
                                    CtaId = item.CtaId,
                                    CtaNumero = item.CtaNumero,
                                    EctId = item.EctId,
                                    PeriodoAnio = parametroIPC.Anio,
                                    PeriodoMes = parametroIPC.Mes,
                                    CtaSaldo = cuenta.SaldoDisponible,

                                    ValorAVAC = item.ValorAVAC,
                                    SaldoAnualAVAC = item.SaldoAnualAVAC,
                                    MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                                    InteresesAVAC = item.InteresesAVAC,
                                    InteresesCausadoAVAC = IAVAC,
                                    InteresesNoCausadoAVAC = InAVAC,
                                    SaldoAnteriorAVAC = valorSaldoMesAnterior,
                                    SaldoFinalAVAC = 0,

                                    CliId = item.CliId,
                                    Identificacion = item.Identificacion,
                                    UnidadEjecutora = item.UnidadEjecutora,

                                    NumeroSolicitud = consecutivo,
                                    TipoSolicitud = "ABONOS INTERESES",

                                    InteresNoAbonadosSaldoInsuficienteAVAC = item.InteresesAVAC + IAVAC,

                                };

                                var responseResult = this._mapper.Map<ObtenerCuentasAVACResult>(
                                await this._verificacionDerechosRepository.InsertarDatosCuentasAVAC(
                                     this._mapper.Map<ObtenerCuentasAVACResult>(reporte)));

                            }
                            else
                            {
                                var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVACResult(item.CtaId, parametroIPC.Anio, parametroIPC.Mes - 1);


                                if (valores == null)
                                {
                                    decimal InAVAC = 0;
                                    decimal IAVAC = 0;
                                    decimal VPAVAC = 0;

                                    VPAVAC = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                                    var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                                    InAVAC = VPAVAC * (decimal)r * (1 - (decimal)Pa);
                                    IAVAC = VPAVAC * (decimal)r * (decimal)Pa;   /// TENER EN CUENTA LOS DECIMALES,


                                    var reporte = new ObtenerCuentasAVACResultDto
                                    {

                                        Id = 0,
                                        CtaId = item.CtaId,
                                        CtaNumero = item.CtaNumero,
                                        EctId = item.EctId,
                                        PeriodoAnio = parametroIPC.Anio,
                                        PeriodoMes = parametroIPC.Mes,
                                        CtaSaldo = cuenta.SaldoDisponible,

                                        ValorAVAC = item.ValorAVAC,
                                        SaldoAnualAVAC = item.SaldoAnualAVAC,
                                        MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                                        InteresesAVAC = item.InteresesAVAC,
                                        InteresesNoCausadoAVAC = InAVAC,
                                        InteresesCausadoAVAC = IAVAC,
                                        SaldoAnteriorAVAC = valorSaldoMesAnterior,
                                        SaldoFinalAVAC = item.CtaSaldo,

                                        Identificacion = item.Identificacion,
                                        UnidadEjecutora = item.UnidadEjecutora,

                                        NumeroSolicitud = consecutivo,
                                        TipoSolicitud = "ABONOS INTERESES",

                                        InteresNoAbonadosSaldoInsuficienteAVAC = item.InteresesAVAC + IAVAC

                                    };

                                    var responseResult = this._mapper.Map<ObtenerCuentasAVACResult>(
                                    await this._verificacionDerechosRepository.InsertarDatosCuentasAVAC(
                                         this._mapper.Map<ObtenerCuentasAVACResult>(reporte)));


                                }
                                else
                                {


                                    var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);


                                    decimal InCesantias = 0;
                                    decimal ICesantias = 0;
                                    decimal VPCesantias = 0;

                                    VPCesantias = (decimal)(valores.SALDO_FINAL_AVAC) + (decimal)(item.MovimientosMensualAVAC);


                                    var r = (decimal)parametroIPC.ValorIPC / 100;
                                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                                    InCesantias = VPCesantias * (decimal)r * (1 - (decimal)Pa);
                                    ICesantias = VPCesantias * (decimal)r * (decimal)Pa;


                                    var reporte = new ObtenerCuentasAVACResultDto
                                    {

                                        Id = 0,
                                        CtaId = item.CtaId,
                                        CtaNumero = item.CtaNumero,
                                        EctId = item.EctId,
                                        PeriodoAnio = parametroIPC.Anio,
                                        PeriodoMes = parametroIPC.Mes,
                                        CtaSaldo = cuenta.SaldoDisponible,

                                        ValorAVAC = item.ValorAVAC,
                                        SaldoAnualAVAC = item.SaldoAnualAVAC,
                                        MovimientosMensualAVAC = item.MovimientosMensualAVAC,
                                        InteresesAVAC = item.InteresesAVAC,
                                        InteresesNoCausadoAVAC = InCesantias,
                                        InteresesCausadoAVAC = ICesantias,
                                        SaldoAnteriorAVAC = valorSaldoMesAnterior,
                                        SaldoFinalAVAC = item.CtaSaldo,

                                        CliId = item.CliId,
                                        Identificacion = item.Identificacion,
                                        UnidadEjecutora = item.UnidadEjecutora,

                                        NumeroSolicitud = consecutivo,
                                        TipoSolicitud = "ABONOS INTERESES",

                                        InteresNoAbonadosSaldoInsuficienteAVAC = item.InteresesAVAC + ICesantias

                                    };

                                    var responseResult = this._mapper.Map<ObtenerCuentasAVACResult>(
                                    await this._verificacionDerechosRepository.InsertarDatosCuentasAVAC(
                                         this._mapper.Map<ObtenerCuentasAVACResult>(reporte)));


                                }

                            }
                        }
                    }
                }
            }
        }








        /// <summary>
        /// Metodo para crear el periodo de abonos de interes para las cuentas AVAV
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task CrearDatosCuentasAVAVResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var lista = await _verificacionDerechosRepository.ConsultarDaraCuentaAVAV();


            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {                //var dat = DateTime.Now.AddMonths(-1);

                var dataCliente = await _verificacionDerechosRepository.ConsultarInformacionCliente(item.Identificacion);
                if (dataCliente != null)
                {
                    var dataCuentas = await _verificacionDerechosRepository.ConsultarInformacioCuentaCliente(dataCliente.IdCliente);

                    var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("07")).ToList();
                    var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                    if (dataInfoCuenta.Count > 0)
                    {
                        DateTime primerDiaMesAnterior = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);

                        foreach (var cuenta in dataInfoCuenta)
                        {

                            cuenta.Conceptos = await _verificacionDerechosRepository.ConsultarInformacioConceptoCuentaCliente(cuenta.IdCuenta);
                            cuenta.Movimientos = await _verificacionDerechosRepository.ConsultarInformacioMovimientoCuentaCliente(cuenta.IdCuenta);
                            var saldoAnterior = new List<decimal>();
                            foreach (var concepto in cuenta.Conceptos)
                            {
                                if (concepto.ConceptoId == 25 || concepto.ConceptoId == 26)
                                {
                                    var saldoAcumulado = new List<decimal>();
                                    foreach (var m in cuenta.Movimientos)
                                    {
                                        saldoAcumulado.Add(m.MVT_VALOR);
                                    }
                                    var saldo = saldoAcumulado.Sum(c => c);
                                    var movimiento = cuenta.Movimientos.Where(x => x.CNC_ID == concepto.ConceptoId).ToList();
                                    var movimientoOrden = movimiento.OrderByDescending(x => x.MVT_FECHA_MOVIMIENTO).ToList();
                                    var dataV = movimientoOrden.Where(mov => mov.MVT_FECHA_MOVIMIENTO > primerDiaMesAnterior).ToList();
                                    var valorMovimiento = dataV.Sum(x => x.MVT_VALOR);
                                    saldoAnterior.Add(dataV.Count == 0 ? (decimal)concepto.saldo : (decimal)concepto.saldo - (decimal)valorMovimiento);
                                }
                            }
                            var valorSaldoMesAnterior = saldoAnterior.Sum();

                            if (parametroIPC.Mes == 1)
                            {

                                decimal InAVAV = 0;
                                decimal IAVAV = 0;
                                decimal VPAVAV = 0;

                                VPAVAV = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                                var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                                var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                                InAVAV = VPAVAV * (decimal)r * (1 - (decimal)Pa);
                                IAVAV = VPAVAV * (decimal)r * (decimal)Pa;


                                var reporte = new ObtenerCuentasAVAVResultDto
                                {

                                    Id = 0,
                                    CtaId = item.CuentaId,
                                    CtaNumero = item.CuentaNumero,
                                    EctId = 0,
                                    PeriodoAnio = parametroIPC.Anio,
                                    PeriodoMes = parametroIPC.Mes,
                                    CtaSaldo = cuenta.SaldoDisponible,

                                    ValorAVAV = 0,
                                    SaldoAnualAVAV = 0,
                                    MovimientosMensualAVAV = 0,
                                    InteresesAVAV = 0,
                                    InteresesCausadoAVAV = IAVAV,
                                    InteresesNoCausadoAVAV = InAVAV,
                                    SaldoAnteriorAVAV = (decimal)valorSaldoMesAnterior,
                                    SaldoFinalAVAV = 0,

                                    CliId = item.IdCliente,
                                    Identificacion = item.Identificacion,
                                    UnidadEjecutora = 0,

                                    NumeroSolicitud = consecutivo,
                                    TipoSolicitud = "ABONOS INTERESES",

                                    InteresNoAbonadosSaldoInsuficienteAVAV = 0 + IAVAV

                                };

                                var responseResult = this._mapper.Map<ObtenerCuentasAVAVResult>(
                                await this._verificacionDerechosRepository.InsertarDatosCuentasAVAV(
                                     this._mapper.Map<ObtenerCuentasAVAVResult>(reporte)));



                            }
                            else
                            {
                                var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVAVResult(item.IdCliente, parametroIPC.Anio, parametroIPC.Mes - 1);


                                if (valores == null)
                                {

                                    decimal InAVAV = 0;
                                    decimal IAVAV = 0;
                                    decimal VPAVAV = 0;
                                    VPAVAV = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                                    var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                                    InAVAV = VPAVAV * (decimal)r * (1 - (decimal)Pa);
                                    IAVAV = VPAVAV * (decimal)r * (decimal)Pa;


                                    var reporte = new ObtenerCuentasAVAVResultDto
                                    {


                                        Id = 0,
                                        CtaId = item.CuentaId,
                                        CtaNumero = item.CuentaNumero,
                                        EctId = 0,
                                        PeriodoAnio = parametroIPC.Anio,
                                        PeriodoMes = parametroIPC.Mes,
                                        CtaSaldo = cuenta.SaldoDisponible,

                                        ValorAVAV = 0,
                                        SaldoAnualAVAV = 0,
                                        MovimientosMensualAVAV = 0,
                                        InteresesAVAV = 0,
                                        InteresesNoCausadoAVAV = InAVAV,
                                        InteresesCausadoAVAV = IAVAV,
                                        SaldoAnteriorAVAV = (decimal)valorSaldoMesAnterior,
                                        SaldoFinalAVAV = 0,

                                        Identificacion = item.Identificacion,
                                        UnidadEjecutora = 0,

                                        NumeroSolicitud = consecutivo,
                                        TipoSolicitud = "ABONOS INTERESES",

                                        InteresNoAbonadosSaldoInsuficienteAVAV = 0 + IAVAV,

                                    };

                                    var responseResult = this._mapper.Map<ObtenerCuentasAVAVResult>(
                                    await this._verificacionDerechosRepository.InsertarDatosCuentasAVAV(
                                         this._mapper.Map<ObtenerCuentasAVAVResult>(reporte)));


                                }
                                else
                                {


                                    var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);


                                    decimal InAVAV = 0;
                                    decimal IAVAV = 0;
                                    decimal VPAVAV = 0;

                                    VPAVAV = valorSaldoMesAnterior;


                                    var r = (decimal)parametroIPC.ValorIPC / 100;
                                    var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                                    InAVAV = VPAVAV * (decimal)r * (1 - (decimal)Pa);
                                    IAVAV = VPAVAV * (decimal)r * (decimal)Pa;


                                    var reporte = new ObtenerCuentasAVAVResultDto
                                    {


                                        Id = 0,
                                        CtaId = item.CuentaId,
                                        CtaNumero = item.CuentaNumero,
                                        EctId = 0,
                                        PeriodoAnio = parametroIPC.Anio,
                                        PeriodoMes = parametroIPC.Mes,
                                        CtaSaldo = cuenta.SaldoDisponible,

                                        ValorAVAV = 0,
                                        SaldoAnualAVAV = 0,
                                        MovimientosMensualAVAV = 0,
                                        InteresesAVAV = 0,
                                        InteresesNoCausadoAVAV = InAVAV,
                                        InteresesCausadoAVAV = IAVAV,
                                        SaldoAnteriorAVAV = (decimal)valorSaldoMesAnterior,
                                        SaldoFinalAVAV = VPAVAV + IAVAV,

                                        CliId = item.IdCliente,
                                        Identificacion = item.Identificacion,
                                        UnidadEjecutora = 0,

                                        NumeroSolicitud = consecutivo,
                                        TipoSolicitud = "ABONOS INTERESES",

                                        InteresNoAbonadosSaldoInsuficienteAVAV = 0 + IAVAV,

                                    };

                                    var responseResult = this._mapper.Map<ObtenerCuentasAVAVResult>(
                                    await this._verificacionDerechosRepository.InsertarDatosCuentasAVAV(
                                         this._mapper.Map<ObtenerCuentasAVAVResult>(reporte)));

                                }
                            }
                        }

                    }

                }


            }
        }



        /// <summary>
        /// Metodo para crear el periodo de abonos de interes para las cuentas AVC
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task CrearDatosCuentasAVCResult(IPCDto parametroIPC, long consecutivo)
        {

            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var lista = await _verificacionDerechosRepository.ConsultarCuentaAVC();


            var cuentasVerificadaas = lista.Count;
            string codigoMensaje = "";
            var contadorCambioEstado = 0;
            var estadoFinal = 0;

            foreach (var item in lista)
            {
                //var dataClienteResponse = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.Identificacion);
                //var dataCliente = dataClienteResponse.Data;
                //var data = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(dataCliente.IdTipoIdentificacion, item.Identificacion);
                //var dataInformacionCliente = data.Data;
                var dataCliente = await _verificacionDerechosRepository.ConsultarInformacionCliente(item.Identificacion);

                var dataCuentas = await _verificacionDerechosRepository.ConsultarInformacioCuentaCliente(dataCliente.IdCliente);

                var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("05")).ToList();
                var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                if (dataInfoCuenta.Count > 0)
                {
                    DateTime primerDiaMesAnterior = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
                    foreach (var cuenta in dataInfoCuenta)
                    {

                        cuenta.Conceptos = await _verificacionDerechosRepository.ConsultarInformacioConceptoCuentaCliente(cuenta.IdCuenta);
                        cuenta.Movimientos = await _verificacionDerechosRepository.ConsultarInformacioMovimientoCuentaCliente(cuenta.IdCuenta);
                        var saldoAnterior = new List<decimal>();
                        foreach (var concepto in cuenta.Conceptos)
                        {
                            if (concepto.ConceptoId == 28 || concepto.ConceptoId == 29)
                            {
                                var saldoAcumulado = new List<decimal>();
                                foreach (var m in cuenta.Movimientos)
                                {
                                    saldoAcumulado.Add(m.MVT_VALOR);
                                }
                                var saldo = saldoAcumulado.Sum(c => c);
                                var movimiento = cuenta.Movimientos.Where(x => x.CNC_ID == concepto.ConceptoId).ToList();
                                var movimientoOrden = movimiento.OrderByDescending(x => x.MVT_FECHA_MOVIMIENTO).ToList();
                                var dataV = movimientoOrden.Where(mov => mov.MVT_FECHA_MOVIMIENTO > primerDiaMesAnterior).ToList();
                                var valorMovimiento = dataV.Sum(x => x.MVT_VALOR);
                                saldoAnterior.Add(dataV.Count == 0 ? (decimal)concepto.saldo : (decimal)concepto.saldo - (decimal)valorMovimiento);
                            }
                        }
                        var valorSaldoMesAnterior = saldoAnterior.Sum();

                        if (parametroIPC.Mes == 1)
                        {
                            decimal InAVC = 0;
                            decimal IAVC = 0;
                            decimal VPAVC = 0;
                            VPAVC = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                            var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                            var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                            InAVC = VPAVC * (decimal)r * (1 - (decimal)Pa);
                            IAVC = VPAVC * (decimal)r * (decimal)Pa;


                            var reporte = new ObtenerCuentasAVCResultDto
                            {

                                Id = 0,
                                CtaId = item.CuentaId,
                                CtaNumero = item.CuentaNumero,
                                EctId = 0,
                                PeriodoAnio = parametroIPC.Anio,
                                PeriodoMes = parametroIPC.Mes,
                                CtaSaldo = cuenta.SaldoDisponible,

                                ValorAVC = 0,
                                SaldoAnualAVC = 0,
                                MovimientosMensualAVC = 0,
                                InteresesAVC = 0,
                                InteresesCausadoAVC = IAVC,
                                InteresesNoCausadoAVC = InAVC,
                                SaldoAnteriorAVC = (decimal)valorSaldoMesAnterior,
                                SaldoFinalAVC = 0,

                                CliId = item.IdCliente,
                                Identificacion = item.Identificacion,
                                UnidadEjecutora = 0,

                                NumeroSolicitud = consecutivo,
                                TipoSolicitud = "ABONOS INTERESES",

                                InteresNoAbonadosSaldoInsuficienteAVC = 0 + IAVC,

                            };

                            var responseResult = this._mapper.Map<ObtenerCuentasAVCResult>(
                            await this._verificacionDerechosRepository.InsertarDatosCuentasAVC(
                                 this._mapper.Map<ObtenerCuentasAVCResult>(reporte)));



                        }
                        else
                        {
                            var valores = await _verificacionDerechosRepository.ObtenerValorMesAnteriorDatosCuentasAVCResult(item.CuentaId, parametroIPC.Anio, parametroIPC.Mes - 1);


                            if (valores == null)
                            {



                                decimal InAVC = 0;
                                decimal IAVC = 0;
                                decimal VPAVC = 0;

                                VPAVC = valorSaldoMesAnterior; /// SALDO DEL MES ANTERIOR

                                var r = (decimal)parametroIPC.ValorIPC / 100; // VARICION MENUSAL DEL IPC
                                var Pa = (decimal)parametroIPC.PorcentajeIPC / 100; // PORCENTAJE DE ABONO


                                InAVC = VPAVC * (decimal)r * (1 - (decimal)Pa);
                                IAVC = VPAVC * (decimal)r * (decimal)Pa;


                                var reporte = new ObtenerCuentasAVCResultDto
                                {
                                    Id = 0,
                                    CtaId = item.CuentaId,
                                    CtaNumero = item.CuentaNumero,
                                    EctId = 0,
                                    PeriodoAnio = parametroIPC.Anio,
                                    PeriodoMes = parametroIPC.Mes,
                                    CtaSaldo = cuenta.SaldoDisponible,

                                    ValorAVC = 0,
                                    SaldoAnualAVC = 0,
                                    MovimientosMensualAVC = 0,
                                    InteresesAVC = 0,
                                    InteresesNoCausadoAVC = InAVC,
                                    InteresesCausadoAVC = IAVC,
                                    SaldoAnteriorAVC = (decimal)valorSaldoMesAnterior,

                                    Identificacion = item.Identificacion,
                                    UnidadEjecutora = 0,

                                    NumeroSolicitud = consecutivo,
                                    TipoSolicitud = "ABONOS INTERESES",

                                    InteresNoAbonadosSaldoInsuficienteAVC = 0 + IAVC,

                                };

                                var responseResult = this._mapper.Map<ObtenerCuentasAVCResult>(
                                await this._verificacionDerechosRepository.InsertarDatosCuentasAVC(
                                     this._mapper.Map<ObtenerCuentasAVCResult>(reporte)));


                            }
                            else
                            {


                                var dat = new DateTime(parametroIPC.Anio, parametroIPC.Mes, ultimoDia.Day);


                                decimal InAVC = 0;
                                decimal IAVC = 0;
                                decimal VPAVC = 0;

                                VPAVC = valorSaldoMesAnterior;


                                var r = (decimal)parametroIPC.ValorIPC / 100;
                                var Pa = (decimal)parametroIPC.PorcentajeIPC / 100;


                                InAVC = VPAVC * (decimal)r * (1 - (decimal)Pa);
                                IAVC = VPAVC * (decimal)r * (decimal)Pa;


                                var reporte = new ObtenerCuentasAVCResultDto
                                {

                                    Id = 0,
                                    CtaId = item.CuentaId,
                                    CtaNumero = item.CuentaNumero,
                                    EctId = 0,
                                    PeriodoAnio = parametroIPC.Anio,
                                    PeriodoMes = parametroIPC.Mes,
                                    CtaSaldo = cuenta.SaldoDisponible,

                                    ValorAVC = 0,
                                    SaldoAnualAVC = 0,
                                    MovimientosMensualAVC = 0,
                                    InteresesAVC = 0,
                                    InteresesNoCausadoAVC = InAVC,
                                    InteresesCausadoAVC = IAVC,
                                    SaldoAnteriorAVC = (decimal)valorSaldoMesAnterior,
                                    SaldoFinalAVC = 0,

                                    CliId = item.IdCliente,
                                    Identificacion = item.Identificacion,
                                    UnidadEjecutora = 0,

                                    NumeroSolicitud = consecutivo,
                                    TipoSolicitud = "ABONOS INTERESES",

                                    InteresNoAbonadosSaldoInsuficienteAVC = 0 + IAVC,

                                };

                                var responseResult = this._mapper.Map<ObtenerCuentasAVCResult>(
                                await this._verificacionDerechosRepository.InsertarDatosCuentasAVC(
                                     this._mapper.Map<ObtenerCuentasAVCResult>(reporte)));




                            }
                        }
                    }
                }

            }

        }



        /// <summary>
        /// Metodo para crear el periodo de abonos de interes
        /// </summary>
        /// <author>Julian Nova</author>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>


        public async Task<Response<ParametrosAbonosInteresDto>> CrearPeriodoValoresInteresAbonados(IPCDto parametroIPC, long consecutivo)
        {

            var parametrosAbonoInteresDto = new ParametrosAbonosInteresDto
            {
                NumeroSolicitud = null,
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes,
                ValorIpc = parametroIPC.ValorIPC,
                PorcentajeIpc = parametroIPC.PorcentajeIPC,
                fechaRecepcion = DateTime.Now,
                ComprobanteSolucionViviendaAportes = "COMPROBANTE: 4-1023",
                ComprobanteSolucionAdministracionCesantias = "COMPROBANTE: 4-1024",
                ComprobanteAVAV = "COMPROBANTE: 4-1031",
                ComprobanteAVC = "COMPROBANTE: 4-1032",
                ComprobanteAVAC = "COMPROBANTE: 4-1029",

                Estado = 1,
                NombreEstado = "Pendiente por procesar",
            };






            parametrosAbonoInteresDto.NumeroSolicitud = consecutivo.ToString();

            var responseData = this._mapper.Map<ParametrosAbonosInteresDto>(
                  await this._verificacionDerechosRepository.CrearPeriodoValoresInteresAbonados(
                       this._mapper.Map<ParametrosAbonoInteres>(parametrosAbonoInteresDto)));



            return new Response<ParametrosAbonosInteresDto>
            {
                Data = responseData,
                ReturnMessage = "Verificación Creado con éxito"
            };
        }

        /// <summary>
        /// Metodo para obtener todos los datos de los intereses abonados creados
        /// </summary>
        /// <author>Julian Nova</author>
        /// <returns></returns>


        public async Task<Response<IEnumerable<ParametrosAbonosInteresDto>>> ObtenerTodoValoresInteresAbonados()
        {
            var lista = await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonados();
            var responseData = this._mapper.Map<IEnumerable<ParametrosAbonosInteresDto>>(lista);



            return new Response<IEnumerable<ParametrosAbonosInteresDto>>
            {
                Data = responseData,
                ReturnMessage = "Periodos Intereses Abonos traídos satisfactoriamente"
            };
        }


        /// <summary>
        /// Metodo para obtener todos los datos de los intereses abonados creados
        /// </summary>
        /// <author>Julian Nova</author>
        /// <returns></returns>

        public async Task<Response<IEnumerable<ParametrosAbonosInteresDto>>> ObtenerTodoValoresInteresAbonadosFiltroByPeriodo(VerificacionDerechosByFiltroPeriodo ID)
        {

            var responseData = this._mapper.Map<IEnumerable<ParametrosAbonosInteresDto>>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadossFiltroByPeriodo(ID));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;



            return new Response<IEnumerable<ParametrosAbonosInteresDto>>
            {
                Data = responseData,
                ReturnMessage = "Abono Interes por rango traido con exito"
            };

        }


        /// <summary>
        /// Metodo para obtener el consolidado de abono de interes por el periodo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 


        public async Task<Response<IEnumerable<InformacionConsolidadoBaseCalculo>>> ObtenerConsolidadoFiltroByPeriodo(VerificacionDerechosPeriodo ID)
        {


            List<InformacionConsolidadoBaseCalculo> datos = new List<InformacionConsolidadoBaseCalculo>();

            var IPC = this._mapper.Map<ParametrosAbonosInteresDto>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(ID));

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterPeriodo(ID);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(ID);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterPeriodo(ID);
            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(ID);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(ID);




            decimal? totalBase = 0;
            decimal? interesAbonado = 0;
            decimal? interesNoAbonado = 0;
            decimal? interesReconocidos = 0;
            decimal? interesIPCNegativo = 0;


            //////////////////ABONAR INTERESES AVAC

            ////EL SALDO SIEMPRE VA HACER EL VALOR 
            //
            var listInteresAbono = new List<Tuple<decimal, string>>();
            var listInteresCausando = new List<Tuple<decimal, string>>();
            var listInteresNoCausado = new List<Tuple<decimal, string>>();
            foreach (var item in AVAC)
            {
                if (item.PERIODO_ANIO == ID.Anio && item.PERIODO_MES == ID.Mes)
                {
                    listInteresAbono.Add(new Tuple<decimal, string>(item.SALDO_ANTERIOR_AVAC, item.IDENTIFICACION));
                    listInteresCausando.Add(new Tuple<decimal, string>(item.INTERESES_CAUSADO_AVAC, item.IDENTIFICACION));
                    listInteresNoCausado.Add(new Tuple<decimal, string>(item.INTERESES_NO_CAUSADO_AVAC, item.IDENTIFICACION));

                }
            }
            var SumaAbonarInteresesAVACBasePeriodo = listInteresAbono.Sum(x => x.Item1);
            var SumaAbonarInteresesAVACAbonadosPeriodo = listInteresCausando.Sum(x => x.Item1);
            var SumaAbonarInteresesAVACNoAbonadosPeriodo = listInteresNoCausado.Sum(x => x.Item1);
            decimal SumaAbonarInteresesAVACReconocidosPeriodo = SumaAbonarInteresesAVACAbonadosPeriodo + SumaAbonarInteresesAVACNoAbonadosPeriodo;
            decimal SumaAbonarInteresesAVACIPCNegativoPeriodo = 0;

            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesAVACIPCNegativoPeriodo = AVAC.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAC);
            }


            var responseDatosAVAC = new InformacionConsolidadoBaseCalculo
            {
                Id = 1,
                TotalBase = SumaAbonarInteresesAVACBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesAVACAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesAVACNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesAVACReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesAVACIPCNegativoPeriodo,

            };


            //////////////////ABONAR INTERESES AVAV


            decimal? SumaAbonarInteresesAVAVBasePeriodo = AVAV.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.SALDO_ANTERIOR_AVAV);
            decimal? SumaAbonarInteresesAVAVAbonadosPeriodo = AVAV.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaAbonarInteresesAVAVNoAbonadosPeriodo = AVAV.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_NO_CAUSADO_AVAV);
            decimal? SumaAbonarInteresesAVAVReconocidosPeriodo = SumaAbonarInteresesAVAVAbonadosPeriodo + SumaAbonarInteresesAVAVNoAbonadosPeriodo;
            decimal? SumaAbonarInteresesAVAVIPCNegativoPeriodo = 0;

            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesAVAVIPCNegativoPeriodo = AVAV.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVAV);
            }

            var responseDatosAVAV = new InformacionConsolidadoBaseCalculo
            {
                Id = 2,
                TotalBase = SumaAbonarInteresesAVAVBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesAVAVAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesAVAVNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesAVAVReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesAVAVIPCNegativoPeriodo,

            };





            //////////////////ABONAR INTERESES CESANTIAS


            decimal? SumaAbonarInteresesAdministracionCesantiasBasePeriodo = administracionCesantias.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.SALDO_FINAL_CESANTIAS);
            decimal? SumaAbonarInteresesAdministracionCesantiasAbonadosPeriodo = administracionCesantias.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaAbonarInteresesAdministracionCesantiasNoAbonadosPeriodo = administracionCesantias.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_NO_CAUSADO_CESANTIAS);
            decimal? SumaAbonarInteresesAdministracionCesantiasReconocidosPeriodo = SumaAbonarInteresesAdministracionCesantiasAbonadosPeriodo + SumaAbonarInteresesAdministracionCesantiasNoAbonadosPeriodo;
            decimal? SumaAbonarInteresesAdministracionCesantiasIPCNegativoPeriodo = 0;

            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesAdministracionCesantiasIPCNegativoPeriodo = administracionCesantias.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS);
            }

            var responseDatosCesantias = new InformacionConsolidadoBaseCalculo
            {
                Id = 3,
                TotalBase = SumaAbonarInteresesAdministracionCesantiasBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesAdministracionCesantiasAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesAdministracionCesantiasNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesAdministracionCesantiasReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesAdministracionCesantiasIPCNegativoPeriodo,

            };


            decimal? SumaAbonarInteresesSolucionViviendaAportesBasePeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.SALDO_FINAL_APORTES);
            decimal? SumaAbonarInteresesSolucionViviendaAportesAbonadosPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_APORTES);
            decimal? SumaAbonarInteresesSolucionViviendaAportesNoAbonadosPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_NO_CAUSADO_APORTES);
            decimal? SumaAbonarInteresesSolucionViviendaAportesReconocidosPeriodo = SumaAbonarInteresesSolucionViviendaAportesAbonadosPeriodo + SumaAbonarInteresesSolucionViviendaAportesNoAbonadosPeriodo;
            decimal? SumaAbonarInteresesSolucionViviendaAportesIPCNegativoPeriodo = 0;

            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesSolucionViviendaAportesIPCNegativoPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_APORTES);
            }

            var responseDatosSolucionViviendaAportes = new InformacionConsolidadoBaseCalculo
            {
                Id = 4,
                TotalBase = SumaAbonarInteresesSolucionViviendaAportesBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesSolucionViviendaAportesAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesSolucionViviendaAportesNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesSolucionViviendaAportesReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesSolucionViviendaAportesIPCNegativoPeriodo,

            };


            //////////////////ABONAR INTERESES VIVIENDA CESANTIAS


            decimal? SumaAbonarInteresesSolucionViviendaCesantiasBasePeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.SALDO_FINAL_CESANTIAS);
            decimal? SumaAbonarInteresesSolucionViviendaCesantiasAbonadosPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaAbonarInteresesSolucionViviendaCesantiasNoAbonadosPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_NO_CAUSADO_CESANTIAS);
            decimal? SumaAbonarInteresesSolucionViviendaCesantiasReconocidosPeriodo = SumaAbonarInteresesSolucionViviendaCesantiasAbonadosPeriodo + SumaAbonarInteresesSolucionViviendaCesantiasNoAbonadosPeriodo;
            decimal? SumaAbonarInteresesSolucionViviendaCesantiasIPCNegativoPeriodo = 0;

            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesSolucionViviendaCesantiasIPCNegativoPeriodo = solucionVivienda.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_CESANTIAS);
            }

            var responseDatosSolucionViviendaCesantias = new InformacionConsolidadoBaseCalculo
            {
                Id = 5,
                TotalBase = SumaAbonarInteresesSolucionViviendaCesantiasBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesSolucionViviendaCesantiasAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesSolucionViviendaCesantiasNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesSolucionViviendaCesantiasReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesSolucionViviendaCesantiasIPCNegativoPeriodo,

            };



            //////////////////ABONAR INTERESES AVC


            decimal? SumaAbonarInteresesAVCBasePeriodo = AVC.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.SALDO_ANTERIOR_AVC);
            decimal? SumaAbonarInteresesAVCAbonadosPeriodo = AVC.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaAbonarInteresesAVCNoAbonadosPeriodo = AVC.Take(3).Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Sum(x => x.INTERESES_NO_CAUSADO_AVC);
            decimal? SumaAbonarInteresesAVCReconocidosPeriodo = SumaAbonarInteresesAVCAbonadosPeriodo + SumaAbonarInteresesAVCNoAbonadosPeriodo;
            decimal? SumaAbonarInteresesAVCIPCNegativoPeriodo = 0;


            if (IPC.ValorIpc < 0)
            {
                SumaAbonarInteresesAVCIPCNegativoPeriodo = AVC.Where(x => x.PERIODO_ANIO == ID.Anio && x.PERIODO_MES == ID.Mes).Where(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC < 0).Sum(x => x.INTERES_NO_ABONADOS_SALDO_INSUFICIENTE_AVC);
            }


            var responseDatosAVC = new InformacionConsolidadoBaseCalculo
            {
                Id = 6,
                TotalBase = SumaAbonarInteresesAVCBasePeriodo,
                TotaInteresAbonadoPeriodo = SumaAbonarInteresesAVCAbonadosPeriodo,
                TotaInteresNoAbonadoPeriodo = SumaAbonarInteresesAVCNoAbonadosPeriodo,
                TotaInteresReconocidoPeriodo = SumaAbonarInteresesAVCReconocidosPeriodo,
                TotaInteresIPCNegativo = SumaAbonarInteresesAVCIPCNegativoPeriodo,

            };

            //valores finales


            datos.Add(responseDatosAVAC);
            datos.Add(responseDatosAVAV);
            datos.Add(responseDatosCesantias);
            datos.Add(responseDatosSolucionViviendaAportes);
            datos.Add(responseDatosSolucionViviendaCesantias);
            datos.Add(responseDatosAVC);




            return new Response<IEnumerable<InformacionConsolidadoBaseCalculo>>
            {
                Data = datos,
                ReturnMessage = "Abono Interes por rango traido con exito"
            };

        }



        /// <summary>
        /// Metodo para obtener el consolidado de abono de interes por unidad ejecutora y por el periodo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns> 
        public async Task<Response<IEnumerable<InformacionAbonoInteresUnidadEjecutora>>> ObtenerInformacionAbonoInteresUnidadEjecutoraFiltroByPeriodo(VerificacionDerechosPeriodo ID)
        {



            List<InformacionAbonoInteresUnidadEjecutora> abonosUnidadEjecutora = new List<InformacionAbonoInteresUnidadEjecutora>();
            ////var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterPeriodo(ID);
            ///
            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterPeriodo(ID);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(ID);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterPeriodo(ID);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(ID);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(ID);


            decimal? SumaEjecitoNacionalSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                          + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaEjecitoNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaEjecitoNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaEjecitoNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaEjecitoNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 1).Sum(x => x.INTERESES_CAUSADO_AVAC);

            var EjecitoNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 1,
                TotalSolucionVivienda = SumaEjecitoNacionalSolucionVivienda,
                TotaAdministracionCesantias = SumaEjecitoNacionalCesantias,
                TotaAVAV = SumaEjecitoNacionalAVAV,
                TotaAVC = SumaEjecitoNacionalAVC,
                TotaAVAC = SumaEjecitoNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(EjecitoNacional);


            decimal? SumaCajaPromotoraMilitarPoliciaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaCajaPromotoraMilitarPoliciaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaCajaPromotoraMilitarPoliciaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaCajaPromotoraMilitarPoliciaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaCajaPromotoraMilitarPoliciaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 2).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var CajaPromotoraMilitarPolicia = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 2,
                TotalSolucionVivienda = SumaCajaPromotoraMilitarPoliciaSolucionVivienda,
                TotaAdministracionCesantias = SumaCajaPromotoraMilitarPoliciaCesantias,
                TotaAVAV = SumaCajaPromotoraMilitarPoliciaAVAV,
                TotaAVC = SumaCajaPromotoraMilitarPoliciaAVC,
                TotaAVAC = SumaCajaPromotoraMilitarPoliciaAVAC,
            };
            abonosUnidadEjecutora.Add(CajaPromotoraMilitarPolicia);


            decimal? SumaPoliciaNacionalSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPoliciaNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPoliciaNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaPoliciaNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaPoliciaNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 3).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var PoliciaNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 3,
                TotalSolucionVivienda = SumaPoliciaNacionalSolucionVivienda,
                TotaAdministracionCesantias = SumaPoliciaNacionalCesantias,
                TotaAVAV = SumaPoliciaNacionalAVAV,
                TotaAVC = SumaPoliciaNacionalAVC,
                TotaAVAC = SumaPoliciaNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(PoliciaNacional);


            decimal? SumaPensionadosPoliciaNacionalSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPensionadosPoliciaNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPensionadosPoliciaNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaPensionadosPoliciaNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaPensionadosPoliciaNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 5).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var PensionadosPoliciaNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 5,
                TotalSolucionVivienda = SumaPensionadosPoliciaNacionalSolucionVivienda,
                TotaAdministracionCesantias = SumaPensionadosPoliciaNacionalCesantias,
                TotaAVAV = SumaPensionadosPoliciaNacionalAVAV,
                TotaAVC = SumaPensionadosPoliciaNacionalAVC,
                TotaAVAC = SumaPensionadosPoliciaNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(PensionadosPoliciaNacional);


            decimal? SumaSanidadPoliciaNacionalSolucionVIvienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSanidadPoliciaNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSanidadPoliciaNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaSanidadPoliciaNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaSanidadPoliciaNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 6).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var SanidadPoliciaNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 6,
                TotalSolucionVivienda = SumaSanidadPoliciaNacionalSolucionVIvienda,
                TotaAdministracionCesantias = SumaSanidadPoliciaNacionalCesantias,
                TotaAVAV = SumaSanidadPoliciaNacionalAVAV,
                TotaAVC = SumaSanidadPoliciaNacionalAVC,
                TotaAVAC = SumaSanidadPoliciaNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(SanidadPoliciaNacional);



            decimal? SumaGabineteSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGabineteCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGabineteAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGabineteAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGabineteAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 7).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var Gabinete = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 7,
                TotalSolucionVivienda = SumaGabineteSolucionVivienda,
                TotaAdministracionCesantias = SumaGabineteCesantias,
                TotaAVAV = SumaGabineteAVAV,
                TotaAVC = SumaGabineteAVC,
                TotaAVAC = SumaGabineteAVAC,
            };
            abonosUnidadEjecutora.Add(Gabinete);

            decimal? SumaPensionadosFFMMSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPensionadosFFMMCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPensionadosFFMMAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaPensionadosFFMMAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaPensionadosFFMMAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 8).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var PensionadosFFMMS = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 8,
                TotalSolucionVivienda = SumaPensionadosFFMMSolucionVivienda,
                TotaAdministracionCesantias = SumaPensionadosFFMMCesantias,
                TotaAVAV = SumaPensionadosFFMMAVAV,
                TotaAVC = SumaPensionadosFFMMAVC,
                TotaAVAC = SumaPensionadosFFMMAVAC,
            };
            abonosUnidadEjecutora.Add(PensionadosFFMMS);


            decimal? SumaComandoGeneralFFMMSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaComandoGeneralFFMMCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaComandoGeneralFFMMAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaComandoGeneralFFMMAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaComandoGeneralFFMMAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 9).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var ComandoGeneralFFMM = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 9,
                TotalSolucionVivienda = SumaComandoGeneralFFMMSolucionVivienda,
                TotaAdministracionCesantias = SumaComandoGeneralFFMMCesantias,
                TotaAVAV = SumaComandoGeneralFFMMAVAV,
                TotaAVC = SumaComandoGeneralFFMMAVC,
                TotaAVAC = SumaComandoGeneralFFMMAVAC,
            };
            abonosUnidadEjecutora.Add(ComandoGeneralFFMM);



            decimal? SumaArmadaNacionalSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaArmadaNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaArmadaNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaArmadaNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaArmadaNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 10).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var ArmadaNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 10,
                TotalSolucionVivienda = SumaArmadaNacionalSolucionVivienda,
                TotaAdministracionCesantias = SumaArmadaNacionalCesantias,
                TotaAVAV = SumaArmadaNacionalAVAV,
                TotaAVC = SumaArmadaNacionalAVC,
                TotaAVAC = SumaArmadaNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(ArmadaNacional);


            decimal? SumaFuerzaAereaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaFuerzaAereaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaFuerzaAereaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaFuerzaAereaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaFuerzaAereaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 11).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var FuerzaAerea = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 11,
                TotalSolucionVivienda = SumaFuerzaAereaSolucionVivienda,
                TotaAdministracionCesantias = SumaFuerzaAereaCesantias,
                TotaAVAV = SumaFuerzaAereaAVAV,
                TotaAVC = SumaFuerzaAereaAVC,
                TotaAVAC = SumaFuerzaAereaAVAC,
            };
            abonosUnidadEjecutora.Add(FuerzaAerea);



            decimal? SumaSueldosRetiroPoliciaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSueldosRetiroPoliciaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSueldosRetiroPoliciaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaSueldosRetiroPoliciaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaSueldosRetiroPoliciaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 12).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var SueldosRetiroPolicia = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 12,
                TotalSolucionVivienda = SumaSueldosRetiroPoliciaSolucionVivienda,
                TotaAdministracionCesantias = SumaSueldosRetiroPoliciaCesantias,
                TotaAVAV = SumaSueldosRetiroPoliciaAVAV,
                TotaAVC = SumaSueldosRetiroPoliciaAVC,
                TotaAVAC = SumaSueldosRetiroPoliciaAVAC,
            };
            abonosUnidadEjecutora.Add(SueldosRetiroPolicia);



            decimal? SumaGeneralSanidadMilitarSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadMilitarCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadMilitarAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGeneralSanidadMilitarAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGeneralSanidadMilitarAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 13).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var GeneralSanidadMilitar = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 13,
                TotalSolucionVivienda = SumaGeneralSanidadMilitarSolucionVivienda,
                TotaAdministracionCesantias = SumaGeneralSanidadMilitarCesantias,
                TotaAVAV = SumaGeneralSanidadMilitarAVAV,
                TotaAVC = SumaGeneralSanidadMilitarAVC,
                TotaAVAC = SumaGeneralSanidadMilitarAVAC,
            };
            abonosUnidadEjecutora.Add(GeneralSanidadMilitar);




            decimal? SumaGeneralSanidadEjercitoSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadEjercitoCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadEjercitoAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGeneralSanidadEjercitoAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGeneralSanidadEjercitoAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 14).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var GeneralSanidadEjercito = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 14,
                TotalSolucionVivienda = SumaGeneralSanidadEjercitoSolucionVivienda,
                TotaAdministracionCesantias = SumaGeneralSanidadEjercitoCesantias,
                TotaAVAV = SumaGeneralSanidadEjercitoAVAV,
                TotaAVC = SumaGeneralSanidadEjercitoAVC,
                TotaAVAC = SumaGeneralSanidadEjercitoAVAC,
            };
            abonosUnidadEjecutora.Add(GeneralSanidadEjercito);


            decimal? SumaGeneralSanidadArmadaVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadArmadaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadArmadaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGeneralSanidadArmadaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGeneralSanidadArmadaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 15).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var GeneralSanidadArmada = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 15,
                TotalSolucionVivienda = SumaGeneralSanidadArmadaVivienda,
                TotaAdministracionCesantias = SumaGeneralSanidadArmadaCesantias,
                TotaAVAV = SumaGeneralSanidadArmadaAVAV,
                TotaAVC = SumaGeneralSanidadArmadaAVC,
                TotaAVAC = SumaGeneralSanidadArmadaAVAC,
            };
            abonosUnidadEjecutora.Add(GeneralSanidadArmada);



            decimal? SumaGeneralSanidadFuerzaAereaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadFuerzaAereaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralSanidadFuerzaAereaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGeneralSanidadFuerzaAereaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGeneralSanidadFuerzaAereaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 16).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var GeneralSanidadFuerzaAerea = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 16,
                TotalSolucionVivienda = SumaGeneralSanidadFuerzaAereaSolucionVivienda,
                TotaAdministracionCesantias = SumaGeneralSanidadFuerzaAereaCesantias,
                TotaAVAV = SumaGeneralSanidadFuerzaAereaAVAV,
                TotaAVC = SumaGeneralSanidadFuerzaAereaAVC,
                TotaAVAC = SumaGeneralSanidadFuerzaAereaAVAC,
            };
            abonosUnidadEjecutora.Add(GeneralSanidadFuerzaAerea);



            decimal? SumaRetiroFFMMSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaRetiroFFMMCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaRetiroFFMMAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaRetiroFFMMAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaRetiroFFMMAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 17).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var RetiroFFMM = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 17,
                TotalSolucionVivienda = SumaRetiroFFMMSolucionVivienda,
                TotaAdministracionCesantias = SumaRetiroFFMMCesantias,
                TotaAVAV = SumaRetiroFFMMAVAV,
                TotaAVC = SumaRetiroFFMMAVC,
                TotaAVAC = SumaRetiroFFMMAVAC,
            };
            abonosUnidadEjecutora.Add(RetiroFFMM);


            decimal? SumaSoldadosEjercitoNacionalSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSoldadosEjercitoNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSoldadosEjercitoNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaSoldadosEjercitoNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaSoldadosEjercitoNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 18).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var SoldadosEjercitoNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 18,
                TotalSolucionVivienda = SumaSoldadosEjercitoNacionalSolucionVivienda,
                TotaAdministracionCesantias = SumaSoldadosEjercitoNacionalCesantias,
                TotaAVAV = SumaSoldadosEjercitoNacionalAVAV,
                TotaAVC = SumaSoldadosEjercitoNacionalAVC,
                TotaAVAC = SumaSoldadosEjercitoNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(SoldadosEjercitoNacional);



            decimal? SumaSoldadosArmadaNacionalVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSoldadosArmadaNacionalCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaSoldadosArmadaNacionalAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaSoldadosArmadaNacionalAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaSoldadosArmadaNacionalAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 19).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var SoldadosArmadaNacional = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 19,
                TotalSolucionVivienda = SumaSoldadosArmadaNacionalVivienda,
                TotaAdministracionCesantias = SumaSoldadosArmadaNacionalCesantias,
                TotaAVAV = SumaSoldadosArmadaNacionalAVAV,
                TotaAVC = SumaSoldadosArmadaNacionalAVC,
                TotaAVAC = SumaSoldadosArmadaNacionalAVAC,
            };
            abonosUnidadEjecutora.Add(SoldadosArmadaNacional);


            decimal? SumaGeneralMaritimaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralMaritimaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaGeneralMaritimaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaGeneralMaritimaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaGeneralMaritimaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 20).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var GeneralMaritima = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 20,
                TotalSolucionVivienda = SumaGeneralMaritimaSolucionVivienda,
                TotaAdministracionCesantias = SumaGeneralMaritimaCesantias,
                TotaAVAV = SumaGeneralMaritimaAVAV,
                TotaAVC = SumaGeneralMaritimaAVC,
                TotaAVAC = SumaGeneralMaritimaAVAC,
            };
            abonosUnidadEjecutora.Add(GeneralMaritima);



            decimal? SumaUnidadEspecialJusticiaSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaUnidadEspecialJusticiaCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaUnidadEspecialJusticiaAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaUnidadEspecialJusticiaAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaUnidadEspecialJusticiaAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 21).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var UnidadEspecialJusticia = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 21,
                TotalSolucionVivienda = SumaUnidadEspecialJusticiaSolucionVivienda,
                TotaAdministracionCesantias = SumaUnidadEspecialJusticiaCesantias,
                TotaAVAV = SumaUnidadEspecialJusticiaAVAV,
                TotaAVC = SumaUnidadEspecialJusticiaAVC,
                TotaAVAC = SumaUnidadEspecialJusticiaAVAC,
            };
            abonosUnidadEjecutora.Add(UnidadEspecialJusticia);



            decimal? SumaPoliciaBienestarSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPoliciaBienestarCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaPoliciaBienestarAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaPoliciaBienestarAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaPoliciaBienestarAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 22).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var PoliciaBienestar = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 22,
                TotalSolucionVivienda = SumaPoliciaBienestarSolucionVivienda,
                TotaAdministracionCesantias = SumaPoliciaBienestarCesantias,
                TotaAVAV = SumaPoliciaBienestarAVAV,
                TotaAVC = SumaPoliciaBienestarAVC,
                TotaAVAC = SumaPoliciaBienestarAVAC,
            };
            abonosUnidadEjecutora.Add(PoliciaBienestar);



            decimal? SumaVeteranosYRehabilitacionSolucionVivienda = solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_APORTES)
                                                           + solucionVivienda.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaVeteranosYRehabilitacionCesantias = administracionCesantias.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_CESANTIAS);
            decimal? SumaVeteranosYRehabilitacionAVAV = AVAV.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_AVAV);
            decimal? SumaVeteranosYRehabilitacionAVC = AVC.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_AVC);
            decimal? SumaVeteranosYRehabilitacionAVAC = AVAC.Where(x => x.UNIDAD_EJECUTORA == 24).Sum(x => x.INTERESES_CAUSADO_AVAC);
            var VeteranosYRehabilitacion = new InformacionAbonoInteresUnidadEjecutora
            {
                Id = 24,
                TotalSolucionVivienda = SumaVeteranosYRehabilitacionSolucionVivienda,
                TotaAdministracionCesantias = SumaVeteranosYRehabilitacionCesantias,
                TotaAVAV = SumaVeteranosYRehabilitacionAVAV,
                TotaAVC = SumaVeteranosYRehabilitacionAVC,
                TotaAVAC = SumaVeteranosYRehabilitacionAVAC,
            };

            abonosUnidadEjecutora.Add(VeteranosYRehabilitacion);



            return new Response<IEnumerable<InformacionAbonoInteresUnidadEjecutora>>
            {
                Data = abonosUnidadEjecutora,
                ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
            };

        }





        /// <summary>
        /// Metodo crear el periodo de verificacion de derechos al mismo tiempo que el ipc
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task<Response<CrearAbonosInteresDatos>> CrearAbonosInteresesDatos(IPCDto parametroIPC)
        {

            //await CrearPeriodoVerificacionDerecho(parametroIPC);
            //await CrearDatosCuentasAVResult(parametroIPC);
            //await CrearDatosCuentasAdministracionCesantiasResult(parametroIPC);




            //var ID = new VerificacionDerechosPeriodo
            //{
            //    Anio = parametroIPC.Anio,
            //    Mes = parametroIPC.Mes


            //};




            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterPeriodo(ID);
            //var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(ID);
            //var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(ID);


            //var info = new CrearAbonosInteresDatos
            //{
            //    resultVerificacionDerecho = solucionVivienda.ToList(),
            //    datosCuentasAVResult = AV.ToList(),
            //    datosCuentasAdministracionCesantiasResult = administracionCesantias.ToList(),


            //};


            //return new Response<CrearAbonosInteresDatos>
            //{
            //    Data = info,
            //    ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
            //};

            return null;

        }


        /// <summary>
        /// Metodo crear el periodo de verificacion de derechos al mismo tiempo que el ipc
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        public async Task EliminarActualizarIpc(IPCDto parametroIPC)
        {

            var ID = new VerificacionDerechosPeriodo
            {
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes


            };

            await _verificacionDerechosRepository.EliminarParamValoresInteresAbonados(ID);
            await _verificacionDerechosRepository.EliminarParamVerificacionDerechosResult(ID);
            await _verificacionDerechosRepository.EliminarVerificacionDerechosSolucionVivienda(ID);
            await _verificacionDerechosRepository.EliminarVerificacionCuentasAdministracionCesantias(ID);
            await _verificacionDerechosRepository.EliminarVerificacionCuentasAV(ID);




        }

        /// <summary>
        /// Metodo para obtener la verificacion de derechos por periodo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCDto" ></param>
        /// <returns></returns> 

        public async Task<Response<ParametrosVerificacionDerechoDto>> ObtenerTodoVerificacionDerechoByPeriodo(IPCDto parametroIPC)
        {

            var verificacionDerechosPeriodo = new VerificacionDerechosPeriodo
            {
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes


            };

            var responseData = this._mapper.Map<ParametrosVerificacionDerechoDto>(await _verificacionDerechosRepository.ObtenerTodoVerificacionDerechoByPeriodo(verificacionDerechosPeriodo));
            string codigoMensaje = ResponseMessageConstants.CodigoMensaje11;
            MensajeDto mensaje = await ObtenerMensaje(codigoMensaje);

            return new Response<ParametrosVerificacionDerechoDto>
            {
                Data = responseData,
                ReturnMessage = mensaje.Mensaje
            };
        }



        /// <summary>
        /// Metodo para obtener los valores de interes abonados por periodo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IPCDto" ></param>
        /// <returns></returns>

        public async Task<Response<ParametrosAbonosInteresDto>> ObtenerTodoValoresInteresAbonadosByPeriodo(IPCDto parametroIPC)
        {

            var verificacionDerechosPeriodo = new VerificacionDerechosPeriodo
            {
                Anio = parametroIPC.Anio,
                Mes = parametroIPC.Mes
            };

            //var responseData = await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(verificacionDerechosPeriodo);
            var responseData = this._mapper.Map<ParametrosAbonosInteresDto>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(verificacionDerechosPeriodo));

            return new Response<ParametrosAbonosInteresDto>
            {
                Data = responseData,
                ReturnMessage = "Parametro por el perido traido con exito"
            };
        }


        /// <summary>
        /// Metodo para obtener los valores dew los intereses no causados por numero de identificacion
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="IDENTIFICACION" ></param>
        /// <returns></returns>

        public async Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacion(int IDENTIFICACION)
        {


            List<InteresesCausadosNoCausadosInfoDto> intereses = new List<InteresesCausadosNoCausadosInfoDto>();


            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterIdentificacion(IDENTIFICACION);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterIdentificacion(IDENTIFICACION);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterIdentificacion(IDENTIFICACION);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterIdentificacion(IDENTIFICACION);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterIdentificacion(IDENTIFICACION);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterIdentificacion(IDENTIFICACION);


            foreach (var item in AVAC)
            {


                if (item.INTERESES_NO_CAUSADO_AVAC != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVAC",
                        Valor = item.INTERESES_NO_CAUSADO_AVAC,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


            }


            foreach (var item in AVAV)
            {


                if (item.INTERESES_NO_CAUSADO_AVAV != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVAV",
                        Valor = item.INTERESES_NO_CAUSADO_AVAV,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


            }

            foreach (var item in AVC)
            {


                if (item.INTERESES_NO_CAUSADO_AVC != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVC",
                        Valor = item.INTERESES_NO_CAUSADO_AVC,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


            }


            foreach (var item in solucionVivienda)
            {


                //if (item.INTERESES_CAUSADO_APORTES != null && item.INTERESES_CAUSADO_APORTES > 0)
                //{

                //    var list = new InteresesCausadosNoCausadosInfoDto
                //    {

                //        TipoSolicitud = null,
                //        TipoConcepto = "Intereses Solición Vivienda Aportes",
                //        Valor = (decimal)item.INTERESES_CAUSADO_APORTES,
                //        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                //        SolicitudModo = "Pago",
                //        TipoInteres = "Causado",
                //        FechaSolicitud = DateTime.Now

                //    };
                //    intereses.Add(list);

                //}

                if (item.INTERESES_NO_CAUSADO_APORTES != null && item.INTERESES_NO_CAUSADO_APORTES != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Solición Vivienda Aportes",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_APORTES,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


                //if (item.INTERESES_CAUSADO_CESANTIAS != null && item.INTERESES_CAUSADO_CESANTIAS > 0)
                //{

                //    var list = new InteresesCausadosNoCausadosInfoDto
                //    {

                //        TipoSolicitud = null,
                //        TipoConcepto = "Intereses Solición Vivienda Cesantias",
                //        Valor = (decimal)item.INTERESES_CAUSADO_CESANTIAS,
                //        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                //        SolicitudModo = "Pago",
                //        TipoInteres = "Causado",
                //        FechaSolicitud = DateTime.Now

                //    };
                //    intereses.Add(list);

                //}

                if (item.INTERESES_NO_CAUSADO_CESANTIAS != null && item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Solición Vivienda Cesantias",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }





            }



            foreach (var item in administracionCesantias)
            {


                //if (item.INTERESES_CAUSADO_CESANTIAS > 0)
                //{

                //    var list = new InteresesCausadosNoCausadosInfoDto
                //    {

                //        TipoSolicitud = null,
                //        TipoConcepto = "Intereses Administración Cesantias",
                //        Valor = (decimal)item.INTERESES_CAUSADO_CESANTIAS,
                //        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                //        SolicitudModo = "Pago",
                //        TipoInteres = "Causado",
                //        FechaSolicitud = DateTime.Now

                //    };
                //    intereses.Add(list);

                //}

                if (item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Administración Cesantias",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


            }


            return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
            {
                Data = intereses,
                ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
            };
        }


        /// <summary>
        /// Metodo el cual obtiene las cuentas por periodo y aplica los movimientos en las cuentas y conceptos
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns>

        public async Task<Response<ParametrosAbonosInteresDto>> AplicarMovimientosInteresesAbonados(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {

            var listaMovimientos = new List<Domain.Entities.Movimiento>();

            var responseData = this._mapper.Map<ParametrosAbonosInteresDto>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(verificacionDerechosPeriodo));

            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterPeriodo(verificacionDerechosPeriodo);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterPeriodo(verificacionDerechosPeriodo);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(verificacionDerechosPeriodo);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterPeriodo(verificacionDerechosPeriodo);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(verificacionDerechosPeriodo);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(verificacionDerechosPeriodo);


            foreach (var item in administracionCesantias)
            {
                if (item.INTERESES_CAUSADO_CESANTIAS > 0)
                {
                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 10,
                        MVT_VALOR = item.INTERESES_CAUSADO_CESANTIAS,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };

                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }


                }



            }


            foreach (var item in AVAC)
            {

                if (item.INTERESES_CAUSADO_AVAC > 0)
                {

                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 24,
                        MVT_VALOR = item.INTERESES_CAUSADO_AVAC,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };
                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }

                }




            }

            foreach (var item in AVAV)
            {

                if (item.INTERESES_CAUSADO_AVAV > 0)
                {

                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 26,
                        MVT_VALOR = item.INTERESES_CAUSADO_AVAV,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };
                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }

                }




            }


            foreach (var item in AVC)
            {

                if (item.INTERESES_CAUSADO_AVC > 0)
                {

                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 26,
                        MVT_VALOR = item.INTERESES_CAUSADO_AVC,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };
                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }

                }




            }



            foreach (var item in solucionVivienda)
            {

                if (item.INTERESES_CAUSADO_APORTES > 0)
                {

                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 9,
                        MVT_VALOR = item.INTERESES_CAUSADO_APORTES ?? 0,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };

                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }

                }

                if (item.INTERESES_CAUSADO_CESANTIAS > 0)
                {

                    var movimiento = new Domain.Entities.Movimiento
                    {
                        CTA_ID = item.CTA_ID,
                        CNC_ID = 10,
                        MVT_VALOR = item.INTERESES_CAUSADO_CESANTIAS ?? 0,
                        CAT_TIPO_MOVIMIENTO = "C"


                    };
                    try
                    {
                        await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                    }
                    catch (Exception)
                    {

                        listaMovimientos.Add(movimiento);
                    }

                }


            }



            Console.WriteLine("los movimientos con error son" + listaMovimientos);





            return new Response<ParametrosAbonosInteresDto>
            {
                Data = responseData,
                ReturnMessage = "Movimientos aplicados con exito para el periodo"
            };
        }



        /// <summary>
        /// Metodo el cual obtiene las cuentas por periodo y aplica los movimientos en las cuentas y conceptos cuando el IPC es negativo
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="VerificacionDerechosPeriodo" ></param>
        /// <returns></returns>

        public async Task<Response<ParametrosAbonosInteresDto>> AplicarMovimientosInteresesAbonadosIPCNegativo(VerificacionDerechosPeriodo verificacionDerechosPeriodo)
        {
            var listaMovimientos = new List<Domain.Entities.Movimiento>();

            var responseData = this._mapper.Map<ParametrosAbonosInteresDto>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(verificacionDerechosPeriodo));

            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterPeriodo(verificacionDerechosPeriodo);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterPeriodo(verificacionDerechosPeriodo);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterPeriodo(verificacionDerechosPeriodo);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterPeriodo(verificacionDerechosPeriodo);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterPeriodo(verificacionDerechosPeriodo);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterPeriodo(verificacionDerechosPeriodo);


            foreach (var item in administracionCesantias)
            {
                if (item.INTERESES_CAUSADO_CESANTIAS != 0)
                {

                    decimal resultado = item.INTERES_CESANTIAS + item.INTERESES_CAUSADO_CESANTIAS;

                    if (resultado >= 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 10,
                            MVT_VALOR = item.INTERESES_CAUSADO_CESANTIAS,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        // await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }
                    }

                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 10,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }
                    }


                }
            }




            foreach (var item in AVAC)
            {
                if (item.INTERESES_CAUSADO_AVAC != 0)
                {

                    decimal resultado = item.INTERES_AVAC + item.INTERESES_CAUSADO_AVAC;

                    if (resultado >= 0)
                    {
                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 24,
                            MVT_VALOR = item.INTERESES_CAUSADO_AVAC,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };
                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }

                    }
                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 24,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }


                    }

                }

            }

            foreach (var item in AVAV)
            {
                if (item.INTERESES_CAUSADO_AVAV != 0)
                {
                    decimal resultado = item.INTERES_AVAV + item.INTERESES_CAUSADO_AVAV;

                    if (resultado >= 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 26,
                            MVT_VALOR = item.INTERESES_CAUSADO_AVAV,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);

                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }


                    }
                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 26,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }


                    }

                }


            }


            foreach (var item in AVC)
            {
                if (item.INTERESES_CAUSADO_AVC != 0)
                {

                    decimal resultado = item.INTERES_AVC + item.INTERESES_CAUSADO_AVC;

                    if (resultado >= 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 29,
                            MVT_VALOR = item.INTERESES_CAUSADO_AVC,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }
                    }

                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 29,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }
                    }
                }

            }



            foreach (var item in solucionVivienda)
            {

                if (item.INTERESES_CAUSADO_APORTES != 0)
                {

                    decimal resultado = item.INTERES_APORTES + item.INTERESES_CAUSADO_APORTES ?? 0;

                    if (resultado >= 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 9,
                            MVT_VALOR = item.INTERESES_CAUSADO_APORTES ?? 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }
                    }

                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 9,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        // await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }

                    }

                }

                if (item.INTERESES_CAUSADO_CESANTIAS != 0)
                {

                    decimal resultado = item.INTERES_CESANTIAS + item.INTERESES_CAUSADO_CESANTIAS ?? 0;

                    if (resultado >= 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 10,
                            MVT_VALOR = item.INTERESES_CAUSADO_CESANTIAS ?? 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);

                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }

                    }
                    else if (resultado < 0)
                    {

                        var movimiento = new Domain.Entities.Movimiento
                        {
                            CTA_ID = item.CTA_ID,
                            CNC_ID = 10,
                            MVT_VALOR = 0,
                            CAT_TIPO_MOVIMIENTO = "D"


                        };

                        //await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);

                        try
                        {
                            await _iClienteRequestProvider.InsertarMovimientoAbonosIntereses(movimiento);
                        }
                        catch (Exception)
                        {

                            listaMovimientos.Add(movimiento);
                        }

                    }

                }


            }



            Console.WriteLine("los movimientos con error son" + listaMovimientos);





            return new Response<ParametrosAbonosInteresDto>
            {
                Data = responseData,
                ReturnMessage = "Movimientos aplicados con exito para el periodo"
            };
        }



        /// <summary>
        /// Metodo el cual obtiene los intereses no causados filtrados tanto por identificacion como por el año
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="InteresNoCausadoDto" ></param>
        /// <returns></returns>

        public async Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacionYAnio(InteresNoCausadoDto request)
        {


            List<InteresesCausadosNoCausadosInfoDto> intereses = new List<InteresesCausadosNoCausadosInfoDto>();


            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterIdentificacion(request.UsuarioId);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterIdentificacion(request.UsuarioId);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterIdentificacion(request.UsuarioId);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterIdentificacion(request.UsuarioId);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterIdentificacion(request.UsuarioId);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterIdentificacion(request.UsuarioId);

            //var AVAnio = AV.Where(a => a.PERIODO_ANIO == request.Anio);

            var AVACAnio = AVAC.Where(a => a.PERIODO_ANIO == request.Anio);
            var AVAVAnio = AVAV.Where(a => a.PERIODO_ANIO == request.Anio);
            var AVCAnio = AVC.Where(a => a.PERIODO_ANIO == request.Anio);

            var solucionViviendaAnio = solucionVivienda.Where(a => a.PERIODO_ANIO == request.Anio);
            var administracionAnio = administracionCesantias.Where(a => a.PERIODO_ANIO == request.Anio);


            foreach (var item in AVACAnio)
            {

                if (item.INTERESES_NO_CAUSADO_AVAC != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVAC",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_AVAC,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }

            }


            foreach (var item in AVAVAnio)
            {

                if (item.INTERESES_NO_CAUSADO_AVAV != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVAV",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_AVAV,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }

            }

            foreach (var item in AVCAnio)
            {

                if (item.INTERESES_NO_CAUSADO_AVC != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {
                        NumeroSolicitud = item.NUMERO_SOLICITUD,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses AVC",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_AVC,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }

            }



            foreach (var item in solucionViviendaAnio)
            {


                if (item.INTERESES_NO_CAUSADO_APORTES != null && item.INTERESES_NO_CAUSADO_APORTES != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Solición Vivienda Aportes",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_APORTES,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }

                if (item.INTERESES_NO_CAUSADO_CESANTIAS != null && item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Solición Vivienda Cesantias",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }





            }



            foreach (var item in administracionAnio)
            {

                if (item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                {

                    var list = new InteresesCausadosNoCausadosInfoDto
                    {

                        NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                        TipoSolicitud = item.TIPO_SOLICITUD,
                        TipoConcepto = "Intereses Administración Cesantias",
                        Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                        Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                        SolicitudModo = "Pago",
                        TipoInteres = "No Causado",
                        FechaSolicitud = DateTime.Now

                    };

                    intereses.Add(list);

                }


            }


            return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
            {
                Data = intereses,
                ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
            };
        }



        /// <summary>
        /// Metodo el cual obtiene los intereses no causados filtrados tanto por identificacion como por el año y el concepto
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="InteresNoCausadoDto" ></param>
        /// <returns></returns>

        public async Task<Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>> ObtenerTodoInteresesNoCausadosByIdentificacionYAnioYConcepto(InteresNoCausadoDto request)
        {


            List<InteresesCausadosNoCausadosInfoDto> intereses = new List<InteresesCausadosNoCausadosInfoDto>();


            //var AV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVResultFilterIdentificacion(request.UsuarioId);

            var AVAC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVACResultFilterIdentificacion(request.UsuarioId);
            var AVAV = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVAVResultFilterIdentificacion(request.UsuarioId);
            var AVC = await _verificacionDerechosRepository.ObtenerTodoDatosCuentasAVCResultFilterIdentificacion(request.UsuarioId);

            var solucionVivienda = await _verificacionDerechosRepository.ObtenerTodoCuentasSolucionViviendaResultFilterIdentificacion(request.UsuarioId);
            var administracionCesantias = await _verificacionDerechosRepository.ObtenerTodoCuentasAdministracionCesantiasResultFilterIdentificacion(request.UsuarioId);

            //var AVAnio = AV.Where(a => a.PERIODO_ANIO == request.Anio);

            var AVACAnio = AVAC.Where(a => a.PERIODO_ANIO == request.Anio);
            var AVAVAnio = AVAV.Where(a => a.PERIODO_ANIO == request.Anio);
            var AVCAnio = AVC.Where(a => a.PERIODO_ANIO == request.Anio);

            var solucionViviendaAnio = solucionVivienda.Where(a => a.PERIODO_ANIO == request.Anio);
            var administracionAnio = administracionCesantias.Where(a => a.PERIODO_ANIO == request.Anio);


            if (request.Concepto == 24)
            {
                foreach (var item in AVACAnio)
                {
                    if (item.VALOR_AVAC > 0)
                    {


                        if (item.INTERESES_NO_CAUSADO_AVAC != 0)
                        {

                            var list = new InteresesCausadosNoCausadosInfoDto
                            {
                                NumeroSolicitud = item.NUMERO_SOLICITUD,
                                TipoSolicitud = item.TIPO_SOLICITUD,
                                TipoConcepto = "Intereses AVAC",
                                Valor = (decimal)item.INTERESES_NO_CAUSADO_AVAC,
                                Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                                SolicitudModo = "Pago",
                                TipoInteres = "No Causado",
                                FechaSolicitud = DateTime.Now

                            };

                            intereses.Add(list);

                        }
                    }

                }

                return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                {
                    Data = intereses,
                    ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                };

            }


            if (request.Concepto == 26)
            {
                foreach (var item in AVAVAnio)
                {
                    if (item.VALOR_AVAV > 0)
                    {


                        if (item.INTERESES_NO_CAUSADO_AVAV != 0)
                        {

                            var list = new InteresesCausadosNoCausadosInfoDto
                            {
                                NumeroSolicitud = item.NUMERO_SOLICITUD,
                                TipoSolicitud = item.TIPO_SOLICITUD,
                                TipoConcepto = "Intereses AVAV",
                                Valor = (decimal)item.INTERESES_NO_CAUSADO_AVAV,
                                Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                                SolicitudModo = "Pago",
                                TipoInteres = "No Causado",
                                FechaSolicitud = DateTime.Now

                            };

                            intereses.Add(list);

                        }
                    }

                }

                return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                {
                    Data = intereses,
                    ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                };

            }

            if (request.Concepto == 29)
            {
                foreach (var item in AVCAnio)
                {
                    if (item.VALOR_AVC > 0)
                    {


                        if (item.INTERESES_NO_CAUSADO_AVC != 0)
                        {

                            var list = new InteresesCausadosNoCausadosInfoDto
                            {
                                NumeroSolicitud = item.NUMERO_SOLICITUD,
                                TipoSolicitud = item.TIPO_SOLICITUD,
                                TipoConcepto = "Intereses AVC",
                                Valor = (decimal)item.INTERESES_NO_CAUSADO_AVC,
                                Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                                SolicitudModo = "Pago",
                                TipoInteres = "No Causado",
                                FechaSolicitud = DateTime.Now

                            };

                            intereses.Add(list);

                        }
                    }

                }

                return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                {
                    Data = intereses,
                    ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                };

            }


            if (request.Concepto == 10 && request.TipoCuentaId == 2)
            {
                foreach (var item in administracionAnio)
                {

                    if (item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                    {

                        var list = new InteresesCausadosNoCausadosInfoDto
                        {
                            NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                            TipoSolicitud = item.TIPO_SOLICITUD,
                            TipoConcepto = "Intereses Administracion Cesantias",
                            Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                            Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                            SolicitudModo = "Pago",
                            TipoInteres = "No Causado",
                            FechaSolicitud = DateTime.Now

                        };

                        intereses.Add(list);

                    }


                }

                return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                {
                    Data = intereses,
                    ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                };

            }


            if (request.TipoCuentaId == 1)
            {
                if (request.Concepto == 9)
                {
                    foreach (var item in solucionViviendaAnio)
                    {
                        if (item.INTERESES_NO_CAUSADO_APORTES != null && item.INTERESES_NO_CAUSADO_APORTES != 0)
                        {

                            var list = new InteresesCausadosNoCausadosInfoDto
                            {
                                NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                                TipoSolicitud = item.TIPO_SOLICITUD,
                                TipoConcepto = "Intereses Solición Vivienda Aportes",
                                Valor = (decimal)item.INTERESES_NO_CAUSADO_APORTES,
                                Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                                SolicitudModo = "Pago",
                                TipoInteres = "No Causado",
                                FechaSolicitud = DateTime.Now

                            };
                            intereses.Add(list);
                        }
                    }

                    return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                    {
                        Data = intereses,
                        ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                    };

                }

                if (request.Concepto == 10)
                {
                    foreach (var item in solucionViviendaAnio)
                    {
                        if (item.INTERESES_NO_CAUSADO_CESANTIAS != null && item.INTERESES_NO_CAUSADO_CESANTIAS != 0)
                        {

                            var list = new InteresesCausadosNoCausadosInfoDto
                            {
                                NumeroSolicitud = item.NUMERO_SOLICITUD ?? 0,
                                TipoSolicitud = item.TIPO_SOLICITUD,
                                TipoConcepto = "Intereses Solición Vivienda Cesantias",
                                Valor = (decimal)item.INTERESES_NO_CAUSADO_CESANTIAS,
                                Periodo = item.PERIODO_ANIO + "/" + item.PERIODO_MES,
                                SolicitudModo = "Pago",
                                TipoInteres = "No Causado",
                                FechaSolicitud = DateTime.Now

                            };
                            intereses.Add(list);
                        }
                    }

                    return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
                    {
                        Data = intereses,
                        ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
                    };

                }

            }

            return new Response<IEnumerable<InteresesCausadosNoCausadosInfoDto>>
            {
                Data = intereses,
                ReturnMessage = "Datos de los abonos de interes por unidad ejecutora con exito"
            };


        }

        private async Task<ObtenerBuaResponse> ObtenerClienteIntegracionGA2(int identificationType, string identificationNumber)
        {
            dynamic responseIntegracion = default;

            //#if DEBUG

            //_logger.LogInformation("Pasa por el debug");
            //responseIntegracion = await _buaIntegraciones.ObtenerBua(new()
            //{
            //    identificacion = identificationNumber,
            //    tipo_Id = identificationType
            //});

            //responseIntegracion = JsonConvert.DeserializeObject<Response<ObtenerBuaResponse>>(responseIntegracion.Data).Data;
            //#else
            _logger.LogInformation("pasa por produccuin");
            responseIntegracion = await _servicioBua.ObtenerAfiliadoBuaCH(new()
            {
                identificacion = identificationNumber,
                tipo_Id = identificationType,
            });

            //#endif


            if (responseIntegracion is null) { return null; }
            ObtenerBuaResponse response = new();
            string responseJson = JsonConvert.SerializeObject(responseIntegracion.Data);
            response.ObtenerBUAResult = JsonConvert.DeserializeObject<ClienteIntegracionGA2DTO>(responseJson);
            if (response.ObtenerBUAResult != null)
            {
                PrepararClienteIntegracion(response);
                response.ObtenerBUAResult.Payload = JsonConvert.SerializeObject(responseIntegracion);
            }
            return response;
        }

        /// <summary>
        /// Metodo para actrualizar el abono de interes al aprobar
        /// </summary>
        /// <author>Julian Nova</author>
        /// <date>03/05/2023</date>
        /// <param name="InteresNoCausadoDto" ></param>
        /// <returns></returns>



        public async Task<Response<ParametrosAbonosInteresDto>> ActualizarParametrosInteresAbonadosMovimientos(ParametrosAbonosInteresDto request)
        {

            var info = new VerificacionDerechosPeriodo
            {
                Anio = request.Anio,
                Mes = request.Mes

            };

            var Existe = await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(info);

            ////var Existe = this._mapper.Map<ParametrosAbonosInteresDto>(await _verificacionDerechosRepository.ObtenerTodoValoresInteresAbonadosByPeriodo(info));



            if (Existe.ESTADO == 1)
            {

                /**
            * Actualizar solicitud tarea
            */

                ActualizarSolicitudTarea actualizarSolicitudTareaProcesada = new ActualizarSolicitudTarea()
                {
                    SLT_ID = Existe.SLT_ID,
                    SLT_ESTADO_SOLICITUD = 328,
                    SLT_OBSERVACION = "Abonos de Interes Procesada"
                };

                await _verificacionDerechosRepository.ActualizarSolicitudTareaVerificacionDerechos(actualizarSolicitudTareaProcesada);


                /**
                * Crear tarea Abonos de Interes pendiente por procesar
                */
                var FLJ_ID_INTERES_APROBAR = Guid.NewGuid();
                //var CodigoPerfilTareaProcesar = "USER_SUPER_ADMIN";
                var CodigoPerfilTareaProcesar = "AOPER_J";


                var USR_IDTareaProcesar = await _verificacionDerechosRepository.ObtenerUsuarioVerificacionDerechos(CodigoPerfilTareaProcesar);


                //var verificacionInteresAbonadoTareaProcesar = new VerificacionDerechoSolicitudTarea
                //{
                //    SLT_ID = FLJ_ID_INTERES_APROBAR,
                //    TRA_ID = Guid.Parse("AE1DEA65-4817-4AC3-9CEF-E841D76F91C4"),
                //    SOL_ID = Guid.Parse("526882C0-B260-4CD4-9788-36A63970970E"),
                //    USR_ID = (Guid)request.UsuarioId,
                //    OBSERVACION = "Abonos de Interes pendiente por aprobar"


                //};
                //await _verificacionDerechosRepository.CrearVerificacionDerechoSolicitudTarea(verificacionInteresAbonadoTareaProcesar);

                /**
                * Crear notificacion tarea Abonos de Interes pendiente por procesar
                 */

                var tareaNotificacionProcesarInteresAbonados = new NotificacionTarea
                {
                    Usuario = USR_IDTareaProcesar.USR_ID,
                    Notificacion = "Abono de Intereses en la etapa de Pendiente por aprobar, para su aprobación con Numero de radicado: " + request.NumeroRadicado + " correspondiente al periodo " + request.Anio + "/" + request.Mes,
                    Modulo = "Abono de intereses",
                    TareaSiguiente = FLJ_ID_INTERES_APROBAR,
                    NumRadicado = Existe.NUMERO_RADICADO,
                    NUM_CEDULA = USR_IDTareaProcesar.NUM_CEDULA,
                    UsuarioEnvia = (Guid)request.UsuarioId,


                };

                await _verificacionDerechosRepository.InsertarNotificacionTarea(tareaNotificacionProcesarInteresAbonados);

                await _verificacionDerechosRepository.ActualizarNotificacionTareaVerificacionYAbonos(FLJ_ID_INTERES_APROBAR, "/AbonoIntereses");

                request.Aprobado = null;
                request.Estado = 2;
                request.NombreEstado = "Pendiente por aprobar";
                request.SltIdAprobar = FLJ_ID_INTERES_APROBAR;
                request.UsuarioId = USR_IDTareaProcesar.USR_ID;
                request.UsuarioNombre = USR_IDTareaProcesar.USR_NOMBRE + " " + USR_IDTareaProcesar.USR_APELLIDO;

                var actualizarParametrosVerificacionDerechosPorAsignacion = this._mapper.Map<ParametrosAbonosInteresDto>(await this._verificacionDerechosRepository.ActualizarParametrosInteresAbonadosMovimientos(
                                                                            this._mapper.Map<ParametrosAbonoInteres>(request)));



                return new Response<ParametrosAbonosInteresDto>
                {
                    Data = actualizarParametrosVerificacionDerechosPorAsignacion,
                    ReturnMessage = "Verificacion Procesada con exito"
                };
            }


            /**
           * Actualizar solicitud tarea
           */

            ActualizarSolicitudTarea actualizarSolicitudTareaAprobada = new ActualizarSolicitudTarea()
            {
                SLT_ID = Existe.SLT_ID_APROBAR,
                SLT_ESTADO_SOLICITUD = 328,
                SLT_OBSERVACION = "Abonos de Interes Aprobada"
            };

            await _verificacionDerechosRepository.ActualizarSolicitudTareaVerificacionDerechos(actualizarSolicitudTareaAprobada);



            request.Estado = 3;
            request.NombreEstado = "Aprobado";
            request.SltId = Existe.SLT_ID;
            request.SltIdAprobar = Existe.SLT_ID_APROBAR;

            var responseData = this._mapper.Map<ParametrosAbonosInteresDto>(await this._verificacionDerechosRepository.ActualizarParametrosInteresAbonadosMovimientos(this._mapper.Map<ParametrosAbonoInteres>(request)));


            if (responseData.ValorIpc > 0)
            {
                await AplicarMovimientosInteresesAbonados(info);
            }
            if (responseData.ValorIpc < 0)
            {
                await AplicarMovimientosInteresesAbonadosIPCNegativo(info);
            }


            return new Response<ParametrosAbonosInteresDto>
            {
                Data = responseData,
                ReturnMessage = "Verificacion Traida con exito"
            };

        }

        public ObtenerBuaResponse PrepararClienteIntegracion(ObtenerBuaResponse clienteIntegracionGA2)
        {
            if (clienteIntegracionGA2 != null)
            {
                if (clienteIntegracionGA2.ObtenerBUAResult != null)
                {
                    if (clienteIntegracionGA2.ObtenerBUAResult.Direccion != null)
                    {
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TelefonoMostrar = clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "" && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "0" ?
                                                                            clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular : "601" + clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TipoTelefono = clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "" && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "0" ?
                                                                            2 : clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono != "" ? 1
                                                                            : 0;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.Correo = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != "" ?
                                                    clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional : clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TipoCorreo = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != "" ?
                                                                            2 : clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal != "" ? 1
                                                                            : 0;
                        if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }

                        if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }

                        if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }
                    }
                }
            }

            return clienteIntegracionGA2;
        }


        /// <summary>
        /// Preparar la informacion de cuenta de BUA para la liquidacion de interes de cuenta Administracion de cesantidas
        /// </summary>
        /// <param name="parametroIPC"></param>
        /// <returns></returns>
        /// 
        public async Task SincronizacionCuentasAV(IPCDto parametroIPC)
        {
            try
            {
                DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
                DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



                var filtro = new VerificacionDerechosByFechaDto
                {
                    FechaInicial = primerDia,
                    FechaFinal = ultimoDia,

                };

                var lista = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAV(filtro);
                if (lista.Data.Count() > 0)
                {
                    var ipcMap = _mapper.Map<IPC>(parametroIPC);
                    var cuentasMapp = _mapper.Map<IEnumerable<CuentaTemp>>(lista.Data);
                    var responseInsert = await _verificacionDerechosRepository.ICT_InsertarCuentaTemporal(cuentasMapp.ToList());
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }
        public async Task SincronizacionCuentasAdmiCesantias(IPCDto parametroIPC)
        {
            var lista = await _iCierreMensualRequestProvider.ObtenerCedulaCuentaCesantias();
            lista.Data = lista.Data;
            var listMap = _mapper.Map<IEnumerable<DatosCedulasCesantias>>(lista.Data);

            await _verificacionDerechosRepository.InsertarDatosCedulasCesantias(listMap.ToList(), parametroIPC.ValorIPC, parametroIPC.Mes, parametroIPC.Anio);
            foreach (var item in lista.Data)
            {
                var requet = new ObtenerInfoCedulaRequestDto
                {
                    CuentaId = item.CuentaId,
                    Periodo = parametroIPC.Anio
                };
                var dataCuentaResponse = await _iCierreMensualRequestProvider.ObtenerInfoCuentaByPeriodo(requet);
                var dataCuenta = dataCuentaResponse.Data;
                var dataCuentaMap = _mapper.Map<DatosInfoCensatiaByCuentaPerioso>(dataCuenta);
                await _verificacionDerechosRepository.InsertarDatosInfoCesantia(dataCuentaMap, item.CuentaId);
            }
        }
        /// <returns></returns>
        //public async Task SincronizacionCuentasAdmiVivienda(IPCDto parametroIPC)
        //{
        //    var lista = await _iCierreMensualRequestProvider.Vi();
        //    lista.Data = lista.Data;
        //    var listMap = _mapper.Map<IEnumerable<DatosCedulasCesantias>>(lista.Data);

        //    await _verificacionDerechosRepository.InsertarDatosCedulasCesantias(listMap.ToList(), parametroIPC.ValorIPC, parametroIPC.Mes, parametroIPC.Anio);
        //    foreach (var item in lista.Data)
        //    {
        //        var requet = new ObtenerInfoCedulaRequestDto
        //        {
        //            CuentaId = item.CuentaId,
        //            Periodo = parametroIPC.Anio
        //        };
        //        var dataCuentaResponse = await _iCierreMensualRequestProvider.ObtenerInfoCuentaByPeriodo(requet);
        //        var dataCuenta = dataCuentaResponse.Data;
        //        var dataCuentaMap = _mapper.Map<DatosInfoCensatiaByCuentaPerioso>(dataCuenta);
        //        await _verificacionDerechosRepository.InsertarDatosInfoCesantia(dataCuentaMap, item.CuentaId);
        //    }
        //}

        public async Task SincronizacionCuentaAdmiVivienda(IPCDto parametroIPC)
        {
            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);
            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,
            };
            var lista = await _iCierreMensualRequestProvider.ObtenerTodoDatosVerificacionDerechos(filtro);

            if (lista.Data.ToList().Count > 0)
            {
                await _verificacionDerechosRepository.InsertarDatosVerificacionDerechoCuenta(lista.Data.ToList());
            }
        }


        public async Task SincronizacionCuentaAVAC(IPCDto parametroIPC)
        {
            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };
            var lista = await _iCierreMensualRequestProvider.ObtenerDatosCuentasAVAC(filtro);
            lista.Data = lista.Data;
            var mapData = _mapper.Map<List<ObtenerCuentasAVAC>>(lista.Data);
            await _verificacionDerechosRepository.Insertar_ObtenerCuentasAVAC(mapData);

            try
            {
                foreach (var item in lista.Data)
                {
                    var dataClienteResponse = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.Identificacion);
                    var dataCliente = dataClienteResponse.Data;
                    var data = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(dataCliente.IdTipoIdentificacion, item.Identificacion);
                    var dataInformacionCliente = data.Data;

                    var dataCuentas = dataInformacionCliente.Cuentas;

                    var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("06")).ToList();
                    var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                    if (dataInfoCuenta.Count > 0)
                    {
                        await _verificacionDerechosRepository.InsertarTempCliente(dataInformacionCliente);
                        await _verificacionDerechosRepository.InsertarTempCuentaCliente(dataInfoCuenta);
                        var dataMovimiento = dataInfoCuenta.SelectMany(x => x.Movimientos).ToList();
                        var dataConceptos = dataInfoCuenta.SelectMany(x => x.Conceptos).ToList();

                        await _verificacionDerechosRepository.InsertarDatosConcetoCuenta(dataConceptos);
                        await _verificacionDerechosRepository.InsertarTempMovimiento(dataMovimiento);


                        var dataMovimientoConcepto = dataConceptos?.Where(x => x?.Movimientos != null)
                            .SelectMany(x => x.Movimientos).ToList() ?? new List<MovimientosCuentaDto>();
                        if (dataMovimientoConcepto != null && dataMovimientoConcepto.Count > 0)
                        {
                            await _verificacionDerechosRepository.InsertarDatosMovimientoConcetoCuenta(dataMovimientoConcepto);
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task SincronizacionCuentaAVAV(IPCDto parametroIPC)
        {
            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);



            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };
            var lista = await _iCierreMensualRequestProvider.ObtemerCedulaCuentaAVA();
            try
            {

                var mapData = _mapper.Map<List<ObtenerCuentaAVAVResponse>>(lista.Data);
                await _verificacionDerechosRepository.SP_TOCAVAV_InsertObtenerCuentaAVAV(mapData);
            }
            catch (Exception e)
            {
            }

            foreach (var item in lista.Data)
            {
                var dataClienteResponse = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.Identificacion);
                var dataCliente = dataClienteResponse.Data;
                var data = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(dataCliente.IdTipoIdentificacion, item.Identificacion);
                var dataInformacionCliente = data.Data;

                var dataCuentas = dataInformacionCliente.Cuentas;

                var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("07")).ToList();
                var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                if (dataInfoCuenta.Count > 0)
                {
                    await _verificacionDerechosRepository.InsertarTempCliente(dataInformacionCliente);
                    await _verificacionDerechosRepository.InsertarTempCuentaCliente(dataInfoCuenta);
                    var dataMovimiento = dataInfoCuenta.SelectMany(x => x.Movimientos).ToList();
                    var dataConceptos = dataInfoCuenta.SelectMany(x => x.Conceptos).ToList();

                    await _verificacionDerechosRepository.InsertarDatosConcetoCuenta(dataConceptos);
                    await _verificacionDerechosRepository.InsertarTempMovimiento(dataMovimiento);


                    var dataMovimientoConcepto = dataConceptos?.Where(x => x?.Movimientos != null)
                        .SelectMany(x => x.Movimientos).ToList() ?? new List<MovimientosCuentaDto>();
                    if (dataMovimientoConcepto != null && dataMovimientoConcepto.Count > 0)
                    {
                        await _verificacionDerechosRepository.InsertarDatosMovimientoConcetoCuenta(dataMovimientoConcepto);
                    }
                }
            }
        }
        public async Task SincronizacionCuentaAVC(IPCDto parametroIPC)
        {
            DateTime primerDia = new DateTime(parametroIPC.Anio, parametroIPC.Mes, 1);
            DateTime ultimoDia = primerDia.AddMonths(1).AddDays(-1);


            var filtro = new VerificacionDerechosByFechaDto
            {
                FechaInicial = primerDia,
                FechaFinal = ultimoDia,

            };

            var lista = await _iCierreMensualRequestProvider.ObtemerCedulaCuentaAVC();

            lista.Data = lista.Data;
            var mapData = _mapper.Map<List<ObtenerCuentaAVAVResponse>>(lista.Data);
            await _verificacionDerechosRepository.InsertarTOCAVC_Temp_ObtenerCuentaAVC(mapData);

            foreach (var item in lista.Data)
            {
                var dataClienteResponse = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.Identificacion);
                var dataCliente = dataClienteResponse.Data;
                var data = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(dataCliente.IdTipoIdentificacion, item.Identificacion);
                var dataInformacionCliente = data.Data;

                var dataCuentas = dataInformacionCliente.Cuentas;

                var dataInfoCuenta = dataCuentas.Where(x => x.SaldoDisponible > 0 && x.NumeroCuentaCompleta.StartsWith("05")).ToList();
                var dtaMovimiento = dataInfoCuenta.Select(x => x.Movimientos).ToList();
                if (dataInfoCuenta.Count > 0)
                {
                    await _verificacionDerechosRepository.InsertarTempCliente(dataInformacionCliente);
                    await _verificacionDerechosRepository.InsertarTempCuentaCliente(dataInfoCuenta);
                    var dataMovimiento = dataInfoCuenta.SelectMany(x => x.Movimientos).ToList();
                    var dataConceptos = dataInfoCuenta.SelectMany(x => x.Conceptos).ToList();

                    await _verificacionDerechosRepository.InsertarDatosConcetoCuenta(dataConceptos);
                    await _verificacionDerechosRepository.InsertarTempMovimiento(dataMovimiento);


                    var dataMovimientoConcepto = dataConceptos?.Where(x => x?.Movimientos != null)
                        .SelectMany(x => x.Movimientos).ToList() ?? new List<MovimientosCuentaDto>();
                    if (dataMovimientoConcepto != null && dataMovimientoConcepto.Count > 0)
                    {
                        await _verificacionDerechosRepository.InsertarDatosMovimientoConcetoCuenta(dataMovimientoConcepto);
                    }
                }
            }
        }
    }
}
