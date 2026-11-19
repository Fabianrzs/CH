using GA2.Domain.Entities;
using GA2.Domain.Entities.MatriculasInmobilarias;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IMatriculasInmobiliariasRepository
    {
        Task<IEnumerable<MatriculaInmibiliaria>> FiltroMatriculaInmobiliaria(string Numero_matricula, string numero_identifiucacion, int tipo_documento, DateTime? fecha_creacion, DateTime? fecha_final, int bloqueo, string nomOficina, string cliId);

        Task<MatriculaInmibiliaria> InformacionBasicaMatriculaInmobiliariaPorId(Guid id);

        Task<IEnumerable<PropietariosMatriculasInmobiliarias>> HistorialPropietariosMatriculaInmobiliariaPorId(Guid id);

        Task<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>> InsertarNovedadMatriculaInmobiliaria(HistotialNovedadesMatriculasInmobiliarias novedad);

        Task<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>> HistorialNovedadesMatriculaInmobiliariaPorId(Guid id);

        Task<IEnumerable<OperacionesMatriculas>> listarOperacionesMatriculas();

        Task<IEnumerable<CausalNovedamatricula>> listarCausalNovedaMatriculaPorId(Guid id);
        Task<CrearMatriculaResponse> CrearMatriculaInmobiliaria(CrearMatricula crearMatricula);
        Task<MatriculaInmibiliaria> EliminarMatriculaInmobiliaria(Guid idMatricula);
        Task<MatriculaInmibiliaria> EditarInformacionBasicaMatricula(MatriculaInmibiliaria request);
        Task<PropietariosMatriculasInmobiliarias> EditarHistorialPropietarios(PropietariosMatriculasInmobiliarias request);
        Task<PropietariosMatriculasInmobiliarias> EliminarHistorialDePropietarios(string request);
        Task<PropietariosMatriculasInmobiliarias> AgregarHistorialDePropietario(PropietariosMatriculasInmobiliarias request,Guid user);
        Task<IEnumerable<TramitesMatriculasInmobiliarias>> ConsultarMatriculasSolicitudes(Guid id);
        Task<IEnumerable<HistorialCambiosMatriculas>> ConsultarMatriculasCambios(Guid id);
        Task<IEnumerable<CausalMatricula>> ConsultarCausalesMatriculaInmobiliaria();

        Task<int> CrearMatriculaPropietario(CrearMatriculaRequest crearMatricula);
    }
}
