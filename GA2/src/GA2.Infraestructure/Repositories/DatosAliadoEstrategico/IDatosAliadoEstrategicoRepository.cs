using GA2.Domain.Entities;
using GA2.Domain.Entities.DatosAliadoEstrategico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.DatosAliadoEstrategico
{
    public interface IDatosAliadoEstrategicoRepository
    {
        Task<DatosAliadoEstrategicos> CrearDatosAliadoEstrategico(DatosAliadoEstrategicos aliadoEstrategico);
        Task<DatosAliadoEstrategicos> ActualizarDatosAliadoEstrategico(DatosAliadoEstrategicos aliadoEstrategico);
        Task<DatosAliadoEstrategicos> ObtenerDatosAliadoEstrategico(Guid aliadoEstrategico);
    }
}