using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using GA2.Application.Dto.Proyecto;
using GA2.Application.Main;
using GA2.Domain.Core.Reportes.ReportesExcelGlobal;
using GA2.Domain.Entities.Proyecto;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.Proyectos;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Proyecto
{
    public class ProyectoBusiness : IProyectoBusiness
    {
        private readonly IMapper _mapper;
        private readonly IProyectoRepository _proyectoRepository;
        private readonly IBlobStorageGenericRepository _blobStorageGenericRepository;
        private readonly IUsersRepository _usersBusinessLogic;

        public ProyectoBusiness(IMapper mapper,
            IProyectoRepository proyecto,
            IBlobStorageGenericRepository blobStorageGenericRepository,
            IUsersRepository usersBusiness)
        {
            _proyectoRepository = proyecto;
            _mapper = mapper;
            _blobStorageGenericRepository = blobStorageGenericRepository;
            _usersBusinessLogic = usersBusiness;
        }
        public async Task<Response<Guid?>> CrearProyecto(ProyectoConstructoraDto constructoraDto)
        {
            var fechaCreacion = constructoraDto.PC_FECHA_CREACION;
            var dateStar = DateTime.ParseExact(fechaCreacion, "yyyy-MM-ddTHH:mm:ssZ", null, System.Globalization.DateTimeStyles.RoundtripKind);
            var mapeoModel = _mapper.Map<ProyectoConstructoraModel>(constructoraDto);
            mapeoModel.PC_FECHA_CREACION = dateStar;    
            var responseProyecto = await _proyectoRepository.CREAR_PROYECTO_CONSTRUCTORA(mapeoModel);
            if (constructoraDto.licenciaUrbanismoDtos is not null)
            {
                var mapLicenciaUrbanismo = _mapper.Map<LicenciaUrbanismoModel>(constructoraDto.licenciaUrbanismoDtos);
                var mapLicenciaConstruccion = _mapper.Map<LicenciaUrbanismoModel>(constructoraDto.licenciaConstrucionDtos);
                mapLicenciaUrbanismo.PC_PROYECTO_ID = responseProyecto;
                mapLicenciaConstruccion.PC_PROYECTO_ID = responseProyecto;
                await _proyectoRepository.CREAR_LICENCIA_URBANISMO_PROYECTO(mapLicenciaUrbanismo);
                await _proyectoRepository.LICENCIA_CONSTRUCCION_PROYECTO(mapLicenciaConstruccion);
                if (constructoraDto.supervisor.Count > 0)
                {
                    foreach (var item in constructoraDto.supervisor)
                    {
                        var supervisor = _mapper.Map<SupervisorProyecoModel>(item);
                        supervisor.PC_PROYECTO_ID = responseProyecto;
                        var response = await _proyectoRepository.CREATE_SUPERVISOR_PROYECTOS(supervisor);
                    }
                }
                else
                {
                    return new Response<Guid?> { Data = null, IsSuccess = false, ReturnMessage = "Se nesecita al menos un superviso" };
                }
                if (constructoraDto.beneficiarios.Count > 0)
                {
                    foreach (var item in constructoraDto.beneficiarios)
                    {
                        var beneficiario = _mapper.Map<BeneficiarioProyectoModel>(item);
                        beneficiario.PC_PROYECTO_ID = responseProyecto;
                        var response = await _proyectoRepository.CREATE_BENEFICIARIO_PROYECTOS(beneficiario);
                    }
                }
                else
                {
                    return new Response<Guid?> { Data = null, IsSuccess = false, ReturnMessage = "Se nesecita al menos un beneficiario" };
                }
                return new Response<Guid?> { Data = responseProyecto, IsSuccess = true, ReturnMessage = "OK" };
            }

            return new Response<Guid?> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }

        public async Task<Response<List<ProyectoConstructoraResponseDto>>> ObtenerProyectos(RequestConstructoraProyectoDto requets)
        {
            var response = new List<ProyectoConstructoraResponseDto>();
            var mapRequest = _mapper.Map<RequestConstructoraProyecto>(requets);
            var responseProyecto = await _proyectoRepository.ObtenerProyecto(mapRequest);
            if (responseProyecto.Count > 0 )
            {
                foreach (var item in responseProyecto) { 
                    var mapProyecto = _mapper.Map<ProyectoConstructoraResponseDto>(item);
                    response.Add(mapProyecto);
                }
                return new Response<List<ProyectoConstructoraResponseDto>> { Data = response, IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<List<ProyectoConstructoraResponseDto>> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
        public async Task<Response<ProyectoConstructoraResponseDto>> ObtenerProyectosById(Guid id)
        {
            var response = new ProyectoConstructoraResponseDto();
            var responseProyecto = await _proyectoRepository.ObtenerProyectoById(id);
            if (responseProyecto != null)
            {
                var responseLicencia = await _proyectoRepository.ObtenerLicenciaConduccionByProyectoId(id);
                var responseLicenciaB = await _proyectoRepository.ObtenerLicenciaUrbanimosByProyectoId(id);
                var responseSupervisor = await _proyectoRepository.ObtenerSupervisorByProyectoId(id);
                var responseBeneficiario = await _proyectoRepository.ObtenerBeneficiarioByProyectoId(id);
                var responsePoliza = await _proyectoRepository.ObtenerPolizaByProyectoId(id);
                var inmuebleProyecto = await _proyectoRepository.ObtenerTipoInmuebleProyecto(id);   
                var clasesImuebles = await _proyectoRepository.ObtenerClaseInmueble(id);
                var ListaAvance = await _proyectoRepository.ObtenerListaAvance(id);

                var mapLicenciaC = responseLicencia == null ? null :  _mapper.Map<LicenciaUrbanismoResponseDto>(responseLicencia);
                var mapLicenciaB = responseLicenciaB == null ? null : _mapper.Map<LicenciaUrbanismoResponseDto>(responseLicenciaB);
                var mapSuperVisro = responseSupervisor == null || responseSupervisor.Count == 0 ? null : _mapper.Map<List<SupervisorResponseDto>>(responseSupervisor);
                var mapBeneficiario = responseBeneficiario == null || responseBeneficiario.Count == 0 ? null : _mapper.Map<List<BeneficiarioResponseDto>>(responseBeneficiario);
                var mapInmuebleProyecto = inmuebleProyecto == null || inmuebleProyecto.Count == 0 ? null : _mapper.Map<List<InmublesProyectoResponseDto>>(inmuebleProyecto);
                var mapClasesInmuebles = clasesImuebles == null || clasesImuebles.Count == 0 ? null : _mapper.Map<List<ClaseInmuebleResponseDto>>(clasesImuebles);
                var mapListaAvance = ListaAvance == null || ListaAvance.Count == 0 ? null : _mapper.Map<List<ListaAvanceResponseDto>>(ListaAvance);
                var mapPolizas = responsePoliza == null || responsePoliza.Count == 0 ? null : _mapper.Map<List<PolizaProyectoResponseDto>>(responsePoliza);
                var mapProyecto =   _mapper.Map<ProyectoConstructoraResponseDto>(responseProyecto);


                response = mapProyecto;
                response.LicenciaConstruccion = mapLicenciaC;
                response.LicenciaUrbanismo = mapLicenciaB;
                response.supervisors = mapSuperVisro;  
                response.beneficiario = mapBeneficiario;
                response.InmublesProyectos = mapInmuebleProyecto;
                response.ClasesInmuebles = mapClasesInmuebles;
                response.ListaAvance = mapListaAvance;
                response.polizas = mapPolizas;

                return new Response<ProyectoConstructoraResponseDto> { Data = response, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<ProyectoConstructoraResponseDto> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
        public async Task<Response<string>> CrearPolizas(List<PolizaProyectoDto> polizas)
        {
            if (polizas.Count > 0)
            {
                foreach (var item in polizas)
                {
                    var mapProyecto = _mapper.Map<PolizaProyectoModel>(item);
                    var response = await _proyectoRepository.CREATE_PP_POLIZA_PROYECTO(mapProyecto);    
                }
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
        public async Task<Response<string>> ActualizarPolizas(List<PolizaProyectoDto> polizas)
        {
            if (polizas.Count > 0)
            {
                foreach (var item in polizas)
                {
                    var mapProyecto = _mapper.Map<PolizaProyectoModel>(item);
                    var response = await _proyectoRepository.ActualizarPolizaProyecto(mapProyecto);
                }
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
        public async Task<Response<string>> ActualizarProyectoPolizas(ProyectoConstructoraDto proyecto)
        {

            var mapProyecto = _mapper.Map<ProyectoConstructoraModel>(proyecto);
            var response = await _proyectoRepository.ActualizarProyectoConstructora(mapProyecto);
            if (response > 0)
            {
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }

        public async Task<Response<List<TipoIncmueblesDto>>> ObtenerTipoInmueble()
        {

            var response = await _proyectoRepository.ObtenerTipoInmuble();
            var mapResponse = _mapper.Map<List<TipoIncmueblesDto>>(response);
            if (mapResponse.Count > 0)
            {
                return new Response<List<TipoIncmueblesDto>> { Data = mapResponse, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<List<TipoIncmueblesDto>> { Data = null, IsSuccess = false, ReturnMessage = "No hay data" };
        }

        public async Task<Response<List<TipoIncmueblesDto>>> ObtenerTipoVivienda(int codigo)
        {

            var response = await _proyectoRepository.ObtenerTipoViviendaProyecto(codigo);
            var mapResponse = _mapper.Map<List<TipoIncmueblesDto>>(response);
            if (mapResponse.Count > 0)
            {
                return new Response<List<TipoIncmueblesDto>> { Data = mapResponse, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<List<TipoIncmueblesDto>> { Data = null, IsSuccess = false, ReturnMessage = "No hay data" };
        }
        public async Task<Response<string>> CrearImuebleProyecto(InmublesProyectoDto inmuebles)
        {
            if (inmuebles.inmuebles.Count > 0)
            {
                foreach (var item in inmuebles.inmuebles)
                {
                    var mapInmuebles = _mapper.Map<InmublesProyecto>(item);
                    var response = await _proyectoRepository.CrearTipoInmuebleProyecto(mapInmuebles);
                }
            }
            if (inmuebles.claseInmuebles.Count > 0)
            {

                foreach (var item in inmuebles.claseInmuebles)
                {
                    var mapClase = _mapper.Map<ClaseInmueble>(item);
                    var responseInmueble = await _proyectoRepository.CrearClaseInmueble(mapClase);
                }
            }
            if (inmuebles.listaAvances.Count > 0)
            {
                foreach (var item in inmuebles.listaAvances)
                {
                    var mapClase = _mapper.Map<ListaAvance>(item);
                    var responseInmueble = await _proyectoRepository.CrearListaAvance(mapClase);
                }
            }
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };
        }
        public async Task<Response<string>> ActualizarEstadoProyecto(ActualizarProyectoDto polizas)
        {
            var response = await _proyectoRepository.ActualizarEstadoProyectoContrato(polizas.ProyectoId, polizas.CodigoProyecto);
            if (response > 0)
            {
                return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };

            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
        public async Task<Response<string>> GuardaInformacionSupervisorTecnico(List<SupervicionTecnicaDto> request)
        {
            var requetMap = _mapper.Map<List<SupervicionTecnica>>(request);
            var itemProcess = new List<int>();
            foreach (var item in requetMap)
            {
                var response = await _proyectoRepository.GuardaInformacionSupervisorTecnico(item);
                itemProcess.Add(response);  
            }
            if (itemProcess.Count > 0)
            {
                return new Response<string> { Data = "Exitoso", IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "No hay item" };
        }
        public async Task<Response<List<SupervicionTecnicaDto>>> ObtenerGuardaInformacionSupervisorTecnico(Guid request)
        {
            var response = await _proyectoRepository.ObtenerGuardaInformacionSupervisorTecnico(request);
            var requetMap = _mapper.Map<List<SupervicionTecnicaDto>>(response);
            if (requetMap.Count > 0)
            {

                return new Response<List<SupervicionTecnicaDto>> { Data = requetMap, IsSuccess = true, ReturnMessage = "OK" };
            }
            return new Response<List<SupervicionTecnicaDto>> { Data = null, IsSuccess = false, ReturnMessage = "No hay item" };

        }
        public async Task<Response<string>> CrearAdjudicacionVivienda(AdjudicacionViviendaDto adjudicacion)
        {
            var responseMap = _mapper.Map<AdjudicacionProyecto>(adjudicacion.adjudicacion);
            if (responseMap != null)
            {
                var response = await _proyectoRepository.CrearAdjudicacionProyecto(responseMap);
                var mapLogistica = _mapper.Map<LogisticaAdjudicacion>(adjudicacion.logistica);
                mapLogistica.AdjudicacionProyecto = response;
                var mapEntregaVivienda = _mapper.Map<EntregaViviendaAdjudicacion>(adjudicacion.EntregaVivienda);
                mapEntregaVivienda.AdjudicacionProyecto = response;
                var responseLogistica = await _proyectoRepository.CrearLogisticaAdjudicacion(mapLogistica);
                var responseEntrega = await _proyectoRepository.CrearEntregaViviendaAdjudicacion(mapEntregaVivienda);
                if (responseEntrega > 0)
                {

                    return new Response<string> { Data = "OK", IsSuccess = true, ReturnMessage = "OK" };
                }
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Error" };
        }
    }
}


