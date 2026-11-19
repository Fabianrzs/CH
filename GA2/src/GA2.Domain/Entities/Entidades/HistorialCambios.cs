using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Entidades
{
    public class HistorialCambios
    {
        public int ENEC_ID { get; set; }
        public Guid ENE_ID { get; set; }
        public string ENEC_PESTANA { get; set; }
        public string ENEC_CAMPO { get; set; }
        public string ENEC_VLR_ANTERIOR { get; set; }
        public string ENEC_VLR_NUEVO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }
        public Guid ENEC_USER { get; set; }
        public DateTime ENEC_FECMOD { get; set; }
    }

    public class HistorialCambiosInterviniente
    {
        public long ITRC_ID { get; set; }
        public Guid ITR_ID { get; set; }
        public string ITRC_PESTANA { get; set; }
        public string ITRC_CAMPO { get; set; }
        public string ITRC_VLR_ANTERIOR { get; set; }
        public string ITRC_VLR_NUEVO { get; set; }
        public string NOMBRE_COMPLETO { get; set; }   
        public DateTime ITRC_FECMOD { get; set; }
        public string ITRC_MODULO { get; set; }
    }

}
