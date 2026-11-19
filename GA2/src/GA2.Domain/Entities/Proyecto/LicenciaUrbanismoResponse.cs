using System;

namespace GA2.Domain.Entities.Proyecto
{
    public class LicenciaUrbanismoResponse
    {
        public int Codigo { get; set; }
        public string Resolucion { get; set; }
        public string Curaduria { get; set; }
        public int DeparamentoId { get; set; }
        public string Departamento { get; set; }
        public int MunicipioId { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Titular { get; set; }
        public DateTime FechaResolucion { get; set; }
        public int MismaLicencia { get; set; }
        public Guid? ProyectoID { get; set; }
    }
}
