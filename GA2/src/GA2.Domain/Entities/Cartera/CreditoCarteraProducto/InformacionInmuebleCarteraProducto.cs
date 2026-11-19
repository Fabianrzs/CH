using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class InformacionInmuebleCarteraProducto
    {
        public decimal ValorInmueble { get; set; }
        public decimal ValorAvaluo { get; set; }    
        public int TipoViviendaID { get; set; }
        public string TipoViviendaDescripcion { get; set; }
        public string ClaseInmueble { get; set; }  
        public int ClaseInmuebleID { get; set; }
        public string NumeroMatricula { get; set; }
        public string Escritura { get; set; }
        public DateTime FechaEscritura { get; set; }
        public string Notaria { get; set; }
        public string Direccion { get; set; }
        public int IdCiudad { get; set; }
        public string CiudadDescripcion { get; set; }
        public int IdDepartamento { get; set; }
        public string DepartamentoDescripcion { get; set; }
        public string Linderos { get; set; }
        public string DescripcionInmueble { get; set; }
        public int IdTipoVivienda { get; set; }
        public string DescripcionTipoVivienda { get; set; }
        public decimal TasasEA { get; set; }
        public string dpc_codigo { get; set; }
        public string dpd_codigo { get; set; }
        public List<MatriculaCredito> MatriculasInmueble { get; set; }
    }
}
