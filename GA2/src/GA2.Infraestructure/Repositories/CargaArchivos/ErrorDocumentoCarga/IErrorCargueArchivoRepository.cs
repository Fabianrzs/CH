using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.CargueNonima.CargaNomina;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IErrorCargueArchivoRepository
    {
        Task<List<ErrorCargueArchivo>> CrearErroresCargueArchivo(List<ErrorCargueArchivo> errorCargueArchivo);
        Task<List<ErrorCargueArchivo>> ConsultarErroresCargueArchivo();
        Task<List<ErrorCargueArchivo>> ConsultarErroresPorCargueArchivo(int documentoId);
        Task<FileNameReported> ObtenerNombreArchivoPorId(int documentoId);
        Task<List<ErrorCargueArchivoDto>> ActualizarEstadoErrorArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir);
        Task<BodyFile> ObtenerUltimoConsecutivoCuerpo(int documentoId);
        Task<ResponseCorreccionMasiva> ActualizarConsecutivos(int documentoId);
        Task<ResponseCorreccionMasiva> GuardarEncabezadoMasivo(int documentoId, int unidadEjecutora, string periodo, string tipoArchivo, string categoria);
        Task<ResponseCorreccionMasiva> GuardarCuerpoMasivo(int documentoId, int unidadEjecutora, string periodo);
        Task<ResponseCorreccionMasiva> GuardarFinMasivo(int documentoId, int valorIndicadorControl);
        Task<ConsultarArchivosCargados> ObtenerTotalErroresPorArchivo(int documentoId);
        Task<ConsultarArchivosCargados> ActualizarEstadoDocumento(int documentoId, int estadoCargueDocumentoId, int estadoCargueArchivoId);
        
    }
}
