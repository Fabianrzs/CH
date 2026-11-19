using System;


namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// MODEL: Comprobante de Cartera
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class ComprobanteCarteraModel
    {
        /// <summary>Identificador único del comprobante</summary>
        public int CC_ID { get; set; }

        /// <summary>ID del comprobante en Seven</summary>
        public string CC_COMPROBANTE_SEVEN_ID { get; set; }

        /// <summary>Fecha del comprobante</summary>
        public DateTime CC_FECHA_COMPROBANTE { get; set; }

        /// <summary>Tipo de comprobante (21=Normal, 22=Reversión)</summary>
        public int TCC_TIPO_COMPROBANTE_ID { get; set; }

        /// <summary>Departamento</summary>
        public int DPD_DEPARTAMENTO_ID { get; set; }

        /// <summary>Ciudad</summary>
        public int DPC_CIUDAD_ID { get; set; }

        /// <summary>Tipo de concepto</summary>
        public int TCC_TIPO_CONCEPTO_ID { get; set; }

        /// <summary>Tipo de beneficiario</summary>
        public int TBC_TIPO_BENEFICIARIO_ID { get; set; }

        /// <summary>Tipo de identificación</summary>
        public int TIPO_IDENTIFICACION { get; set; }

        /// <summary>Identificación del cliente</summary>
        public string CLI_IDENTIFICACION { get; set; }

        /// <summary>Fecha de aprobación</summary>
        public DateTime? CC_FECHA_APROBACION { get; set; }

        /// <summary>Usuario que aprobó</summary>
        public Guid? CC_APROBADO_POR { get; set; }

        /// <summary>Flag de aprobación</summary>
        public bool CC_APROBADO { get; set; }

        /// <summary>Total crédito del comprobante</summary>
        public decimal TOTAL_COMPROMBANTE_CREDITO { get; set; }

        /// <summary>Total débito del comprobante</summary>
        public decimal TOTAL_COMPROMBANTE_DEBITO { get; set; }

        /// <summary>Estado del comprobante (1=Activo, 0=Inactivo)</summary>
        public int ESTADO_ID { get; set; }

        /// <summary>Fecha de creación</summary>
        public DateTime CRE_FECHA_CREACION { get; set; }

        /// <summary>Usuario que creó</summary>
        public Guid CRE_CREADO_POR { get; set; }

        /// <summary>Fecha de modificación</summary>
        public DateTime? CRE_FECHA_MODIFICACION { get; set; }

        /// <summary>Detalles del comprobante</summary>
        public DetalleComprobanteCarteraModel Detalle { get; set; }
    }
}
