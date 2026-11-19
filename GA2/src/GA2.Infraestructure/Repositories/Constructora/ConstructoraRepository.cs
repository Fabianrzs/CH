using Azure.Core;
using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Constructora;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Constructora;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public class ConstructoraRepository: DapperGenericRepository, IConstructoraRepository
    {
        public ConstructoraRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Constructora> AgregaConstructora(Constructora constructora)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.NombreConstructora), constructora.NombreConstructora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Direccion), constructora.Direccion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.IdDepartamento), constructora.IdDepartamento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.IdCiudad), constructora.IdCiudad);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Email), constructora.Email);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Nit), constructora.Nit);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Telefono), constructora.Telefono);

            return await GetAsyncFirst<Constructora>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<int> ActualizarConstructora(Constructora constructora)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@Id_Constructuora", constructora.Id_Constructuora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.NombreConstructora), constructora.NombreConstructora);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Direccion), constructora.Direccion);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.IdDepartamento), constructora.IdDepartamento);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.IdCiudad), constructora.IdCiudad);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Email), constructora.Email);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Nit), constructora.Nit);
            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Telefono), constructora.Telefono);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Constructora>> ConsultarConstructora()
        {
            return await GetAsyncList<Constructora>(HelperDBParameters.BuilderFunction(
                        HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<Constructora> ConsultarConstructoraById(Guid constructora)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Id_Constructuora), constructora);

            return await GetAsyncFirst<Constructora>(HelperDBParameters.BuilderFunction(
            HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
        public async Task<int> EliminarConstructora(Guid constructora)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add(HelpersEnums.GetEnumDescription(EnumConstructoraParameter.Id_Constructuora), constructora);

            return await GetAsyncFirst<int>(HelperDBParameters.BuilderFunction(
            HelperDBParameters.EnumSchemas.DBO), parametros, CommandType.StoredProcedure);
        }
    }
}
