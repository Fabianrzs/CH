using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CronogramaNovedades
{
    public class DescargaDefinitiva
    {
        public int NVA_DES_ID { get; set; }
        public string NVA_DES_NOMBRE_ARCHIVO { get; set; }
        public string NVA_DES_PERIODO { get; set; }
        public int CNV_ID { get; set; }
        public int UEJ_ID { get; set; }
        public DateTime NVA_DES_FECHA { get; set; }
        public string NVA_DES_ESTADO { get; set; }
        public Guid NVA_DES_USUARIO { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public string UEJ_SIGLA { get; set; }
        public string CNV_DESCRIPCION { get; set; }
        public string NVD_CODIGO_ALTA { get; set; }
        public bool NVA_DES_DESCARGADO { get; set; }
    }
}
