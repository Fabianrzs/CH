using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Embargos.Seven;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Embargos;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class EmbargosRepository : DapperGenericRepository, IEmbargosRepository
    {
        #region Juzgados


        public async Task<JuzgadosGuid> ObtenerJuzgadoPorId(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_JUZGADO), id);

            return await GetAsyncFirst<JuzgadosGuid>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        public EmbargosRepository(IConfiguration configuration) : base(configuration) { }
        public async Task<Juzgados> ActualizarJuzgado(Juzgados juzgado)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_JUZGADO), juzgado.ID_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_INTERNO), juzgado.CODIGO_INTERNO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NOMBRE), juzgado.NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPD), juzgado.ID_DPD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPC), juzgado.ID_DPC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NRO_CUENTA), juzgado.NRO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_OFICINA), juzgado.CODIGO_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CORREO_ELECTRONICO), juzgado.CORREO_ELECTRONICO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.EMB_MODIFICADOPOR), juzgado.EMB_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.TPR_ID), juzgado.TPR_ID);

            return await GetAsyncFirst<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<Juzgados> CrearJuzgado(Juzgados juzgado)
        {
            DynamicParameters parametros = new();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_INTERNO), juzgado.CODIGO_INTERNO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NOMBRE), juzgado.NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPD), juzgado.ID_DPD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPC), juzgado.ID_DPC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NRO_CUENTA), juzgado.NRO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_OFICINA), juzgado.CODIGO_OFICINA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CORREO_ELECTRONICO), juzgado.CORREO_ELECTRONICO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CELULAR), juzgado.CELULAR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CONTACTO), juzgado.CONTACTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ESTADO), juzgado.ESTADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.EMB_CREADOPOR), juzgado.EMB_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.EMB_FECHA_CREACION), juzgado.EMB_FECHA_CREACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.EMB_MODIFICADOPOR), juzgado.EMB_MODIFICADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.EMB_FECHA_MODIFICACION), juzgado.EMB_FECHA_MODIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.TPR_ID), juzgado.TPR_ID);

            return await GetAsyncFirst<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<Juzgados> InsertarActor(string TipoActor)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.TPR_NOMBRE), TipoActor);

            return await GetAsyncFirst<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<Juzgados> EliminarJuzgadoPorId(string idJuzgado)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_JUZGADO), idJuzgado);

            return await GetAsyncFirst<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        

        public async Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorCiudad(int? idDpto, int? idCiudad)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCiudadesPorDepartamentoParametros.DPD_ID), idDpto);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumCiudadesPorDepartamentoParametros.DPC_ID), idCiudad);

            return await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorDepartamento(int idDpto)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPD), idDpto);

            return await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorNombre(string nombreJuzgado)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NOMBRE), nombreJuzgado);

            return await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }


        #endregion



        public async Task<IEnumerable<TipoEmbargos>> ObtenerTiposEmbargos()
        {
            return await GetAsyncList<TipoEmbargos>(HelperDBParameters.BuilderFunction( HelperDBParameters.EnumSchemas.DBO),null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TiposProcesos>> ObtenerTiposProcesosEmbargos()
        {
            return await GetAsyncList<TiposProcesos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),null, CommandType.StoredProcedure);
        }

        public async Task<Embargo> InsertarEmbargo(InsertEmbargo insertEmbargo)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_WORK_MANAGER), insertEmbargo.EBA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_FECHA_RADICADO_WORK_MANAGER), insertEmbargo.EBA_FECHA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_JUZGADO), insertEmbargo.EBA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_FECHA_RADICADO_JUZGADO), insertEmbargo.EBA_FECHA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_EMAIL_RESPUESTA), insertEmbargo.EBA_EMAIL_RESPUESTA);
            //parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_DIRECCION_RESPUESTA), embargo.EBA_DIRECCION_RESPUESTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_NOMBRE_DEMANDADO), insertEmbargo.EBA_NOMBRE_DEMANDADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_EXPEDIENTE_BANCO_AGRARIO), insertEmbargo.EBA_EXPEDIENTE_BANCO_AGRARIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_REMANENTE), insertEmbargo.EBA_REMANENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_INCONSISTENCIA), insertEmbargo.EBA_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_OBSERVACIONES), insertEmbargo.EBA_OBSERVACIONES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ACLARACION), insertEmbargo.EBA_ACLARACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RESPUESTA), insertEmbargo.EBA_RESPUESTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_ID), insertEmbargo.CLI_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EMB_ID_JUZGADO), insertEmbargo.EMB_ID_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TPE_ID), insertEmbargo.TPE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_CREADOPOR), insertEmbargo.EBA_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TID_ID), insertEmbargo.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), insertEmbargo.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_PROCESO), insertEmbargo.EBA_PROCESO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_DIRECCION), insertEmbargo.EBA_DIRECCION);

            DataTable BeneficiariosDataTable = new DataTable();
            BeneficiariosDataTable.Columns.Add("BPE", typeof(Guid));
            BeneficiariosDataTable.Columns.Add("TID_ID",typeof(int));
            BeneficiariosDataTable.Columns.Add("BPE_NUMERO_IDENTIFICACION", typeof(string));
            BeneficiariosDataTable.Columns.Add("BPE_PRIMER_NOMBRE", typeof(string));
            BeneficiariosDataTable.Columns.Add("BPE_SEGUNDO_NOMBRE", typeof(string));
            BeneficiariosDataTable.Columns.Add("BPE_PRIMER_APELLIDO", typeof(string));
            BeneficiariosDataTable.Columns.Add("BPE_SEGUNGO_APELLIDO", typeof(string));
            BeneficiariosDataTable.Columns.Add("DEMANDANTE", typeof(bool));
            BeneficiariosDataTable.Columns.Add("INSERTAR", typeof(bool));

            DataTable ConceptosDataTable = new DataTable();
            ConceptosDataTable.Columns.Add("ECC_ID", typeof(Guid));
            ConceptosDataTable.Columns.Add("ECC_VALOR", typeof(double));
            ConceptosDataTable.Columns.Add("ECC_FECHA_INICIO_EMBARGO", typeof(DateTime));
            ConceptosDataTable.Columns.Add("ECC_FECHA_FIN_EMBARGO", typeof(DateTime));
            ConceptosDataTable.Columns.Add("ECC_INDICADOR_CESANTIAS", typeof(bool));
            ConceptosDataTable.Columns.Add("TRE_ID ", typeof(Guid));
            ConceptosDataTable.Columns.Add("TIE_ID", typeof(Guid));
            ConceptosDataTable.Columns.Add("CTA_ID", typeof(int));
            ConceptosDataTable.Columns.Add("CCT_ID", typeof(int));
            ConceptosDataTable.Columns.Add("NUMERO_CUENTA", typeof(int));
            ConceptosDataTable.Columns.Add("INSERTAR", typeof(bool));


            foreach (BeneficiariosPagoEmbargo beneficiario in insertEmbargo.BENEFICIARIOS) {
                BeneficiariosDataTable.Rows.Add(null,beneficiario.TID_ID, beneficiario.BPE_NUMERO_IDENTIFICACION, beneficiario.BPE_PRIMER_NOMBRE, beneficiario.BPE_SEGUNDO_NOMBRE, beneficiario.BPE_PRIMER_APELLIDO, beneficiario.BPE_SEGUNGO_APELLIDO, beneficiario.DEMANDANTE,true);
            }

            foreach (InsertEmbargoCuentaConcepto concepto in insertEmbargo.CONCEPTOS) {
                ConceptosDataTable.Rows.Add(null,concepto.ECC_VALOR,concepto.ECC_FECHA_INICIO_EMBARGO,concepto.ECC_FECHA_FIN_EMBARGO,concepto.ECC_INDICADOR_CESANTIAS,concepto.TRE_ID,concepto.TIE_ID,concepto.CTA_ID,concepto.CCT_ID,concepto.NUMERO_CUENTA,true);
            }

            parametros.Add("@LST_BENEFICIARIOS", BeneficiariosDataTable.AsTableValuedParameter("[dbo].[BeneficiariosEmbargos]"));
            parametros.Add("@LST_CONCEPTOS", ConceptosDataTable.AsTableValuedParameter("[dbo].[ConceptosEmbargos]"));

            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }



        public async Task<InsertarPagoEmbargo> InsertarPagoEmbargo(InsertarPagoEmbargo insertPagoEmbarg)
        {

            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID), insertPagoEmbarg.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TRE_ID), insertPagoEmbarg.TRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_CREADOPOR), insertPagoEmbarg.PE_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), insertPagoEmbarg.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TPI_ID), insertPagoEmbarg.TPI_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_NOMBRE), insertPagoEmbarg.PE_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_VALOR), insertPagoEmbarg.PE_VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_OBSERVACIONES), insertPagoEmbarg.PE_OBSERVACIONES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_APROBADO), insertPagoEmbarg.PE_APROBADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.ENE_ENTIDAD_ID), insertPagoEmbarg.ENE_ENTIDAD_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.NUMERO_CUENTA), insertPagoEmbarg.NUMERO_CUENTA);


            return await GetAsyncFirst<InsertarPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<InsertarPagoEmbargo> ObtenerpagoEmbargo(DateTime insertPagoEmbarg)
        {

            DynamicParameters parametros = new DynamicParameters();
          
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.PE_FECHA), insertPagoEmbarg);
           
            return await GetAsyncFirst<InsertarPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Embargo>> ObtenerEmbargos()
        {            
            return await GetAsyncList<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<Embargo> EliminarEmbargo(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID),id);

            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Embargo> ActualizarEmbargo(InsertEmbargo insertEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID), insertEmbargo.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_WORK_MANAGER), insertEmbargo.EBA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_FECHA_RADICADO_WORK_MANAGER), insertEmbargo.EBA_FECHA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_JUZGADO), insertEmbargo.EBA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_FECHA_RADICADO_JUZGADO), insertEmbargo.EBA_FECHA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_EMAIL_RESPUESTA), insertEmbargo.EBA_EMAIL_RESPUESTA);
           // parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_DIRECCION_RESPUESTA), embargo.EBA_DIRECCION_RESPUESTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_NOMBRE_DEMANDADO), insertEmbargo.EBA_NOMBRE_DEMANDADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_EXPEDIENTE_BANCO_AGRARIO), insertEmbargo.EBA_EXPEDIENTE_BANCO_AGRARIO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_REMANENTE), insertEmbargo.EBA_REMANENTE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_INCONSISTENCIA), insertEmbargo.EBA_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_OBSERVACIONES), insertEmbargo.EBA_OBSERVACIONES);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ACLARACION), insertEmbargo.EBA_ACLARACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RESPUESTA), insertEmbargo.EBA_RESPUESTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_ID), insertEmbargo.CLI_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EMB_ID_JUZGADO), insertEmbargo.EMB_ID_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TPE_ID), insertEmbargo.TPE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_CREADOPOR), insertEmbargo.EBA_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TID_ID), insertEmbargo.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), insertEmbargo.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_DIRECCION), insertEmbargo.EBA_DIRECCION);
      
            


            if (insertEmbargo.CONCEPTOS.Count > 0 && insertEmbargo.EBA_INCONSISTENCIA == false && insertEmbargo.BENEFICIARIOS.Count  > 0) {
                DataTable BeneficiariosDataTable = new DataTable();
                BeneficiariosDataTable.Columns.Add("BPE", typeof(Guid));
                BeneficiariosDataTable.Columns.Add("TID_ID", typeof(int));
                BeneficiariosDataTable.Columns.Add("BPE_NUMERO_IDENTIFICACION", typeof(string));
                BeneficiariosDataTable.Columns.Add("BPE_PRIMER_NOMBRE", typeof(string));
                BeneficiariosDataTable.Columns.Add("BPE_SEGUNDO_NOMBRE", typeof(string));
                BeneficiariosDataTable.Columns.Add("BPE_PRIMER_APELLIDO", typeof(string));
                BeneficiariosDataTable.Columns.Add("BPE_SEGUNGO_APELLIDO", typeof(string));
                BeneficiariosDataTable.Columns.Add("DEMANDANTE", typeof(bool));
                BeneficiariosDataTable.Columns.Add("BPE_BENEFICIARIO", typeof(bool));
                BeneficiariosDataTable.Columns.Add("INSERTAR", typeof(bool));


                DataTable ConceptosDataTable = new DataTable();
                ConceptosDataTable.Columns.Add("ECC_ID", typeof(Guid));
                ConceptosDataTable.Columns.Add("ECC_VALOR", typeof(double));
                ConceptosDataTable.Columns.Add("ECC_FECHA_INICIO_EMBARGO", typeof(DateTime));
                ConceptosDataTable.Columns.Add("ECC_FECHA_FIN_EMBARGO", typeof(DateTime));
                ConceptosDataTable.Columns.Add("ECC_INDICADOR_CESANTIAS", typeof(bool));
                ConceptosDataTable.Columns.Add("TRE_ID ", typeof(Guid));
                ConceptosDataTable.Columns.Add("TIE_ID", typeof(Guid));
                ConceptosDataTable.Columns.Add("CTA_ID", typeof(int));
                ConceptosDataTable.Columns.Add("CCT_ID", typeof(int));
                ConceptosDataTable.Columns.Add("NUMERO_CUENTA", typeof(int));
                ConceptosDataTable.Columns.Add("INSERTAR", typeof(bool));
                foreach (BeneficiariosPagoEmbargo beneficiario in insertEmbargo.BENEFICIARIOS)
                {
                    BeneficiariosDataTable.Rows.Add(beneficiario.BPE_ID, beneficiario.TID_ID, beneficiario.BPE_NUMERO_IDENTIFICACION, beneficiario.BPE_PRIMER_NOMBRE, beneficiario.BPE_SEGUNDO_NOMBRE, beneficiario.BPE_PRIMER_APELLIDO, beneficiario.BPE_SEGUNGO_APELLIDO, beneficiario.DEMANDANTE, beneficiario.INSERTAR, beneficiario.BPE_BENEFICIARIO);
                }
           
                foreach (InsertEmbargoCuentaConcepto concepto in insertEmbargo.CONCEPTOS)
                {
                    ConceptosDataTable.Rows.Add(concepto.ECC_ID, concepto.ECC_VALOR, concepto.ECC_FECHA_INICIO_EMBARGO, concepto.ECC_FECHA_FIN_EMBARGO, concepto.ECC_INDICADOR_CESANTIAS, concepto.TRE_ID, concepto.TIE_ID, concepto.CTA_ID, concepto.CCT_ID, concepto.NUMERO_CUENTA, concepto.INSERTAR);
                }

                parametros.Add("@LST_BENEFICIARIOS", BeneficiariosDataTable.AsTableValuedParameter("[dbo].[BeneficiariosEmbargos]"));
                parametros.Add("@LST_CONCEPTOS", ConceptosDataTable.AsTableValuedParameter("[dbo].[ConceptosEmbargos]"));
            }

           
            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Embargo> ActualizarEstadoEmbargo(ActualizarEstadoEmbargo insertEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID), insertEmbargo.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_INCONSISTENCIA), insertEmbargo.EBA_INCONSISTENCIA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ACLARACION), insertEmbargo.EBA_ACLARACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_CREADOPOR), insertEmbargo.EBA_CREADOPOR);
 
            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<EmbargoCuentaConcepto> InsertarEmbargoCuentaConcepto(EmbargoCuentaConcepto embargoCuentaConcepto)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_VALOR), embargoCuentaConcepto.ECC_VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_FECHA_INICIO_EMBARGO), embargoCuentaConcepto.ECC_FECHA_INICIO_EMBARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_FECHA_FIN_EMBARGO), embargoCuentaConcepto.ECC_FECHA_FIN_EMBARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_INDICADOR_CESANTIAS), embargoCuentaConcepto.ECC_INDICADOR_CESANTIAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.EBA_ID), embargoCuentaConcepto.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TRE_ID), embargoCuentaConcepto.TRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TIE_ID), embargoCuentaConcepto.TIE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.CTA_ID), embargoCuentaConcepto.CTA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.CCT_ID), embargoCuentaConcepto.CCT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.EBA_CREADOPOR), embargoCuentaConcepto.EBA_CREADOPOR);

            return await GetAsyncFirst<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Embargo> ObtenerEmbargoAdministradorDeTrabajo(string numero)
        {

            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_WORK_MANAGER), numero);

            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargosCuentaConcepto()
        {
            return await GetAsyncList<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<EmbargoCuentaConcepto> EliminarEmbargoCuentaConcepto(Guid id, string descripcion)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_ID), id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_DESCRIPCION), descripcion);
            return await GetAsyncFirst<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<EmbargoCuentaConcepto> ActualizarEmbargoCuentaConcepto(EmbargoCuentaConcepto embargoCuentaConcepto)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_ID), embargoCuentaConcepto.ECC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_VALOR), embargoCuentaConcepto.ECC_VALOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_FECHA_INICIO_EMBARGO), embargoCuentaConcepto.ECC_FECHA_INICIO_EMBARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_FECHA_FIN_EMBARGO), embargoCuentaConcepto.ECC_FECHA_FIN_EMBARGO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_INDICADOR_CESANTIAS), embargoCuentaConcepto.ECC_INDICADOR_CESANTIAS);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.EBA_ID), embargoCuentaConcepto.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TRE_ID), embargoCuentaConcepto.TRE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TIE_ID), embargoCuentaConcepto.TIE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.CTA_ID), embargoCuentaConcepto.CTA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.CCT_ID), embargoCuentaConcepto.CCT_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.EBA_ACTUALIZADOPOR), embargoCuentaConcepto.EBA_ACTUALIZADOPOR);

            return await GetAsyncFirst<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<TipoRetencion>> ObtenerTiposRetenciones()
        {
            return await GetAsyncList<TipoRetencion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<BeneficiariosPagoEmbargo> InsertarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargo beneficiariosPagoEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.EBA_ID), beneficiariosPagoEmbargo.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.TID_ID), beneficiariosPagoEmbargo.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_NUMERO_IDENTIFICACION), beneficiariosPagoEmbargo.BPE_NUMERO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_PRIMER_NOMBRE), beneficiariosPagoEmbargo.BPE_PRIMER_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_SEGUNDO_NOMBRE), beneficiariosPagoEmbargo.BPE_SEGUNDO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_PRIMER_APELLIDO), beneficiariosPagoEmbargo.BPE_PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_SEGUNGO_APELLIDO), beneficiariosPagoEmbargo.BPE_SEGUNGO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_CREADOPOR), beneficiariosPagoEmbargo.BPE_CREADOPOR);

            return await GetAsyncFirst<BeneficiariosPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<BeneficiariosPagoEmbargo> ActualizarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargo beneficiariosPagoEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_ID), beneficiariosPagoEmbargo.BPE_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.EBA_ID), beneficiariosPagoEmbargo.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.TID_ID), beneficiariosPagoEmbargo.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_NUMERO_IDENTIFICACION), beneficiariosPagoEmbargo.BPE_NUMERO_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_PRIMER_NOMBRE), beneficiariosPagoEmbargo.BPE_PRIMER_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_SEGUNDO_NOMBRE), beneficiariosPagoEmbargo.BPE_SEGUNDO_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_PRIMER_APELLIDO), beneficiariosPagoEmbargo.BPE_PRIMER_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_SEGUNGO_APELLIDO), beneficiariosPagoEmbargo.BPE_SEGUNGO_APELLIDO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_ACTUALIZADOPOR), beneficiariosPagoEmbargo.BPE_ACTUALIZADOPOR);

            return await GetAsyncFirst<BeneficiariosPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<BeneficiariosPagoEmbargo> EliminarBeneficiariosPagoEmbargos(Guid id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.BPE_ID), id);
            return await GetAsyncFirst<BeneficiariosPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<BeneficiariosPagoEmbargo>> ObtenerBeneficiariosEmbargo(Guid idEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumBeneficiariosPagoEmbargo.EBA_ID), idEmbargo);

            return await GetAsyncList<BeneficiariosPagoEmbargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Embargo>> ObtenerEmbargosRangoFiltros(ConsultaEmbargosfiltro filtro)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.FECHA_INICIAL), filtro.FECHA_INICIAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.FECHA_FINAL), filtro.FECHA_FINAL);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TID_ID), filtro.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), filtro.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_WORK_MANAGER), filtro.EBA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ESTADO), filtro.Estado);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ESTADO_EMBARGO), filtro.Estados);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_CREADOPOR), filtro.UsrId);
            return await GetAsyncList<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);  
        }

        public async Task<Desembargo> InsertarDesembargo(Desembargo id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_ID), id.ECC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.DES_OBSERVACIONES), id.DES_OBSERVACIONES);
            return await GetAsyncFirst<Desembargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerDesembargo>> ObtenerDesembargo()
        {
            return await GetAsyncList<ObtenerDesembargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);    
        }

        public async Task<TipoIdentificacion> ObtenerTipoIdentificacionPorId(int tipo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.TID_ID), tipo);

            return await GetAsyncFirst<TipoIdentificacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }



        public async Task<Embargo> ObtenerEmbargoPorId(Guid idEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID), idEmbargo);

            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);         
        }

        public async Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargoCuentaConceptoPorId(Guid idEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_ID), idEmbargo);

            return await GetAsyncList<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<Embargo> InsertarEmbargoGestorDocumental(InsertEmbargoGestorDocumental embargo)
        {
            DynamicParameters parametros = new DynamicParameters();          
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_WORK_MANAGER), embargo.EBA_RADICADO_WORK_MANAGER);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_RADICADO_JUZGADO), embargo.EBA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_FECHA_RADICADO_JUZGADO), embargo.EBA_FECHA_RADICADO_JUZGADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_NOMBRE_DEMANDADO), embargo.EBA_NOMBRE_DEMANDADO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_ID), embargo.CLI_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.EBA_CREADOPOR), embargo.EBA_CREADOPOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.TID_ID), embargo.TID_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), embargo.CLI_IDENTIFICACION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CODIGO_INTERNO), embargo.CODIGO_INTERNO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.Modulo), embargo.Modulo);

            return await GetAsyncFirst<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<EmbargoCuentaConcepto>> ObtenerEmbargosConceptoCuentaByCuentaId(int idCuenta)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CTA_ID), idCuenta);

            var a = await GetAsyncList<EmbargoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

            return a;
        }

        public async Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorDepartamentoCiudadCodigo(int? IdDepartamento, int? IdCiudad, string? Codigo, string? CuentaDeposito, int? Estado)
        {
            DynamicParameters parametros = new();    
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_INTERNO), Codigo);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPD), IdDepartamento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ID_DPC), IdCiudad);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NRO_CUENTA), CuentaDeposito);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.ESTADO), Estado);
            return await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Juzgados>> ObtenerJuzgadosPorNumeroCuentaCodigo(string nCuenta, string codigoInterno)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.NRO_CUENTA), nCuenta);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumJuzgadosParameters.CODIGO_INTERNO), codigoInterno);            
            return await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);

        }

        public async Task<bool> CambiarEstadoEmbargo(Guid idEmbargoCuentaConcepto)
        {
            DynamicParameters parametros = new();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoscuentaConcepto.ECC_ID), idEmbargoCuentaConcepto);
            await GetAsyncList<Juzgados>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return true;
        }

        public async Task<IEnumerable<Embargo>> ObtenerEmbargoByIdentificacion(string idEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumEmbargoParameters.CLI_IDENTIFICACION), idEmbargo);

            return await GetAsyncList<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<bool> AfectacionEmbargoMasivo(EmbargarMasivoDto embargarMasivoDto)
        {
            DynamicParameters parametros = new DynamicParameters();


            foreach (var item in embargarMasivoDto.ListaMasiva)
            {
                parametros.Add("@TipoEmbargo", item.TIE_ID);
                parametros.Add("@Valor", item.ECC_VALOR);
                parametros.Add("@FechaInicio", item.ECC_FECHA_INICIO_EMBARGO);
                parametros.Add("@FechaFin", item.ECC_FECHA_FIN_EMBARGO);
                parametros.Add("@TipoRetencion", item.TRE_ID);
                parametros.Add("@IndicadorCesantias", item.ECC_INDICADOR_CESANTIAS);
                parametros.Add("@IdCuenta", embargarMasivoDto.IdCuenta);

            }

            await GetAsyncList<Embargo>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
            return true;
        }
        public async Task<IEnumerable<EstadoCuentaDto>> ObtenerEctEstadoCuenta()
        {
            var response = await GetAsyncList<EstadoCuentaDto>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);

            return response;
        }

        public async Task<bool> InsertarPagosEmbargosSeven(PagosEmbargosSevenRespuesta embargosSevenRespuesta)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.EmbargoID), embargosSevenRespuesta.EBA_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.CodigoProveedor), embargosSevenRespuesta.PEM_CODIGO_PROVEEDOR);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.CDP), embargosSevenRespuesta.PEM_CDP);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.NumeroFactura), embargosSevenRespuesta.PEM_NUMERO_FACTURA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.Flujo), embargosSevenRespuesta.PEM_FLUJO);



            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<PagosEmbargosSevenRespuesta> ConsultarDatosEmbargosPagosSeven(Guid idEmbargo)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumPagosEmbargosSevenRespuesta.EmbargoID), idEmbargo);
            return await GetAsyncFirst<PagosEmbargosSevenRespuesta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }



    }

   


}
