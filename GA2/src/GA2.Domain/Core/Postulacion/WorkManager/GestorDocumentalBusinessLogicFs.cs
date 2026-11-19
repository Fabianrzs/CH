using AutoMapper;
using GA2.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GA2.Application.Main;
using GA2.Domain.Core.ReferenciasSvc.WorkManager;
using GA2.Domain.Core.Servicios.WorkManager;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AttachRequestDto = GA2.Domain.Core.ReferenciasSvc.WorkManager.AttachRequestDto;

namespace GA2.Domain.Core.Postulacion.WorkManager
{
    public class GestorDocumentalBusinessLogicFs : IGestorDocumentalBusinessLogicFs
    {
        private readonly IPostulacionRepository _postulacionRepository;
        private readonly ICatalogosRepository _catalogosRepository;
        private readonly IClientRequestProvider _clientRequestProvider;
        private readonly IWorkManagerService _workmanagerService;
        private readonly IUsersRepository _usersRepository;
        private readonly CHKeysOptions _optionsCH;
        private readonly ICreditoRepository _creditoRepository;
        public GestorDocumentalBusinessLogicFs(IPostulacionRepository postulacion,
            ICatalogosRepository catalogosRepository,
            IClientRequestProvider clientRequestProvider,
            IWorkManagerService workmanagerService,
            IOptions<CHKeysOptions> optionsCH,
            IUsersRepository usersRepository,
            ICreditoRepository creditoRepository)
        {
            _postulacionRepository = postulacion ?? throw new ArgumentNullException(nameof(postulacion));
            _catalogosRepository = catalogosRepository ?? throw new ArgumentNullException(nameof(catalogosRepository));
            _clientRequestProvider = clientRequestProvider ?? throw new ArgumentNullException(nameof(clientRequestProvider));
            _workmanagerService = workmanagerService ?? throw new ArgumentNullException(nameof(workmanagerService));
            _optionsCH = optionsCH.Value ?? throw new ArgumentNullException(nameof(optionsCH));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _creditoRepository = creditoRepository ?? throw new ArgumentNullException(nameof(creditoRepository));
        }       

        public async Task<Response<Form_InsertResponse>> RadicarGDFondoSolidaridad(Guid idSolicitud)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();
            var status = parametrosIntegraciones.Where(p => p.INTEGRACION.ToUpper().Equals("DODO")).FirstOrDefault();
            if (status.VALOR == "true") //reemplazar por opciones fenix
            {
                InsertRequestDto insertRequest = new InsertRequestDto();
                Response<Form_InsertResponse> insert = new Response<Form_InsertResponse>();

                var datosSolicitud = await _postulacionRepository.ObtenerSolicitudPostulacionBySolicitudId(idSolicitud);
                insertRequest = await GetInsertRequestFondoSolidaridad(datosSolicitud);

                insert = await _workmanagerService.FormInsertAsyncCH(insertRequest);

                return insert;
            }
            else
            {
                return new Response<Form_InsertResponse>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la integracion con el servicio de dodo", Data = null };
            }
        }

