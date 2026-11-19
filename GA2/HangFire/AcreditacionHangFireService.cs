using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Main;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace HangFire
{
    public class AcreditacionHangFireService
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IMapper _mapper;
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        public AcreditacionHangFireService(
            ISolicitudRepository solicitudRepository,
            IClientRequestProvider clientRequestProvider,
            ICatalogosRepository catalogsRepository,
            IMapper mapper,
            ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudRepository = solicitudRepository;
            _clientRequestProvider = clientRequestProvider;
            _catalogsRepository = catalogsRepository;
            _mapper = mapper;
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        public async Task Execute()
        {

            try
            {
                var procesos = await _catalogsRepository.ObtenerTipoSolicitud(2);
                var procesoVivienda = procesos.FirstOrDefault(x => x.PCS_NOMBRE == "VIVIENDA 8");
                await _catalogsRepository.InsertarLogHangFire($"Ejecutando tarea diaria: {DateTime.Now}", null);
                var peticionDto = new PeticionConsultarSolicitudCesantiasDto
                {
                    Identificacion = "",
                    NumeroSolicitud = "",
                    PageNumber = 1,
                    PageSize = 100,
                    EstadoTarea = 328,
                    TipoSolicitud = procesoVivienda.PCS_ID
                };

                var consulta = _mapper.Map<PeticionConsultarSolicitudCesantias>(peticionDto);
                var solicitudeProcessPayment = await _solicitudBusinessLogic.ConsultarSolicitudCesantias(peticionDto);
                await _catalogsRepository.InsertarLogHangFire($"Se consulto las solicitudes: {DateTime.Now}", null);

                if (solicitudeProcessPayment.Data.Count() > 0)
                {
                    var typeSolicitudesCensatias = solicitudeProcessPayment.Data.ToList();

                    var typeSolicitudes = typeSolicitudesCensatias.Where(x => x.TpsSubDescripcion.Equals("COMPRA O ADQUISICIÓN DE VIVIENDA USADA ")
                    || x.TpsSubDescripcion.Equals("COMPRA O ADQUISICIÓN DE VIVIENDA USADA ")).ToList();
                    await _catalogsRepository.InsertarLogHangFire($"Se Filtro las solicitudes de vivienda : {DateTime.Now}", null);
                    if (typeSolicitudes.Count > 0)
                    {
                        var typeSolicitudFilter = typeSolicitudes.Where(x => x.TareaNombre.Equals("Trámite de pago V 8")
                        && x.EstadoSolicitud.Equals("APROBADA")).ToList();
                        await _catalogsRepository.InsertarLogHangFire($"Se Filtro de trmite pago aprobdas: {DateTime.Now}", null);
                        if (typeSolicitudFilter.Count > 0)
                        {
                            await SolicitudProcessPayment(typeSolicitudFilter);
                        }

                    }
                }
                var solicitudes = await _solicitudRepository.ObtenerSolicitudAcreditacion();
                if (solicitudes.Count > 0)
                {
                    await ProcessSolicitudes(solicitudes);
                }
            }
            catch (Exception e)
            {
                var mesageError = $"Error: ${e.Message} | StackTrace: {e.StackTrace}";
                await _catalogsRepository.InsertarLogHangFire(mesageError, "Fallo en la ejecucion de la tarea"); ;
            }


            await Task.CompletedTask;
        }
        private async Task SolicitudProcessPayment(List<RespuestaConsultarSolicitudCesantiasDto> solicitudes)
        {
            foreach (var solicitud in solicitudes)
            {
                var json = JsonSerializer.Serialize(solicitud);
                await _catalogsRepository.InsertarLogHangFire($"Procesando solicitud de tramite de pago aprobadas", json);
                try
                {
                    var peticionDto = new PeticionConsultarSolicitudCesantiasDto
                    {
                        Identificacion = solicitud.CedulaCliente,
                        NumeroSolicitud = "",
                        PageNumber = 1,
                        PageSize = 10
                    };

                    var consulta = _mapper.Map<PeticionConsultarSolicitudCesantias>(peticionDto);
                    var solicitudeConsult = await _solicitudBusinessLogic.ConsultarSolicitudCesantias(peticionDto);

                    if (solicitudeConsult.Data.Count() > 0)
                    {
                        var solicitu = solicitudeConsult.Data.Where(x => x.TpsSubDescripcion?.Contains("ACREDITACION V8") == true
                        || x.TpsSubDescripcion?.Contains("NO ACREDITACION V8") == true).ToList();
                        if (solicitu.Count == 0)
                        {
                            await CreateSolicitudAcreditacin(solicitud);
                        }
                    }
                }
                catch (Exception e)
                {
                    await _catalogsRepository.InsertarLogHangFire($"Fallo solicitud  - {e.Message}", json); ;
                }

            }
        }
        private async Task CreateSolicitudAcreditacin(RespuestaConsultarSolicitudCesantiasDto request)
        {
            try
            {
                if (request.FechaSolicitud < DateTime.Now)
                {
                    var procesos = await _catalogsRepository.ObtenerTipoSolicitud(2);
                    var procesoAcreditacion = procesos.FirstOrDefault(x => x.PCS_NOMBRE == "ACREDITACIÓN V8");

                    if (procesoAcreditacion == null) return;

                    var modalidades = await _catalogsRepository.ObtenerTipoModalidad(procesoAcreditacion.PCS_ID.ToString());
                    var modalidad = modalidades.FirstOrDefault(x => x.TIM_DESCRIPCION == "NO ACREDITACION V8");

                    if (modalidad == null) return;

                    var subModalidades = await _catalogsRepository.ObtenerTipoSubModalidad(modalidad.TIM_ID.ToString());


                    var crearSolicitudDto = new CrearSolicitudDto
                    {
                        procesoId = procesoAcreditacion.PCS_ID,
                        timCodigo = modalidad.TIM_CODIGO,
                        tipoSubModalidad = subModalidades.First().TPS_SUB_ID,
                        clienteId = request.IdCliente,
                        creadoPor = (Guid)request.IdUsuarioAsignado
                    };

                    await _solicitudBusinessLogic.CrearSolicitud(crearSolicitudDto);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    
        private async Task ProcessSolicitudes(List<SolicitudAcreditacionWorker> solicitudes)
        {
            foreach (var solicitud in solicitudes)
            { var json = JsonSerializer.Serialize(solicitud);   
                await _catalogsRepository.InsertarLogHangFire($"Procesando solicitud ", json);
                try
                {
                    var peticionDto = new PeticionConsultarSolicitudCesantiasDto
                    {
                        Identificacion = solicitud.SOL_CEDULA,
                        NumeroSolicitud = "",
                        PageNumber = 1,
                        PageSize = 10

                    };

                    var consulta = _mapper.Map<PeticionConsultarSolicitudCesantias>(peticionDto);
                    var solicitudeConsult = await _solicitudBusinessLogic.ConsultarSolicitudCesantias(peticionDto);

                    var solicitudAprobada = solicitudeConsult.Data.Where(x =>
                        (x.TpsSubDescripcion?.Contains("VIVIENDA USADA") == true || x.TpsSubDescripcion?.Contains("VIVIENDA NUEVA") == true)
                        ).ToList();

                    if (solicitudAprobada.Count > 0)
                    {
                        await ProcessSolicitudVivienda(solicitudAprobada.FirstOrDefault(), solicitud);
                    }
                }
                catch (Exception e)
                {
                    await _catalogsRepository.InsertarLogHangFire($"Fallo solicitud  - {e.Message}", json); ;
                }

            }
        }

        private async Task ProcessSolicitudVivienda(RespuestaConsultarSolicitudCesantiasDto solicitud, SolicitudAcreditacionWorker solicitudAcreditacion)
        {

            var infoActividad = await _solicitudRepository.ConsultarSolicitudPorActividad(new ConsultarSolicitudPorActividad
            {
                SLT_ID = solicitud.SolicitudTareaId
            });
            var infoSolictud = await _solicitudRepository.ConsultarInformacionSolicitud(infoActividad.SOL_NUMERO_SOLICITUD);
            var trazabilidad = await _solicitudRepository.ObtenerTrazabilidadSolicitud(infoActividad.SOL_ID);
            var tramite = trazabilidad.FirstOrDefault(x => x.TRA_NOMBRE == "Trámite de pago V 8");
            var date = solicitud.TpsSubDescripcion.Contains("VIVIENDA NUEVA") ? infoSolictud.FECHA_FIRMA_ESCRITURA : tramite.SLT_FECHACREACION;
            if (date >= DateTime.Now) return;

            await _solicitudRepository.ActualizarEstadoInconsistente(solicitudAcreditacion.SOL_ID, "");

            var procesos = await _catalogsRepository.ObtenerTipoSolicitud(2);
            var procesoAcreditacion = procesos.FirstOrDefault(x => x.PCS_NOMBRE == "ACREDITACIÓN V8");

            if (procesoAcreditacion == null) return;

            var modalidades = await _catalogsRepository.ObtenerTipoModalidad(procesoAcreditacion.PCS_ID.ToString());
            var modalidad = modalidades.FirstOrDefault(x => x.TIM_DESCRIPCION == "NO ACREDITACION V8");

            if (modalidad == null) return;

            var subModalidades = await _catalogsRepository.ObtenerTipoSubModalidad(modalidad.TIM_ID.ToString());

            var crearSolicitudDto = new CrearSolicitudDto
            {
                procesoId = procesoAcreditacion.PCS_ID,
                timCodigo = modalidad.TIM_CODIGO,
                tipoSubModalidad = subModalidades.First().TPS_SUB_ID,
                clienteId = solicitudAcreditacion.CLI_ID,
                creadoPor = (Guid)solicitud.IdUsuarioAsignado
            };

            await _solicitudBusinessLogic.CrearSolicitud(crearSolicitudDto);
        }
    }
}
