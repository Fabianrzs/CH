using GA2.Application.Dto.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;
using GA2.Transversals.Commons;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Credito.Seven
{
    public interface ISevenBusinessLogic
    {
        //Movimiento contable

        #region MovimientoContable
        Task<Response<AnularCnMcontResponse>> AnularCnMcontAsync(AnularCnMcontDto request, CancellationToken cancellationToken);
        Task<Response<AplicarCnMcontResponse>> AplicarCnMcontAsync(AplicarCnMcontDto request, CancellationToken cancellationToken);
        Task<Response<InsertarCnDmconResponse>> InsertarCnDmconAsync(InsertarCnDmconDto request, CancellationToken cancellationToken);
        Task<Response<InsertarCnMcontResponse>> InsertarCnMcontAsync(InsertarCnMcontDto request, CancellationToken cancellationToken);
        Task<Response<InsertarMovContableResponse>> InsertarMovContableAsync(InsertarMovContableDto request, CancellationToken cancellationToken);
        Task<Response<RevertirCnMcontResponse>> RevertirCnMcontAsync(RevertirCnMcontDto request, CancellationToken cancellationToken);
        Task<Response<SetContabToperResponse>> SetContabToperAsync(SetContabToperRequest request, CancellationToken cancellationToken);
        #endregion MovimientoContable

        #region Aportes
        // Aportes
        Task<Response<GetAporteResponse>> GetAporteAsync(ValidarAportesDto request, CancellationToken cancellationToken);
        #endregion Aportes

        #region CDP

        // CDP
        Task<Response<AnularCDPResponse>> AnularCDPAsync(AnularCDPDto request, CancellationToken cancellationToken);
        Task<Response<AplicarCDPResponse>> AplicarCDPAsync(AplicarCDPDto request, CancellationToken cancellationToken);
        Task<Response<CrearCDPResponse>> CrearCDPAsync(CrearCDPDto request, CancellationToken cancellationToken);
        Task<Response<InsertarCDPResponse>> InsertarCDPAsync(InsertarCDPDto request, CancellationToken cancellationToken);
        Task<Response<InsertarDetCDPResponse>> InsertarDetCDPAsync(InsertarDetCDPDto request, CancellationToken cancellationToken);
        #endregion CDP

        #region Factura
        //Factura 
        Task<Response<AnularFacturaResponse>> AnularFacturaAsync(AnularFacturaDto request, CancellationToken cancellationToken);
        Task<Response<AnularPoFactuResponse>> AnularPoFactuAsync(AnularPoFactuDto request, CancellationToken cancellationToken);
        Task<Response<InsertarFacturaPResponse>> InsertarFacturaPAsync(InsertarFacturaPDto request, CancellationToken cancellationToken);
        Task<Response<InsertarPoFactuResponse>> InsertarPoFactuAsync(InsertarPoFactuDto request, CancellationToken cancellationToken);
        Task<Response<InsertarPoFactuPResponse>> InsertarPoFactuPAsync(InsertarPoFactuPDto request, CancellationToken cancellationToken);
        #endregion Factura

        #region Proveedor
        // WSProveedor
        Task<Response<ActModalidadCupoResponse>> ActModalidadCupoAsync(ActModalidadCupoDto request, CancellationToken cancellationToken);
        Task<Response<ChangeNitResponse>> InsertarCnMcontAsync(ChangeNitDto request, CancellationToken cancellationToken);
        Task<Response<SyncEmpleadoPvdorResponse>> SyncEmpleadoPvdorAsync(SyncEmpleadoPvdorDto request, CancellationToken cancellationToken);
        Task<Response<SyncFaClienResponse>> SyncFaClienAsync(SyncFaClienDto request, CancellationToken cancellationToken);
        Task<Response<SyncFaClienComplexResponse>> SyncFaClienComplexAsync(SyncFaClienComplexDto request, CancellationToken cancellationToken);
        Task<Response<SyncFaContaResponse>> SyncFaContaAsync(SyncFaContaDto request, CancellationToken cancellationToken);
        Task<Response<SyncGnTerceResponse>> SyncGnTerceAsync(SyncGnTerceRequest request, CancellationToken cancellationToken);
        Task<Response<SyncPoPvdorResponse>> SyncPoPvdorAsync(SyncPoPvdorRequest request, CancellationToken cancellationToken);
        #endregion Proveedor

        #region Tercero
        //WSTercero
        Task<Response<InsertarGnTerceResponse>> InsertarGnTerceAsync(InsertarTerceroDto request, CancellationToken cancellationToken);
        #endregion Tercero

        #region Winitpro

        //WSWinipro
        Task<Response<GetCuentaContableResponse>> GetCuentaContableAsync(CuentaContableDto request, CancellationToken cancellationToken);
        Task<Response<GetCuentaContableTOResponse>> GetCuentaContableTOAsync(CuentaContableToDto request, CancellationToken cancellationToken);
        #endregion Winitpro
    }
}
