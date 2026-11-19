using GA2.Application.Dto.MatriculasInmobiliarias;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class MatriculaInmibiliariaDto
    {
        public Guid MaiId { get; set; }
        public string MaiNumeroMatricula { get; set; }
        public string MaiDireccion { get; set; }
        public DateTime MaiFechaRegistro { get; set; }
        public string PrpTipoIdentificacion { get; set; }
        public string PrpNumeroIdentificacion { get; set; }
        public string PrpNombreRazonSocial { get; set; }
        public string PrpPrimerNombre { get; set; }
        public string PrpSegundoNombre { get; set; }
        public string PrpPrimerApellido { get; set; }
        public string PrpSegundoApellido { get; set; }
        public int DcpId { get; set; }
        public int DpdId { get; set; }
        public int IdOficinaRegistro { get; set; }
        public string Estado { get; set; }
        public string nombreUsuario { get; set; }
        public Guid IdUsuario { get; set; }
        public string Correo { get; set; }
        public string celular { get; set; }

        public List<PropietariosMatriculasInmobiliariasDto> Propietarios { get; set; }
        public List<HistotialNovedadesMatriculasInmobiliariasDto> Novedades { get; set; }
        public List<TramitesMatriculasInmobiliariasDto> Tramites { get; set; }
        public List<HistorialCambiosMatriculasDto> cambios { get; set; }
        public List<CausalMatriculaInmobiliariaDto> causales { get; set; }

    }


    public class OperacionesMatriculasDto
    {

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

    }

    public class CausalNovedamatriculaDto {

        public Guid Id { get; set; }
        public Guid Top_Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }

    }


}
