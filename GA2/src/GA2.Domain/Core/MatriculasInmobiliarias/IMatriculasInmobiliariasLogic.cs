using System;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;
using System.Threading.Tasks;
using GA2.Domain.Entities;
using GA2.Application.Dto.MatriculasInmobiliarias;

namespace GA2.Domain.Core
{
    public interface IMatriculasInmobiliariasLogic
    {

        Task<Response<IEnumerable<MatriculaInmibiliariaDto>>> FilterMatriculasInmobiliarias(FiltroMatriculasInmobiliariasDto filto);

        Task<Response<MatriculaInmibiliariaDto>> InformacionBasicaMatriculaInmobiliariaById(Guid id);

        Task<Response<IEnumerable<PropietariosMatriculasInmobiliariasDto>>> HistorialPropietariosMatriculaInmobiliariaById(Guid id);

        Task<Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>> InsertNovedMatriculaInmobiliaria(HistotialNovedadesMatriculasInmobiliariasDto novedad);

        Task<Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>> HistorialNovedadsMatriculaInmobiliariaById(Guid id);


        Task<Response<IEnumerable<OperacionesMatriculasDto>>> listarOperacionesMatriculas();

        Task<Response<IEnumerable<CausalNovedamatriculaDto>>> listarCausalNovedamatriculaById(Guid id);
        Task<Response<CrearMatriculRespondeDto>> CrearMatriculaInmobiliaria(CrearMatriculaDto crearMatricula);
        Task<Response<MatriculaInmibiliariaDto>> EliminarMatriculaInmobiliaria(MatriculaInmibiliariaDto request);
        Task<Response<MatriculaInmibiliariaDto>> EditarInformacionBasicaMatricula(MatriculaInmibiliariaDto request);
        Task<Response<PropietariosMatriculasInmobiliariasDto>> EditarHistorialPropietarios(PropietariosMatriculasInmobiliariasDto request);
        Task<Response<PropietariosMatriculasInmobiliariasDto>> EliminarHistorialDePropietarios(string request);
        Task<Response<PropietariosMatriculasInmobiliariasDto>> AgregarHistorialDePropietario(PropietariosMatriculasInmobiliariasDto request,Guid user);
        Task<Response<IEnumerable<CausalMatriculaInmobiliariaDto>>> ObtenerMatriculaInmobiliariaCausal();
    }
}
