using System;

namespace GA2.Application.Dto.BUA
{
    public class InformacionBasicaClienteDto
    {
        public int IdCliente { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaExpedicionDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreCompletoInvertido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int IdFuerza { get; set; }
        public int IdGrado { get; set; }
        public int IdCategoria { get; set; }
        public string Sexo { get; set; }
        public int IdDepartamento { get; set; }
        public int IdCiudad { get; set; }
        public string ID_UNIDADEJECITORA { get; set; }
        public string TipoVinculacion { get; set; }


        public string Nombres => $"{PrimerNombre} {SegundoNombre}".Trim();
        public string Apellidos => $"{PrimerApellido} {SegundoApellido}".Trim();
    }
}