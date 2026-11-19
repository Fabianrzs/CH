using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Embargos;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Application.Main.ProviderRequest;
using GA2.Application.Main.ProviderRequest.Reintegro;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Movimiento = GA2.Domain.Entities.Movimiento;

namespace GA2.Domain.Core
{
    public class CuentasConceptosBusinessLogic: ICuentasConceptosBusinessLogic
    {

        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly ICuentasConceptosRepository _cuentasConceptosRepository;
        private readonly IEmbargosProvider _iEmbargosRequestProvider;
        private readonly IReintegroProviders _reintegroProviders;
        private readonly IBuaService _servicioBua;

        public CuentasConceptosBusinessLogic(IMapper mapper, ICuentasConceptosRepository cuentasConceptosRepository, IEmbargosProvider embargosProvider, IReintegroProviders reintegroProviders, IBuaService buaService)
        {
            _mapper = mapper;
            _cuentasConceptosRepository = cuentasConceptosRepository;
            _iEmbargosRequestProvider = embargosProvider;
            _reintegroProviders = reintegroProviders;
            _servicioBua = buaService;
        }


        public async Task<Response<BloqueoCuentaConceptoDto>> InsertarBoqueoCuentaConcecto(BloqueoCuentaConceptoDto bloqueo)
        {
            BloqueoCuentaConcepto param =this._mapper.Map<BloqueoCuentaConcepto>(bloqueo);
            BloqueoCuentaConceptoDto result= _mapper.Map<BloqueoCuentaConceptoDto>(await _cuentasConceptosRepository.InsertarBoqueoCuentaConcecto(param));
              
            return new Response<BloqueoCuentaConceptoDto> { Data = result };
        }

        public async Task<Response<IEnumerable<CausalBloqueoCuentaDto>>> ObtenerCausalBloqueoCuentasConceptos()
        {
            IEnumerable<CausalBloqueoCuentaDto> causalesBloqueos = _mapper.Map<IEnumerable<CausalBloqueoCuentaDto>>(await _cuentasConceptosRepository.ObtenerCausalBloqueoCuentasConceptos());
            return new Response<IEnumerable<CausalBloqueoCuentaDto>> { Data = causalesBloqueos };
        }

        

        public async Task<Response<IEnumerable<CuentaPorConceptoDto>>> ObtenerCuentaConceptoIdCuenta(int idCuenta)
        {
            IEnumerable<CuentaPorConceptoDto> conceptoCuenta = _mapper.Map<IEnumerable<CuentaPorConceptoDto>>(await _cuentasConceptosRepository.ObtenerCuentaConcepto(idCuenta));
            return new Response<IEnumerable<CuentaPorConceptoDto>> { Data = conceptoCuenta };
        }

