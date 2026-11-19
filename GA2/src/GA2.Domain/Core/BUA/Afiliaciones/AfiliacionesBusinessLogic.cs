using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class AfiliacionesBusinessLogic : IAfiliacionesBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly IAfiliacionesRepository _afiliacionesRepository;

        /// <summary>
        /// Constructor Negocio Clients
        /// </summary>
        /// <param name="mapper">Instancia mapper</param>
        /// <param name="clientsRepository">Instancia repositorio usuarios</param>
        public AfiliacionesBusinessLogic(
            IMapper mapper,
            IAfiliacionesRepository afiliacionesRepository
        )
        {
            _mapper = mapper;
            _afiliacionesRepository = afiliacionesRepository;
        }

        public async Task<Response<AfiliacionDto>> ActualizarAfiliacion(AfiliacionDto afiliacion)
        {
            var afiliacionRequest = this._mapper.Map<Afiliacion>(afiliacion);
            foreach (BeneficiariosAfiliacionesDto item in afiliacion.Beneficiarios)
            {
                await _afiliacionesRepository.ActualizarBeneficiario(_mapper.Map<BeneficiariosAfiliaciones>(item));
            }
            foreach (AportesCategoriaAfiliacionesDto item in afiliacion.Aportes)
            {
                await _afiliacionesRepository.ActualizarAportes(_mapper.Map<AportesCategoriaAfiliaciones>(item));
            }
            foreach (TramiteDesafiliacionDto item in afiliacion.TramitesDesafiliacion)
            {
                item.IdAfiliacion = afiliacion.idAfiliacion;
                await _afiliacionesRepository.ActualizarTramiteDesafiliacion(_mapper.Map<TramiteDesafiliacion>(item));
            }
            return new Response<AfiliacionDto> { Data = this._mapper.Map<AfiliacionDto>(await _afiliacionesRepository.ActualizarAfiliacion(afiliacionRequest)) };
        }

        public async Task<Response<AfiliacionDto>> EliminarAfiliacion(int afiliacion)
        {
            return new Response<AfiliacionDto> { Data = this._mapper.Map<AfiliacionDto>(await _afiliacionesRepository.EliminarAfiliacion(afiliacion)) };
        }

        public async Task<Response<AfiliacionDto>> InsertarAfiliacion(AfiliacionDto afiliacion)
        {
            var afiliacionRequest = this._mapper.Map<Afiliacion>(afiliacion);
            foreach (BeneficiariosAfiliacionesDto item in afiliacion.Beneficiarios)
            {
                await _afiliacionesRepository.ActualizarBeneficiario(_mapper.Map<BeneficiariosAfiliaciones>(item));
            }
            foreach (AportesCategoriaAfiliacionesDto item in afiliacion.Aportes)
            {
                await _afiliacionesRepository.ActualizarAportes(_mapper.Map<AportesCategoriaAfiliaciones>(item));
            }

            var afiliacionInsertada = this._mapper.Map<AfiliacionDto>(await _afiliacionesRepository.InsertarAfiliacion(afiliacionRequest));
            foreach (TramiteDesafiliacionDto item in afiliacion.TramitesDesafiliacion)
            {
                item.IdAfiliacion = afiliacionInsertada.idAfiliacion;
                await _afiliacionesRepository.ActualizarTramiteDesafiliacion(_mapper.Map<TramiteDesafiliacion>(item));
            }

            return new Response<AfiliacionDto> { Data = afiliacionInsertada };
        }

        public async Task<Response<AfiliacionDto>> InsertarAfiliacionGestorDocumental(InsertarAfiliacionGestorDocumentalDto request)
        {
            var dto = new InsertarAfiliacionGestorDocumental
            {
                NumeroRadicado = request.NumeroRadicado,
                TipoAfiliacion = request.TipoAfiliacion,
                TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento
            };
            var afiliacionInsertada = this._mapper.Map<AfiliacionDto>(await _afiliacionesRepository.InsertarAfiliacionGestorDocumental(dto));

            return new Response<AfiliacionDto> { Data = afiliacionInsertada };



        }


        /// <summary>
        /// Método para obtener la información de afiliación por identificación
        /// </summary>
        /// <param name="consultaAfiliacion"> Objeto de consulta </param>
        /// <returns></returns>
        public async Task<Response<IEnumerable<AfiliacionDto>>> ObtenerAfiliacionByIdentificacion(ConsultaAfiliacionesRequestDTO consultaAfiliacion)
        {
            var afiliacionRequest = this._mapper.Map<ConsultaAfiliacion>(consultaAfiliacion);

            IEnumerable<AfiliacionDto> afiliacionesDto = this._mapper.Map<IEnumerable<AfiliacionDto>>(await _afiliacionesRepository.ObtenerAfiliacionByIdentificacion(afiliacionRequest));
            foreach (var afiliacion in afiliacionesDto)
            {
                afiliacion.Beneficiarios = new();
                afiliacion.Beneficiarios = this._mapper.Map<IEnumerable<BeneficiariosAfiliacionesDto>>(await _afiliacionesRepository.ObtenerBeneficiariosAfiliacion(afiliacion.idCliente)).ToList();

                afiliacion.Aportes = new();
                afiliacion.Aportes = this._mapper.Map<IEnumerable<AportesCategoriaAfiliacionesDto>>(await _afiliacionesRepository.ObtenerAportesAfiliacion(afiliacion.idCliente)).ToList();

                afiliacion.TramitesDesafiliacion = new();
                afiliacion.TramitesDesafiliacion = this._mapper.Map<IEnumerable<TramiteDesafiliacionDto>>(await _afiliacionesRepository.ObtenerTramitesDesafiliacion(afiliacion.idAfiliacion)).ToList();
            }

            return new Response<IEnumerable<AfiliacionDto>> { Data = afiliacionesDto };
        }

        public async Task<Response<IEnumerable<AfiliacionDto>>> ObtenerAfiliacionesRangoFiltros(ObtenerAfiliacionesRangoFiltrosDto consultaAfiliacion)
        {
            var afiliacionRequest = this._mapper.Map<ObtenerAfiliacionesRangoFiltros>(consultaAfiliacion);

            IEnumerable<AfiliacionDto> afiliacionesDto = this._mapper.Map<IEnumerable<AfiliacionDto>>(await _afiliacionesRepository.ObtenerAfiliacionesRangoFiltros(afiliacionRequest));


            return new Response<IEnumerable<AfiliacionDto>> { Data = afiliacionesDto };
        }

        public async Task<Response<IEnumerable<InformacionClienteDto>>> ReportesInformacionCliente(string identificacion)
        {
            IEnumerable<InformacionClienteDto> informacionClienteDto = this._mapper.Map<IEnumerable<InformacionClienteDto>>(await _afiliacionesRepository.ReporteInformacionCliente(identificacion));
            return new Response<IEnumerable<InformacionClienteDto>> { Data = informacionClienteDto };
        }

        public async Task<Response<ReporteInformacionClienteDto>> ReporteObtenerInformacionCliente(string identificacion)
        {
            var result = await _afiliacionesRepository.ReporteObtenerInformacionCliente(identificacion);
            return new Response<ReporteInformacionClienteDto> { Data = result };
        }

        public async Task<Response<IEnumerable<TransferenciaCesantiasDto>>> ObtenerDatosOficioRespuesta(int idAfiliacion)
        {
            IEnumerable<TransferenciaCesantiasDto> informacionClienteDto = this._mapper.Map<IEnumerable<TransferenciaCesantiasDto>>(await _afiliacionesRepository.ObtenerDatosOficioRespuesta(idAfiliacion));
            return new Response<IEnumerable<TransferenciaCesantiasDto>> { Data = informacionClienteDto };
        }

        public async Task<Response<IEnumerable<OficioImprocedenteDto>>> ObtenerDatosOficioImprocedente(int idAfiliacion)
        {
            IEnumerable<OficioImprocedenteDto> informacionClienteDto = this._mapper.Map<IEnumerable<OficioImprocedenteDto>>(await _afiliacionesRepository.ObtenerDatosOficioImprocedente(idAfiliacion));
            return new Response<IEnumerable<OficioImprocedenteDto>> { Data = informacionClienteDto };
        }

        public async Task<Response<IEnumerable<OficiosAfiliacionDto>>> GenerarReportesAfiliacion(ParametrosReportesAfiliacionesDto parametros)
        {
            var param = this._mapper.Map<ParametrosReportesAfiliaciones>(parametros);
            IEnumerable<OficiosAfiliacionDto> informacionClienteDto = this._mapper.Map<IEnumerable<OficiosAfiliacionDto>>(await _afiliacionesRepository.GenerarReportesAfiliacion(param));
            return new Response<IEnumerable<OficiosAfiliacionDto>> { Data = informacionClienteDto };
        }

        public async Task<Response<AfiliadoDto>> ObtenerNombresCliente(string identificacion)
        {
            AfiliadoDto cliente = this._mapper.Map<AfiliadoDto>(await _afiliacionesRepository.ObtenerNombresCliente(identificacion));
            return new Response<AfiliadoDto> { Data = cliente };
        }

        public async Task<Response<List<AfiliadoDto>>> ObtenerNombresClientesPorDocumentos(List<string> documentos)
        {
            List<AfiliadoDto> cliente = this._mapper.Map<List<AfiliadoDto>>(await _afiliacionesRepository.ObtenerNombresClientesPorDocumentos(documentos));
            return new Response<List<AfiliadoDto>> { Data = cliente };
        }

        public async Task<Response<IEnumerable<ReporteClienteCorreosErradosDto>>> ObtenerClientesCorreoErrados()
        {
            IEnumerable<ReporteClienteCorreosErradosDto> informacionClienteDto = this._mapper.Map<IEnumerable<ReporteClienteCorreosErradosDto>>(await _afiliacionesRepository.ObtenerClientesCorreoErrados());
            return new Response<IEnumerable<ReporteClienteCorreosErradosDto>> { Data = informacionClienteDto };
        }
    }
}
