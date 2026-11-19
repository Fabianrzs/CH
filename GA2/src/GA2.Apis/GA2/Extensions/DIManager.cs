using GA2.Application.Main;
using GA2.Application.Main.ProviderRequest;
using GA2.Application.Main.UserProvider;
using GA2.Domain.Core;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.Credito.WorkManager;
using GA2.Domain.Core.DatosAliadoEstrategico;
using GA2.Domain.Core.Embargos.GestorDocumental;
using GA2.Domain.Core.Escrituras;
using GA2.Domain.Core.FichaAnalista;
using GA2.Domain.Core.InfoVendedor;
using GA2.Domain.Core.PAXC;
using GA2.Domain.Core.Postulacion;
using GA2.Domain.Core.Postulacion.WorkManager;
using GA2.Domain.Core.PuntosAtencion;
using GA2.Domain.Core.ReferenciasSvc.Bua;
using GA2.Domain.Core.ReferenciasSvc.Fenix;
using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Domain.Core.ReferenciasSvc.Otp;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaProductoTercero;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTercero;
using GA2.Domain.Core.ReferenciasSvc.Vigia.CargaTransaccion;
using GA2.Domain.Core.ReferenciasSvc.Vigia.VerificacionTercero;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Domain.Core.Reportes.ReportesXMlClosed;
using GA2.Domain.Core.ReporteSimulacionBL;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Domain.Core.Servicios.Fenix;
using GA2.Domain.Core.Servicios.Haberes;
using GA2.Domain.Core.Servicios.Vigia.CargaProductoTercero;
using GA2.Domain.Core.Servicios.Vigia.CargaTercero;
using GA2.Domain.Core.Servicios.Vigia.CargaTransaccion;
using GA2.Domain.Core.Servicios.Vigia.VerificaTercero;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Domain.Core.Solicitud.WorkManager;
using GA2.Domain.Core.SolicitudConvocatoria;
using GA2.Domain.Core.ValidacionTelAfiliado;
using GA2.Infraestructure;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.DatosAliadoEstrategico;
using GA2.Infraestructure.Repositories.Escrituras;
using GA2.Infraestructure.Repositories.EstadoConvocatoria;
using GA2.Infraestructure.Repositories.FichaAnalista;
using GA2.Infraestructure.Repositories.InfoVendedor;
using GA2.Infraestructure.Repositories.PAXC;
using GA2.Infraestructure.Repositories.PuntosAtencion;
using GA2.Infraestructure.Repositories.ValidacionTelAfiliado;
using GA2.Transversals.Mappers;
using GA2.Transversals.Mappers.GA2;
using GA2.Transversals.Mappers.GA2.Credito.Seven;
using GA2.Transversals.Mappers.GA2.EstadoConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using WkHtmlToPdfDotNet.Contracts;
using WkHtmlToPdfDotNet;
using GA2.Domain.Core.ValoresCategoria;
using GA2.Infraestructure.Repositories.ValoresCategoria;
using GA2.Domain.Core.Reintegros.GestorDocumental;
using GA2.Domain.Entities;
using GA2.Transversals.Commons.BackgroundTask;
using GA2.Apis.GA2.BackgroundServices;
using GA2.Infraestructure.Repositories.Proyectos;
using GA2.Domain.Core.Proyecto;
using GA2.Transversals.Mappers.GA2.Proyecto;
using GA2.Infraestructure.Repositories.LocalBlogStorage;
using GA2.Infraestructure.Repositories.ServiciosDinamicosRep;
using Hangfire;
using Hangfire.MemoryStorage;
using HangFire;
using ServiceVigiaVerificacionTercero;
using GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero;
using GA2.Domain.Core.Connected_Services.ServiceReference1;
using GA2.Domain.Core.Connected_Services.ServiceSeven;
using GA2.Infraestructure.Repositories.EmailNotificacion;
using GA2.Domain.Core.ReferenciasSvc.Storage;
using GA2.Domain.Core.ReferenciasSvc.Storage.Files;

