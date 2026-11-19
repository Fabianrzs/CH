using GA2.Application.Dto.GrupoOpciones;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class LoginDto : BaseRequest
    {
        public string UserName { get; set; }
        public string Credential { get; set; }
        public bool IsActiveDirectory { get; set; }
    }


    public class RecuperarContrasenaDto : BaseRequest
    {

        public string Url { get; set; }
        public int Tipo { get; set; }
        public string Numero { get; set; }
    }

    public class ResponseLoginDto 
    {

        public string token { get; set; }

        public List<OpcionesDto> opciones { get; set; }
        public Guid? USR_ID { get; set; }
        public string USR_PRIMER_NOMBRE { get; set; }
        public string USR_SEGUNDO_NOMBRE { get; set; }
        public string USR_PRIMER_APELLIDO { get; set; }
        public string USR_SEGUNDO_APELLIDO { get; set; }


    }


}
