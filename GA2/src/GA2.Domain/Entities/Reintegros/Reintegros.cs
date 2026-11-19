using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class Reintegros
    {
        public int RT_REI_ID { get; set; }
        public string NRO_RADICADO { get; set; }
        public int UEJ_ID { get; set; }
        public int RT_TIPO_ID { get; set; }
        public int RT_ESTADO_ID { get; set; }
        public DateTime FECHA_REINTEGRO { get; set; }
        public decimal VALOR_SOLICITADO { get; set; }
        public Guid CREADO_POR { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public Guid MODIFICADO_POR { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }

    public class ListaReintegros
    {
        public Guid Id_Reintegro { get; set; }
        public string Numero_Radicado { get; set; }
        public int Id_Unidad_Ejecutora { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public string TipoReintegro { get; set; }
        public long NumeroDocumento { get; set; }
        public string NombreAfiliado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal ValorSueldo { get; set; }
        public string EstadoReintegro { get; set; }
        public Guid UsuarioCreacion { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Numero_Radicado_Relacionado { get; set; }
    }

    public class ReintegrosParams
    {
        public string NRO_RADICADO { get; set; }
        public int UEJ_ID { get; set; }
        public int RT_TIPO_ID { get; set; }
    }
}
