using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ReintegrosRepository : DapperGenericRepository, IReintegrosRepository
    {
        public ReintegrosRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// ObtenerEstadoReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EstadoReintegros>> ObtenerEstadoReintegros()
        {
            return await GetAsyncList<EstadoReintegros>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ObtenerTipoReintegros
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TipoReintegros>> ObtenerTipoReintegros()
        {
            return await GetAsyncList<TipoReintegros>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TipoReintegros> ObtenerTipoReintegrosById(int idTipo)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.TipoReintegro), idTipo);

            return await GetAsyncFirst<TipoReintegros>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// CrearReintegro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Reintegros> CrearReintegro(Reintegros request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.NumeroRadicado), request.NRO_RADICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.UnidadEjecutora), request.UEJ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.TipoReintegro), request.RT_TIPO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.EstadoReintegro), request.RT_ESTADO_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.FechaReintegro), request.FECHA_REINTEGRO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.ValorSolicitado), request.VALOR_SOLICITADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumReintegrosParameters.CreadoPor), request.CREADO_POR);

            return await GetAsyncFirst<Reintegros>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        /// <summary>
        /// CrearDetalleReintegro
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DetalleReintegro> CrearDetalleReintegro(DetalleReintegro request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetalleReintegrosParameters.IdReintegro), request.RT_REI_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetalleReintegrosParameters.Identificacion), request.IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumDetalleReintegrosParameters.CreadoPor), request.CREADO_POR);

            return await GetAsyncFirst<DetalleReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<ListaReintegros>> ObtenerListadoReintegros(ReintegrosParams request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumListadoReintegrosParameters.Radicado), request.NRO_RADICADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumListadoReintegrosParameters.UnidadEjecutora), request.UEJ_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumListadoReintegrosParameters.TipoReintegros), request.RT_TIPO_ID);

            return await GetAsyncList<ListaReintegros>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Reintegro> CreateReintregro(Reintegro request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.NoRadicado), request.Numero_Radicado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Id_Unidad_Ejecutora), request.Id_Unidad_Ejecutora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Unidad_Ejecutora), request.Unidad_Ejecutora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Id_Tipo_Reintegro), request.Id_Tipo_Reintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.FechaCreacion), request.FechaCreacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.UsuarioCreacion), request.UsuarioCreacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Estado), request.Estado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Observacion), request.Observacion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.NumeroSolicitud), request.NumeroSolicitud);

            return await GetAsyncFirst<Reintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<AfiliadosReintegro> CreateAfiliadoReintregro(AfiliadosReintegro request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), request.IdReintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), request.TipoDocumentoId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), request.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Nombres), request.Nombres);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Apellidos), request.Apellidos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Id_Tipo_Reintegro), request.Id_Tipo_Reintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.EstadoAfiliao), request.EstadoAfiliao);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), request.ValorSueldo);
            return await GetAsyncFirst<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Reintegro> GetDataReintegrById(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), id);

            return await GetAsyncFirst<Reintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AfiliadosReintegro>> GetDataAfiliadoById(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), id);

            var x = await GetAsyncList<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return x;
        }
        public async Task<AfiliadosReintegro> UpdateAfiliadoReintegro(AfiliadosReintegro request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), request.Id_Afiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), request.IdReintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), request.TipoDocumentoId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), request.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Nombres), request.Nombres);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Apellidos), request.Apellidos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), request.ValorSueldo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Id_Tipo_Reintegro), request.Id_Tipo_Reintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ObservacionAfiliado), request.ObservacionAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoValor), request.TipoValor);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.EstadoAfiliao), request.EstadoAfiliao);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.FechaFin), request.FechaFin);

            return await GetAsyncFirst<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<int> SaveListAfiliado(AfiliadoReinegroList request)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), request.IdReintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), request.TipoDocumentoId);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), request.NumeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Nombres), request.Nombres);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Apellidos), request.Apellidos);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.Id_Tipo_Reintegro), request.Id_Tipo_Reintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.EstadoAfiliao), request.EstadoAfiliao);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), request.ValorSueldo);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
     HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AfiliadoReinegroList>> GetListAfiliado(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), id); ;
            return await GetAsyncList<AfiliadoReinegroList>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AfiliadosReintegro>> GetValidateAfiliado(long numeroDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), numeroDocumento); ;
            return await GetAsyncList<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> UpdateStatusAfiliadoReintegro(int idAfiliado, Guid idReintegro, string status)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.EstadoAfiliao), status);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<int> UpdateValuePercent(int idAfiliado, decimal valor)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), valor);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> UpdateValueSolicitado(int idAfiliado, decimal valor)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), valor);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<AfiliadosReintegro> GetAfiliadoByNumeroDocumento(string numeroDocumento, int tipoDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), numeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), tipoDocumento);

            return await GetAsyncFirst<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<AfiliadoResponse>> GetDataReintegro(string numeroDocumento, int tipoDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), numeroDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), tipoDocumento);

            return await GetAsyncList<AfiliadoResponse>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<ConsecutiveReintegro> GetConsecutiveSolReintegro()
        {
            return await GetAsyncFirst<ConsecutiveReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<int> UpdateConsecutiveSolReintegro(int consecutive)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsConsecutiveReintegro.Consecutive), consecutive);
            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<decimal> GetValueSolicitado(int idAfiliado)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);

            return await GetAsyncFirst<decimal>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<decimal> GetValuePercentSolicitado(int idAfiliado)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);

            return await GetAsyncFirst<decimal>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> ClenValueAfiliado(int idAfiliado, decimal valor)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.ValorSueldo), valor);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AfiliadosSeven>> GetAfiliadosReintegro(Guid id_reintegro)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), id_reintegro); ;
            return await GetAsyncList<AfiliadosSeven>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> ActualizaMPRCONTReintegro(Guid idReintegro, int Mpr_Cont)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID_REINTEGRO", idReintegro);
            parameters.Add("@MPR_CONT", Mpr_Cont);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<bool> ActualizaFACTReintegro(Guid idReintegro, string factCont, string factNume)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID_REINTEGRO", idReintegro);
            parameters.Add("@FAC_CONT", factCont);
            parameters.Add("@FAC_NUME", factNume);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> ActualizarCodProvReintegros(Guid idReintegro, long identificacion, int codProveeSeven)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID_REINTEGRO", idReintegro);
            parameters.Add("@IDENTIFICACION", identificacion);
            parameters.Add("@PVD_CODD", codProveeSeven);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> UpdateDataGenerarPago(DataGenerarPago data)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@IdReintro", data.IdReintegro);
            parameters.Add("@TipoCuenta", data.TipoCuenta);
            parameters.Add("@CodigoBanco", data.CodigoBanco);
            parameters.Add("@NumeroCuenta", data.NumeroCuenta);
            parameters.Add("@Direccion", data.Direccion);

            var  response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<AfiliadoReintegroResponse>> ObtnerAfiliadoReintegro(string radicado)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateReintegro.NoRadicado), radicado);

            return await GetAsyncList<AfiliadoReintegroResponse>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<int>>ObtenerListadoSolicituByAfiliado(string idAfiliado)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);

            return await GetAsyncList<int>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<AfiliadosReintegro> UpdateFechaFinReintegro(long idAfiliado,Guid idReintegro)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Afiliado), idAfiliado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.Id_Reintregro), idReintegro);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.FechaFin), DateTime.Now);

            return await GetAsyncFirst<AfiliadosReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<DataPanillaPagoReintegro>> ObtnerDataPlanillaPagoReintegro(string numeroDocumento, int tipoDocumento)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.TipoDocumentoId), tipoDocumento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumsCreateAfiliadoReintegro.NumeroDocumento), numeroDocumento);

            return await GetAsyncList<DataPanillaPagoReintegro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<bool> ActualizarNumeroRadicadoReintegro(Guid idReintegro, string radicado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ID_REINTEGRO", idReintegro);
            parameters.Add("@NRO_RADICADO", radicado);

            bool response = new bool();
            response = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<DataUserPefil> ObtnerUserLider()
        {
            var result = string.Empty;
            var response = await GetAsyncFirst<DataUserPefil>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }
    }
}
