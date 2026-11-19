namespace GA2.Application.Dto.Cartera.Reportes.ResumenConceptosFinancieros
{
    public class InformacionFinancieraDto
    {
        public decimal SALARIO_TOTAL { get; set; }
        public decimal SCORE_CLEAR { get; set; }
        public decimal SCORE_CAJA_HONOR { get; set; }
        public string CAPACIDAD_PAGO { get; set; }
        public string HABITO_PAGO { get; set; }
        public string PRE_APROBADO_EXPERTO { get; set; }
        public string CONSULTA_CIFIN { get; set; }
        public int CONSULTA_CIFIN_DIAS { get; set; }
        public decimal TOTAL_ACTIVOS { get; set; }
        public string DESCRIPCION_ACTIVOS { get; set; }
        public decimal TOTAL_PASIVOS { get; set; }
    }
}