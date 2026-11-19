
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IConvocatoriaRepository
    {
        Task<Convocatoria> CrearConvocatoria(Convocatoria crearConvocatoriaDto);
        Task<IEnumerable<ConvocatoriaResponse>> ConsultarConvocatoria(ConvocatoriaRequest request);
        Task<int> Consultar_Id_Convocatoria();
        Task<Convocatoria> OBTENER_ID_CONVOCATORIA();
        Task<IEnumerable<ConvocatoriaResponse>> Obtener_Convocatorias();
        Task<int> EliminarConvocatoria(int idCon, int delete, int estadoConvocatoria);
        Task<IEnumerable<ConvocatoriaPostulante>> ConsultarPostulanteByConvocatoriaId(int idCon);
        Task<int> CreateCierreConvocatoria(CierreConvocatoria cierreConvocatoria);
        Task<int> UpdateEstadoCierreConvocatoria(int idConvocatoia, int estado);
        Task<IEnumerable<ConvocatoriaDecline>> GetDataConvocatoriaDecline(int idCon);
        Task<int> GetDataConvocatoriaSinCupo(int idConvocatoia);
        Task<CierreConvocatoria> GetDataCierreConvocatoria(int idConvocatoria);
        Task<ConvocatoriaResponse> GetDataConvocatoriaById(int idConvocatoria);
        Task<Convocatoria> UpdatDataConvocatoria(Convocatoria crearConvocatoriaDto);
        Task<int> UpdateFechaCierre(DateTime fechaDiario, DateTime fechaNotificacion,int idConvocatoria);
        Task<int> UpdateStatusConvocatoria(int idConvocatoia, int estado);
        Task<IEnumerable<int>> OBTENER_LIST_CONVOCATORIA();
    }
}
