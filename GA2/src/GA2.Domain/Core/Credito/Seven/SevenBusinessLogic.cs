using AutoMapper;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;
using GA2.Transversals.Commons;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Credito.Seven
{
    public class SevenBusinessLogic : ISevenBusinessLogic
    {
        private readonly IConsumoSCnMcont _sCnMcont;
        private readonly IConsumoWSAportes _wsAportes;
        private readonly IConsumoCDP _cdp;
        private readonly IConsumoWSFactura _wsFactura;
        private readonly IConsumoWSProveedor _wsProveedor;
        private readonly IConsumoWSTercero _wsTercero;
        private readonly IConsumoWSWintipro _wsIntipro;
        private readonly IMapper _mapper;

        public SevenBusinessLogic(IConsumoSCnMcont sCnMcont,
                                  IConsumoWSAportes wsAportes,
                                  IConsumoCDP cdp,
                                  IConsumoWSFactura wsFactura,
                                  IConsumoWSProveedor wsProveedor,
                                  IConsumoWSTercero wsTercero,
                                  IConsumoWSWintipro wsIntipro,
                                  IMapper mapper)
        {
            _sCnMcont = sCnMcont ?? throw new ArgumentNullException(nameof(sCnMcont));
            _wsAportes = wsAportes ?? throw new ArgumentNullException(nameof(wsAportes));
            _cdp = cdp ?? throw new ArgumentNullException(nameof(cdp));
            _wsFactura = wsFactura ?? throw new ArgumentNullException(nameof(wsFactura));
            _wsProveedor = wsProveedor ?? throw new ArgumentNullException(nameof(wsProveedor));
            _wsTercero = wsTercero ?? throw new ArgumentNullException(nameof(wsTercero));
            _wsIntipro = wsIntipro ?? throw new ArgumentNullException(nameof(wsIntipro));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Response<ActModalidadCupoResponse>> ActModalidadCupoAsync(ActModalidadCupoDto request, CancellationToken cancellationToken)
        {
            Response<ActModalidadCupoResponse> response = new();
            var actModalrequest = _mapper.Map<ActModalidadCupoRequest>(request);
            response.Data = await _wsProveedor.ActModalidadCupoAsync(actModalrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.ActModalidadCupoResult : "";
            return response;
        }
        public async Task<Response<AnularCDPResponse>> AnularCDPAsync(AnularCDPDto request, CancellationToken cancellationToken)
        {
            Response<AnularCDPResponse> response = new();
            var cdprequest = _mapper.Map<AnularCDPRequest>(request);
            response.Data = await _cdp.AnularCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data != null ? ConstantesSeven.CdpAnuladoExitoso : ConstantesSeven.CdpAnuladoFallido;
            return response;
        }
        public async Task<Response<AnularCnMcontResponse>> AnularCnMcontAsync(AnularCnMcontDto request, CancellationToken cancellationToken)
        {
            Response<AnularCnMcontResponse> response = new();
            var cdprequest = _mapper.Map<AnularCnMcontRequest>(request);
            response.Data = await _sCnMcont.AnularCnMcontAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.AnularCnMcontResult : "";
            return response;
        }
        public async Task<Response<AnularFacturaResponse>> AnularFacturaAsync(AnularFacturaDto request, CancellationToken cancellationToken)
        {
            Response<AnularFacturaResponse> response = new();
            var afacturarequest = _mapper.Map<AnularFacturaRequest>(request);
            response.Data = await _wsFactura.AnularFacturaAsync(afacturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.AnularFacturaResult : "";
            return response;
        }
        public async Task<Response<AnularPoFactuResponse>> AnularPoFactuAsync(AnularPoFactuDto request, CancellationToken cancellationToken)
        {
            Response<AnularPoFactuResponse> response = new();
            var aPoFacturarequest = _mapper.Map<AnularPoFactuRequest>(request);
            response.Data = await _wsFactura.AnularPoFactuAsync(aPoFacturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.AnularPoFactuResult : "";
            return response;
        }
        public async Task<Response<AplicarCDPResponse>> AplicarCDPAsync(AplicarCDPDto request, CancellationToken cancellationToken)
        {
            Response<AplicarCDPResponse> response = new();
            var cdprequest = _mapper.Map<AplicarCDPRequest>(request);
            response.Data = await _cdp.AplicarCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.AplicarCDPResult : "";
            return response;
        }
        public async Task<Response<AplicarCnMcontResponse>> AplicarCnMcontAsync(AplicarCnMcontDto request, CancellationToken cancellationToken)
        {
            Response<AplicarCnMcontResponse> response = new();
            var cnMcontrequest = _mapper.Map<AplicarCnMcontRequest>(request);
            response.Data = await _sCnMcont.AplicarCnMcontAsync(cnMcontrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.AplicarCnMcontResult : "";
            return response;
        }
        public async Task<Response<CrearCDPResponse>> CrearCDPAsync(CrearCDPDto request, CancellationToken cancellationToken)
        {
            Response<CrearCDPResponse> response = new();
            var cdprequest = _mapper.Map<CrearCDPRequest>(request);
            response.Data = await _cdp.CrearCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.CrearCDPResult : "";
            return response;
        }
        public async Task<Response<GetAporteResponse>> GetAporteAsync(ValidarAportesDto request, CancellationToken cancellationToken)
        {
            Response<GetAporteResponse> response = new();
            var aportesrequest = _mapper.Map<GetAporteRequest>(request);
            response.Data = await _wsAportes.GetAporteAsync(aportesrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.GetAporteResult : "";
            return response;
        }
        public async Task<Response<GetCuentaContableResponse>> GetCuentaContableAsync(CuentaContableDto request, CancellationToken cancellationToken)
        {
            Response<GetCuentaContableResponse> response = new();
            var ccontrequest = _mapper.Map<GetCuentaContableRequest>(request);
            response.Data = await _wsIntipro.GetCuentaContableAsync(ccontrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.GetCuentaContableResult : "";
            return response;
        }
        public async Task<Response<GetCuentaContableTOResponse>> GetCuentaContableTOAsync(CuentaContableToDto request, CancellationToken cancellationToken)
        {
            Response<GetCuentaContableTOResponse> response = new();
            var ccontTrequest = _mapper.Map<GetCuentaContableTORequest>(request); 
            response.Data = await _wsIntipro.GetCuentaContableTOAsync(ccontTrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.GetCuentaContableTOResult : "";
            return response;
        }
        public async Task<Response<InsertarCDPResponse>> InsertarCDPAsync(InsertarCDPDto request, CancellationToken cancellationToken)
        {
            Response<InsertarCDPResponse> response = new();
            var cdprequest = _mapper.Map<InsertarCDPRequest>(request);
            response.Data = await _cdp.InsertarCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.InsertarCDPResult : "";
            return response;
        }
        public async Task<Response<InsertarCnDmconResponse>> InsertarCnDmconAsync(InsertarCnDmconDto request, CancellationToken cancellationToken)
        {
            Response<InsertarCnDmconResponse> response = new();
            var cdprequest = _mapper.Map<InsertarCnDmconRequest>(request);
            response.Data = await _sCnMcont.InsertarCnDmconAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarCnDmconResult : "";
            return response;
        }
        public async Task<Response<InsertarCnMcontResponse>> InsertarCnMcontAsync(InsertarCnMcontDto request, CancellationToken cancellationToken)
        {
            Response<InsertarCnMcontResponse> response = new();
            var cdprequest = _mapper.Map<InsertarCnMcontRequest>(request);
            response.Data = await _sCnMcont.InsertarCnMcontAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarCnMcontResult : "";
            return response;
        }
        public async Task<Response<ChangeNitResponse>> InsertarCnMcontAsync(ChangeNitDto request, CancellationToken cancellationToken)
        {
            Response<ChangeNitResponse> response = new();
            var nitrequest = _mapper.Map<ChangeNitRequest>(request);
            response.Data = await _wsProveedor.InsertarCnMcontAsync(nitrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.ChangeNitResult : "";
            return response;
        }
        public async Task<Response<InsertarDetCDPResponse>> InsertarDetCDPAsync(InsertarDetCDPDto request, CancellationToken cancellationToken)
        {
            Response<InsertarDetCDPResponse> response = new();
            var cdprequest = _mapper.Map<InsertarDetCDPRequest>(request);
            response.Data = await _cdp.InsertarDetCDPAsync(cdprequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.InsertarDetCDPResult : "";
            return response;
        }
        public async Task<Response<InsertarFacturaPResponse>> InsertarFacturaPAsync(InsertarFacturaPDto request, CancellationToken cancellationToken)
        {
            Response<InsertarFacturaPResponse> response = new();
            var ifacturarequest = _mapper.Map<InsertarFacturaPRequest>(request);
            response.Data = await _wsFactura.InsertarFacturaPAsync(ifacturarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarFacturaPResult : "";
            return response;
        }
        public async Task<Response<InsertarGnTerceResponse>> InsertarGnTerceAsync(InsertarTerceroDto request, CancellationToken cancellationToken)
        {
            Response<InsertarGnTerceResponse> response = new();
            var tercrequest = _mapper.Map<InsertarGnTerceRequest>(request);
            response.Data = await _wsTercero.InsertarGnTerceAsync(tercrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.InsertarGnTerceResult : "";
            return response;
        }
        public async Task<Response<InsertarMovContableResponse>> InsertarMovContableAsync(InsertarMovContableDto request, CancellationToken cancellationToken)
        {
            Response<InsertarMovContableResponse> response = new();
            var iMovContrequest = _mapper.Map<InsertarMovContableRequest>(request);
            response.Data = await _sCnMcont.InsertarMovContableAsync(iMovContrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarMovContableResult : "";
            return response;
        }
        public async Task<Response<InsertarPoFactuResponse>> InsertarPoFactuAsync(InsertarPoFactuDto request, CancellationToken cancellationToken)
        {
            Response<InsertarPoFactuResponse> response = new();
            var iPoFactrequest = _mapper.Map<InsertarPoFactuRequest>(request);
            response.Data = await _wsFactura.InsertarPoFactuAsync(iPoFactrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarPoFactuResult : "";
            return response;
        }
        public async Task<Response<InsertarPoFactuPResponse>> InsertarPoFactuPAsync(InsertarPoFactuPDto request, CancellationToken cancellationToken)
        {
            Response<InsertarPoFactuPResponse> response = new();
            var iPoFactPrequest = _mapper.Map<InsertarPoFactuPRequest>(request);
            response.Data = await _wsFactura.InsertarPoFactuPAsync(iPoFactPrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.InsertarPoFactuPResult : "";
            return response;
        }
        public async Task<Response<RevertirCnMcontResponse>> RevertirCnMcontAsync(RevertirCnMcontDto request, CancellationToken cancellationToken)
        {
            Response<RevertirCnMcontResponse> response = new();
            var revertrequest = _mapper.Map<RevertirCnMcontRequest>(request);
            response.Data = await _sCnMcont.RevertirCnMcontAsync(revertrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.RevertirCnMcontResult : "";
            return response;
        }
        public async Task<Response<SetContabToperResponse>> SetContabToperAsync(SetContabToperRequest request, CancellationToken cancellationToken)
        {
            Response<SetContabToperResponse> response = new();
            response.Data = await _sCnMcont.SetContabToperAsync(request, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.Body.SetContabToperResult.FirstOrDefault().Retorno : "";
            return response;
        }
        public async Task<Response<SyncEmpleadoPvdorResponse>> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorDto request, CancellationToken cancellationToken)
        {
            Response<SyncEmpleadoPvdorResponse> response = new();
            var empleadorequest = _mapper.Map<SyncEmpleadoPvdorRequest>(request);
            response.Data = await _wsProveedor.SyncEmpleadoPvdorAsync(empleadorequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncEmpleadoPvdorResult : "";
            return response;
        }
        public async Task<Response<SyncFaClienResponse>> SyncFaClienAsync(SyncFaClienDto request, CancellationToken cancellationToken)
        {
            Response<SyncFaClienResponse> response = new();
            var syncFaClienrequest = _mapper.Map<SyncFaClienRequest>(request);
            response.Data = await _wsProveedor.SyncFaClienAsync(syncFaClienrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncFaClienResult : "";
            return response;
        }
        public async Task<Response<SyncFaClienComplexResponse>> SyncFaClienComplexAsync(SyncFaClienComplexDto request, CancellationToken cancellationToken)
        {
            Response<SyncFaClienComplexResponse> response = new();
            var syncFaClienComplexrequest = _mapper.Map<SyncFaClienComplexRequest>(request); 
            response.Data = await _wsProveedor.SyncFaClienComplexAsync(syncFaClienComplexrequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncFaClienComplexResult.CliCodi.ToString() : response.Data.SyncFaClienComplexResult.TxtError;
            return response;
        }
        public async Task<Response<SyncFaContaResponse>> SyncFaContaAsync(SyncFaContaDto request, CancellationToken cancellationToken)
        {
            Response< SyncFaContaResponse > response = new();
            var syncFaContarequest = _mapper.Map<SyncFaContaRequest>(request); 
            response.Data = await _wsProveedor.SyncFaContaAsync(syncFaContarequest, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncFaContaResult : "";
            return response;
        }
        public async Task<Response<SyncGnTerceResponse>> SyncGnTerceAsync(SyncGnTerceRequest request, CancellationToken cancellationToken)
        {
            Response<SyncGnTerceResponse> response = new();
            response.Data = await _wsProveedor.SyncGnTerceAsync(request, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncGnTerceResult : "";
            return response;
        }
        public async Task<Response<SyncPoPvdorResponse>> SyncPoPvdorAsync(SyncPoPvdorRequest request, CancellationToken cancellationToken)
        {
            Response<SyncPoPvdorResponse> response = new();
            response.Data = await _wsProveedor.SyncPoPvdorAsync(request, cancellationToken);
            response.ReturnMessage = response.Data is not null ? response.Data.SyncPoPvdorResult : "";
            return response;
        }
   }
}