        public async Task<Response<InfoClienteDto>> ObtenerDatosAdministracionCuentas(int tpo_identificacion, string num_identificacion)
        {

            InfoCliente reult = await _cuentasConceptosRepository.ObtenerInformacionCliente(tpo_identificacion, num_identificacion);

            InfoClienteDto cliente= _mapper.Map<InfoClienteDto>(reult);

            Response<InfoClienteDto> respuesta = new Response<InfoClienteDto>();

            if (cliente != null)
            {

                IEnumerable<AportesClienteDto> aportes = _mapper.Map<IEnumerable<AportesClienteDto>>(await _cuentasConceptosRepository.ObtenerAportesCategoriaCliente(cliente.ClienteId));
                IEnumerable<CuentaClienteDto> cuentasDto = _mapper.Map<IEnumerable<CuentaClienteDto>>(await _cuentasConceptosRepository.ObtenerCuentasCliente(cliente.ClienteId));
                ObtenerBuaResponse clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2(tpo_identificacion, num_identificacion);
                cliente.Aportes = (List<AportesClienteDto>)aportes;
                var cuentasIntegracion = clienteIntegracionGA2.ObtenerBUAResult.Cuentas;
                List<CuentaClienteDto> listCuentas = new List<CuentaClienteDto>();
                var estadosCuentasGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeCuentasGA2();
                foreach (CuentaClienteDto cuenta in cuentasDto)
                {
                    cuenta.ListaEmbargos = new List<EmbargoCuentaConceptoDto>();
                    IEnumerable<ConceptoCuentaDto> conceptos = _mapper.Map<IEnumerable<ConceptoCuentaDto>>(await _cuentasConceptosRepository.ObtenerConceptosCuentas(cuenta.CuentaId));
                    IEnumerable<MovimientosCuentaDto> movimientos = _mapper.Map<IEnumerable<MovimientosCuentaDto>>(await _cuentasConceptosRepository.ObtenerMovimientosCuentaAfiliado(cuenta.CuentaId));
                    IEnumerable<BloqueoCuentaConceptoDto> bloqueos = _mapper.Map<IEnumerable<BloqueoCuentaConceptoDto>>(await _cuentasConceptosRepository.ObtenerBloqueosCuentasConceptos(cuenta.CuentaId));
                    IEnumerable<InclusionSaldoRequestDto> inclusionSaldos = _mapper.Map<IEnumerable<InclusionSaldoRequestDto>>(await _cuentasConceptosRepository.ObtenerInclusionSaldos(cuenta.CuentaId));
                    foreach (var inclusionSaldo in inclusionSaldos)
                    {
                        inclusionSaldo.Opciones = new();
                        inclusionSaldo.Opciones = _mapper.Map<IEnumerable<InclusionSaldoDto>>(await _cuentasConceptosRepository.ObtenerInclusionSaldoOpciones(inclusionSaldo.IdInclusionSaldoEncabezado)).ToList();
                        inclusionSaldo.Opciones.ForEach(x => x.CuotasAntiguas = x.Cuotas);
                    }
                    double embargos = 0;
                    List<ConceptoCuentaDto> listConceptos = new List<ConceptoCuentaDto>();
                    List<Application.Dto.ConceptosSaldosBloqueos> conceptosIntegracion = new List<Application.Dto.ConceptosSaldosBloqueos>();
                   
                    try
                    {
                        cuenta.Saldo = cuentasIntegracion.Find(x => x.NumeroCuenta == cuenta.NumeroCuenta.ToString()).Saldo;
                        cuenta.SaldoDisponible = cuentasIntegracion.Find(x => x.NumeroCuenta == cuenta.NumeroCuenta.ToString()).SaldoDisponible;
                        conceptosIntegracion = cuentasIntegracion.Find(x => x.NumeroCuenta == cuenta.NumeroCuenta.ToString()).ConceptosSaldosBloqueos;

                    }
                    catch
                    {
                        cuenta.Saldo = 0;
                        cuenta.SaldoDisponible = 0;
                    }
                   
                    decimal saldoDisponible = 0;
                    decimal saldo=0;
                    decimal valorTotalBloqueo = 0;
                    var conceptosCuenta = conceptos.GroupBy(x => x.ConceptoId);
                    foreach (var conceptoCuenta in conceptosCuenta) {
                        decimal valorBloqueo=0;
                        saldo += Convert.ToDecimal(conceptoCuenta.First().saldo);
                        saldoDisponible += Convert.ToDecimal(conceptoCuenta.First().saldo);
                        foreach (var concepto in conceptoCuenta)
                        {
                            IEnumerable<MovimientosCuentaDto> movimientosconceptos = _mapper.Map<IEnumerable<MovimientosCuentaDto>>(await _cuentasConceptosRepository.ObtenerMovimientosConceptos(concepto.CuentaId, concepto.ConceptoId));
                            concepto.Movimientos = (List<MovimientosCuentaDto>)movimientosconceptos;

                            IEnumerable<EmbargoCuentaConceptoDto> embargoCuentaConceptos = _iEmbargosRequestProvider.ObtenerEmbargoByConceptoCuentaId(concepto.Id).Result.Data;
                            if (embargoCuentaConceptos.Count() > 0)
                            {
                                cuenta.ListaEmbargos = cuenta.ListaEmbargos.Concat(embargoCuentaConceptos).ToList();
                                embargos += embargoCuentaConceptos.First().Valor;
                                concepto.ValorEmbargo = (decimal)embargoCuentaConceptos.First().Valor;
                                saldoDisponible -= concepto.ValorEmbargo;
                            }
                            try
                            {
                                concepto.ValorBloqueoEmbargo = conceptosIntegracion.Find(x => x.IdConcepto == concepto.ConceptoId)?.ValorAcumulado ?? 0;
                            }
                            catch
                            {
                                concepto.ValorBloqueoEmbargo = 0;
                            }
                            saldoDisponible -= concepto.ValorBloqueoEmbargo;
                            valorTotalBloqueo+= concepto.ValorBloqueoEmbargo;
                            valorBloqueo+= concepto.ValorBloqueoEmbargo;
                        }
                        conceptoCuenta.First().ValorBloqueoEmbargo= valorBloqueo;
                        conceptoCuenta.First().ValorEmbargo = valorBloqueo;
                        listConceptos.Add(conceptoCuenta.First());
                    }
                    cuenta.Saldo=saldo;
                    cuenta.SaldoDisponible = saldoDisponible;
                    cuenta.Embargos = embargos;
                    cuenta.Conceptos = listConceptos;
                    cuenta.Movimientos = (List<MovimientosCuentaDto>)movimientos;
                    cuenta.Bloqueos = bloqueos.ToList();
                    cuenta.InclusionSaldos = inclusionSaldos.ToList();
                    cuenta.EstadoCuenta = estadosCuentasGA2.Data.First(x => x.IdEstado == cuenta.EstadoCuentaId).Estado;
                    listCuentas.Add(cuenta);
                }

                cliente.Cuentas = listCuentas;

                return new Response<InfoClienteDto> { Data = cliente };
            }
            else {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El cliente no existe";
                return respuesta;

            }

        }

