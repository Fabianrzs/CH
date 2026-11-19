using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CapXc
{
    public class ObtenerPAXCDto
    {
        public List<ObtenerUserXCiudadDTO> LstAbogados { get; set; }
        public List<ObtenerUserXCiudadDTO> LstPeritos { get; set; }
    }
    public class ObtenerUserXCiudadDTO
    {
        public int PAXC_ID { get; set; }
        public Guid USR_ID { get; set; }
        public string USR_NAME { get; set; }
        public int DPC_ID { get; set; }
        public string DPC_NOMBRE { get; set; }
    }
}
