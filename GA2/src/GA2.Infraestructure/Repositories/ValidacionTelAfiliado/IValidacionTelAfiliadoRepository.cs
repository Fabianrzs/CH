using GA2.Domain.Entities.DatosAliadoEstrategico;
using GA2.Domain.Entities.ValidacionTelAfiliado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ValidacionTelAfiliado
{
    public interface IValidacionTelAfiliadoRepository
    {
        Task<ValidacionTelAfiliados> CrearValTelAfiliado(ValidacionTelAfiliados ValTelAfiliado);
        Task<ValidacionTelAfiliados> ObtenerValTelAfiliado(Guid Soc_Id);
    }
}