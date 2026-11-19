using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Application.Main;
using GA2.Application.Main.ProviderRequest;
using GA2.Domain.Core.Embargos.GestorDocumental;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Integraciones;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GA2.Domain.Core.IAuditoriaServicesBusinessLogic;


namespace GA2.Domain.Core
{
    /// <summary>
    /// Consecutivo Logica
    /// <author>Erwin Pantoja España</author>
    /// <date>05/04/2022</date>
    /// </summary>
    public class EmbargosBusinessLogic : IEmbargosBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Embargos
        /// </summary>
        private readonly IMapper _mapper;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly IGestorDocumentarEmbargosBL _iGestorDocumentarEmbargosBL;
        private readonly IEmbargosProvider _iEmbargosProvider;
        private readonly IAuditoriaRepository _auditoria;


        public EmbargosBusinessLogic(
            IMapper mapper,
            IEmbargosRepository JuzgadosRepository,
            IClientRequestProvider iClientRequestProvider,
            IGestorDocumentarEmbargosBL iGestorDocumentarEmbargosBL,
            IEmbargosProvider iEmbargosProvider,
            IAuditoriaRepository auditoria
            )
        {
            _mapper = mapper;
            _juzgadosRepository = JuzgadosRepository;
            _iClientRequestProvider = iClientRequestProvider;
            _iGestorDocumentarEmbargosBL = iGestorDocumentarEmbargosBL;
            _iEmbargosProvider = iEmbargosProvider;
            _auditoria = auditoria;

        }


        #region Juzgados


        public async Task<Response<JuzgadosGuidDto>> ObtenerJuzgadoById(Guid id)
        {
            JuzgadosGuid ju = await this._juzgadosRepository.ObtenerJuzgadoPorId(id);

            JuzgadosGuidDto juzgado = _mapper.Map<JuzgadosGuidDto>(ju);
            return new Response<JuzgadosGuidDto> { Data = juzgado };
        }



        public async Task<Response<JuzgadosDto>> ActualizarJuzgado(JuzgadosDto juzgado)
        {
            return new Response<JuzgadosDto>
            {
                Data = _mapper.Map<JuzgadosDto>(
                    await _juzgadosRepository.ActualizarJuzgado(
                         _mapper.Map<Juzgados>(juzgado)))
            };
        }

        public async Task<Response<JuzgadosDto>> CrearJuzgado(JuzgadosDto juzgado)
        {
            juzgado.FechaCreacion = DateTime.Now;
            return new Response<JuzgadosDto>
            {
                Data = _mapper.Map<JuzgadosDto>(
                    await _juzgadosRepository.CrearJuzgado(
                         _mapper.Map<Juzgados>(juzgado)))
            };
        }
        public async Task<Response<JuzgadosDto>> InsertarActor(TipoactorDto TipoActor)
        {
            return new Response<JuzgadosDto>
            {
                Data = _mapper.Map<JuzgadosDto>(
                    await this._juzgadosRepository.InsertarActor(TipoActor.TPR_NOMBRE))
            };
        }

        public async Task<Response<JuzgadosDto>> EliminarJuzgadoPorId(string idJuzgado)
        {
            return new Response<JuzgadosDto>
            {
                Data = _mapper.Map<JuzgadosDto>(
                    await this._juzgadosRepository.EliminarJuzgadoPorId(idJuzgado))
            };
        }



        public async Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorCiudad(JuzgadoDptoCiudadDto juzgadoDptoCiudad)
        {
            IEnumerable<JuzgadosDto> juzgados = _mapper.Map<IEnumerable<JuzgadosDto>>(await
                _juzgadosRepository.ObtenerJuzgadosPorCiudad(juzgadoDptoCiudad.IdDepartamento,
                juzgadoDptoCiudad.IdCiudad));
            return new Response<IEnumerable<JuzgadosDto>> { Data = juzgados };
        }

        public async Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorDepartamento(int idDpto)
        {
            IEnumerable<JuzgadosDto> juzgados = _mapper.Map<IEnumerable<JuzgadosDto>>(await _juzgadosRepository.ObtenerJuzgadosPorDepartamento(idDpto));
            return new Response<IEnumerable<JuzgadosDto>> { Data = juzgados };

        }

        public async Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorNombre(string nombreDpto)
        {
            IEnumerable<JuzgadosDto> juzgados = _mapper.Map<IEnumerable<JuzgadosDto>>(await _juzgadosRepository.ObtenerJuzgadosPorNombre(nombreDpto));
            return new Response<IEnumerable<JuzgadosDto>> { Data = juzgados };
        }

        public async Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorNumeroCuentaCodigo(DespachoDto despachoDto)
        {
            IEnumerable<JuzgadosDto> juzgados = _mapper.Map<IEnumerable<JuzgadosDto>>(await
               _juzgadosRepository.ObtenerJuzgadosPorNumeroCuentaCodigo(despachoDto.nCuenta,
               despachoDto.codigoInterno));
            return new Response<IEnumerable<JuzgadosDto>> { Data = juzgados };
        }


        #endregion

        public async Task<Response<IEnumerable<TipoEmbargosDto>>> ObtenerTiposEmbargos()
        {
            IEnumerable<TipoEmbargosDto> tipoEmbargos = _mapper.Map<IEnumerable<TipoEmbargosDto>>(await _juzgadosRepository.ObtenerTiposEmbargos());
            return new Response<IEnumerable<TipoEmbargosDto>> { Data = tipoEmbargos };
        }

