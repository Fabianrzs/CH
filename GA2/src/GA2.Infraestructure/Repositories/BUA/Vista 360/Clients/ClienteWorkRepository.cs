using Dapper;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ClienteWorkRepository : DapperGenericWorkerRepository, IClienteWorkRepository
    {
        public ClienteWorkRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Application.Dto.CuerpoArchivoDto> InsertarClienteNomina(Application.Dto.CuerpoArchivoDto idCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_CODIGO_MILITAR), idCuenta.CODIGO_MILITAR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), idCuenta.DIGITO_FUERZA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.GRD_ID), idCuenta.GRADO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_APELLIDO), idCuenta.PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_NOMBRE), idCuenta.PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_APELLIDO), idCuenta.SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_NOMBRE), idCuenta.SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), idCuenta.TIPO_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), idCuenta.IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), idCuenta.UNIDAD_EJECUTORA);

            return await GetAsyncFirtsOrDefaultBUA<Application.Dto.CuerpoArchivoDto>(HelperDBParameters.BuilderFunction(
                  HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Metodo Obtener cliente mediante tipo y número de identificacion
        /// </summary>
        /// <param name="identificationType"></param>
        /// <param name="identificationNumber"></param>
        /// <returns>Client</returns>
        public async Task<Cliente> ObtenerClientePorTipoIdentificationYNumero(int identificationType, string identificationNumber)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), identificationType);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), identificationNumber);

            return await GetAsyncFirtsOrDefaultBUA<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
