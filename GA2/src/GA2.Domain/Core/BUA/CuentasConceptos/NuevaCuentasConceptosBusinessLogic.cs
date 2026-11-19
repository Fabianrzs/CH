using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Main.ProviderRequest;
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
    public class NuevaCuentasConceptosBusinessLogic : ICuentasBusinessLogic
    {
        private readonly ICuentasConceptosRepository _cuentasConceptosRepository;
        private readonly IEmbargosProvider _iEmbargosRequestProvider;
        private readonly IMapper _mapper;

        public NuevaCuentasConceptosBusinessLogic(ICuentasConceptosRepository cuentasConceptosRepository,
                                                    IEmbargosProvider iEmbargosRequestProvider,
                                                    IMapper mapper)
        {
            _cuentasConceptosRepository = cuentasConceptosRepository;
            _iEmbargosRequestProvider = iEmbargosRequestProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bloqueo"></param>
        /// <returns></returns>
        public async Task<Response<InsertCuentaConcepto>> CrearCuentaAfiliado(InsertCuentaConcepto bloqueo)
        {
            InsertCuentaConcepto cuentaResult = await _cuentasConceptosRepository.ValidarExisteCuenta(bloqueo.CTA_NUMERO);
            InsertCuentaConcepto cuentaTipoResult = await _cuentasConceptosRepository.ValidarExisteCuentaTipo(bloqueo.CLI_ID, bloqueo.TCT_ID);
            
            if (cuentaResult != null || cuentaTipoResult != null)
            {
                Response<InsertCuentaConcepto> respuesta = new Response<InsertCuentaConcepto>();
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "La Cuenta ya existe!";
                return respuesta;
            }
            else
            {
                InsertCuentaConcepto result = await _cuentasConceptosRepository.InsertarNuevaCuenta(bloqueo);
                //CrearSolicitudDto crearSolicitudDto = new CrearSolicitudDto()
                //{
                //    clienteId = result.CLI_ID,
                //    creadoPor = Guid.NewGuid(),
                //    cuentaId = result.CTA_ID,
                //    procesoId = Guid.Parse("b61024bb-f484-4aa5-94f5-73d0750d95a2"),
                //    timCodigo = "1",
                //    tipoSubModalidad=Guid.NewGuid()
                //};
                //var resultSolicitud = await _iEmbargosRequestProvider.CrearSolicitudGA2(crearSolicitudDto);
                return new Response<InsertCuentaConcepto> { Data = result };
            }
        }

    }
}
