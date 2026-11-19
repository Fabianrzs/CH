using GA2.Application.Dto;
using GA2.Application.Dto.CargaNomina;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IDocumentoBusinessLogic
    {
        Task<Response<DocumentoDto>> CambiarEstadoAProcesar(int documentoId);
        Task<Response<IEnumerable<DocumentoDto>>> ObtenerDocumentosCarga(int estadoDocumento);
        Task<Response<IEnumerable<EstadoArchivoDto>>> ObtenerEstadosArchivos();
        Task<List<DocumentoDto>> ConsultarDocumentosCarga(ConsultarDocumentosCargaFilterDTO consultarDocumentosCargaFilter);
        Task<List<ArchivoDto>> ConsultarArchivosCargados(ConsultarArchivosCargadosFilterDTO consultarArchivosCargadosFilter);
        Task<List<ArchivoConErrorDto>> GuardarArchivosCorregido(GuardarArchivoCorregirDto guardarArchivoCorregir);
        //Task<ResponseCorreccionMasiva> GuardarCorreccionMasiva(CorreccionMasivaArchivoDto guardarCorreccionMasiva, GuardarArchivoCorregirDto asignarDatosCorreccionMasiva);
        Task<Response<DocumentoDto>> ObtenerDocumentosPorId(int documentoId);
        Task<DocumentoDto> RegistroDocumento(ComponentesDTO componentesDto, Guid userId, CuentaStorage cuentaStorage, int estadoCargueDocumentoId, int estadoCargueArchivoId);
        Task<NombreArchivoDto> RegistroNombreArchivo(NombreArchivoDto Name);
        Task<EncabezadoArchivoDto> RegistroEnzabezadoArchivo(EncabezadoArchivoDto Header);
        Task<IEnumerable<CuerpoArchivoDto>> RegistroCuerpoArchivo(IEnumerable<CuerpoArchivoDto> Body);
        Task<FinDeArchivoDto> RegistroFinArchivo(FinDeArchivoDto End);
        Task<FinDeArchivoDto> ObtenerFinArchivo(int dct_id);
        Task<ComponentesDTO> ValidarArchivoNomina(CargaNominaDTO request, Stream stream, string nombreArchivo, ParamCargueNomDTO parametros, List<ErrorCargueArchivo> exceptions, DocumentoDto documento = default);
        Task<IEnumerable<DocumentoDto>> ObtenerDocumentoPorNombre(string nombre);
        Task<DocumentoDto> RechazarArchivoCargueNomina(int documentoId);
        Task<DocumentoDto> CambiarEstadoRechazoACargado(int documentoId);
        Task<List<ArchivoConErrorDto>> ConsultarArchivoConError(ConsultarArchivosConErrorDto ConsultarArchivoConError);
        Task<DocumentoDto> EliminarArchivosCargadosConErrores(CargaNominaDTO eliminarArchivoConError);
        Task<SolicitudNominaDto> CrearSolicitudNomina(SolicitudNominaDto request);
        Task<SolicitudNominaDto> ActualizaSolicitudNomina(SolicitudNominaDto request);
        Task<Response<SolicitudNominaDto>> ObtenerSolicitudNomina(int documentoId);
        Task<Response<List<ArchivoConErrorDto>>> ObtenerCuerpoNomina(int documentoId);
        Task<ReconstruccionArchivoDto> ReconstruirArchivoNomina(int dct_id);
        Task<Response<DocumentoDto>> AprobarCargueNominaSolicitud(int documentoId);
        Task<Response<DescargarArchivoCorregidoSolicitudNomina>> ArchivoCorregidoSolicitudNomina(int documentoID);
        Task<Response<IEnumerable<CausalRechazoNomina>>> CausalRechazoNomina();
        Task<Response<string>> InsertarInformacionAfiliadoCargueNomina(DatosInformacionCliente datosClientes); 
    }
}