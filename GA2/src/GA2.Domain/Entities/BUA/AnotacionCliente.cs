using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.BUA
{
    public class AnotacionCliente
    {
        public int ACL_ANOTACION_ID { get; set; }
        public string ACL_ANOTACION { get; set; }
        public string ACL_ANOTACION_USUARIO { get; set; }
        public DateTime ACL_ANOTACION_FECHA { get; set; }
        public int CLI_ID { get; set; }
    }
}
