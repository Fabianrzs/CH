using Dapper;
using GA2.Application.Main.ProviderRequest;
using GA2.Domain.Entities;
using GA2.Infraestructure.Data;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GA2.Domain.Entities.BUA;
using static GA2.Application.Dto.Credito.TU.Responses.ResponseGeneralTu;
using GA2.Application.Dto.BUA;
using GA2.Infraestructure.Data.GrupoUsuarios;
using System.Security.Cryptography;
using Azure.Core;
using GA2.Application.Dto;
using ConceptosSaldosBloqueos = GA2.Domain.Entities.ConceptosSaldosBloqueos;

namespace GA2.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio para los metodos de informacion de clientes
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
    /// <date>12/04/2021</date>
    public class ClienteRepository : DapperGenericRepository, IClienteRepository
    {
        /// <summary>
        /// Constructor del repositorio
        /// </summary>
        /// <author>Nicolas Florez Sarrazola</author>
        /// <date>12/04/2021</date>
        /// <param name="configuration"></param>
        private readonly IEmbargosProvider _ga2Provider;
        public ClienteRepository(IConfiguration configuration, IEmbargosProvider ga2Provider) : base(configuration)
        {
            _ga2Provider = ga2Provider;
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


            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }


        /// <summary>
        /// Metodo obtener info cliente por tipo y número de documento 
        /// </summary>
        /// <param name="identificationNumber">Identificacion del cliente</param>
        /// <param name="identificationType">Tipo identificacion</param>
        /// <uthor>Erwin Pantoja España</uthor>
        /// <date>4/10/2021</date>
        /// <returns>Modelo cliente</returns>
        public async Task<ClienteCesantias> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCLiente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), identificationType);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), identificationNumber);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), idCLiente);

            return await GetAsyncFirst<ClienteCesantias>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Metodo Obtener cliente mediante Id
        /// </summary>
        /// <author>Nicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Cliente> ObtenerClienteInformacion(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), Id);

            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        /// <summary>
        /// Metodo Obtener cliente filtrado por cedula
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <param name="identificacionCliente">Cedula del cliente</param>
        /// <date>08/10/2021</date>
        /// <returns></returns>
        public async Task<ClienteCedula> ObtenerInformacionClienteCedula(int identificacionCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), identificacionCliente);
            ClienteCedula clienteCedula = new ClienteCedula();

            clienteCedula = await GetAsyncFirst<ClienteCedula>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);


            return clienteCedula;
        }

        /// <summary>
        /// Metodo Actualizar datos del cliente
        /// </summary>
        /// <author>ONicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Cliente> ActualizarCliente(Cliente client,ClienteDto datosAnteriores)
        {
            DynamicParameters parameters = new DynamicParameters();
            DateTime? fechaAltaCalculada = client.CTS_SLP;
            if (client.CTS_SLP.Year < 1800) { fechaAltaCalculada = null; }

            //Información básica cliente
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), client.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), client.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPA_ID), client.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), client.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_EXPEDICION), client.CLI_FECHA_EXPEDICION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_IDENTIFICACION_EXPEDIDA), client.DPC_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_IDENTIFICACION_EXPEDIDA), client.DPD_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_LUGAR_EXPEDICION), client.CLI_LUGAR_EXPEDICION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_NOMBRE), client.CLI_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_NOMBRE), client.CLI_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_APELLIDO), client.CLI_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_APELLIDO), client.CLI_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_NACIMIENTO), client.CLI_FECHA_NACIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_PAIS_NACIMIENTO), client.DPP_ID_PAIS_NACIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID), client.DPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID), client.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_LUGAR_NACIMIENTO), client.CLI_LUGAR_NACIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_SEXO), client.CAT_SEXO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_ESTADO_CIVIL), client.CAT_ESTADO_CIVIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PROFESION), client.CLI_PROFESION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), client.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), client.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTG_ID), client.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.GRD_ID), client.GRD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_INSCRIPCION), client.CLI_FECHA_INSCRIPCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_ALTA), client.CLI_FECHA_ALTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTS_SLP), fechaAltaCalculada);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_VALIDACION_BIOMETRICA), client.CLI_VALIDACION_BIOMETRICA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_REGIMEN), client.CLI_REGIMEN); 
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_CODIGO_MILITAR), client.CLI_CODIGO_MILITAR);
            //Información económica
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ACC_ID), client.ACC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_MENSUAL), client.CIE_INGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_EGRESO_MENSUAL), client.CIE_EGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_ACTIVOS), client.CIE_TOTAL_ACTIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PASIVOS), client.CIE_TOTAL_PASIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PATRIMONIO), client.CIE_TOTAL_PATRIMONIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_ADICIONAL), client.CIE_INGRESO_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_CONCEPTO_INGRESO_ADICIONAL), client.CIE_CONCEPTO_INGRESO_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TRANS_EXTRANJERO), client.CIE_TRANS_EXTRANJERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.MND_ID), client.MND_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_VALOR_TRANSFERENCIA), client.CIE_VALOR_TRANSFERENCIA);

            //Información de direccion
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_ID), client.DRC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_RESIDENCIA), client.DPP_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_RESIDENCIA), client.DPD_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_RESIDENCIA), client.DPC_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_CIUDAD_RESIDENCIA_EXTRANJERO), client.DRC_CIUDAD_RESIDENCIA_EXTRANJERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPC_TIPO_CALLE), client.TPC_TIPO_CALLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_NUMERO_1), client.DRC_NUMERO_1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.LTR_LETRA_1), client.LTR_LETRA_1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.BS_BIS_1), client.BS_BIS_1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CRD_CARDINAL_1), client.CRD_CARDINAL_1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_NUMERO_2), client.DRC_NUMERO_2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.LTR_LETRA_2), client.LTR_LETRA_2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.BS_BIS_2), client.BS_BIS_2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CRD_CARDINAL_2), client.CRD_CARDINAL_2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_NUMERO_3), client.DRC_NUMERO_3);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.LTR_LETRA_3), client.LTR_LETRA_3);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CRD_CARDINAL_3), client.CRD_CARDINAL_3);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_COMPLEMENTO_DIRECCION), client.DRC_COMPLEMENTO_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_DIRECCION), client.DRC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_BARRIO), client.DRC_BARRIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_TIPO_DIR), client.TPD_ID);

            //Información de correo
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_CORREO), client.DCE_CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_CORREO2), client.DCE_CORREO2);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TCE_ID), client.TCE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_ENVIO_INFORMACION), client.DCE_ENVIO_INFORMACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_ACTIVO), client.DCE_ACTIVO);

            //Información de direccion
            if (!string.IsNullOrEmpty(client.DTL_TELEFONO))
            {
                string telefono = client.DTL_TELEFONO.Replace("(", "").Replace(")", "").Replace("-", "");
                telefono = Regex.Replace(telefono, @"\s", "");
                parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), telefono);
            }
            else
            {
                parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), string.Empty);
            }
            if (!string.IsNullOrEmpty(client.DTL_TELEFONO2))
            {
                string telefono2 = client.DTL_TELEFONO2.Replace("(", "").Replace(")", "").Replace("-", "");
                telefono2 = Regex.Replace(telefono2, @"\s", "");
                parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO2), telefono2);
            }
            else
            {
                parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO2), string.Empty);
            }
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPT_ID), client.TPT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_ENVIO_INFORMACION), client.DTL_ENVIO_INFORMACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_ACTIVO), client.DTL_ACTIVO);

            //Información cónyuge
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_TID_ID), client.CNY_TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_IDENTIFICACION), client.CNY_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_NOMBRE), client.CNY_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_NOMBRE), client.CNY_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_APELLIDO), client.CNY_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_APELLIDO), client.CNY_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_ACTIVO), client.CNY_ACTIVO);

            //Tipo de actualizacio 1:datos personales, 2:datos financieros y 3 datos de contacto
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ACTUALIZA), client.ACTUALIZA);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FECHA_ACTUALIZA), DateTime.Now);

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.USUARIO), client.USUARIO);

            try
            {
                var paises = await _ga2Provider.ObtenerPaisesGA2();
                var departamentos = await _ga2Provider.ObtenerDepartamentosGA2();
                var ciudades = await _ga2Provider.ObtenerCiudadesGA2();
                var unidadesEjecutoras = await _ga2Provider.ObtenerUnidadesEjecutorasGA2();
                var grados = await _ga2Provider.ObtenerGradosGA2();
                var fuerzas = await _ga2Provider.ObtenerFuerzasGA2();
                var estadosCiviles = await _ga2Provider.ObtenerEstadosCivilesGA2();
                var ocupaciones = await _ga2Provider.ObtenerOcupacionesGA2();
                var categorias = await _ga2Provider.ObtenerCategoriasGA2();
                var actividadesEconomicas = await _ga2Provider.ObtenerActividadesEconomicasGA2();
                var monedas = await _ga2Provider.ObtenerMonedasGA2();
                var tiposDireccion = await _ga2Provider.ObtenerTiposDireccionGA2();
                var tiposTelefono = await _ga2Provider.ObtenerTiposTelefonoGA2();
                var tiposCorreo = await _ga2Provider.ObtenerTiposCorreoGA2();

                var paisNacimiento = paises.Data.FirstOrDefault(x => x.Codigo == client.DPP_ID_PAIS_NACIMIENTO);
                var paisNacimientoAnterior = paises.Data.FirstOrDefault(x => x.Codigo == datosAnteriores.IdPaisNacimiento);
                parameters.Add("@PaisNacimiento", paisNacimiento==null?"":paisNacimiento.Nombre);
                parameters.Add("@PaisNacimientoAnterior", paisNacimientoAnterior == null ? "" : paisNacimientoAnterior.Nombre );

                var paisResidencia = paises.Data.FirstOrDefault(x => x.Codigo == client.DPP_ID_RESIDENCIA);
                var paisResidenciaAnterior = paises.Data.FirstOrDefault(x => x.Codigo == datosAnteriores.IdPaisResidencia);
                parameters.Add("@PaisResidencia", paisResidencia == null ? "": paisResidencia.Nombre);
                parameters.Add("@PaisResidenciaAnterior", paisResidenciaAnterior == null ? "" : paisResidenciaAnterior.Nombre);

                var departamentoNacimiento = departamentos.Data.FirstOrDefault(x => x.Id == client.DPD_ID);
                var departamentoNacimientoAnterior = departamentos.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdDepartamentoNacimiento);
                parameters.Add("@DepartamentoNacimiento", departamentoNacimiento == null ? "" : departamentoNacimiento.Nombre );
                parameters.Add("@DepartamentoNacimientoAnterior", departamentoNacimientoAnterior == null ? "" : departamentoNacimientoAnterior.Nombre);

                var departamentoResidencia = departamentos.Data.FirstOrDefault(x => x.Id == client.DPD_ID_RESIDENCIA);
                var departamentoResidenciaAnterior = departamentos.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdDepartamentoResidencia);
                parameters.Add("@DepartamentoResidencia", departamentoResidencia == null ? "": departamentoResidencia.Nombre);
                parameters.Add("@DepartamentoResidenciaAnterior", departamentoResidenciaAnterior == null ? "": departamentoResidenciaAnterior.Nombre);

                var departamentoExpedicion = departamentos.Data.FirstOrDefault(x => x.Id == client.DPD_ID_IDENTIFICACION_EXPEDIDA);
                var departamentoExpedicionAnterior = departamentos.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdDepartamentoExpedicionDocumento);
                parameters.Add("@DepartamentoExpedicion", departamentoExpedicion == null ? "": departamentoExpedicion.Nombre);
                parameters.Add("@DepartamentoExpedicionAnterior", departamentoExpedicionAnterior == null ? "" : departamentoExpedicionAnterior.Nombre);

                var ciudadNacimiento = ciudades.Data.FirstOrDefault(x => x.Id == client.DPC_ID);
                var ciudadNacimientoAnterior = ciudades.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdCiudadNacimiento);
                parameters.Add("@CiudadNacimiento", ciudadNacimiento == null ? "" : ciudadNacimiento.Nombre);
                parameters.Add("@CiudadNacimientoAnterior", ciudadNacimientoAnterior == null ? "": ciudadNacimientoAnterior.Nombre);

                var ciudadResidencia = ciudades.Data.FirstOrDefault(x => x.Codigo == client.DPC_ID_RESIDENCIA);
                var ciudadResidenciaAnterior = ciudades.Data.FirstOrDefault(x => x.Codigo == datosAnteriores.IdCiudadResidencia);
                parameters.Add("@CiudadResidencia", ciudadResidencia == null ? "": ciudadResidencia.Nombre);
                parameters.Add("@CiudadResidenciaAnterior", ciudadResidenciaAnterior == null ? "" : ciudadResidenciaAnterior.Nombre);

                var ciudadExpedicion = ciudades.Data.FirstOrDefault(x => x.Codigo == client.DPC_ID_IDENTIFICACION_EXPEDIDA);
                var ciudadExpedicionAnterior = ciudades.Data.FirstOrDefault(x => x.Codigo == datosAnteriores.IdCiudadExpedicionDocumento);
                parameters.Add("@CiudadExpedicion", ciudadExpedicion == null ? "" : ciudadExpedicion.Nombre);
                parameters.Add("@CiudadExpedicionAnterior", ciudadExpedicionAnterior == null ? "" : ciudadExpedicionAnterior.Nombre);

                var unidadEjecutora = unidadesEjecutoras.Data.FirstOrDefault(x => x.Id == client.UEJ_ID);
                var unidadEjecutoraAnterior = unidadesEjecutoras.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdUnidadEjecutora);
                parameters.Add("@UnidadEjecutora", unidadEjecutora == null ? "" : unidadEjecutora.Nombre);
                parameters.Add("@UnidadEjecutoraAnterior", unidadEjecutoraAnterior == null ? "" : unidadEjecutoraAnterior.Nombre);

                var grado = grados.Data.FirstOrDefault(x => x.Id == client.GRD_ID);
                var gradoAnterior = grados.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdGrado);
                parameters.Add("@Grado", grado == null ? "" : grado.Descripcion);
                parameters.Add("@GradoAnterior", gradoAnterior == null ? "" : gradoAnterior.Descripcion);

                var fuerza = fuerzas.Data.FirstOrDefault(x => x.Id == client.FRZ_ID);
                var fuerzaAnterior = fuerzas.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdFuerza);
                parameters.Add("@Fuerza", fuerza == null ? "": fuerza.Descripcion);
                parameters.Add("@FuerzaAnterior", fuerzaAnterior == null ? "": fuerzaAnterior.Descripcion);

                var estadoCivil = estadosCiviles.Data.FirstOrDefault(x => x.Id == client.CAT_ESTADO_CIVIL);
                var estadoCivilAnterior = estadosCiviles.Data.FirstOrDefault(x => x.Id == datosAnteriores.EstadoCivil);
                parameters.Add("@DescripcionEstadoCivil", estadoCivil == null ? "" : estadoCivil.Descripcion);
                parameters.Add("@EstadoCivilAnterior", estadoCivilAnterior == null ? "" : estadoCivilAnterior.Descripcion) ;

                var ocupacion = ocupaciones.Data.FirstOrDefault(x => x.Id == client.CLI_PROFESION);
                var ocupacionAnterior = ocupaciones.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdProfesion);
                parameters.Add("@Profesion", ocupacion == null ? "": ocupacion.Descripcion);
                parameters.Add("@ProfesionAnterior", ocupacionAnterior == null ? "": ocupacionAnterior.Descripcion);

                var categoria = categorias.Data.FirstOrDefault(x => x.Id == client.CTG_ID);
                var categoriaAnterior = categorias.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdCategoria);
                parameters.Add("@Categoria", categoria == null ? "": categoria.Descripcion);
                parameters.Add("@CategoriaAnterior", categoriaAnterior == null ? "": categoriaAnterior.Descripcion);

                var actividadEconomica = actividadesEconomicas.Data.FirstOrDefault(x => x.Id == client.ACC_ID);
                var actividadEconomicaAnterior = actividadesEconomicas.Data.FirstOrDefault(x => x.Id == datosAnteriores.IdActividadEconomica);
                parameters.Add("@ActividadEconomica", actividadEconomica == null ? "" : actividadEconomica.Descripcion);
                parameters.Add("@ActividadEconomicaAnterior", actividadEconomicaAnterior == null ? "" : actividadEconomicaAnterior.Descripcion);

                var moneda = monedas.Data.FirstOrDefault(x => x.IdMoneda == client.MND_ID);
                var monedaAnterior = monedas.Data.FirstOrDefault(x => x.IdMoneda == datosAnteriores.IdMoneda);
                parameters.Add("@Moneda", moneda == null ? "" : moneda.DescripcionMoneda);
                parameters.Add("@MonedaAnterior", monedaAnterior == null ? "" : monedaAnterior.DescripcionMoneda);

                var listAfiliaciones = await _ga2Provider.ObtenerTiposAfiliacionGA2();
                var tiposVinculacion = listAfiliaciones.Data.Where(x => x.AplicaClienteTipoActor).ToList();

                var tipoVinculacion = tiposVinculacion.FirstOrDefault(x => x.IdTipoActor == client.TPA_ID);
                var tipoVinculacionAnterior = tiposVinculacion.FirstOrDefault(x => x.IdTipoActor == datosAnteriores.IdTipoActor);
                parameters.Add("@TipoVinculacion", tipoVinculacion == null ? "": tipoVinculacion.DescripcionTipoActor);
                parameters.Add("@TipoVinculacionAnterior", tipoVinculacionAnterior == null ? "": tipoVinculacionAnterior.DescripcionTipoActor);

                var tipoDireccion = tiposDireccion.Data.FirstOrDefault(x => x.TipoDireccionId == client.TPD_ID);
                var tipoDireccionAnterior = tiposDireccion.Data.FirstOrDefault(x => x.TipoDireccionId == datosAnteriores.IdTipoDireccion);
                parameters.Add("@TipoDireccion", moneda == null ? "" : tipoDireccion.Descripcion);
                parameters.Add("@TipoDireccionAnterior", monedaAnterior == null ? "" : tipoDireccionAnterior.Descripcion);

                var tipoTelefono = tiposTelefono.Data.FirstOrDefault(x => x.IdTipoTelefono == client.TPT_ID);
                var tipoTelefonoAnterior = tiposTelefono.Data.FirstOrDefault(x => x.IdTipoTelefono == datosAnteriores.IdTipoTelefono);
                parameters.Add("@TipoTelefono", moneda == null ? "" : tipoTelefono.DescripcionTipoTelefono);
                parameters.Add("@TipoTelefonoAnterior", monedaAnterior == null ? "" : tipoTelefonoAnterior.DescripcionTipoTelefono);

                var tipoCorreo = tiposCorreo.Data.FirstOrDefault(x => x.IdTipoCorreo == client.TCE_ID);
                var tipoCorreoAnterior = tiposCorreo.Data.FirstOrDefault(x => x.IdTipoCorreo == datosAnteriores.IdTipoCorreo);
                parameters.Add("@TipoCorreo", moneda == null ? "" : tipoCorreo.DescripcionTipoCorreo);
                parameters.Add("@TipoCorreoAnterior", monedaAnterior == null ? "" : tipoCorreoAnterior.DescripcionTipoCorreo);
            }
            catch(Exception ex){ }
           


            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener cliente login paa
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="numeroIdentificacion"></param>
        /// <param name="numeroCelular"></param>
        /// <param name="fechaExpedicion"></param>
        /// <returns></returns>
        public async Task<Cliente> ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular(string tipoDocumento, string numeroIdentificacion, string numeroCelular, DateTime fechaExpedicion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), tipoDocumento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), numeroIdentificacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_EXPEDICION), fechaExpedicion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), numeroCelular);

            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        /// <summary>
        /// Metodo que valida los registros de afiliados que deben ser creados
        /// </summary>
        /// <param name="json">Json con los clientes a crear</param>
        /// <returns>Lista de clientes sin confirmar</returns>
        public async Task<IEnumerable<ClienteSinCrear>> ValidarNomina(string json)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(HelpersEnums.GetEnumDescription(EnumEjecutorParameters.Json), json);
            return await GetAsyncTransaction<ClienteSinCrear>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameter, CommandType.StoredProcedure, IsolationLevel.ReadUncommitted);
        }

        /// <summary>
        /// Metodo que crea los afiliados apartir del archivo de carga
        /// </summary>
        /// <param name="clientes">Lista de clientes a ser creados</param>
        /// <returns>Confirmacion de creacion de clientes</returns>
        public async Task<IEnumerable<ClienteSinCrear>> CrearClientesPorCargaNomina(IEnumerable<ClienteSinCrear> clientes)
        {
            DynamicParameters parameter = new DynamicParameters();
            parameter.Add(HelpersEnums.GetEnumDescription(EnumEjecutorParameters.Json), JsonConvert.SerializeObject(clientes));
            return await GetAsyncTransaction<ClienteSinCrear>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameter, CommandType.StoredProcedure, IsolationLevel.ReadCommitted);
        }

        public async Task<Cliente> InsertarCliente(Cliente client)
        {
            DynamicParameters parameters = new DynamicParameters();
            //Información básica cliente
            DateTime? fechaExpedicion = client.CLI_FECHA_EXPEDICION;
            DateTime? fechaAlta = client.CLI_FECHA_ALTA;
            DateTime? fechaInscripcion = client.CLI_FECHA_INSCRIPCION;
            DateTime? fechaAltaCalculada = client.CTS_SLP;
            DateTime? fechaNacimiento = client.CLI_FECHA_NACIMIENTO;
            int paisNacimiento = client.DPP_ID_PAIS_NACIMIENTO > 0 ? client.DPP_ID_PAIS_NACIMIENTO : 1;
            int paisResidencia = client.DPP_ID_RESIDENCIA > 0 ? client.DPP_ID_RESIDENCIA : 1;

            if (client.CLI_FECHA_EXPEDICION.Year < 1800) { fechaExpedicion = null; }
            if (client.CLI_FECHA_ALTA.Year < 1800) { fechaAlta = null; }
            if (client.CLI_FECHA_INSCRIPCION.Year < 1800) { fechaInscripcion = null; }
            if (client.CTS_SLP.Year < 1800) { fechaAltaCalculada = null; }
            if (client.CLI_FECHA_NACIMIENTO.Year < 1800) { fechaNacimiento = null; }


            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), client.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPA_ID), client.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), client.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_EXPEDICION), fechaExpedicion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_IDENTIFICACION_EXPEDIDA), client.DPP_ID_PAIS_EXPEDICION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_IDENTIFICACION_EXPEDIDA), client.DPD_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_IDENTIFICACION_EXPEDIDA), client.DPC_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_NOMBRE), client.CLI_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_NOMBRE), client.CLI_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_APELLIDO), client.CLI_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_APELLIDO), client.CLI_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_NACIMIENTO), fechaNacimiento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_PAIS_NACIMIENTO), paisNacimiento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID), client.DPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID), client.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_SEXO), client.CAT_SEXO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_ESTADO_CIVIL), client.CAT_ESTADO_CIVIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PROFESION), 1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_UNIDAD), client.CLI_UNIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), client.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.GRD_ID), client.GRD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTG_ID), client.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), client.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_INSCRIPCION), fechaInscripcion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_ALTA), fechaAlta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTS_SLP), fechaAltaCalculada);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_VALIDACION_BIOMETRICA), 1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_REGIMEN), client.CLI_REGIMEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_INTEGRACION_GA2_PAYLOAD), client.CLI_INTEGRACION_GA2_PAYLOAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ECL_ID), client.ECL_ID);

            //Información económica
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ACC_ID), client.ACC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_MENSUAL), client.CIE_INGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_EGRESO_MENSUAL), client.CIE_EGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_ACTIVOS), client.CIE_TOTAL_ACTIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PASIVOS), client.CIE_TOTAL_PASIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PATRIMONIO), client.CIE_TOTAL_PATRIMONIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_ADICIONAL), client.CIE_INGRESO_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_CONCEPTO_INGRESO_ADICIONAL), client.CIE_CONCEPTO_INGRESO_ADICIONAL);

            //Información de direccion
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_RESIDENCIA), paisResidencia);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_RESIDENCIA), client.DPD_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_RESIDENCIA), client.DPC_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_DIRECCION), client.DRC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_BARRIO), client.DRC_BARRIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_FECHA_MODIFICACION), client.DRC_FECHA_MODIFICACION);


            //Información de correo
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_CORREO), client.DCE_CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TCE_ID), client.TCE_ID);

            //Información de direccion
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), client.DTL_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPT_ID), client.TPT_ID);

            //Información cónyuge
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_TID_ID), client.CNY_TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_IDENTIFICACION), client.CNY_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_NOMBRE), client.CNY_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_NOMBRE), client.CNY_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_APELLIDO), client.CNY_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_APELLIDO), client.CNY_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_ACTIVO), 1);

            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Cliente> RestaurarIntegracionCliente(Cliente client)
        {
            DynamicParameters parameters = new DynamicParameters();
            //Información básica cliente
            DateTime? fechaExpedicion = client.CLI_FECHA_EXPEDICION;
            DateTime? fechaAlta = client.CLI_FECHA_ALTA;
            DateTime? fechaInscripcion = client.CLI_FECHA_INSCRIPCION;
            DateTime? fechaAltaCalculada = client.CTS_SLP;
            DateTime? fechaNacimiento = client.CLI_FECHA_NACIMIENTO;
            int paisNacimiento = client.DPP_ID_PAIS_NACIMIENTO > 0 ? client.DPP_ID_PAIS_NACIMIENTO : 169;
            int paisResidencia = client.DPP_ID_RESIDENCIA > 0 ? client.DPP_ID_RESIDENCIA : 169;


            fechaExpedicion = client.CLI_FECHA_EXPEDICION.Year < 1800
             ? DateTime.Today
             : client.CLI_FECHA_EXPEDICION;

            fechaAlta = client.CLI_FECHA_ALTA.Year < 1800
                ? DateTime.Today
                : client.CLI_FECHA_ALTA;

            fechaInscripcion = client.CLI_FECHA_INSCRIPCION.Year < 1800
                ? DateTime.Today
                : client.CLI_FECHA_INSCRIPCION;

            fechaAltaCalculada = client.CTS_SLP.Year < 1800
                ? DateTime.Today
                : client.CTS_SLP;

            fechaNacimiento = client.CLI_FECHA_NACIMIENTO.Year < 1800
                ? DateTime.Today
                : client.CLI_FECHA_NACIMIENTO;

            var fechaModificacion = client.DRC_FECHA_MODIFICACION.GetValueOrDefault().Year < 1800
                ? DateTime.Today
                : client.DRC_FECHA_MODIFICACION.GetValueOrDefault();
  

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), client.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), client.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLV_ID), client.CLV_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPA_ID), client.TPA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), client.CLI_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_EXPEDICION), fechaExpedicion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_IDENTIFICACION_EXPEDIDA), client.DPP_ID_PAIS_EXPEDICION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_IDENTIFICACION_EXPEDIDA), client.DPD_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_IDENTIFICACION_EXPEDIDA), client.DPC_ID_IDENTIFICACION_EXPEDIDA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_NOMBRE), client.CLI_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_NOMBRE), client.CLI_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PRIMER_APELLIDO), client.CLI_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_SEGUNDO_APELLIDO), client.CLI_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_NACIMIENTO), fechaNacimiento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_PAIS_NACIMIENTO), paisNacimiento);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID), client.DPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID), client.DPC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_SEXO), client.CAT_SEXO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CAT_ESTADO_CIVIL), client.CAT_ESTADO_CIVIL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_PROFESION), 1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_UNIDAD), client.CLI_UNIDAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.FRZ_ID), client.FRZ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.GRD_ID), client.GRD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTG_ID), client.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), client.UEJ_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_INSCRIPCION), fechaInscripcion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_FECHA_ALTA), fechaAlta);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CTS_SLP), fechaAltaCalculada);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_VALIDACION_BIOMETRICA), 1);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_REGIMEN), client.CLI_REGIMEN);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_INTEGRACION_GA2_PAYLOAD), client.CLI_INTEGRACION_GA2_PAYLOAD);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ECL_ID), client.ECL_ID);

            //Información económica
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_ID), client.CIE_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.ACC_ID), client.ACC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_MENSUAL), client.CIE_INGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_EGRESO_MENSUAL), client.CIE_EGRESO_MENSUAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_ACTIVOS), client.CIE_TOTAL_ACTIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PASIVOS), client.CIE_TOTAL_PASIVOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_TOTAL_PATRIMONIO), client.CIE_TOTAL_PATRIMONIO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_INGRESO_ADICIONAL), client.CIE_INGRESO_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CIE_CONCEPTO_INGRESO_ADICIONAL), client.CIE_CONCEPTO_INGRESO_ADICIONAL);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TasaVoluntariaAportes), client.TasaVoluntariaAportes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TasaAportes), client.TasaAportes);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TasaSalarioBase), client.TasaSalarioBase);

            //Información de direccion
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_ID), client.DRC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPP_ID_RESIDENCIA), paisResidencia);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPD_ID_RESIDENCIA), client.DPD_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DPC_ID_RESIDENCIA), client.DPC_ID_RESIDENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_DIRECCION), client.DRC_DIRECCION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_BARRIO), client.DRC_BARRIO);
           // parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DRC_FECHA_MODIFICACION), fechaModificacion);

            //Información de correo
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DCE_CORREO), client.DCE_CORREO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TCE_ID), client.TCE_ID);

            //Información de direccion
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.DTL_TELEFONO), client.DTL_TELEFONO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TPT_ID), client.TPT_ID);

            //Información cónyuge
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_TID_ID), client.CNY_TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_IDENTIFICACION), client.CNY_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_NOMBRE), client.CNY_PRIMER_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_NOMBRE), client.CNY_SEGUNDO_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_PRIMER_APELLIDO), client.CNY_PRIMER_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_SEGUNDO_APELLIDO), client.CNY_SEGUNDO_APELLIDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CNY_ACTIVO), 1);

            return await GetAsyncFirst<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Domain.Entities.Cuenta> InsertarCuentaIntegracion(Domain.Entities.Cuenta cuenta)
        {
            DateTime? fechaCreacion = cuenta.CTA_FECHA_CREACION;
            DateTime? fechaCierre = cuenta.CTA_FECHA_CIERRE;
            DateTime? fechaPrimerAporte = cuenta.CTA_FECHA_PRIMER_APORTE;

            if (cuenta.CTA_FECHA_CREACION.Year < 1800) { fechaCreacion = null; }
            if (cuenta.CTA_FECHA_CIERRE.Year < 1800) { fechaCierre = null; }
            if (cuenta.CTA_FECHA_PRIMER_APORTE.Year < 1800) { fechaPrimerAporte = null; }

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID_INTEGRACION), cuenta.CTA_ID_INTEGRACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CLI_ID), cuenta.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_NUMERO), cuenta.CTA_NUMERO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.ECT_ID), cuenta.ECT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.TCT_ID), cuenta.TCT_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_FECHA_CREACION), fechaCreacion);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_FECHA_CIERRE), fechaCierre);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_FECHA_PRIMER_APORTE), fechaPrimerAporte);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_SALDO), cuenta.CTA_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_SALDO_CANJE), cuenta.CTA_SALDO_CANJE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_SALDO_DISPONIBLE), cuenta.CTA_SALDO_DISPONIBLE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_CUOTAS), cuenta.CTA_CUOTAS);

            return await GetAsyncFirst<Domain.Entities.Cuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Dependiente> InsertarDependienteIntegracion(Dependiente dependiente)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumDependientParameters.CLI_ID), dependiente.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDependientParameters.TID_ID), dependiente.TID_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDependientParameters.DPT_IDENTIFICACION), dependiente.DPT_IDENTIFICACION);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDependientParameters.DPT_NOMBRE), dependiente.DPT_NOMBRE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumDependientParameters.DPT_PARTICIPACION), dependiente.DPT_PARTICIPACION);
            return await GetAsyncFirst<Dependiente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AportesCliente> InsertarAportesIntegracion(AportesCliente aportesCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            DateTime? fechaPrimera = aportesCliente.APC_FECHA_PRIMERA_CUOTA;
            DateTime? fechaUltima = aportesCliente.APC_FECHA_ULTIMA_CUOTA;
            if (aportesCliente.APC_FECHA_PRIMERA_CUOTA.Year < 1800) { fechaPrimera = null; }
            if (aportesCliente.APC_FECHA_ULTIMA_CUOTA.Year < 1800) { fechaUltima = null; }

            parameters.Add(HelpersEnums.GetEnumDescription(EnumAportesParameters.CLI_ID), aportesCliente.CLI_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAportesParameters.CGT_ID), aportesCliente.CTG_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAportesParameters.APC_CUOTAS_ACUMULADAS), aportesCliente.APC_CUOTAS_ACUMULADAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAportesParameters.APC_FECHA_PRIMERA_CUOTA), fechaPrimera);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumAportesParameters.APC_FECHA_ULTIMA_COUTA), fechaUltima);

            return await GetAsyncFirst<AportesCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }



        public async Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerCuentas(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), idCliente);

            return await GetAsyncList<Domain.Entities.Cuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AportesCliente>> ObtenerAportesCliente(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), idCliente);

            return await GetAsyncList<AportesCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Dependiente>> ObtenerDependientes(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_ID), idCliente);

            return await GetAsyncList<Dependiente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Domain.Entities.Movimiento> InsertarMovimientoIntegracion(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Concepto), movimiento.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Cuenta), movimiento.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Valor), movimiento.MVT_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.TipoMovimiento), movimiento.CAT_TIPO_MOVIMIENTO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.FechaProceso), movimiento.MVT_FECHA_PROCESO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.FechaMovimiento), DateTime.Today);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.AñoAportes), movimiento.MVT_ANO_PERIODO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.IdDocumento), movimiento.DCT_ID); 

            return await GetAsyncFirst<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CuentaAfiliado>> ObtenerCuentasMovimientosAfiliado(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), identificacion);

            
            var cuentas = await GetAsyncList<CuentaAfiliado>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            List<CuentaAfiliado> lista = new List<CuentaAfiliado>();
            if (cuentas.Count() > 0)
            {
                var conceptos = await _ga2Provider.ObtenerConceptosGA2();
                foreach (var cuenta in cuentas)
                {
                    var concepto = conceptos.Data.AsList().Find(x => x.IdConcepto == cuenta.CNC_ID);
                    CuentaAfiliado cuentaAfiliado = new CuentaAfiliado()
                    {
                        CNC_ID = cuenta.CNC_ID,
                        CNC_DESCRIPCION = concepto.Descripcion,
                        CTA_NUMERO = cuenta.CTA_NUMERO,
                        MOVIMIENTOS = cuenta.MOVIMIENTOS,
                        SALDO = cuenta.SALDO,
                        VALOR_SOLICITADO = cuenta.VALOR_SOLICITADO
                    };
                    lista.Add(cuentaAfiliado);
                }
            }
            return lista;
        }

        public async Task<Domain.Entities.Cuenta> ObtenerCuentaById(string request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaByIdParams.IDENTIFICACION), request);

            return await GetAsyncFirst<Domain.Entities.Cuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Domain.Entities.Movimiento>> ObtenerMovimientosCuenta(int idCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Cuenta), idCuenta);

            return await GetAsyncList<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Domain.Entities.Cuenta> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int numeroCuenta, int idCliente)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CLI_ID), idCliente);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_NUMERO), numeroCuenta);

            return await GetAsyncFirst<Domain.Entities.Cuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<Cliente>> ObtenerClientesById(string clientesIds)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLIENTES_IDS), clientesIds);

            return await GetAsyncList<Cliente>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ClienteIdentificacion>> ConsultarClientePorIdentificacion(string identificaciones)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.IDENTIFICACION), identificaciones);

            return await GetAsyncList<ClienteIdentificacion>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Domain.Entities.Cuenta>> ObtenerCuentaIdCuenta(string idCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaByIdParams.CTA_ID), idCuenta);

            return await GetAsyncList<Domain.Entities.Cuenta>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ConceptosSaldosBloqueos> InsertarConceptosSaldosBloqueos(ConceptosSaldosBloqueos conceptosSaldosBloqueos)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CTA_ID), conceptosSaldosBloqueos.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CNC_ID), conceptosSaldosBloqueos.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CCT_SALDO), conceptosSaldosBloqueos.CCT_SALDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CCT_FECHA_ULTIMO_APORTE), conceptosSaldosBloqueos.CCT_FECHA_ULTIMO_APORTE.Year < 1900 ? null : conceptosSaldosBloqueos.CCT_FECHA_ULTIMO_APORTE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CCT_INGRESOS), conceptosSaldosBloqueos.CCT_INGRESOS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumConceptosSaldosCuentas.CCT_SVA), conceptosSaldosBloqueos.CCT_SVA);

            return await GetAsyncFirst<ConceptosSaldosBloqueos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptosSaldosBloqueos>> ObtenerConceptosSaldosBloqueos(int idCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumCuentaParametros.CTA_ID), idCuenta);

            return await GetAsyncList<ConceptosSaldosBloqueos>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                                parameters, CommandType.StoredProcedure);
        }

        public async Task<ArchivoAplicacionPagoCSV> ObtenerClienteAplicacionPagos(int identificationType, string identificationNumber)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.TID_ID), identificationType);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.CLI_IDENTIFICACION), identificationNumber);

            return await GetAsyncFirst<ArchivoAplicacionPagoCSV>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InclusionAportes> InsertarInclusionSaldo(InclusionAportes inclusionAportes)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_ID), inclusionAportes.IPD_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_ID), inclusionAportes.IAP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_PERIODO_DESDE), inclusionAportes.IPD_PERIODO_DESDE);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_PERIODO_HASTA), inclusionAportes.IPD_PERIODO_HASTA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CNC_ID), inclusionAportes.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_VALOR), inclusionAportes.IPD_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_CUOTAS), inclusionAportes.IPD_CUOTAS);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_CUOTAS_ANTIGUAS), inclusionAportes.IPD_CUOTAS_ANTIGUAS);

            return await GetAsyncFirst<InclusionAportes>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InclusionAportesRequest> InsertarInclusionSaldoEncabezado(InclusionAportesRequest inclusionAportes)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_ID), inclusionAportes.IAP_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CTA_ID), inclusionAportes.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_NUMERO_REFERENCIA), inclusionAportes.IAP_NUMERO_REFERENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_OBSERVACIONES), inclusionAportes.IAP_OBSERVACIONES);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_GENERAR_ORDEN_RECAUDO), inclusionAportes.IAP_GENERAR_ORDEN_RECAUDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_FECHA_INICIAL_RECAUDO), inclusionAportes.IAP_FECHA_INICIAL_RECAUDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_FECHA_FINAL_RECAUDO), inclusionAportes.IAP_FECHA_FINAL_RECAUDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_APROBADA), inclusionAportes.IAP_APROBADA);

            return await GetAsyncFirst<InclusionAportesRequest>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ConsecutivoInclusionSaldos> ObtenerConsecutivoInclusionSaldos()
        {
            return await GetAsyncFirst<ConsecutivoInclusionSaldos>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), null, CommandType.StoredProcedure);
        }

        public async Task<InclusionAportesRequest> InsertarRecaudos(InclusionAportesRequest inclusionAportes, InclusionAportes aportes)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_NUMERO_REFERENCIA), inclusionAportes.IAP_NUMERO_REFERENCIA);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_FECHA_INICIAL_RECAUDO), inclusionAportes.IAP_FECHA_INICIAL_RECAUDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_FECHA_FINAL_RECAUDO), inclusionAportes.IAP_FECHA_FINAL_RECAUDO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_VALOR), aportes.IPD_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_ID), aportes.IAP_ID);

            return await GetAsyncFirst<InclusionAportesRequest>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ConceptoCuenta>> ObtenerCuentasConceptoPorIdentificadorCliente(int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CLI_ID), idCliente);

            return await GetAsyncList<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ConceptoCuenta> ActualizarSaldosCuentasConceptos(decimal valor, int idConcepto, int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CLI_ID), idCliente);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_VALOR), valor);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CNC_ID), idConcepto);

            return await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<MovimientosCuenta> InsertarMovimientoInclusionSaldos(MovimientosCuenta movimiento, int idCliente)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.CLI_ID), idCliente);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Concepto), movimiento.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Valor), movimiento.MVT_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.FechaProceso), movimiento.MVT_FECHA_PROCESO);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_ID), movimiento.IPD_ID);

            return await GetAsyncFirst<MovimientosCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InclusionAportesRequest> EliminarInclusionSaldosEncabezado(int inclusionAportes)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IAP_ID), inclusionAportes);

            return await GetAsyncFirst<InclusionAportesRequest>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<InclusionAportes> EliminarInclusionSaldos(int inclusionAportes)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumInclusionSaldoParameters.IPD_ID), inclusionAportes);

            return await GetAsyncFirst<InclusionAportes>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ResponseDatosSimulacionParaEstudio> ObtenerCuotasNumeroIdentificacion(string numeroDocumento)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.IDENTIFICACION), numeroDocumento);

            return await GetAsyncFirst<ResponseDatosSimulacionParaEstudio>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<Domain.Entities.Movimiento> InsertarMovimiento(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Cuenta), movimiento.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Concepto), movimiento.CNC_ID);            
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Valor), movimiento.MVT_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.TipoMovimiento), movimiento.CAT_TIPO_MOVIMIENTO);
            
            //PREGUNTAR AL INGE SI ES RELEVANTE
            //parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.IdDocumento), movimiento.DCT_ID);

            return await GetAsyncFirst<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ConceptoCuenta> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CTA_ID", movimiento.CTA_ID);
            parameters.Add("@CNC_ID", movimiento.CNC_ID);
            parameters.Add("@VALOR_CTA", movimiento.VALOR_CTA);

            return await GetAsyncFirst<ConceptoCuenta>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            
        }




        public async Task<EstadoCuenta> ObetenerNombreEstadoCuenta(int IdEstadoCuenta)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumObetenerNombreEstadoCuenta.Cuenta), IdEstadoCuenta);

            var result = await GetAsyncFirst<EstadoCuenta>(HelperDBParameters.BuilderFunction(
             HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Cliente>> ObtenetClientesPorUnidadEjecutora(int idUnidadEjecutora)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumClientParameters.UEJ_ID), idUnidadEjecutora);

            var reuslt = await GetAsyncList<Cliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return reuslt;
        }

        public async Task<Domain.Entities.Movimiento> InsertarMovimientoAbonoInteres(Domain.Entities.Movimiento movimiento)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Cuenta), movimiento.CTA_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Concepto), movimiento.CNC_ID);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.Valor), movimiento.MVT_VALOR);
            parameters.Add(HelpersEnums.GetEnumDescription(EnumMovimientoParameters.TipoMovimiento), movimiento.CAT_TIPO_MOVIMIENTO);

            
            return await GetAsyncFirst<Domain.Entities.Movimiento>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ClienteCedulaReporte> ObtenerClienteReporte(string Identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerClienteReporte.CLI_IDENTIFICACION), Identificacion);
            ClienteCedulaReporte clienteCedula = new ClienteCedulaReporte();

            clienteCedula = await GetAsyncFirst<ClienteCedulaReporte>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return clienteCedula;

        }
        public async Task<ClienteNomina> ObtenerClienteNomina(string Identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(HelpersEnums.GetEnumDescription(EnumObtenerClienteReporte.CLI_IDENTIFICACION), Identificacion);
            ClienteNomina clienteCedula = new ClienteNomina();

            clienteCedula = await GetAsyncFirst<ClienteNomina>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);

            return clienteCedula;

        }


        public async Task<AfiliadoDeBeneficiario> ObtenerAfiliadoDeBeneficiario(string identificacion)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Identificacion", identificacion);
            return await GetAsyncFirst<AfiliadoDeBeneficiario>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Cliente>> ObtenerNombreClientePorCedulas(string idUnidadEjecutora)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CedulasAfiliados", idUnidadEjecutora);
            return await GetAsyncList<Cliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<ConsultarPermisoActualizacionDto> ConsultarPermisoActualizacion(string request)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@PERFIL", request);
            return await GetAsyncFirst<ConsultarPermisoActualizacionDto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<ResponseInsertarPermisoActualizacionDto> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PERFIL", request.Perfil);
            parameters.Add("@ELIMINA", request.Elimina);
            return await GetAsyncFirst<ResponseInsertarPermisoActualizacionDto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ReporteFuerza>> ConsultaReporteAfiliadosPorFuerza(int idFuerza)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FRZ_ID", idFuerza);
            return await GetAsyncList<ReporteFuerza>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<ReporteTotalesDto>> GenerarReporteTotalesPorConcepto()
        {
            DynamicParameters parameters = new DynamicParameters();

            return await GetAsyncList<ReporteTotalesDto>(
                HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO),
                parameters,
                CommandType.StoredProcedure);
        }
       
        public async Task<IEnumerable<ReporteAfiliadosResponseDto>> ConsultaReporteAfiliados(Application.Dto.ReporteAfiliadosRequestDto requestDto)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTipoIdentificacion", requestDto.IdTipoIdentificacion);
            parameters.Add("@Identificacion", requestDto.Identificacion);
            parameters.Add("@IdFuerza", requestDto.IdFuerza);
            parameters.Add("@IdUnidadEjecutora", requestDto.IdUnidadEjecutora);
            parameters.Add("@IdCategoria", requestDto.IdCategoria);
            parameters.Add("@IdGrado", requestDto.IdGrado);
            parameters.Add("@IdTipoVinculacion", requestDto.IdTipoVinculacion);
            parameters.Add("@estado", requestDto.estado);

            parameters.Add("@FechaInscripcionDesde", requestDto.FechaInscripcionDesde == DateTime.MinValue ? null : requestDto.FechaInscripcionDesde);
            parameters.Add("@FechaInscripcionHasta", requestDto.FechaInscripcionHasta == DateTime.MinValue ? null : requestDto.FechaInscripcionHasta);
            parameters.Add("@FechaActualizacionDesde", requestDto.FechaActualizacionDesde == DateTime.MinValue ? null : requestDto.FechaActualizacionDesde);
            parameters.Add("@FechaActualizacionHasta", requestDto.FechaActualizacionHasta == DateTime.MinValue ? null : requestDto.FechaActualizacionHasta);
            parameters.Add("@RegimenDesde", requestDto.RegimenDesde);
            parameters.Add("@RegimenHasta", requestDto.RegimenHasta);
            return await GetAsyncList<ReporteAfiliadosResponseDto>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Cliente>> GenerarReporteDatosBasicosAfiliado(int idDepartamento, int idCiudad)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@DPD_ID", idDepartamento);
            parameters.Add("@DPC_ID", idCiudad);

            return await GetAsyncList<Cliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<InformacionBasicaCliente>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos)
        {
            var parameters = new DynamicParameters();
            var dataTable = new DataTable();
            dataTable.Columns.Add("DOCUMENTO", typeof(string));

            foreach (string documento in documentos)
            {
                dataTable.Rows.Add(documento);
            }

            parameters.Add("@IDENTIFICACIONES", dataTable.AsTableValuedParameter("[DocumentosClientes]"));

            return await GetAsyncList<InformacionBasicaCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AnotacionCliente> InsertarAnotacionCliente(AnotacionClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ANOTACION", request.Anotacion);
            parameters.Add("@USUARIO", request.Usuario);
            parameters.Add("@CLI_ID", request.ClienteId);
            parameters.Add("@FECHA", DateTime.Now);
            return await GetAsyncFirst<AnotacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<AnotacionCliente>> ObtenerAnotacionesCliente(AnotacionClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CLI_ID", request.ClienteId);

            return await GetAsyncList<AnotacionCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CausalEstadoCliente>> ObtenerCausalesEstadoCliente(CausalEstadoClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CLI_ID", request.ClienteId);

            return await GetAsyncList<CausalEstadoCliente>(HelperDBParameters.BuilderFunction(
                 HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<CausalEstadoCliente> InsertarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RAZON", request.Razon);
            parameters.Add("@ESTADO", request.EstadoId);
            parameters.Add("@CAUSAL", request.CausalId);
            parameters.Add("@CLI_ID", request.ClienteId);
            parameters.Add("@OBSERVACIONES", request.Observaciones);
            parameters.Add("@USUARIO", request.Usuario);
            parameters.Add("@FECHA",DateTime.Now);

            return await GetAsyncFirst<CausalEstadoCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<AnotacionCliente> EliminarAnotacionCliente(AnotacionClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ACL_ANOTACION_ID", request.AnotacionClienteId);

            return await GetAsyncFirst<AnotacionCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<CausalEstadoCliente> EliminarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ECC_ESTADO_CAUSAL_ID", request.CausalEstadoId);

            return await GetAsyncFirst<CausalEstadoCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }

        public async Task<CausalEstadoCliente> LevantarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ECC_ESTADO_CAUSAL_ID", request.CausalEstadoId);
            parameters.Add("@RAZON", request.Razon);
            parameters.Add("@FECHA", DateTime.Now);

            return await GetAsyncFirst<CausalEstadoCliente>(HelperDBParameters.BuilderFunction(
                HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}
