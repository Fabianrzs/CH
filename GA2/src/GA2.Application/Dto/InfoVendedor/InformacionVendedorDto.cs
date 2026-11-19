using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GA2.Application.Dto.InfoVendedor
{
    public class InformacionVendedorDto
    {
        public int Id { get; set; }
        public Guid Soc_id { get; set; }
        public int Tipo_Doc { get; set; }
        public string Numero_Doc { get; set; }
        public string Nombre_Vendedor { get; set; }
        public string Direccion { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public string Correo_Electronico { get; set; }
        public string Telefono { get; set; }
        public DateTime? Fecha_Entrega { get; set; }
        public int? Tipo_Cuenta { get; set; }
        public Guid? Banco { get; set; }
        public long? Cuenta { get; set; }
        public int? CodProveeSeven { get; set; }


    }
    public class RequestEliminarVendedorCredito
    {
        public int Id { get; set; }


    }

    public class consultarVendedorDto
    {
        public Guid? Sls_Id { get; set; }
        public Guid? Soc_Id { get; set; }



    }
}
