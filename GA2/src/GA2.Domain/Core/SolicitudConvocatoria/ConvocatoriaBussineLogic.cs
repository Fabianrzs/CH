using AutoMapper;
using ClosedXML.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.Convocatoria;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core.Postulacion.Enum;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReportePostuladoCalificado;
using GA2.Domain.Core.Reportes.ReportesXMlClosed.ReportePreliminarCartera;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.SolicitudConvocatoria
{
    public class ConvocatoriaBussineLogic : IConvocatoriaBussigneLogic
    {
        private readonly IMapper _mapper;
        private readonly IConvocatoriaRepository _convocatoriaRepository;
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly IEstadoConvocatoriaRepository _estadoConvocatoriaRepository;

        public ConvocatoriaBussineLogic(IMapper mapper, IConvocatoriaRepository convocatoriaRepository, IPostulacionRepository postulacionRepository, IEstadoConvocatoriaRepository estadoConvocatoria)
        {
            _convocatoriaRepository = convocatoriaRepository;
            _mapper = mapper;
            _postulacionRepository = postulacionRepository;
            _estadoConvocatoriaRepository = estadoConvocatoria;
        }

        public async Task<Response<ConvocatoriaDTO>> CrearConvocatoria(ConvocatoriaDTO crearConvocatoriato)
        {
            if (crearConvocatoriato != null)
            {
                var responseRepository = await _convocatoriaRepository.Obtener_Convocatorias();
                if (responseRepository.Any())
                {
                    foreach (var item in responseRepository)
                    {
                        if (item.NOMBRE_ESTADO.Equals("Apertura") || item.NOMBRE_ESTADO.Equals("Verificación"))
                        {
                            return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "Ya existe una convocatoria en curso" };
                        }
                    }
                }
                if (crearConvocatoriato.FechaApertura.Date < DateTime.Today)
                {
                    return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "La fecha de apertura no puede ser menor que la fecha del sistema" };
                }
                if (crearConvocatoriato.FechaCierre.Date <= crearConvocatoriato.FechaApertura.Date)
                {
                    return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "La fecha de cierre no puede ser menor que la fecha del apertura" };
                }
                var parametrosSolicitud = this._mapper.Map<Convocatoria>(crearConvocatoriato);

                var resultRepository = await _convocatoriaRepository.CrearConvocatoria(parametrosSolicitud);
                if (resultRepository != null)
                {
                    var convocatoriaMap = this._mapper.Map<ConvocatoriaDTO>(resultRepository);
                    return new Response<ConvocatoriaDTO> { Data = convocatoriaMap, IsSuccess = true, ReturnMessage = "Creado con exito" };
                }
                return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "No se puedo crear la solicitud" };
            }
            return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "Valide los datos ingresados" };

        }
        public async Task<Response<IEnumerable<ConvocatoriaResponseDto>>> ConsultarConvocatoria(ConsultarConvocatoriaRequest request)
        {
            var response = new Response<ConvocatoriaResponseDto>();
            var mapRequest = _mapper.Map<ConvocatoriaRequest>(request);
            var convocatoriaRepositoty = await _convocatoriaRepository.ConsultarConvocatoria(mapRequest);
            if (convocatoriaRepositoty != null)
            {
                foreach (var item in convocatoriaRepositoty)
                {
                    var dataCierreConvocatoria = await _convocatoriaRepository.GetDataCierreConvocatoria((int)item.ID_CONVOCATORIA);
                    if (dataCierreConvocatoria is not null)
                    {
                        item.FECHA_PUBLICACION = dataCierreConvocatoria.FechaPublicacionDiario;
                        item.FECHA_CIERRE = dataCierreConvocatoria.FechaCierre;
                        item.NUMERO_BENEFICIARIO = dataCierreConvocatoria.NumeroBeneficiarios;
                    }
                }
                var resultMap = _mapper.Map<IEnumerable<ConvocatoriaResponseDto>>(convocatoriaRepositoty);
                return new Response<IEnumerable<ConvocatoriaResponseDto>> { Data = resultMap, IsSuccess = true, ReturnMessage = "Succeful" };
            }



            return new Response<IEnumerable<ConvocatoriaResponseDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay registros" };

        }
        public async Task<Response<IEnumerable<ConvocatoriaDTO>>> ConsultarConvocatoriaById(ConsultarConvocatoriaRequest request)
        {

            var mapRequest = _mapper.Map<ConvocatoriaRequest>(request);
            var convocatoriaRepositoty = await _convocatoriaRepository.ConsultarConvocatoria(mapRequest);
            if (convocatoriaRepositoty.Count() > 0)
            {

                var resultMap = _mapper.Map<IEnumerable<ConvocatoriaDTO>>(convocatoriaRepositoty);
                return new Response<IEnumerable<ConvocatoriaDTO>> { Data = resultMap, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<IEnumerable<ConvocatoriaDTO>> { Data = null, IsSuccess = true, ReturnMessage = "No hay registros" };

        }
        public async Task<Response<int>> Consultar_Id_Convocatoria()
        {
            var responseRepository = await _convocatoriaRepository.Consultar_Id_Convocatoria();
            return new Response<int> { Data = responseRepository, IsSuccess = true, ReturnMessage = "Succeful" };

        }

        public async Task<Response<ConvocatoriaDTO>> Obtener_Id_Convocatoria()
        {
            var responseRepository = await _convocatoriaRepository.OBTENER_ID_CONVOCATORIA();
            var responseMap = _mapper.Map<ConvocatoriaDTO>(responseRepository);
            return new Response<ConvocatoriaDTO> { Data = responseMap, IsSuccess = true, ReturnMessage = "Succeful" };
        }

        public async Task<Response<IEnumerable<int>>> Obtener_List_Convocatoria()
        {
            var responseRepository = await _convocatoriaRepository.OBTENER_LIST_CONVOCATORIA();
            return new Response<IEnumerable<int>> { Data = responseRepository, IsSuccess = true, ReturnMessage = "Succeful" };
        }
        public async Task<Response<string>> EliminarConvocatoria(EliminarConvocatoriaRequest convocatoriaRequest)
        {
            if (convocatoriaRequest.Delete != 1)
            {
                return new Response<string> { Data = "No se pudo eliminar la convocatoria, por favor valide", IsSuccess = false, ReturnMessage = "ERROR" };
            }
            var EstadoConvocatoria = 6;
            var deleteRepository = await _convocatoriaRepository.EliminarConvocatoria(convocatoriaRequest.IdConvocatoria, convocatoriaRequest.Delete, EstadoConvocatoria);
            if (deleteRepository <= 0)
            {
                return new Response<string> { Data = "No se pudo eliminar la convocatoria, por favor valide", IsSuccess = false, ReturnMessage = "ERROR" };
            }
            return new Response<string> { Data = "Se elimino exitosamente la convocatoria", IsSuccess = true, ReturnMessage = "Succeful" };
        }
        public async Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> ConsultarPostulanteByConvocatoria(int idConvoatoria)
        {
            var result = await Consulta(idConvoatoria);
            if (result.Any())
            {
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = result, IsSuccess = false, ReturnMessage = "OK" };

            }
            return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = true, ReturnMessage = "No hay datos" };
        }
        public async Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> CrearCierreConvocatoria(CierreConvocatoriaDto crearConvocatoriato)
        {
            var parametrosConvocatoria = this._mapper.Map<CierreConvocatoria>(crearConvocatoriato);
            var resultPostulate = await Consulta(parametrosConvocatoria.IdConvocatoria);
            if (!resultPostulate.Any())
            {
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = false, ReturnMessage = "No hay postulante para crear el cierre" };
            }
            if (resultPostulate.Count() < parametrosConvocatoria.NumeroBeneficiarios)
            {
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = false, ReturnMessage = "No hay tantos benefeciario para crear este cierre" };
            }
            var resultRepository = await _convocatoriaRepository.CreateCierreConvocatoria(parametrosConvocatoria);
            if (resultRepository > 0)
            {
                var estadoConvcatoria = await _estadoConvocatoriaRepository.ObtenerEstadosConvocatoria();
                foreach (var item in estadoConvcatoria)
                {
                    if (item.NombreEstado.Equals("Cerrada"))
                    {
                        var updateEstado = await _convocatoriaRepository.UpdateEstadoCierreConvocatoria(parametrosConvocatoria.IdConvocatoria, item.IdEstado);
                        if (updateEstado > 0)
                        {
                            var result =  await Consulta(crearConvocatoriato.IdConvocatoria);
                            var order = result.OrderByDescending(x => x.puntaje).ToList();
                            order.Take(parametrosConvocatoria.NumeroBeneficiarios).ToList();
                            var resultData = order.Take(parametrosConvocatoria.NumeroBeneficiarios).ToList();
                            return new Response< IEnumerable<PostulanteConvocatoriaDto>> { Data = resultData, IsSuccess = true, ReturnMessage = "Creado con exito" };
                        }
                    }
                }            }
            return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = false, ReturnMessage = "No se puedo crear la solicitud" };

        }
        public async Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> ConsultarPostulanteByConvocatoriaOrden(int idConvoatoria)
        {
            var result = await Consulta(idConvoatoria);
            if (result is not null)
            {
                var dataCierre = await _convocatoriaRepository.GetDataCierreConvocatoria(idConvoatoria);
                if (dataCierre is not null)
                {
                    var NoBeneficiario = await _convocatoriaRepository.GetDataConvocatoriaSinCupo(idConvoatoria);
                    var dataFilter = result.Where(x => x.Estado != EstadoPostulacionEnums.Anulado
                    || x.Estado != EstadoPostulacionEnums.Rechazado || x.Estado != EstadoPostulacionEnums.Inconsistente);
                    var orden = dataFilter.OrderByDescending(x => x.puntaje).ToList();
                    var  resulData = orden.Take(NoBeneficiario);
                    return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = resulData, IsSuccess = false, ReturnMessage = "OK" };
                }
                var resultFilter = result.Where(x => x.Estado != EstadoPostulacionEnums.Anulado
                && x.Estado != EstadoPostulacionEnums.Rechazado && x.Estado != EstadoPostulacionEnums.Inconsistente).ToList();
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = resultFilter.OrderByDescending(x => x.puntaje), IsSuccess = false, ReturnMessage = "OK" };

            }
            return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = true, ReturnMessage = $"No hay postualante con el numero de convocatotia {idConvoatoria}" };
        }
        public async Task<Response<IEnumerable<ConvocatoriaDeclineDto>>> GetConvocatoriaDeline(int idConvocatoria)

        {
            if (idConvocatoria <= 0)
            {
                return new Response<IEnumerable<ConvocatoriaDeclineDto>> { Data = null, IsSuccess = false, ReturnMessage = "Ingrese el numero de convocatoria" };
            }
            var convocatoriaDecline = await _convocatoriaRepository.GetDataConvocatoriaDecline(idConvocatoria);
            if (convocatoriaDecline is null)
            {
                return new Response<IEnumerable<ConvocatoriaDeclineDto>> { Data = null, IsSuccess = true, ReturnMessage = $"No hay postualante con el numero de convocatotia {idConvocatoria}" };
            }
            var listResponse = new List<SolicitudPostulacionResponse>();
            var listSolicitud = new List<SolicitudPostulante>();
            foreach (var decline in convocatoriaDecline)
            {
                var calificacion = await _postulacionRepository.ObtenerDatosCalificacionById(decline.SolicitudId);
                if (calificacion != null)
                {
                    if (calificacion.EtapaProceso.Equals("ANULADO"))
                    {
                        listResponse.Add(calificacion);
                    }
                }
                var entrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(decline.SolicitudId);
                if (entrevista != null)
                {
                    if (entrevista.EtapaProceso.Equals("ANULADO"))
                    {
                        listResponse.Add(entrevista);
                    }
                }
                var arcchivo = await _postulacionRepository.ObtenerDatosArchivoById(decline.SolicitudId);
                if (arcchivo != null)
                {
                    if (arcchivo.EtapaProceso.Equals("ANULADO"))
                    {
                        listResponse.Add(arcchivo);
                    }
                }
                var solicitud = await _postulacionRepository.Consultar_Solicitud_Postulacion_By_Id(decline.NumeroDcoumento);
                if (solicitud != null)
                {
                    if (solicitud.EtapaProceso.Equals("ANULADO"))
                    {
                        listResponse.Add(solicitud);

                    }
                }
            }
            var dataDecline = from x in listResponse
                              join y in convocatoriaDecline
                              on x.SolicitudId equals y.SolicitudId
                              select new ConvocatoriaDeclineDto
                              {
                                  SolicitudId = y.SolicitudId,
                                  TipoModalidad = y.TipoModalidad,
                                  NombreAfiliado = y.NombreAfiliado,
                                  Categoria = y.Categoria,
                                  UnidadEjecutora = y.UnidadEjecutora,
                                  EtapaProceso = x.Actividad,
                                  NoRadicado = x.NoRadicado
                                 
                              };
            return new Response<IEnumerable<ConvocatoriaDeclineDto>> { Data = dataDecline, IsSuccess = true, ReturnMessage = "OK" };
        
        }
        public async Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> GetConvocatoriaSinCupo(int idConvoatoria)
        {
            var result = await Consulta(idConvoatoria);
            if (result is not null)
            {
                var validateDateCierre = await _convocatoriaRepository.GetDataCierreConvocatoria(idConvoatoria); 
                if (validateDateCierre is not null)
                {
                    var NoBeneficiario = await _convocatoriaRepository.GetDataConvocatoriaSinCupo(idConvoatoria);
                    var dataFilter = result.Where(x => x.Estado != EstadoPostulacionEnums.Anulado
                    && x.Estado != EstadoPostulacionEnums.Rechazado && x.Estado != EstadoPostulacionEnums.Inconsistente).ToList();
                    var orden = dataFilter.OrderByDescending(x => x.puntaje).ToList();
                    if (orden.Count > 0)
                    {
                        orden.RemoveRange(0, NoBeneficiario);
                    }
                    return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = orden, IsSuccess = true, ReturnMessage = "OK" };
                }
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = false, ReturnMessage = $"No se registrado cierre de convocatoria para este id {idConvoatoria}" };
            }
            return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = false, ReturnMessage = $"No hay postualante con el numero de convocatotia {idConvoatoria}" };
        }
        public async Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> GetConvocatoriaRechazado(int idConvoatoria)
        {
                var result = await Consulta(idConvoatoria);
                if (result is not null)
                {
                    var dataFilter = result.Where(x => x.Estado == EstadoPostulacionEnums.Anulado
                    || x.Estado == EstadoPostulacionEnums.Rechazado || x.Estado == EstadoPostulacionEnums.Inconsistente);
                    var orden = dataFilter.OrderByDescending(x => x.puntaje).ToList();
                    return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = orden, IsSuccess = true, ReturnMessage = "OK" };
                }
                return new Response<IEnumerable<PostulanteConvocatoriaDto>> { Data = null, IsSuccess = true, ReturnMessage = $"No hay postualante con el numero de convocatotia {idConvoatoria}" };
            
        }

        private async Task<IEnumerable<PostulanteConvocatoriaDto>> Consulta(int idConvoatoria)
        {
            var consultarDataConvocatoria = await _convocatoriaRepository.ConsultarPostulanteByConvocatoriaId(idConvoatoria);
            if (consultarDataConvocatoria.Any())
            {

                var result = new List<PostulanteConvocatoriaDto>();
                foreach (var item in consultarDataConvocatoria)
                {
                    var consultarEntrevista = new SolicitudPostulacionResponse();
                    var consultarArchivos = new SolicitudPostulacionResponse();
                    var consultarSolicitud = new SolicitudPostulacionResponse();

                    var consultarCalificacion = await _postulacionRepository.ObtenerDatosCalificacionById(item.SolicitudId);
                    if (consultarCalificacion != null)
                    {
                        var calificacion = await _postulacionRepository.ConsultarCalificacionBySolId(item.SolicitudId);
                        var imputServicio = 0.65M;
                        var porcentaje = 0.30M;
                        var hecho = 0.05M;
                        var imputServicioResult = calificacion.ImputabilidadAlServicio * imputServicio;
                        var porcentajeResult = calificacion.PorcentajePostulacion * porcentaje;
                        var hechoResult = calificacion.Hecho * hecho;
                        calificacion.Puntaje = imputServicioResult + porcentajeResult + hechoResult;

                        var map = new PostulanteConvocatoriaDto
                        {
                            SolicitudId = consultarCalificacion.SolicitudId,
                            TipoModalidad = consultarCalificacion.TipoModalidadId,
                            ModalidadName = consultarCalificacion.TipoModalidadNombre,
                            Afiliado = consultarCalificacion.NumeroDocumento +" "+consultarCalificacion.NombreCompletoAfiliado,
                            TipoDocumento = consultarCalificacion.TipoDocumentoId,
                            NumeroDocumento = consultarCalificacion.NumeroDocumento,
                            puntaje = calificacion.Puntaje,
                            EtapaProceso = consultarCalificacion.Actividad,
                            UnidadEjecutora = consultarCalificacion.UnidadEjecutora,
                            Categoria = consultarCalificacion.Categoria,
                            Usuario = consultarCalificacion.UserName,
                            NoRadicado = consultarCalificacion.NoRadicado,
                            ModalidadSolucionVivienda = consultarCalificacion.ModalidadSolucionVivienda,
                            Estado = consultarCalificacion.EtapaProceso
                        };
                        result.Add(map);
                    }
                    if (consultarCalificacion == null)
                    {
                        consultarEntrevista = await _postulacionRepository.ObtenerDatosEntrevitsById(item.SolicitudId);
                        if (consultarEntrevista != null)
                        {
                            var map = new PostulanteConvocatoriaDto
                            {
                                SolicitudId = consultarEntrevista.SolicitudId,
                                TipoModalidad = consultarEntrevista.TipoModalidadId,
                                ModalidadName = consultarEntrevista.TipoModalidadNombre,
                                Afiliado = consultarEntrevista.NumeroDocumento + " " +consultarEntrevista.NombreCompletoAfiliado,
                                TipoDocumento = consultarEntrevista.TipoDocumentoId,
                                NumeroDocumento = consultarEntrevista.NumeroDocumento,
                                puntaje = 0,
                                EtapaProceso = consultarEntrevista.Actividad,
                                UnidadEjecutora = consultarEntrevista.UnidadEjecutora,
                                Categoria = consultarEntrevista.Categoria,
                                Usuario = consultarEntrevista.UserName,
                                NoRadicado = consultarEntrevista.NoRadicado,
                                ModalidadSolucionVivienda = consultarEntrevista.ModalidadSolucionVivienda,
                                Estado =consultarEntrevista.EtapaProceso
                            };
                            result.Add(map);
                        }
                    }
                    if (consultarEntrevista == null)
                    {
                        consultarArchivos = await _postulacionRepository.ObtenerDatosArchivoById(item.SolicitudId);
                        if (consultarArchivos != null)
                        {
                            var map = new PostulanteConvocatoriaDto
                            {
                                SolicitudId = consultarArchivos.SolicitudId,
                                TipoModalidad = consultarArchivos.TipoModalidadId,
                                ModalidadName = consultarArchivos.TipoModalidadNombre,
                                Afiliado = consultarArchivos.NumeroDocumento  + " "+consultarArchivos.NombreCompletoAfiliado,
                                TipoDocumento = consultarArchivos.TipoDocumentoId,
                                NumeroDocumento = consultarArchivos.NumeroDocumento,
                                puntaje = 0,
                                EtapaProceso = consultarArchivos.Actividad,
                                UnidadEjecutora = consultarArchivos.UnidadEjecutora,
                                Categoria = consultarArchivos.Categoria,
                                Usuario = consultarArchivos.UserName,
                                NoRadicado = consultarArchivos.NoRadicado,
                                ModalidadSolucionVivienda = consultarArchivos.ModalidadSolucionVivienda,
                                Estado = consultarArchivos.EtapaProceso
                            };
                            result.Add(map);
                        }
                    }
                    if (consultarArchivos == null)
                    {
                        consultarSolicitud = await _postulacionRepository.Consultar_Solicitud_Postulacion_By_Id(item.NumeroDocumento);
                        if (consultarSolicitud != null)
                        {
                            var map = new PostulanteConvocatoriaDto
                            {
                                SolicitudId = consultarSolicitud.SolicitudId,
                                TipoModalidad = consultarSolicitud.TipoModalidadId,
                                ModalidadName = consultarSolicitud.TipoModalidadNombre,
                                Afiliado = consultarSolicitud.NumeroDocumento + " " +consultarSolicitud.NombreCompleto,
                                TipoDocumento = consultarSolicitud.TipoDocumentoId,
                                NumeroDocumento = consultarSolicitud.NumeroDocumento,
                                puntaje = 0,
                                EtapaProceso = consultarSolicitud.Actividad,
                                UnidadEjecutora = consultarSolicitud.UnidadEjecutora,
                                Categoria = consultarSolicitud.Categoria,
                                Usuario = consultarSolicitud.UserName,
                                NoRadicado = consultarSolicitud.NoRadicado,
                                ModalidadSolucionVivienda = consultarSolicitud.ModalidadSolucionVivienda,
                                Estado = consultarSolicitud.EtapaProceso
                            };
                            result.Add(map);
                        }
                    }
                }
                return result;
            }
            return null;
        }
        public async Task<Response<CierreConvocatoriaDto>> GetDataCierreConvocatoria(int idConvocatoria)
        {
            var convocatoriaRepositoty = await _convocatoriaRepository.GetDataCierreConvocatoria(idConvocatoria);
            if (convocatoriaRepositoty is not null)
            {
                var resultMap = _mapper.Map<CierreConvocatoriaDto>(convocatoriaRepositoty);
                return new Response<CierreConvocatoriaDto> { Data = resultMap, IsSuccess = true, ReturnMessage = "Succeful" };
            }
            return new Response<CierreConvocatoriaDto> { Data = null, IsSuccess = true, ReturnMessage = "No hay registros" };

        }

        public byte[] GetReportePostuladoCalificadoExcel(List<PostulanteCalificadoFileDto> reportePreliminarCarteraDtos)
        {
            MemoryStream stream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.AddWorksheet("Reporte postulante calificados");
                ReportePostuladoCalificado tgc = new ReportePostuladoCalificado(worksheet, reportePreliminarCarteraDtos);
                worksheet = tgc.GetReportePostuladoCalificado();
                workbook.SaveAs(stream);
            }

            return stream.ToArray();
        }
        public async Task<Response<IEnumerable<PostulanteCalificadoFileDto>>> GetReportePostulanteCalificado(int idConvocatoria)
        {
            var response = await Consulta(idConvocatoria);
            var NoBeneficiario = await _convocatoriaRepository.GetDataConvocatoriaSinCupo(idConvocatoria);
            var order = response.OrderByDescending(x => x.puntaje).ToList();
            order.Take(NoBeneficiario).ToList();
            var resultData = order.Take(NoBeneficiario).ToList();

            List<PostulanteCalificadoFileDto> list = (from r in resultData
                                                      select new PostulanteCalificadoFileDto
                                                      {
                                                          NoRadicado = r.NoRadicado,
                                                          SolicitudId =r.SolicitudId,
                                                          ModalidadName = r.ModalidadName,
                                                          Afiliado =r.Afiliado,
                                                          UnidadEjecutora = r.UnidadEjecutora,
                                                          Categoria = r.Categoria,
                                                          puntaje = r.puntaje,
                                                          Usuario = r.Usuario,
                                                          EtapaProceso = r.EtapaProceso,
                                                      }).ToList();

            if (list != null && list.Count > 0)
            {
                var result = GetReportePostuladoCalificadoExcel(list);
                list[0].File = Get(result);
            }
            else
            {
                list = null;
            }
            return new Response<IEnumerable<PostulanteCalificadoFileDto>> { Data = list };
        }
        private FileResult Get(byte[] bytes, [System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileContentResult = new FileContentResult(bytes, contentType)
            {
                FileDownloadName = $"{name}.xlsx",
                FileContents = bytes
            };

            return fileContentResult;
        }

        public async Task<Response<string>> AnularConvocatoria(int idConvocatoria)

        {
            if (idConvocatoria <= 0)
            {
                return new Response<string>{ Data =null,  IsSuccess = false, ReturnMessage = "Ingrese el numero de convocatoria" };
            }
            var dataConvocatoria = await _convocatoriaRepository.GetDataConvocatoriaById(idConvocatoria);
            if (dataConvocatoria.NOMBRE_ESTADO.Equals("Cerrada") || dataConvocatoria.NOMBRE_ESTADO.Equals("Verificación"))
            {
                var postulado = await _convocatoriaRepository.ConsultarPostulanteByConvocatoriaId(idConvocatoria);
                if (postulado.Count() <= 0)
                {
                    return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se puede anular la convocatoria" };
                }
                var updateStatus = await _convocatoriaRepository.UpdateEstadoCierreConvocatoria(idConvocatoria, (int)StatusConvocatoriaEnum.Anulado);
                if (updateStatus > 0 )
                {
                    return new Response<string> { Data = "OK", IsSuccess = false, ReturnMessage = "Anulado exitosamente" };
                }
            }
            if (dataConvocatoria.NOMBRE_ESTADO.Equals("Apertura"))
            {
                var updateStatus = await _convocatoriaRepository.UpdateEstadoCierreConvocatoria(idConvocatoria, (int)StatusConvocatoriaEnum.Anulado);
                if (updateStatus > 0)
                {
                    return new Response<string> { Data = "OK", IsSuccess = false, ReturnMessage = "Anulado exitosamente" };
                }
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Verique con el adminitrador del sistema" };

        }

        public async Task<Response<ConvocatoriaDTO>> UpdateDataConvocatoria(ConvocatoriaDTO data)
        {
            var mapConvocatoria = _mapper.Map<Convocatoria>(data);
            if (mapConvocatoria is not null)
            {
                var updatData = await _convocatoriaRepository.UpdatDataConvocatoria(mapConvocatoria);
                var resultMap = _mapper.Map<ConvocatoriaDTO>(updatData);
                return new Response<ConvocatoriaDTO> { Data = resultMap, IsSuccess = true, ReturnMessage = "OK" };
            }

            return new Response<ConvocatoriaDTO> { Data = null, IsSuccess = false, ReturnMessage = "No se logro actualizar" };
        }
        public async Task<Response<string>> UpdateFechaCierre(UpdateFechaCierreDto data)
        {
            var updatData = await _convocatoriaRepository.UpdateFechaCierre(data.FechaPublicacionDiario,data.FechaNotificacion,data.IdConvocatoria);
            if (updatData >= 1)
            {
                return new Response<string> { Data = "Fechas Actualizada con exito", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No se logro actualizar" };
        }
    }

}
