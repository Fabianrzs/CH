using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IErrorCargueArchivoBusinessLogic
    {
        Task<List<ErrorCargueArchivo>> CrearErroresCargueArchivo(List<ErrorCargueArchivo> errorCargueArchivo);
        Task<List<ErrorCargueArchivo>> ConsultarErroresCargueArchivo();
        Task<List<ErrorCargueArchivo>> ConsultarErroresPorCargueArchivo(int documentoId);
        Task<ErrorCargueArchivo> ValidarCargueArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir, NombreArchivoDto nombre, Response<List<ArchivoConErrorDto>> consultaArchivoConError);
        Task<GuardarArchivoCorregirDto> AsignarDatosCorreccionMasiva(CorreccionMasivaArchivoDto CorreccionMasiva, NombreArchivoDto nombre); 
        Task GuardarDatosCorreccionMasiva(CorreccionMasivaArchivoDto CorreccionMasiva, NombreArchivoDto nombre, Response<List<ArchivoConErrorDto>> consultaArchivoConError);
        Task<NombreArchivoDto> ObtenerNombreArchivoPorId(int documentoId);
        Task<List<ErrorCargueArchivoDto>> ActualizarEstadoErrorArchivo(GuardarArchivoCorregirDto guardarArchivoCorregir);
        Task<ArchivoDto> ObtenerTotalErroresPorArchivo(int documentoId);
        Task<ArchivoDto> ActualizarEstadoDocumento(int documentoId, int estadoCargueDocumentoId, int estadoCargueArchivoId);

        
    }
}