namespace GA2.Apis.GA2.Extensions
{
    /// <summary>
    /// Manejador de inyeccion de dependencias
    /// </summary>
    /// <author>Oscar Julian Rojas Garces</author>
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
            services.AddAutoMapper(typeof(SerializedObjectMapper));
            services.AddAutoMapper(typeof(MapperNotification));
            services.AddAutoMapper(typeof(CatalogsMapper));
            services.AddAutoMapper(typeof(MapperParametrization));
            services.AddAutoMapper(typeof(MapperLock));
            services.AddAutoMapper(typeof(MapperMessage));
            services.AddAutoMapper(typeof(CarteraMapper));
            services.AddAutoMapper(typeof(ReportesMapper));
            services.AddAutoMapper(typeof(SevenMappers));
            services.AddAutoMapper(typeof(EstadoConvocatoriaMapper));
            services.AddAutoMapper(typeof(ConvocatoriaMapper));
            services.AddAutoMapper(typeof(TipoSolicitudPostulacionMapper));
            services.AddAutoMapper(typeof(PostulacioMapper));
            services.AddAutoMapper(typeof(CargaArchivosMapper));
            services.AddAutoMapper(typeof(ConstructoraMapper)); 
            services.AddAutoMapper(typeof(ProyectoConstructoraMapper));
            #endregion

            #region Main 
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddTransient<ITokenClaims, TokenClaims>();
            services.AddTransient<ICryptoGA2, CryptoGA2>();
            services.AddTransient<IProviderRequestG2, ProviderRequestG2>();
            services.AddTransient<IGestorTransUnionBusinessLogic, GestorTransUnionBusinessLogic>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IGetUserProvider, GetUserProvider>();

            //
            services.AddTransient<IIntegracionesGA2Bua, IntegracionesGA2>();
            services.AddTransient<IIntegracionesGA2Vigia, IntegracionesGA2>();
            services.AddTransient<IIntegracionesGA2WorkManager, IntegracionesGA2>();
            services.AddTransient<IIntegracionesGA2Fenix, IntegracionesGA2>();

            #endregion

            #region SignalR
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            #endregion

