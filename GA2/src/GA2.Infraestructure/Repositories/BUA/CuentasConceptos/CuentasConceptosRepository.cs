using Azure.Core;
using Dapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAC;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVAV;
using GA2.Domain.Entities.VerificacionDerechos.CuentasAVC;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Verificacion;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Categoria = GA2.Application.Dto.Categoria;

namespace GA2.Infraestructure.Repositories
{
    public class CuentasConceptosRepository : DapperGenericRepository, ICuentasConceptosRepository
    {
        public CuentasConceptosRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<BloqueoCuentaConcepto> InsertarBoqueoCuentaConcecto(BloqueoCuentaConcepto bloqueo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.CSB_ID), bloqueo.CSB_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.CTA_ID), bloqueo.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.CCT_ID), bloqueo.CCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.BCC_TIPO_BLOQUEO), bloqueo.BCC_TIPO_BLOQUEO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.BCC_MONTO), bloqueo.BCC_MONTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.BCC_VALOR), bloqueo.BCC_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumBloqueoCuentaConceptoParameters.BCC_CREADOPOR), bloqueo.BCC_CREADOPOR);

            var a = await GetAsyncFirst<BloqueoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
            return a;
        }

        public async Task<InsertCuentaConcepto> InsertarNuevaCuenta(InsertCuentaConcepto bloqueo)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_ID_INTEGRACION), bloqueo.CTA_ID_INTEGRACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_NUMERO), bloqueo.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CLI_ID), bloqueo.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.TCT_ID), bloqueo.TCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.ECT_ID), bloqueo.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CCN_ID), bloqueo.CCN_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.DCT_ID), bloqueo.DCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO), bloqueo.CTA_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_CANJE), bloqueo.CTA_SALDO_CANJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_CUOTAS), bloqueo.CTA_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_BLOQUEO), bloqueo.CTA_BLOQUEO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_DISPONIBLE), bloqueo.CTA_SALDO_DISPONIBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.EJE_UNIDAD_EJECUTORA), bloqueo.EJE_UNIDAD_EJECUTORA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_PORCENTAJE), bloqueo.CTA_PORCENTAJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_ASIGNACION_SALARIAL), bloqueo.CTA_ASIGNACION_SALARIAL);

            return await GetAsyncFirst<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalBloqueoCuenta>> ObtenerCausalBloqueoCuentasConceptos()
        {
            return await GetAsyncList<CausalBloqueoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<InfoCliente> ObtenerInformacionCliente(int tpo_identificacion, string num_identificacion)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientConsultaParameters.TID_ID), tpo_identificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientConsultaParameters.CLI_IDENTIFICACION), num_identificacion);

            return await GetAsyncFirst<InfoCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InsertCuentaConcepto> ValidarExisteCuenta(int num_cuenta)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_NUMERO), num_cuenta);

            return await GetAsyncFirst<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CuentaPorConcepto>> ObtenerCuentaConcepto(int idCuenta)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_ID), idCuenta);
            return await GetAsyncList<CuentaPorConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCuenta"></param>
        /// <param name="tipoCuenta"></param>
        /// <returns></returns>
        public async Task<InsertCuentaConcepto> ValidarExisteCuentaTipo(int idCuenta, int? tipoCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CLI_ID), idCuenta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.TCT_ID), tipoCuenta);

            return await GetAsyncFirst<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CuentaCliente>> ObtenerCuentasCliente(int clienteId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientConsultaParameters.CLI_ID), clienteId);
            return await GetAsyncList<CuentaCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoCuenta>> ObtenerConceptosCuentas(int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);
            return await GetAsyncList<ConceptoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AportesCliente>> ObtenerAportesCategoriaCliente(int clienteId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientConsultaParameters.CLI_ID), clienteId);
            return await GetAsyncList<AportesCliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MovimientosCuenta>> ObtenerMovimientosCuentaAfiliado(int cuentaId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);
            return await GetAsyncList<MovimientosCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<MovimientosCuenta>> ObtenerMovimientosConceptos(int cuentaId, int conceptoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CNC_ID), conceptoId);

            return await GetAsyncList<MovimientosCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<CuentaPorConcepto> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId, int conceptoId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CLI_ID), clienteId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CCT_ID), conceptoId);

            return await GetAsyncFirst<CuentaPorConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BloqueoCuentaConcepto>> ObtenerBloqueosCuentasConceptos(int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);

            return await GetAsyncList<BloqueoCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InclusionAportesRequest>> ObtenerInclusionSaldos(int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), cuentaId);

            return await GetAsyncList<InclusionAportesRequest>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InclusionAportes>> ObtenerInclusionSaldoOpciones(int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_ID), cuentaId);

            return await GetAsyncList<InclusionAportes>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DatosVerificacionDerechoCuenta>> ObtenerDatosVerificacionDerechosCuentas(VerificacionDerechosByFechaDto request)
        {
            try
            {

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
                parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

                return await GetAsyncList<DatosVerificacionDerechoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public  async Task<DatosVerificacionDerechoCuenta> ActualizarECTCuentas(ActualizarEctCuentasDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoActualizarECTCuentas.CTA_ID), request.ctaID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechoActualizarECTCuentas.ECT_ID), request.EctId);
          
            return await GetAsyncFirst<DatosVerificacionDerechoCuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<int> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request)
        {
            DynamicParameters parameters = new DynamicParameters();
            var dataTable = CrearDataTableECT(request);
            parameters.Add(name: "@Lista", value: dataTable, dbType: DbType.Object, direction: ParameterDirection.Input);
            try
            {

                return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            }
            catch (Exception E)
            {

                throw;
            }
        }
        private DataTable CrearDataTableECT(List<ActualizarEctCuentasDto> lista)
        {
            var table = new DataTable();
            table.Columns.Add("CtaID", typeof(int));
            table.Columns.Add("EctId", typeof(int));

            foreach (var item in lista)
            {
                table.Rows.Add(item.ctaID, item.EctId);
            }

            return table;
        }

        public async Task<IEnumerable<DatosCuentasAdministracionCesantias>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

            return await GetAsyncList<DatosCuentasAdministracionCesantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<DatosInfoCensatiaByCuentaPerioso> ObtenerDatosCesantiasByCuentaId(ObtenerInfoCedulaRequest request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CuentaId", request.CuentaId);
            parameters.Add("@Periodo", request.Periodo);

            var result = await GetAsyncFirst<DatosInfoCensatiaByCuentaPerioso>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;    
           }
        public async Task<IEnumerable<DatosCedulasCesantias>> ObtenerCedulaCuentaCesantidas()
        {
            return await GetAsyncList<DatosCedulasCesantias>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DatosCuentasAV>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

            return await GetAsyncList<DatosCuentasAV>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVAC>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

            return await GetAsyncList<ObtenerCuentasAVAC>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ObtenerCuentasAVAV>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

            return await GetAsyncList<ObtenerCuentasAVAV>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVAV()
        {
            return await GetAsyncList<ObtenerCuentaAVAVResponse>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVAC()
        {
            return await GetAsyncList<ObtenerCuentaAVAVResponse>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ObtenerCuentaAVAVResponse>> ObtenerCedulaCuentaAVC()
        {
            return await GetAsyncList<ObtenerCuentaAVAVResponse>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ObtenerCuentasAVC>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_INICIAL), request.FechaInicial);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumVerificacionDerechosFiltroFechas.FECHA_FINAL), request.FechaFinal);

            return await GetAsyncList<ObtenerCuentasAVC>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<ConceptoCuenta> validarSaldoCuentaConcepto(int conceptoId, int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), conceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);

            var reuslt = await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }
        public async Task<ConceptoCuenta> ActualizarSaldoAndValorDevolver(int coceptoId, int cuentaId, double valorSueldo,double bloqueo, int tipoReintegro)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorDevuelto), valorSueldo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorBloqueo), bloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.Transaccion), tipoReintegro);

            var reuslt = await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }
        public async Task<ConceptoCuenta> ActualizarSaldoAndValorDevolverDR(int coceptoId, int cuentaId, double valorSueldo, double bloqueo, double valorDisponible)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorDevuelto), valorSueldo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorBloqueo), bloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorSaldo), valorDisponible);

            var reuslt = await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }
        public async Task<ConceptoCuenta> CreateLogMovimientoCuenta(int coceptoId, int cuentaId, double valorBloqueo, double valorSaldo, int idReintegro)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorSaldo), valorSaldo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorBloqueo), valorBloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.IdTipoReintegro), idReintegro);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.DateCreation), DateTime.Now);

            var reuslt = await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }

        public async Task<bool> ActualizarValorRegresar(int coceptoId, int cuentaId, double valorBloqueo, double valorSaldo,int idReintegro)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorSaldo), valorSaldo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorBloqueo), valorBloqueo);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.Transaccion), idReintegro);

            var reuslt = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }
        public async Task<bool> ProcesarSaldoReintegro(int coceptoId, int cuentaId,  double valorSaldo)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ValorSaldo), valorSaldo);

            var reuslt = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }

        public async Task<LogConceptoCuenta> GetDataLogConceptoCuenta(int coceptoId, int cuentaId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.ConceptoId), coceptoId);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaConceptoById.CuentaId), cuentaId);

            var reuslt = await GetAsyncFirst<LogConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }

        public async Task<bool> AfectacionConceptoMasivoEmbargo(int CTA_ID, int CNC_ID, int CCT_SALDO)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CTA_ID", CTA_ID);
            parameters.Add("@CNC_ID", CNC_ID);
            parameters.Add("@CCT_SALDO", CCT_SALDO);
            

            var resul = await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return resul;
        }
        public async Task<Domain.Entities.Movimiento> InsertarMovimientoReitegroUnidadEjecutora(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Cuenta), movimiento.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Concepto), movimiento.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Valor), movimiento.MVT_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.TipoMovimiento), movimiento.CAT_TIPO_MOVIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.FechaMovimiento), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.FechaProceso), DateTime.Now);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.AñoAportes), DateTime.Now.Year);

            //PREGUNTAR AL INGE SI ES RELEVANTE
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.IdDocumento), movimiento.DCT_ID);

            return await GetAsyncFirst<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
         public async Task<Domain.Entities.Movimiento> InsertarLogMovimientoReintegro(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("LMR_CONCEPTO_ID", movimiento.CNC_ID);
            parameters.Add("LMR_CUENTA_ID", movimiento.CTA_ID);
            parameters.Add("LMR_MOVIMIENTO_ID", movimiento.MVT_ID);
            parameters.Add("@LMR_DOCUMENTO_ID", movimiento.DCT_ID);
            parameters.Add("@LMR_FECHA_MOVIMIENTO", DateTime.Now);
            parameters.Add("@LMR_VALOR", movimiento.MVT_VALOR);

            
            return await GetAsyncFirst<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<MovimientoReintro>> ObtenerMovimientoReintegro()
        {

            return await GetAsyncList<MovimientoReintro>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }
    }
}
