using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICierreMensualRepositoryBua
    {

        Task<RespuestaCierreMensualCuentas> CierreMensual(ProgramacionCierreMensualCuentas programacion);
        Task<IEnumerable<ProgramacionCierreMensualCuentas>> ConsultarProgramacionCierreMensual();
        Task<IEnumerable<ReporteCierreMensualCuentas>> ReporteCierreMensual(Guid Id);
    }
}
