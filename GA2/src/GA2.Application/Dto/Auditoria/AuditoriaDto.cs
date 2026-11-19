using GA2.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class AuditoriaDto
    {

        public int Id { get; set; }
        public string NombreTabla { get; set; }
        public string TipoAccion { get; set; }
        public int IdTipoAccion { get; set; }
        public string Ip { get; set; }
        public string Navegador { get; set; }
        public string SistemaOperativo { get; set; }
        public string Descripcion { get; set; }
        public string NombreColumna { get; set; }
        public string ValorAnterior { get; set; }
        public string ValorNuevo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Guid UsurioId { get; set; }
        public string NombreUsuario { get; set; }
        public Guid Agrupador { get; set; }
        public string Hostname { get; set; }
        public string Modulo { get; set; }
        public string CamposActualizados { get; set; }
    }


    public class listaAuditorias { 

        public int NumeroPaginas { get; set; }
        public int TotalRegistros { get; set; }
        public List<AuditoriaDto> Auditorias { get; set; }

    }

    public class ConsultaAuditoriaDto
    {
        public string? NombreTabla { get; set; }
    
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Guid? UserId { get; set; }
        public string? Usuario { get; set; }
        public string? Agrupador { get; set; }
        public string TipoAccion { get; set; }
        public bool VerDetalle { get; set; }
        public string Descripcion { get; set; }
        public int PageNum { get; set; }
        public string Modulo { get; set; }
        public int PageSize { get; set; }

    }

    public class TablasAuditoriaDto
    {
        public string NombreTabla { get; set; }

    }
}
