using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Seven;
using GA2.Application.Dto.InfoVendedor;
using GA2.Application.Dto.Seven;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Embargos.Seven;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GA2.Domain.Core.Embargos.Seven
{
    public class PagosEmbargosSeven : IPagosEmbargosSeven
    {

        private readonly IConsumoWSTercero _wsTercero;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IMapper _mapper;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly IEmbargosBusinessLogic _embargosBusinessLogic;
        private readonly ICatalogosRepository _catalogsRepository;
        private readonly IConsumoWSProveedor _wsProveedor;
        private readonly IConsumoCDP _wsCDP;
        private readonly IConsumoWSFactura _wsFactura;

        public PagosEmbargosSeven(
            IConsumoWSTercero wsTercero, 
            IClientRequestProvider iClientRequestProvider, 
            IMapper mapper,
            IEmbargosRepository juzgadosRepository,
            IEmbargosBusinessLogic embargosBusinessLogic,
            ICatalogosRepository catalogosRepository,
            IConsumoWSProveedor wsProveedor,
            IConsumoCDP wsCDP,
            IConsumoWSFactura wsFactura)
        {
            _wsTercero = wsTercero;
            _iClientRequestProvider = iClientRequestProvider;
            _mapper = mapper;
            _juzgadosRepository = juzgadosRepository;
            _embargosBusinessLogic = embargosBusinessLogic;
            _catalogsRepository = catalogosRepository;
            _wsProveedor = wsProveedor; 
            _wsCDP = wsCDP;
            _wsFactura = wsFactura;
        }


        /// <summary>
        /// Paso 1 crear tercero Seven - Embargo.
        /// </summary>
        /// <param name="solicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<InsertarGnTerceResponse>> InsertTerceroEmbargoSeven(PagosEmbargosSevenParam embargosSevenParam, CancellationToken cancellationToken)
        {

            EmbargosDto embargo = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ObtenerEmbargoPorId(embargosSevenParam.EBA_ID));

            Response<InsertarGnTerceResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(embargo.TipoIdentificacionId, embargo.IdentificacionCliente)).Data;

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

        public async Task<Response<SyncPoPvdorResponse>> CrearProveedorEmbargoSeven(PagosEmbargosSevenParam embargosSevenParam, CancellationToken cancellationToken)
        {
            Response<SyncPoPvdorResponse> response = new();

            Response<EmbargoByDto> datosEmbargos = await _embargosBusinessLogic.ObtenerEmbargoBy(embargosSevenParam.EBA_ID);


            List<BeneficiariosPagoEmbargoDto> Beneficiarios = datosEmbargos.Data.Beneficiarios;

            BeneficiariosPagoEmbargoDto beneficiariosPagoEmbargo = new BeneficiariosPagoEmbargoDto(); 
            foreach (var item in Beneficiarios)
            {
                if (item.BPE_BENEFICIARIO==true)
                {
                    beneficiariosPagoEmbargo.TipoDocumentoId = item.TipoDocumentoId;
                    beneficiariosPagoEmbargo.NumeroIdentificacion = item.NumeroIdentificacion; 
                    beneficiariosPagoEmbargo.PrimerNombre = item.PrimerNombre;
                    beneficiariosPagoEmbargo.SegundoNombre = item.SegundoNombre;
                    beneficiariosPagoEmbargo.PrimerApellido = item.PrimerApellido;
                    beneficiariosPagoEmbargo.SegundoApellido = item.SegundoApellido; 
                }
            }



            //var banco = (await this._catalogsRepository.ObtenerEntidadBancaria()).Where(x => x.ENT_ID == datosEmbargos.Data.Beneficiarios).FirstOrDefault();
            var ProveeRequest = new SyncPoPvdorRequest()
            {
                emp_codi = 294,
                tip_codi = beneficiariosPagoEmbargo.TipoDocumentoId,
                pvd_coda = beneficiariosPagoEmbargo.NumeroIdentificacion,
                pvd_dive = 0,
                pvd_nomb = $"{beneficiariosPagoEmbargo.PrimerNombre} {beneficiariosPagoEmbargo.SegundoNombre} {beneficiariosPagoEmbargo.PrimerApellido} {beneficiariosPagoEmbargo.SegundoApellido}",
                pvd_apel = "",
                pvd_noco = $"{beneficiariosPagoEmbargo.PrimerNombre} {beneficiariosPagoEmbargo.SegundoNombre} {beneficiariosPagoEmbargo.PrimerApellido} {beneficiariosPagoEmbargo.SegundoApellido}",
                pai_codi = 169, //COLOMBIA,
                dep_codi = 0, //DEPARTAMENTO "BOGOTA",
                mun_codi = 0, //CIUDAD "BOGOTA"
                tpr_codi = 0,
                pvd_clad = "J", // persona juridica y natural: "N",
                arb_csuc = "1001",
                cal_codi = 0,
                coc_codi = 1,
                cim_codi = 98,
                act_codi = 0,
                mon_codi = 1,
                dep_dire = "",//Por el momento no hay dirección,
                cup_nume = "",//numero cuenta benefiario,
                cup_tipo = "",//tipo de cuenta benefiario "A" : "C",
                ban_codi = 0, //banco benefiario,
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

                PagosEmbargosSevenRespuesta pagosEmbargosSevenRespuesta = new PagosEmbargosSevenRespuesta()
                {
                    EBA_ID = embargosSevenParam.EBA_ID, 
                    PEM_CODIGO_PROVEEDOR = codProvee, 
                    PEM_FLUJO = 1
                }; 

                await _juzgadosRepository.InsertarPagosEmbargosSeven(pagosEmbargosSevenRespuesta);
            }

            return response;
        }

        public async Task<Response<CrearCDPResponse>> CrearCDPSevenEmbargo(PagosEmbargosSevenParam embargosSevenParam, CancellationToken cancellationToken)
        {

            Response<EmbargoByDto> datosEmbargos = await _embargosBusinessLogic.ObtenerEmbargoBy(embargosSevenParam.EBA_ID);

            Response<CrearCDPResponse> response = new();
            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(datosEmbargos.Data.Embargo.TipoIdentificacionId, datosEmbargos.Data.Embargo.IdentificacionCliente)).Data;

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }
          
           
            GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre[] tSPgDmpres = ObtenerDeTalleCDPEmbargo(cliente, datosEmbargos).Result.vDetalle;

            var request = new CrearCDPDto
            {
                Emp_codi = 294,
                Top_codi = 3015,
                Mpr_nume = 0,
                Mpr_fech = DateTime.Now,
                Mpr_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                $"{datosEmbargos.Data.Embargo.RadicadoWorkManager} - {datosEmbargos.Data.Embargo.NumeroSolicitud}",
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


                PagosEmbargosSevenRespuesta pagosEmbargosSevenRespuesta = new PagosEmbargosSevenRespuesta()
                {
                    EBA_ID = embargosSevenParam.EBA_ID,
                    PEM_CODIGO_PROVEEDOR = MPR_CONT,
                    PEM_FLUJO = 2
                };

                await _juzgadosRepository.InsertarPagosEmbargosSeven(pagosEmbargosSevenRespuesta);
            }

          

            return response;
        }
        private async Task<TSPgMpres> ObtenerDeTalleCDPEmbargo(ClienteDto cliente, Response<EmbargoByDto> datosEmbargos)
        {
            TSPgMpres detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre>();
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            int codAreanegocio = unidadesEjecutoras.Where(x => x.Id == cliente.IdUnidadEjecutora).Select(x => x.AreaNegocio).FirstOrDefault();

            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP.TSPgDmpre
            {

                Pro_codi = "0",//tipo de embargo ej: credito hipotecario 1 o leasgin 2 
                Arb_codc = "00000",// este codigo depende del tipo de credito "601102" : "601103",
                Arb_coda = "3011",//codAreanegocio.ToString(),
                Arb_codp = "099",
                Dmp_valo = (double)datosEmbargos.Data.Conceptos.Select(x => x.Valor).FirstOrDefault(),
                Dmp_desc = $"{cliente.Identificacion} - {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido} - " +
                $"{datosEmbargos.Data.Embargo.RadicadoWorkManager} - {datosEmbargos.Data.Embargo.NumeroSolicitud}"
            };

            items.Add(itemDetalle);
            detalle.vDetalle = items.ToArray();

            return detalle;
        }

        public async Task<Response<InsertarFacturaPResponse>> CrearFacturaEmbargosSeven(PagosEmbargosSevenParam embargosSevenParam, CancellationToken cancellationToken)
        {

            Response<EmbargoByDto> datosEmbargos = await _embargosBusinessLogic.ObtenerEmbargoBy(embargosSevenParam.EBA_ID);

            var cliente = (await _iClientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(datosEmbargos.Data.Embargo.TipoIdentificacionId, datosEmbargos.Data.Embargo.IdentificacionCliente)).Data;

            Response<InsertarFacturaPResponse> response = new();

            if (cliente == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage = "El afiliado no existe en BUA";
                return response;
            }

            List<BeneficiariosPagoEmbargoDto> Beneficiarios = datosEmbargos.Data.Beneficiarios;

            BeneficiariosPagoEmbargoDto beneficiariosPagoEmbargo = new BeneficiariosPagoEmbargoDto();
            foreach (var item in Beneficiarios)
            {
                if (item.BPE_BENEFICIARIO == true)
                {
                    beneficiariosPagoEmbargo.TipoDocumentoId = item.TipoDocumentoId;
                    beneficiariosPagoEmbargo.NumeroIdentificacion = item.NumeroIdentificacion;
                    beneficiariosPagoEmbargo.PrimerNombre = item.PrimerNombre;
                    beneficiariosPagoEmbargo.SegundoNombre = item.SegundoNombre;
                    beneficiariosPagoEmbargo.PrimerApellido = item.PrimerApellido;
                    beneficiariosPagoEmbargo.SegundoApellido = item.SegundoApellido;
                }
            }

            PagosEmbargosSevenRespuesta datosSevenEmbargo = await _juzgadosRepository.ConsultarDatosEmbargosPagosSeven(embargosSevenParam.EBA_ID);


            GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact[] detalleFactuProve = ObtenerDeTalleFacturaEmbargo(datosEmbargos).Result.vDetalle;

            if (detalleFactuProve == null)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.ReturnMessage = "El cliente no cuenta con los recursos para efectuar el primer pago";
                return response;
            }

            var _dataFactuProv = new TSPoFactu();

            _dataFactuProv.Emp_codi = 294;
            _dataFactuProv.Top_codi = 1; //datosSolicitud.TCR_ID == 2 ? 4137 : 4133; no sabemos que codigo va para credito
            _dataFactuProv.Fac_nume = 0;//Esta es para la factura de primer pago y acá no tenemos eso
            _dataFactuProv.Fac_fech = DateTime.Now;
            _dataFactuProv.Arb_cods = "1001";
            _dataFactuProv.Fac_desc = $"{beneficiariosPagoEmbargo.PrimerNombre} - {beneficiariosPagoEmbargo.SegundoNombre} - {beneficiariosPagoEmbargo.PrimerApellido} - {beneficiariosPagoEmbargo.SegundoApellido} - {datosEmbargos.Data.Embargo.RadicadoWorkManager} - {datosEmbargos.Data.Embargo.NumeroSolicitud}";
            _dataFactuProv.Pvd_coda = beneficiariosPagoEmbargo.NumeroIdentificacion;
            _dataFactuProv.Dep_codd = datosSevenEmbargo.PEM_CODIGO_PROVEEDOR; //codigo proveedor
            _dataFactuProv.Fac_pref = datosEmbargos.Data.Embargo.NumeroSolicitud.ToString();
            _dataFactuProv.Fac_nfap = "0";//Esta es para la factura de primer pago y acá no tenemos eso
            _dataFactuProv.Mon_codi = 1;
            _dataFactuProv.Mpr_cont = datosSevenEmbargo.PEM_CDP; // CDP
            _dataFactuProv.vDetalle = detalleFactuProve;

            var body = new InsertarFacturaPRequestBody(_dataFactuProv);
            var facturarequest = new InsertarFacturaPRequest(body);

            response.Data = await _wsFactura.InsertarFacturaPAsync(facturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "Error inesperado al crear la factura en Seven.";
            if (response.Data != null)
            {
                int FAC_CONT = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 0);
                int FAC_NUME = GetValueResponseXmlSevenFactura(response.Data.Body.InsertarFacturaPResult, 1);

                PagosEmbargosSevenRespuesta pagosEmbargosSevenRespuesta = new PagosEmbargosSevenRespuesta()
                {
                    EBA_ID = embargosSevenParam.EBA_ID,
                    PEM_NUMERO_FACTURA = FAC_NUME,
                    PEM_FLUJO = 3
                };

                await _juzgadosRepository.InsertarPagosEmbargosSeven(pagosEmbargosSevenRespuesta);
            }

            return response;
        }
        private async Task<TSPoFactu> ObtenerDeTalleFacturaEmbargo(Response<EmbargoByDto> datosEmbargos)
        {
            TSPoFactu detalle = new();
            var items = new List<GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact>();

            var itemDetalle = new GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura.TSPoDfact
            {
                Pro_codi = "0", //solicitud.TCR_ID == 2 ? "40401" : "40301", no sabemos que mandar en embargo en este campo
                Dfa_dest = "200",
                Dfa_valo = (double)datosEmbargos.Data.Conceptos.Select(x => x.Valor).FirstOrDefault(),
                Arb_codc = "0", //solicitud.TCR_ID == 2 ? "601102" : "601103", no sabemos que mandar en embargo en este campo
                Arb_coda = "3011", // Caja honor
                Arb_codp = "099"
            };

            items.Add(itemDetalle);
            detalle.vDetalle = items.ToArray();

            return detalle;
        }

        public async Task<Response<PagosSevenDto>> PagosEmbargosPresupuestalSeven(PagosEmbargosSevenParam solicitud, CancellationToken cancellationToken)

        {
            var responseTextMensaje = "";

            Response<PagosSevenDto> result = new Response<PagosSevenDto> { Data = new PagosSevenDto() };

            Response<InsertarGnTerceResponse> responseTerceroSeven = await this.InsertTerceroEmbargoSeven(solicitud, cancellationToken);

            if (responseTerceroSeven.Data == null)
            {
                return new Response<PagosSevenDto> { Data = null, IsSuccess = false, ReturnMessage = responseTerceroSeven.ReturnMessage };
            }
            else
            {

                var jsonResponseTercero = ConvertXmlToJson(responseTerceroSeven.Data.InsertarGnTerceResult);
                result.Data.insertarGnTerceResponse = jsonResponseTercero;

            }

            Response<CrearCDPResponse> crearCDPResponse = await CrearCDPSevenEmbargo(solicitud, cancellationToken);

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

            Response<SyncPoPvdorResponse> syncPoPvdorResponse = await CrearProveedorEmbargoSeven(solicitud, cancellationToken);

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

            Response<InsertarFacturaPResponse> insertarFacturaPResponse = await CrearFacturaEmbargosSeven(solicitud, cancellationToken);

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

        private string ConvertXmlToJson(string xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            string jsonResul = JsonConvert.SerializeXmlNode(doc);

            return jsonResul;
        }

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


        private Int32 GetValueResponseXmlSeven(string xmlResponse, int sevenOpc)
        {
            XmlDocument xmlString = new XmlDocument();
            xmlString.LoadXml(xmlResponse);
            Int32 Valor = 0;
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
                case 2:
                    XmlNodeList elemlistRetorno = xmlString.GetElementsByTagName("RETORNO");
                    Valor = Convert.ToInt32(elemlistRetorno[0].InnerXml);
                    break;
                default:
                    break;
            }

            return Valor;
        }
    }
}
