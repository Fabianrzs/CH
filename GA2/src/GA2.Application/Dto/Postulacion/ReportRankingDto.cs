namespace GA2.Application.Dto.Postulacion
{
    public class ReportRankingDto
    {
        public int Puesto { get; set; }
        public int Consecutivo { get; set; }
        
        public string NoRadicado { get; set; }
        public string NombreAfiliado { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Fuerza { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Categoria { get; set; }
        public string Ciudad1 { get; set; }
        public string Ciudad2 { get; set; }
        public string Ciudad3 { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string MuerteDiscapacidad { get; set; }
        public string ImputabilidadServicio { get; set; }
        public int? Criterio1 { get; set; }
        public string? PorcentajePostulacionDescripcion { get; set; }
        public int? Criterio2 { get; set; }
        public string? PorcentajeMuerteDiscapacidad { get; set; }
        public double? Criterio3 { get; set; }
        public string? DisfrutePension { get; set; }
        public int? Calificacion { get; set; }
        public string? Resolucion { get; set; }
        public int? PorcentajePostulacion { get; set; }
        public int? PorcentajeAportes { get; set; }
        public int? PorcentajeCesantías { get; set; }
        public string? NombresBeneficiarios { get; set; }
        public string? TipoIdBeneficiarios { get; set; }
        public long? NrosIdentBeneficiarios { get; set; }
        public int? Particip​aciónBeneficiarios { get; set; }
        public string? ParentescoBeneficiarios { get; set; }
        public string? PoseeVivienda { get; set; }
        public string? AfectacionFisica { get; set; }
        public string? AfectacionSensorial { get; set; }
        public string? EnfermedadEspecifica { get; set; }
        public string? RecibeTratamiento { get; set; }
        public string? RequiereMedicamento { get; set; }
        public string? RequiereAyuda { get; set; }
        public string CausalRetiro { get; set; }
        public string? CausalObservacion { get; set; }

        ///INFORMACION BUA
        ///
        public double? SaldoCuentas { get; set; }
        public decimal ValorBloqueo { get; set; }
        public decimal ValorSubsidio { get; set; }
        public decimal ValorCaja { get; set; }
        public string Favorecido { get; set; }
        public string Mecanimos { get; set; }
        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Barrio { get; set; }

        public string Ciudad { get; set; }
        public string Departamento { get; set; }
    }
    public class Beneficiary
    {
        public int? PorcentajeCesantia { get; set; }
        public int? PorcentajeAporte { get; set; }
        public int? PorcentajePostulacion { get; set; }
        public double? PorcentajeBeneficiario { get; set; }
        public string TipoBeneficiario { get; set; }
        public long? IdentificacionBeneficiario { get; set; }
        public string NombreCompleto { get; set; }
        public string AfectaccionFisica { get; set; }
        public string AfectaccionSensorial { get; set; }
        public string EnfermedadEspecifica { get; set; }
        public string RecibeTratamiento { get; set; }
        public string RequiereMedicamento { get; set; }
        public string RequiereAyuda { get; set; }
        public string NombreIdentificacion { get; set; }
    }

}
