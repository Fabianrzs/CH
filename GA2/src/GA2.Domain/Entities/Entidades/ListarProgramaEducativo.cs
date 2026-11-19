using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Programa educativo
    /// </summary>
    public class ListarProgramaEducativo
    {
        public Guid ENE_ID { get; set; }
        public string ENE_NOMBRE_RAZON_SOCIAL { get; set; }
        public Guid PGE_ID { get; set; }
        public string PGE_DESCRIPCION { get; set; }
        public Guid NVE_ID { get; set; }
        public string NVE_DESCRIPCION { get; set; }
        public int CVL_ID { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public bool PGE_ESTADO { get; set; }
        public Guid PGE_CREADOPOR { get; set; }
        public DateTime PGE_FECHACREACION { get; set; }
        public Guid PGE_MODIFICADOPOR { get; set; }
        public DateTime PGE_FECHAMODIFICACION { get; set; }
        public Guid PGN_ID { get; set; }
    }
}