using GA2.Application.Dto.GrupoOpciones;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public interface IGruposOpcionesLogic
    {

        Task<Response<IEnumerable<GrupoOpcionesDto>>> ObtenerGruposOpciones();
        Task<Response<GrupoOpcionesDto>> CrearGruposOpciones(GrupoOpcionesDto grupo, string userId);

        Task<Response<GrupoOpcionesDto>> EliminarGruposOpciones(GrupoOpcionesDto grupo, string userId);
        Task<Response<GrupoOpcionesDto>> ActualizaGruposOpciones(GrupoOpcionesDto grupo, string userId);

        Task<Response<IEnumerable<OpcionesDto>>> ObtenerOpciones();

        Task<Response<OpcionesDto>> CrearOpciones(OpcionesDto Opcion, string userId);

    }
}
