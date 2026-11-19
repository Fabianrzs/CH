using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Constructora
{
    public enum EnumConstructoraParameter
    {
        [Description("@Id_Constructuora")]
        Id_Constructuora,

        [Description("@NombreConstructora")]
        NombreConstructora,

        [Description("@Direccion")]
        Direccion,

        [Description("@IdDepartamento")]
        IdDepartamento,

        [Description("@IdCiudad")]
        IdCiudad,

        [Description("@Email")]
        Email,

        [Description("@Nit")]
        Nit,

        [Description("@Telefono")]
        Telefono,

    }
}
