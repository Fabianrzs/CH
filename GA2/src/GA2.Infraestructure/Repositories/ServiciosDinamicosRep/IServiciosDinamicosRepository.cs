using GA2.Domain.Entities.ServiciosDinamicosEnt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories.ServiciosDinamicosRep
{
    public interface IServiciosDinamicosRepository
    {
        Task<ServiciosDinamicos> ConsultarEstadoServicio(); 
    }
}
