using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.FichaAnalista
{
    public class FichaAnalistaDto
    {
        public int Fa_Id { get; set; }
        public Guid Soc_id { get; set; }
        public string Fa_Nombre { get; set; }
        public string Fa_Cargo { get; set; }
    }
}