            #region Domain
            services.AddTransient<IBuaService, BuaService>();
            services.AddTransient<IDatosAliadoEstrategicoBusinessLogic, DatosAliadoEstrategicoBusinessLogic>();
            services.AddTransient<IValidacionTelAfiliadoBusinessLogic, ValidacionTelAfiliadoBusinessLogic>();
            services.AddTransient<ILogErrorBusinessLogic, LogErrorBusinessLogic>();
            services.AddTransient<ITokenClaims, TokenClaims>();
            services.AddTransient<IUsersBusinessLogic, UsersBusinessLogic>();
            services.AddTransient<IEscrituraBusinessLogic, EscrituraBusinessLogic>();
            services.AddTransient<IInformacionVendedorBusinessLogic, InformacionVendedorBusinessLogic>();
            services.AddTransient<IFichaAnalistaBusinessLogic, FichaAnalistaBusinessLogic>();
            services.AddTransient<IPAXCBusinessLogic, PAXCBusinessLogic>();
            services.AddTransient<IPuntoAtencionBusinessLogic, PuntoAtencionBusinessLogic>();
            services.AddTransient<IClientRequestProvider, ClientRequestProvider>();
            services.AddTransient<ICreditoBusinessLogic, CreditoBusinessLogic>();
            services.AddTransient<ICatalogosBusinessLogic, CatalogosBusinessLogic>();
            services.AddTransient<IParametrizacionBusinessLogic, ParametrizacionBusinessLogic>();
            services.AddTransient<IEstadoBusinessLogic, EstadoBusinessLogic>();
            services.AddTransient<IMovimientoBusinessLogic, MovimientoBusinessLogic>();
            services.AddTransient<IErrorCargueArchivoBusinessLogic, ErrorCargueArchivoBusinessLogic>();
            services.AddTransient<IProductoBusinessLogic, ProductoBusinessLogic>();
            services.AddTransient<IBloqueoBusinessLogic, BloqueoBusinessLogic>();
            services.AddTransient<INotificacionBusinessLogic, NotificacionBusinessLogic>();
            services.AddTransient<IValidacionIdentidadBusinessLogic, ValidacionIdentidadBusinessLogic>();
            services.AddTransient<IGestorTransUnionBusinessLogic, GestorTransUnionBusinessLogic>();
            services.AddTransient<IGestorIdVisionBusinessLogic, GestorIdVisionBusinessLogic>();
            services.AddTransient<IGestorDocumentalBusinessLogic, GestorDocumentalBusinessLogic>();
            services.AddTransient<IEjemploDocumentos, EjemploDocumentos>();
            services.AddTransient<ICargaArchivoBusinessLogic, CargaArchivoBusinessLogic>();
            services.AddTransient<IDescomposicionArchivo, DescomposicionArchivo>();
            services.AddTransient<IDocumentoBusinessLogic, DocumentoBusinessLogic>();
            services.AddTransient<ICuentaBusinessLogic, CuentaBusinessLogic>();
            services.AddTransient<IVigiaBusinessLogic, VigiaBusinessLogic>();
            services.AddTransient<ICarteraBusinessLogic, CarteraBusinessLogic>();
            services.AddTransient<IFenixBusinessLogic, FenixBusinessLogic>();
            services.AddTransient<IHaberesBusinessLogic, HaberesBusinessLogic>();
            services.AddTransient<ICesantiasBusinessLogic, CesantiasBusinessLogic>();
            services.AddTransient<ISolicitudBusinessLogic, SolicitudBusinessLogic>();
            services.AddTransient<IProcesoBusinessLogic, ProcesoBusinessLogic>();
            services.AddTransient<ICronogramaNovedadesBusinessLogic, CronogramaNovedadesBusinessLogic>();
            services.AddTransient<IMatriculasInmobiliariasLogic, MatriculasInmobilariasLogic>();
            services.AddTransient<IMatriculasInmobiliariasLogic, MatriculasInmobilariasLogic>();
            services.AddTransient<IEntidadesBusinessLogic,EntidadesBusinessLogic> ();
            services.AddTransient<IEmbargosBusinessLogic, EmbargosBusinessLogic>();
            services.AddTransient<IAuditoriaBusinessLogic, AuditoriaBusinessLogic>();
            services.AddTransient<IAuditoriaServicesBusinessLogic, AuditoriaServicesBusinessLogic>();
            services.AddTransient<ICuentasClientesBusinessLogic, CuentasClientesBusinessLogic>();            
            services.AddTransient<IProspeccionBusinessLogic, ProspeccionBusinessLogic>();
            services.AddTransient<ICierreMensualCuentasBusinessLogic, CierreMensualCuentasBusinessLogic>();
            services.AddTransient<ICierreMensualRequestProvider, CierreMensualRequestProvider>();
            services.AddTransient<ICierreAnualCuentasBusinessLogic, CierreAnualCuentasBusinessLogic>();
            services.AddTransient<ICierreAnualCuentasRequestProvider, CierreAnualCuentasRequestProvider>();
            services.AddTransient<IGestorDocumentSolicitudes, GestorDocumentSolicitudes>();
            services.AddTransient<IReportesBusinessLogic, ReportesBusinessLogic>();
            services.AddTransient<IReportesExcelLogic, ReportesExcelLogic>();
            services.AddTransient<IReportesExcelInvariantCultureLogic, ReportesExcelInvariantCultureLogic>();
            services.AddTransient<ITemplateExcelReporteRCredito, TemplateExcelReporteRCredito>();
            services.AddTransient<IReportesRequestProvider, ReportesRequestProvider>();
            services.AddTransient<IIntegracionesGa2BuaBusinessLogic, IntegracionesGA2BuaBusinessLogic>();
            services.AddTransient<IIntegracionesGA2WorkManagerBusinessLogic, IntegracionesGA2WorkManagerBusinessLogic>();
            services.AddTransient<IIntegracionesGA2VgiaBusinessLogic, IntegracionesGA2VigiaBusinessLogic>();
            services.AddTransient<IIntegracionesGA2FenixBusinessLogic, IntegracionesGA2FenixBusinessLogic>();
            services.AddTransient<IIntegracionesGA2Haberes, IntegracionesGA2>();
            services.AddTransient<IIntegracionesGA2HaberesBusinessLogic, IntegracionesGA2HaberesBusinessLogic>();
            services.AddTransient<IReintegrosBusinessLogic, ReintegrosBusinessLogic>();
            services.AddTransient<ITemplateExcel,TemplateExcel>();
            services.AddTransient<IHipotecaBusinessLogic, HipotecaBusinessLogic>();
            services.AddTransient<IEstadoConvocatoriaBussinesLogic, EstadoConvocatoriaBussineLogic>();
            services.AddTransient<IConvocatoriaBussigneLogic, ConvocatoriaBussineLogic>();
            services.AddTransient<IPostulacionBussineLogic, PostulacionBussineLogic>();
            services.AddTransient<IVerificacionDerechosBusinessLogic, VerificacionDerechosBusinessLogic>();
            services.AddTransient<IAuditoriaNovedadesBussinessLogic, AuditoriaNovedadesBussinessLogic>();
            services.AddTransient<IGestorDocumentalBusinessLogicFs, GestorDocumentalBusinessLogicFs>();
            services.AddTransient<IReporteSimulacionBusinessLogic, ReporteSimulacionBusinessLogic>();
            services.AddTransient<IGestorDocumentarEmbargosBL, GestorDocumentarEmbargosBL>();
            services.AddTransient<IValoresCategoriaBusinessLogic, ValoresCategoriaBusinessLogic>();
            services.AddTransient<IGestorDocumentalReintegrosBL, GestorDocumentalReintegrosBL>();
            services.AddTransient<IClientRequestProvider, ClientRequestProvider>();
            services.AddTransient<IConstructoraBussines, ConstructoraBussines>();
            services.AddTransient<IProyectoBusiness, ProyectoBusiness>();
            services.AddTransient<IVerificacionTerceroVigia, VerificacionTerceroVigiaBusinessLogic>();
            services.AddTransient<IBuaService2BusinessLogic, BuaService2BusinessLogic>();

