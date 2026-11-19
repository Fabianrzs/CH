using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICierreAnualCuentasRepositoryBua
    {

        Task<RespuestaCierreAnualCuentas> CierreAnualCuentas(ProgramacionCierreAnualCuentas programacion);
        Task<IEnumerable<ProgramacionCierreAnualCuentas>> ConsultarProgramacionCierreAnualCuentas();
    }
}
