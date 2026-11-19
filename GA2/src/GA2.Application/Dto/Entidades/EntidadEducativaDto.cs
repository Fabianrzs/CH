using GA2.Application.Dto.Entidades;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    /// <summary>
    /// Entidad Educativa Dto
    /// </summary>
    public class EntidadEducativaDto
    {
        public Guid? IdEntidad { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Nit { get; set; }
        public string RazonSocial { get; set; }
        public string CorreoElectronico { get; set; }
        public int Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public bool Estado { get; set; }
        public string NombreCiudad { get; set; }
        public string NombreDepartamento { get; set; }
        public int Departamento { get; set; }
        public Guid? CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int esAcreditada { get; set; }
        public int tipoPersona { get; set; }
        public List<ListarProgramaEducativoDto> Programas { get; set; }
        public List<SedeEntidadEducativaDto> Sedes { get; set; }
        public List<CuentaBancariaEntidadEducativaDto> Cuentas { get; set; }
        public List<ConsultarArchivosPorEntidadDto> Archivos { get; set; }
        public DateTime FechaCreacionArchivo { get; set; }
        public bool bloqueo { get; set; }
        public int renovarArchivo { get; set; }
        public bool? SolicitudesProcesadas { get; set; }

    }
    public class FidecomisoDto
    {
        public int idTipoIdentificacion { get; set; }
        public string nroIdentificacion { get; set; }
        public Guid entidad_id { get; set; }
        public Guid? usr_id { get; set; }
        public Guid? ENE_FIDEICOMISO_ID { get; set; }
        public string nombreRazonSocial { get; set; }
        public DateTime fecCreacion { get; set; }
        public string TipoDocumento { get; set; }
    }
}
