namespace GA2.Domain.Entities
{
    public class ParametrosGenerales
    {
        public int SalarioMinimo { get; set; }
        public int LimiteVIP { get; set; }
        public int LimiteVIS { get; set; }
        public int ValorMinimoCuota { get; set; }
        public int MaximoPlazoMeses { get; set; }
        public int MinimoPlazoMeses { get; set; }
        public Tasas Tasas { get; set; }
        public Factores Factores { get; set; }
        public string Status { get; set; }
        public bool ActiveTU { get; set; }
        public int Score_Min_TU { get; set; }
        public decimal PorcentajeMinimoEndeudamiento { get; set; }
        public bool AhorroIntereses { get; set; }
        public bool CesantiasIntereses { get; set; }
        public bool RangoAscenso { get; set; }
        public bool ObligacionesExternasCapacidadPago { get; set; }
        public bool SeleccionCargoConSalario { get; set; }
    }
}

    public class Tasas
    {
        public decimal EfectivaAnual { get; set; }
        public decimal NominalMesVencido { get; set; }
        public decimal Mixta { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal Min_Canon_Inicial { get; set; }
        public decimal Max_Canon_Inicial { get; set; }
        public decimal Max_Opcion_Compra { get; set; }
    }

    public class Factores
    {
        public decimal FactorIncremento { get; set; }
        public decimal FactorIncrementoSubsidio { get; set; }
        public decimal FactorAhorro { get; set; }
        public decimal FactorInflacion { get; set; }
        public decimal FactorCesantias { get; set; }
        public decimal InteresesCesantias { get; set; }
    }

