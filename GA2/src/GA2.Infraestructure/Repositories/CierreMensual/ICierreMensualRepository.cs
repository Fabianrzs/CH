using Azure;
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICierreMensualRepository
    {
        Task<List<int>> ObtenerCategoriaConceptos(string concepto);
        Task<int> ObtenerCuentaAbono(string concepto);
        Task<ProgramacionCierreMensualCuentas> ProgramarCierreMensualCuentas(ParametrosCierreMensual programacionCierre, CierreMensualCuentas cierre);
        Task<IPC> ObtenerIPCMes();
        Task<ProgramacionCierreMensualCuentas> ActualizaEstadoCierreMensualCuentas(Guid id, int estado);
        Task<IEnumerable<ProgramacionCierreMensualCuentas>> ConsultarProgramacionCierreMensualCuentas();
        Task<bool> ValidarExisteCierre(int anio, int mes);
        Task<IEnumerable<ReporteCierreMensualCuentas>> ReporteCierreMensualCuentas(Guid Id);


    }
}