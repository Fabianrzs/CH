using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.MatriculasInmobiliarias;
using GA2.Application.Main;
using GA2.Domain.Entities;
using GA2.Domain.Entities.MatriculasInmobilarias;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class MatriculasInmobilariasLogic : IMatriculasInmobiliariasLogic
    {

        private readonly IMatriculasInmobiliariasRepository _matriculasInmobiliariasRepository;
        private readonly IMapper _mapper;
        private readonly IClientRequestProvider _iClientRequestProvider;

        public MatriculasInmobilariasLogic(IMatriculasInmobiliariasRepository matriculasInmobiliariasRepository, IMapper mapper, IClientRequestProvider clientRequestProvider)
        {
            _matriculasInmobiliariasRepository = matriculasInmobiliariasRepository;
            _mapper = mapper;
            _iClientRequestProvider = clientRequestProvider;
        }

        public async Task<Response<PropietariosMatriculasInmobiliariasDto>> AgregarHistorialDePropietario(PropietariosMatriculasInmobiliariasDto request, Guid user)
        {
            var gp = this._mapper.Map<PropietariosMatriculasInmobiliariasDto>(await this._matriculasInmobiliariasRepository.AgregarHistorialDePropietario(this._mapper.Map<PropietariosMatriculasInmobiliarias>(request), user));
            return new Response<PropietariosMatriculasInmobiliariasDto> { Data = gp };
        }

        public async Task<Response<CrearMatriculRespondeDto>> CrearMatriculaInmobiliaria(CrearMatriculaDto crearMatricula)
        {
            CrearMatricula parametros = this._mapper.Map<CrearMatricula>(crearMatricula);
            var resultMatriculas = await this._matriculasInmobiliariasRepository.FiltroMatriculaInmobiliaria(crearMatricula.NumeroMatricula, null, 0, DateTime.Now, DateTime.Now, 0, "", "");

            if (!resultMatriculas.Any(x => x.MAI_NUMERO_MATRICULA == crearMatricula.NumeroMatricula))
            {
                var gp = await _matriculasInmobiliariasRepository.CrearMatriculaInmobiliaria(parametros);
                if (gp is not null)
                {
                    var propietario = new CrearMatriculaRequest
                    {
                        MAI_ID = gp.MAI_ID,
                        PRP_ID = gp.PRP_ID,
                        MAI_CREATEDOPOR = DateTime.Now
                    };
                    var createPropiertatioMatricula = await _matriculasInmobiliariasRepository.CrearMatriculaPropietario(propietario);
                }
                var resultDta = _mapper.Map<CrearMatriculaResponse, CrearMatriculRespondeDto>(gp);
                return new Response<CrearMatriculRespondeDto> { Data = resultDta };
            }


            return new Response<CrearMatriculRespondeDto> { ReturnMessage = "Esta matricula ya se encuentra creada: " + crearMatricula.NumeroMatricula, Data = null };
        }

        public async Task<Response<PropietariosMatriculasInmobiliariasDto>> EditarHistorialPropietarios(PropietariosMatriculasInmobiliariasDto request)
        {
            var gp = this._mapper.Map<PropietariosMatriculasInmobiliariasDto>(await this._matriculasInmobiliariasRepository.EditarHistorialPropietarios(this._mapper.Map<PropietariosMatriculasInmobiliarias>(request)));
            return new Response<PropietariosMatriculasInmobiliariasDto> { Data = gp };
        }

        public async Task<Response<MatriculaInmibiliariaDto>> EditarInformacionBasicaMatricula(MatriculaInmibiliariaDto request)
        {
            var gp = this._mapper.Map<MatriculaInmibiliariaDto>(await this._matriculasInmobiliariasRepository.EditarInformacionBasicaMatricula(this._mapper.Map<MatriculaInmibiliaria>(request)));
            return new Response<MatriculaInmibiliariaDto> { Data = gp };
        }

        public async Task<Response<PropietariosMatriculasInmobiliariasDto>> EliminarHistorialDePropietarios(string request)
        {
            var gp = await this._matriculasInmobiliariasRepository.EliminarHistorialDePropietarios(request);
            var result = this._mapper.Map<PropietariosMatriculasInmobiliariasDto>(gp);
            return new Response<PropietariosMatriculasInmobiliariasDto> { Data = result };

        }

        public async Task<Response<MatriculaInmibiliariaDto>> EliminarMatriculaInmobiliaria(MatriculaInmibiliariaDto request)
        {
            await this._matriculasInmobiliariasRepository.EliminarMatriculaInmobiliaria(request.MaiId);
            return new Response<MatriculaInmibiliariaDto> { Data = request };
        }

        public async Task<Response<IEnumerable<MatriculaInmibiliariaDto>>> FilterMatriculasInmobiliarias(FiltroMatriculasInmobiliariasDto filtro)
        {
            string cliId = "";

            if (!string.IsNullOrEmpty(filtro.NumeroIdentificacion))
            {
                cliId = await ObtenerIdCliente(filtro.NumeroIdentificacion);
            }
            var gp = await this._matriculasInmobiliariasRepository.FiltroMatriculaInmobiliaria(filtro.NumeroMatricula, filtro.NumeroIdentificacion, filtro.TipoIdentificacion, filtro.fechaCreacion, filtro.fechaFin, filtro.bloqueo, filtro.nomOfc, cliId);
            var result = this._mapper.Map<IEnumerable<MatriculaInmibiliaria>, IEnumerable<MatriculaInmibiliariaDto>>(gp);
            return new Response<IEnumerable<MatriculaInmibiliariaDto>> { Data = result };

        }

        private async Task<string> ObtenerIdCliente(string identificacion)
        {
            var clienteDto = await _iClientRequestProvider.ObtenerClientesByNumeroIdentificacion(identificacion);

            return clienteDto.Data != null ? clienteDto.Data.IdCliente.ToString() : "";
        }

        public async Task<Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>> HistorialNovedadsMatriculaInmobiliariaById(Guid id)
        {
            var gp = await this._matriculasInmobiliariasRepository.HistorialNovedadesMatriculaInmobiliariaPorId(id);
            var result = this._mapper.Map<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>, IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>(gp);
            return new Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<PropietariosMatriculasInmobiliariasDto>>> HistorialPropietariosMatriculaInmobiliariaById(Guid id)
        {
            var gp = await this._matriculasInmobiliariasRepository.HistorialPropietariosMatriculaInmobiliariaPorId(id);
            var result = this._mapper.Map<IEnumerable<PropietariosMatriculasInmobiliarias>, IEnumerable<PropietariosMatriculasInmobiliariasDto>>(gp);
            return new Response<IEnumerable<PropietariosMatriculasInmobiliariasDto>> { Data = result };

        }

        public async Task<Response<MatriculaInmibiliariaDto>> InformacionBasicaMatriculaInmobiliariaById(Guid id)
        {
            var resulBasica = await this._matriculasInmobiliariasRepository.InformacionBasicaMatriculaInmobiliariaPorId(id);
            var resultInfoMatricula = this._mapper.Map<MatriculaInmibiliariaDto>(resulBasica);

            var resultPropi = await this._matriculasInmobiliariasRepository.HistorialPropietariosMatriculaInmobiliariaPorId(id);
            var resultPropietarios = this._mapper.Map<IEnumerable<PropietariosMatriculasInmobiliarias>, IEnumerable<PropietariosMatriculasInmobiliariasDto>>(resultPropi);

            resultInfoMatricula.Propietarios = (List<PropietariosMatriculasInmobiliariasDto>)resultPropietarios;

            var resultnovedades = await this._matriculasInmobiliariasRepository.HistorialNovedadesMatriculaInmobiliariaPorId(id);
            var resultNovedadesMatricula = this._mapper.Map<IEnumerable<HistotialNovedadesMatriculasInmobiliarias>, IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>(resultnovedades);
            resultInfoMatricula.Novedades = (List<HistotialNovedadesMatriculasInmobiliariasDto>)resultNovedadesMatricula;

            var resultTramites = await this._matriculasInmobiliariasRepository.ConsultarMatriculasSolicitudes(id);
            var resultTramitesMatriculas = this._mapper.Map<IEnumerable<TramitesMatriculasInmobiliarias>, IEnumerable<TramitesMatriculasInmobiliariasDto>>(resultTramites);
            resultInfoMatricula.Tramites = (List<TramitesMatriculasInmobiliariasDto>)resultTramitesMatriculas;

            foreach (var item in resultInfoMatricula.Tramites)
            {
                var cliente = await AsignarNombreEIdentificacion(item.idCliente.ToString());
                if (cliente != null)
                {
                    item.tipoDoc = cliente.IdTipoIdentificacion.ToString();
                    item.nroDoc = cliente.Identificacion;
                    item.nombreRazonSocial = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}";
                }
            }

            resultInfoMatricula.Tramites.RemoveAll(x => x.nroDoc == null || x.nroDoc == "");

            var resultCaambios = await this._matriculasInmobiliariasRepository.ConsultarMatriculasCambios(id);
            var resultCaambiosMatriculas = this._mapper.Map<IEnumerable<HistorialCambiosMatriculas>, IEnumerable<HistorialCambiosMatriculasDto>>(resultCaambios);
            resultInfoMatricula.cambios = (List<HistorialCambiosMatriculasDto>)resultCaambiosMatriculas;

            var resultCausal = await this._matriculasInmobiliariasRepository.ConsultarCausalesMatriculaInmobiliaria();
            var resultCausalMatriculas = this._mapper.Map<IEnumerable<CausalMatricula>, IEnumerable<CausalMatriculaInmobiliariaDto>>(resultCausal);
            resultInfoMatricula.causales = (List<CausalMatriculaInmobiliariaDto>)resultCausalMatriculas;

            return new Response<MatriculaInmibiliariaDto> { Data = resultInfoMatricula };

        }

        private async Task<ClienteDto> AsignarNombreEIdentificacion(string identificacion)
        {
            var clientesDto = await _iClientRequestProvider.ObtenerClientesById(identificacion);

            var clienteDto = clientesDto.Data.FirstOrDefault();

            if (clienteDto != null)
            {

            }

            return clienteDto;
        }

        public async Task<Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>> InsertNovedMatriculaInmobiliaria(HistotialNovedadesMatriculasInmobiliariasDto novedad)
        {

            var nov = this._mapper.Map<HistotialNovedadesMatriculasInmobiliarias>(novedad);
            var gp = await this._matriculasInmobiliariasRepository.InsertarNovedadMatriculaInmobiliaria(nov);

            var result = this._mapper.Map<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>>(gp);
            return new Response<IEnumerable<HistotialNovedadesMatriculasInmobiliariasDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<CausalNovedamatriculaDto>>> listarCausalNovedamatriculaById(Guid id)
        {
            var gp = await this._matriculasInmobiliariasRepository.listarCausalNovedaMatriculaPorId(id);
            var result = this._mapper.Map<IEnumerable<CausalNovedamatricula>, IEnumerable<CausalNovedamatriculaDto>>(gp);
            return new Response<IEnumerable<CausalNovedamatriculaDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<OperacionesMatriculasDto>>> listarOperacionesMatriculas()
        {
            var gp = await this._matriculasInmobiliariasRepository.listarOperacionesMatriculas();
            var result = this._mapper.Map<IEnumerable<OperacionesMatriculas>, IEnumerable<OperacionesMatriculasDto>>(gp);
            return new Response<IEnumerable<OperacionesMatriculasDto>> { Data = result };

        }
        public async Task<Response<IEnumerable<CausalMatriculaInmobiliariaDto>>> ObtenerMatriculaInmobiliariaCausal()
        {
            var gp = await this._matriculasInmobiliariasRepository.ConsultarCausalesMatriculaInmobiliaria();
            var result = this._mapper.Map<IEnumerable<CausalMatricula>, IEnumerable<CausalMatriculaInmobiliariaDto>>(gp);
            return new Response<IEnumerable<CausalMatriculaInmobiliariaDto>> { Data = result };


        }


    }
}
