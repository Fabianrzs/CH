using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IAuditoriaRepository
    {
        Task<IEnumerable<Auditoria>> ObtenerlogAuditoria(ConsultaAuditoria parametros);
        Task<Auditoria> InsertarLogAuditoria(AuditoriaDto request);
        Task<IEnumerable<TablasAuditoria>> ObtenerTablasAuditoria();
        Task<int> ObtenerNumeroRegistrologAuditoria(ConsultaAuditoria parametros);
        Task<bool> InsertReportIntegraciones(ReportIntegracion parametros);
        Task<bool> InsertReportIntegracionesRest(ReportIntegracion parametros);
        Task<bool> UpdateReportIntegraciones(ReportIntegracion reportIntegracion);
        Task<int> InsertarAuditoriaDetalle(AuditLog request);
        Task<int> ObtenerTipoAcionAuditoria(int request);
    }
}
