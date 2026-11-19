using System.ComponentModel.DataAnnotations;

namespace GA2.Domain.Entities
{
    public class TiposArchivos
    {
        [Key]
        public int TAR_ID { get; set; }
        public string TAR_TIPO { get; set; }
        public string TAR_CLASIFICACION { get; set; }
        public string TAR_DESCRIPCION { get; set; }
    }
}
