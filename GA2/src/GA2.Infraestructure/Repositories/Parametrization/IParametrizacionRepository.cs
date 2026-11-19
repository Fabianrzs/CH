using GA2.Application.Dto.Parametrization;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Parametrization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// lRepository de parametrizacion
    /// <author>Camilo Andres Yaya Poveda</author>
    /// <date>20/04/202</date>
    /// </summary>
    public interface IParametrizacionRepository
    {
        /// <summary>
        /// Metodo Obtener seguro
        /// </summary>
        /// <author>Edgar Andrés Riaño Suarez</author>
        /// <date>2/03/202</date>
        /// <returns>Lista de seguros</returns>
        Task<SeguroParametrizacion> ObtenerSeguro();
        /// <summary>
        /// Metodo Crear y Actualizar seguro mediante el Dto
        /// </summary>
        /// <author>Edgar Andrés Riaño Suarez</author>
        /// <date>2/03/202</date>
        /// <returns>Modelo Crear Seguro</returns>
        Task<SeguroParametrizacion> CrearSeguro(SeguroParametrizacion insuraceparametrizacion);
        /// <summary>
        /// Metodo Obtener Tasa
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de tasas</returns>
        Task<TasaParametrizacion> ObtenerTasa();
        /// <summary>
        /// Metodo Crear y Actualizar Tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Tasa</returns>
        Task<TasaParametrizacion> CrearTasa(TasaParametrizacion rateparametrizacion);
        /// <summary>
        /// Metodo Obtener Plazo
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de plazos</returns>
        Task<PlazoParametrizacion> ObtenerPlazo();
        /// <summary>
        /// Metodo Crear y Actualizar Plazo mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Plazos</returns>
        Task<PlazoParametrizacion> CrearPlazo(PlazoParametrizacion deadlineparametrizacion);
        /// <summary>
        /// Metodo Obtener Legal Tasa
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de Legal Tasa</returns>
        Task<IEnumerable<LegalTasaParametrizacion>> ObtenerLegalTasa();
        /// <summary>
        /// Metodo Crear Legal Tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Legal Tasa</returns>
        Task<LegalTasaParametrizacion> CrearLegalTasa(LegalTasaParametrizacion legaltasaparametrizacion);
        /// <summary>
        /// Metodo Actualizar Legal Tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Actualizar Legal Tasa</returns>
        Task<LegalTasaParametrizacion> ActualizarLegalTasa(LegalTasaParametrizacion legalTasaparametrizacion);
        /// <summary>
        /// Metodo Eliminar Legal Tasa mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Eliminar Legal Tasa</returns>
        Task<LegalTasaParametrizacion> EliminarLegalTasa(LegalTasaParametrizacion legalTasaparametrizacion);
        /// <summary>
        /// Metodo Obtener Legal Alivio
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de Legal Alivio</returns>
        Task<LegalAlivioParametrizacion> ObtenerLegalAlivio();
        /// <summary>
        /// Metodo Crear y Actualizar Legal Alivio mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Legal Alivio</returns>
        Task<LegalAlivioParametrizacion> CrearLegalAlivio(LegalAlivioParametrizacion legalalivioparametrizacion);
        /// <summary>
        /// Metodo Obtener Legal GMF
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de Legal GMF</returns>
        Task<LegalGmfParametrizacion> ObtenerLegalGmf();
        /// <summary>
        /// Metodo Crear y Actualizar Legal GMF mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Legal GMF</returns>
        Task<LegalGmfParametrizacion> CrearLegalGmf(LegalGmfParametrizacion legalgmfparametrizacion);
        /// <summary>
        /// Metodo Obtener Parametrizacion de Liquidacion
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Lista de Liquidacion</returns>
        Task<LiquidacionParametrizacion> ObtenerLiquidacion();
        /// <summary>
        /// Metodo Crear y Actualizar Parametrizacion de Liquidacion mediante el Dto
        /// </summary>
        /// <author>Camilo Andres Yaya Poveda</author>
        /// <date>20/04/202</date>
        /// <returns>Modelo Crear Liquidacion</returns>
        Task<LiquidacionParametrizacion> CrearLiquidacion(LiquidacionParametrizacion settlementparametrizacion);
        Task<IEnumerable<ParametroTransaccion>> ObtenerParametrosTransaccion();
        Task<ParametroTransaccion> ActualizarParametroTransaccion(ParametroTransaccion parametrotransaccion);
        Task<ParametroTransaccion> CrearParametroTransaccion(ParametroTransaccion parametrotransaccion);

        /// <summary>
        /// Metodo para obtener los archivos parametrizados por modalidad 
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <date>14/10/2021</date>
        /// <returns>Lista de archivos parametrizados</returns>
        Task<IEnumerable<ParametrizacionArchivoModalidad>> ObtenerParametrizacionArchivosModalidad(int tipoModalidadId);

        /// <summary>
        /// Metodo para obtener los nombres de tablas parametros por categoria
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>18/07/2023</date>
        /// <returns>Lista de Nombres de tablas parametros</returns>
        Task<IEnumerable<TablaParametro>> ObtenerTablasParametrosPorCategoria(int categoriaId);
        Task<IEnumerable<ValorInmuebleResponseDto>> SP_CONSULTAR_VALOR_INMUEBLE(ValorInmuebleDto request);

        /// <summary>
        /// Metodo para agregar registros tablas parametros
        /// </summary>
        /// <author>Michel Parra</author>
        /// <date>18/08/2023</date>
        /// <returns>Agregar registro tablas parametros</returns>
        Task<dynamic> AgregarTablasParametros(dynamic data);


        /// <summary>
        /// Metodo para obtener los archivos parametrizados por entidad
        /// </summary>
        /// <author> Edwin Lopez </author>
        /// <date>29/03/2022</date>
        /// <param name="entidadId"></param>
        /// <returns></returns>
        Task<IEnumerable<ParametrizacionArchivoEntidadEducativa>> ObtenerParametrizacionArchivosEntidadEducativa(string entidadId);

        /// <summary>
        /// Metodo para crear un IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>04/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>

        Task<IPC> CrearIPC(IPC IPCparametrizacion);

        /// <summary>
        /// Metodo para actualizar un IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>05/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        Task<IPC> ActualizarIPC(IPC IPCparametrizacion);

        /// <summary>
        /// Metodo para Obtener filtrado de IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>08/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        Task<IEnumerable<IPC>> ObtenerReporteIPC(IPC IPCparametrizacion);

        /// <summary>
        /// Metodo para saber su existe un IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>07/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        Task<IEnumerable<IPC>> ExisteIPC(IPC IPCparametrizacion);

        /// <summary>
        /// Metodo para obtener todos los IPC
        /// </summary>
        /// <author> Julian Nova </author>
        /// <date>02/05/2023</date>
        /// <param name="IPCparametrizacion"></param>
        /// <returns></returns>
        Task<IEnumerable<IPC>> ObtenerTodoReporteIPC();

        Task<int> EliminacionParametrizacionCuentas();



    }
}
