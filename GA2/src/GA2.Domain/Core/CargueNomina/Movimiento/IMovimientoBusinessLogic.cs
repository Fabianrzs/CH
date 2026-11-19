using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Application.Dto.Catalogs;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IMovimientoBusinessLogic
    {
        /// <summary>
        /// Obtener lista de documento por estado documento
        /// </summary>
        /// <param name="estadoDocumento">Estado de documento carga</param>
        /// <returns>Lista de documento cargados por estado</returns>
        Task<Response<IEnumerable<DocumentoDto>>> ObtenerDocumentosCarga(int estadoDocumento = 0);

        /// <summary>
        /// Obtener lista de estado de los archivos
        /// </summary>
        /// <returns>Lista de estados cargados para los archivos</returns>
        Task<Response<IEnumerable<EstadoArchivoDto>>> ObtenerEstadosArchivos();

        /// <summary>
        /// Método para consultar la lista de documentos por los filtros de la pantalla
        /// </summary>
        /// <param name="consultarDocumentosCargaFilter"> Clase con los filtros de la pantalla </param>
        /// <returns>Lista de documento cargados por estado</returns>
        Task<Response<List<DocumentoDto>>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter);

        /// <summary>
        /// Método para consultar la lista de archivos por los filtros de la pantalla de cargue de nomina
        /// </summary>
        /// <param name="consultarArchivosCargadosFilter"> Clase con los filtros de la pantalla </param>
        /// <returns>Lista de documento cargados por estado</returns>
        Task<Response<List<ArchivoDto>>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter);

        /// <summary>
        /// Método para consultar la lista de archivos con errores del cargue de nomina
        /// </summary>
        /// <param name="consultarArchivosConError"></param>
        /// <returns>Lista de errores cargados por cada archivo</returns>
        Task<Response<List<ArchivoConErrorDto>>> ConsultarArchivoConError(ConsultarArchivosConErrorDto consultarArchivosConError);

        /// <summary>
        /// Método para guardar las correcciones del archivo de cargue de nomina
        /// </summary>
        /// <param name="guardarArchivoCorregir"></param>
        /// <returns>Lista de errores cargados por cada archivo</returns>
        Task<Response<List<ArchivoConErrorDto>>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir);

        /// <summary>
        /// Método para guardar las correcciones del archivo de cargue de nomina
        /// </summary>
        /// <param name="guardarCorreccionMasiva"></param>
        /// <returns>Lista de errores cargados por cada archivo</returns>
        Task<ResponseCorreccionMasiva> GuardarCorreccionMasiva(CorreccionMasivaArchivoDto guardarCorreccionMasiva);

        /// <summary>
        /// Metodo para rechazar la nomina cargada
        /// </summary>
        /// <param name="documentos">Documentos a rechazar</param>
        /// <returns>documentos rechazados</returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        Task<Response<IEnumerable<DocumentoDto>>> RechazarCargueNomina(IEnumerable<DocumentoDto> documentos);

        /// <summary>
        /// Metodo para rechazar la nomina cargada
        /// </summary>
        /// <param name="documentos">documentos a procesar</param>
        /// <returns>Lista de documento aprobados</returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        Task<Response<IEnumerable<ArchivoDto>>> AprobarCargueNomina(IEnumerable<DocumentoDto> documentos);

        /// <summary>
        /// Metodo para relaizar la validacion de la nomina cargada
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId">Usuario id que realiza el cargue de archivos</param>
        /// <returns></returns>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        Task<Response<IEnumerable<ArchivoDto>>> CargarArchivoNomina(CargaNominaDTO request, Guid userId);

        Task<Response<IEnumerable<DocumentoDto>>> CargarArchivoNominaTemp(CargaNominaDTO request, Guid userId);

        /// <summary>
        /// Método para descargar el documento de excel de los errores al cargar el archivo de nomina
        /// </summary>
        /// <param name="documentoId"> Identificador del documento archivo de nomina </param>
        /// <returns></returns>
        /// <author>Faiber Torres</author>
        /// <date>25/11/2022</date>
        Task<FileResult> DescargarArchivoErroresCargueNomina(int documentoId);

        /// <summary>
        /// Método para descargar el documento de nomina
        /// </summary>
        /// <param name="documentoId"> Identificador del documento archivo de nomina </param>
        /// <returns></returns>
        /// <author>Michel Parra</author>
        /// <date>12/11/2024</date>
        Task<FileResult> DescargarArchivoNomina(int documentoId);

        /// <summary>
        /// Método para eliminar archivos archivo de cargue de nomina
        /// </summary>
        /// <param name="guardarArchivoCorregir"></param>
        /// <returns>Elimina los archivos cargados con errores</returns>
        Task<Response<DocumentoDto>> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError);

        //Task<ErrorCargueArchivo> ConsultarErroresNominaPorId(int documentoId);
        Task<Response<List<ErrorCargueArchivoDto>>> ConsultarErroresNominaPorId(ConsultarArchivosIdDto consultarArchivosConError);

        /// <summary>
        /// Método para crear solicitud nomina
        /// </summary>
        /// <param name="CrearSolicitudNomina"></param>
        /// <returns>crear solicitud nomina</returns>
        Task<Response<SolicitudNominaDto>> CrearSolicitudNomina(SolicitudNominaDto request);


        /// <summary>
        /// Método para actualizar solicitud nomina
        /// </summary>
        /// <param name="ActualizaSolicitudNomina"></param>
        /// <returns>Actualizar solicitud nomina</returns>
        Task<Response<SolicitudNominaDto>> ActualizaSolicitudNomina(SolicitudNominaDto request);


        /// <summary>
        /// Método para obtener solicitud nomina
        /// </summary>
        /// <param name="ObtenerSolicitudNomina"></param>
        /// <returns>obtine solicitud nomina</returns>
        Task<Response<SolicitudNominaDto>> ObtenerSolicitudNomina(int estadoDocumento = 0);

        /// <summary>
        /// Método para validar nomina frente a bua
        /// </summary>
        /// <param name="ValidarNominaBua"></param>
        /// <returns>validar nomina frente a bua</returns>
        Task<Response<List<ErrorCargueArchivoDto>>> ValidarNominaBua(ConsultarArchivosIdDto request);


        Task<Response<IEnumerable<ConsultarNotificacionNomina>>> ConsultarNotificacionNominaSolicitud(Guid usuarioID); 

    }
}