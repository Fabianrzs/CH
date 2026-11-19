using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.AuditoriaNovedades;
using GA2.Application.Main;
using GA2.Domain.Core.Reportes.Diseño;
using GA2.Domain.Entities;
using GA2.Infraestructure;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    public class AuditoriaNovedadesBussinessLogic : IAuditoriaNovedadesBussinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IAuditoriaNovedadesRepository _auditoriaNovedadesRepository;
        private readonly IReportesBusinessLogic _reportesBusinessLogic;
        private readonly IClientRequestProvider _clientRequestProvider;

        public AuditoriaNovedadesBussinessLogic(
            IMapper mapper,
            IAuditoriaNovedadesRepository auditoriaNovedadesRepository,
            IReportesBusinessLogic reportesBusinessLogic,
            IClientRequestProvider clientRequestProvider
            )
        {
            _mapper = mapper;
            _auditoriaNovedadesRepository = auditoriaNovedadesRepository;
            _reportesBusinessLogic = reportesBusinessLogic;
            _clientRequestProvider = clientRequestProvider;
        }

        public async Task<Response<IEnumerable<AuditoriaNovedadesGetDto>>> ConsultaAuditoriaNovedades(int idModulo)
        {
            return new Response<IEnumerable<AuditoriaNovedadesGetDto>> { Data = this._mapper.Map<IEnumerable<AuditoriaNovedadesGetDto>>(await _auditoriaNovedadesRepository.ConsultarAuditoriaNovedades(idModulo)) };
        }

        public async Task<Response<GeneracionArchivosResponse>> GenerarExcelNovedaes(int idModulo)
        {
            var data = await _auditoriaNovedadesRepository.ConsultarAuditoriaNovedades(idModulo);
            return new Response<GeneracionArchivosResponse> { Data = new() { ArchivoB64 = await _reportesBusinessLogic.GenerarExcel(ConvertListToDataTable(data.ToList()), "Auditoria Excel"), NameFile = "Auditoria Excel" } };            
        }

        public async Task<Response<AuditoriaNovedadesDto>> InsertarAuditoriaNovedades(AuditoriaNovedadesDto request)
        {
            if (request.TipoDocumento == 9)
            {
                var cliente = await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(request.TipoDocumento.HasValue? request.TipoDocumento.Value:0, request.NDocumento);
                if(cliente.Data != null)
                {
                    request.TipoDocumento = cliente.Data.IdTipoIdentificacion;
                    request.NDocumento = cliente.Data.Identificacion;
                }
            }
            return new Response<AuditoriaNovedadesDto> { Data = this._mapper.Map<AuditoriaNovedadesDto>(await _auditoriaNovedadesRepository.InsertarAuditoriaNovedades(this._mapper.Map<AuditoriaNovedades>(request))) };
        }

        public async Task<Response<ResponseValidaNovedadesFondoSolidaridadDto>> ValidaNovedadesFondoSolidaridad(RequestValidaNovedadesFondoSolidaridadDto request)
        {
            return new Response<ResponseValidaNovedadesFondoSolidaridadDto> { Data = await _auditoriaNovedadesRepository.ValidaNovedadesFondoSolidaridad(request) };
        }

        public DataTable ConvertListToDataTable(List<AuditoriaNovedadesGet> auditoriaNovedadesGetDtos)
        {
            DataTable dataTable = new DataTable();

            // Agregar las columnas al DataTable
            dataTable.Columns.Add("TIPO_IDENTIFICACION_AFILIADO", typeof(string));
            dataTable.Columns.Add("NUMERO_IDENTIFICACION_AFILIADO", typeof(int));
            dataTable.Columns.Add("UNIDAD_EJECUTORA", typeof(string));
            dataTable.Columns.Add("MÓDULO", typeof(string));
            dataTable.Columns.Add("ACCIÓN", typeof(string));
            dataTable.Columns.Add("DETALLE_MODIFICACION", typeof(string));            
            dataTable.Columns.Add("FECHA_INICIO", typeof(DateTime));
            dataTable.Columns.Add("FECHA_FIN", typeof(DateTime));
            dataTable.Columns.Add("FECHA_MODIFICACION", typeof(DateTime));
            dataTable.Columns.Add("USUARIO_MODIFICACION", typeof(string));            

            // Recorrer la lista y agregar cada elemento como una fila en el DataTable
            foreach (AuditoriaNovedadesGet auditoriaNovedadesGet in auditoriaNovedadesGetDtos.OrderByDescending(x=>x.FECHA_INICIO))
            {
                dataTable.Rows.Add(
                    auditoriaNovedadesGet.TID_DESCRIPCION, 
                    auditoriaNovedadesGet.AUN_IDENTIFICACION,
                    auditoriaNovedadesGet.UNIDAD_EJECUTORA,
                    auditoriaNovedadesGet.nombre_modulo,
                    auditoriaNovedadesGet.AUN_ACCION,
                    auditoriaNovedadesGet.AUN_MODIFICACION,
                    auditoriaNovedadesGet.FECHA_INICIO, 
                    auditoriaNovedadesGet.FECHA_FIN,
                    auditoriaNovedadesGet.AUN_FECHA_MODIFICACION, 
                    auditoriaNovedadesGet.NOMBRE_USUARIO);
            }

            return dataTable;
        }
    }
}
