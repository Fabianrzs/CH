using GA2.Application.Dto;
using GA2.Application.Dto.Convocatoria;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Domain.Core.SolicitudConvocatoria
{
    public interface IConvocatoriaBussigneLogic
    {
        Task<Response<ConvocatoriaDTO>> CrearConvocatoria(ConvocatoriaDTO crearConvocatoriato);
        Task<Response<IEnumerable<ConvocatoriaResponseDto>>> ConsultarConvocatoria(ConsultarConvocatoriaRequest request);
        Task<Response<int>> Consultar_Id_Convocatoria();
        Task<Response<ConvocatoriaDTO>>Obtener_Id_Convocatoria();
        Task<Response<string>> EliminarConvocatoria(EliminarConvocatoriaRequest convocatoriaRequest);
        Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> ConsultarPostulanteByConvocatoria(int idConvoatoria);
        Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> CrearCierreConvocatoria(CierreConvocatoriaDto crearConvocatoriato);
        Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> ConsultarPostulanteByConvocatoriaOrden(int idConvoatoria);
        Task<Response<IEnumerable<ConvocatoriaDeclineDto>>> GetConvocatoriaDeline(int idConvocatoria);
        Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> GetConvocatoriaSinCupo(int idConvoatoria);
        Task<Response<IEnumerable<PostulanteConvocatoriaDto>>> GetConvocatoriaRechazado(int idConvoatoria);
        Task<Response<CierreConvocatoriaDto>> GetDataCierreConvocatoria(int idConvocatoria);
        Task<Response<IEnumerable<PostulanteCalificadoFileDto>>> GetReportePostulanteCalificado(int idConvocatoria);
        Task<Response<string>> AnularConvocatoria(int idConvocatoria);
        Task<Response<ConvocatoriaDTO>> UpdateDataConvocatoria(ConvocatoriaDTO crearConvocatoriato);
        Task<Response<string>> UpdateFechaCierre(UpdateFechaCierreDto data);
        Task<Response<IEnumerable<int>>> Obtener_List_Convocatoria();

        Task<Response<IEnumerable<ConvocatoriaDTO>>> ConsultarConvocatoriaById(ConsultarConvocatoriaRequest request);
    }
}
