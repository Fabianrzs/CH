using GA2.Application.Dto;
using GA2.Domain.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface ICargaArchivoBusinessLogic
    {
        Task<ComponentesDTO> ValidacionArchivoNomina(CargaNominaDTO request, Stream streamFile, string fileName, ParamCargueNomDTO parametros,List<ErrorCargueArchivo> exceptions, DocumentoDto documentoId= default);
    }
}