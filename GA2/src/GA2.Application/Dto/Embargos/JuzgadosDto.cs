using System;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Juzgado Dto
    /// </summary>
    public class JuzgadosDto
    {
        public Guid? IdJuzgado { get; set; }
        public string CodigoInterno { get; set; }
        public string Nombre { get; set; }        
        public int IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public string NroCuenta { get; set; }
        public string CodigoOficina { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Celular { get; set; }
        public string? Contacto { get; set; }
        public int codigoCiudad { get; set; }
        public bool Estado { get; set; }
        public int IdTipoActor { get; set; }
        public int TipoActor { get; set; }
        public string TipoActorDescripcion { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }


    public class JuzgadosGuidDto
    {
        public Guid IdJuzgado { get; set; }
        public string CodigoInterno { get; set; }
        public string Nombre { get; set; }
        public int IdDepartamento { get; set; }
        public string Departamento { get; set; }
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public string NroCuenta { get; set; }
        public string CodigoOficina { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Celular { get; set; }
        public string? Contacto { get; set; }
        public int codigoCiudad { get; set; }
        public bool Estado { get; set; }

    }

    public class TipoactorDto
    {
  
        public string TPR_NOMBRE { get; set; }
        public int? TPR_ID { get; set; }
        
    }
}
