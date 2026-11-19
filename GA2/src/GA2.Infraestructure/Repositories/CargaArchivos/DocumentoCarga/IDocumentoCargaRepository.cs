using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Domain.Entities.CargueNonima.Document;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IDocumentoCargaRepository
    {
        Task<Documento> ActualizarDocumentoCarga(Documento documentoCarga);
        Task<Documento> CambiarEstadoAProcesar(int documentoId);
        Task<Documento> CrearDocumentoCarga(Documento documentoCarga);
        Task<FileNameReported> CrearNombreArchivo(FileNameReported nombre);
        Task<HeaderFileReported> CrearEncabezadoArchivo(HeaderFileReported encabezado);
        Task<IEnumerable<BodyFile>> CrearCuerpoArchivo(IEnumerable<BodyFile> cuerpo);
        Task<EndFileReported> CrearFinArchivo(EndFileReported fin);
        Task<EndFileReported> ObtenerFinArchivo(int dct_id);
        Task<IEnumerable<Documento>> ObtenerDocumentosCarga(int esdId);
        Task<IEnumerable<NMN_ESTADOS_ARCHIVOS_NOMINA>> ObtenerEstadosArchivos();
        Task<IEnumerable<CausalRechazoNomina>> ConsultarCausalRechazoNomina(); 
        Task<List<ConsultarDocumentosCarga>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter);
        Task<List<ConsultarArchivosCargados>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter);
        Task<List<GuardarArchivoCorregido>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir);
        Task<List<ArchivosConError>> ConsultarArchivoConError(ConsultarArchivosConErrorDto consultarArchivosCargadosFilter);
        Task<Documento> ObtenerDocumentosPorId(int documentoId);
        Task<IEnumerable<Documento>> ObtenerDocumentoPorNombre(string nombre);
        Task<Documento> CambiarEstadoDocumentoCargadoARechazado(int documentoId);
        Task<Documento> CambiarEstadoDocumentoRechazadoACargado(int documentoId);
        Task<Documento> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError);
        Task<SolicitudNomina> CrearSolicitudNomina(SolicitudNominaDto request);
        Task<SolicitudNomina> ActualizaSolicitudNomina(SolicitudNominaDto request);
        Task<SolicitudNomina> ObtenerSolicitudNomina(int documentoId);
        Task<List<ArchivosConError>> ObtenerCuerpoNomina(int documentoId);
        Task<List<ArchivoNombreDto>> ObtenerNombreArchivoReconstruido(int documentoId);
        Task<List<ArchivoHeaderDto>> ObtenerHeaderArchivoReconstruido(int documentoId);
        Task<List<ArchivoBodyDto>> ObtenerBodyArchivoReconstruido(int documentoId);
        Task<List<ArchivoFinDto>> ObtenerFinArchivoReconstruido(int documentoId);
        Task<Documento> AprobarCargueNominaSolicitud(int documentoId);
        Task<IEnumerable<InformacionAfiliadoCargueNomina>> InsertarInformacionAfiliadoCargueNomina(InformacionAfiliadoCargueNomina informacionAfiliado);
        Task<bool> RegistrarNotificacionNomina(NotificacionCargueNomina notificacionCargue); 
        Task<IEnumerable<ConsultarNotificacionNomina>> ConsultaNotificacionNomina(Guid usuarioID); 

    }
}