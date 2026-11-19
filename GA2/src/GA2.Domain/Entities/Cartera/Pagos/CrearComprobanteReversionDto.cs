using System;
// ═════════════════════════════════════════════════════════════════════════════════
// DTO: Respuesta de Creación de Comprobante de Reversión
// ═════════════════════════════════════════════════════════════════════════════════
public class CrearComprobanteReversionDto
{
    /// <summary>CC_ID del comprobante original</summary>
    public int CC_ID_Original { get; set; }

    /// <summary>CC_ID del nuevo comprobante de reversión</summary>
    public int? CC_ID_Reversion { get; set; }

    /// <summary>GUID del comprobante de reversión (CPG_ID)</summary>
    public Guid Cpg_Id_Reversion { get; set; }

    /// <summary>Total de crédito (después de invertir)</summary>
    public decimal TotalCreditoInvertido { get; set; }

    /// <summary>Total de débito (después de invertir)</summary>
    public decimal TotalDebitoInvertido { get; set; }

    /// <summary>Cantidad de movimientos invertidos</summary>
    public int CantidadMovimientosInvertidos { get; set; }

    /// <summary>Fecha de creación de la reversión</summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>Estado del comprobante (CREADO, ENVIADO, ERROR)</summary>
    public string Estado { get; set; }

    /// <summary>Descripción del comprobante</summary>
    public string Descripcion { get; set; }
}
