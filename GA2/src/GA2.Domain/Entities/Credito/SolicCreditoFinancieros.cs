using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class SolicCreditoFinancieros
    {

        public Guid SOF_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public string SOF_DIRECCION_VIVIENDA { get; set; }
        public string SOF_MATRICULA_VIVIENDA { get; set; }
        public decimal SOF_VALOR_COMERCIAL_VIVIENDA { get; set; }
        public string SOF_MARCA_VEHICULO { get; set; }
        public string SOF_PLACA_VEHICULO { get; set; }
        public decimal SOF_VALOR_COMERCIAL_VEHICULO { get; set; }
        public Guid SOF_CREADO_POR { get; set; }
        public DateTime SOF_FECHA_CREACION { get; set; }
        public Guid SOF_ACTUALIZADO_POR { get; set; }
        public DateTime SOF_FECHA_ACTUALIZA { get; set; }
    }

    public class SolicCreditoRefererncias {


        public Guid SOR_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public string SOR_TIPO_REFERENCIA { get; set; }
        public string SOR_PRIMER_APELLIDO { get; set; }
        public string SOR_SEGUNDO_APELLIDO { get; set; }
        public string SOR_NOMBRES { get; set; }
        public string SOR_PARENTESCO { get; set; }
        public string SOR_RELACION { get; set; }
        public string SOR_DIRECCION { get; set; }
        public int DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public string SOR_TELEFONO { get; set; }
        public string SOR_CELULAR { get; set; }
        public Guid SOR_CREADO_POR { get; set; }
        public DateTime SOR_FECHA_CREACION { get; set; }
        public Guid SOR_ACTUALIZADO_POR { get; set; }
        public DateTime SOR_FECHA_ACTUALIZA { get; set; }
        public string SOR_REFERENCIA { get; set; }
        public string DESCRIPCION_PARENTESCO { get; set; }

    }
}