            #endregion

            #region Inrfraestructure Repository

            services.AddTransient<IDatosAliadoEstrategicoRepository, DatosAliadoEstrategicoRepository>();
            services.AddTransient<IValidacionTelAfiliadoRepository, ValidacionTelAfiliadoRepository>();
            services.AddTransient<ILogErrorRepository, LogErrorRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IBlobStorageGenericRepository, BlobStorageGenericRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<ICreditoRepository, CreditoRepository>();
            services.AddTransient<IEscriturasRepository, EscrituraRepository>();
            services.AddTransient<IInformacionVendedorRepository, InformacionVendedorRepository>();
            services.AddTransient<IFichaAnalistaRepository, FichaAnalistaRepository>();
            services.AddTransient<IPAXCRepository, PAXCRepository>();
            services.AddTransient<IPuntoAtencionRepository, PuntoAtencionRepository>();
            services.AddTransient<ICatalogosRepository, CatalogosRepository>();
            services.AddTransient<IParametrizacionRepository, ParametrizacionRepository>();
            services.AddTransient<IEstadoRepository, EstadoRepository>();
            services.AddTransient<IMovimientoRepository, MovimientoRepository>();
            services.AddTransient<IProductoRepository, ProductoRepository>();
            services.AddTransient<IParamCargueNominaRepository, ParamCargueNominaRepository>();
            services.AddTransient<IBloqueoRepository, BloqueoRepository>();
            services.AddTransient<IMensajeRepository, MensajeRepository>();
            services.AddTransient<INotificacionRepository, NotificacionRepository>();
            services.AddTransient<IValidacionIdentidadRepository, ValidacionIdentidadRepository>();
            services.AddTransient<ITransUnionRepository, TransUnionRepository>();
            services.AddTransient<IDocumentoCargaRepository, DocumentoCargaRepository>();
            services.AddTransient<IErrorCargueArchivoRepository, ErrorCargueArchivoRepository>();
            services.AddTransient<ICuentaRepository, CuentaRepository>();
            services.AddTransient<ICarteraRepository, CarteraRepository>();
            services.AddTransient<ITareaRepository, TareaRepository>();
            services.AddTransient<ICesantiasRepository, CesantiasRepository>();
            services.AddTransient<ISolicitudRepository, SolicitudRepository>();
            services.AddTransient<IProcesoRepository, ProcesoRepository>();
            services.AddTransient<ICronogramaRepository, CronogramaRepository>();
            services.AddTransient<IMatriculasInmobiliariasRepository, MatriculasInmobiliariasRepository>();
            services.AddTransient<IEntidadesRepository, EntidadesRepository>();
            services.AddTransient<IEmbargosRepository, EmbargosRepository>();
            services.AddTransient<IAuditoriaRepository, AuditoriaRepository>();
            services.AddTransient<ICuentasClientesRepository, CuentasClientesRepository>();
            services.AddTransient<ICierreMensualRepository, CierreMensualRepository>();
            services.AddTransient<ICierreAnualCuentasRepository, CierreAnualCuentasRepository>();
            services.AddTransient<IReportesRepository, ReportesRepository>();
            services.AddTransient<IReintegrosRepository, ReintegrosRepository>();
            services.AddTransient<IEstadoConvocatoriaRepository, EstadoConvocatoriaRepository>();
            services.AddTransient<IConvocatoriaRepository, ConvocatoriaRepository>();
            services.AddTransient<IPostulacionRepository, PostulacionRepository>();
            services.AddTransient<IVerificacionDerechosRepository, VerificacionDerechosRepository>();
            services.AddTransient<IAuditoriaNovedadesRepository, AuditoriaNovedadesRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IValoresCategoriaRepository, ValoresCategoriaRepository>();
            services.AddTransient<IConstructoraRepository, ConstructoraRepository>();
            services.AddTransient<IProyectoRepository, ProyectoRepository>();
            services.AddTransient<IEmailNotificacionRepository, EmailNotificacionRepository>();