        public async Task<InsertRequestDto> GetInsertRequestFondoSolidaridad(SolicitudPostulante solicitud)
        {
            List<ReferenciasSvc.WorkManager.DictionaryDto> dictionary = new();
            var cliente = (await _clientRequestProvider.ObtenerClientePorTipoIdentificationYNumero(solicitud.SPO_TIPO_DOCUMENTO_ID, solicitud.SPO_NUMERO_DOCUMENTO.ToString())).Data;
            var UnidadEjecutora = _catalogosRepository.ObtenerUnidadesEjecutoras().Result.First(x => x.UEJ_ID == cliente.IdUnidadEjecutora).UEJ_NOMBRE;
            var categoria = (await _catalogosRepository.ObtenerCategorias()).First(x => x.CTG_ID == cliente.IdCategoria).CTG_DESCRIPCION;        
            var ciudad = await _catalogosRepository.ObtenerCiudadPorIdDPCyIdDPD(cliente.IdCiudadResidencia, cliente.IdDepartamentoResidencia);
            var user = await _usersRepository.ObtenerUsuarioPorId(solicitud.SPO_USERID);
           
            var departament = (await _catalogosRepository.ObtenerDepartamentoPorId(cliente.IdDepartamentoResidencia));
            var direccion = string.IsNullOrEmpty(cliente.Direccion) ? "" : cliente.Direccion;
            var Barrio = string.IsNullOrEmpty(cliente.Barrio) ? "" : cliente.Barrio;
            var telefono = string.IsNullOrEmpty(cliente.Telefono) ? "" : cliente.Telefono;
            var correo = string.IsNullOrEmpty(cliente.Correo) ? "" : cliente.Correo;
            var nombre = "ga2";
            var apellido = "web";
            if (user!=null)
            {
                 nombre = user.USR_NOMBRE.ToLower();
                 apellido = user.USR_APELLIDO.ToLower();
            }
         
         
            var modalidad = _postulacionRepository.ObtenerModalidadPostulacionById(solicitud.SPO_ID_MODALIDAD);
            var TipoPostulacion = "";
            var Modelo = "";
            if (modalidad!=null)
            {
                if (modalidad.Result.MPO_NOMBRE == "RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA" || modalidad.Result.MPO_NOMBRE == "BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA")
                {
                    Modelo = "Héroes FSA";
                }
                else
                {
                    Modelo = "Proyectos y mecanismos especiales";
                }

                if (modalidad.Result.MPO_NOMBRE == "BENEFICIARIO(A) POR FALLECIMIENTO DEL AFILIADO PARA SOLUCIÓN DE VIVIENDA")
                {
                    TipoPostulacion = "Beneficiario(a)  a causa de fallecimiento del afiliado para solucion de vivienda";
                }
                else if (modalidad.Result.MPO_NOMBRE == "RETIRO O DESVINCULACIÓN POR DISMINUCIÓN DE LA CAPACIDAD PSICOFÍSICA")
                {
                    TipoPostulacion = "Retiro o desvinculacion por disminucion de la capacidad psicofisica";
                }
                else
                {
                    TipoPostulacion = "Proyectos y mecanismos especiales";
                }
            }
         
            dictionary.Add(new() { Field = "C0002", Value = $"Sede Principal Bogotá" });
            dictionary.Add(new() { Field = "C0003", Value = $"{cliente.Identificacion} | {cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" });
            dictionary.Add(new() { Field = "C0007", Value = $"{cliente.IdTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0008", Value = $"{cliente.Identificacion}" });
            dictionary.Add(new() { Field = "C0009", Value = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" });
            dictionary.Add(new() { Field = "C0010", Value = $"Abierto" });
            dictionary.Add(new() { Field = "C0011", Value = $"{UnidadEjecutora}" });
            dictionary.Add(new() { Field = "C0018", Value = $"Postulación Fondo de Solidaridad FSA" });
            dictionary.Add(new() { Field = "C0019", Value = $"{Modelo}" });
            dictionary.Add(new() { Field = "C0020", Value = $"{categoria}" });
            dictionary.Add(new() { Field = "C0021", Value = $"{TipoPostulacion}" });
            dictionary.Add(new() { Field = "C0026", Value = $"{cliente.Correo}" });
            dictionary.Add(new() { Field = "C0034", Value = $"0" });
            dictionary.Add(new() { Field = "C0037", Value = $"Convocatoria No. {solicitud.SPO_NUMERO_CONVOCATORIA}" });
            dictionary.Add(new() { Field = "C0038", Value = $"SI" });
            dictionary.Add(new() { Field = "C0042", Value = $"{cliente.IdTipoIdentificacion}" });
            dictionary.Add(new() { Field = "C0043", Value = $"{cliente.Identificacion}" });
            dictionary.Add(new() { Field = "C0044", Value = $"{cliente.PrimerNombre} {cliente.SegundoNombre} {cliente.PrimerApellido} {cliente.SegundoApellido}" });
            dictionary.Add(new() { Field = "C0016", Value = $"{departament.DPD_CODIGO_DODO}" });
            dictionary.Add(new() { Field = "C0022", Value = $"{ciudad.DPC_CODIGO_DODO}" });
            dictionary.Add(new() { Field = "C0023", Value = $"{telefono}" });
            dictionary.Add(new() { Field = "C0014", Value = $"{direccion}" });
            dictionary.Add(new() { Field = "C0015", Value = $"{Barrio}" });

            return new InsertRequestDto()
            {
                CheckWorkflow = true,
                Data = dictionary.ToArray(),
                FormCode = "39",
                OfficeCode = "01",
                OperationUser = $"{nombre}.{apellido}",
                Header = new()
                {
                    Token = _optionsCH.Token,
                    User = _optionsCH.User
                }
            };
        }
    }
}
