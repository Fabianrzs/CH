using Azure;
using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface ICierreAnualCuentasRepository
    {
        Task<List<int>> ObtenerCategoriaConceptos(string concepto);
        Task<int> ObtenerCuentaAbono(string concepto);
        Task<ProgramacionCierreAnualCuentas> ProgramarCierreAnual(ParametrosCierreAnualCuentas programacionCierre, CierreAnualCuentas cierre);
        Task<IPC> ObtenerIPCMes();
        Task<ProgramacionCierreAnualCuentas> ActualizaEstadoCierreAnualCuentas(Guid id, int estado);
        Task<IEnumerable<ProgramacionCierreAnualCuentas>> ConsultarProgramacionCierreAnualCuentas();
        Task<bool> ValidarExisteCierreMensual(int anio, int mes);


    }
}