        public async Task<Response<IEnumerable<TiposProcesosDto>>> ObtenerTiposProcesosEmbargos()
        {
            IEnumerable<TiposProcesosDto> tiposProcesos = _mapper.Map<IEnumerable<TiposProcesosDto>>(await _juzgadosRepository.ObtenerTiposProcesosEmbargos());
            return new Response<IEnumerable<TiposProcesosDto>> { Data = tiposProcesos };
        }

        public async Task<Response<EmbargosDto>> InsertarEmbargo(InsetEmbargosDto embargo, Guid userId)
        {

            Response<ClienteCesantiasDto> clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(embargo.ClienteTipoIdentificacion, embargo.ClienteIdentificacion, 0);
            Response<EmbargosDto> respuesta = new Response<EmbargosDto>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existe usuario con este número de identificacion y tipo";
                return respuesta;

            }

            if (embargo.BeneficiariosPagos.Count <= 0)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Debe enviar almenos un beneficiario de pago";
                return respuesta;

            }

            if (embargo.ConceptosEmbargos.Count <= 0)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Debe enviar almenos un concepto a embargar";
                return respuesta;

            }

            bool demandante = false;


            foreach (BeneficiariosPagoEmbargoDto beneficiario in embargo.BeneficiariosPagos)
            {
                if (beneficiario.Demandante)
                {
                    demandante = true;
                    break;
                }
            }

            if (!demandante)
            {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Debe existir un demandante";
                return respuesta;

            }


            foreach (InsetEmbargoCuentaConceptoDto embargoConcepto in embargo.ConceptosEmbargos)
            {

                Response<CuentaPorConceptoDto> conceptosCuentas = await _iClientRequestProvider.ObtenerConceptoPorClientePorCuentaPorConcepto(clienteDto.Data.IdCliente, embargoConcepto.CuentaId, embargoConcepto.ConceptoId);

                if (conceptosCuentas.Data == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = "No existe la cuenta Nro: " + embargoConcepto.NumeroCuenta + " o el concepto ha embargar";
                    return respuesta;
                }
            }

            InsertEmbargo embargoMapper = new InsertEmbargo();
            embargoMapper.EBA_RADICADO_WORK_MANAGER = embargo.RadicadoWorkManager;
            embargoMapper.EBA_FECHA_RADICADO_WORK_MANAGER = embargo.FechaRadicadoWorkManager;
            embargoMapper.EBA_RADICADO_JUZGADO = embargo.RadicadoJuzgado;
            embargoMapper.EBA_FECHA_RADICADO_JUZGADO = embargo.FechaRadicadoJuzgado;
            embargoMapper.EBA_EMAIL_RESPUESTA = embargo.EmailRespuesta;
            embargoMapper.EBA_EXPEDIENTE_BANCO_AGRARIO = embargo.ExpedienteBancoAgrario;
            embargoMapper.EBA_REMANENTE = embargo.Remanente;
            embargoMapper.EBA_INCONSISTENCIA = embargo.Inconsistencia;
            embargoMapper.EBA_OBSERVACIONES = embargo.Observaciones;
            embargoMapper.EBA_ACLARACION = embargo.aclaracion;
            embargoMapper.EBA_RESPUESTA = embargo.Respuesta;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.EMB_ID_JUZGADO = embargo.JuzgadoId;
            embargoMapper.TPE_ID = embargo.ProcesoId;
            embargoMapper.EBA_PROCESO = embargo.proceso;
            embargoMapper.EBA_CREADOPOR = userId;
            embargoMapper.CLI_ID = clienteDto.Data.IdCliente;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.EBA_NOMBRE_DEMANDADO = clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre + " " + clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido;
            embargoMapper.EBA_DIRECCION = embargo.Direccion;

            IEnumerable<BeneficiariosPagoEmbargo> beneficiarioPago = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargo>>(embargo.BeneficiariosPagos);
            embargoMapper.BENEFICIARIOS = (List<BeneficiariosPagoEmbargo>)beneficiarioPago;

            IEnumerable<InsertEmbargoCuentaConcepto> conceptos = _mapper.Map<IEnumerable<InsertEmbargoCuentaConcepto>>(embargo.ConceptosEmbargos);
            embargoMapper.CONCEPTOS = (List<InsertEmbargoCuentaConcepto>)conceptos;

            EmbargosDto tipoEmbargos = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.InsertarEmbargo(embargoMapper));

            if (tipoEmbargos == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Se generó un error al insertar el embargo";
                return respuesta;
            }
            return new Response<EmbargosDto> { Data = tipoEmbargos };
        }

        public async Task<Response<PagoEmbargoDto>> InsertarPagoEmbargo(PagoEmbargoDto embargo, Guid userId)
        {


            InsertarPagoEmbargo pagoEmbargoMapper = new InsertarPagoEmbargo();

            pagoEmbargoMapper.EBA_ID = embargo.IdEmargo;
            pagoEmbargoMapper.TRE_ID = embargo.TipoRetencionId;
            pagoEmbargoMapper.PE_CREADOPOR = userId;
            pagoEmbargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            pagoEmbargoMapper.TPI_ID = embargo.ClienteTipoIdentificacion;
            pagoEmbargoMapper.PE_NOMBRE = embargo.NombreRazonSocial;
            pagoEmbargoMapper.PE_VALOR = embargo.Valor;
            pagoEmbargoMapper.PE_FECHA = embargo.Fecha;
            pagoEmbargoMapper.PE_OBSERVACIONES = embargo.Observacion.ToString();
            pagoEmbargoMapper.PE_APROBADO = embargo.Aprobado;
            pagoEmbargoMapper.ENE_ENTIDAD_ID = embargo.Entidad;
            pagoEmbargoMapper.NUMERO_CUENTA = embargo.NumeroCuenta.ToString();


             PagoEmbargoDto pagoEmbargo = _mapper.Map<PagoEmbargoDto>(await this._juzgadosRepository.InsertarPagoEmbargo(pagoEmbargoMapper));

            return new Response<PagoEmbargoDto> { Data = pagoEmbargo };
        }


        public async Task<Response<PagoEmbargoDto>> ObtenerPagoEmbargo(DateTime embargo)
        {

            PagoEmbargoDto pagoEmbargo = _mapper.Map<PagoEmbargoDto>(await this._juzgadosRepository.ObtenerpagoEmbargo(embargo));
            return new Response<PagoEmbargoDto> { Data = pagoEmbargo };
        }


        public async Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargos()
        {
            IEnumerable<EmbargosDto> embargos = _mapper.Map<IEnumerable<EmbargosDto>>(await this._juzgadosRepository.ObtenerEmbargos());

            string txtClientes = "";

            foreach (var embargosDto in embargos)
            {
                txtClientes = txtClientes + embargosDto.ClienteId + ",";
            }

            Response<IEnumerable<ClienteDto>> clienteDto = await _iClientRequestProvider.ObtenerClientesById(txtClientes);
            List<EmbargosDto> list = new List<EmbargosDto>();

            foreach (EmbargosDto embargosDto in embargos)
            {

                foreach (ClienteDto cliente in clienteDto.Data)
                {

                    if (cliente.IdCliente == embargosDto.ClienteId)
                    {

                        embargosDto.PrimerNombreCliente = cliente.PrimerNombre;
                        embargosDto.SegundoNombreCliente = cliente.SegundoNombre;
                        embargosDto.PrimerApellidoCliente = cliente.PrimerApellido;
                        embargosDto.SegundoApellidoCliente = cliente.SegundoApellido;
                        embargosDto.IdentificacionCliente = cliente.Identificacion;
                        embargosDto.NombreCompletoCliente = cliente.PrimerNombre + " " + cliente.SegundoNombre + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                        TipoIdentificacion tipoDocumemto = await this._juzgadosRepository.ObtenerTipoIdentificacionPorId(cliente.IdTipoIdentificacion);
                        embargosDto.TipoIdentificacion = tipoDocumemto.TID_DESCRIPCION;

                        break;
                    }

                }


                list.Add(embargosDto);

            }

            return new Response<IEnumerable<EmbargosDto>> { Data = list };
        }

        public async Task<Response<EmbargosDto>> EliminarEmbargo(Guid Id)
        {
            EmbargosDto embargo = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.EliminarEmbargo(Id));
            return new Response<EmbargosDto> { Data = embargo };
        }

        public async Task<Response<EmbargosDto>> ActualizarEmbargo(InsetEmbargosDto embargo, Guid userId)
        {


            Response<ClienteCesantiasDto> clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(embargo.ClienteTipoIdentificacion, embargo.ClienteIdentificacion, 0);
            Response<EmbargosDto> respuesta = new Response<EmbargosDto>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existe usuario con este número de identificacion y tipo";
                return respuesta;

            }

            foreach (InsetEmbargoCuentaConceptoDto embargoConcepto in embargo.ConceptosEmbargos)
            {

                Response<CuentaPorConceptoDto> conceptosCuentas = await _iClientRequestProvider.ObtenerConceptoPorClientePorCuentaPorConcepto(clienteDto.Data.IdCliente, embargoConcepto.CuentaId, embargoConcepto.ConceptoId);

                if (conceptosCuentas.Data == null)
                {
                    respuesta.IsSuccess = false;
                    respuesta.ReturnMessage = "No existe la cuenta Nro: " + embargoConcepto.NumeroCuenta + " o el concepto ha embargar";
                    return respuesta;
                }
            }

            InsertEmbargo embargoMapper = new InsertEmbargo();

            embargoMapper.EBA_ID = embargo.Id;
            embargoMapper.EBA_RADICADO_WORK_MANAGER = embargo.RadicadoWorkManager;
            embargoMapper.EBA_FECHA_RADICADO_WORK_MANAGER = embargo.FechaRadicadoWorkManager;
            embargoMapper.EBA_RADICADO_JUZGADO = embargo.RadicadoJuzgado;
            embargoMapper.EBA_FECHA_RADICADO_JUZGADO = embargo.FechaRadicadoJuzgado;
            embargoMapper.EBA_EMAIL_RESPUESTA = embargo.EmailRespuesta;
            embargoMapper.EBA_EXPEDIENTE_BANCO_AGRARIO = embargo.ExpedienteBancoAgrario;
            embargoMapper.EBA_REMANENTE = embargo.Remanente;
            embargoMapper.EBA_INCONSISTENCIA = embargo.Inconsistencia;
            embargoMapper.EBA_OBSERVACIONES = embargo.Observaciones;
            embargoMapper.EBA_ACLARACION = embargo.aclaracion;
            embargoMapper.EBA_RESPUESTA = embargo.Respuesta;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.EMB_ID_JUZGADO = embargo.JuzgadoId;
            embargoMapper.TPE_ID = embargo.ProcesoId;
            embargoMapper.EBA_DIRECCION = embargo.Direccion;



            embargoMapper.EBA_CREADOPOR = userId;
            embargoMapper.CLI_ID = clienteDto.Data.IdCliente;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.EBA_NOMBRE_DEMANDADO = clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre + " " + clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido;


            IEnumerable<BeneficiariosPagoEmbargo> beneficiarioPago = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargo>>(embargo.BeneficiariosPagos);
            embargoMapper.BENEFICIARIOS = (List<BeneficiariosPagoEmbargo>)beneficiarioPago;

            IEnumerable<InsertEmbargoCuentaConcepto> conceptos = _mapper.Map<IEnumerable<InsertEmbargoCuentaConcepto>>(embargo.ConceptosEmbargos);
            embargoMapper.CONCEPTOS = (List<InsertEmbargoCuentaConcepto>)conceptos;

            EmbargosDto tipoEmbargos = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ActualizarEmbargo(embargoMapper));

            if (tipoEmbargos == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Se generó un error al actualizar el embargo";
                return respuesta;
            }


            InsertEmbargo actualizarEmbargo = new InsertEmbargo();
            actualizarEmbargo.EBA_ID = tipoEmbargos.Id;
            actualizarEmbargo.EBA_RADICADO_WORK_MANAGER = tipoEmbargos.RadicadoWorkManager;
            actualizarEmbargo.EBA_FECHA_RADICADO_WORK_MANAGER = tipoEmbargos.FechaRadicadoWorkManager;
            actualizarEmbargo.EBA_RADICADO_JUZGADO = tipoEmbargos.RadicadoJuzgado;
            actualizarEmbargo.EBA_FECHA_RADICADO_JUZGADO = tipoEmbargos.FechaRadicadoJuzgado;
            actualizarEmbargo.EBA_EMAIL_RESPUESTA = tipoEmbargos.EmailRespuesta;
            actualizarEmbargo.EBA_EXPEDIENTE_BANCO_AGRARIO = tipoEmbargos.ExpedienteBancoAgrario;
            actualizarEmbargo.EBA_REMANENTE = tipoEmbargos.Remanente;
            actualizarEmbargo.EBA_INCONSISTENCIA = tipoEmbargos.Inconsistencia;
            actualizarEmbargo.EBA_OBSERVACIONES = tipoEmbargos.Observaciones;
            actualizarEmbargo.EBA_ACLARACION = embargo.aclaracion;
            actualizarEmbargo.EBA_RESPUESTA = tipoEmbargos.Respuesta;
            actualizarEmbargo.CLI_IDENTIFICACION = tipoEmbargos.IdentificacionCliente;
            actualizarEmbargo.TID_ID = tipoEmbargos.TipoIdentificacionId;
            actualizarEmbargo.EMB_ID_JUZGADO = tipoEmbargos.JuzgadoId;
            actualizarEmbargo.TPE_ID = tipoEmbargos.ProcesoId;
            actualizarEmbargo.EBA_PROCESO = tipoEmbargos.TipoProceso;

            actualizarEmbargo.EBA_CREADOPOR = userId;
            actualizarEmbargo.CLI_ID = clienteDto.Data.IdCliente;
            actualizarEmbargo.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            actualizarEmbargo.EBA_NOMBRE_DEMANDADO = clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre + " " + clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido;
            actualizarEmbargo.EBA_DIRECCION = tipoEmbargos.Direccion;

            IEnumerable<BeneficiariosPagoEmbargo> beneficiarioP = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargo>>(embargo.BeneficiariosPagos);
            actualizarEmbargo.BENEFICIARIOS = (List<BeneficiariosPagoEmbargo>)beneficiarioP;

            IEnumerable<InsertEmbargoCuentaConcepto> concepto = _mapper.Map<IEnumerable<InsertEmbargoCuentaConcepto>>(embargo.ConceptosEmbargos);
            actualizarEmbargo.CONCEPTOS = (List<InsertEmbargoCuentaConcepto>)concepto;
            EmbargosDto reponse = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ActualizarEmbargo(actualizarEmbargo));


            return new Response<EmbargosDto> { Data = tipoEmbargos };

        }

        public async Task<Response<EmbargosDto>> ActualizarEstadoEmbargo(ActualizarEstadoEmbargoDto embargo, Guid userId)
        {

            ActualizarEstadoEmbargo embargoMapper = new ActualizarEstadoEmbargo();

            embargoMapper.EBA_ID = embargo.Id;
            embargoMapper.EBA_INCONSISTENCIA = embargo.Inconsistencia;
            embargoMapper.EBA_ACLARACION = embargo.aclaracion;
            embargoMapper.EBA_CREADOPOR = userId;

            EmbargosDto tipoEmbargos = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ActualizarEstadoEmbargo(embargoMapper));
            Response<EmbargosDto> respuesta = new Response<EmbargosDto>();

            if (tipoEmbargos == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Se generó un error al actualizar el embargo";
                return respuesta;
            }

            return new Response<EmbargosDto> { Data = tipoEmbargos };

        }

        public async Task<Response<EmbargoCuentaConceptoDto>> InsertarEmbargoCuentaConcepto(EmbargoCuentaConceptoDto dto, Guid userId)
        {


            Embargo emb = await this._juzgadosRepository.ObtenerEmbargoAdministradorDeTrabajo(dto.RadicadoWorkManager);

            Response<EmbargoCuentaConceptoDto> respuesta = new Response<EmbargoCuentaConceptoDto>();

            if (emb == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No hay Embargos con el numero de radicado work manager";
                return respuesta;
            }

            EmbargoCuentaConcepto ecc = new EmbargoCuentaConcepto();
            ecc.ECC_VALOR = dto.Valor;
            ecc.ECC_FECHA_INICIO_EMBARGO = dto.FechaInicio;
            ecc.ECC_FECHA_FIN_EMBARGO = dto.FechaFin;
            ecc.ECC_INDICADOR_CESANTIAS = dto.IndicadorCesantias;
            ecc.EBA_ID = emb.EBA_ID;
            ecc.TRE_ID = dto.TipoRetencionId;
            ecc.TIE_ID = dto.TipoEmbargoId;
            ecc.CTA_ID = dto.CuentaId;
            ecc.CCT_ID = dto.ConceptoId;
            ecc.EBA_CREADOPOR = userId;


            EmbargoCuentaConceptoDto embargoCuenta = _mapper.Map<EmbargoCuentaConceptoDto>(await this._juzgadosRepository.InsertarEmbargoCuentaConcepto(ecc));
            return new Response<EmbargoCuentaConceptoDto> { Data = embargoCuenta };

        }

        public async Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargosCuentaConcepto()
        {

            IEnumerable<EmbargoCuentaConceptoDto> embargos = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await this._juzgadosRepository.ObtenerEmbargosCuentaConcepto());

            string idsEcc = "";

            foreach (EmbargoCuentaConceptoDto embargosDto in embargos)
            {
                idsEcc = idsEcc + embargosDto.CuentaId + ",";
            }

            Response<IEnumerable<CuentaDto>> cuentas = await _iClientRequestProvider.ObtenerCuentaIdCuenta(idsEcc);

            List<EmbargoCuentaConceptoDto> list = new List<EmbargoCuentaConceptoDto>();




            if (cuentas.Data.Count() > 0)
            {
                foreach (EmbargoCuentaConceptoDto embargosDto in embargos)
                {
                    foreach (CuentaDto cuenta in cuentas.Data)
                    {

                        if (cuenta.IdCuenta == embargosDto.CuentaId)
                        {
                            embargosDto.NumeroCuenta = cuenta.NumeroCuenta;
                        }
                    }

                    list.Add(embargosDto);
                }
            }


            return new Response<IEnumerable<EmbargoCuentaConceptoDto>> { Data = list };

        }

        public async Task<Response<List<EmbargoCuentaConceptoDto>>> EliminarEmbargoCuentaConcepto(EmbargoCuentaConceptosDto conceptos)
        {

            List<EmbargoCuentaConceptoDto> embargos = new List<EmbargoCuentaConceptoDto>();


            foreach (Guid id in conceptos.Id)
            {
                EmbargoCuentaConceptoDto embargoCuenta = _mapper.Map<EmbargoCuentaConceptoDto>(await this._juzgadosRepository.EliminarEmbargoCuentaConcepto(id, conceptos.Descripcion));
                embargos.Add(embargoCuenta);
            }
            return new Response<List<EmbargoCuentaConceptoDto>> { Data = embargos };

        }

        public async Task<Response<EmbargoCuentaConceptoDto>> ActualizarEmbargoCuentaConcepto(EmbargoCuentaConceptoDto emb, Guid userId)
        {

            Embargo emba = await this._juzgadosRepository.ObtenerEmbargoAdministradorDeTrabajo(emb.RadicadoWorkManager);

            Response<EmbargoCuentaConceptoDto> respuesta = new Response<EmbargoCuentaConceptoDto>();

            if (emba == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No hay Embargos con el numero de radicado work manager";
                return respuesta;
            }

            /// Response<CuentaDto> cuenta = await _iClientRequestProvider.ObtenerCuentaClientePorIdentificadorNumeroCuenta(emba.CLI_ID, emb.NumeroCuenta);



            EmbargoCuentaConcepto ecc = new EmbargoCuentaConcepto();
            ecc.ECC_ID = emb.Id;
            ecc.ECC_VALOR = emb.Valor;
            ecc.ECC_FECHA_INICIO_EMBARGO = emb.FechaInicio;
            ecc.ECC_FECHA_FIN_EMBARGO = emb.FechaFin;
            ecc.ECC_INDICADOR_CESANTIAS = emb.IndicadorCesantias;
            ecc.EBA_ID = emba.EBA_ID;
            ecc.TRE_ID = emb.TipoRetencionId;
            ecc.TIE_ID = emb.TipoEmbargoId;
            ecc.CTA_ID = emb.CuentaId;
            ecc.CCT_ID = emb.ConceptoId;
            ecc.EBA_ACTUALIZADOPOR = userId;

            EmbargoCuentaConceptoDto embargoCuenta = _mapper.Map<EmbargoCuentaConceptoDto>(await this._juzgadosRepository.ActualizarEmbargoCuentaConcepto(ecc));
            return new Response<EmbargoCuentaConceptoDto> { Data = embargoCuenta };

        }

        public async Task<Response<IEnumerable<TipoRetencionDto>>> ObtenerTiposRetenciones()
        {
            IEnumerable<TipoRetencionDto> retenciones = _mapper.Map<IEnumerable<TipoRetencionDto>>(await this._juzgadosRepository.ObtenerTiposRetenciones());
            return new Response<IEnumerable<TipoRetencionDto>> { Data = retenciones };
        }

        public async Task<Response<BeneficiariosPagoEmbargoDto>> InsertarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargoDto be, Guid userId)
        {
            Embargo embargo = await this._juzgadosRepository.ObtenerEmbargoAdministradorDeTrabajo("");
            BeneficiariosPagoEmbargo emb = new BeneficiariosPagoEmbargo();
            emb.EBA_ID = embargo.EBA_ID;
            emb.TID_ID = be.TipoDocumentoId;
            emb.BPE_NUMERO_IDENTIFICACION = be.NumeroIdentificacion;
            emb.BPE_PRIMER_NOMBRE = be.PrimerNombre;
            emb.BPE_SEGUNDO_NOMBRE = be.SegundoNombre;
            emb.BPE_PRIMER_APELLIDO = be.PrimerApellido;
            emb.BPE_SEGUNGO_APELLIDO = be.SegundoApellido;
            emb.BPE_CREADOPOR = userId;

            BeneficiariosPagoEmbargoDto beneficiario = _mapper.Map<BeneficiariosPagoEmbargoDto>(await this._juzgadosRepository.InsertarBeneficiariosPagoEmbargos(emb));
            return new Response<BeneficiariosPagoEmbargoDto> { Data = beneficiario };

        }

        public async Task<Response<BeneficiariosPagoEmbargoDto>> ActualizarBeneficiariosPagoEmbargos(BeneficiariosPagoEmbargoDto be, Guid userId)
        {

            Embargo embargo = await this._juzgadosRepository.ObtenerEmbargoAdministradorDeTrabajo("");



            BeneficiariosPagoEmbargo emb = new BeneficiariosPagoEmbargo();
            emb.BPE_ID = be.Id;
            emb.EBA_ID = embargo.EBA_ID;
            emb.TID_ID = be.TipoDocumentoId;
            emb.BPE_NUMERO_IDENTIFICACION = be.NumeroIdentificacion;
            emb.BPE_PRIMER_NOMBRE = be.PrimerNombre;
            emb.BPE_SEGUNDO_NOMBRE = be.SegundoNombre;
            emb.BPE_PRIMER_APELLIDO = be.PrimerApellido;
            emb.BPE_SEGUNGO_APELLIDO = be.SegundoApellido;
            emb.BPE_ACTUALIZADOPOR = userId;

            BeneficiariosPagoEmbargoDto beneficiario = _mapper.Map<BeneficiariosPagoEmbargoDto>(await this._juzgadosRepository.ActualizarBeneficiariosPagoEmbargos(emb));
            return new Response<BeneficiariosPagoEmbargoDto> { Data = beneficiario };
        }

        public async Task<Response<BeneficiariosPagoEmbargoDto>> EliminarBeneficiariosPagoEmbargos(Guid Id)
        {
            BeneficiariosPagoEmbargoDto beneficiario = _mapper.Map<BeneficiariosPagoEmbargoDto>(await this._juzgadosRepository.EliminarBeneficiariosPagoEmbargos(Id));
            return new Response<BeneficiariosPagoEmbargoDto> { Data = beneficiario };
        }

        public async Task<Response<IEnumerable<BeneficiariosPagoEmbargoDto>>> ObtenerBeneficiariosEmbargo()
        {
            IEnumerable<BeneficiariosPagoEmbargoDto> retenciones = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargoDto>>(await this._juzgadosRepository.ObtenerBeneficiariosEmbargo(Guid.NewGuid()));
            return new Response<IEnumerable<BeneficiariosPagoEmbargoDto>> { Data = retenciones };
        }

        public async Task<Response<IEnumerable<EmbargosDto>>> ObtenerEmbargosRangoFiltros(ConsultaEmbargosfiltroDto filtros)
        {

            if (filtros.clienteTipoIndentificacion == 0)
            {
                filtros.clienteTipoIndentificacion = null;
            }

            if (filtros.ClienteIdentificacion == "")
            {
                filtros.ClienteIdentificacion = null;
            }

            if (filtros.RadicadoWorkManager == "")
            {
                filtros.RadicadoWorkManager = null;
            }
            if (filtros.Estados == 398)
            {
                filtros.Estado = 0;
            }


            IEnumerable<EmbargosDto> embargos = _mapper.Map<IEnumerable<EmbargosDto>>(await this._juzgadosRepository.ObtenerEmbargosRangoFiltros(_mapper.Map<ConsultaEmbargosfiltro>(filtros)));

            return new Response<IEnumerable<EmbargosDto>> { Data = embargos };
        }

        public async Task<Response<IEnumerable<DesembargoInfoGeneralDto>>> ObtenerEmbargosADesembargar(ConsultaEmbargosfiltroDto filtros)
        {

            if (filtros.ClienteTipoIdentificacion == 0)
            {
                filtros.ClienteTipoIdentificacion = null;
            }

            if (filtros.ClienteIdentificacion == "")
            {
                filtros.ClienteIdentificacion = null;
            }

            if (filtros.RadicadoWorkManager == "")
            {
                filtros.RadicadoWorkManager = null;
            }


            IEnumerable<EmbargosDto> embargos = _mapper.Map<IEnumerable<EmbargosDto>>(await this._juzgadosRepository.ObtenerEmbargosRangoFiltros(_mapper.Map<ConsultaEmbargosfiltro>(filtros)));

            List<DesembargoInfoGeneralDto> desembargos = new();
            foreach (var item in embargos)
            {
                if (desembargos.Where(x => x.IdentificacionCliente == item.IdentificacionCliente).Count() == 0)
                {
                    DesembargoInfoGeneralDto desembargo = new()
                    {
                        NombreDemandado = item.NombreDemandado,
                        IdentificacionCliente = item.IdentificacionCliente
                    };

                    desembargos.Add(desembargo);
                }
            }

            return new Response<IEnumerable<DesembargoInfoGeneralDto>> { Data = desembargos };
        }

        public async Task<Response<DesembargoDto>> InsertarDesembargo(DesembargoDto Id)
        {
            DesembargoDto desembargo = _mapper.Map<DesembargoDto>(await this._juzgadosRepository.InsertarDesembargo(_mapper.Map<Desembargo>(Id)));
            //await _juzgadosRepository.CambiarEstadoEmbargo(Id.EmbargoCuentaConceptoId);
            return new Response<DesembargoDto> { Data = desembargo };
        }

        public async Task<Response<IEnumerable<ObtenerDesembargoDto>>> ObtenerDesembargo()
        {
            IEnumerable<ObtenerDesembargoDto> desembargosDto = _mapper.Map<IEnumerable<ObtenerDesembargoDto>>(await this._juzgadosRepository.ObtenerDesembargo());

            string txtClientes = "";
            string txtCuentas = "";

            foreach (ObtenerDesembargoDto desembargo in desembargosDto)
            {
                txtClientes = txtClientes + desembargo.ClienteId + ",";
                txtCuentas = txtCuentas + desembargo.CuentaId + ",";
            }

            Response<IEnumerable<ClienteDto>> clienteDto = await _iClientRequestProvider.ObtenerClientesById(txtClientes);
            Response<IEnumerable<CuentaDto>> cuentas = await _iClientRequestProvider.ObtenerCuentaIdCuenta(txtCuentas);

            List<ObtenerDesembargoDto> list = new List<ObtenerDesembargoDto>();

            foreach (ObtenerDesembargoDto desembargoDto in desembargosDto)
            {
                foreach (ClienteDto cliente in clienteDto.Data)
                {
                    if (cliente.IdCliente == desembargoDto.ClienteId)
                    {
                        desembargoDto.Cliente = cliente.PrimerNombre + " " + cliente.SegundoNombre + " " + cliente.PrimerApellido + " " + cliente.SegundoApellido;
                        break;
                    }
                }

                foreach (CuentaDto cuenta in cuentas.Data)
                {

                    if (cuenta.IdCuenta == desembargoDto.CuentaId)
                    {
                        desembargoDto.Cuenta = cuenta.NumeroCuenta;
                        break;
                    }
                }
                list.Add(desembargoDto);
            }


            return new Response<IEnumerable<ObtenerDesembargoDto>> { Data = list };
        }

        public async Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasPorRadicadoWordManager(string numeroRadicado)
        {

            Embargo embargo = await this._juzgadosRepository.ObtenerEmbargoAdministradorDeTrabajo(numeroRadicado);

            if (embargo != null)
            {

                Response<IEnumerable<CuentaClienteDto>> clienteDto = await _iClientRequestProvider.ObtenerCuentasClienteId(embargo.CLI_ID);
                return clienteDto;

            }
            else
            {

                Response<IEnumerable<CuentaClienteDto>> respuesta = new Response<IEnumerable<CuentaClienteDto>>();
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No hay Embargos con el numero de radicado work manager";
                return respuesta;

            }

        }

        public async Task<Response<DetalleEmbargosDto>> ObtenerConceptosEmbargoById(Guid idEmbargo)
        {

            EmbargosDto embargo = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ObtenerEmbargoPorId(idEmbargo));
            IEnumerable<EmbargoCuentaConceptoDto> conceptos = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await this._juzgadosRepository.ObtenerEmbargoCuentaConceptoPorId(idEmbargo));

            DetalleEmbargosDto detalle = new DetalleEmbargosDto();
            detalle.Embargo = embargo;
            detalle.Conceptos = (List<EmbargoCuentaConceptoDto>)conceptos;

            return new Response<DetalleEmbargosDto> { Data = detalle };
        }

        public async Task<Response<IEnumerable<EmbargoByDto>>> ObtenerEmbargoByIdentificacion(string idCliente)
        {

            IEnumerable<EmbargosDto> embargos = _mapper.Map<IEnumerable<EmbargosDto>>(await this._juzgadosRepository.ObtenerEmbargoByIdentificacion(idCliente));
            Response<IEnumerable<EmbargoByDto>> respuesta = new();

            if (embargos == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El embargo no existe, es posible que lo hayan eliminado";
                return respuesta;
            }

            List<EmbargoByDto> dataembargos = new();
            foreach (var item in embargos)
            {
                IEnumerable<BeneficiariosPagoEmbargoDto> beneficiarios = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargoDto>>(await this._juzgadosRepository.ObtenerBeneficiariosEmbargo(item.Id));
                IEnumerable<EmbargoCuentaConceptoDto> conceptos = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await this._juzgadosRepository.ObtenerEmbargoCuentaConceptoPorId(item.Id));

                EmbargoByDto dataEmbargo = new()
                {
                    Embargo = item,
                    Beneficiarios = (List<BeneficiariosPagoEmbargoDto>)beneficiarios,
                    Conceptos = (List<EmbargoCuentaConceptoDto>)conceptos
                };

                dataembargos.Add(dataEmbargo);
            }

            return new Response<IEnumerable<EmbargoByDto>> { Data = dataembargos };

        }
        private string SerializarEmbargoMapper(InsertEmbargoGestorDocumental embargoMapper)
        {
            return JsonConvert.SerializeObject(embargoMapper);
        }
        public async Task<Response<EmbargosDto>> InsertarEmbargoGestorDocumental(InsertEmbargoGestorDocumentalDto embargo, Guid userId)
        {

            Response<ClienteCesantiasDto> clienteDto = await _iClientRequestProvider.ObtenerInformacionClientePorDocumentoYTipo(embargo.ClienteTipoIdentificacion, embargo.ClienteIdentificacion, 0);
            Response<EmbargosDto> respuesta = new Response<EmbargosDto>();

            if (clienteDto.Data == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existe usuario con este número de identificacion y tipo";
                return respuesta;

            }

            InsertEmbargoGestorDocumental embargoMapper = new InsertEmbargoGestorDocumental();
            embargoMapper.EBA_RADICADO_WORK_MANAGER = embargo.RadicadoWorkManager;
            embargoMapper.EBA_RADICADO_JUZGADO = embargo.RadicadoJuzgado;
            embargoMapper.EBA_FECHA_RADICADO_JUZGADO = DateTime.Now;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.EBA_CREADOPOR = userId;
            embargoMapper.CLI_ID = clienteDto.Data.IdCliente;
            embargoMapper.TID_ID = embargo.ClienteTipoIdentificacion;
            embargoMapper.CLI_IDENTIFICACION = embargo.ClienteIdentificacion;
            embargoMapper.CODIGO_INTERNO = embargo.CodigoInterno;
            embargoMapper.Modulo = embargo.Modulo;
            embargoMapper.EBA_NOMBRE_DEMANDADO = clienteDto.Data.PrimerNombre + " " + clienteDto.Data.SegundoNombre + " " + clienteDto.Data.PrimerApellido + " " + clienteDto.Data.SegundoApellido;

            EmbargosDto tipoEmbargos = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.InsertarEmbargoGestorDocumental(embargoMapper));

            ReportIntegracion integracion = new ReportIntegracion();
            integracion.ID_INTEGRACIONES = Guid.NewGuid();
            integracion.TIPOSERVICIO = "WorkManagent";
            integracion.REQUEST = SerializarEmbargoMapper(embargoMapper);

            await this._auditoria.InsertReportIntegraciones(integracion);

            if (tipoEmbargos == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "Se generó un error al insertar el embargo";
                return respuesta;
            }

            return new Response<EmbargosDto> { Data = tipoEmbargos };
        }

        public async Task<Response<IEnumerable<EmbargoCuentaConceptoDto>>> ObtenerEmbargosConceptoCuentaByCuentaId(int idCuenta)
        {
            var a = new Response<IEnumerable<EmbargoCuentaConceptoDto>>()
            {
                Data = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await _juzgadosRepository.ObtenerEmbargosConceptoCuentaByCuentaId(idCuenta))
            };
            return a;
        }

        public async Task<Response<IEnumerable<JuzgadosDto>>> ObtenerJuzgadosPorDepartamentoCiudadCodigo(JuzgadoDptoCiudadDto juzgadoDptoCiudad)
        {
            IEnumerable<JuzgadosDto> juzgados = _mapper.Map<IEnumerable<JuzgadosDto>>(
                await _juzgadosRepository.ObtenerJuzgadosPorDepartamentoCiudadCodigo(
                    juzgadoDptoCiudad.IdDepartamento,
                    juzgadoDptoCiudad.IdCiudad,
                    juzgadoDptoCiudad.Codigo,
                    juzgadoDptoCiudad.CuentaDeposito,
                    juzgadoDptoCiudad.Estado

                )
            );
            return new Response<IEnumerable<JuzgadosDto>> { Data = juzgados };
        }

        public async Task<Response<EmbargoByDto>> ObtenerEmbargoBy(Guid idEmbargo)
        {

            EmbargosDto embargo = _mapper.Map<EmbargosDto>(await this._juzgadosRepository.ObtenerEmbargoPorId(idEmbargo));
            Response<EmbargoByDto> respuesta = new Response<EmbargoByDto>();

            if (embargo == null)
            {
                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El embargo no existe, es posible que lo hayan eliminado";
                return respuesta;
            }

            EmbargoByDto dataembargo = new EmbargoByDto();
            dataembargo.Embargo = embargo;

            IEnumerable<BeneficiariosPagoEmbargoDto> beneficiarios = _mapper.Map<IEnumerable<BeneficiariosPagoEmbargoDto>>(await this._juzgadosRepository.ObtenerBeneficiariosEmbargo(idEmbargo));
            IEnumerable<EmbargoCuentaConceptoDto> conceptos = _mapper.Map<IEnumerable<EmbargoCuentaConceptoDto>>(await this._juzgadosRepository.ObtenerEmbargoCuentaConceptoPorId(idEmbargo));
            dataembargo.Beneficiarios = (List<BeneficiariosPagoEmbargoDto>)beneficiarios;
            dataembargo.Conceptos = (List<EmbargoCuentaConceptoDto>)conceptos;


            return new Response<EmbargoByDto> { Data = dataembargo };

        }

        public async Task<Response<bool>> AfectacionEmbargoMasivo(EmbargarMasivoDto embargarMasivoDto)
        {
            var AfectacionConcepto = await _iEmbargosProvider.AfectacionEmbargoConcepto(embargarMasivoDto);
            //Validacion 
            var afectacionEmbargo = _mapper.Map<bool>(await this._juzgadosRepository.AfectacionEmbargoMasivo(embargarMasivoDto));



            return new Response<bool> { Data = afectacionEmbargo };

        }


    }
}