        public async Task<Response<IEnumerable<CuentaClienteDto>>> ObtenerCuentasCliente(int id)
        {

            IEnumerable<CuentaClienteDto> cuentasDto = _mapper.Map<IEnumerable<CuentaClienteDto>>(await _cuentasConceptosRepository.ObtenerCuentasCliente(id));

            return new Response<IEnumerable<CuentaClienteDto>> { Data = cuentasDto };
          
        }

        public async Task<Response<CuentaPorConceptoDto>> ObtenerConceptoPorClientePorCuentaPorConcepto(int clienteId, int cuentaId, int conceptoId)
        {

            CuentaPorConceptoDto cuentasDto = _mapper.Map<CuentaPorConceptoDto>(await _cuentasConceptosRepository.ObtenerConceptoPorClientePorCuentaPorConcepto(clienteId,cuentaId,conceptoId));
            return new Response<CuentaPorConceptoDto> { Data = cuentasDto };
        }

        public async Task<Response<InfoClienteDto>> ObtenerCuentasConceptosPorTipoNumeroIdentidad(int tpo_identificacion, string num_identificacion)
        {
            InfoCliente reult = await _cuentasConceptosRepository.ObtenerInformacionCliente(tpo_identificacion, num_identificacion);
            InfoClienteDto cliente = _mapper.Map<InfoClienteDto>(reult);
            Response<InfoClienteDto> respuesta = new Response<InfoClienteDto>();



            if (reult==null) {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No hay cliente registrado con el tipo y número de identificación proporcionado.";
                return respuesta;

            }

          

            IEnumerable<CuentaClienteDto> cuentasDto = _mapper.Map<IEnumerable<CuentaClienteDto>>(await _cuentasConceptosRepository.ObtenerCuentasCliente(reult.CLI_ID));
            List<CuentaClienteDto> listConceptos = new List<CuentaClienteDto>();

            foreach (CuentaClienteDto cuenta in cuentasDto)
            {
                IEnumerable<ConceptoCuentaDto> conceptos = _mapper.Map<IEnumerable<ConceptoCuentaDto>>(await _cuentasConceptosRepository.ObtenerConceptosCuentas(cuenta.CuentaId));
                cuenta.Conceptos = (List<ConceptoCuentaDto>)conceptos;
                listConceptos.Add(cuenta);
            }

            cliente.Cuentas = listConceptos;
           
            return new Response<InfoClienteDto> { Data = cliente };

        }

