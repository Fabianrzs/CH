using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes.MovimientoComprobante
{
    public class MovimientoComprobantePorProcesoDto
    {
        public int Movimiento_Comprobante_CarteraId { get; set; }
        public int Comprobante_CarteraId { get; set; }
        public string Cuenta_Contable { get; set; }
        public string Descripcion_Cuenta { get; set; }
        public string Numero_Identificacion { get; set; }
        public string Nombre_Tercero { get; set; }
        public string Tipo_Credito { get; set; }
        public string Centro_Costo { get; set; }
        public string Centro_Costo_Credito { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public string Detalle { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public int Total_Registros { get; set; }
        public string TIPO_OPERACION { get; set; }
        public string Numero_Credito { get; set; }
        public int? Comprobante_Interno { get; set; }
        public int? Comprobante_Contable { get; set; }
        public string? Tipo_Comprobante { get; set; }
        public string Cuenta_NIF { get; set; }

    }
    public class ObtenerComprobanteTotalizadoPorCuentaDto
    {
        public string Cuenta_Contable { get; set; }
        public string Descripcion_Cuenta { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public string Cuentas_NIF { get; set; }

    }    
}

