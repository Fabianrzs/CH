using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Application.Dto.Postulacion;
using GA2.Application.Dto.Postulacion.Tipo;
using GA2.Application.Dto.Reportes;
using GA2.Application.Main;
using GA2.Domain.Core.Postulacion.Enum;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Postulacion
{
    public class PostulacionBussineLogic : IPostulacionBussineLogic
    {
        private readonly IMapper _mapper;
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly WorkManager.IGestorDocumentalBusinessLogicFs _WMBusinessLogic;
        private readonly IConvocatoriaRepository _convocatoriaLogic;
        private readonly IReportesExcelLogic _reportesExcelLogic;
        private readonly IClientRequestProvider _iClienteRequestProvider;
        private readonly ICatalogosRepository _catalogsRepository;

        public PostulacionBussineLogic(IMapper mapper,
            IPostulacionRepository postulacion,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            WorkManager.IGestorDocumentalBusinessLogicFs wMBusinessLogic,
            IClientRequestProvider iClienteRequestProvider,
            IConvocatoriaRepository convocatoriaBussigne,
            IReportesExcelLogic reportesExcelLogic,
            ICatalogosRepository catalogsRepository)
        {
            _postulacionRepository = postulacion;
            _mapper = mapper;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _WMBusinessLogic = wMBusinessLogic;
            _convocatoriaLogic = convocatoriaBussigne;
            _reportesExcelLogic = reportesExcelLogic;
            _iClienteRequestProvider = iClienteRequestProvider ?? throw new ArgumentNullException(nameof(iClienteRequestProvider));
            _catalogsRepository = catalogsRepository;
        }

        public async Task<Response<bool>> CargarDocumentosSolicitud(List<CargarDocumentosSolicitudDto> cargarDocumentosSolicitudDto, Guid IdSolicitud, Guid userId)
        {
            var attachRequestDto = new AttachRequestDto
            {
                UserOperation = "web",
                Files = new List<FileDto>()
            };
            string nombreArchivo = "";

            try
            {

                foreach (var x in cargarDocumentosSolicitudDto)
                {
                    FileStreamResult fileStreamResult = new(x.archivosCargados.OpenReadStream(), x.archivosCargados.ContentType);

                    FileInfo fi = new(x.archivosCargados.FileName.Split("--")[1]);
                    nombreArchivo = fi.Name;
                    if (fi.Extension == ".pdf" || fi.Extension == ".PDF")
                    {
                        var newName = Path.GetRandomFileName() + fi.Extension.ToLower();
                        var result = await _blobStorageGenericRepository.SubirArchivoBlobStorage("fondoso", newName.ToString(), fileStreamResult);

                        CargeArchivosPostulacion insertarArchivo = new()
                        {
                            Asp_Nombre_Archivo = fi.Name,
                            Asp_Ruta_Storage = newName,
                            Asp_Extension = fi.Extension,
                            Asp_Solicitud_Id = IdSolicitud,
                            Asp_Numero_Folio = x.numeroFolio,
                            Asp_Id_Archivo_Fk = x.archivoParametrizadoId,
                            UserId = userId
                        };

                        attachRequestDto.Files.Add(CrearObjetoWorkmanager(fi, x.archivosCargados));

                        var resultReposotory = await _postulacionRepository.InsertarArchivoPorSolicitudPostulacion(insertarArchivo);
                    }
                    else { return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = "Algunos documentos no fueron almacenados. Solo puede cargar documentos PDF" }; }

                }

            }
            catch (Exception e)
            {
                return new Response<bool> { Data = false, IsSuccess = false, ReturnMessage = $"Error al cargar los archivos, {nombreArchivo} -{e.Message}" };
            }
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            foreach (var statusTwo in estadoRepository)
            {
                if (statusTwo.ESP_NOMBRE.Equals("PENDIENTE"))
                {
                    var resultActividad = await _postulacionRepository.ObtenerActividadFondoSolidaridad();
                    foreach (var item in resultActividad)
                    {
                        if (item.NOMBRE.Equals("CARGUE DE DOCUMENTOS FONDO SOLIDARIDAD"))
                        {
                            await _postulacionRepository.ActualizarEstadoArchivoPostulacion(IdSolicitud, statusTwo.ESP_ID, item.ID);
                        }
                    }

                }
            }
            return new Response<bool> { Data = true, IsSuccess = true, ReturnMessage = "Documentos almacenados correctamente" };
        }

        public async Task<Response<IEnumerable<TipoModaliadPostulacionDto>>> ConsultarModalidad(int id)
        {
            var responseRepository = await _postulacionRepository.Consultar_Tipo_Modalidad_Postulacion(id);
            if (responseRepository.Any())
            {
                var resultMapper = _mapper.Map<IEnumerable<TipoModaliadPostulacionDto>>(responseRepository);
                return new Response<IEnumerable<TipoModaliadPostulacionDto>> { Data = resultMapper, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<TipoModaliadPostulacionDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos" };
        }

        public async Task<Response<IEnumerable<TipoSolicitudPostulacionDto>>> ConsultarSolicitud()
        {
            var responseRepository = await _postulacionRepository.Consultar_Tipo_Solicitud_Postulacion();
            if (responseRepository.Any())
            {
                var resultMapper = _mapper.Map<IEnumerable<TipoSolicitudPostulacionDto>>(responseRepository);
                return new Response<IEnumerable<TipoSolicitudPostulacionDto>> { Data = resultMapper, IsSuccess = true, ReturnMessage = "Succeful" };

            }

            return new Response<IEnumerable<TipoSolicitudPostulacionDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos" };
        }

        private async Task<Response<IEnumerable<SolicituPostulancionResponseDto>>> FilterDataFondo(SolicitudPostulacionRequest requestDto)
        {
            var listResult = new List<SolicituPostulancionResponseDto>();
            switch (requestDto.EtapasId)
            {
                case 1:
                    var resultFondoRepository = await _postulacionRepository.ConsultarSolicitudPostulacion(requestDto);
                    if (resultFondoRepository != null)
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultFondoRepository);
                        foreach (var item in mapResult)
                        {
                            if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("ANULADO"))
                            {
                                listResult.Add(item);
                            }
                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                case 2:
                    var resultArchivoRepository = await _postulacionRepository.ConsultarArchivoFs(requestDto);
                    if (resultArchivoRepository.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultArchivoRepository);
                        foreach (var item in mapResult)
                        {
                            if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("ANULADO"))
                            {
                                listResult.Add(item);
                            }
                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };

                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                case 3:
                    var resultEntrevistaRepository = await _postulacionRepository.ConsultarEntrevistaFs(requestDto);
                    if (resultEntrevistaRepository.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultEntrevistaRepository);
                        foreach (var item in mapResult)
                        {
                            if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("ANULADO"))
                            {
                                listResult.Add(item);
                            }
                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                case 4:
                    var resultCalificacionRepository = await _postulacionRepository.ConsultarCalicacionFs(requestDto);
                    if (resultCalificacionRepository.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultCalificacionRepository);
                        foreach (var item in mapResult)
                        {
                            if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("APROBADO"))
                            {
                                listResult.Add(item);
                            }
                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };

                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                case 5:
                    var resultVfsRepository = await _postulacionRepository.ConsultarDataVerificatePostulacinoFs(requestDto);
                    if (resultVfsRepository.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultVfsRepository);
                        foreach (var item in mapResult)
                        {

                            if (item.EstadoProceso == "BLOQUEADO")
                            {
                                listResult.Add(item);
                            }

                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };

                case 6:
                    var resultRepositoryEmbargo = await _postulacionRepository.ConsultarDataVerificatePostulacinoFs(requestDto);
                    if (resultRepositoryEmbargo.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultRepositoryEmbargo);
                        foreach (var item in mapResult)
                        {

                            if (item.EstadoProceso == "BLOQUEADO")
                            {
                                listResult.Add(item);
                            }

                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };

                case 7:
                    var resultVfIndividual = await _postulacionRepository.ConsultarDataVerificatePostulacinoFs(requestDto);
                    if (resultVfIndividual.Any())
                    {
                        var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultVfIndividual);
                        foreach (var item in mapResult)
                        {

                            if (item.EstadoProceso == "BLOQUEADO")
                            {
                                listResult.Add(item);
                            }

                        }
                        return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };

                default:
                    break;
            }
            return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
        }
        public async Task<Response<IEnumerable<SolicituPostulancionResponseDto>>> ConsultarSolicitudPostultacion(SolicitudPostulacionRequestDto requestDto)
        {
            var mapRequest = _mapper.Map<SolicitudPostulacionRequest>(requestDto);
            var listResult = new List<SolicituPostulancionResponseDto>();
            if (mapRequest.EtapasId == null && mapRequest.Identificacion == null && mapRequest.TipoSolicitudId == null && mapRequest.NoRadicado == null && mapRequest.NoConvocatoria == null)
            {
                var result = await _postulacionRepository.ConsultarSolicitudPostulacion(mapRequest);
                if (result.Any())
                {
                    var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(result);
                    foreach (var item in mapResult)
                    {
                        var resultArchivo = await _postulacionRepository.ObtenerDatosArchivoById(item.IdSolicitud);
                        if (resultArchivo != null)
                        {
                            if (resultArchivo.EtapaProceso.Equals("PENDIENTE") || resultArchivo.EtapaProceso.Equals("INCONSISTENTE") || resultArchivo.EtapaProceso.Equals("ANULADO"))
                            {
                                var mapArchivo = _mapper.Map<SolicituPostulancionResponseDto>(resultArchivo);
                                listResult.Add(mapArchivo);
                            }
                        }

                        var resultVerificated = await _postulacionRepository.ObtenerDatoVrificatePostulacionById(item.IdSolicitud);
                        if (resultVerificated != null)
                        {
                            if (resultVerificated.EtapaProceso.Equals("BLOQUEADO"))
                            {
                                var mapArchivo = _mapper.Map<SolicituPostulancionResponseDto>(resultVerificated);
                                listResult.Add(mapArchivo);
                            }
                        }
                        var resultEntrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(item.IdSolicitud);
                        if (resultEntrevista != null)
                        {
                            if (resultEntrevista.EtapaProceso.Equals("PENDIENTE") || resultEntrevista.EtapaProceso.Equals("INCONSISTENTE") || resultEntrevista.EtapaProceso.Equals("ANULADO"))
                            {
                                var mapEntrevist = _mapper.Map<SolicituPostulancionResponseDto>(resultEntrevista);
                                listResult.Add(mapEntrevist);
                            }
                        }
                        var resultCalificacion = await _postulacionRepository.ObtenerDatosCalificacionById(item.IdSolicitud);
                        if (resultCalificacion != null)
                        {
                            var mapCalificacion = _mapper.Map<SolicituPostulancionResponseDto>(resultCalificacion);
                            if (mapCalificacion.EstadoProceso.Equals("PENDIENTE") || mapCalificacion.EstadoProceso.Equals("INCONSISTENTE") || mapCalificacion.EstadoProceso.Equals("ANULADO") || mapCalificacion.EstadoProceso.Equals("APROBADO"))
                            {
                                listResult.Add(mapCalificacion);
                            }
                        }
                        if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("ANULADO"))
                        {
                            listResult.Add(item);
                        }
                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                }
            }
            if (mapRequest.EtapasId != null)
            {
                var data = await FilterDataFondo(mapRequest);
                return data;
            }
            else
            {
                var resultArchivoRepository = await _postulacionRepository.ConsultarSolicitudPostulacion(mapRequest);
                if (resultArchivoRepository.Any())
                {
                    var mapResult = _mapper.Map<IEnumerable<SolicituPostulancionResponseDto>>(resultArchivoRepository);
                    foreach (var item in mapResult)
                    {
                        var resultArchivo = await _postulacionRepository.ObtenerDatosArchivoById(item.IdSolicitud);
                        if (resultArchivo != null)
                        {
                            if (resultArchivo.EtapaProceso.Equals("PENDIENTE") || resultArchivo.EtapaProceso.Equals("INCONSISTENTE") || resultArchivo.EtapaProceso.Equals("ANULADO"))
                            {
                                var mapArchivo = _mapper.Map<SolicituPostulancionResponseDto>(resultArchivo);
                                listResult.Add(mapArchivo);
                            }
                        }
                        var resultVerificated = await _postulacionRepository.ObtenerDatoVrificatePostulacionById(item.IdSolicitud);
                        if (resultVerificated != null)
                        {
                            if (resultVerificated.EtapaProceso.Equals("BLOQUEADO"))
                            {
                                var mapArchivo = _mapper.Map<SolicituPostulancionResponseDto>(resultVerificated);
                                listResult.Add(mapArchivo);
                            }
                        }
                        var resultEntrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(item.IdSolicitud);
                        if (resultEntrevista != null)
                        {
                            if (resultEntrevista.EtapaProceso.Equals("PENDIENTE") || resultEntrevista.EtapaProceso.Equals("INCONSISTENTE") || resultEntrevista.EtapaProceso.Equals("ANULADO"))
                            {
                                var mapEntrevist = _mapper.Map<SolicituPostulancionResponseDto>(resultEntrevista);
                                listResult.Add(mapEntrevist);
                            }
                        }
                        var resultCalificacion = await _postulacionRepository.ObtenerDatosCalificacionById(item.IdSolicitud);
                        if (resultCalificacion != null)
                        {
                            var mapCalificacion = _mapper.Map<SolicituPostulancionResponseDto>(resultCalificacion);
                            if (mapCalificacion.EstadoProceso.Equals("PENDIENTE") || mapCalificacion.EstadoProceso.Equals("INCONSISTENTE") || mapCalificacion.EstadoProceso.Equals("ANULADO") || mapCalificacion.EstadoProceso.Equals("APROBADO"))
                            {
                                listResult.Add(mapCalificacion);
                            }
                        }
                        if (item.EstadoProceso.Equals("PENDIENTE") || item.EstadoProceso.Equals("INCONSISTENTE") || item.EstadoProceso.Equals("ANULADO"))
                        {
                            listResult.Add(item);
                        }

                    }
                    return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = listResult, IsSuccess = true, ReturnMessage = "Succeful" };
                }
            }
            return new Response<IEnumerable<SolicituPostulancionResponseDto>> { Data = null, IsSuccess = true, ReturnMessage = "Succeful" };
        }


        public async Task<Response<IEnumerable<TipoSubModalidadPostulacionDto>>> ConsultarSubModalidad(int id)
        {

            var responseRepository = await _postulacionRepository.Consultar_Spo_Submodalidad_Postulacion(id);
            if (responseRepository.Any())
            {
                var resultMapper = _mapper.Map<IEnumerable<TipoSubModalidadPostulacionDto>>(responseRepository);
                return new Response<IEnumerable<TipoSubModalidadPostulacionDto>> { Data = resultMapper, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<TipoSubModalidadPostulacionDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos" };
        }

        private async Task<int> ValidarReglaNegocioFondo(SolicitudPostulante request)
        {
            request.SPO_ESTADO = request.SPO_DISFRUTE_PENSION == "NO" && request.SPO_ID_SUBMODALIDAD == (int)SubModalidadPostulacionEnum.ProyectoMecanismoEspeciales
                ? (int)EstadoPostulacionEnum.Inconsistente
                : (int)EstadoPostulacionEnum.Pendiente;



            if (request.SPO_FECHA_ASIGNACION_INVALIDEZ is not null)
            {
                request.SPO_ESTADO = request.SPO_DISFRUTE_PENSION == "SI"
                         && request.SPO_ID_SUBMODALIDAD == (int)SubModalidadPostulacionEnum.Heroes
                         && request.SPO_FECHA_ASIGNACION_INVALIDEZ < new DateTime(2009, 6, 3)
                     ? (int)EstadoPostulacionEnum.Inconsistente
                      : (int)EstadoPostulacionEnum.Pendiente;
            }
            if (request.SPO_FECHA_ASIGNACION_INVALIDEZ is not null)
            {
                request.SPO_ESTADO = request.SPO_DISFRUTE_PENSION == "SI"
                         && request.SPO_ID_SUBMODALIDAD == (int)SubModalidadPostulacionEnum.Heroes
                         && request.SPO_FECHA_ASIGNACION_INVALIDEZ < new DateTime(2009, 6, 3)
                         && request.SPO_MISMO_AFILIADO
                     ? (int)EstadoPostulacionEnum.Inconsistente
                      : (int)EstadoPostulacionEnum.Pendiente;
            }

            var dataResponse = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.SPO_TIPO_DOCUMENTO_ID, request.SPO_NUMERO_DOCUMENTO.ToString());
            var client = dataResponse.Data;
            if (client is not null)
            {
                var solicitudes = client.Solicitudes.Where(x => x.EstadoSolicitud == "Procesada" &&
                (x.TipoSolicitud == "Desafiliación Voluntaria CI con cesantías")
                || x.TipoSolicitud == "Desafiliación"
                || x.TipoSolicitud == "Desafiliación Voluntaria CI solo aportes"
                ).FirstOrDefault();
                if (solicitudes is not null)
                {
                    request.SPO_ESTADO = (int)EstadoPostulacionEnum.Inconsistente;
                }
                var cuenta = client.Cuentas.Where(x => x.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA").FirstOrDefault();
                if (cuenta is not null)
                {
                    request.SPO_ESTADO = cuenta.EstadoCuenta == "CANCELADA"
                        ? (int)EstadoPostulacionEnum.Inconsistente
                        : (int)EstadoPostulacionEnum.Pendiente; ;
                }
            }
            return request.SPO_ESTADO;
        }
        private async Task<List<DependienteDto>> ValidateSolicitudReplica(SolicitudPostulante request)
        {
            var dataResponse = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.SPO_TIPO_DOCUMENTO_ID, request.SPO_NUMERO_DOCUMENTO.ToString());
            var client = dataResponse.Data;
            if (client.Dependientes.Count > 0 && client.TipoVinculacion == "AFILIADO ADMINISTRACION CESANTIAS")
            {
                return client.Dependientes;
            }
            return null;
        }
        public async Task<Response<SolicitudPostulacionDto>> CrearSolicitudPostante(SolicitudPostulacionDto solicitudPostulacion)
        {
            if (solicitudPostulacion != null)
            {
                if (solicitudPostulacion.IdTipoSolicitud <= 0)
                {
                    return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor ingrese el tipo de solicitud" };
                }
                if (solicitudPostulacion.IdModalidad <= 0)
                {
                    return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor ingrese el tipo modalidad" };
                }
                if (solicitudPostulacion.IdSubModalidad <= 0)
                {
                    return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor ingrese el tipo sub modalidad" };
                }
                if (solicitudPostulacion.NumeroConvocatoria <= 0)
                {
                    return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor ingrese el numero de convocatoria" };
                }

                var resultMap = _mapper.Map<SolicitudPostulante>(solicitudPostulacion);

                resultMap.SPO_FECHA_ASIGNACION_INVALIDEZ = solicitudPostulacion.FechaAsignacionInvalidez != ""
                ? DateTime.Parse(solicitudPostulacion.FechaAsignacionInvalidez) : null;


                resultMap.SPO_FECHA_RETIRO = solicitudPostulacion?.FechaRetiro == null || solicitudPostulacion?.FechaRetiro == ""
                ? null : DateTime.Parse(solicitudPostulacion.FechaRetiro);


                resultMap.SPO_ESTADO = await ValidarReglaNegocioFondo(resultMap);

                var client = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(resultMap.SPO_TIPO_DOCUMENTO_ID, resultMap.SPO_NUMERO_DOCUMENTO.ToString());
                var dataClient = client.Data;

                resultMap.ACTIVIDA_POSTULACION = (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD;
                resultMap.SPO_FECHA_CREACION = DateTime.Now;
                resultMap.SPO_USERID = solicitudPostulacion.UserId;
                resultMap.IdConvocatoria = solicitudPostulacion.IdConvocatoria;
                if (dataClient.AportesCategoria.Any())
                {

                    resultMap.SPO_FECHA_PRIMER_APORTE = dataClient.AportesCategoria.Select(x => x.APC_FECHA_PRIMERA_CUOTA).FirstOrDefault();
                    resultMap.SPO_FECHA_ULTIMO_APORTE = dataClient.AportesCategoria.Select(x => x.APC_FECHA_ULTIMA_COUTA).FirstOrDefault();
                }
                var validateSolicitudPrincipal = await ValidateSolicitudReplica(resultMap);
                resultMap.SPO_PRINCIPAL = true;
                resultMap.SPO_ID_SOLICITUD_PRINCIPAL = null;


                var resultRepository = await _postulacionRepository.Spo_Crear_Solicitud_Postulante(resultMap);
                await _convocatoriaLogic.UpdateStatusConvocatoria(
                   (int)resultMap.SPO_NUMERO_CONVOCATORIA, (int)EstadoConvocatoriaEnum.Verificacion);
                if (validateSolicitudPrincipal?.Count > 0)
                {
                    foreach (var item in validateSolicitudPrincipal)
                    {
                        resultMap.SPO_PRINCIPAL = true;
                        resultMap.SPO_ID_SOLICITUD_PRINCIPAL = null;
                        resultMap.SPO_NUMERO_DOCUMENTO = long.Parse(item.Identificacion);
                        resultMap.SPO_TIPO_DOCUMENTO_ID = item.TipoIdentificacion;
                        resultMap.SPO_NOMBRE_AFILIADO_PRINCIPAL = item.Nombre;
                        resultRepository = await _postulacionRepository.Spo_Crear_Solicitud_Postulante(resultMap);
                    }
                }
                var responseMap = _mapper.Map<SolicitudPostulacionDto>(resultRepository);
                var radicado = await _WMBusinessLogic.RadicarGDFondoSolidaridad(resultRepository.SPO_ID_SOLICITUD);
                if (radicado.IsSuccess && radicado.ReturnMessage == "El formulario fue guardado")
                {
                    var resultRadicado = radicado.Data.Form_InsertResult.BarCode == string.Empty ? "RADICADO NO DISPONIBLE." : radicado.Data.Form_InsertResult.BarCode;
                    await _postulacionRepository.ActualizarRadicadoAprobado(resultRepository.SPO_ID_SOLICITUD, resultRadicado);

                    responseMap.NoRadicado = resultRadicado;
                }
                else
                {
                    if (radicado.IsSuccess && radicado.ReturnMessage == "El formulario se ha encolado para su procesamiento")
                    {
                        await _postulacionRepository.DeleteSolicitudFondoSolidaridad(resultRepository.SPO_ID_SOLICITUD);
                        return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "No se creo la solictud exitosamente,por inconveniente en la rdicacion" };

                    }
                    else
                    {
                        await _postulacionRepository.DeleteSolicitudFondoSolidaridad(resultRepository.SPO_ID_SOLICITUD);
                        return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "No se creo la solictud exitosamente,por inconveniente en la rdicacion" };
                    }
                }
                if (responseMap != null)
                {
                    return new Response<SolicitudPostulacionDto> { Data = responseMap, IsSuccess = true, ReturnMessage = "Succeful" };
                }
            }
            return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide la informacion a ingresar" };

        }

        public async Task<Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>>> ObtenerListaArchivoPorModalidad(DocumentoModalidadPostulacionDto documento)
        {
            var resultMap = _mapper.Map<DocumentoModalidadPostulacion>(documento);
            var responseRepository = await _postulacionRepository.ConsultarDocumentosPorModalidadPostulacion(resultMap);
            if (responseRepository.Any())
            {
                var resultMapper = _mapper.Map<IEnumerable<TipoListaCargarArchivoPostulacionDto>>(responseRepository);
                return new Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>> { Data = resultMapper, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<TipoListaCargarArchivoPostulacionDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos" };
        }

        public async Task<Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>>> TipoEstadoSolicitudPostulacion()
        {
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (estadoRepository.Any())
            {
                var EstadoMap = _mapper.Map<IEnumerable<TipoEstadoSolicitudPostulacionDto>>(estadoRepository);
                return new Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>> { Data = EstadoMap, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<TipoEstadoSolicitudPostulacionDto>> { Data = null, IsSuccess = false, ReturnMessage = "Succeful" };
        }

        public async Task<Response<SolicitudPostulacionDto>> UpdateSolicitud(SolicitudPostulacionDto solicitud)
        {
            var mapSolicitud = _mapper.Map<SolicitudPostulante>(solicitud);
            var updateSolicitud = await _postulacionRepository.Update_Solicitud_Postulante(mapSolicitud);
            var mapUpdateSolicitud = _mapper.Map<SolicitudPostulacionDto>(updateSolicitud);
            if (updateSolicitud != null)
            {
                return new Response<SolicitudPostulacionDto> { Data = mapUpdateSolicitud, IsSuccess = true, ReturnMessage = "Actualizado Exitoso" };
            }
            return new Response<SolicitudPostulacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Valide la informacion a actualizar" };
        }

        public async Task<Response<SolicituPostulancionResponseDto>> UpdateStatusAproved(Guid idSolicitud, Guid userId)
        {

            var postulanteRepository = await _postulacionRepository.ConsultarSolicitudPostulacionBySolId(idSolicitud);
            if (postulanteRepository != null)
            {
                if (postulanteRepository.EtapaProceso == EstadoPostulacionEnums.Pendiente)
                {
                    var updateStatus = await _postulacionRepository.Update_Status_Aproved(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Aprobado);
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        postulanteRepository.SolicitudId,
                        userId,
                        postulanteRepository.Observacion,
                        (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD,
                        (int)EstadoPostulacionEnum.Aprobado
                        );

                    var numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(updateStatus.NumeroConvocatoria);
                    var validateNumero = numeroResolucion?.NumeroResolucionCierre;
                    var mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(updateStatus);
                    if (validateNumero is null)
                    {
                        mapSolicitud.NumeroResolucionCierre = null;
                    }
                    else
                    {
                        mapSolicitud.NumeroResolucionCierre = validateNumero;
                    }
                    var updateArchivo = await _postulacionRepository.CrearCargueArchivo(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD, userId);
                    if (updateArchivo > 0)
                    {
                        return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = true, ReturnMessage = "Aprobado con exito" };

                    }
                    return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "Se aprobo, pero no se creo la siguiente etapa, por favor verifique" };
                }
                if (postulanteRepository.EtapaProceso.Equals(EstadoPostulacionEnums.Inconsistente))
                {
                    if (postulanteRepository.FechaAsignacionInvalidez is not null)
                    {
                        int estado;
                        if (postulanteRepository.FechaAsignacionInvalidez is not null)
                        {
                            estado = postulanteRepository.DisfrutePension == "SI"
                                     && int.Parse(postulanteRepository.TipoSubModalidadId) == (int)SubModalidadPostulacionEnum.Heroes
                                     && postulanteRepository.FechaAsignacionInvalidez < new DateTime(2009, 6, 3)
                                 ? (int)EstadoPostulacionEnum.Inconsistente
                                  : (int)EstadoPostulacionEnum.Aprobado;
                        }
                        else
                        {
                            estado = postulanteRepository.DisfrutePension == "NO"
                                && int.Parse(postulanteRepository.TipoSubModalidadId) == (int)SubModalidadPostulacionEnum.ProyectoMecanismoEspeciales
                                ? (int)EstadoPostulacionEnum.Inconsistente
                                : (int)EstadoPostulacionEnum.Aprobado;
                        }


                        var updateStatus = await _postulacionRepository.Update_Status_Aproved(postulanteRepository.SolicitudId, estado);
                        var mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(updateStatus);
                        var resultDocumentoReposity = await _postulacionRepository.ObtenerDatosArchivoById(mapSolicitud.IdSolicitud);
                        if (resultDocumentoReposity == null && estado == (int)EstadoPostulacionEnum.Inconsistente)
                        {
                            await _postulacionRepository.CrearCargueArchivo(postulanteRepository.SolicitudId, estado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD, userId);
                            return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

                        }
                        if (resultDocumentoReposity != null)
                        {
                            await _postulacionRepository.ActualizarEstadoArchivoPostulacion(resultDocumentoReposity.SolicitudId, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                            await _postulacionRepository.CreateTrazabilidadFondo(
                                                postulanteRepository.SolicitudId,
                                                userId,
                                                postulanteRepository.Observacion,
                                                (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD,
                                                estado
                                                );
                            return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

                        }
                        return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

                    }
                }
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "La solicitud no se encuentra en estado PENDIENTE" };

            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "No existe solicitud con ese numero de documento" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> UpdateStatusRecline(Guid idSolicitud, Guid Userid)
        {
            if (string.IsNullOrEmpty(idSolicitud.ToString()))
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (estadoRepository.Any())
            {
                foreach (var item in estadoRepository)
                {
                    if (item.ESP_NOMBRE.Equals("ANULADO"))
                    {
                        var actividaPostulante = await _postulacionRepository.ObtenerActividadFondoSolidaridad();
                        foreach (var actividad in actividaPostulante)
                        {
                            if (actividad.ID == 1)
                            {
                                var updateStatus = await _postulacionRepository.Update_Status_Aproved(idSolicitud, item.ESP_ID);
                                await _postulacionRepository.CreateTrazabilidadFondo(
                                   updateStatus.SolicitudId,
                                   Userid,
                                   updateStatus.Observacion,
                                   (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD,
                                   (int)EstadoPostulacionEnum.Anulado
                                   );
                                await _postulacionRepository.Obtener_Postulante_By_Id_Solicitud(updateStatus.SolicitudId);
                                var mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(updateStatus);
                                return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };
                            }
                        }

                    }
                }
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "NO EXISTE ESTADO RECHAZADO" };
            }

            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "OPS! ALGO SALIO MAL" };
        }



        public async Task<Response<SolicituPostulancionResponseDto>> ObtenerInformcacionPostulacion(string radicado)
        {
            var solicitudRepository = await _postulacionRepository.ObtenerInformacionSolicitudByRadicado(radicado);
            if (solicitudRepository != null)
            {
                CierreConvocatoria numeroResolucion;
                string validateNumero;
                var mapoliciud = new SolicituPostulancionResponseDto();
                var calificacionRepository = await _postulacionRepository.ObtenerDatosCalificacionById(solicitudRepository.SolicitudId);
                if (calificacionRepository != null)
                {
                    if (calificacionRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                         validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(calificacionRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }

                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var entrevistaRepository = await _postulacionRepository.ObtenerDatosEntrevitsById(solicitudRepository.SolicitudId);
                if (entrevistaRepository != null)
                {
                    if (entrevistaRepository.EtapaProceso != "RECHAZADO")
                    {

                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(entrevistaRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var dataVerificateRepository = await _postulacionRepository.ObtenerDatoVrificatePostulacionById(solicitudRepository.SolicitudId);
                if (dataVerificateRepository != null)
                {
                    if (dataVerificateRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(dataVerificateRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var cargueDocumentoRepository = await _postulacionRepository.ObtenerDatosArchivoById(solicitudRepository.SolicitudId);
                if (cargueDocumentoRepository != null)
                {
                    if (cargueDocumentoRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(cargueDocumentoRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                validateNumero = numeroResolucion?.NumeroResolucionCierre;
                if (validateNumero is null)
                {
                    mapoliciud.NumeroResolucionCierre = null;
                }
                else
                {
                    mapoliciud.NumeroResolucionCierre = validateNumero;
                }
                mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(solicitudRepository);
                mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;

                return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide la informacion a ingresar" };
        }

        public async Task<Response<SolicituPostulancionResponseDto>> ValidarSolicitud(long identificacion)
        {
            var solicitudRepository = await _postulacionRepository.Consultar_Solicitud_Postulacion_By_Id(identificacion);
            if (solicitudRepository != null)
            {
                CierreConvocatoria numeroResolucion;
                string validateNumero;
                var mapoliciud = new SolicituPostulancionResponseDto();
                var calificacionRepository = await _postulacionRepository.ObtenerDatosCalificacionById(solicitudRepository.SolicitudId);
                if (calificacionRepository != null)
                {
                    if (calificacionRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(calificacionRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }

                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var entrevistaRepository = await _postulacionRepository.ObtenerDatosEntrevitsById(solicitudRepository.SolicitudId);
                if (entrevistaRepository != null)
                {
                    if (entrevistaRepository.EtapaProceso != "RECHAZADO")
                    {

                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(entrevistaRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var dataVerificateRepository = await _postulacionRepository.ObtenerDatoVrificatePostulacionById(solicitudRepository.SolicitudId);
                if (dataVerificateRepository != null)
                {
                    if (dataVerificateRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(dataVerificateRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                var cargueDocumentoRepository = await _postulacionRepository.ObtenerDatosArchivoById(solicitudRepository.SolicitudId);
                if (cargueDocumentoRepository != null)
                {
                    if (cargueDocumentoRepository.EtapaProceso != "RECHAZADO")
                    {
                        numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                        validateNumero = numeroResolucion?.NumeroResolucionCierre;
                        mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(cargueDocumentoRepository);
                        if (validateNumero is null)
                        {
                            mapoliciud.NumeroResolucionCierre = null;
                        }
                        else
                        {
                            mapoliciud.NumeroResolucionCierre = validateNumero;
                        }
                        mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;
                        return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
                    }
                }
                numeroResolucion = await _convocatoriaLogic.GetDataCierreConvocatoria(solicitudRepository.NumeroConvocatoria);
                validateNumero = numeroResolucion?.NumeroResolucionCierre;
                if (validateNumero is null)
                {
                    mapoliciud.NumeroResolucionCierre = null;
                }
                else
                {
                    mapoliciud.NumeroResolucionCierre = validateNumero;
                }
                mapoliciud = _mapper.Map<SolicituPostulancionResponseDto>(solicitudRepository);
                mapoliciud.EstadoProceso = mapoliciud.Actividad.Contains("VERIFICACIÓN DE NOVEDADES") ? "BLOQUEADO" : mapoliciud.EstadoProceso;

                return new Response<SolicituPostulancionResponseDto> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide la informacion a ingresar" };
        }
        private static FileDto CrearObjetoWorkmanager(FileInfo fi, IFormFile file)
        {
            return new FileDto(ConvertBase64Pdf(file), fi.FullName, "00", "00", ".pdf", "");
        }
        private static string ConvertBase64Pdf(IFormFile file)
        {

            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
        public async Task<FileResult> DescargarArchivoPorSolicitud(string rutaStorage)
        {
            var blobCheck = await _blobStorageGenericRepository.DescargarArchivoStorage("fondoso", rutaStorage);
            var descargado = new FileStreamResult(blobCheck.Content, blobCheck.ContentType)
            {
                FileDownloadName = rutaStorage
            };
            await descargado.FileStream.FlushAsync();

            return descargado;
        }
        public async Task<Response<bool>> EliminarArchivoPorSolicitud(EliminarArchivoPorSolicitudDto eliminarArchivoPorSolicitudDto)
        {
            Response<bool> respuesta = new()
            {
                Data = false,
                IsSuccess = false
            };

            //elimina del blobstorage
            await _blobStorageGenericRepository.EliminarArchivoStorage("fondoso", eliminarArchivoPorSolicitudDto.rutaStorage);

            //elimina de la bd
            EliminarArchivoFSPorSolicitud eliminarArchivo = new()
            {
                Asp_Id = eliminarArchivoPorSolicitudDto.idArchivo,
                Asp_Ruta_Storage = eliminarArchivoPorSolicitudDto.rutaStorage
            };

            respuesta.Data = await _postulacionRepository.EliminarArchivoFSPorSolicitud(eliminarArchivo);
            respuesta.IsSuccess = true;

            return respuesta;
        }
        public async Task<Response<IEnumerable<ArchivoSolicitudDto>>> ConsultarArchivoPorSolicitudPostulacion(Guid idSolicitud)
        {
            var solicitudRepository = await _postulacionRepository.ConsultarArchivoPorSolicitudFs(idSolicitud);
            if (solicitudRepository != null)
            {
                var mapoliciud = _mapper.Map<IEnumerable<ArchivoSolicitudDto>>(solicitudRepository);
                return new Response<IEnumerable<ArchivoSolicitudDto>> { Data = mapoliciud, IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<IEnumerable<ArchivoSolicitudDto>> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide la informacion a ingresar" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> AprobarCargoDocumento(AprobarCargueDocumentoDto request, Guid userId)
        {
            if (string.IsNullOrEmpty(request.IdSolicitud.ToString()))
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }
            var postulanteRepository = await _postulacionRepository.ObtenerDatosArchivoById(request.IdSolicitud);
            if (postulanteRepository != null)
            {
                if (postulanteRepository.EtapaProceso == EstadoPostulacionEnums.Pendiente)
                {
                    var mapSolicitud = new SolicituPostulancionResponseDto();
                    await _postulacionRepository.ActualizarEstadoArchivoPostulacion(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Aprobado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                    var resultRepository = await _postulacionRepository.ConsultarObservationArchivoFsBySolId(postulanteRepository.SolicitudId);
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        postulanteRepository.SolicitudId,
                        userId,
                        resultRepository.Observacion,
                        (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                        (int)EstadoPostulacionEnum.Aprobado
                        );
                    var validateVericate = await CreateValidateVerificate(request.IdSolicitud, request.Etapa, userId);
                    if (validateVericate == 0)
                    {
                      await _postulacionRepository.CrearEntrevista(request.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.ENTREVISTA, userId);
                    }
                    return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

                }
                if (postulanteRepository.EtapaProceso.Equals(EstadoPostulacionEnums.Inconsistente))
                {
                    int estado;
                    if (postulanteRepository.FechaAsignacionInvalidez is not null)
                    {
                        estado = postulanteRepository.DisfrutePension == "SI"
                                 && int.Parse(postulanteRepository.TipoSubModalidadId) == (int)SubModalidadPostulacionEnum.Heroes
                                 && postulanteRepository.FechaAsignacionInvalidez < new DateTime(2009, 6, 3)
                             ? (int)EstadoPostulacionEnum.Inconsistente
                              : (int)EstadoPostulacionEnum.Aprobado;
                    }
                    else
                    {
                        estado = postulanteRepository.DisfrutePension == "NO"
                                   && int.Parse(postulanteRepository.TipoSubModalidadId) == (int)SubModalidadPostulacionEnum.ProyectoMecanismoEspeciales
                                 ? (int)EstadoPostulacionEnum.Inconsistente
                                 : (int)EstadoPostulacionEnum.Aprobado;
                    }


                    if (postulanteRepository.FechaAsignacionInvalidez is not null)
                    {
                        estado = postulanteRepository.DisfrutePension == "SI"
                                 && int.Parse(postulanteRepository.TipoSubModalidadId) == (int)SubModalidadPostulacionEnum.Heroes
                                 && postulanteRepository.FechaAsignacionInvalidez < new DateTime(2009, 6, 3)
                                  && postulanteRepository.MismoAfiliado
                             ? (int)EstadoPostulacionEnum.Inconsistente
                              : (int)EstadoPostulacionEnum.Aprobado;
                    }

                    var mapSolicitu = new SolicituPostulancionResponseDto();

                    var resultObservacion = await _postulacionRepository.ConsultarObservationArchivoFsBySolId(postulanteRepository.SolicitudId);

                    if (estado == (int)EstadoPostulacionEnum.Inconsistente)
                    {
                        await _postulacionRepository.ActualizarEstadoArchivoPostulacion(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                        await _postulacionRepository.CreateTrazabilidadFondo(postulanteRepository.SolicitudId,
                                userId,
                                resultObservacion.Observacion,
                                (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                                (int)EstadoPostulacionEnum.Inconsistente
                                );
                        return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitu, IsSuccess = false, ReturnMessage = "OK" };
                    }
                    else
                    {

                         await _postulacionRepository.ActualizarEstadoArchivoPostulacion(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Aprobado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                         await _postulacionRepository.CreateTrazabilidadFondo(postulanteRepository.SolicitudId,
                                     userId,
                                     resultObservacion.Observacion,
                                     (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                                     (int)EstadoPostulacionEnum.Aprobado
                                     );
                        var resultRepository = await _postulacionRepository.ObtenerDatosArchivoById(postulanteRepository.SolicitudId);
                        mapSolicitu = _mapper.Map<SolicituPostulancionResponseDto>(resultRepository);
                        var validateVerificate = await _postulacionRepository.GetDataVerificatePostulacion(postulanteRepository.SolicitudId);
                        var calificacionDatos = await _postulacionRepository.ConsultarCalificacionBySolId(postulanteRepository.SolicitudId);
                        if (calificacionDatos != null && calificacionDatos.EstadoId == (int)EstadoPostulacionEnum.Rechazado)
                        {
                            await _postulacionRepository.UpdateStatusCalificacionFs(request.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente);
                            return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitu, IsSuccess = false, ReturnMessage = "OK" };
                        }
                        if (validateVerificate is null)
                        {
                            if (mapSolicitu != null)
                            {
                                var obtenerEntrevista = await _postulacionRepository.ConsultarEntrevistaBySolId(postulanteRepository.SolicitudId);
                                if (obtenerEntrevista != null)
                                {
                                    await _postulacionRepository.UpdateStatusEntrevista(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Pendiente);
                                }
                                return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitu, IsSuccess = false, ReturnMessage = "OK" };
                            }
                        }
                        var updateStatusVerificate = await _postulacionRepository.UpdateStatusVerificate(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Pendiente);
                        if (updateStatusVerificate > 0)
                        {
                            return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitu, IsSuccess = false, ReturnMessage = "OK" };
                        }
                    }
                }
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay ningun archivo con el id{request.IdSolicitud}" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> RechazarCargueDocumento(Guid IdSolicitud)
        {
            if (string.IsNullOrEmpty(IdSolicitud.ToString()))
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }
            var postulanteRepository = await _postulacionRepository.ObtenerDatosArchivoById(IdSolicitud);
             await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (postulanteRepository != null)
            {
                if (postulanteRepository.EtapaProceso == EstadoPostulacionEnums.Pendiente)
                {
                    await _postulacionRepository.ActualizarEstadoArchivoPostulacion(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Rechazado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                    var resultRepository = await _postulacionRepository.ObtenerDatosArchivoById(postulanteRepository.SolicitudId);
                     await _postulacionRepository.CreateTrazabilidadFondo(
                        postulanteRepository.SolicitudId,
                        resultRepository.UserId,
                        resultRepository.Observacion,
                        (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                        (int)EstadoPostulacionEnum.Rechazado
                        );
                   var  mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(resultRepository);

                    if (mapSolicitud.EstadoProceso.Equals(EstadoPostulacionEnums.Rechazado))
                    {
                        var updateStatus = await _postulacionRepository.Update_Status_Aproved(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Inconsistente);
                        mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(updateStatus);
                        await _postulacionRepository.CreateTrazabilidadFondo(
                                postulanteRepository.SolicitudId,
                                resultRepository.UserId,
                                resultRepository.Observacion,
                                (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD,
                                (int)EstadoPostulacionEnum.Inconsistente
                                );
                    }
                    return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

                }
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Valide con el Admi" };
        }

        public async Task<Response<SolicituPostulancionResponseDto>> AnularCargoDocumento(Guid IdSolicitud)
        {
            if (string.IsNullOrEmpty(IdSolicitud.ToString()))
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }
            var postulanteRepository = await _postulacionRepository.ObtenerDatosArchivoById(IdSolicitud);
            if (postulanteRepository != null)
            {
                await _postulacionRepository.ActualizarEstadoArchivoPostulacion(postulanteRepository.SolicitudId, (int)EstadoPostulacionEnum.Anulado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                var resultRepository = await _postulacionRepository.ObtenerDatosArchivoById(postulanteRepository.SolicitudId);
                await _postulacionRepository.CreateTrazabilidadFondo(
    postulanteRepository.SolicitudId,
    resultRepository.UserId,
    resultRepository.Observacion,
    (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
    (int)EstadoPostulacionEnum.Anulado
    );
                var mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(resultRepository);
                return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = false, ReturnMessage = "OK" };

            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay ningun archivo con el id{IdSolicitud}" };
        }
        public async Task<Response<string>> ActualizarFolio(Guid IdSolicitud, int numeroFolios)
        {
            if (string.IsNullOrEmpty(IdSolicitud.ToString()))
            {
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }

            var updateArchivo = await _postulacionRepository.ActualizarN_FolioArchivoPostulacion(IdSolicitud, numeroFolios);
            if (updateArchivo > 0)
            {
                return new Response<string> { Data = "Actualizdo con exito", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se logro actualizar" };
        }
        public async Task<Response<IEnumerable<PostulanteDto>>> CrearPostulanteFondoSolidaridad(IEnumerable<PostulanteDto> solicitudPostulacion)
        {
            if (!solicitudPostulacion.Any())
            {
                return new Response<IEnumerable<PostulanteDto>> { Data = null, IsSuccess = false, ReturnMessage = "Por favor ingrese el tipo de solicitud" };
            }

            var resultMap = _mapper.Map<IEnumerable<Postulante>>(solicitudPostulacion);
             await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            var resultDataPostulante = new List<PostulanteDto>();
            foreach (var item in resultMap)
            {
                var resultRepository = await _postulacionRepository.ObtenerSolicitudPostulacionBySolicitudId(item.POS_ID_SOLICIUD);
                item.POS_ID_SOLICIUD = resultRepository.SPO_ID_SOLICITUD;
                var mapPostulante = _mapper.Map<Postulante>(item);
                var resultPostulanteRepository = await _postulacionRepository.POS_CREAR_POSTULANTES(mapPostulante);
                var postulanteMap = _mapper.Map<IEnumerable<PostulanteDto>>(resultPostulanteRepository);
                foreach (var postulante in postulanteMap)
                {
                    resultDataPostulante.Add(postulante);
                }

            }
            if (resultDataPostulante.Any())
            {
                return new Response<IEnumerable<PostulanteDto>> { Data = resultDataPostulante, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<PostulanteDto>> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide la informacion a ingresar" };
        }

        public async Task<Response<string>> CrearEntrevista(EntrevistaDto entrevista)
        {
            if (entrevista == null)
            {
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Verifique la informacion" };
            }
            var mapRepository = _mapper.Map<Entrevista>(entrevista);
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (estadoRepository.Any())
            {
                foreach (var item in estadoRepository)
                {
                    if (item.ESP_NOMBRE.Equals("PENDIENTE"))
                    {
                        var insertEntrevitas = 0;
                        mapRepository.EFS_ESTADO = item.ESP_ID;
                        var resultActividad = await _postulacionRepository.ObtenerActividadFondoSolidaridad();
                        if (resultActividad != null)
                        {
                            foreach (var actividad in resultActividad)
                            {
                                if (actividad.NOMBRE.Equals("ENTREVISTA"))
                                {
                                    mapRepository.EFS_ACTIVIDAD = actividad.ID;
                                    insertEntrevitas = await _postulacionRepository.CrearEntrenvistaFs(mapRepository);
                                }

                            }
                        }
                        if (insertEntrevitas > 0)
                        {
                            var mapPostulante = _mapper.Map<IEnumerable<Postulante>>(entrevista.postulantes);
                            if (mapPostulante.Any())
                            {
                                foreach (var postulante in mapPostulante)
                                {
                                    postulante.POS_ID_SOLICIUD = entrevista.EFS_ID_SOLICITUD;
                                    var insertPostulante = await _postulacionRepository.POS_CREAR_POSTULANTES(postulante);
                                }
                            }
                        }

                    }
                    return new Response<string> { Data = "Creado exitosamente", IsSuccess = true, ReturnMessage = "Exitoso" };
                }
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No hay estado de solicitud" };
        }

        public async Task<Response<EntresvistaResponseDto>> ObtenerEntrevitsById(Guid solId)
        {
            var resultRepository = await _postulacionRepository.ConsultarEntrevistaBySolId(solId);
            if (resultRepository != null)
            {
                var mapEntrevista = _mapper.Map<EntresvistaResponseDto>(resultRepository);
                var obtenerPostulante = await _postulacionRepository.Obtener_Postulante_By_Id_Solicitud(solId);
                if (obtenerPostulante.Any())
                {
                    var mapPostulante = _mapper.Map<IEnumerable<PostulanteResponseDto>>(obtenerPostulante);
                    mapEntrevista.postulantes = mapPostulante;
                    return new Response<EntresvistaResponseDto> { Data = mapEntrevista, IsSuccess = true, ReturnMessage = "Exitoso" };
                }
                return new Response<EntresvistaResponseDto> { Data = mapEntrevista, IsSuccess = true, ReturnMessage = "Exitoso" };
            }
            return new Response<EntresvistaResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "No hay entrevista con id de solicitud" };
        }
        public async Task<Response<IEnumerable<PostulanteResponseDto>>> ObtenerPostulanteById(Guid solId)
        {
            var obtenerPostulante = await _postulacionRepository.Obtener_Postulante_By_Id_Solicitud(solId);
            if (obtenerPostulante.Any())
            {
                var mapPostulante = _mapper.Map<IEnumerable<PostulanteResponseDto>>(obtenerPostulante);
                return new Response<IEnumerable<PostulanteResponseDto>> { Data = mapPostulante, IsSuccess = true, ReturnMessage = "Exitoso" };
            }
            return new Response<IEnumerable<PostulanteResponseDto>> { Data = null, IsSuccess = true, ReturnMessage = "Failed" };

        }
        public async Task<Response<string>> UpdateEntrevista(EntrevistaDto entrevista)
        {
            if (entrevista == null)
            {
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Verifique la informacion" };
            }

            var mapRepository = _mapper.Map<Entrevista>(entrevista);
            if (entrevista.EFS_ID == null)
            {
                var mapPostulante = _mapper.Map<IEnumerable<Postulante>>(entrevista.postulantes);
                foreach (var postulante in mapPostulante)
                {
                    postulante.POS_ID_SOLICIUD = entrevista.EFS_ID_SOLICITUD;
                    postulante.POS_FECHA_NACIMIENTO = null;
                     await _postulacionRepository.POS_CREAR_POSTULANTES(postulante);
                }
                return new Response<string> { Data = "Se logro crear los beneficiario", IsSuccess = true, ReturnMessage = "Exitoso" };

            }
            else
            {
                var insertEntrevitas = await _postulacionRepository.UpdateDataEntrevistaFs(mapRepository);
                if (insertEntrevitas > 0)
                {
                    var mapPostulante = _mapper.Map<IEnumerable<Postulante>>(entrevista.postulantes);
                    if (mapPostulante.Any())
                    {
                         await _postulacionRepository.DeletePostulanteByIdSolicitud(entrevista.EFS_ID_SOLICITUD);
                        foreach (var postulante in mapPostulante)
                        {
                            postulante.POS_ID_SOLICIUD = entrevista.EFS_ID_SOLICITUD;
                             await _postulacionRepository.POS_CREAR_POSTULANTES(postulante);
                        }
                    }
                    return new Response<string> { Data = "Actualizado exitosamente", IsSuccess = true, ReturnMessage = "Exitoso" };
                }
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No hay estado de solicitud" };
            }


        }
        public async Task<Response<SolicituPostulancionResponseDto>> AprobarEntrevista(Guid solId, Guid userId)
        {
            var resultRepository = await _postulacionRepository.ConsultarEntrevistaBySolId(solId);
            SolicituPostulancionResponseDto mapData;
            if (resultRepository == null)
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No existe entrevista con ese id solicitud" };
            }
            var obtenterData = await _postulacionRepository.ObtenerDatosEntrevitsById(solId);
            if (resultRepository.NombreEstado == "PENDIENTE")
            {
                var updateStatus = await _postulacionRepository.UpdateStatusEntrevista(solId, (int)EstadoPostulacionEnum.Aprobado);
                if (updateStatus <= 0)
                {
                    return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No se puedo actualizar el estado, verifique que sucedio" };
                }
                var dataEntrevista = await _postulacionRepository.ConsultarEntrevistaBySolId(solId);
                await _postulacionRepository.CreateTrazabilidadFondo(
                    solId, userId, dataEntrevista.EFS_OBSERVACION,
                  (int)EtapaPostulacionEnum.ENTREVISTA,
                  (int)EstadoPostulacionEnum.Aprobado);
                mapData = _mapper.Map<SolicituPostulancionResponseDto>(obtenterData);
                 await _postulacionRepository.CrearCalificacion(solId, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.CALIFICACIÓN, userId);
                return new Response<SolicituPostulancionResponseDto> { Data = mapData, IsSuccess = false, ReturnMessage = "OK" };
            }
            if (resultRepository.NombreEstado.Equals("INCONSISTENTE"))
            {
                var updateStatus = await _postulacionRepository.UpdateStatusEntrevista(solId, (int)EstadoPostulacionEnum.Aprobado);
                if (updateStatus <= 0)
                {
                    return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No se puedo actualizar el estado, verifique que sucedio" };
                }
                var dataEntrevista = await _postulacionRepository.ConsultarEntrevistaBySolId(solId);

                mapData = _mapper.Map<SolicituPostulancionResponseDto>(obtenterData);
                 await _postulacionRepository.CreateTrazabilidadFondo(
                        solId, userId, dataEntrevista.EFS_OBSERVACION, (int)EtapaPostulacionEnum.ENTREVISTA, (int)EstadoPostulacionEnum.Aprobado);
                var obtenerEntrevista = await _postulacionRepository.ConsultarCalificacionBySolId(mapData.IdSolicitud);
                if (obtenerEntrevista != null)
                {
                     await _postulacionRepository.UpdateStatusCalificacionFs(mapData.IdSolicitud, (int)EstadoPostulacionEnum.Aprobado);
                }
                return new Response<SolicituPostulancionResponseDto> { Data = mapData, IsSuccess = false, ReturnMessage = "OK" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "La entrevista tiene que estar en estado PENDIENTE" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> AnularEntrevista(Guid solId, Guid userId)
        {
            var resultRepository = await _postulacionRepository.ConsultarEntrevistaBySolId(solId);
            if (resultRepository != null)
            {
               await _postulacionRepository.UpdateStatusEntrevista(solId, (int)EstadoPostulacionEnum.Anulado);
                await _postulacionRepository.CreateTrazabilidadFondo(
                    solId, userId, resultRepository.EFS_OBSERVACION,
                    (int)EtapaPostulacionEnum.ENTREVISTA,
                    (int)EstadoPostulacionEnum.Anulado);
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "Anulado Exitosamente!" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "Valide la informacion, no se pudo anular" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> RechazarEntrevista(Guid IdSolicitud, Guid userId)
        {
            var resultEntrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(IdSolicitud);
            if (resultEntrevista == null)
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay se pudo actualizar la entrevista con el id{IdSolicitud}" };
            }
            if (resultEntrevista.EtapaProceso.Equals("PENDIENTE"))
            {
                var updateStatu = await _postulacionRepository.UpdateStatusEntrevista(resultEntrevista.SolicitudId, (int)EstadoPostulacionEnum.Anulado);
                if (updateStatu == 0)
                {
                    return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay se pudo actualizar la entrevista con el id{IdSolicitud}" };
                }
            

                ///TRAZABILIDAD
                 await _postulacionRepository.CreateTrazabilidadFondo(
                    IdSolicitud, userId, resultEntrevista.Observacion,
                     (int)EtapaPostulacionEnum.ENTREVISTA,
                    (int)EstadoPostulacionEnum.Rechazado);

                await _postulacionRepository.ActualizarEstadoArchivoPostulacion(resultEntrevista.SolicitudId, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);

                ///TRAZABILIDAD
                await _postulacionRepository.CreateTrazabilidadFondo(
                   IdSolicitud, userId, resultEntrevista.Observacion,
                    (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                   (int)EstadoPostulacionEnum.Inconsistente);

                var resultRepository = await _postulacionRepository.ObtenerDatosArchivoById(resultEntrevista.SolicitudId);
                var mapSolicitud = _mapper.Map<SolicituPostulancionResponseDto>(resultRepository);
                return new Response<SolicituPostulancionResponseDto> { Data = mapSolicitud, IsSuccess = true, ReturnMessage = $"Rechazado exitosamente" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay nignuna entrevita en estado PENDIENTE con el id{IdSolicitud}" };
        }
        public async Task<Response<ClasficacionDto>> CrearClasificacion(ClasficacionDto clasficacion)
        {
            if (clasficacion == null)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Verifique la informacion" };
            }
            if (clasficacion.ImputabilidadAlServicio > 100 || clasficacion.ImputabilidadAlServicio < 0)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "El valor imput servico tiene que estar entre 0 y 100" };
            }
            if (clasficacion.PorcentajePostulacion > 100 || clasficacion.PorcentajePostulacion < 0)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "El valor porcentaje de postulacion tiene que estar entre 0 y 100" };
            }
            if (clasficacion.Hecho > 100 || clasficacion.Hecho < 0)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "El valor del hecho tiene que estar entre 0 y 100" };
            }
            var validarCalificacion = await _postulacionRepository.ConsultarCalificacionBySolId(clasficacion.SolicitudId);
            if (validarCalificacion != null)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Ya se guardo una calificacion con este id de solciitud por favor valide" };
            }
            var mapRepository = _mapper.Map<Clasificacion>(clasficacion);
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (estadoRepository.Any())
            {
                //var imputServicioResult = mapRepository.ImputabilidadAlServicio * imputServicio;
                //var porcentajeResult = mapRepository.PorcentajePostulacion * porcentaje;
                //var hechoResult = mapRepository.Hecho * hecho;
                //mapRepository.Puntaje = imputServicioResult + porcentajeResult + hechoResult;
                foreach (var item in estadoRepository)
                {
                    if (item.ESP_NOMBRE.Equals("PENDIENTE"))
                    {
                        var insertClasificacion = new Clasificacion();
                        mapRepository.EstadoId = item.ESP_ID;
                        var resultActividad = await _postulacionRepository.ObtenerActividadFondoSolidaridad();
                        if (resultActividad != null)
                        {
                            foreach (var actividad in resultActividad)
                            {
                                if (actividad.NOMBRE.Equals("CALIFICACIÓN"))
                                {
                                    mapRepository.Actividad = actividad.ID;
                                    insertClasificacion = await _postulacionRepository.CrearCalificacionFs(mapRepository);
                                    if (insertClasificacion != null)
                                    {
                                        var mapClasificacion = _mapper.Map<ClasficacionDto>(insertClasificacion);
                                        return new Response<ClasficacionDto> { Data = mapClasificacion, IsSuccess = true, ReturnMessage = "Creado exitosamente" };

                                    }
                                    return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Algo sucedio por favor valide" };

                                }
                            }
                        }
                    }
                }
            }
            return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "No hay estado de solicitud" };
        }
        public async Task<Response<CalificacionResponseDto>> ConsultarCalificacionByIdSolcitud(Guid solId)
        {
            var resultRepository = await _postulacionRepository.ConsultarCalificacionBySolId(solId);
            if (resultRepository != null)
            {
                var mapEntrevista = _mapper.Map<CalificacionResponseDto>(resultRepository);
               await _postulacionRepository.Obtener_Postulante_By_Id_Solicitud(solId);
                return new Response<CalificacionResponseDto> { Data = mapEntrevista, IsSuccess = true, ReturnMessage = "Exitoso" };
            }
            return new Response<CalificacionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "No hay entrevista con id de solicitud" };
        }
        public async Task<Response<ClasficacionDto>> UpdateClasificacion(ClasficacionDto clasficacion)
        {
            if (clasficacion == null)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Verifique la informacion" };
            }
            if (clasficacion.PorcentajePostulacion > 100 || clasficacion.PorcentajePostulacion < 0)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "El valor porcentaje de postulacion tiene que estar entre 0 y 100" };
            }

            var validarCalificacion = await _postulacionRepository.ConsultarCalificacionBySolId(clasficacion.SolicitudId);
            if (validarCalificacion == null)
            {
                return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "No existe ningun con este id para actualizar" };
            }
            var mapRepository = _mapper.Map<Clasificacion>(clasficacion);

            mapRepository.Puntaje = mapRepository.PorcentajePostulacion;
            var insertClasificacion = await _postulacionRepository.UpdateCalificacionFs(mapRepository);
            if (insertClasificacion != null)
            {
                var mapClasificacion = _mapper.Map<ClasficacionDto>(insertClasificacion);
                return new Response<ClasficacionDto> { Data = mapClasificacion, IsSuccess = true, ReturnMessage = "Creado exitosamente" };
            }
            return new Response<ClasficacionDto> { Data = null, IsSuccess = false, ReturnMessage = "Algo sucedio por favor valide" };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> AprobarCalificacion(Guid solId, Guid userId)
        {
            var resultRepository = await _postulacionRepository.ConsultarCalificacionBySolId(solId);
            if (resultRepository == null)
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No existe entrevista con ese id solicitud" };
            }

            var updateStatus = await _postulacionRepository.UpdateStatusCalificacionFs(solId, (int)EstadoPostulacionEnum.Aprobado);
            if (updateStatus <= 0)
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No se puedo actualizar el estado, verifique que sucedio" };
            }
             await _postulacionRepository.CreateTrazabilidadFondo(
                solId, userId, resultRepository.Observacion, (int)EtapaPostulacionEnum.CALIFICACIÓN,
                (int)EstadoPostulacionEnum.Aprobado);
            var obtenterData = await _postulacionRepository.ObtenerDatosCalificacionById(solId);
            var mapData = _mapper.Map<SolicituPostulancionResponseDto>(obtenterData);
            return new Response<SolicituPostulancionResponseDto> { Data = mapData, IsSuccess = true, ReturnMessage = "Aprobado Exitosamente!" };

        }
        public async Task<Response<SolicituPostulancionResponseDto>> AnularCalificacion(Guid solId, Guid userId)
        {
            var resultRepository = await _postulacionRepository.ConsultarCalificacionBySolId(solId);
            var estadoRepository = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            if (resultRepository == null)
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay calificacion existente" };

            }
            foreach (var item in estadoRepository)
            {
                if (item.ESP_NOMBRE.Equals("ANULADO"))
                {
                    var updateStatus = await _postulacionRepository.UpdateStatusCalificacionFs(solId, item.ESP_ID);
                    if (updateStatus <= 0)
                    {
                         await _postulacionRepository.CreateTrazabilidadFondo(
                            solId, userId, resultRepository.Observacion, (int)EtapaPostulacionEnum.CALIFICACIÓN, (int)EstadoPostulacionEnum.Aprobado);
                        return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "No se puedo actualizar el estado, verifique que sucedio" };
                    }
                    var obtenterData = await _postulacionRepository.ObtenerDatosCalificacionById(solId);
                    var mapData = _mapper.Map<SolicituPostulancionResponseDto>(obtenterData);
                    return new Response<SolicituPostulancionResponseDto> { Data = mapData, IsSuccess = true, ReturnMessage = "Anulado Exitosamente!" };
                }
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "Por favor vefique, no se pudo anular" };
        }

        public async Task<Response<SolicituPostulancionResponseDto>> RechazarCalificacion(Guid IdSolicitud, Guid userId)
        {
            if (string.IsNullOrEmpty(IdSolicitud.ToString()))
            {
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Por favor valide el id de la solicitud" };
            }
            var estados = await _postulacionRepository.ESP_OBTENER_ESTADO_SOLICITUD();
            var postulanteRepository = await _postulacionRepository.ObtenerDatosCalificacionById(IdSolicitud);
            if (postulanteRepository != null)
            {
                var resultRepository = await _postulacionRepository.ConsultarCalificacionBySolId(IdSolicitud);

                int updateStatus;
                if (postulanteRepository.EtapaProceso == "PENDIENTE")
                {
                    foreach (var item in estados)
                    {
                        if (item.ESP_NOMBRE.Equals("RECHAZADO"))
                        {
                            var createTrazabilidad = await _postulacionRepository.CreateTrazabilidadFondo(
                                IdSolicitud, userId, resultRepository.Observacion, (int)EtapaPostulacionEnum.CALIFICACIÓN,
                                (int)EstadoPostulacionEnum.Rechazado);
                            updateStatus = await _postulacionRepository.UpdateStatusCalificacionFs(postulanteRepository.SolicitudId, item.ESP_ID);
                            if (updateStatus < 0)
                            {
                                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "No se puedo rechazar la solicitud" };

                            }
                        }
                    }
                }
                foreach (var item in estados)
                {
                    if (item.ESP_NOMBRE.Equals("INCONSISTENTE"))
                    {
                        await _postulacionRepository.CreateTrazabilidadFondo(
                            IdSolicitud, userId, resultRepository.Observacion, (int)EtapaPostulacionEnum.ENTREVISTA,
                            (int)EstadoPostulacionEnum.Inconsistente);
                        var updateArchivo = await _postulacionRepository.UpdateStatusEntrevista(postulanteRepository.SolicitudId, item.ESP_ID);
                        var dataEntrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(postulanteRepository.SolicitudId);
                        var mapData = _mapper.Map<SolicituPostulancionResponseDto>(dataEntrevista);
                        if (updateArchivo > 0)
                        {
                            return new Response<SolicituPostulancionResponseDto> { Data = mapData, IsSuccess = true, ReturnMessage = $"Rechazado exitosamente" };
                        }
                    }
                }
                return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "El cargue de documento no se encuentra en estado PENDIENTE" };
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = false, ReturnMessage = $"No hay ningun archivo con el id{IdSolicitud}" };
        }
        public async Task<Response<string>> ActualizarObservacionArchivos(ActualizarObservacionDto observacion)
        {
            var resultArchivo = await _postulacionRepository.ObtenerDatosArchivoById(observacion.SolicitudId);
            if (resultArchivo == null)
            {
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No existe ningun archivo con esa solicitud a actualzar" };
            }
            var update = await _postulacionRepository.UpdateObservationArchivoFs(observacion.SolicitudId, observacion.Observacion);
            if (update <= 0)
            {
                return new Response<string> { Data = null, IsSuccess = true, ReturnMessage = "Por favor valide, no se actualizo la informacion" };
            }
            return new Response<string> { Data = "Actualizado exitosamente", IsSuccess = true, ReturnMessage = "Actualizado exitosamente" };
        }
        public async Task<Response<ObservacionArchivoDto>> ConsultarObservacionArchivos(Guid IdSolicitud)
        {
            var resultArchivo = await _postulacionRepository.ObtenerDatosArchivoById(IdSolicitud);
            if (resultArchivo == null)
            {
                return new Response<ObservacionArchivoDto> { Data = null, IsSuccess = false, ReturnMessage = "No existe ningun archivo con esa solicitud a actualzar" };
            }
            var update = await _postulacionRepository.ConsultarObservationArchivoFsBySolId(IdSolicitud);
            if (update == null)
            {
                return new Response<ObservacionArchivoDto> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos de observacion" };
            }
            var resultMap = _mapper.Map<ObservacionArchivo, ObservacionArchivoDto>(update);
            return new Response<ObservacionArchivoDto> { Data = resultMap, IsSuccess = true, ReturnMessage = "Consultado exitosamente" };
        }
        public async Task<Response<IEnumerable<EtapaPostulacionDto>>> ObtenerEtapaPostulacion()
        {
            var result = await _postulacionRepository.ObtenerActividadFondoSolidaridad();
            var mapper = _mapper.Map<IEnumerable<EtapaPostulacionDto>>(result);
            return new Response<IEnumerable<EtapaPostulacionDto>> { Data = mapper, IsSuccess = false, ReturnMessage = "Exitoso" };
        }
        public async Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoIntegrante()
        {
            var result = await _postulacionRepository.ObtenerTipoIntegrante();
            var mapper = _mapper.Map<IEnumerable<TipoICatalogoPostulacionDto>>(result);
            return new Response<IEnumerable<TipoICatalogoPostulacionDto>> { Data = mapper, IsSuccess = false, ReturnMessage = "Exitoso" };
        }
        public async Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoParentescoPostulacion()
        {
            var result = await _postulacionRepository.ObtenerTipoParentescoPostulacion();
            var mapper = _mapper.Map<IEnumerable<TipoICatalogoPostulacionDto>>(result);
            return new Response<IEnumerable<TipoICatalogoPostulacionDto>> { Data = mapper, IsSuccess = false, ReturnMessage = "Exitoso" };
        }
        public async Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoAyudaTecnica()
        {
            var result = await _postulacionRepository.ObtenerTipoAyudaTecnica();
            var mapper = _mapper.Map<IEnumerable<TipoICatalogoPostulacionDto>>(result);
            return new Response<IEnumerable<TipoICatalogoPostulacionDto>> { Data = mapper, IsSuccess = false, ReturnMessage = "Exitoso" };
        }
        public async Task<Response<IEnumerable<TipoICatalogoPostulacionDto>>> ObtenerTipoDiscapacidaPostulacion()
        {
            var result = await _postulacionRepository.ObtenerTipoDiscapacidaPostulacion();
            var mapper = _mapper.Map<IEnumerable<TipoICatalogoPostulacionDto>>(result);
            return new Response<IEnumerable<TipoICatalogoPostulacionDto>> { Data = mapper, IsSuccess = false, ReturnMessage = "Exitoso" };
        }
        public async Task<int> CreateValidateVerificate(Guid idSolicitud, int etapa, Guid userId)
        {
            var createVerification = 0;
            if (etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES)
            {
                createVerification = await _postulacionRepository.CreateVerification(idSolicitud, etapa, userId);
            }
            if (etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS)
            {
                createVerification = await _postulacionRepository.CreateVerification(idSolicitud, etapa, userId);
            }
            if (etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL)
            {
                createVerification = await _postulacionRepository.CreateVerification(idSolicitud, etapa, userId);
            }
            return createVerification;
        }

        public async Task<Response<string>> UpdateDateVerificatePostulacion(VeficatePostulacionDto request)
        {
            var updateDate = await _postulacionRepository.UpdateDataVericatePostulacion(request.IdSolicitud, request.Observacion, request.IdEtapa);
            if (updateDate > 0)
            {
                return new Response<string> { Data = "Actualizizado extitosamente", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> AprovedVerificateNovedadesFs(VeficateFondoDto verificate, Guid userId)
        {
            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificate(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Aprobado);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(verificate.IdSolicitud);
                 await _postulacionRepository.CreateTrazabilidadFondo(
                        verificate.IdSolicitud,
                        userId,
                       dataVf.Observacion,
                        (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES,
                        (int)EstadoPostulacionEnum.Aprobado
                        );
                if (verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS || verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL)
                {

                    await _postulacionRepository.ActualizarEtapaVerificacion(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Bloqueado, verificate.IdEtapa);
                }
                else
                {
                    var validateExist = await _postulacionRepository.ConsultarEntrevistaBySolId(verificate.IdSolicitud);
                    if (validateExist is null)
                    {
                        await _postulacionRepository.CrearEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.ENTREVISTA, userId);
                    }
                    else
                    {
                        await _postulacionRepository.UpdateStatusEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente);
                    }
                }
                return new Response<string> { Data = "Aprobado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> AprovedVerificateEmbargo(VeficateFondoDto verificate, Guid userId)
        {
            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificate(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Aprobado);
            if (aprovedVerifcate > 0)
            {

                var dataVf = await _postulacionRepository.GetDataVerificate(verificate.IdSolicitud);
                await _postulacionRepository.CreateTrazabilidadFondo(
                    verificate.IdSolicitud,
                    userId,
                    dataVf.Observacion,
                    (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS,
                    (int)EstadoPostulacionEnum.Aprobado
                    );
                if (verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES || verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL)
                {

                    await _postulacionRepository.ActualizarEtapaVerificacion(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Bloqueado, verificate.IdEtapa);
                }
                else
                {
                    var validateExist = await _postulacionRepository.ConsultarEntrevistaBySolId(verificate.IdSolicitud);
                    if (validateExist is null)
                    {
                        await _postulacionRepository.CrearEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.ENTREVISTA, userId);
                    }
                    else
                    {
                        await _postulacionRepository.UpdateStatusEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente);
                    }
                }
                return new Response<string> { Data = "Aprobado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> RechazarVerificateEmbargosPostulacion(VeficateFondoDto veficate, Guid userId)
        {

            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificateAoR(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Rechazado, veficate.IdEtapa);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(veficate.IdSolicitud);

                 await _postulacionRepository.CreateTrazabilidadFondo(
                      veficate.IdSolicitud,
                             userId,
                          dataVf.Observacion,
                              (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS,
        (int)EstadoPostulacionEnum.Rechazado
        );
                await _postulacionRepository.ActualizarEstadoArchivoPostulacion(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> AnularVerificateEmbargosPostulacion(VeficateFondoDto veficate, Guid userId)
        {

            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificateAoR(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Anulado, veficate.IdEtapa);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(veficate.IdSolicitud);

                 await _postulacionRepository.CreateTrazabilidadFondo(
                      veficate.IdSolicitud,
                             userId,
                          dataVf.Observacion,
                              (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS,
        (int)EstadoPostulacionEnum.Anulado
        );
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> RechazarVerificatePostulacion(Guid idSolicitud, Guid userId)
        {
            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificate(idSolicitud, (int)EstadoPostulacionEnum.Rechazado);
            if (aprovedVerifcate > 0)
            {
                 await _postulacionRepository.ActualizarEstadoArchivoPostulacion(idSolicitud, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> AnularVerificatePostulacion(Guid idSolicitud, Guid userId)
        {

            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificate(idSolicitud, (int)EstadoPostulacionEnum.Anulado);
            if (aprovedVerifcate > 0)
            {
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        /// <summary>
        /// VERIFICACION DE CUENTA INDIVIDUALES
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        /// 
        public async Task<Response<string>> AprovedVerificateCuentasIndividuales(VeficateFondoDto verificate, Guid userId)
        {
            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificate(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Aprobado);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(verificate.IdSolicitud);

                 await _postulacionRepository.CreateTrazabilidadFondo(
                    verificate.IdSolicitud,
                    userId,
                    dataVf.Observacion,
                    (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL,
                    (int)EstadoPostulacionEnum.Aprobado
                    );
                if (verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES || verificate.IdEtapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS)
                {
                    await _postulacionRepository.ActualizarEtapaVerificacion(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Bloqueado, verificate.IdEtapa);
                }
                else
                {
                    var validateExist = await _postulacionRepository.ConsultarEntrevistaBySolId(verificate.IdSolicitud);
                    if (validateExist is null)
                    {
                        await _postulacionRepository.CrearEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente, (int)EtapaPostulacionEnum.ENTREVISTA, userId);
                    }
                    else
                    {
                        await _postulacionRepository.UpdateStatusEntrevista(verificate.IdSolicitud, (int)EstadoPostulacionEnum.Pendiente);
                    }
                }
                return new Response<string> { Data = "Aprobado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> RechazarVerificateCuentaIndividuales(VeficateFondoDto veficate, Guid userId)
        {

            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificateAoR(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Rechazado, veficate.IdEtapa);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(veficate.IdSolicitud);
                await _postulacionRepository.CreateTrazabilidadFondo(
                     veficate.IdSolicitud,
                            userId,
                         dataVf.Observacion,
                             (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL,
       (int)EstadoPostulacionEnum.Rechazado
       );
                await _postulacionRepository.ActualizarEstadoArchivoPostulacion(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<string>> AnularVerificateCuentasIndividuales(VeficateFondoDto veficate, Guid userId)
        {

            var aprovedVerifcate = await _postulacionRepository.UpdateStatusVerificateAoR(veficate.IdSolicitud, (int)EstadoPostulacionEnum.Anulado, veficate.IdEtapa);
            if (aprovedVerifcate > 0)
            {
                var dataVf = await _postulacionRepository.GetDataVerificate(veficate.IdSolicitud);

                 await _postulacionRepository.CreateTrazabilidadFondo(
                      veficate.IdSolicitud,
                             userId,
                          dataVf.Observacion,
                              (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL,
        (int)EstadoPostulacionEnum.Anulado
        );
                return new Response<string> { Data = "Rechazado exitoxamente", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se pudo actualizar la data" };
        }
        public async Task<Response<IEnumerable<VerificatePostulacionDto>>> GetDataVerificatePostulacion(Guid idSolicitud)
        {

            var dataVerificate = await _postulacionRepository.GetDataVerificatePostulacion(idSolicitud);
            if (dataVerificate is not null)
            {
                var mapRsult = _mapper.Map<IEnumerable<VerificatePostulacionDto>>(dataVerificate);
                return new Response<IEnumerable<VerificatePostulacionDto>> { Data = mapRsult, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<IEnumerable<VerificatePostulacionDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se logro consultar" };
        }

        public async Task<Response<List<StagePostulationDto>>> GetStagePostulacion(Guid idSolicitud)
        {
            var stageList = await _postulacionRepository.ObtenerTrazabilidadFondo(idSolicitud);
            if (stageList.Any())
            {
                var StageMap = _mapper.Map<List<StagePostulationDto>>(stageList);

                foreach (var stage in StageMap)
                {
                    var date = stageList.Where(x => x.Status == stage.Status
                    && x.Observation == stage.Observation && x.UserName == stage.UserName)
                        .Select(x => x.DateCreated).FirstOrDefault();
                    stage.DateCreated = date.ToString("G");
                }

                return new Response<List<StagePostulationDto>> { Data = StageMap, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<List<StagePostulationDto>> { Data = null, IsSuccess = false, ReturnMessage = "No existe trazabilidad con ese id solicitud" };

        }
        public async Task<FileResult> GetReporteGenralRanking(int idConvocatoria)
        {
            var dataReport = await GetReporteRanking(idConvocatoria);
            if (dataReport.Data.Any())
            {

                var dataRport = dataReport.Data.ToList();

                var listRepor = new List<ReportRankingDto>();
                listRepor = dataRport;

                var dataColumun = new List<ColumnData>();
                var column = new List<ReportRankingDto>();

                var columnNames = ObtenerPropiedades(typeof(ReportRankingDto));
                foreach (var item in listRepor)
                {
                    var rowData = new ColumnData
                    {
                        Data = columnNames.Select(name => item.GetType().GetProperty(name).GetValue(item, null)?.ToString()).ToList()
                    };
                    dataColumun.Add(rowData);
                }

                var data = new ReportRequestExcelDto()
                {
                    ColumnName = columnNames.ToArray(),
                    ColumnDataList = dataColumun,
                };

                var result = await _reportesExcelLogic.GenerarExcelRanking(data);
                return result;
            }
            return null;
        }

        public static List<string> ObtenerPropiedades(Type tipo)
        {
            var ColumnData = new List<string>();
            PropertyInfo[] columns = tipo.GetProperties();
            foreach (var item in columns)
            {
                ColumnData.Add(item.Name);
            }
            return ColumnData;
        }
        #region reporte postulacion
        public async Task<Response<IEnumerable<ReportRankingDto>>> GetReporteRanking(int idConvocatoria)
        {
            var getAllSolicitud = await _postulacionRepository.GetAllSolicitud(idConvocatoria);
            var reportRanking = new List<ReportRankingDto>();
            if (getAllSolicitud.Any())
            {
                foreach (var item in getAllSolicitud)
                {
                    var dataCliente = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TipoDocumentoId, item.NumeroDocumento);
                    var data = await _iClienteRequestProvider.ObtenerClientesByNumeroIdentificacion(item.NumeroDocumento);
                    var dataCuenta = dataCliente?.Data.Cuentas;
                    var listDeparment = this._mapper.Map<IEnumerable<DepartamentoDto>>(await this._catalogsRepository.ObtenerDepartamentos());

                    var listCity = this._mapper.Map<IEnumerable<Ciudad>>(await this._catalogsRepository.ObtenerCiudades());
                    var deparmentName = listDeparment.Where(x => x.Id == dataCliente.Data.IdDepartamentoResidencia).Select(x => x.Nombre).FirstOrDefault();

                    var cityName = listCity.Where(x => x.DPC_ID == dataCliente.Data.IdCiudadResidencia).Select(x => x.DPC_NOMBRE).FirstOrDefault();
                    decimal valorBloqueoReintegros = 0;
                    decimal totalBloqueo = 0;
                    decimal totalSaldo = 0;

                    var cuentaAviables = dataCuenta.Where(x => x.EstadoCuenta != "CANCELADA" && x.TipoCuenta == "CUENTA INDIVIDUAL SOLUCIÓN VIVIENDA").ToList();
                    if (cuentaAviables?.Count > 0)
                    {
                        var valorBloqueoEmbargo = cuentaAviables.Select(x => x.ConceptosEmbargosBloqueos).ToList();
                        foreach (var bloqueo in valorBloqueoEmbargo)
                        {
                            if (bloqueo?.Count > 0)
                            {
                                var dataClienteBloque = bloqueo.Where(x => x.EstadoBloqueo == "VIGENTE" && x.ValorAcumulado > 0)
                                     .Select(x => x.ValorAcumulado);
                                totalBloqueo = dataClienteBloque.Sum();
                                break;
                            }
                        }
                        var dataConcepto = cuentaAviables.Select(x => x.Conceptos).ToList();
                        foreach (var bloqueo in dataConcepto)
                        {
                            if (bloqueo?.Count > 0)
                            {
                                var dataBloqueoReinetgro = bloqueo.Where(x => x.ValorBloqueoEmbargo > 0)
                                    .Select(x => x.ValorBloqueoEmbargo);
                                valorBloqueoReintegros = dataBloqueoReinetgro.Sum();
                                break;
                            }
                        }
                        if (valorBloqueoReintegros > 0)
                        {
                            totalBloqueo = +valorBloqueoReintegros;
                        }
                        var dataValorSaldo = cuentaAviables.Select(x => x.ConceptosSaldosBloqueos).ToList();
                        foreach (var saldo in dataValorSaldo)
                        {
                            if (saldo?.Count > 0)
                            {
                                var resultSaldo = saldo.Select(x => x.SaldoConceptoCuenta);
                                totalSaldo = resultSaldo.Sum();
                                break;
                            }
                        }
                        if (totalSaldo < totalBloqueo)
                        {
                            totalSaldo = 0;
                        }
                        else
                        {
                            totalSaldo -= totalBloqueo;
                        }
                    }

                    var getAllEntrevits = await _postulacionRepository.ConsultarEntrevistaBySolId(item.IdSolicitud);
                    var getAllCalifications = await _postulacionRepository.ConsultarCalificacionBySolId(item.IdSolicitud);
                    var postulantesSolicitud = await _postulacionRepository.Consultar_Postulacion_By_Id(item.IdSolicitud);

                    var imputServicio = 0.65M;
                    var porcentaje = 0.30M;
                    var hecho = 0.05M;
                    decimal resultSumMuerteDiscapacidad = 0;
                    decimal imputServicioResult = 0;
                    decimal porcentajeResult = 0;
                    decimal hechoResult = 0;
                    var ImputabilidadServicio = "";
                    var porcentaje1 = "";
                    var muerteDiscapacidad = "";
                    double criterio3 = 0;
                    if (getAllCalifications is not null)
                    {
                        ImputabilidadServicio = getAllCalifications.ImputabilidadAlServicio switch
                        {
                            100 => ImputabilidadServicioDisminucionCapacidad.ConsecuenciaCombate,
                            90 => ImputabilidadServicioDisminucionCapacidad.ServicioCausaORazon,
                            10 => ImputabilidadServicioDisminucionCapacidad.ServicioNoCusa + ": " + getAllCalifications.PorcentajeDisminucion,
                            _ => "",
                        };
                        porcentaje1 = getAllCalifications.PorcentajePostulacion switch
                        {
                            100 => PorcentajePostulacion.Nuclueox100,
                            50 => PorcentajePostulacion.Nucleo,
                            _ => "",
                        };
                        muerteDiscapacidad = getAllCalifications.Hecho switch
                        {
                            100 => CatalogPostulacionDto.MuerteAfiliado,
                            0 => CatalogPostulacionDto.DisminucionPsicofisica,
                            _ => "",
                        };
                        resultSumMuerteDiscapacidad = getAllCalifications.Hecho + getAllCalifications.Puntaje;
                        imputServicioResult = getAllCalifications.ImputabilidadAlServicio * imputServicio;
                        porcentajeResult = getAllCalifications.PorcentajePostulacion * porcentaje;
                        hechoResult = getAllCalifications.Hecho * hecho;
                        getAllCalifications.Puntaje = imputServicioResult + porcentajeResult + hechoResult;
                        if (item.ModalidadSolicitud != ModalidadFondoSolidaridad.FallecimientoAfiliado)
                        {
                            criterio3 = getAllCalifications.PorcentajeDisminucion * 0.05;
                        }
                    }
                    if (item.ModalidadSolicitud == ModalidadFondoSolidaridad.FallecimientoAfiliado)
                    {
                        criterio3 = 5;
                    }
                    var resultCalificacion = 0;
                    if ((int)imputServicioResult + (int)porcentajeResult + (int)criterio3 > 0)
                    {
                        resultCalificacion = (int)imputServicioResult + (int)porcentajeResult + (int)criterio3;
                    }
                    var reportObject = new ReportRankingDto()
                    {
                        Consecutivo = item.ConsecutivoSolicitud,
                        NoRadicado = item.NoRadicado,
                        NombreAfiliado = item.NombreAfiliado,
                        TipoDocumento = item.TipoDocumento,
                        NumeroDocumento = item.NumeroDocumento,
                        Fuerza = item.Fuerza,
                        UnidadEjecutora = item.UnidadEjecutora,
                        Categoria = item.Categoria,
                        Ciudad1 = item.Ciudad1,
                        Ciudad2 = item.Ciudad2,
                        Ciudad3 = item.Ciudad3,
                        Fecha = item.Fecha,
                        Hora = item.Hora,
                        MuerteDiscapacidad = getAllCalifications?.HechoDescripcion is null ? "N/A" : getAllCalifications?.HechoDescripcion,
                        ImputabilidadServicio = ImputabilidadServicio,
                        Criterio1 = getAllCalifications?.ImputabilidadAlServicio <= 0 ? 0 : (int)imputServicioResult,
                        PorcentajePostulacionDescripcion = porcentaje1,
                        Criterio2 = getAllCalifications?.PorcentajePostulacion <= 0 ? 0 : (int)porcentajeResult,
                        PorcentajeMuerteDiscapacidad = muerteDiscapacidad,
                        Criterio3 = criterio3,
                        DisfrutePension = item?.DisfrutePension,
                        Calificacion = resultCalificacion,
                        Resolucion = getAllCalifications?.DatosResolucion is null ? "N/A" : getAllCalifications?.DatosResolucion,
                        PoseeVivienda = getAllEntrevits?.EFS_VIVIENDA_PROPIA is null ? "N/A" : getAllEntrevits?.EFS_VIVIENDA_PROPIA,
                        CausalRetiro = getAllCalifications?.TipoImputabilidad,
                        CausalObservacion = getAllEntrevits?.EFS_INFORMATIVO_ADMIN,
                        SaldoCuentas = (double)totalSaldo,
                        ValorBloqueo = totalBloqueo,
                        ValorSubsidio = 0,
                        ValorCaja = 0,
                        Favorecido = "NO",
                        Mecanimos = item.ModalidadSolucionVivienda,
                        Direccion = dataCliente.Data.Direccion,
                        Telefono = dataCliente.Data.Telefono,
                        Email = dataCliente.Data.Correo,
                        Barrio = dataCliente.Data.Barrio,
                        Ciudad = cityName,
                        Departamento = deparmentName
                    };
                    if (postulantesSolicitud.Any())
                    {

                        if (postulantesSolicitud.Any() && postulantesSolicitud.Count() == 1)
                        {
                            foreach (var postulante in postulantesSolicitud)
                            {
                                var porcentajeCesantia = postulante.PorcentajeCesantias;
                                var porcentajeAporte = postulante.PorcentajeAportes;
                                var porcentajePostulacion = postulante.PorcentajePostulacion;
                                var porcentajeBeneficiario = (postulante.PorcentajeAportes + postulante.PorcentajeCesantias + postulante.PorcentajePostulacion) / 3;
                                var tipopeneficario = postulante.TipoParentescoId == "NINGUNO" ? "MISMO BENEFICIARIO" : postulante.TipoParentescoId;
                                var identificacionBeneficiario = postulante.nombreIdentificacion;
                                var nombreCompleto = postulante.NombrePostulante + " " + postulante.ApellidoPostulante;
                                var afeccionFisica = postulante.AfectacionFisica;
                                var afeccionPsicologica = postulante.AfectacionSensorial;
                                var enfermedadEspecifica = postulante.EnfermedadesEspecificas;
                                var recibeTratamiento = postulante.TratamientoMedico;
                                var requieremeMedicamento = postulante.MedicamentoPermanente;
                                var requiereAyuda = postulante.AyudaTecnica;
                                reportObject.PorcentajeCesantías = porcentajeCesantia;
                                reportObject.PorcentajeAportes = porcentajeAporte;
                                reportObject.PorcentajePostulacion = porcentajePostulacion;
                                reportObject.Particip​aciónBeneficiarios = porcentajeBeneficiario;
                                reportObject.TipoIdBeneficiarios = identificacionBeneficiario;
                                reportObject.NrosIdentBeneficiarios = postulante.NumeroDocumentoPostulante;
                                reportObject.ParentescoBeneficiarios = tipopeneficario;
                                reportObject.NombresBeneficiarios = nombreCompleto;
                                reportObject.AfectacionFisica = afeccionFisica;
                                reportObject.AfectacionSensorial = afeccionPsicologica;
                                reportObject.EnfermedadEspecifica = enfermedadEspecifica;
                                reportObject.RecibeTratamiento = recibeTratamiento;
                                reportObject.RequiereMedicamento = requieremeMedicamento;
                                reportObject.RequiereAyuda = requiereAyuda;
                            }
                            reportRanking.Add(reportObject);
                        }
                        else if (postulantesSolicitud.Count() > 1)
                        {
                            var listPorcentajeParticipacionAporte = new List<int?>();
                            var listPorcentajeParticipacionCesantias = new List<int?>();
                            var objectData = new List<Beneficiary>();

                            foreach (var postulante in postulantesSolicitud)
                            {
                                if (postulante.TipoIntegranteId == "BP")
                                {
                                    var beneficiary = new Beneficiary
                                    {
                                        PorcentajeCesantia = postulante.PorcentajeCesantias,
                                        PorcentajeAporte = postulante.PorcentajeAportes,
                                        PorcentajePostulacion = postulante.PorcentajePostulacion,
                                        PorcentajeBeneficiario = (postulante.PorcentajeAportes + postulante.PorcentajeCesantias + postulante.PorcentajePostulacion) / 3.0,
                                        TipoBeneficiario = postulante.TipoParentescoId == "NINGUNO" ? "MISMO BENEFICIARIO" : postulante.TipoParentescoId,
                                        IdentificacionBeneficiario = postulante.NumeroDocumentoPostulante,
                                        NombreCompleto = postulante.NombrePostulante + " " + postulante.ApellidoPostulante,
                                        AfectaccionFisica = postulante.AfectacionFisica,
                                        AfectaccionSensorial = postulante.AfectacionSensorial,
                                        EnfermedadEspecifica = postulante.EnfermedadesEspecificas,
                                        RecibeTratamiento = postulante.TratamientoMedico,
                                        RequiereMedicamento = postulante.MedicamentoPermanente,
                                        RequiereAyuda = postulante.AyudaTecnica,
                                        NombreIdentificacion = postulante.nombreIdentificacion
                                    };

                                    objectData.Add(beneficiary);
                                }


                                listPorcentajeParticipacionAporte.Add(postulante.PorcentajeAportes);
                                listPorcentajeParticipacionCesantias.Add(postulante.PorcentajeCesantias);
                            }

                            var dataConceptoSaldo = dataCuenta.Select(x => x.Conceptos);
                            var listSaldoConceptoDisponible = new List<double>();
                            var listSaldoCensatia = new List<double>();

                            foreach (var conceptos in dataConceptoSaldo)
                            {
                                if (conceptos.Count > 0)
                                {
                                    foreach (var concepto in conceptos)
                                    {
                                        if (concepto.ConceptoDescripcion.Equals("AHORROS OBLIGATORIOS") ||
                                            concepto.ConceptoDescripcion.Equals("AHORROS RETROACTIVOS") ||
                                            concepto.ConceptoDescripcion.Equals("AHORROS VOLUNTARIOS") ||
                                            concepto.ConceptoDescripcion.Equals("EXCEDENTE FINANCIERO") ||
                                            concepto.ConceptoDescripcion.Equals("COMPENSACIONES") ||
                                            concepto.ConceptoDescripcion.Equals("INTERESES APORTES"))
                                        {
                                            listSaldoConceptoDisponible.Add(concepto.saldo);
                                        }
                                        if (concepto.ConceptoDescripcion.Equals("CESANTÍAS") ||
                                            concepto.ConceptoDescripcion.Equals("CESANTÍAS RETROACTIVOS") ||
                                            concepto.ConceptoDescripcion.Equals("INTERESES CESANTÍAS"))
                                        {
                                            listSaldoCensatia.Add(concepto.saldo);
                                        }
                                    }

                                }
                            }
                            var totalAporte = listPorcentajeParticipacionAporte.Sum();
                            var saldoConceptoAporte = listSaldoConceptoDisponible.Sum();
                            var porcentajeAporte = (decimal)totalAporte / 100;

                            var saldoAporte = (decimal)saldoConceptoAporte * porcentajeAporte;


                            var saldoConceptoCesantias = listSaldoCensatia.Sum();
                            var totalCensatias = listPorcentajeParticipacionCesantias.Sum();
                            var porcenteCesantias = (decimal)totalCensatias / 100;

                            var saldoCesantias = (decimal)saldoConceptoCesantias * porcenteCesantias;
                            var saldoTotalDisponible = saldoAporte + saldoCesantias;

                            foreach (var beneficiary in objectData)
                            {
                                var reportObjects = new ReportRankingDto()
                                {
                                    Consecutivo = item.ConsecutivoSolicitud,
                                    NoRadicado = item.NoRadicado,
                                    NombreAfiliado = item.NombreAfiliado,
                                    TipoDocumento = item.TipoDocumento,
                                    NumeroDocumento = item.NumeroDocumento,
                                    Fuerza = item.Fuerza,
                                    UnidadEjecutora = item.UnidadEjecutora,
                                    Categoria = item.Categoria,
                                    Ciudad1 = item.Ciudad1,
                                    Ciudad2 = item.Ciudad2,
                                    Ciudad3 = item.Ciudad3,
                                    Fecha = item.Fecha,
                                    Hora = item.Hora,
                                    MuerteDiscapacidad = getAllCalifications?.HechoDescripcion ?? "N/A",
                                    ImputabilidadServicio = ImputabilidadServicio,
                                    Criterio1 = getAllCalifications?.ImputabilidadAlServicio <= 0 ? 0 : (int)imputServicioResult,
                                    PorcentajePostulacionDescripcion = porcentaje1,
                                    Criterio2 = getAllCalifications?.PorcentajePostulacion <= 0 ? 0 : (int)porcentajeResult,
                                    PorcentajeMuerteDiscapacidad = muerteDiscapacidad,
                                    Criterio3 = criterio3,
                                    DisfrutePension = item?.DisfrutePension,
                                    Calificacion = resultCalificacion,
                                    Resolucion = getAllCalifications?.DatosResolucion ?? "N/A",
                                    PoseeVivienda = getAllEntrevits?.EFS_VIVIENDA_PROPIA ?? "N/A",
                                    CausalRetiro = getAllCalifications?.TipoImputabilidad,
                                    CausalObservacion = getAllEntrevits?.EFS_INFORMATIVO_ADMIN,
                                    SaldoCuentas = (double)saldoTotalDisponible,
                                    ValorBloqueo = totalBloqueo,
                                    ValorSubsidio = 0,
                                    ValorCaja = 0,
                                    Favorecido = "NO",
                                    Mecanimos = item.ModalidadSolucionVivienda,
                                    Direccion = dataCliente.Data.Direccion,
                                    Telefono = dataCliente.Data.Telefono,
                                    Email = dataCliente.Data.Correo,
                                    Barrio = dataCliente.Data.Barrio,
                                    Ciudad = cityName,
                                    Departamento = deparmentName,
                                    PorcentajeCesantías = beneficiary.PorcentajeCesantia,
                                    PorcentajeAportes = beneficiary.PorcentajeAporte,
                                    PorcentajePostulacion = beneficiary.PorcentajePostulacion,
                                    Particip​aciónBeneficiarios = (int)beneficiary.PorcentajeBeneficiario,
                                    TipoIdBeneficiarios = beneficiary.NombreIdentificacion,
                                    NrosIdentBeneficiarios = beneficiary.IdentificacionBeneficiario,
                                    ParentescoBeneficiarios = beneficiary.TipoBeneficiario,
                                    NombresBeneficiarios = beneficiary.NombreCompleto,
                                    AfectacionFisica = beneficiary.AfectaccionFisica,
                                    AfectacionSensorial = beneficiary.AfectaccionSensorial,
                                    EnfermedadEspecifica = beneficiary.EnfermedadEspecifica,
                                    RecibeTratamiento = beneficiary.RecibeTratamiento,
                                    RequiereMedicamento = beneficiary.RequiereMedicamento,
                                    RequiereAyuda = beneficiary.RequiereAyuda
                                };
                                reportRanking.Add(reportObjects);
                            }
                        }

                        else
                        {
                            reportRanking.Add(reportObject);
                        }
                    }
                    else
                    {
                        reportRanking.Add(reportObject);
                    }

                }
            }
            var report = reportRanking.OrderByDescending(x => x.Calificacion).ToList();

            var puestosAsignados = new Dictionary<string, int>();
            var puesto = 1;
            foreach (var item in report)
            {
                if (puestosAsignados.ContainsKey(item.NumeroDocumento))
                {
                    item.Puesto = puestosAsignados[item.NumeroDocumento];
                }
                else
                {
                    item.Puesto = puesto;
                    puestosAsignados[item.NumeroDocumento] = puesto;
                    item.Puesto = puesto++;
                }
            }

            var numeroBeneficiario = getAllSolicitud.Select(x => x.NumeroBeneficiario).FirstOrDefault();
            int iteraciones = Math.Min(numeroBeneficiario, report.Count);
            for (int i = 0; i < iteraciones; i++)
            {
                report[i].Favorecido = "SI";
            }
            return new Response<IEnumerable<ReportRankingDto>> { Data = report, IsSuccess = true };
        }
        public async Task<Response<SolicituPostulancionResponseDto>> UpdateStatusRequestStage(UpdateRequestStageDto request, Guid userId)
        {
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Rechazado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        request.idSolicitud, userId, request.Observacion, request.Etapa,
                        (int)EstadoPostulacionEnum.Rechazado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Rechazado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                         request.idSolicitud, userId, request.Observacion, request.Etapa,
                         (int)EstadoPostulacionEnum.Rechazado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Rechazado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                         request.idSolicitud, userId, request.Observacion, request.Etapa,
                         (int)EstadoPostulacionEnum.Rechazado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.ENTREVISTA)
            {
                var result = await _postulacionRepository.UpdateStatusEntrevista(request.idSolicitud, (int)EstadoPostulacionEnum.Rechazado);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        request.idSolicitud, userId, request.Observacion, request.Etapa,
                        (int)EstadoPostulacionEnum.Rechazado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.CALIFICACIÓN)
            {
                var result = await _postulacionRepository.UpdateStatusCalificacionFs(request.idSolicitud, (int)EstadoPostulacionEnum.Rechazado);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                       request.idSolicitud, userId, request.Observacion, request.Etapa,
                       (int)EstadoPostulacionEnum.Rechazado);
                }
            }
            var updateArchivo = await _postulacionRepository.ActualizarEstadoArchivoPostulacion(request.idSolicitud, (int)EstadoPostulacionEnum.Inconsistente, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
            if (updateArchivo.Any())
            {
                var dataCargueDocumento = await _postulacionRepository.ObtenerDatosArchivoById(request.idSolicitud);
                await _postulacionRepository.CreateTrazabilidadFondo(
                     request.idSolicitud, dataCargueDocumento.UserId, "", (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                    (int)EstadoPostulacionEnum.Inconsistente);
            }
            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "Todo OK" };
        }

        public async Task<Response<SolicituPostulancionResponseDto>> AnularEtapaFondoSolidaridad(UpdateRequestStageDto request, Guid userId)
        {
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_NOVEDADES)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        request.idSolicitud, userId, request.Observacion, request.Etapa,
                        (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_CUENTA_INDIVIDUAL)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                       request.idSolicitud, userId, request.Observacion, request.Etapa,
                       (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.VERIFICACIÓN_DE_EMBARGOS)
            {
                var result = await _postulacionRepository.UpdateStatusVerificateAoR(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado, request.Etapa);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                       request.idSolicitud, userId, request.Observacion, request.Etapa,
                       (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.ENTREVISTA)
            {
                var result = await _postulacionRepository.UpdateStatusEntrevista(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        request.idSolicitud, userId, request.Observacion, request.Etapa,
                        (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.CALIFICACIÓN)
            {
                var result = await _postulacionRepository.UpdateStatusCalificacionFs(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado);
                if (result > 0)
                {
                    await _postulacionRepository.CreateTrazabilidadFondo(
                       request.idSolicitud, userId, request.Observacion, request.Etapa,
                       (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD)
            {
                var updateArchivo = await _postulacionRepository.ActualizarEstadoArchivoPostulacion(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD);
                if (updateArchivo.Any())
                {
                    //var dataCargueDocumento = await _postulacionRepository.ObtenerDatosArchivoById(request.idSolicitud);
                    await _postulacionRepository.CreateTrazabilidadFondo(
                        request.idSolicitud, userId, request.Observacion, (int)EtapaPostulacionEnum.CARGUE_DE_DOCUMENTOS_FONDO_SOLIDARIDAD,
                       (int)EstadoPostulacionEnum.Anulado);
                }
            }
            if (request.Etapa == (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD)
            {
                var updateArchivo = await _postulacionRepository.Update_Status_Aproved(request.idSolicitud, (int)EstadoPostulacionEnum.Anulado);
                if (updateArchivo is not null)
                {
                    //var dataCargueDocumento = await _postulacionRepository.ObtenerDatosArchivoById(request.idSolicitud);
                    await _postulacionRepository.CreateTrazabilidadFondo(
                         request.idSolicitud, userId, request.Observacion, (int)EtapaPostulacionEnum.SOLICITUD_POSTULACIÓN_FONDO_SOLIDARIDAD,
                        (int)EstadoPostulacionEnum.Anulado);
                }
            }

            return new Response<SolicituPostulancionResponseDto> { Data = null, IsSuccess = true, ReturnMessage = "OK" };
        }

        public async Task<Response<EstadoSolicitudFondoDto>> ObtenerEstadoSolicitudFondo(string noRadicado)
        {
            var dataResponse = await _postulacionRepository.ObtenerEstadoFondoByRadicado(noRadicado);
            if (dataResponse is not null)
            {
                var estado = dataResponse.EstadoCalificacion ?? dataResponse.EstadoEntrevista
                    ?? dataResponse.EstadoVerificacion ?? dataResponse.EstadoArchivo ??
                    dataResponse.Estado;

                if (dataResponse.EstadoCalificacion is null && dataResponse.EstadoEntrevista is null &&
                    dataResponse.EstadoVerificacion is null && dataResponse.EstadoArchivo is null)
                {
                    estado = estado == EstadoPostulacionEnums.Pendiente ? "Radicada" : estado;
                }

                var mapData = _mapper.Map<EstadoSolicitudFondoDto>(dataResponse);
                mapData.Estado = estado;

                return new Response<EstadoSolicitudFondoDto> { Data = mapData, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<EstadoSolicitudFondoDto> { Data = null, IsSuccess = true, ReturnMessage = "OK" };
        }
        public async Task<Response<string>> GuardarIncosistenciaSolicitudFondo(IncosistenciaFondoRequest request)
        {
            var result = await _postulacionRepository.CrearInconsistenciaSolictudFondo(request.IdSolicitud, request.Description);

            return result > 0 ? new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" }
                : new Response<string> { Data = "fallido", IsSuccess = true, ReturnMessage = "Fallido" };
        }
        public async Task<Response<List<InconsistenciaFondoSolidaridadDTO>>> ObtemerIncosistenciaSolicitudFondo(Guid idSolicitud)
        {
            var result = await _postulacionRepository.ObtemerIncosistenciaSolicitudFondo(idSolicitud);
            var dataMap = _mapper.Map<List<InconsistenciaFondoSolidaridadDTO>>(result);
            return result.Count > 0 ?
                new Response<List<InconsistenciaFondoSolidaridadDTO>> { Data = dataMap, IsSuccess = true, ReturnMessage = "OK" }
                : new Response<List<InconsistenciaFondoSolidaridadDTO>> { Data = null, IsSuccess = true, ReturnMessage = "Fallido" };
        }

        public async Task<List<ReportModelFondoSolidariaResponse>> ObtenerDataReportModelFondoSolidaridad(ReportModelFondoSolidaria request)
        {
            var dataSolicitud = await _postulacionRepository.ObtenerDataReportModelFondoSolidaridad(request);
            if (dataSolicitud.Any())
            {
                if (request.SinAportarAlaFecha)
                {
                    DateTime limite = DateTime.Now.AddMonths(-4);
                    dataSolicitud = dataSolicitud.Where(x => x.FechaUltimoAporte == null || x.FechaUltimoAporte < limite).ToList();
                }
                var grados = await _catalogsRepository.ObtenerGrados();
                var fuerzas = await _catalogsRepository.ObtenerFuerzas();
                var responseList = new List<ReportModelFondoSolidariaResponse>();
                foreach (var item in dataSolicitud)
                {
                    var response = new ReportModelFondoSolidariaResponse();
                    var client = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TipoDocumentoId, item.NumeroDocumento.ToString());
                    if (client != null)
                    {
                        var dateFirt = DateTime.Now;
                        var dateLast = DateTime.Now;
                        var qteAporte = 0;
                        var dataClient = client.Data;
                        var saldo = 0m;
                        if (dataClient.AportesCategoria.Count > 0)
                        {
                            dateFirt = dataClient.AportesCategoria.Select(x => x.APC_FECHA_PRIMERA_CUOTA).FirstOrDefault();
                            dateLast = dataClient.AportesCategoria.Select(x => x.APC_FECHA_ULTIMA_COUTA).FirstOrDefault();
                            qteAporte = dataClient.AportesCategoria.Select(x => x.APC_CUOTAS_ACUMULADAS).FirstOrDefault();
                        }
                        saldo = dataClient.Cuentas.Select(x => x.Saldo).FirstOrDefault();
                        response.TipoIdentificacionAfiliado = dataClient.NombreTipoIdentificacion;
                        response.NumeroDocumentoAfiliado = item.NumeroDocumento.ToString();
                        response.Modelo = item.TipoModalidadNombre;
                        response.FechaSolicitud = item.FechaCreacion;
                        response.FechaPrimerAporte = dateFirt;
                        response.FechaUltimoAporte = dateLast;
                        response.NumeroAporte = qteAporte;
                        response.Saldo = saldo;
                        response.Fuerza = fuerzas.Where(x => x.FRZ_ID == dataClient.IdFuerza).Select(x => x.FRZ_DESCRIPCION).FirstOrDefault();
                        response.UnidadEjecutora = item.UnidadEjecutora ?? "N/A";
                        response.Grado = grados.Where(x => x.GRD_ID == dataClient.IdGrado).Select(x => x.GRD_DESCRIPCION).FirstOrDefault();
                        response.NombreAfiliado = item.NombreCompleto;
                        var dataPostulante = await _postulacionRepository.Consultar_Postulacion_By_Id(item.SolicitudId);
                        if (dataPostulante.Any())
                        {
                            foreach (var postulante in dataPostulante)
                            {
                                response.NombreBeneficiario = postulante.NombrePostulante + " " + postulante.ApellidoPostulante;
                                response.IdentificacionBeneficiario = postulante.NumeroDocumentoPostulante.ToString();
                                response.PorcentajeParticipacion = (int)(postulante.PorcentajePostulacion ?? 0 + postulante.PorcentajeCesantias ?? 0 + postulante.PorcentajeAportes ?? 0) / 3;
                            }
                        }
                        else
                        {
                            response.NombreBeneficiario = "N/A";
                            response.IdentificacionBeneficiario = "N/A";
                            response.PorcentajeParticipacion = 0;
                        }
                        responseList.Add(response);

                    }
                }
                return responseList;
            }
            return null;
        }
        public async Task<List<ReportStatusFondoSolidariaResponse>> ObtenerDataReporlFondoSolidaridadPorEstadoTramite(ReportStatusFondoSolidaridad request)
        {
            var dataSolicitud = await _postulacionRepository.ObtenerDataReportEstadoFondoSolidaridad(request);
            if (dataSolicitud.Any())
            {
                var consecutivo = 0;
                var fuerzas = await _catalogsRepository.ObtenerFuerzas();
                var ciuedad = await _catalogsRepository.ObtenerCiudades();
                var responseList = new List<ReportStatusFondoSolidariaResponse>();
                foreach (var item in dataSolicitud)
                {
                    var response = new ReportStatusFondoSolidariaResponse();
                    var client = await _iClienteRequestProvider.ObtenerClientePorTipoIdentificationYNumero(item.TipoDocumentoId, item.NumeroDocumento.ToString());
                    if (client != null)
                    {
                        consecutivo++;
                        var dateFirt = DateTime.Now;
                        var dateLast = DateTime.Now;
                        var qteAporte = 0;
                        var dataClient = client.Data;
                        var saldo = 0m;
                        if (dataClient.AportesCategoria.Count > 0)
                        {
                            dateFirt = dataClient.AportesCategoria.Select(x => x.APC_FECHA_PRIMERA_CUOTA).FirstOrDefault();
                            dateLast = dataClient.AportesCategoria.Select(x => x.APC_FECHA_ULTIMA_COUTA).FirstOrDefault();
                            qteAporte = dataClient.AportesCategoria.Select(x => x.APC_CUOTAS_ACUMULADAS).FirstOrDefault();
                        }
                        saldo = dataClient.Cuentas.Select(x => x.Saldo).FirstOrDefault();
                        response.Consecutivo = consecutivo;
                        response.CedulaAfiliado = dataClient.NombreTipoIdentificacion;
                        response.FechaSolicitud = item.FechaCreacion;
                        response.Radicado = item.NoRadicado;
                        response.Fuerza = fuerzas.Where(x => x.FRZ_ID == dataClient.IdFuerza).Select(x => x.FRZ_DESCRIPCION).FirstOrDefault();
                        response.Direccion = dataClient.Direccion;
                        response.Telefono = dataClient.Telefono;
                        response.Ciuedad = ciuedad.Where(x => x.DPC_ID == dataClient.IdCiudadResidencia).Select(x => x.DPC_NOMBRE).FirstOrDefault();
                        response.EstadoSolicitud = item.EtapaProceso;
                        response.FechaSolicitud = item.FechaCreacion;
                        response.Categoria = item.Categoria;
                        response.Participacion = dataClient.Participacion;
                        response.NombrePostulante = item.NombreCompleto;
                        response.IdentificacionPostulante = item.NumeroDocumento.ToString();
                        var dataPostulante = await _postulacionRepository.Consultar_Postulacion_By_Id(item.SolicitudId);
                        //if (dataPostulante?.Count() > 0)
                        //{
                        //    foreach (var postulante in dataPostulante)
                        //    {
                        //        response.bene = postulante.NombrePostulante;
                        //        response.IdentificacionPostulante = postulante.NumeroDocumentoPostulante.ToString();
                        //    }
                        //}
                        //else
                        //{
                        //    response.NombrePostulante = "N/A";
                        //    response.IdentificacionPostulante = "N/A";
                        //}
                        responseList.Add(response);

                    }
                }
                return responseList;
            }
            return null;
        }
        #endregion
    }
}
