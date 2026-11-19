using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Dto.Catalogs.ComprobantesCartera;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categoria = GA2.Domain.Entities.Categoria;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Catalogs Core Negocio
    /// <author>Edgar Andrés Riaño</author>
    /// <date>8/03/2021</date>
    /// </summary>
    public class CatalogosBusinessLogic : ICatalogosBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly ICatalogosRepository _catalogsRepository;

        public CatalogosBusinessLogic(IMapper mapper, ICatalogosRepository catalogsRepository)
        {
            _mapper = mapper;
            _catalogsRepository = catalogsRepository;
        }
        /// <summary>
        /// Obtener vatalogos
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<CatalogoDto>>> ObtenerCatalogosAsync()
        {
            IEnumerable<CatalogoDto> catalogos = this._mapper.Map<IEnumerable<Catalogo>, IEnumerable<CatalogoDto>>(await this._catalogsRepository.ObtenerCatalogos());
            return new Response<IEnumerable<CatalogoDto>> { Data = catalogos };
        }

        /// <summary>
        /// Obtener valores catalogos
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<CatalogoValorDto>>> ObtenerValoresCatalogos()
        {
            IEnumerable<CatalogoValorDto> catalogoValores = this._mapper.Map<IEnumerable<CatalogoValorDto>>(await this._catalogsRepository.ObtenerValoresCatalogos());
            return new Response<IEnumerable<CatalogoValorDto>> { Data = catalogoValores };
        }

        public async Task<Response<IEnumerable<CatalogoValorDto>>> ObtenerValoresCatalogosPorIdCatalogo(int catalogoId)
        {
            IEnumerable<CatalogoValorDto> catalogoValores = this._mapper.Map<IEnumerable<CatalogoValorDto>>(await this._catalogsRepository.ObtenerValoresCatalogosPorIdCatalogo(catalogoId));
            return new Response<IEnumerable<CatalogoValorDto>> { Data = catalogoValores };
        }

        public async Task<Response<IEnumerable<CatalogoValorDto>>> ObtenerValoresCatalogoPorNombre(String nombreCatalogo)
        {
            IEnumerable<CatalogoValorDto> catalogoValores = this._mapper.Map<IEnumerable<CatalogoValorDto>>(await this._catalogsRepository.ObtenerValoresCatalogoPorNombre(nombreCatalogo));
            return new Response<IEnumerable<CatalogoValorDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener vcategorias
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<CategoriaDto>>> ObtenerCategorias()
        {
            IEnumerable<CategoriaDto> categorias = this._mapper.Map<IEnumerable<CategoriaDto>>(await this._catalogsRepository.ObtenerCategorias());
            return new Response<IEnumerable<CategoriaDto>> { Data = categorias };
        }

        public async Task<Response<string>> ObtenerIpLocal()
        {
            string ip = _catalogsRepository.ObtenerIpLocal();
            return new Response<string> { Data = ip };
        }

        /// <summary>
        /// Obtener ciudades
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<CiudadDto>>> ObtenerCiudades()
        {
            IEnumerable<CiudadDto> ciudades = this._mapper.Map<IEnumerable<CiudadDto>>(await this._catalogsRepository.ObtenerCiudades());
            return new Response<IEnumerable<CiudadDto>> { Data = ciudades };
        }

        /// <summary>
        /// Obtener ciudades por departamento
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<CiudadDto>>> ObtenerCiudadesPorDepartamento(int Id)
        {
            IEnumerable<CiudadDto> catalogoValores = this._mapper.Map<IEnumerable<CiudadDto>>(await this._catalogsRepository.ObtenerCiudadesPorDepartamento(Id));
            return new Response<IEnumerable<CiudadDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener paises 
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<PaisDto>>> ObtenerPaises()
        {
            IEnumerable<PaisDto> paises = this._mapper.Map<IEnumerable<PaisDto>>(await this._catalogsRepository.ObtenerPaises());
            return new Response<IEnumerable<PaisDto>> { Data = paises };
        }

        /// <summary>
        /// Obtener departamento 
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<DepartamentoDto>>> ObtenerDepartamentos()
        {
            IEnumerable<DepartamentoDto> departamentos = this._mapper.Map<IEnumerable<DepartamentoDto>>(await this._catalogsRepository.ObtenerDepartamentos());
            return new Response<IEnumerable<DepartamentoDto>> { Data = departamentos };
        }

        /// <summary>
        /// Obtener unidades ejecutoras
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<UnidadEjecutoraDto>>> ObtenerUnidadesEjecutoras()
        {
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutoras());
            return new Response<IEnumerable<UnidadEjecutoraDto>> { Data = unidadesEjecutoras };
        }


        public async Task<Response<IEnumerable<UnidadEjecutoraDto>>> ObtenerUnidadesEjecutorasPorUsuario(Guid usuarioID)
        {
            IEnumerable<UnidadEjecutoraDto> unidadesEjecutoras = this._mapper.Map<IEnumerable<UnidadEjecutoraDto>>(await this._catalogsRepository.ObtenerUnidadesEjecutorasPorUsuario(usuarioID));
            return new Response<IEnumerable<UnidadEjecutoraDto>> { Data = unidadesEjecutoras };
        }

        /// <summary>
        /// Obtener fuerzas
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<FuerzasDto>>> ObtenerFuerzas()
        {
            IEnumerable<FuerzasDto> fuerzas = this._mapper.Map<IEnumerable<FuerzasDto>>(await this._catalogsRepository.ObtenerFuerzas());
            return new Response<IEnumerable<FuerzasDto>> { Data = fuerzas };
        }

        /// <summary>
        /// Obtener grados
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<GradoDto>>> ObtenerGrados()
        {
            IEnumerable<GradoDto> grados = this._mapper.Map<IEnumerable<GradoDto>>(await this._catalogsRepository.ObtenerGrados());
            return new Response<IEnumerable<GradoDto>> { Data = grados };
        }
        public async Task<Response<IEnumerable<GradoDto>>> ObtenerGradosPorFuerzaCategoria(GradoDto grado)
        {
            var gradoEnt = this._mapper.Map<Entities.Grado>(grado);
            IEnumerable<GradoDto> grados = this._mapper.Map<IEnumerable<GradoDto>>(await this._catalogsRepository.ObtenerGradosPorFuerzaCategoria(gradoEnt));
            return new Response<IEnumerable<GradoDto>> { Data = grados };
        }
        public async Task<Response<IEnumerable<ConceptoNominaDto>>> ObtenerConceptoNominaCat(ConceptoNominaDto concepto)
        {
            if (concepto.Salario == null) concepto.Salario = "0";
            ConceptoNomina conceptoEnt = this._mapper.Map<ConceptoNomina>(concepto);
            IEnumerable<ConceptoNominaDto> conceptos = this._mapper.Map<IEnumerable<ConceptoNominaDto>>(await this._catalogsRepository.ObtenerConceptoNominaCat(conceptoEnt));
            return new Response<IEnumerable<ConceptoNominaDto>> { Data = conceptos };
        }

        /// <summary>
        /// Obtener tipos de identificacion
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<TipoIdentificacionDto>>> ObtenerTiposIdentificacion()
        {
            IEnumerable<TipoIdentificacionDto> tiposIdentificacion = this._mapper.Map<IEnumerable<TipoIdentificacionDto>>(await this._catalogsRepository.ObtenerTiposIdentificacion());
            return new Response<IEnumerable<TipoIdentificacionDto>> { Data = tiposIdentificacion };
        }

        /// <summary>
        /// Obtener tipos de vivienda
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<TipoViviendaDto>>> ObtenerTiposDeVivienda()
        {
            IEnumerable<TipoViviendaDto> tiposVivienda = this._mapper.Map<IEnumerable<TipoViviendaDto>>(await this._catalogsRepository.ObtenerTiposDeVivienda());
            return new Response<IEnumerable<TipoViviendaDto>> { Data = tiposVivienda };
        }

        /// <summary>
        /// Obtener sexos
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<SexoDto>>> ObtenerTipoDeSexo()
        {
            var a = await this._catalogsRepository.ObtenerTipoDeSexo();
            IEnumerable<SexoDto> sexoTypes = this._mapper.Map<IEnumerable<SexoDto>>(a);
            return new Response<IEnumerable<SexoDto>> { Data = sexoTypes };
        }

        /// <summary>
        /// Obtener estados civiles
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<EstadoCivilDto>>> ObtenerEstadosCiviles()
        {
            IEnumerable<EstadoCivilDto> estadoCivilTypes = this._mapper.Map<IEnumerable<EstadoCivilDto>>(await this._catalogsRepository.ObtenerEstadosCiviles());
            return new Response<IEnumerable<EstadoCivilDto>> { Data = estadoCivilTypes };
        }

        /// <summary>
        /// Obtener actividades economicas
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<ActividadEconomicaDto>>> ObtenerActividadesEconomicas()
        {
            var lista = await this._catalogsRepository.ObtenerActividadesEconomicas();
            IEnumerable<ActividadEconomicaDto> actividadEconomica = this._mapper.Map<IEnumerable<ActividadEconomicaDto>>(lista);
            return new Response<IEnumerable<ActividadEconomicaDto>> { Data = actividadEconomica };
        }

        /// <summary>
        /// Obtener profesiones
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<ProfesionDto>>> ObtenerProfesiones()
        {
            IEnumerable<ProfesionDto> profesiones = this._mapper.Map<IEnumerable<ProfesionDto>>(await this._catalogsRepository.ObtenerProfesiones());
            return new Response<IEnumerable<ProfesionDto>> { Data = profesiones };
        }

        /// <summary>
        /// Obtener direcciones abecedarios 
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<AbecedarioDto>>> ObtenerDireccionesAbecedario()
        {
            IEnumerable<AbecedarioDto> catalogoValores = this._mapper.Map<IEnumerable<AbecedarioDto>>(await this._catalogsRepository.ObtenerDireccionesAbecedario());
            return new Response<IEnumerable<AbecedarioDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener direcciones cardinal 
        /// </summary>
        /// <returns>Catalogo direcciones cardinales</returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        public async Task<Response<IEnumerable<CardinalidadDto>>> ObtenerDireccionesCardinal()
        {
            IEnumerable<CardinalidadDto> catalogoValores = this._mapper.Map<IEnumerable<CardinalidadDto>>(await this._catalogsRepository.ObtenerDireccionesCardinal());
            return new Response<IEnumerable<CardinalidadDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener direcciones bis 
        /// </summary>
        /// <returns>Catalogo direcciones cardinales</returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        public async Task<Response<IEnumerable<BisDto>>> ObtenerDireccionesBis()
        {
            IEnumerable<BisDto> catalogoValores = this._mapper.Map<IEnumerable<BisDto>>(await this._catalogsRepository.ObtenerDireccionesBis());
            return new Response<IEnumerable<BisDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener direcciones tipos  
        /// </summary>
        /// <returns>Catalogo direcciones tipos</returns>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        public async Task<Response<IEnumerable<TipoCalleDto>>> ObtenerDireccionesTipos()
        {
            IEnumerable<TipoCalleDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCalleDto>>(await this._catalogsRepository.ObtenerDireccionesTipos());
            return new Response<IEnumerable<TipoCalleDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtiene los tipos de crédito
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoCreditoDto>>> ObtenerTipoCredito()
        {
            IEnumerable<TipoCreditoDto> catalogoValores = this._mapper.Map<IEnumerable<TipoCreditoDto>>(await this._catalogsRepository.ObtenerTipoCredito());
            return new Response<IEnumerable<TipoCreditoDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtiene los tipos de vivienda
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TiViviendaDto>>> ObtenerTipoVivienda()
        {
            IEnumerable<TiViviendaDto> catalogoValores = this._mapper.Map<IEnumerable<TiViviendaDto>>(await this._catalogsRepository.ObtenerTipoVivienda());
            return new Response<IEnumerable<TiViviendaDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtiene los estados vivienda
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EstadoViviendaDto>>> ObtenerEstadoVivienda()
        {
            IEnumerable<EstadoViviendaDto> catalogoValores = this._mapper.Map<IEnumerable<EstadoViviendaDto>>(await this._catalogsRepository.ObtenerEstadoVivienda());
            return new Response<IEnumerable<EstadoViviendaDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Logica de negocio para obtener los estados de actividad
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EstadoActividadDto>>> ObtenerEstadoActividad()
        {
            IEnumerable<EstadoActividadDto> catalogoValores = this._mapper.Map<IEnumerable<EstadoActividadDto>>(await this._catalogsRepository.ObtenerEstadoActividad());
            return new Response<IEnumerable<EstadoActividadDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtiene tipos de correo
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoCorreoDto>>> ObtenerTiposCorreo()
        {
            IEnumerable<TipoCorreoDto> tiposCorreo = this._mapper.Map<IEnumerable<TipoCorreoDto>>(await this._catalogsRepository.ObtenerTiposCorreo());
            return new Response<IEnumerable<TipoCorreoDto>> { Data = tiposCorreo };


        }

        /// <summary>
        /// Obtiene tipos de telefonos
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoTelefonoDto>>> ObtenerTiposTelefono()
        {
            IEnumerable<TipoTelefonoDto> tiposTelefono = this._mapper.Map<IEnumerable<TipoTelefonoDto>>(await this._catalogsRepository.ObtenerTiposTelefono());
            return new Response<IEnumerable<TipoTelefonoDto>> { Data = tiposTelefono };
        }

        /// <summary>
        /// Obtener monedas
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<TipoMonedaDto>>> ObtenerTiposMoneda()
        {
            IEnumerable<TipoMonedaDto> tipoMonedas = this._mapper.Map<IEnumerable<TipoMonedaDto>>(await this._catalogsRepository.ObtenerTiposMoneda());
            return new Response<IEnumerable<TipoMonedaDto>> { Data = tipoMonedas };
        }

        /// <summary>
        /// Obtener monedas
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<DominiosCorreoDto>>> ObtenerDominiosCorreo()
        {
            IEnumerable<DominiosCorreoDto> tipoMonedas = this._mapper.Map<IEnumerable<DominiosCorreoDto>>(await this._catalogsRepository.ObtenerDominiosCorreo());
            return new Response<IEnumerable<DominiosCorreoDto>> { Data = tipoMonedas };
        }

        /// <summary>
        /// Obtener monedas
        /// </summary>
        /// <author>Oscar Julian Rojas</author>
        /// <date>07/04/2021</date>
        /// <returns>Catalogo direcciones abecedarios</returns>
        public async Task<Response<IEnumerable<TipoDireccionDto>>> ObtenerTiposDireccion()
        {
            IEnumerable<TipoDireccionDto> tiposDireccion = this._mapper.Map<IEnumerable<TipoDireccionDto>>(await this._catalogsRepository.ObtenerTiposDireccion());
            return new Response<IEnumerable<TipoDireccionDto>> { Data = tiposDireccion };
        }


        #region Tipo vivienda

        public async Task<Response<TipoViviendaDto>> CrearTipoVivienda(TipoViviendaDto TipoVivienda)
        {
            return new Response<TipoViviendaDto>
            {
                Data = this._mapper.Map<TipoViviendaDto>(
                    await this._catalogsRepository.CrearTipoVivienda(
                         this._mapper.Map<TipoVivienda>(TipoVivienda)))
            };
        }
        public async Task<Response<TipoViviendaDto>> ActualizarTipoVivienda(TipoViviendaDto TipoVivienda)
        {
            return new Response<TipoViviendaDto>
            {
                Data = this._mapper.Map<TipoViviendaDto>(
                    await this._catalogsRepository.ActualizarTipoVivienda(
                         this._mapper.Map<TipoVivienda>(TipoVivienda)))
            };
        }

        public async Task<Response<TipoViviendaDto>> EliminarTipoViviendaPorid(string TipoViviendaId)
        {
            return new Response<TipoViviendaDto>
            {
                Data = this._mapper.Map<TipoViviendaDto>(
                    await this._catalogsRepository.EliminarTipoViviendaPorid(TipoViviendaId))
            };
        }
        #endregion

        #region Tipo Credito
        public async Task<Response<TipoCreditoDto>> CrearTipoCredito(TipoCreditoDto TipoCredito)
        {
            return new Response<TipoCreditoDto>
            {
                Data = this._mapper.Map<TipoCreditoDto>(
                    await this._catalogsRepository.CrearTipoCredito(
                         this._mapper.Map<TipoCredito>(TipoCredito)))
            };
        }

        public async Task<Response<bool>> RegistrarSelFuerzaUnidadUsuario(SelFuerzaUnidadUsuarioDto SelFuerzaUnidadUsuarioDto)
        {
            return new Response<bool>
            {
                Data = await this._catalogsRepository.RegistrarSelFuerzaUnidadUsuario(this._mapper.Map<SLFZUN_FUERZA_UNIDAD_USUARIO>(SelFuerzaUnidadUsuarioDto))
            };
        }

        public async Task<Response<bool>> ActualizarSelFuerzaUnidadUsuario(SelFuerzaUnidadUsuarioDto SelFuerzaUnidadUsuarioDto)
        {
            return new Response<bool>
            {
                Data = await this._catalogsRepository.ActualizarSelFuerzaUnidadUsuario(this._mapper.Map<SLFZUN_FUERZA_UNIDAD_USUARIO>(SelFuerzaUnidadUsuarioDto))
            };
        }

        public async Task<Response<SelFuerzaUnidadUsuarioDto>> ObtenerSelFuerzaUnidadUsuario(string usuarioId)
        {
            return new Response<SelFuerzaUnidadUsuarioDto>
            {
                Data = this._mapper.Map<SelFuerzaUnidadUsuarioDto>(await this._catalogsRepository.ObtenerSelFuerzaUnidadUsuario(usuarioId))
            };
        }

        public async Task<Response<TipoCreditoDto>> ActualizarTipoCredito(TipoCreditoDto TipoCredito)
        {
            return new Response<TipoCreditoDto>
            {
                Data = this._mapper.Map<TipoCreditoDto>(
                    await this._catalogsRepository.ActualizarTipoCredito(
                         this._mapper.Map<TipoCredito>(TipoCredito)))
            };
        }
        public async Task<Response<TipoCreditoDto>> EliminarTipoCreditoPorid(string TipoCreditoId)
        {
            return new Response<TipoCreditoDto>
            {
                Data = this._mapper.Map<TipoCreditoDto>(
                    await this._catalogsRepository.EliminarTipoCreditoPorid(TipoCreditoId))
            };
        }

        #endregion

        /// <summary>
        /// Obtiene los tipos de abono
        /// </summary>
        /// <returns></returns>
        /// <Author>Nicolas Florez Sarrazola</Author>
        /// <date>09/09/2021</date>
        public async Task<Response<IEnumerable<TipoAbonoDto>>> ObtenerTipoAbono()
        {
            return new Response<IEnumerable<TipoAbonoDto>> { Data = this._mapper.Map<IEnumerable<TipoAbonoDto>>(await this._catalogsRepository.ObtenerTipoAbono()) };
        }

        /// <summary>
        /// Obtiene los tipos de subsidio
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoSubsidioDto>>> ObtenerTipoSubsidio()
        {
            return new Response<IEnumerable<TipoSubsidioDto>> { Data = this._mapper.Map<IEnumerable<TipoSubsidioDto>>(await this._catalogsRepository.ObtenerTipoSubsidio()) };
        }

        /// <summary>
        /// Obtiene las opciones del abono unico
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<AbonoUnicoDto>>> ObtenerAbonoUnico()
        {
            return new Response<IEnumerable<AbonoUnicoDto>> { Data = this._mapper.Map<IEnumerable<AbonoUnicoDto>>(await this._catalogsRepository.ObtenerAbonoUnico()) };
        }


        /// <summary>
        /// Obtiene los tipos de Solicitud
        /// <paramref name="IdTipoActor">Filtro tipo actor</paramref>
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoSolicitudDto>>> ObtenerTiposSolicitud(int IdTipoActor)
        {
            return new Response<IEnumerable<TipoSolicitudDto>> { Data = this._mapper.Map<IEnumerable<TipoSolicitudDto>>(await this._catalogsRepository.ObtenerTipoSolicitud(IdTipoActor)) };
        }


        /// <summary>
        /// Obtener tipo de modalidad
        /// </summary>
        /// <param name="IdTipoSolicitud"></param>
        /// <returns>Lista de tipos de modalidad</returns>
        public async Task<Response<IEnumerable<TipoModalidadDto>>> ObtenerTiposModalidad(string IdTipoSolicitud)
        {
            return new Response<IEnumerable<TipoModalidadDto>> { Data = this._mapper.Map<IEnumerable<TipoModalidadDto>>(await this._catalogsRepository.ObtenerTipoModalidad(IdTipoSolicitud)) };
        }


        /// <summary>
        /// Obtener tipo de modalidad
        /// </summary>
        /// <param name="IdTipoModalidad"></param>
        /// <returns>Lista de tipos de modalidad</returns>
        public async Task<Response<IEnumerable<TipoSubModalidadDto>>> ObtenerTiposSubModalidad(string IdTipoModalidad)
        {
            return new Response<IEnumerable<TipoSubModalidadDto>> { Data = this._mapper.Map<IEnumerable<TipoSubModalidadDto>>(await this._catalogsRepository.ObtenerTipoSubModalidad(IdTipoModalidad)) };
        }

        public async Task<Response<IEnumerable<EstadoCarteraDto>>> ObtenerEstadoCartera()
        {
            return new Response<IEnumerable<EstadoCarteraDto>> { Data = this._mapper.Map<IEnumerable<EstadoCarteraDto>>(await this._catalogsRepository.ObtenerEstadoCartera()) };
        }
        /// <summary>
        /// Metodo para obtener los estados de cuenta
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>>> ObtenerEctEstadoCuenta()
        {
            return new Response<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>> { Data = this._mapper.Map<IEnumerable<GA2.Application.Dto.Catalogs.EstadoCuentaDto>>(await this._catalogsRepository.ObtenerEctEstadoCuenta()) };
        }
        /// <summary>
        /// Metodo para obtener las categorias de las tablas de parametros 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<CategoriaTablaParametroDto>>> ObtenerCategoriasTablasParametros()
        {
            return new Response<IEnumerable<CategoriaTablaParametroDto>> { Data = this._mapper.Map<IEnumerable<CategoriaTablaParametroDto>>(await this._catalogsRepository.ObtenerCategoriasTablasParametros()) };
        }

        /// <summary>
        /// Metodo para obtener las datos de las tablas de parametros 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<dynamic>>> ObtenerDatosTablasParametros(string nombreTabla)
        {
            return new Response<IEnumerable<dynamic>> { Data = await this._catalogsRepository.ObtenerDatosTablasParametros(nombreTabla) };
        }

        /// <summary>
        /// Metodo para obtener esquema de las tablas de parametros 
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EsquemaTablaParametroDto>>> ObtenerEsquemasTablasParametros(string nombreTabla)
        {
            return new Response<IEnumerable<EsquemaTablaParametroDto>> { Data = this._mapper.Map<IEnumerable<EsquemaTablaParametroDto>>(await this._catalogsRepository.ObtenerEsquemasTablasParametros(nombreTabla)) };
        }

        /// <summary>
        /// Metodo para obtener los tipos de cuenta
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoCuentaDto>>> ObtenerTipoCuenta()
        {
            return new Response<IEnumerable<TipoCuentaDto>> { Data = this._mapper.Map<IEnumerable<TipoCuentaDto>>(await this._catalogsRepository.ObtenerTipoCuenta()) };
        }

        /// <summary>
        /// Metodo para obtener los tipos de cuenta
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EntidadBancariaDto>>> ObtenerEntidadBancaria()
        {
            return new Response<IEnumerable<EntidadBancariaDto>> { Data = this._mapper.Map<IEnumerable<EntidadBancariaDto>>(await this._catalogsRepository.ObtenerEntidadBancaria()) };
        }

        public async Task<Response<IEnumerable<FormatoDto>>> ObtenerFormato()
        {
            return new Response<IEnumerable<FormatoDto>> { Data = this._mapper.Map<IEnumerable<FormatoDto>>(await this._catalogsRepository.ObtenerFormato()) };
        }

        public async Task<Response<IEnumerable<MedioDeEnvioDto>>> ObtenerMediosDeEnvio()
        {
            return new Response<IEnumerable<MedioDeEnvioDto>> { Data = this._mapper.Map<IEnumerable<MedioDeEnvioDto>>(await this._catalogsRepository.ObtenerMediosDeEnvio()) };
        }

        #region EntidadEducativa

        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativa()
        {
            var arespuestaEntidades = await this._catalogsRepository.ObtenerEntidadEducativa();
            IEnumerable<EntidadEducativaDto> entidadEducativaTypes = this._mapper.Map<IEnumerable<EntidadEducativaDto>>(arespuestaEntidades);
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidadEducativaTypes };
        }

        /// <summary>
        /// Obtener entidades educativas
        /// </summary>
        /// <param name="entidadEducativa"></param>
        /// <author>Hanson Restrepo</author>
        /// <date26/01/2022</date>
        /// <returns>Catalogo direcciones abecedarios</returns>        
        public async Task<Response<IEnumerable<EntidadEducativaDto>>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativaDto entidadEducativa)
        {
            var entidadEnt = _mapper.Map<EntidadEducativa>(entidadEducativa);
            IEnumerable<EntidadEducativaDto> entidad = _mapper.Map<IEnumerable<EntidadEducativaDto>>(await _catalogsRepository.ObtenerEntidadEducativaPorNombreNit(entidadEnt));
            return new Response<IEnumerable<EntidadEducativaDto>> { Data = entidad };
        }

        /// <summary>
        /// CrearEntidadEducativa
        /// </summary>
        /// <param name="EntidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<EntidadEducativaDto>> CrearEntidadEducativa(EntidadEducativaDto EntidadEducativa)
        {
            return new Response<EntidadEducativaDto>
            {
                Data = _mapper.Map<EntidadEducativaDto>(
                    await _catalogsRepository.CrearEntidadEducativa(
                         _mapper.Map<EntidadEducativa>(EntidadEducativa)))
            };
        }

        public async Task<Response<EntidadEducativaDto>> ActualizarEntidadEducativa(EntidadEducativaDto EntidadEducativa)
        {
            return new Response<EntidadEducativaDto>
            {
                Data = _mapper.Map<EntidadEducativaDto>(
                    await _catalogsRepository.ActualizarEntidadEducativa(
                         _mapper.Map<EntidadEducativa>(EntidadEducativa)))
            };
        }

        /// <summary>
        /// EliminarEntidadEducativaPorId
        /// </summary>
        /// <param name="IdEntidadEducativa"></param>
        /// <returns></returns>
        public async Task<Response<EntidadEducativaDto>> EliminarEntidadEducativaPorId(string idEntidadEducativa)
        {
            return new Response<EntidadEducativaDto>
            {
                Data = _mapper.Map<EntidadEducativaDto>(
                    await this._catalogsRepository.EliminarEntidadEducativaPorId(idEntidadEducativa))
            };
        }

        public async Task<Response<IEnumerable<SedeEntidadEducativaDto>>> ObtenerSedesPorEntidadEducativa(string idEntidadEducativa)
        {
            IEnumerable<SedeEntidadEducativaDto> sedes = _mapper.Map<IEnumerable<SedeEntidadEducativaDto>>(await _catalogsRepository.ObtenerSedesPorEntidadEducativa(idEntidadEducativa));
            return new Response<IEnumerable<SedeEntidadEducativaDto>> { Data = sedes };

        }

        public async Task<Response<SedeEntidadEducativaDto>> CrearSedeEntidadEducativa(SedeEntidadEducativaDto sedeEntidadEducativa)
        {
            return new Response<SedeEntidadEducativaDto>
            {
                Data = _mapper.Map<SedeEntidadEducativaDto>(
                    await _catalogsRepository.CrearSedeEntidadEducativa(
                         _mapper.Map<SedeEntidadEducativa>(sedeEntidadEducativa)))
            };
        }

        public async Task<Response<SedeEntidadEducativaDto>> EliminarSedesEntidadEducativaPorId(string idSedeEntidadEducativa)
        {
            return new Response<SedeEntidadEducativaDto>
            {
                Data = _mapper.Map<SedeEntidadEducativaDto>(
                    await this._catalogsRepository.EliminarSedeEntidadEducativaPorId(idSedeEntidadEducativa))
            };
        }

        public async Task<Response<BloqueoEntidadEducativaDto>> BloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativaDto)
        {
            return new Response<BloqueoEntidadEducativaDto>
            {
                Data = _mapper.Map<BloqueoEntidadEducativaDto>(
                    await _catalogsRepository.BloqueoEntidadEducativa(
                         _mapper.Map<BloqueoEntidadEducativa>(bloqueoEntidadEducativaDto)))
            };
        }

        public async Task<Response<BloqueoEntidadEducativaDto>> DesbloqueoEntidadEducativa(BloqueoEntidadEducativaDto bloqueoEntidadEducativaDto)
        {
            return new Response<BloqueoEntidadEducativaDto>
            {
                Data = _mapper.Map<BloqueoEntidadEducativaDto>(
                    await _catalogsRepository.DesbloqueoEntidadEducativa(
                         _mapper.Map<BloqueoEntidadEducativa>(bloqueoEntidadEducativaDto)))
            };
        }

        public async Task<Response<IEnumerable<BloqueoEntidadEducativaDto>>> ObtenerBloqueoEntidadEducativa(string idEntidadEducativa)
        {
            IEnumerable<BloqueoEntidadEducativaDto> bloqueo = _mapper.Map<IEnumerable<BloqueoEntidadEducativaDto>>(await _catalogsRepository.ObtenerBloqueoEntidadEducativa(idEntidadEducativa));
            return new Response<IEnumerable<BloqueoEntidadEducativaDto>> { Data = bloqueo };
        }

        public async Task<Response<CuentaBancariaEntidadEducativaDto>> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativa)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await _catalogsRepository.CrearCuentaBancariaEntidadEducativa(
                         _mapper.Map<CuentaBancariaEntidadEducativa>(cuentaBancariaEntidadEducativa)))
            };
        }

        public async Task<Response<CuentaBancariaEntidadEducativaDto>> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativaDto cuentaBancariaEntidadEducativaDto)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await _catalogsRepository.ActualizarCuentaBancariaEntidadEducativa(
                         _mapper.Map<CuentaBancariaEntidadEducativa>(cuentaBancariaEntidadEducativaDto)))
            };
        }

        public async Task<Response<IEnumerable<CuentaBancariaEntidadEducativaDto>>> ObtenerCuentaBancariaPorEntidad(Guid idEntidad)
        {
            IEnumerable<CuentaBancariaEntidadEducativaDto> cuenta = _mapper.Map<IEnumerable<CuentaBancariaEntidadEducativaDto>>(await _catalogsRepository.ObtenerCuentaBancariaPorEntidad(idEntidad));
            return new Response<IEnumerable<CuentaBancariaEntidadEducativaDto>> { Data = cuenta };
        }
        public async Task<Response<CuentaBancariaEntidadEducativaDto>> EliminarCuentaBancariaEntidadEducativa(Guid idCuenta)
        {
            return new Response<CuentaBancariaEntidadEducativaDto>
            {
                Data = _mapper.Map<CuentaBancariaEntidadEducativaDto>(
                    await this._catalogsRepository.EliminarCuentaBancariaEntidadEducativa(idCuenta))
            };
        }


        #endregion EntidadEducativa

        /// <summary>
        /// Obtener programas educativos
        /// </summary>
        /// <param name="idEntidad"></param>
        /// <returns>Lista de programas educativos</returns>
        public async Task<Response<IEnumerable<ProgramaEducativoDto>>> ObtenerProgramaEducativo(Guid idEntidad)
        {
            return new Response<IEnumerable<ProgramaEducativoDto>> { Data = this._mapper.Map<IEnumerable<ProgramaEducativoDto>>(await this._catalogsRepository.ObtenerProgramaEducativo(idEntidad)) };
        }

        /// <summary>
        /// Obtener niveles educativos
        /// </summary>
        /// <param name="idPrograma"></param>
        /// <returns>Lista de niveles educativos</returns>
        [Obsolete("ObtenerNivelEducativoCesantias is deprecated, please use ObtenerNivelEducativoPorPrograma instead.")]
        public async Task<Response<IEnumerable<NivelEducativoDto>>> ObtenerNivelEducativoCesantias(Guid idPrograma)
        {
            return new Response<IEnumerable<NivelEducativoDto>> { Data = this._mapper.Map<IEnumerable<NivelEducativoDto>>(await this._catalogsRepository.ObtenerNivelEducativoCesantias(idPrograma)) };
        }

        public async Task<Response<IEnumerable<NivelEducativoDto>>> ObtenerNivelEducativo()
        {
            return new Response<IEnumerable<NivelEducativoDto>> { Data = this._mapper.Map<IEnumerable<NivelEducativoDto>>(await this._catalogsRepository.ObtenerNivelEducativo()) };
        }

        public async Task<Response<IEnumerable<EntidadSeguroEducativoDto>>> ObtenerEntidadSeguroEducativo()
        {
            return new Response<IEnumerable<EntidadSeguroEducativoDto>> { Data = this._mapper.Map<IEnumerable<EntidadSeguroEducativoDto>>(await this._catalogsRepository.ObtenerEntidadSeguroEducativo()) };
        }
        public async Task<Response<EntidadSeguroEducativoDto>> InsertarEntidadSeguroEducativo(EntidadSeguroEducativoDto request)
        {
            var data = _mapper.Map<EntidadSeguroEducativo>(request);

            return new Response<EntidadSeguroEducativoDto>
            {
                Data = _mapper.Map<EntidadSeguroEducativoDto>(
                    await _catalogsRepository.InsertarEntidadSeguroEducativo(data)
                )
            };
        }

        public async Task<Response<IEnumerable<ListarProgramaEducativoDto>>> ObtenerProgramaEducativoPorEntidad(Guid idEntidad)
        {
            return new Response<IEnumerable<ListarProgramaEducativoDto>> { Data = this._mapper.Map<IEnumerable<ListarProgramaEducativoDto>>(await _catalogsRepository.ObtenerProgramaEducativoPorEntidad(idEntidad)) };
        }
        public async Task<Response<ProgramaEducativoDto>> CrearProgramaEducativo(ProgramaEducativoDto programaEducativo)
        {
            return new Response<ProgramaEducativoDto>
            {
                Data = _mapper.Map<ProgramaEducativoDto>(
             await _catalogsRepository.CrearProgramaEducativo(
                  _mapper.Map<ProgramaEducativo>(programaEducativo)))
            };
        }
        public async Task<Response<ProgramaEducativoDto>> EliminarProgramaEducativoPorId(string idProgramaEducativo)
        {
            return new Response<ProgramaEducativoDto>
            {
                Data = _mapper.Map<ProgramaEducativoDto>(
                    await this._catalogsRepository.EliminarProgramaEducativoPorId(idProgramaEducativo))
            };
        }

        public async Task<Response<ProgramaEducativoDto>> ActualizarProgramaEducativo(ProgramaEducativoDto programaEducativo)
        {
            return new Response<ProgramaEducativoDto>
            {
                Data = _mapper.Map<ProgramaEducativoDto>(
                    await _catalogsRepository.ActualizarProgramaEducativo(
                         _mapper.Map<ProgramaEducativo>(programaEducativo)))
            };
        }

        /// <summary>
        /// Obtiene los tipos de documentos activos
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<TipoIdentificacionDto>>> ObtenerDocumentoTipo()
        {
            return new Response<IEnumerable<TipoIdentificacionDto>> { Data = this._mapper.Map<IEnumerable<TipoIdentificacionDto>>(await this._catalogsRepository.ObtenerDocumentoTipo()) };
        }

        /// <summary>
        /// Crear tipo de documento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<TipoIdentificacionDto>> CrearDocumentoTipo(TipoIdentificacionDto request)
        {
            var data = _mapper.Map<TipoIdentificacion>(request);
            return new Response<TipoIdentificacionDto> { Data = this._mapper.Map<TipoIdentificacionDto>(await this._catalogsRepository.CrearDocumentoTipo(data)) };
        }

        /// <summary>
        /// Actualiza tipo documento
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<TipoIdentificacionDto>> ActualizarDocumentoTipo(TipoIdentificacionDto request)
        {
            var data = _mapper.Map<TipoIdentificacion>(request);
            return new Response<TipoIdentificacionDto> { Data = this._mapper.Map<TipoIdentificacionDto>(await this._catalogsRepository.ActualizarDocumentoTipo(data)) };
        }

        /// <summary>
        /// Crea parametro fuerza
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<FuerzasDto>> CrearFuerza(FuerzasDto request)
        {
            var data = _mapper.Map<Fuerzas>(request);
            return new Response<FuerzasDto> { Data = this._mapper.Map<FuerzasDto>(await this._catalogsRepository.CrearFuerza(data)) };
        }

        /// <summary>
        /// Actualiza parametro fuerza
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<FuerzasDto>> ActualizarFuerza(FuerzasDto request)
        {
            var data = _mapper.Map<Fuerzas>(request);
            return new Response<FuerzasDto> { Data = this._mapper.Map<FuerzasDto>(await this._catalogsRepository.ActualizarFuerza(data)) };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<SexoDto>> CrearSexo(SexoDto request)
        {
            var data = _mapper.Map<CatalogoValor>(request);
            return new Response<SexoDto> { Data = this._mapper.Map<SexoDto>(await this._catalogsRepository.CrearSexo(data)) };

        }

        public async Task<Response<SexoDto>> ActualizarSexo(SexoDto request)
        {
            var data = _mapper.Map<CatalogoValor>(request);
            return new Response<SexoDto> { Data = this._mapper.Map<SexoDto>(await this._catalogsRepository.ActualizarSexo(data)) };
        }

        /// <summary>
        /// Crea una nueva categoria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<CategoriaDto>> CrearCategoria(CategoriaDto request)
        {
            var data = this._mapper.Map<Categoria>(request);
            return new Response<CategoriaDto> { Data = _mapper.Map<CategoriaDto>(await _catalogsRepository.CrearCategoria(data)) };
        }

        public async Task<ActionResult<Response<CategoriaDto>>> ActualizarCategoria(CategoriaDto request)
        {
            var data = this._mapper.Map<Categoria>(request);
            return new Response<CategoriaDto> { Data = _mapper.Map<CategoriaDto>(await _catalogsRepository.ActualizarCategoria(data)) };
        }

        public async Task<Response<IEnumerable<TipoAfiliacionDto>>> ObtenerTiposAfiliacion()
        {
            return new Response<IEnumerable<TipoAfiliacionDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoAfiliacionDto>>(await _catalogsRepository.ObtenerTiposAfiliacion())
            };
        }

        public async Task<Response<IEnumerable<EstadoAfiliacionDto>>> ObtenerEstadosAfiliacion()
        {
            return new Response<IEnumerable<EstadoAfiliacionDto>>
            {
                Data = _mapper.Map<IEnumerable<EstadoAfiliacionDto>>(await _catalogsRepository.ObtenerEstadosAfiliacion())
            };
        }

        public async Task<Response<IEnumerable<CampoCalidadDto>>> ObtenerCampoCalidadAfiliacion()
        {
            return new Response<IEnumerable<CampoCalidadDto>>
            {
                Data = _mapper.Map<IEnumerable<CampoCalidadDto>>(await _catalogsRepository.ObtenerCampoCalidadAfiliacion())
            };
        }

        public async Task<Response<IEnumerable<TipoAfiliacionProcedenteDto>>> ObtenerTipoAfiliacionProcedente()
        {
            return new Response<IEnumerable<TipoAfiliacionProcedenteDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoAfiliacionProcedenteDto>>(await _catalogsRepository.ObtenerTiposAfiliacionProcedente())
            };
        }

        public async Task<Response<IEnumerable<PorcentajeDescuentoDto>>> ObtenerPorcentajesDescuento()
        {
            return new Response<IEnumerable<PorcentajeDescuentoDto>>
            {
                Data = _mapper.Map<IEnumerable<PorcentajeDescuentoDto>>(await _catalogsRepository.ObtenerPorcentajesDescuento())
            };
        }

        public async Task<Response<PorcentajeDescuentoDto>> InsertarPorcentajeDescuento(PorcentajeDescuentoDto PorcentajeDescuento)
        {
            PorcentajeDescuento porcentajeDescuento = new();
            porcentajeDescuento.PRC_DESCRIPCION = PorcentajeDescuento.DescripcionPorcentajeDescuento;
            porcentajeDescuento.PRC_PRODUCTO = PorcentajeDescuento.ProductoDescuento;

            return new Response<PorcentajeDescuentoDto>
            {
                Data = _mapper.Map<PorcentajeDescuentoDto>(await _catalogsRepository.InsertarPorcentajeDescuento(porcentajeDescuento))
            };
        }

        public async Task<Response<IEnumerable<TipoCuentaCDto>>> ObtenerTipoCuentaC()
        {
            return new Response<IEnumerable<TipoCuentaCDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoCuentaCDto>>(await _catalogsRepository.ObtenerTipoCuentaC())
            };
        }

        public async Task<Response<IEnumerable<PersonasCargoDto>>> ObtenerPersonasCargo()
        {
            return new Response<IEnumerable<PersonasCargoDto>>
            {
                Data = _mapper.Map<IEnumerable<PersonasCargoDto>>(await _catalogsRepository.ObtenerPersonasCargo())
            };
        }

        public async Task<Response<IEnumerable<TipoContratoDto>>> ObtenerTipoContrato()
        {
            return new Response<IEnumerable<TipoContratoDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoContratoDto>>(await _catalogsRepository.ObtenerTipoContrato())
            };
        }

        public async Task<Response<IEnumerable<TipoViviendaPropiaDto>>> ObtenerTipoViviendaPropia()
        {
            return new Response<IEnumerable<TipoViviendaPropiaDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoViviendaPropiaDto>>(await _catalogsRepository.ObtenerTipoViviendaPropia())
            };
        }

        public async Task<Response<IEnumerable<EstratoDto>>> ObtenerEstrato()
        {
            return new Response<IEnumerable<EstratoDto>>
            {
                Data = _mapper.Map<IEnumerable<EstratoDto>>(await _catalogsRepository.ObtenerEstrato())
            };
        }

        public async Task<Response<IEnumerable<GrupoInconsistenciaDto>>> ListarGruposInconsistencia()
        {
            var gp = await this._catalogsRepository.ListarGruposInconsistencia();
            var result = this._mapper.Map<IEnumerable<GrupoInconsistencia>, IEnumerable<GrupoInconsistenciaDto>>(gp);

            return new Response<IEnumerable<GrupoInconsistenciaDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<TipoInconsistenciaDto>>> ObtenerInconsistencias(int GrupoInconsistencia)
        {
            return new Response<IEnumerable<TipoInconsistenciaDto>>
            {
                Data = this._mapper.Map<IEnumerable<TipoInconsistenciaDto>>(await this._catalogsRepository.ObtenerInconsistencia(GrupoInconsistencia))
            };
        }

        public async Task<Response<IEnumerable<HerramientaInconsistenciaDto>>> ObtenerHerramientaInconsistencia()
        {
            var gp = await this._catalogsRepository.ObtenerHerramientaInconsistencia();
            var result = this._mapper.Map<IEnumerable<HerramientaInconsistencia>, IEnumerable<HerramientaInconsistenciaDto>>(gp);

            return new Response<IEnumerable<HerramientaInconsistenciaDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<PuntosAtencionInconsistenciaDto>>> ObtenerPuntosAtencionInconsistencia()
        {
            var gp = await this._catalogsRepository.ObtenerPuntosAtencionInconsistencia();
            var result = this._mapper.Map<IEnumerable<PuntosAtencionInconsistencia>, IEnumerable<PuntosAtencionInconsistenciaDto>>(gp);

            return new Response<IEnumerable<PuntosAtencionInconsistenciaDto>> { Data = result };
        }

        public async Task<Response<IEnumerable<NivelAcademicoDto>>> ObtenerNivelAcademico()
        {
            return new Response<IEnumerable<NivelAcademicoDto>>
            {
                Data = _mapper.Map<IEnumerable<NivelAcademicoDto>>(await _catalogsRepository.ObtenerNivelAcademico())
            };
        }

        public async Task<Response<IEnumerable<ConceptoNominaDto>>> ObtenerConceptoNomina()
        {
            return new Response<IEnumerable<ConceptoNominaDto>>
            {
                Data = _mapper.Map<IEnumerable<ConceptoNominaDto>>(await _catalogsRepository.ObtenerConceptoNomina())
            };
        }

        public async Task<Response<IEnumerable<CausalBloqueoDto>>> ObtenerCausalBloqueo()
        {
            return new Response<IEnumerable<CausalBloqueoDto>>
            {
                Data = _mapper.Map<IEnumerable<CausalBloqueoDto>>(await _catalogsRepository.ObtenerCausalBloqueo())
            };
        }

        public async Task<Response<IEnumerable<TipoSolicitudNovedadMatriculaDto>>> ObtenerTipoSolicitudNovedadMatricula()
        {
            return new Response<IEnumerable<TipoSolicitudNovedadMatriculaDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoSolicitudNovedadMatriculaDto>>(await _catalogsRepository.ObtenerTipoSolicitudNovedadMatricula())
            };
        }

        /// <summary>
        /// Logica de negocio para obtener los estados de tareas de una solicitud
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<EstadoTareaSolicitudDto>>> ObtenerEstadoTareaSolicitud()
        {
            IEnumerable<EstadoTareaSolicitudDto> catalogoValores = this._mapper.Map<IEnumerable<EstadoTareaSolicitudDto>>(await this._catalogsRepository.ObtenerEstadoTareaSolicitud());
            return new Response<IEnumerable<EstadoTareaSolicitudDto>> { Data = catalogoValores };
        }

        public async Task<Response<IEnumerable<OficinaDeRegistroDto>>> ObtenerOficinasDeRegistro()
        {
            return new Response<IEnumerable<OficinaDeRegistroDto>>
            {
                Data = _mapper.Map<IEnumerable<OficinaDeRegistroDto>>(await _catalogsRepository.ObtenerOficinasDeRegistro())
            };
        }

        public async Task<Response<CargoCategoriaDto>> ObtenerSimulacionCargoPorCategoria(int categoriaId, int Cargo)
        {
            CargoCategoriaDto value = this._mapper.Map<CargoCategoriaDto>(await this._catalogsRepository.ObtenerSimulacionCargoPorCategoria(categoriaId, Cargo));
            return new Response<CargoCategoriaDto> { Data = value };
        }

        /// <summary>
        /// Logica de negocio para obtener los niveles educativos del postulante al fondo de solidaridad y su núcleo familiar
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<NivelAcademicoCatalogoDto>>> ObtenerNivelEducativoPostulante()
        {
            IEnumerable<NivelAcademicoCatalogoDto> catalogoValores = this._mapper.Map<IEnumerable<NivelAcademicoCatalogoDto>>(await this._catalogsRepository.ObtenerNivelEducativoPostulante());
            return new Response<IEnumerable<NivelAcademicoCatalogoDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Logica de negocio para obtener la ocupación del postulante al fondo de solidaridad y su núcleo familiar
        /// </summary>
        /// <returns></returns>
        public async Task<Response<IEnumerable<OcupacionDto>>> ObtenerOcupacionPostulante()
        {
            IEnumerable<OcupacionDto> catalogoValores = this._mapper.Map<IEnumerable<OcupacionDto>>(await this._catalogsRepository.ObtenerOcupacionPostulante());
            return new Response<IEnumerable<OcupacionDto>> { Data = catalogoValores };
        }

        public async Task<Response<IEnumerable<ModalidadAfiliacionDto>>> ObtenerModalidadAfiliacion()
        {
            IEnumerable<ModalidadAfiliacionDto> catalogoValores = this._mapper.Map<IEnumerable<ModalidadAfiliacionDto>>(await this._catalogsRepository.ObtenerModalidadAfiliacion());
            return new Response<IEnumerable<ModalidadAfiliacionDto>> { Data = catalogoValores };
        }

        /// <summary>
        /// Obtener estados documento
        /// </summary>
        /// <returns>Lista de estados documento</returns>
        public async Task<Response<IEnumerable<EstadoDocumentoDto>>> ObtenerEstadosDocumento()
        {
            return new Response<IEnumerable<EstadoDocumentoDto>> {
                Data = _mapper.Map<IEnumerable<EstadoDocumentoDto>>(await this._catalogsRepository.ObtenerEstadosDocumento())
            };
        }

        public async Task<Response<IEnumerable<TipoSolicitudDto>>> ObtenerTipoSolicitudDesafiliacion()
        {
            return new Response<IEnumerable<TipoSolicitudDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoSolicitudDto>>(await this._catalogsRepository.ObtenerTipoSolicitudDesafiliacion())
            };
        }

        public async Task<Response<IEnumerable<FormatoAplicarDto>>> ObtenerFormatoAAplicar()
        {
            return new Response<IEnumerable<FormatoAplicarDto>>
            {
                Data = _mapper.Map<IEnumerable<FormatoAplicarDto>>(await this._catalogsRepository.ObtenerFormatoAAplicar())
            };
        }

        public async Task<Response<IEnumerable<CausalesBloqueoCuentaDto>>> ObtenerCausalesBloqueoCuenta()
        {
            return new Response<IEnumerable<CausalesBloqueoCuentaDto>>
            {
                Data = _mapper.Map<IEnumerable<CausalesBloqueoCuentaDto>>(await this._catalogsRepository.ObtenerCausalesBloqueoCuenta())
            };
        }

        public async Task<Response<IEnumerable<ConceptoDto>>> ObtenerConceptos()
        {
            return new Response<IEnumerable<ConceptoDto>>
            {
                Data = _mapper.Map<IEnumerable<ConceptoDto>>(await this._catalogsRepository.ObtenerConceptos())
            };
        }

        public async Task<Response<IEnumerable<ConceptoNovedadDto>>> ObtenerConceptosNovedades()
        {
            return new Response<IEnumerable<ConceptoNovedadDto>>
            {
                Data = _mapper.Map<IEnumerable<ConceptoNovedadDto>>(await this._catalogsRepository.ObtenerConceptosNovedades())
            };
        }

        public async Task<Response<bool>> EliminarTiposIdentificacion(int Id)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = await _catalogsRepository.EliminarTiposIdentificacion(Id);
            respuesta.IsSuccess = true;
            return respuesta;
        }

        public async Task<Response<bool>> EliminarTipoDeSexo(int Id)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = await _catalogsRepository.EliminarTipoDeSexo(Id);
            respuesta.IsSuccess = true;
            return respuesta;
        }

        public async Task<Response<bool>> EliminarFuerzas(int Id)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = await _catalogsRepository.EliminarFuerzas(Id);
            respuesta.IsSuccess = true;
            return respuesta;
        }

        public async Task<Response<bool>> EliminarCategorias(int Id)
        {
            Response<bool> respuesta = new Response<bool>();
            respuesta.Data = await _catalogsRepository.EliminarCategorias(Id);
            respuesta.IsSuccess = true;
            return respuesta;
        }

        public async Task<Response<IEnumerable<TipoActorEmbargoDto>>> ObtenerTipoDeActorEmbargo()
        {
            return new Response<IEnumerable<TipoActorEmbargoDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoActorEmbargoDto>>(await this._catalogsRepository.ObtenerTipoActorEmbargo())
            };
        }

        public async Task<Response<IEnumerable<TipoActorEmbargoDto>>> EliminarTipoActorEmbargo(int Id)
        {
            return new Response<IEnumerable<TipoActorEmbargoDto>>
            {
                Data = _mapper.Map<IEnumerable<TipoActorEmbargoDto>>(await this._catalogsRepository.EliminarTipoActorEmbargo(Id))
            };
        }

        public async Task<Response<IEnumerable<TipoUnidadEjecutoraReporteDto>>> ObtenerUnidadEjecutoraReporte()
        {
            return new Response<IEnumerable<TipoUnidadEjecutoraReporteDto>> { Data = this._mapper.Map<IEnumerable<TipoUnidadEjecutoraReporteDto>>(await this._catalogsRepository.ObtenerUnidadEjecutoraReporte()) };
        }

        public async Task<Response<IEnumerable<TipoReporteGA2Dto>>> ObtenerTipoReporteGA2()
        {
            return new Response<IEnumerable<TipoReporteGA2Dto>> { Data = this._mapper.Map<IEnumerable<TipoReporteGA2Dto>>(await this._catalogsRepository.ObtenerTipoReporteGA2()) };
        }

        public async Task<Response<IEnumerable<LineaCreditoDto>>> ObtenerLineasCredito(int IdTipoCredito)
        {
            return new Response<IEnumerable<LineaCreditoDto>> { Data = this._mapper.Map<IEnumerable<LineaCreditoDto>>(await this._catalogsRepository.ObtenerLineasCredito(IdTipoCredito)) };
        }

        #region Comprobante, Concepto, Beneficiario Cartera
        public async Task<Response<IEnumerable<TipoBeneficiarioDto>>> ObtenerBeneficiarioCartera()
        {
            return new Response<IEnumerable<TipoBeneficiarioDto>> { Data = this._mapper.Map<IEnumerable<TipoBeneficiarioDto>>(await this._catalogsRepository.ObtenerBeneficiarioCartera()) };
        }

        public async Task<Response<IEnumerable<TipoConceptoDto>>> ObtenerConceptosCartera()
        {
            return new Response<IEnumerable<TipoConceptoDto>> { Data = this._mapper.Map<IEnumerable<TipoConceptoDto>>(await this._catalogsRepository.ObtenerConceptosCartera()) };
        }

        public async Task<Response<IEnumerable<TipoComprobanteDto>>> ObtenerTipoComprobanteCartera()
        {
            return new Response<IEnumerable<TipoComprobanteDto>> { Data = this._mapper.Map<IEnumerable<TipoComprobanteDto>>(await this._catalogsRepository.ObtenerTipoComprobanteCartera()) };
        }
        #endregion

        public async Task<Response<IEnumerable<MediosRecepcionDto>>> ObtenerMediosRecepcion()
        {
            return new Response<IEnumerable<MediosRecepcionDto>> { Data = this._mapper.Map<IEnumerable<MediosRecepcionDto>>(await this._catalogsRepository.ObtenerMediosRecepcion()) };

        }

        public async Task<Response<ConceptoNominaDto>> InsertarConceptoNomina(ConceptoNominaDto request)
        {
            var methodName = nameof(InsertarConceptoNomina);

            try
            {
                #region VALIDACIONES

                if (request == null)
                {
                    return new Response<ConceptoNominaDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "La solicitud es obligatoria."
                    };
                }


                #endregion

                #region MAPEO

                ConceptoNomina concepto = new ConceptoNomina();
                concepto.CNN_DESCRIPCION = request.Descripcion;
                concepto.CNN_SIGLA = request.Sigla;
                concepto.CNN_ESTADO = request.Estado;
                concepto.CNN_CREADO_POR = request.CreadoPor;

                #endregion

                #region REPOSITORY

                var result = await _catalogsRepository.InsertarConceptoNomina(concepto);

                #endregion

                #region VALIDACION_RESPUESTA

                if (result)
                {
                    return new Response<ConceptoNominaDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No fue posible insertar el concepto de nómina."
                    };
                }

                #endregion

                #region RESPUESTA

                return new Response<ConceptoNominaDto>
                {
                    IsSuccess = true,
                    ReturnMessage = "Concepto de nómina creado correctamente.",
                    Data = null
                };

                #endregion
            }
            catch (Exception ex)
            {
                return new Response<ConceptoNominaDto>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en {methodName}: {ex.Message}"
                };
            }
        }

        public async Task<Response<ConceptoNominaDto>> ActualizarConceptoNomina(ConceptoNominaDto request)
        {
            var methodName = nameof(ActualizarConceptoNomina);

            try
            {
                #region VALIDACIONES

                if (request == null)
                {
                    return new Response<ConceptoNominaDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "La solicitud es obligatoria."
                    };
                }

                #endregion

                #region MAPEO

                ConceptoNomina concepto = new ConceptoNomina();
                concepto.CNN_ID = request.Id;
                concepto.CNN_DESCRIPCION = request.Descripcion;
                concepto.CNN_SIGLA = request.Sigla;
                concepto.CNN_ESTADO = request.Estado;
                concepto.CNN_MODIFICADO_POR = request.ModificadoPor;

                #endregion

                #region REPOSITORY

                var result = await _catalogsRepository.ActualizarConceptoNomina(concepto);

                #endregion

                #region VALIDACION_RESPUESTA

                if (result == null || !result.Any())
                {
                    return new Response<ConceptoNominaDto>
                    {
                        IsSuccess = false,
                        ReturnMessage = "No fue posible actualizar el concepto de nómina."
                    };
                }

                #endregion

                #region RESPUESTA

                return new Response<ConceptoNominaDto>
                {
                    IsSuccess = true,
                    ReturnMessage = "Concepto de nómina actualizado correctamente.",
                    Data = request
                };

                #endregion
            }
            catch (Exception ex)
            {
                return new Response<ConceptoNominaDto>
                {
                    IsSuccess = false,
                    ReturnMessage = $"Error en {methodName}: {ex.Message}"
                };
            }
        }

        public async Task<Response<IEnumerable<ConceptoNominaDto>>> ObtenerConceptoNominaParam()
        {
            return new Response<IEnumerable<ConceptoNominaDto>>
            {
                Data = _mapper.Map<IEnumerable<ConceptoNominaDto>>(await _catalogsRepository.ObtenerConceptoNominaParam())
            };
        }
    }
}