            #endregion Inrfraestructure Repository

            #region AutoMapper

            //services.AddAutoMapper(typeof(AutenticacionProfile));
            //services.AddAutoMapper(typeof(OtpMappers));

            #endregion

            #region Core
            services.AddScoped<LocalStorageProvider>();
            services.AddScoped<AzureStorageProvider>();
            services.AddScoped<ApiStorageProvider>();

            services.AddScoped<IStorageFactory, StorageFactory>();


            // services.AddTransient<IIntifierRequestService, IntifierRequestService>();

            // BUA
            services.AddTransient<IBuaService, BuaService>();
            services.AddTransient<IConsumoBua, ConsumoBua>();
            services.AddTransient<IWorkManagerService, WorkManagerService>();
            services.AddTransient<IConsumoWorkManager, ConsumoWorkManager>();

            // Haberes
            services.AddTransient<IHaberesService, HaberesService>();
            services.AddTransient<IConsumoHaberes, ConsumoHaberes>();

            ///////  Seven
            services.AddTransient<IConsumoSCnMcont, ConsumoSCnMcont>();
            services.AddTransient<IConsumoWSAportes, ConsumoWSAportes>();
            services.AddTransient<IConsumoCDP, ConsumoCDP>();
            services.AddTransient<IConsumoWSFactura, ConsumoWSFactura>();
            services.AddTransient<IConsumoWSProveedor, ConsumoWSProveedor>();
            services.AddTransient<IConsumoWSTercero, ConsumoWSTercero>();
            services.AddTransient<IConsumoWSWintipro, ConsumoWSWintipro>();            
            services.AddTransient<ISevenBusinessLogic, SevenBusinessLogic>();
            services.AddTransient<IEmbargosProvider, EmbargoProvider>();


            // Opciones Facturacion
            // Otros Conceptos

            // Vigia
            services.AddTransient<ICargaProductoTerceroService, CargaProductoTerceroService>();
            services.AddTransient<IConsumoVigiaCargaProductoTercero, ConsumoVigiaCargaProductoTercero>();

            services.AddTransient<ICargaTerceroService, CargaTerceroService>();
            services.AddTransient<IConsumoVigiaCargaTercero, ConsumoVigiaCargaTercero>();

            services.AddTransient<IMovimientoContableService, MovimientoContableService>();

            services.AddTransient<ICargaTransaccionService, CargaTransaccionService>();
            services.AddTransient<IConsumoVigiaCargaTransaccion, ConsumoVigiaCargaTransaccion>();

            services.AddTransient<IVerificacionTerceroService, VerificacionTerceroService>();
            services.AddTransient<IConsumoVigiaVerificacionTercero, ConsumoVigiaVerificacionTercero>();

            //Fenix
            services.AddTransient<IFenixService, FenixService>();
            services.AddTransient<IConsumoFenix, ConsumoFenix>();

            //OTP
            services.AddTransient<IConsumoServiceOtp, ConsumoServiceOtp>();
            // services.AddTransient<IOtpService, OtpService>();

            services.AddHostedService<GeneracionReporteBackgroundService>();
            services.AddSingleton<IBackgroundReportQueue, DefaultBackgroundReportQueue>();


            //LocalBlogStore
            services.AddTransient<ILocalBlogStorageRepository, LocalBlogStorageRepository>();
            services.AddTransient<IServiciosDinamicosRepository, ServiciosDinamicosRepository>();

            ///HangFire
            services.AddTransient<AcreditacionHangFireService>();
            services.AddSingleton<HangfireJobsInitializer>();

            #endregion

            return services;
        }
    }
}