        public  async Task<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>> ObtenerDatosVerificacionDerechosCuentas(VerificacionDerechosByFechaDto request)
        {
            try
            {
                IEnumerable<DatosVerificacionDerechoCuentaDto> cuentasDto = _mapper.Map<IEnumerable<DatosVerificacionDerechoCuentaDto>>(await _cuentasConceptosRepository.ObtenerDatosVerificacionDerechosCuentas(request));
                return new Response<IEnumerable<DatosVerificacionDerechoCuentaDto>> { Data = cuentasDto };
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async Task<Response<DatosVerificacionDerechoCuentaDto>> ActualizarECTCuentas(ActualizarEctCuentasDto request)
        {
            DatosVerificacionDerechoCuentaDto cuentasDto = _mapper.Map<DatosVerificacionDerechoCuentaDto>(await _cuentasConceptosRepository.ActualizarECTCuentas(request));

            return new Response<DatosVerificacionDerechoCuentaDto>
            { 
                
                Data = cuentasDto 
            };
        }
        public async Task<Response<List<DatosVerificacionDerechoCuentaDto>>> ActualizarECTCuentasList(List<ActualizarEctCuentasDto> request)
        {
            var cuentasDto = new List<DatosVerificacionDerechoCuentaDto>();
            await _cuentasConceptosRepository.ActualizarECTCuentasList(request);    
            return new Response<List<DatosVerificacionDerechoCuentaDto>>
            {

                Data = cuentasDto
            };
        }

        public async Task<Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>>> ObtenerDatosCuentasAdministracionCesantias(VerificacionDerechosByFechaDto request)
        {
            IEnumerable<DatosCuentasAdministracionCesantiasDto> cuentasDto = _mapper.Map<IEnumerable<DatosCuentasAdministracionCesantiasDto>>(await _cuentasConceptosRepository.ObtenerDatosCuentasAdministracionCesantias(request));
            return new Response<IEnumerable<DatosCuentasAdministracionCesantiasDto>> { Data = cuentasDto };
        }

        public async Task<Response<IEnumerable<DatosCuentasAVDto>>> ObtenerDatosCuentasAV(VerificacionDerechosByFechaDto request)
        {
            IEnumerable<DatosCuentasAVDto> cuentasDto = _mapper.Map<IEnumerable<DatosCuentasAVDto>>(await _cuentasConceptosRepository.ObtenerDatosCuentasAV(request));
            return new Response<IEnumerable<DatosCuentasAVDto>> { Data = cuentasDto };
        }


        public async Task<Response<IEnumerable<ObtenerCuentasAVACDto>>> ObtenerDatosCuentasAVAC(VerificacionDerechosByFechaDto request)
        {
            IEnumerable<ObtenerCuentasAVACDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentasAVACDto>>(await _cuentasConceptosRepository.ObtenerDatosCuentasAVAC(request));
            return new Response<IEnumerable<ObtenerCuentasAVACDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ObtenerCuentasAVAVDto>>> ObtenerDatosCuentasAVAV(VerificacionDerechosByFechaDto request)
        {
            IEnumerable<ObtenerCuentasAVAVDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentasAVAVDto>>(await _cuentasConceptosRepository.ObtenerDatosCuentasAVAV(request));
            return new Response<IEnumerable<ObtenerCuentasAVAVDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVAV()
        {
            IEnumerable<ObtenerCuentaAVAVResponseDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentaAVAVResponseDto>>(await _cuentasConceptosRepository.ObtenerCedulaCuentaAVAV());
            return new Response<IEnumerable<ObtenerCuentaAVAVResponseDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVAC()
        {
            IEnumerable<ObtenerCuentaAVAVResponseDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentaAVAVResponseDto>>(await _cuentasConceptosRepository.ObtenerCedulaCuentaAVAC());
            return new Response<IEnumerable<ObtenerCuentaAVAVResponseDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ObtenerCuentaAVAVResponseDto>>> ObtenerCedulaCuentaAVC()
        {
            IEnumerable<ObtenerCuentaAVAVResponseDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentaAVAVResponseDto>>(await _cuentasConceptosRepository.ObtenerCedulaCuentaAVC());
            return new Response<IEnumerable<ObtenerCuentaAVAVResponseDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ObtenerCuentasAVCDto>>> ObtenerDatosCuentasAVC(VerificacionDerechosByFechaDto request)
        {
            IEnumerable<ObtenerCuentasAVCDto> cuentasDto = _mapper.Map<IEnumerable<ObtenerCuentasAVCDto>>(await _cuentasConceptosRepository.ObtenerDatosCuentasAVC(request));
            return new Response<IEnumerable<ObtenerCuentasAVCDto>> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<DatosCedulasCesantiasDto>>> ObtenerCedulaCuentaCesantidas()
        {
            IEnumerable<DatosCedulasCesantiasDto> cuentasDto = _mapper.Map<IEnumerable<DatosCedulasCesantiasDto>>(await _cuentasConceptosRepository.ObtenerCedulaCuentaCesantidas());
            return new Response<IEnumerable<DatosCedulasCesantiasDto>> { Data = cuentasDto };
        }
        public async Task<Response<DatosInfoCensatiaByCuentaPeriodoDto>> ObtenerDatosCesantiasByCuentaId(ObtenerInfoCedulaRequestDto request)
        {
            var requestMap = _mapper.Map<ObtenerInfoCedulaRequest>(request);
            var cuentasDto = _mapper.Map< DatosInfoCensatiaByCuentaPeriodoDto> (await _cuentasConceptosRepository.ObtenerDatosCesantiasByCuentaId(requestMap));
            return new Response<DatosInfoCensatiaByCuentaPeriodoDto> { Data = cuentasDto };
        }
        public async Task<Response<IEnumerable<ConceptoCuentaDto>>> ActualizarSaldoConceptoCuenta(List<CuentaConceptoDto> conceptos)
        {
            var resultCuenta = new List<ConceptoCuentaDto>();
            var conceptoList = new List<ConceptoCuenta>();
            foreach (var item in conceptos)
            {
                var bloqeo = item.TipoReintegro == 2 ? item.ValorSueldo : 0;
                var valorDevolver = item.TipoReintegro == 3 ? item.ValorSueldo : 0;


                if (item.TipoValor == "VALOR")
                {
                    var responseConcepto = await _cuentasConceptosRepository.validarSaldoCuentaConcepto(item.ConceptoId, item.CuentaId);
                    var valorBloqueo = bloqeo > 0 ? bloqeo + (double)responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO : bloqeo;
                    var valorDebloqueo = valorDevolver > 0 ? responseConcepto.CCT_SALDO + valorDevolver : valorDevolver;
                    //if (conceptos.Count == 1)
                    //{   
                    if (responseConcepto is not null)
                    {
                        if (item.ValorSueldo == responseConcepto.CCT_SALDO || item.ValorSueldo < responseConcepto.CCT_SALDO)
                        {
                            if (item.TipoReintegro == 2)
                            {
                                var newValueSaldo = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId,newValueSaldo, valorBloqueo, item.TipoReintegro);
                                var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                         //      var result = await _cuentasConceptosRepository.CreateLogMovimientoCuenta(item.ConceptoId, item.CuentaId, valorBloqueo, responseConcepto.CCT_SALDO, item.TipoReintegro);
                                await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                resultCuenta.Add(resultMap);
                            }
                            if (item.TipoReintegro == 3)
                            {

                                    var valorBloque = responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - (decimal)item.ValorSueldo;
                                    var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId, (double)item.ValorSueldo, (double)valorBloque, item.TipoReintegro);
                                    var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                    await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                    resultCuenta.Add(resultMap);

                            }
                            if (item.TipoReintegro == 1)
                            {
                                if (responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO > 0)
                                {
                                    var newValueDevolver = item.ValorSueldo;
                                    var valorDescontarDesbloqueo = (double)responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - item.ValorSueldo;
                                    double valorBloqueoNuevo = 0;
                                    double valorDescontarDisponible = 0;
                                    if(valorDescontarDesbloqueo < 0)
                                    {
                                        valorDescontarDesbloqueo = valorDescontarDesbloqueo * -1;
                                        valorDescontarDisponible = responseConcepto.CT_VALOR_DISPONIBLE - valorDescontarDesbloqueo;
                                        valorBloqueoNuevo = 0;
                                    }
                                    else
                                    {
                                        valorDescontarDisponible = responseConcepto.CT_VALOR_DISPONIBLE;
                                        valorBloqueoNuevo = valorDescontarDesbloqueo;
                                    }
                                    var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, valorBloqueoNuevo, valorDescontarDisponible);
                                    var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                    await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                    resultCuenta.Add(resultMap);
                                }
                                else
                                {
                                    var newValueDevolver = item.ValorSueldo;
                                    var newValueDisponible = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                    var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, 0, newValueDisponible);
                                    var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                    await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                    resultCuenta.Add(resultMap);
                                }
                            }
                            else
                            {
                                if (item.ValorSueldo > responseConcepto.CCT_SALDO)
                                {
                                    var nuevoValorDevolver = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                    if (nuevoValorDevolver < 0)
                                    {
                                        nuevoValorDevolver = 0;
                                        item.ValorSueldo = responseConcepto.CCT_SALDO;
                                    }
                                    if (item.TipoReintegro == 2)
                                    {
                                        var newValueSaldo = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                        var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId, newValueSaldo, valorBloqueo, item.TipoReintegro);
                                        var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                        await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                        resultCuenta.Add(resultMap);
                                      //  var result = await _cuentasConceptosRepository.CreateLogMovimientoCuenta(item.ConceptoId, item.CuentaId, valorBloqueo, responseConcepto.CCT_SALDO, item.TipoReintegro);
                                    }
                                    if (item.TipoReintegro == 3)
                                    {
                                        var valorBloque = responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - (decimal)item.ValorSueldo;
                                        var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId, (double)valorBloque, (double)valorBloque, item.TipoReintegro);
                                        var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                        await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                        resultCuenta.Add(resultMap);
                                    }
                                    if (item.TipoReintegro == 1)
                                    {
                                        if (responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO > 0)
                                        {
                                            var newValueDevolver = item.ValorSueldo;
                                            var newValueDisponible = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                            var newValorBlooque = (double)responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - item.ValorSueldo;
                                            var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, newValorBlooque, newValueDisponible);
                                            var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                            await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                            resultCuenta.Add(resultMap);
                                        }
                                        else
                                        {
                                            var newValueDevolver = item.ValorSueldo;
                                            var newValueDisponible = responseConcepto.CCT_SALDO - item.ValorSueldo;
                                            var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, 0, newValueDisponible);
                                            var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                            await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)item.ValorSueldo);
                                            resultCuenta.Add(resultMap);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else if (item.TipoValor == "PORCENTAJE")
                {
                    var responseConcepto = await _cuentasConceptosRepository.validarSaldoCuentaConcepto(item.ConceptoId, item.CuentaId);
                    if (responseConcepto is not null)
                    {
                        var porcentaje = item.ValorSueldo / 100;
                        var valorSueldo = valorDevolver > 0 ? (double)responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO * porcentaje : 0;
                        var Valorbloqeo = bloqeo > 0 ? (responseConcepto.CCT_SALDO * bloqeo) / 100 : 0;
                        if (item.ValorSueldo > responseConcepto.CCT_SALDO)
                        {
                            valorSueldo = responseConcepto.CCT_SALDO;
                        }
                        if (item.TipoReintegro == 1)
                        {
                            if (responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO > 0)
                            {
                                var newValueDevolver = porcentaje * responseConcepto.CCT_SALDO;
                                var newValueDisponible = responseConcepto.CT_VALOR_DISPONIBLE - newValueDevolver;
                                var newValorBlooque = (double)responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - newValueDevolver;
                                if (newValorBlooque < 0)
                                {
                                    newValorBlooque = newValorBlooque * -1;
                                    newValueDisponible = responseConcepto.CT_VALOR_DISPONIBLE - newValorBlooque;
                                    newValorBlooque = 0;
                                }
                                else
                                {
                                    newValueDisponible = responseConcepto.CT_VALOR_DISPONIBLE;
                                }
                                var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, newValorBlooque, newValueDisponible);
                                var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)newValueDevolver);
                                resultCuenta.Add(resultMap);
                            }
                            else
                            {
                                var newValueDevolver = porcentaje * responseConcepto.CCT_SALDO;
                                var newValueDisponible = responseConcepto.CCT_SALDO - newValueDevolver;
                                var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolverDR(item.ConceptoId, item.CuentaId, newValueDevolver, 0, newValueDisponible);
                                var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                                await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)newValueDevolver);
                                resultCuenta.Add(resultMap);
                            }
                        }
                        if (item.TipoReintegro == 2)
                        {

                            var newValueSaldo = responseConcepto.CCT_SALDO - Valorbloqeo;
                            var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId, newValueSaldo, Valorbloqeo, item.TipoReintegro);
                            var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                            await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)Valorbloqeo);
                            await _reintegroProviders.ActualizarValorSaldoPorcentaje(item.IdAfiliado, (decimal)item.ValorSueldo);
                            resultCuenta.Add(resultMap);
                        }
                        if (item.TipoReintegro == 3)
                        {
                            var valorBloque = responseConcepto.CCT_VALOR_BLOQUEO_EMBARGO - (decimal)valorSueldo;
                            var updateSueldo = await _cuentasConceptosRepository.ActualizarSaldoAndValorDevolver(item.ConceptoId, item.CuentaId, (double)valorSueldo, (double)valorBloque, item.TipoReintegro);
                            var resultMap = _mapper.Map<ConceptoCuentaDto>(updateSueldo);
                            await _reintegroProviders.ActualizarValorSaldo(item.IdAfiliado, (decimal)valorSueldo);
                            await _reintegroProviders.ActualizarValorSaldoPorcentaje(item.IdAfiliado, (decimal)item.ValorSueldo);
                            resultCuenta.Add(resultMap);
                        }
                    }

                }
            }
            if (resultCuenta.Any())
            {
                return new Response<IEnumerable<ConceptoCuentaDto>> { Data = resultCuenta };
            }
            return new Response<IEnumerable<ConceptoCuentaDto>> { Data = null };

        }

        private async Task<ObtenerBuaResponse> ObtenerClienteIntegracionGA2(int identificationType, string identificationNumber)
        {
            dynamic responseIntegracion = default;

            //#if DEBUG

            //_logger.LogInformation("Pasa por el debug");
            //responseIntegracion = await _buaIntegraciones.ObtenerBua(new()
            //{
            //    identificacion = identificationNumber,
            //    tipo_Id = identificationType
            //});

            //responseIntegracion = JsonConvert.DeserializeObject<Response<ObtenerBuaResponse>>(responseIntegracion.Data).Data;
            //#else
            //_logger.LogInformation("pasa por produccuin");
            responseIntegracion = await _servicioBua.ObtenerAfiliadoBuaCH(new()
            {
                identificacion = identificationNumber,
                tipo_Id = identificationType,
            });

            //#endif


            if (responseIntegracion is null) { return null; }
            ObtenerBuaResponse response = new();
            string responseJson = JsonConvert.SerializeObject(responseIntegracion.Data);
            response.ObtenerBUAResult = JsonConvert.DeserializeObject<ClienteIntegracionGA2DTO>(responseJson);
            if (response.ObtenerBUAResult != null)
            {
                PrepararClienteIntegracion(response);
                response.ObtenerBUAResult.Payload = JsonConvert.SerializeObject(responseIntegracion);
            }
            return response;
        }
        public ObtenerBuaResponse PrepararClienteIntegracion(ObtenerBuaResponse clienteIntegracionGA2)
        {
            if (clienteIntegracionGA2 != null)
            {
                if (clienteIntegracionGA2.ObtenerBUAResult != null)
                {
                    if (clienteIntegracionGA2.ObtenerBUAResult.Direccion != null)
                    {
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TelefonoMostrar = clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "" && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "0" ?
                                                                            clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular : "601" + clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TipoTelefono = clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "" && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Celular != "0" ?
                                                                            2 : clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.Telefono != "" ? 1
                                                                            : 0;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.Correo = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != "" ?
                                                    clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional : clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal;
                        clienteIntegracionGA2.ObtenerBUAResult.Direccion.TipoCorreo = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional != "" ?
                                                                            2 : clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal != "" ? 1
                                                                            : 0;
                        if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.LugarExpedicion.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }

                        if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.CiudadNacimiento.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }

                        if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia != null)
                        {
                            clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudadEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad != "" ?
                                                                                                Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.IdCiudad) : 0;

                            if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento != null)
                            {
                                clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamentoEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento != "" ?
                                                                                                    Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.IdDepartamento) : 0;

                                if (clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais != null)
                                {
                                    clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPaisEntero = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais != null && clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais != "" ?
                                                                                                        Convert.ToInt32(clienteIntegracionGA2.ObtenerBUAResult.Direccion.CiudadResidencia.Departamento.Pais.IdPais) : 0;

                                }
                            }
                        }
                    }
                }
            }

            return clienteIntegracionGA2;
        }

        public async Task<Response<bool>> AfectacionConceptoMasivoEmbargo(EmbargarMasivoDto embargarMasivoDTO)
        {
            foreach (var item in embargarMasivoDTO.ListaMasiva)
            {

                var responseConcepto = await _cuentasConceptosRepository.AfectacionConceptoMasivoEmbargo(item.CTA_ID, item.CNC_ID ,item.CCT_SALDO);

            }

            return new Response<bool> { Data = true };

        }
        public async Task<Response<string>> ClealConceptoCuenta(List<ConceptoCuentaCleanDto> conceptos)
        {
            var resultCuenta = new List<int>();   
            if (conceptos.Count >= 1)
            {
                foreach (var item in conceptos)
                {
                    if (item.IdReintegro == 2)
                    {
                        var dataConcepto = await _cuentasConceptosRepository.validarSaldoCuentaConcepto(item.ConceptoId, item.CuentaId);
                        if (dataConcepto is not null)
                        {
                            //SI TIENE VALOR DISPONIBLE Y ES UN BLOQUEO ES POR PUEDA QUE TENGA OTRA SOLICITUD
                            if (dataConcepto.CT_VALOR_DISPONIBLE > 0)
                            {
                                var dataLog = await _cuentasConceptosRepository.GetDataLogConceptoCuenta(item.ConceptoId, item.CuentaId);
                                if (dataLog is not null)
                                {
                                    var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, dataLog.ValorBloque, dataLog.ValorSaldo, item.IdReintegro);
                                }
                                else
                                {
                                    var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, 0, 0, item.IdReintegro);
                                }
                            }
                            else
                            {
                                var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, 0, 0, item.IdReintegro);
                            }
                        }
                    }
                    if (item.IdReintegro == 3)
                    {
                        var dataLog = await _cuentasConceptosRepository.GetDataLogConceptoCuenta(item.ConceptoId, item.CuentaId);
                        if (dataLog is not null)
                        {
                            if (dataLog.IdTipoReintegro == 2)
                            {
                                var dataConcepto = await _cuentasConceptosRepository.validarSaldoCuentaConcepto(item.ConceptoId, item.CuentaId);
                                if (dataConcepto is not null)
                                {
                                    var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, dataLog.ValorBloque, dataLog.ValorSaldo, item.IdReintegro);
                                }
                            }
                        }
                    }
                    if (item.IdReintegro == 1)
                    {
                        var dataLog = await _cuentasConceptosRepository.GetDataLogConceptoCuenta(item.ConceptoId, item.CuentaId);
                        if (dataLog is not null)
                        {
                            var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, dataLog.ValorBloque, dataLog.ValorSaldo, item.IdReintegro);
                        }
                        else
                        {
                            var dataConcepto = await _cuentasConceptosRepository.validarSaldoCuentaConcepto(item.ConceptoId, item.CuentaId);
                            if (dataConcepto.CCT_VALOR_DEVOLVER > 0)
                            {
                                var dataResponseConcepto = await _cuentasConceptosRepository.ActualizarValorRegresar(item.ConceptoId, item.CuentaId, 0, (double)dataConcepto.CCT_VALOR_DEVOLVER, item.IdReintegro);
                            }
                        }
                    }
                }
                var idAfiliado = conceptos.Select(x => x.IdAfiliado).First();
                await _reintegroProviders.CleanValorSaldo(idAfiliado, 0 );
                return new Response<string> { Data = "Limpiado exitosamente", IsSuccess = true, ReturnMessage = "OK" };
            }

            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Fallido" };

        }

        public async Task<Response<string>> ProcesarSaldoConceptoCuenta(List<ConceptoUpdate> conceptos)
        {
            if (conceptos.Any())
            {
                foreach (var item in conceptos)
                {
                    await _cuentasConceptosRepository.ProcesarSaldoReintegro(item.ConceptoId, item.CuentaId, item.Saldo);
                    if (item.ValorDevolver > 0)
                    {
                        var data = await _cuentasConceptosRepository.InsertarMovimientoReitegroUnidadEjecutora(new Movimiento
                        {
                            CAT_TIPO_MOVIMIENTO = "D",
                            CTA_ID = item.CuentaId,
                            CNC_ID = item.ConceptoId,
                            MVT_FECHA_MOVIMIENTO = DateTime.Now,
                            MVT_FECHA_PROCESO = DateTime.Now,
                            MVT_VALOR = (decimal)item.ValorDevolver,
                            MVT_ANO_PERIODO = DateTime.Now.Year.ToString()
                        });
                        var dataResponse = await _cuentasConceptosRepository.InsertarLogMovimientoReintegro(new Movimiento
                        {
                            CTA_ID = data.CTA_ID,
                            CNC_ID = data.CNC_ID,
                            MVT_ID = data.MVT_ID,
                            DCT_ID = data.DCT_ID,
                            MVT_VALOR = (decimal)item.ValorDevolver,
                            MVT_ANO_PERIODO = DateTime.Now.Year.ToString()
                        });
                    }

                }
                
                return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "OK" };
            }
            return new Response<string> { Data = null, IsSuccess = false, ReturnMessage = "Fallido" };

        }

        public async Task<Response< List<ConceptoCuentaDto>>> CrearLogMovimientoCuenta(List<LogMovimientoDto> conceptos)
        {
            var listData = new List<ConceptoCuentaDto>();
            var insertConcepto= new ConceptoCuenta();
            foreach (var item in conceptos)
            {
                if (item.IdTipoReintegro == 2 && item.ValorBloque != 0)
                {
                     insertConcepto = await _cuentasConceptosRepository.CreateLogMovimientoCuenta
                                             (item.ConceptoId,
                                             item.CuentaId,
                                             item.ValorBloque,
                                             item.ValorSaldo,
                                             item.IdTipoReintegro
                                             );
                }
                else
                {
                    if (item.IdTipoReintegro == 3 || item.IdTipoReintegro == 1)
                    {
                        insertConcepto = await _cuentasConceptosRepository.CreateLogMovimientoCuenta
                                                (item.ConceptoId,
                                                item.CuentaId,
                                                item.ValorBloque,
                                                item.ValorSaldo,
                                                item.IdTipoReintegro
                                                );
                    }
                }


                var map = _mapper.Map<ConceptoCuentaDto>(insertConcepto);
                listData.Add(map);
            }
            
            return new Response<List<ConceptoCuentaDto>> { Data = listData};

        }

    }
}
