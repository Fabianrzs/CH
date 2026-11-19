using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto.GrupoOpciones
{
    public class GrupoOpcionesDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }
        public string Estado { get; set; }

        public List<OpcionesDto> opciones { get; set; }


    }


    public class OpcionesDto : GrupoOpcionesDto
    {
        public int IdGrupo { get; set; }
        public List<SubOpcionesDto> SubOpciones { get; set; }
    }

    public class SubOpcionesDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set;}
        public string Descripcion { get; set; }
        public int Estado { get; set; }
        public int IdOpcion { get; set; }
        public bool Checked { get; set; }
    }

}
