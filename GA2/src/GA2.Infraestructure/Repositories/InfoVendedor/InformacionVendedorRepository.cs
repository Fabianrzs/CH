using Dapper;
using GA2.Application.Dto.InfoVendedor;
using GA2.Domain.Entities.InfoVendedor;
using GA2.Infraestructure.Data.InfoVendedor;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.InfoVendedor
{
    public class InformacionVendedorRepository : DapperGenericRepository, IInformacionVendedorRepository
    {
        public InformacionVendedorRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<InformacionVendedor> CrearInformacionVendedor(InformacionVendedor informacionVendedor)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.SOC_ID), informacionVendedor.SOC_ID);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_TIPO_DOC), informacionVendedor.IVEN_TIPO_DOC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_NRO_DOC), informacionVendedor.IVEN_NRO_DOC);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_NOMBRE), informacionVendedor.IVEN_NOMBRE);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_DIRECCION), informacionVendedor.IVEN_DIRECCION);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_DEPARTAMANTO), informacionVendedor.IVEN_DEPARTAMANTO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_CIUDAD), informacionVendedor.IVEN_CIUDAD);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_CORREO), informacionVendedor.IVEN_CORREO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_TELEFONO), informacionVendedor.IVEN_TELEFONO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_FECHA_ENTREGA), informacionVendedor.IVEN_FECHA_ENTREGA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_NRO_CUENTA), informacionVendedor.IVEN_NRO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_TIPO_CUENTA), informacionVendedor.IVEN_TIPO_CUENTA);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_BANCO), informacionVendedor.IVEN_BANCO);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.PVD_CODD), informacionVendedor.PVD_CODD);


            return await GetAsyncFirst<InformacionVendedor>(HelperDBParameters.BuilderFunction(
                    HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<InformacionVendedor>> ObtenerInformacionVendedor(consultarVendedorDto request)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.SOC_ID), request.Soc_Id);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.SLS_ID), request.Sls_Id);

            return await GetAsyncList<InformacionVendedor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<InformacionVendedor> ObtenerInformacionVendedorPorIdSolicCredito(Guid soc_id)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.SOC_ID), soc_id);

            return await GetAsyncFirst<InformacionVendedor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionVendedor>> EliminarInformacionVendedor(InformacionVendedor informacionVendedor)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumInformacionVendedor.IVEN_ID), informacionVendedor.IVEN_ID);

            return await GetAsyncList<InformacionVendedor>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}
