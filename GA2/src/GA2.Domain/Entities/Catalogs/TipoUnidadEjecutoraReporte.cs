using System.ComponentModel.DataAnnotations;


namespace GA2.Domain.Entities.Catalogs
{
    public class TipoUnidadEjecutoraReporte
    {
        [Key]
        public int CNC_ID { get; set; }
        public string CNC_DESCRIPCION { get; set; }
        public string CAT_TIPO_CONCEPTO { get; set; }


    }
}
