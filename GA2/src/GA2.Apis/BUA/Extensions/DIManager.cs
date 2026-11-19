using GA2.Application.Main;
using GA2.Application.Main.ProviderRequest;
using GA2.Application.Main.ProviderRequest.Auditoria;
using GA2.Application.Main.ProviderRequest.Reintegro;
using GA2.Application.Main.UserProvider;
using GA2.Domain.Core;
using GA2.Domain.Core.Connected_Services.ServiceReference1;
using GA2.Domain.Core.ReferenciasSvc.Bua;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace GA2.Apis.BUA.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DIManager
    {
        /// <summary>
        /// Metodo que inyecta las dependencias del proyecto
        /// </summary>
        /// <param name="services">Proveedor de servicios</param>
        /// <returns>Proveedor de servicios</returns>
        internal static IServiceCollection AddServicesDependencies(this IServiceCollection services)
        {

            #region AutoMapper
            services.AddAutoMapper(typeof(IdentityMapper));
            services.AddAutoMapper(typeof(BUAMapper));


            #endregion

            #region Main 
            services.AddTransient<ITokenClaims, TokenClaims>();
            services.AddTransient<ICryptoGA2, CryptoGA2>();
            services.AddTransient<IIntegracionesGA2Bua, IntegracionesGA2>();
            services.AddTransient<IProviderRequestG2, ProviderRequestG2>();
            services.AddTransient<IGetUserProvider, GetUserProvider>();
            services.AddTransient<IEmbargosProvider, EmbargoProvider>();
            services.AddTransient<IAuditoriaProvider, AuditoriaProvider>();
            services.AddTransient<IReintegroProviders, ReintegroProvider>();



            #endregion

            #region Domain
            
            services.AddTransient<ILogErrorBusinessLogic, LogErrorBusinessLogic>();
            services.AddTransient<IClientesBusinessLogic, ClientesBusinessLogic>();
            services.AddTransient<IAfiliacionesBusinessLogic, AfiliacionesBusinessLogic>();
            services.AddTransient<IIntegracionesGa2BuaBusinessLogic, IntegracionesGA2BuaBusinessLogic>();

            services.AddTransient<ICuentasConceptosBusinessLogic, CuentasConceptosBusinessLogic>();
            
            services.AddTransient<ICierreMensualCuentasBusinessLogic, CierreMensualCuentasBusinessLogic>();
            services.AddTransient<ICierreMensualBusinessLogicBua, CierreMensualBusinessLogicBua>();

            services.AddTransient<ICierreAnualCuentasBusinessLogic, CierreAnualCuentasBusinessLogic>();
            services.AddTransient<ICierreAnualCuentasBusinessLogicBua, CierreAnualCuentasBusinessLogicBua>();

            services.AddTransient<ICuentasBusinessLogic, NuevaCuentasConceptosBusinessLogic>();

            services.AddTransient<IBuaService, BuaService>();
            services.AddTransient<IBuaService2BusinessLogic, BuaService2BusinessLogic>();
            services.AddTransient<IConsumoBua, ConsumoBua>();

            #endregion

            #region Inrfraestructure Repository

            services.AddTransient<ILogErrorRepository, LogErrorRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IAfiliacionesRepository, AfiliacionesRepository>();
            services.AddTransient<IBlobStorageGenericRepository, BlobStorageGenericRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<ICuentasConceptosRepository, CuentasConceptosRepository>();
            services.AddTransient<ICierreMensualRepository, CierreMensualRepository>();
            services.AddTransient<ICierreMensualRepositoryBua, CierreMensualRepositoryBua>();
            services.AddTransient<ICierreAnualCuentasRepository, CierreAnualCuentasRepository>();
            services.AddTransient<ICierreAnualCuentasRepositoryBua, CierreAnualCuentasRepositoryBua>();

            #endregion

            return services;
        }
    }
}
