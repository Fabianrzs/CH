using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Repositories
{
    public interface IEntidadesRepository
    {
        Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativa();
        Task<IEnumerable<EntidadEducativa>> ObtenerEntidadEducativaPorNombreNit(EntidadEducativa entidad);
        Task<IEnumerable<ProgramaEducativo>> ObtenerProgramaEducativo(Guid idEntidad);
        Task<ListarProgramaEducativo> CrearProgramaEducativo(ProgramaEducativo programaEducativo);
        Task<IEnumerable<NivelEducativo>> ObtenerNivelEducativoCesantias(Guid idPrograma);
        Task<EntidadEducativa> CrearEntidadEducativa(EntidadEducativa entidadEducativa);
        Task<EntidadEducativa> ActualizarEntidadEducativa(EntidadEducativa entidadEducativa);
        Task<EntidadEducativa> EliminarEntidadEducativaPorId(string EntidadEducativaId);
        Task<IEnumerable<SedeEntidadEducativa>> ObtenerSedesPorEntidadEducativa(Guid entidadEducativaId);
        Task<SedeEntidadEducativa> CrearSedeEntidadEducativa(SedeEntidadEducativa sedeEntidadEducativa);
        Task<crearAnotacion> CrearAnotacion(crearAnotacion crearAnotacion);
        Task<IEnumerable<RespuestaAnotacion>> ConsultarAnotacion(Guid entidadId);
        Task<SedeEntidadEducativa> ActualizarSedeEntidadEducativa(SedeEntidadEducativa sedeEntidadEducativa);
        Task<SedeEntidadEducativa> EliminarSedeEntidadEducativaPorId(IdentificadoresEntidadEducativa entidad); 
        Task<IEnumerable<ListarProgramaEducativo>> ObtenerProgramaEducativoPorEntidad(Guid idEntidad);
        Task<ProgramaEducativo> EliminarProgramaEducativoPorId(IdentificadoresEntidadEducativa entidadEducativa); 
        Task<ProgramaEducativo> ActualizarProgramaEducativo(ProgramaEducativo programaEducativo);
        Task<BloqueoEntidadEducativa> BloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa);
        Task<BloqueoEntidadEducativa> DesbloqueoEntidadEducativa(BloqueoEntidadEducativa bloqueoEntidadEducativa);
        Task<IEnumerable<BloqueoEntidadEducativa>> ObtenerBloqueoEntidadEducativa(string idEntidadEducativa);
        Task<IEnumerable<Fideicomiso>> ObtenerCuentaBancariaPorEntidad(string idEntidadEducativa);
        Task<CuentaBancariaEntidadEducativa> CrearCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa);
        Task<CuentaBancariaEntidadEducativa> ActualizarCuentaBancariaEntidadEducativa(CuentaBancariaEntidadEducativa cuentaBancariaEntidadEducativa);
        Task<CuentaBancariaEntidadEducativa> EliminarCuentaBancariaEntidadEducativa(Guid idCuenta);
        Task<InsertarArchivoEntidad> InsertarArchivoPorEntidad(InsertarArchivoEntidad insertarArchivoEntidad);
        Task<IEnumerable<ConsultarArchivosPorEntidad>> ConsultarArchivosPorEntidad(Guid IdEntidad);
        Task<EntidadEducativa> ObtenerEntidadEducativaDetalle(Guid IdEntidad);
        Task<bool> EliminarArchivoEntidad(EliminarArchivoEntidad eliminarArchivo);
        Task<ConsultarArchivosPorId> ConsultarArchivosEntidadPorId(Guid IdEntidad);
        Task<Fideicomiso> ConsultarFideicomisosEntidad(Fideicomiso fidecomiso);
        Task<IEnumerable<Fideicomiso>> InsertarFideicomisoEntidad(Fideicomiso fidecomiso);
        Task<IEnumerable<Fideicomiso>> ActualizarFideicomisoEntidadEducativa(Fideicomiso fidecomiso);
        Task<IEnumerable<Fideicomiso>> EliminarFideicomiso(IdentificadoresEntidadEducativa fidecomiso);
        Task<IEnumerable<TramiteEntidades>> DataTramitesEntidades(Guid idEntidad);
        Task<int> EliminarSolicitudes(string Identificacion);
        Task<int> ObtenerEntidadEducativaPorNit(string entidad);
        Task<IEnumerable<HistorialCambios>> ObtenerHistorialCambios(Guid idEntidad);
        Task<IEnumerable<HistorialCambiosInterviniente>> ObtenerHistorialCambiosIntervinientes(Guid idIntervinientes);
        Task<int> CrearNovedadesEntidadesEducativas(NovedadesEntidadEducativaRequest request);
        Task<int> CrearHistorialEntidad(HistorialCambios request);
    }
}