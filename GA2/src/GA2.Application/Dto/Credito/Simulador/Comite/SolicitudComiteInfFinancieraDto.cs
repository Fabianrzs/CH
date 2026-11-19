using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador.Comite
{
    public class SolicitudComiteInfFinancieraDto
    {
        public InformacionAfiliadoComiteDto InformacionAfilidado { get; set; }
        public InformacionFinancieraComiteDto InformacionFinanciera { get; set; }
        public InformacionInmuebleComiteDto InformacionInmueble { get; set; }
        public Guid Sol_id { get; set; }
    }

    public class InformacionAfiliadoComiteDto
    {
        public int CuotasAportadas { get; set; }
        public double SalarioBasico { get; set; }
        public double SalarioTotal { get; set; }
        public int Edad { get; set; }
        public string EstadoCivil { get; set; }
        public int Score { get; set; }
    }

    public class InformacionFinancieraComiteDto
    {
        public double ValorCredito { get; set; }
        public decimal TasaEA { get; set; }
        public int Plazo { get; set; }
        public int PlazoAnio { get; set; }
        public decimal CuotaMensual { get; set; }
        public double CuentaIndividual { get; set; }
        public double Subsidio { get; set; }
        public double RecursosPropios { get; set; }
        public int EndeudamientoSCH { get; set; }
        public int EndeudamientoCCH { get; set; }
    }

    public class InformacionInmuebleComiteDto
    {
        public string EstadoVivienda { get; set; }
        public string Ubicación { get; set; }
        public double AvaluoComercial { get; set; }
        public double AvaluoCatastral { get; set; }
        public double ValorVenta { get; set; }
        public decimal PorcFinanciacion { get; set; }
    }
}
