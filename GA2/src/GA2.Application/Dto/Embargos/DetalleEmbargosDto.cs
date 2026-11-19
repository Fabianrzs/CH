using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class DetalleEmbargosDto
    {
        public EmbargosDto Embargo { get; set; }

       public List<EmbargoCuentaConceptoDto> Conceptos { get; set; }

        public static implicit operator List<object>(DetalleEmbargosDto v)
        {
            throw new NotImplementedException();
        }
    }
}
