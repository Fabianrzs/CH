using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ConvocatoriaRepository : DapperGenericRepository, IConvocatoriaRepository
    {
        public ConvocatoriaRepository(IConfiguration configuration) : base(configuration) { }



        public async Task<Convocatoria> CrearConvocatoria(Convocatoria crearConvocatoriaDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaApertura), crearConvocatoriaDto.FECHA_APERTURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaCierre), crearConvocatoriaDto.FECHA_CIERRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaPublicacion), crearConvocatoriaDto.FECHA_PUBLICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumBeneficiarion), crearConvocatoriaDto.NUMERO_BENEFICIARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumResolucion), crearConvocatoriaDto.NUMERO_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumDiarioOficial), crearConvocatoriaDto.NUMERO_DIARIOFICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.EstadoConvocatoriaId), crearConvocatoriaDto.ESTADO_CONVOCATORIA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaResolucion), crearConvocatoriaDto.FECHARESOLUCION);


            // ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            var response = await GetAsyncFirst<Convocatoria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ConvocatoriaResponse>> ConsultarConvocatoria(ConvocatoriaRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarConvocatoria.NoConvocatoria), request.NoConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarConvocatoria.EstadoId), request.EstadoConvocatoriaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarConvocatoria.FechaInicio), request.FechaApertura);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarConvocatoria.FechaFin), request.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConsultarConvocatoria.FechaPublicacion), request.FechaPublicacion);

            var response = await GetAsyncList<ConvocatoriaResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;


        }

        public async Task<int> Consultar_Id_Convocatoria()
        {

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<Convocatoria> OBTENER_ID_CONVOCATORIA()
        {
            var response = await GetAsyncFirst<Convocatoria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<int>> OBTENER_LIST_CONVOCATORIA()
        {
            var response = await GetAsyncList<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<ConvocatoriaResponse>> Obtener_Convocatorias()
        {
            var response = await GetAsyncList<ConvocatoriaResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<int> EliminarConvocatoria(int idCon, int delete, int estadoConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsEliminarConvocatoria.IdConvocatoria), idCon);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsEliminarConvocatoria.Delete), delete);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsEliminarConvocatoria.EstadoConvocatoria), estadoConvocatoria);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ConvocatoriaPostulante>> ConsultarPostulanteByConvocatoriaId(int idCon)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.IdConvocatoria), idCon);

            var response = await GetAsyncList<ConvocatoriaPostulante>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> CreateCierreConvocatoria(CierreConvocatoria cierreConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.IdConvocatoria),cierreConvocatoria.IdConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.NumeroResolucionCierre), cierreConvocatoria.NumeroResolucionCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.FechaCierre), cierreConvocatoria.FechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.FechaApertura), cierreConvocatoria.FechaApertura);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.NumeroBeneficiarios), cierreConvocatoria.NumeroBeneficiarios);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.NumeroConvocatoria), cierreConvocatoria.NumeroConvocatoria);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.ValorOficial), cierreConvocatoria.ValorOficial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.ValorSubOficial), cierreConvocatoria.ValorSubOficial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.ValorSoldado), cierreConvocatoria.ValorSoldado);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.Acuerdo), cierreConvocatoria.Acuerdo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.FechaPublicacionDiario), cierreConvocatoria.FechaPublicacionDiario);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsCreateCierreConvocatoria.FechaNotificacion), cierreConvocatoria.FechaNotificacion);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> UpdateEstadoCierreConvocatoria(int idConvocatoia, int estado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idConvocatoia);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdEstado), estado);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
        public async Task<int> UpdateStatusConvocatoria(int idConvocatoia, int estado)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idConvocatoia);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdEstado), estado);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<IEnumerable<ConvocatoriaDecline>> GetDataConvocatoriaDecline(int idCon)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idCon);

            var response = await GetAsyncList<ConvocatoriaDecline>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> GetDataConvocatoriaSinCupo(int idConvocatoia)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idConvocatoia);

            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<CierreConvocatoria> GetDataCierreConvocatoria(int idConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idConvocatoria);

            var response = await GetAsyncFirst<CierreConvocatoria>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public  async Task<ConvocatoriaResponse> GetDataConvocatoriaById(int idConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumsUpdateEstadoConvocatoria.IdConvocatoria), idConvocatoria);

            var response = await GetAsyncFirst<ConvocatoriaResponse>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<Convocatoria> UpdatDataConvocatoria(Convocatoria crearConvocatoriaDto)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaApertura), crearConvocatoriaDto.FECHA_APERTURA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaCierre), crearConvocatoriaDto.FECHA_CIERRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.FechaPublicacion), crearConvocatoriaDto.FECHA_PUBLICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumBeneficiarion), crearConvocatoriaDto.NUMERO_BENEFICIARIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumResolucion), crearConvocatoriaDto.NUMERO_RESOLUCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.NumDiarioOficial), crearConvocatoriaDto.NUMERO_DIARIOFICIAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumOCrearConvocatoriaParameters.IdConvocatoria), crearConvocatoriaDto.ID_CONVOCATORIA);


            // ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            var response = await GetAsyncFirst<Convocatoria>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }

        public async Task<int> UpdateFechaCierre(DateTime fechaPublicacion,DateTime fechaNotificacion,int idConvocatoria)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateFechaCiiere.FechaPublicacionDiaro),fechaPublicacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateFechaCiiere.FechaNotificacion), fechaNotificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumUpdateFechaCiiere.IdConvocatoria), idConvocatoria);


            // ObtenerTramiteSolicitud response = new ObtenerTramiteSolicitud();
            var response = await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
               HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return response;
        }
    }
}
