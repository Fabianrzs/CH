using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public  class CuentasClienteWorkRepository : DapperGenericWorkerRepository
    {
        private readonly ILogger<CuentasClienteWorkRepository> _logger;

        public CuentasClienteWorkRepository(IConfiguration configuration, ILogger<CuentasClienteWorkRepository> logger) : base(configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<InsertCuentaConcepto>> ObtenerCuentasCliente(Guid idCliente)
        {
            _logger.LogInformation($"Buscando cuentas del cliente id: {idCliente}");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CLI_ID), idCliente);
            return await GetAsyncListBUA<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<InsertCuentaConcepto> InsertarCuentaCliente(InsertCuentaConcepto cuenta)
        {
            _logger.LogInformation($"Insertando cuenta para el cliente {cuenta.CLI_ID}");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_ID_INTEGRACION), cuenta.CTA_ID_INTEGRACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_NUMERO), cuenta.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CLI_ID), cuenta.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.TCT_ID), cuenta.TCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.ECT_ID), cuenta.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CCN_ID), cuenta.CCN_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.DCT_ID), cuenta.DCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO), cuenta.CTA_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_CANJE), cuenta.CTA_SALDO_CANJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_CUOTAS), cuenta.CTA_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_BLOQUEO), cuenta.CTA_BLOQUEO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_DISPONIBLE), cuenta.CTA_SALDO_DISPONIBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.EJE_UNIDAD_EJECUTORA), cuenta.EJE_UNIDAD_EJECUTORA);

            return await GetAsyncFirtsOrDefaultBUA<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }

        public async Task<InsertCuentaConcepto> InsertarConceptoCuenta(InsertCuentaConcepto cuenta)
        {
            _logger.LogInformation($"Insertando cuenta para el cliente {cuenta.CLI_ID}");
            DynamicParameters parameters = new DynamicParameters();
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_ID_INTEGRACION), cuenta.CTA_ID_INTEGRACION);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_NUMERO), cuenta.CTA_NUMERO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CLI_ID), cuenta.CLI_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.TCT_ID), cuenta.TCT_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.ECT_ID), cuenta.ECT_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CCN_ID), cuenta.CCN_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.DCT_ID), cuenta.DCT_ID);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO), cuenta.CTA_SALDO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_CANJE), cuenta.CTA_SALDO_CANJE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_CUOTAS), cuenta.CTA_CUOTAS);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_BLOQUEO), cuenta.CTA_BLOQUEO);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.CTA_SALDO_DISPONIBLE), cuenta.CTA_SALDO_DISPONIBLE);
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumNuevaCuentaConceptoParameters.EJE_UNIDAD_EJECUTORA), cuenta.EJE_UNIDAD_EJECUTORA);

            return await GetAsyncFirtsOrDefaultBUA<InsertCuentaConcepto>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, typeCommand: CommandType.StoredProcedure);
        }
    }
}
