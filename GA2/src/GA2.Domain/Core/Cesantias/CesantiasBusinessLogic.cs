using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using GA2.Transversals.Commons.Constants;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using System.Xml;
using Newtonsoft.Json;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Application.Dto.VerificacionDerechos;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Consecutivo Logica
    /// <author>Erwin Pantoja España</author>
    /// <date>27/09/2021</date>
    /// </summary>
    /// 
    public class CesantiasBusinessLogic : ICesantiasBusinessLogic
    {

        private readonly IMapper _mapper;
        private readonly ICesantiasRepository _cesantiasRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly IConsumoWSTercero _wsTercero;
        private readonly IConsumoCDP _wsCDP;
        private readonly IConsumoWSProveedor _wsProveedor;
        private readonly IConsumoWSFactura _wsFactura;
        private readonly IConsumoSCnMcont _wsMovContable;
        private readonly IVerificacionDerechosBusinessLogic _verificacionDerechosBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="cesantiasRepository"></param>
        /// <param name="iClientRequestProvider"></param>
        /// <param name="catalogsRepository"></param>
        /// <param name="solicitudRepository"></param>
        /// <param name="cuentasConceptosBusinessLogic"></param>
        /// <param name="wsTercero"></param> 
        /// <param name="wsCDP"></param> 
        public CesantiasBusinessLogic(
            IMapper mapper,
            ICesantiasRepository cesantiasRepository,
            IClientRequestProvider iClientRequestProvider,
            ICatalogosRepository catalogsRepository,
            ISolicitudRepository solicitudRepository,
            IConsumoWSTercero wsTercero,
            IConsumoCDP wsCDP,
            IConsumoWSProveedor wsProveedor,
            IConsumoWSFactura wsFactura,
            IConsumoSCnMcont wsMovContable,
            IVerificacionDerechosBusinessLogic verificacionDerechosBusinessLogic
            )
        {
            _mapper = mapper;
            _cesantiasRepository = cesantiasRepository;
            _iClientRequestProvider = iClientRequestProvider;
            _catalogsRepository = catalogsRepository;
            _solicitudRepository = solicitudRepository;
            _wsTercero = wsTercero ?? throw new ArgumentNullException(nameof(wsTercero));
            _wsCDP = wsCDP ?? throw new ArgumentNullException(nameof(wsCDP));
            _wsProveedor = wsProveedor ?? throw new ArgumentNullException(nameof(wsProveedor));
            _wsFactura = wsFactura ?? throw new ArgumentNullException(nameof(wsFactura));
            _wsMovContable = wsMovContable ?? throw new ArgumentNullException(nameof(wsMovContable));
            _verificacionDerechosBusinessLogic = verificacionDerechosBusinessLogic;
        }

        /// <summary>
        /// Metodo logica de negocio para obtener la solicitud del tramite para cesantias
        /// </summary>
        /// <author>Erwin Pantoja España</author>
        /// <param name="solicitud"></param>
        /// <returns>IEnumerable<ObtenerTramiteCesantiasDto></returns>
        public async Task<Response<ObtenerTramiteCesantiasDto>> ObtenerTramiteCesantias(ParametrosObtenerCesantiasDto solicitud)
        {
            Response<ClienteCesantiasDto> clienteDto = new Response<ClienteCesantiasDto>();
            clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(0, "", (int)solicitud.IdCliente);

            Response<ObtenerTramiteCesantiasDto> respuesta = new Response<ObtenerTramiteCesantiasDto>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El afiliado no existe en BUA";
                return respuesta;
            }

            var tramiteSolicitud = this._mapper.Map<ParametrosObtenerCesantias>(solicitud);
            tramiteSolicitud.CLI_ID = clienteDto.Data.IdCliente;


            respuesta.Data = this._mapper.Map<ObtenerTramiteCesantiasDto>(await this._cesantiasRepository.ObtenerTramiteCesantias(tramiteSolicitud));

            respuesta.Data.NOMBRE_CLIENTE = $"{clienteDto.Data.PrimerNombre} {clienteDto.Data.SegundoNombre} {clienteDto.Data.PrimerApellido} {clienteDto.Data.SegundoApellido}";
            respuesta.Data.CORREO_PERSONAL = clienteDto.Data.CorreoPersonal;
            respuesta.Data.NO_CELULAR = clienteDto.Data.NumeroCelular;
            respuesta.Data.TELEFONO_RESIDENCIA = clienteDto.Data.TelefonoResidencia;
            respuesta.Data.CEDULA_CLIENTE = int.Parse(clienteDto.Data.Identificacion);
            respuesta.Data.DIRECCION_RESIDENCIA = clienteDto.Data.Direccion;

            return respuesta;
        }

        /// <summary>
        /// Metodo logica de negocio para obtener la información básica del tramite de cesantías.
        /// </summary>
        /// <param name="numeroIdentificacion"></param>
        /// <returns></returns>
        public async Task<Response<InformacionBasicaCesantiasDto>> InformacionBasicaCesantias(int numeroIdentificacion)
        {
            Response<ClienteCedulaDto> clienteDto = await
                _iClientRequestProvider.ObtenerInformacionClienteCedula(numeroIdentificacion);

            IEnumerable<TipoIdentificacion> litaTipoIdentificacion = await _catalogsRepository.ObtenerTiposIdentificacion();
            IEnumerable<Entities.Departamento> departamentos = await _catalogsRepository.ObtenerDepartamentos();
            IEnumerable<Ciudad> ciudad = await _catalogsRepository.ObtenerCiudades();
            IEnumerable<CatalogoValor> catalogoValor = await _catalogsRepository.ObtenerValoresCatalogos();
            IEnumerable<Entities.Fuerzas> fuerzas = await _catalogsRepository.ObtenerFuerzas();
            IEnumerable<Entities.Categoria> categorias = await _catalogsRepository.ObtenerCategorias();
            IEnumerable<Entities.Grado> grados = await _catalogsRepository.ObtenerGrados();
            IEnumerable<Profesion> profesiones = await _catalogsRepository.ObtenerProfesiones();


            Response<InformacionBasicaCesantiasDto> respuesta = new Response<InformacionBasicaCesantiasDto>();
            InformacionBasicaCesantiasDto informacionBasicaCesantias = new InformacionBasicaCesantiasDto();

            informacionBasicaCesantias.clientIdentificationType = litaTipoIdentificacion
                                                    .Where(x => x.TID_ID == clienteDto.Data.IdTipoIdentificacion)
                                                    .FirstOrDefault()
                                                    .TID_DESCRIPCION;
            informacionBasicaCesantias.sobIDNumber = clienteDto.Data.Identificacion;
            informacionBasicaCesantias.sobExpeditionDate = clienteDto.Data.FechaExpedicionDocumento;
            informacionBasicaCesantias.expeditionState = departamentos.Where(x => x.DPD_ID == clienteDto.Data.IdDepartamentoExpedicionDocumento)
                                                    .FirstOrDefault()
                                                    .DPD_NOMBRE;
            informacionBasicaCesantias.expeditionCity = ciudad.Where(x => x.DPC_ID == clienteDto.Data.IdCiudadExpedicionDocumento)
                                                    .FirstOrDefault()
                                                    .DPC_NOMBRE;
            informacionBasicaCesantias.sobFirstLastName = clienteDto.Data.PrimerApellido;
            informacionBasicaCesantias.sobSecondLastName = clienteDto.Data.SegundoApellido;
            informacionBasicaCesantias.sobFirstName = clienteDto.Data.PrimerNombre;
            informacionBasicaCesantias.sobMiddleName = clienteDto.Data.SegundoNombre;
            informacionBasicaCesantias.sobBirthDate = clienteDto.Data.FechaNacimiento;
            informacionBasicaCesantias.bornState = departamentos.Where(x => x.DPD_ID == clienteDto.Data.IdDepartamentoNacimiento)
                                                    .FirstOrDefault()
                                                    .DPD_NOMBRE;
            informacionBasicaCesantias.bornCity = ciudad.Where(x => x.DPC_ID == clienteDto.Data.IdCiudadNacimiento)
                                                    .FirstOrDefault()
                                                    .DPC_NOMBRE;
            informacionBasicaCesantias.sobSex = catalogoValor.Where(x => x.CAT_ID == 1 && x.CVL_CODIGO == clienteDto.Data.Sexo)
                                                    .FirstOrDefault()
                                                    .CVL_DESCRIPCION;
            informacionBasicaCesantias.civilState = catalogoValor.Where(x => x.CAT_ID == 2 && x.CVL_CODIGO == clienteDto.Data.EstadoCivil)
                                                    .FirstOrDefault()
                                                    .CVL_DESCRIPCION;
            informacionBasicaCesantias.educationLevel = "Falta";
            informacionBasicaCesantias.force = fuerzas.Where(x => x.FRZ_ID == clienteDto.Data.IdFuerza)
                                                    .FirstOrDefault()
                                                    .FRZ_DESCRIPCION;
            informacionBasicaCesantias.category = categorias.Where(x => x.CTG_ID == clienteDto.Data.IdCategoria)
                                                    .FirstOrDefault()
                                                    .CTG_DESCRIPCION;
            informacionBasicaCesantias.grade = grados.Where(x => x.GRD_ID == clienteDto.Data.IdGrado)
                                                    .FirstOrDefault()
                                                    .GRD_DESCRIPCION;
            informacionBasicaCesantias.profession = profesiones.Where(x => x.PRF_ID == clienteDto.Data.IdProfesion)
                                                    .FirstOrDefault()
                                                    .PRF_DESCRIPCION;
            respuesta.Data = informacionBasicaCesantias;

            return respuesta;
        }
        /// <summary>
        /// Paso 1 integración Cesantias con Seven creación del Tercero.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarGnTerceResponse>> InsertTerceroSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken)
        {
            Response<InsertarGnTerceResponse> response = new();
            int idCliente = solicitud.IdCliente != null ? solicitud.IdCliente.Value : 0;
            var cliente = (await _iClientRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            var terceroRequest = new InsertarGnTerceRequest
            {
                emp_codi = 294,
                tip_codi = cliente.IdTipoIdentificacion,
                ter_coda = cliente.Identificacion,
                ter_dive = 0,
                ter_nomb = $"{cliente.PrimerNombre} {cliente.SegundoNombre}",
                ter_apel = $"{cliente.PrimerApellido} {cliente.SegundoApellido}",
                ter_nocm = string.Empty,
                mod_codi = 1,
                pai_codi = cliente.IdPaisResidencia,
                dep_codi = cliente.IdDepartamentoResidencia,
                mun_codi = cliente.IdCiudadResidencia,
                ter_dire = cliente.Direccion,
                ter_ntel = cliente.Telefono,
                ter_mail = cliente.Correo,
                ter_nfax = string.Empty,
                arb_clte = "0",
                ter_audp = "N"
            };

            response.Data = await _wsTercero.InsertarGnTerceAsync(terceroRequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.InsertarGnTerceResult : "Error inesperado creado el tercero en Seven.";

            return response;
        }
        /// <summary>
        /// Paso 2 integración Cesantias con Seven creación del CDP
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<CrearCDPResponse>> CrearCDPSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken)
        {
            Response<CrearCDPResponse> response = new();
            int idCliente = solicitud.IdCliente != null ? solicitud.IdCliente.Value : 0;
            var cliente = (await _iClientRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            ObtenerSolicitud datosSolicitud = await _solicitudRepository.ConsultaSolicitud((Guid)solicitud.solicitudId);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud no fué encontrada";
                return response;
            }

            GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre[] tSPgDmpres = ObtenerDeTalleCDP(cliente, datosSolicitud).Result.vDetalle;

            var request = new CrearCDPDto
            {
                Emp_codi = 294,
                Top_codi = 3015,
                Mpr_nume = 0,
                Mpr_fech = DateTime.Now,
                Mpr_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - {datosSolicitud.SOL_RADICADO_GD} - {datosSolicitud.SOL_NUMERO_SOLICITUD}",
                Arb_csuc = "1001",
                Ter_coda = cliente.Identificacion,
                Mpr_tdos = "COMPROBANTE",
                Mpr_ndos = 0,
                Uni_codi = 1
            };

            var _data = new TSPgMpres();
            _data.Emp_codi = request.Emp_codi;
            _data.Top_codi = request.Top_codi;
            _data.Mpr_nume = request.Mpr_nume;
            _data.Mpr_fech = request.Mpr_fech;
            _data.Mpr_desc = request.Mpr_desc;
            _data.Arb_csuc = request.Arb_csuc;
            _data.Ter_coda = request.Ter_coda;
            _data.Mpr_tdos = request.Mpr_tdos;
            _data.Mpr_ndos = request.Mpr_ndos;
            _data.Uni_codi = request.Uni_codi;
            _data.vDetalle = tSPgDmpres;

            var cdprequest = new CrearCDPRequest(_data);
            
            response.Data = await _wsCDP.CrearCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.CrearCDPResult : "Error inesperado creando el CDP en Seven.";

            if (response.Data.CrearCDPResult.Contains("MPR_CONT"))
            {
                int MPR_CONT = GetValueResponseXmlSeven(response.Data.CrearCDPResult, 0);
                await this._solicitudRepository.ActualizaMPRCONTSolicitud(solicitud, MPR_CONT);
            }

            return response;
        }
        /// <summary>
        /// Construye el detalle del CDP.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        private async Task<TSPgMpres> ObtenerDeTalleCDP(ClienteDto cliente, ObtenerSolicitud solicitud)
        {
            TSPgMpres detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre>();
            double valorRetiro = (double)solicitud.SOL_VALOR_A_RETIRAR;
            double saldoRetirado = 0;
            double saldoPendiente = (double)solicitud.SOL_VALOR_A_RETIRAR;
            int IdCuentaActiva = 0;
            int tcpId = 0;
            var tipoSolicitud = await _cesantiasRepository.ObtenerTipoSolicitudPorSolIdySubModalidad(solicitud.SOL_ID, solicitud.TPS_SUB_ID);
            Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);

            if (tipoSolicitud.PCS_NOMBRE == "VIVIENDA 14" || tipoSolicitud.PCS_NOMBRE == "VIVIENDA 8" || tipoSolicitud.PCS_NOMBRE == "SUBSIDIO SECUESTRADOS")
            {
                IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1 || x.EstadoCuentaId == 2).Select(x => x.CuentaId).FirstOrDefault();
                tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1 || x.EstadoCuentaId == 2).Select(x => x.TipoCuentaId).FirstOrDefault();
            }
            else
            {
                IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
                tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
            }
            Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _cesantiasRepository.ObtenerOrdenDescuentoSaldos());
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            foreach (var item in orden)
            {
                var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                if (cuenta != null)
                {
                    if (saldoPendiente > 0)
                    {
                        if (saldoPendiente <= cuenta.ConceptoSaldoCuenta)
                        {
                            saldoRetirado = saldoPendiente;
                            saldoPendiente = 0;
                        }
                        else
                        {
                            saldoRetirado = cuenta.ConceptoSaldoCuenta;
                            saldoPendiente -= saldoRetirado;
                        }

                        var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre
                        {
                            Rub_codi = "0",
                            Pro_codi = item.concepto,
                            Arb_codc = "107706",
                            Arb_coda = codAreanegocio.ToString(),
                            Arb_codp = "099",
                            Dmp_valo = saldoRetirado,
                            Dmp_desc = $"{solicitud.SOL_RADICADO_GD}-{item.preProducto}"
                        };

                        items.Add(itemDetalle);
                    }
                }
            }

            detalle.vDetalle = items.ToArray();

            return detalle;
        }
        /// <summary>
        /// Obtiene el número de MPR_CONT que se da al crear el CDP en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>
        private int GetValueResponseXmlSeven(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("MPR_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("PVD_CODD");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }

        /// <summary>
        /// Obtiene el número de FAC_CONT que se da al crear Factura  en Seven.
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>CDP
        private int GetValueResponseXmlSevenFactura(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            switch (sevenOpc)
            {
                case 0:
                    XmlNodeList elemlistCDP = xmlString.GetElementsByTagName("FAC_CONT");
                    Valor = Convert.ToInt32(elemlistCDP[0].InnerXml);
                    break;
                case 1:
                    XmlNodeList elemlistProve = xmlString.GetElementsByTagName("FAC_NUME");
                    Valor = Convert.ToInt32(elemlistProve[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }

        /// <summary>
        /// Paso 3 Integración Cesantías con Seven creación Proveedor.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<SyncPoPvdorResponse>> CrearProveedorSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken)
        {
            Response<SyncPoPvdorResponse> response = new();
            var tercero = (await _solicitudRepository.ConsultarTerceroBeneficiario((Guid)solicitud.solicitudId, SolicitudConstants.TipoTerceroBeneficiarioId)).FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            var ProveeRequest = new SyncPoPvdorRequest()
            {
                emp_codi = 294,
                tip_codi = tercero.TID_ERP,
                pvd_coda = tercero.TER_NUMERO_DOCUMENTO,
                pvd_dive = 0,
                pvd_nomb = $"{tercero.TER_PRIMER_NOMBRE} {tercero.TER_SEGUNDO_NOMBRE}",
                pvd_apel = $"{tercero.TER_PRIMER_APELLIDO} {tercero.TER_SEGUNDO_APELLIDO}",
                pvd_noco = tercero.TER_NOMBRE_RAZON_SOCIAL,
                pai_codi = tercero.DPP_CODIGO,
                dep_codi = Convert.ToInt16(tercero.DPD_CODIGO),
                mun_codi = tercero.DPC_CODIGO,
                tpr_codi = 0,
                pvd_clad = tercero.TID_EMPRESARIAL == 1 ? "J" : "N",
                arb_csuc = "1001",
                cal_codi = 0,
                coc_codi = 1,
                cim_codi = 98,
                act_codi = 0,
                mon_codi = 1,
                dep_dire = tercero.TER_DIRECCION,
                cup_nume = tercero.TER_NUMERO_CUENTA,
                cup_tipo = tercero.CVL_CODIGO == "1" ? "A" : "C",
                ban_codi = tercero.ENT_CODIGO_BANCO,
                sub_codi = 1,
                cup_idba = "0",
                cup_esta = "A",
                pvd_audp = "N",
                cup_clas = "S",
                pvr_riva = "C",
                pvr_auto = "N"
            };

            response.Data = await _wsProveedor.SyncPoPvdorAsync(ProveeRequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncPoPvdorResult : "Error inesperado creando el proveedor en Seven";

            if (response.Data.SyncPoPvdorResult.Contains("PVD_CODD"))
            {
                int codProvee = GetValueResponseXmlSeven(response.Data.SyncPoPvdorResult, 1);
                await this._solicitudRepository.ActualizarCodProvSevenTerceroBeneficiario(solicitud, SolicitudConstants.TipoTerceroBeneficiarioId, codProvee);
            }

            return response;
        }
        /// <summary>
        /// Paso 4 creación de factura Proveedor en Seven basado en la info. de la solicitud el tercero y el cliente.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarFacturaPResponse>> CrearFacturaProveSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken)
        {
            Response<InsertarFacturaPResponse> response = new();
            int idCliente = solicitud.IdCliente != null ? solicitud.IdCliente.Value : 0;
            var cliente = (await _iClientRequestProvider.ObtenerClienteInformacion(idCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            var tercero = (await _solicitudRepository.ConsultarTerceroBeneficiario((Guid)solicitud.solicitudId, SolicitudConstants.TipoTerceroBeneficiarioId)).FirstOrDefault();

            if (tercero == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "Debe agregar un beneficiario de pago a la solicitud.";
                return response;
            }

            ObtenerSolicitud datosSolicitud = await _solicitudRepository.ConsultaSolicitud((Guid)solicitud.solicitudId);

            if (datosSolicitud == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "La solicitud no fué encontrada";
                return response;
            }

            var _dataFactuProv = new TSPoFactu();           

            _dataFactuProv.Emp_codi = 294;
            _dataFactuProv.Top_codi = 4170;
            _dataFactuProv.Fac_nume = 0;
            _dataFactuProv.Fac_fech = DateTime.Now;
            _dataFactuProv.Arb_cods = "1001";
            _dataFactuProv.Fac_desc = $"{tercero.TER_NUMERO_DOCUMENTO} - {tercero.TER_PRIMER_NOMBRE} {tercero.TER_SEGUNDO_NOMBRE} {tercero.TER_PRIMER_APELLIDO} " +
                $"{tercero.TER_SEGUNDO_APELLIDO} - {datosSolicitud.SOL_RADICADO_GD} - {datosSolicitud.SOL_NUMERO_SOLICITUD}";
            _dataFactuProv.Pvd_coda = tercero.TER_NUMERO_DOCUMENTO;
            _dataFactuProv.Dep_codd = tercero.PVD_CODD ?? 0;
            _dataFactuProv.Fac_pref = datosSolicitud.SOL_NUMERO_SOLICITUD.ToString();
            _dataFactuProv.Fac_nfap = datosSolicitud.SOL_RADICADO_GD;
            _dataFactuProv.Mon_codi = 1;
            _dataFactuProv.Fac_tipo = "C";
            _dataFactuProv.Mpr_cont = int.Parse(datosSolicitud.MPR_CONT);            
            _dataFactuProv.Fac_feta = DateTime.Now;
            GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact[] detalleFactuProve = ObtenerDetalleFactuProveedor(cliente, datosSolicitud).Result.vDetalle;
            _dataFactuProv.vDetalle = detalleFactuProve;

            var body = new InsertarFacturaPRequestBody(_dataFactuProv);
            var facturarequest = new InsertarFacturaPRequest(body);

            response.Data = await _wsFactura.InsertarFacturaPAsync(facturarequest, cancellationToken);

            if (response.Data != null)
            {
                int FAC_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 0);
                int FAC_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1);
                await ActualizarSaldosyMovimientosCtaCli(cliente, datosSolicitud);
                await _solicitudRepository.ActualizaFACTSolicitud(solicitud.solicitudId, FAC_CONT.ToString(), FAC_NUME.ToString());
            }
            else
                response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "Error inesperado al crear la factura en Seven.";

            return response;
        }

        private async Task<TSPoFactu> ObtenerDetalleFactuProveedor(ClienteDto cliente, ObtenerSolicitud solicitud)
        {
            TSPoFactu detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact>();
            double valorRetiro = (double)solicitud.SOL_VALOR_A_RETIRAR;
            double saldoRetirado = 0;
            double saldoPendiente = (double)solicitud.SOL_VALOR_A_RETIRAR;
            int IdCuentaActiva = 0;
            int tcpId=0;
            var tipoSolicitud = await _cesantiasRepository.ObtenerTipoSolicitudPorSolIdySubModalidad(solicitud.SOL_ID, solicitud.TPS_SUB_ID);
            Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);

            if (tipoSolicitud.PCS_NOMBRE== "VIVIENDA 14" || tipoSolicitud.PCS_NOMBRE == "VIVIENDA 8" || tipoSolicitud.PCS_NOMBRE == "SUBSIDIO SECUESTRADOS")
            {
                IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1 || x.EstadoCuentaId == 2).Select(x => x.CuentaId).FirstOrDefault();
                tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1 || x.EstadoCuentaId == 2).Select(x => x.TipoCuentaId).FirstOrDefault();
            }
            else
            {
                IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
                tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
            }
            
            
            Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _cesantiasRepository.ObtenerOrdenDescuentoSaldos());
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            foreach (var item in orden)
            {
                var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                if (cuenta != null)
                {
                    if (saldoPendiente > 0)
                    {
                        if (saldoPendiente <= cuenta.ConceptoSaldoCuenta)
                        {
                            saldoRetirado = saldoPendiente;
                            saldoPendiente = 0;
                        }
                        else
                        {
                            saldoRetirado = cuenta.ConceptoSaldoCuenta;
                            saldoPendiente -= saldoRetirado;
                        }

                        var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact
                        {
                            Pro_codi = item.concepto,
                            Dfa_dest = "100",
                            Dfa_valo = saldoRetirado,
                            Arb_codc = "107706",
                            Arb_coda = codAreanegocio.ToString(),
                            Arb_codp = "099"
                        };
                        items.Add(itemDetalle);
                    }
                }
            }

            detalle.vDetalle = items.ToArray();

            return detalle;
        }

        private async Task<Entities.Movimiento> ActualizarSaldosyMovimientosCtaCli(ClienteDto cliente, ObtenerSolicitud solicitud)
        {
            Entities.Movimiento movimiento = new Entities.Movimiento();
            double valorRetiro = (double)solicitud.SOL_VALOR_A_RETIRAR;
            double saldoRetirado = 0;
            double saldoPendiente = (double)solicitud.SOL_VALOR_A_RETIRAR;

            Response<IEnumerable<CuentaClienteDto>> cuentasCliente = await _iClientRequestProvider.ObtenerCuentasClienteId(cliente.IdCliente);
            int IdCuentaActiva = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.CuentaId).FirstOrDefault();
            int tcpId = cuentasCliente.Data.Where(x => x.EstadoCuentaId == 1).Select(x => x.TipoCuentaId).FirstOrDefault();
            Response<IEnumerable<CuentaPorConceptoDto>> conceptoCuenta = await _iClientRequestProvider.ObtenerCuentaConceptoIdCuenta(IdCuentaActiva);
            IEnumerable<ObtenerOrdenDescuentosDto> orden = _mapper.Map<IEnumerable<ObtenerOrdenDescuentosDto>>(await _cesantiasRepository.ObtenerOrdenDescuentoSaldos());
            

            foreach (var item in orden)
            {
                var cuenta = conceptoCuenta.Data.Where(x => x.IdConcepto == item.conceptoId).FirstOrDefault();

                if (cuenta != null)
                {
                    if (saldoPendiente > 0)
                    {
                        if (saldoPendiente <= cuenta.ConceptoSaldoCuenta)
                        {
                            saldoRetirado = saldoPendiente;
                            saldoPendiente = 0;
                        }
                        else
                        {
                            saldoRetirado = cuenta.ConceptoSaldoCuenta;
                            saldoPendiente -= saldoRetirado;
                        }

                        movimiento.CTA_ID = IdCuentaActiva;
                        movimiento.CNC_ID = item.conceptoId;
                        movimiento.MVT_VALOR = (decimal)saldoRetirado;
                        movimiento.CAT_TIPO_MOVIMIENTO = "D";

                        movimiento = _iClientRequestProvider.InsertarMovimiento(movimiento).Result.Data;
                    }
                }
            }

            movimiento.VALOR_CTA = solicitud.SOL_VALOR_A_RETIRAR;
            var result = _iClientRequestProvider.ActualizarSaldoDebitoCuenta(movimiento);
            
            return movimiento;
        }
        /// <summary>
        /// Obtiene el número de RETORNO y TXTERROR que nos devuelve Seven en cada proceso
        /// </summary>
        /// <param name="xmlResponse"></param>
        /// <returns></returns>
        private int GetValueResponseXmlSevenValidarTipoRetorno(string xmlResponse, out string mensaje)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            int Valor = 0;
            string ValorTexto = "";
           
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                  
                    XmlNodeList elemlistText = xmlString.GetElementsByTagName("TXTERROR");
                    ValorTexto = Convert.ToString(elemlistText[0].InnerXml);
           
            mensaje = ValorTexto;
            return Valor;
        }
        public async Task<Response<PagosSevenDto>> PagosCesantiasSeven(ParametrosObtenerCesantiasDto solicitud, CancellationToken cancellationToken)

        {
            var responseTextMensaje = "";

            Response<PagosSevenDto> result = new Response<PagosSevenDto> { Data = new PagosSevenDto() };

            Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroSeven(solicitud, cancellationToken);

            if (responseTerceroSeven.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTerceroSeven.ReturnMessage };
            }
            else
            {
                   
                var jsonResponseTercero = ConvertXmlToJson(responseTerceroSeven.Data.InsertarGnTerceResult);
                result.Data.insertarGnTerceResponse = jsonResponseTercero;
                    
            }

            Response<CrearCDPResponse> crearCDPResponse = await CrearCDPSeven(solicitud, cancellationToken);

            if (crearCDPResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = crearCDPResponse.ReturnMessage };
            }
            else
            {
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(crearCDPResponse.Data.CrearCDPResult, out responseTextMensaje);
                var jsonResponseCDP = ConvertXmlToJson(crearCDPResponse.Data.CrearCDPResult);
                result.Data.crearCDPResponse = jsonResponseCDP;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            Response<SyncPoPvdorResponse> syncPoPvdorResponse = await CrearProveedorSeven(solicitud, cancellationToken);

            if (syncPoPvdorResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = syncPoPvdorResponse.ReturnMessage };
            }
            else
            {
                    
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(syncPoPvdorResponse.Data.SyncPoPvdorResult, out responseTextMensaje);
                var jsonResponseProveedor = ConvertXmlToJson(syncPoPvdorResponse.Data.SyncPoPvdorResult);
                result.Data.syncPoPvdorResponse = jsonResponseProveedor;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            Response<InsertarFacturaPResponse> insertarFacturaPResponse = await CrearFacturaProveSeven(solicitud, cancellationToken);

            if (insertarFacturaPResponse.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = insertarFacturaPResponse.ReturnMessage };
            }
            else
            {
                   
                var valorRetorno = GetValueResponseXmlSevenValidarTipoRetorno(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult, out responseTextMensaje);
                var jsonResponseFactura = ConvertXmlToJson(insertarFacturaPResponse.Data.Body.InsertarFacturaPResult);
                result.Data.insertarFacturaPResponse = jsonResponseFactura;

                if (valorRetorno == 1)
                {
                    return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTextMensaje };

                }
            }

            return result;

        }
        public async Task<Response<InsertarMovContableResponse>> MovContableCesantiasSeven(ParametrosMovContableDto solicitud, CancellationToken cancellationToken)

        {
            Response<InsertarMovContableResponse> response = new();
            InsertarMovContableRequest movContableRequest = new InsertarMovContableRequest();
            var dataMovContaCesantias = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaCesantias = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var dataMovContaAportes = new TSCnMcont();
            List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon> detalleMovContaAportes = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            var InfoFinancieraCliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.IdTipoIdentificacion, solicitud.Identificacion)).Data;
            var responseSolicitud = await this._solicitudRepository.ConsultaSolicitud(solicitud.solicitudId);
            var InteresAfiliados = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(1023);
            var InteresFondoCesantiasSolucionVivienda = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(1024);
            //var InteresAfiliadosAVAC = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(1029);
            //var InteresAfiliadosAVAV = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(1031);
            //var InteresAfiliadosAVC = await this._cesantiasRepository.ObtenerEquivalenciaCuentasContablesSevenByCodOperacion(1032);
            var interesPersona= await _verificacionDerechosBusinessLogic.ObtenerTodoInteresesNoCausadosByIdentificacion(int.Parse(solicitud.Identificacion));
            var cabeceraCesantias = 0;
            var cabeceraAportes = 0;
            var numeroSolicitud = responseSolicitud.SOL_NUMERO_SOLICITUD != null ? responseSolicitud.SOL_NUMERO_SOLICITUD : 0;
            if (interesPersona.Data.Count()>0)
            {
                foreach (var item in interesPersona.Data)
                {
                    if (item.TipoConcepto.Contains("Cesantias"))
                    {
                        if (cabeceraCesantias==0)
                        {
                            dataMovContaCesantias.Emp_codi = 294;
                            dataMovContaCesantias.Top_codi = 1024;
                            dataMovContaCesantias.Mco_nume = 0;
                            dataMovContaCesantias.Mco_fech = DateTime.Now;
                            dataMovContaCesantias.Mod_codi = 7;
                            dataMovContaCesantias.Arb_csuc = "1001";
                            dataMovContaCesantias.Mco_desc = item.TipoConcepto;
                            cabeceraCesantias++;
                        }
                       
                       
                        var items = ObtenerDeTalleInteresesCesantiasSeven(InteresFondoCesantiasSolucionVivienda, InfoFinancieraCliente, numeroSolicitud, item).Result;
                        detalleMovContaCesantias.AddRange(items);
                       
                    }
                    else
                    {
                        if (cabeceraAportes==0)
                        {
                            dataMovContaAportes.Emp_codi = 294;
                            dataMovContaAportes.Top_codi = 1023;
                            dataMovContaAportes.Mco_nume = 0;
                            dataMovContaAportes.Mco_fech = DateTime.Now;
                            dataMovContaAportes.Mod_codi = 7;
                            dataMovContaAportes.Arb_csuc = "1001";
                            dataMovContaAportes.Mco_desc = item.TipoConcepto;
                            cabeceraAportes++;
                        }
                      
                        var items = ObtenerDeTalleInteresesCesantiasSeven(InteresAfiliados, InfoFinancieraCliente, numeroSolicitud, item).Result;
                        detalleMovContaAportes.AddRange(items);
                        
                    }

                }

                if (detalleMovContaCesantias.Count > 0)
                {
                    dataMovContaCesantias.vDetalle = detalleMovContaCesantias.ToArray();

                    var bodyCesantias = new InsertarMovContableRequestBody(dataMovContaCesantias);
                    movContableRequest = new InsertarMovContableRequest(bodyCesantias);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    var mensaje = "\n Resultado Mov. Contable Seven intereses Cesantias: ";
                    response.ReturnMessage += response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven ";

                }
                if (detalleMovContaAportes.Count > 0)
                {
                    dataMovContaAportes.vDetalle = detalleMovContaAportes.ToArray();

                    var bodyAportes = new InsertarMovContableRequestBody(dataMovContaAportes);
                    movContableRequest = new InsertarMovContableRequest(bodyAportes);
                    response.Data = await _wsMovContable.InsertarMovContableAsync(movContableRequest, cancellationToken);
                    var mensaje = "\n Resultado Mov. Contable Seven intereses Aportes: ";
                    response.ReturnMessage += response.Data is not null ? mensaje + response.Data.Body.InsertarMovContableResult : "Error inesperado al crear el mov. contable en seven ";


                }

            }
            else
                response.ReturnMessage = "No hay intereses para crear un mov. contable en seven.";

            return response;
        }
        private async Task<List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>> ObtenerDeTalleInteresesCesantiasSeven(IEnumerable<EquivalenciaCuentasContablesSeven> cuentasContables, ClienteDto InfoFinancieraCliente, int numeroSolicitud, InteresesCausadosNoCausadosInfoDto interes)
        {
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == InfoFinancieraCliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            var items = new List<ReferenciasSvc.Seven.SCnMcont.TSCnDmcon>();
            foreach (var item in cuentasContables)
            {
                var itemDetalle = new ReferenciasSvc.Seven.SCnMcont.TSCnDmcon();

                itemDetalle.Cue_codi = item.CUENTA_CONTABLE_SEVEN;
                itemDetalle.Dmc_desc = $"cliente: " + InfoFinancieraCliente.Identificacion;
                itemDetalle.Dmc_cant = 1;
                itemDetalle.Dmc_acti = 0;
                itemDetalle.Dmc_refe = numeroSolicitud.ToString();
                itemDetalle.Dmc_vadb = item.TIPO_OPERACION == "D" ? interes.Valor : 0;
                itemDetalle.Dmc_vacr = item.TIPO_OPERACION == "C" ? interes.Valor : 0;
                itemDetalle.Dmc_vaba = 0;
                itemDetalle.Ter_coda = InfoFinancieraCliente.Identificacion;
                itemDetalle.Arb_codc = item.TCR_ID == 1 ? "601102" : "601103";
                itemDetalle.Arb_coda = codAreanegocio.ToString();
                itemDetalle.Arb_codp = "099";
                itemDetalle.Arb_cods = "1001";
                itemDetalle.Ter_codm = "0";

                items.Add(itemDetalle);
            }
            return items;
        }
        private string ConvertXmlToJson(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            string jsonResul = JsonConvert.SerializeXmlNode(doc);

            return jsonResul;
        }
    }
}
