namespace GA2.Application.Dto.Cartera.Reportes.ResumenConceptosFinancieros
{
    public class InformacionProductoDto
    {
        public string PRODUCTO { get; set; }
        public string ENTIDAD { get; set; }
        public decimal CUPO_APROBADO { get; set; }
        public decimal SALDO { get; set; }
        public decimal CUOTA { get; set; }
        public string CLASIFICACION { get; set; }
    }
}