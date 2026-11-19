using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IReintegrosRepository
    {
        Task<IEnumerable<TipoReintegros>> ObtenerTipoReintegros();

        Task<TipoReintegros> ObtenerTipoReintegrosById(int idTipo);

        Task<IEnumerable<EstadoReintegros>> ObtenerEstadoReintegros();

        Task<Reintegros> CrearReintegro(Reintegros request);

        Task<DetalleReintegro> CrearDetalleReintegro(DetalleReintegro request);

        Task<IEnumerable<ListaReintegros>> ObtenerListadoReintegros(ReintegrosParams request);
        Task<Reintegro> CreateReintregro(Reintegro request);
        Task<AfiliadosReintegro> CreateAfiliadoReintregro(AfiliadosReintegro request);
        Task<Reintegro> GetDataReintegrById(Guid id);
        Task<IEnumerable<AfiliadosReintegro>> GetDataAfiliadoById(Guid id);
        Task<AfiliadosReintegro> UpdateAfiliadoReintegro(AfiliadosReintegro request);
        Task<int> SaveListAfiliado(AfiliadoReinegroList request);

        Task<IEnumerable<AfiliadoReinegroList>> GetListAfiliado(Guid id);
        Task<IEnumerable<AfiliadosReintegro>> GetValidateAfiliado(long numeroDocumento);
        Task<int> UpdateStatusAfiliadoReintegro(int idAfiliado, Guid idReintegro, string status);
        Task<int> UpdateValuePercent(int idAfiliado, decimal valor);
        Task<int> UpdateValueSolicitado(int idAfiliado, decimal valor);
        Task<AfiliadosReintegro> GetAfiliadoByNumeroDocumento(string numeroDocumento, int tipoDocumento);
        Task<IEnumerable<AfiliadoResponse>> GetDataReintegro(string numeroDocumento, int tipoDocumento);
        Task<ConsecutiveReintegro> GetConsecutiveSolReintegro();
        Task<int> UpdateConsecutiveSolReintegro(int consecutive);
        Task<decimal> GetValueSolicitado(int idAfiliado);
        Task<decimal> GetValuePercentSolicitado(int idAfiliado);
        Task<int> ClenValueAfiliado(int idAfiliado, decimal valor);
        Task<IEnumerable<AfiliadosSeven>> GetAfiliadosReintegro(Guid id_reintegro);
        Task<bool> ActualizaMPRCONTReintegro(Guid idReintegro, int Mpr_Cont);
        Task<bool> ActualizaFACTReintegro(Guid idReintegro, string factCont, string factNume);
        Task<bool> ActualizarCodProvReintegros(Guid idReintegro, long identificacion, int codProveeSeven);
        Task<int> UpdateDataGenerarPago(DataGenerarPago data);
        Task<IEnumerable<AfiliadoReintegroResponse>> ObtnerAfiliadoReintegro(string radicado);
        Task<AfiliadosReintegro> UpdateFechaFinReintegro(long idAfiliado, Guid idReintegro);
        Task<IEnumerable<int>> ObtenerListadoSolicituByAfiliado(string idAfiliado);
        Task<IEnumerable<DataPanillaPagoReintegro>> ObtnerDataPlanillaPagoReintegro(string numeroDocumento, int tipoDocumento);
        Task<bool> ActualizarNumeroRadicadoReintegro(Guid idReintegro, string radicado);
        Task<DataUserPefil> ObtnerUserLider();
    }
}
