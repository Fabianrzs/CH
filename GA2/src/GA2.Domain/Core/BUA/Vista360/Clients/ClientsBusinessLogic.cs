using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Dto.Credito;
using GA2.Application.Main.ProviderRequest;
using GA2.Domain.Core.BuaServiceSvc;
using GA2.Domain.Core.Connected_Services.ServiceReference1;
using GA2.Domain.Core.Servicios.Bua;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Reportes;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GA2.Domain.Core
{
    /// <summary>
    /// Client Core Negocio
    /// <author>Nicolas FLorez Sarrazola</author>
    /// <date>24/02/2021</date>
    /// </summary>
    public class ClientesBusinessLogic : IClientesBusinessLogic
    {
        /// <summary>
        /// Propiedades privadas Core Negocio
        /// </summary>
        private readonly IMapper _mapper;
        private readonly IClienteRepository _clientsRepository;
        private readonly IBuaService _servicioBua;
        private readonly IIntegracionesGa2BuaBusinessLogic _buaIntegraciones;
        private readonly ILogger<ClientesBusinessLogic> _logger;
        private readonly ICuentasConceptosRepository _cuentasConceptosRepository;
        private readonly IEmbargosProvider _iEmbargosRequestProvider;
        private readonly IBuaService2BusinessLogic _IBuaService2BusinessLogic;


        /// <summary>
        /// Constructor Negocio Clients
        /// </summary>
        /// <param name="mapper">Instancia mapper</param>
        /// <param name="clientsRepository">Instancia repositorio usuarios</param>
        public ClientesBusinessLogic(IMapper mapper,
                                     IClienteRepository clientsRepository,
                                     IIntegracionesGa2BuaBusinessLogic buaIntegraciones,
                                     IBuaService servicioBua,
                                     ILogger<ClientesBusinessLogic> logger,
                                     ICuentasConceptosRepository cuentasConceptosRepository,
                                     IEmbargosProvider embargosProvider,
                                     IBuaService2BusinessLogic IBuaService2BusinessLogic
            )
        {
            _mapper = mapper;
            _clientsRepository = clientsRepository;
            _buaIntegraciones = buaIntegraciones ?? throw new ArgumentNullException(nameof(buaIntegraciones));
            _servicioBua = servicioBua ?? throw new ArgumentNullException(nameof(servicioBua));
            _cuentasConceptosRepository = cuentasConceptosRepository;
            _iEmbargosRequestProvider = embargosProvider;
            _logger = logger;
            _IBuaService2BusinessLogic = IBuaService2BusinessLogic;
        }

        /// <summary>
        /// Metodo obtener info cliente
        /// </summary>
        /// <param name="Cliente">Modelo dto cliente</param>
        /// <author>ONicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<ClienteDto>> ObtenerClienteInformacion(int Id)
        {
            var usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClienteInformacion(Id));
            usuarios.Saldos = new();
            var listSaldos = await BuscarSaldos(usuarios.Identificacion);

            foreach (var saldo in listSaldos)
                usuarios.Saldos.Add(saldo);

            return new Response<ClienteDto> { Data = usuarios };
        }

        public async Task<Response<ClienteReporteDto>> ObtenerClienteReporte(string Identificacion)
        {
            ClienteReporteDto usuarios = new ClienteReporteDto();
            usuarios = this._mapper.Map<ClienteCedulaReporte, ClienteReporteDto>(
                await _clientsRepository.ObtenerClienteReporte(Identificacion));
            return new Response<ClienteReporteDto> { Data = usuarios };
        }

        public async Task<Response<ClienteCedulaDto>> ObtenerInformacionClienteCedula(int identificacionCliente)
        {
            ClienteCedulaDto usuarios = new ClienteCedulaDto();
            usuarios = this._mapper.Map<ClienteCedula, ClienteCedulaDto>(await _clientsRepository.ObtenerInformacionClienteCedula(identificacionCliente));
            return new Response<ClienteCedulaDto> { Data = usuarios };
        }

        /// <summary>
        /// Metodo actualizar info cliente
        /// </summary>
        /// <author>ONicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <param name="cliente"></param>
        /// <returns>Modelo Dto</returns>
        public async Task<Response<ClienteDto>> ActualizarCliente(ClienteDto client)
        {
            var cliente = this._mapper.Map<Cliente>(client);
            var datosAnteriores = await ObtenerClientePorTipoIdentificationYNumero(cliente.TID_ID, cliente.CLI_IDENTIFICACION, null);
            return new Response<ClienteDto> { Data = this._mapper.Map<ClienteDto>(await this._clientsRepository.ActualizarCliente(cliente, datosAnteriores.Data)) };
        }
        /// <summary>
        /// Obtener la informacion de la navegacion
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public UserAgentInfo ParseUserAgent(string userAgent)
        {
            string browserPattern = @"(Chrome|Firefox|Safari|Opera|Edge)/(\d+\.\d+\.\d+\.\d+)";
            string osPattern = @"\((Windows NT \d+\.\d+);.*\)";

            var browserMatch = Regex.Match(userAgent, browserPattern);
            var osMatch = Regex.Match(userAgent, osPattern);

            string browser = "Unknown";
            string browserVersion = "Unknown";
            string os = "Unknown";
            string osVersion = "Unknown";

            if (browserMatch.Success)
            {
                browser = browserMatch.Groups[1].Value;
                browserVersion = browserMatch.Groups[2].Value;
            }

            if (osMatch.Success)
            {
                os = osMatch.Groups[1].Value;
                osVersion = osMatch.Groups[1].Value;
            }

            return new UserAgentInfo
            {
                Browser = browser,
                BrowserVersion = browserVersion,
                OperatingSystem = os,
                OsVersion = osVersion
            };
        }
        /// <summary>
        /// Obtener la ip 
        /// </summary>
        /// <returns></returns>
        public string Ip()
        {
            string ip;
            var ipList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            ip = ipList[1].ToString();

            return ip;
        }

        /// <summary>
        /// Metodo Obtener cliente validacion nomina
        /// </summary>
        /// <author>nn</author>
        /// <date>08/05/2024</date>
        /// <param name="cliente"></param>
        /// <returns>Modelo Dto</returns>
        public async Task<Response<ClienteNominaDto>> ObtenerClienteNomina(int identificationType, string identificationNumber)
        {
            ClienteNominaDto usuarios = null;
            //ClienteDto usuarios2 = null;
            ObtenerBuaResponse clienteIntegracionGA2 = null;
            try
            {
                usuarios = this._mapper.Map<ClienteNomina, ClienteNominaDto>(await _clientsRepository.ObtenerClienteNomina(identificationNumber));
                if (usuarios != null)
                {
                    if (identificationType == 9)
                    {
                        identificationType = usuarios.IdTipoIdentificacion;
                        identificationNumber = usuarios.Identificacion;
                    }
                }


                if (usuarios == null)
                {
                    clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2(identificationType, identificationNumber);

                    if (clienteIntegracionGA2.ObtenerBUAResult == null)
                    {
                        return new Response<ClienteNominaDto> { IsSuccess = false, ReturnMessage = "Error: No se pudo obtener información del cliente." };
                    }
                    //usuarios2 = this._mapper.Map<ClienteDto>(clienteIntegracionGA2.ObtenerBUAResult);
                    //usuarios = this._mapper.Map<ClienteDto, ClienteNominaDto> (usuarios2);
                    usuarios = new ClienteNominaDto
                    {
                        IdCliente = clienteIntegracionGA2.ObtenerBUAResult.IdRegistro,
                        Identificacion = clienteIntegracionGA2.ObtenerBUAResult.Identificacion,
                        IdFuerza = clienteIntegracionGA2.ObtenerBUAResult.Grado.Fuerza.IdFuerza,
                        IdGrado = clienteIntegracionGA2.ObtenerBUAResult.Grado.IdGrado,
                        IdUnidadEjecutora = clienteIntegracionGA2.ObtenerBUAResult.UnidadEjecutora,
                        IdEstadoCliente = clienteIntegracionGA2.ObtenerBUAResult.EstadoActualCli.IdEstado,

                    };
                }
                var cuentas4 = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(usuarios.IdCliente));

                IEnumerable<CuentaClienteDto> cuentasCliente = _mapper.Map<IEnumerable<CuentaClienteDto>>(await _cuentasConceptosRepository.ObtenerCuentasCliente(usuarios.IdCliente));

                if (cuentasCliente.Count() == 0)
                {
                    clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2(identificationType, identificationNumber);


                    usuarios.Cuentas = clienteIntegracionGA2.ObtenerBUAResult.Cuentas
                                        .Select(x =>
                                            new CuentaClienteDto
                                            {
                                                CuentaId = x.IdCuenta,
                                                EstadoCuentaId = x.EstadoCuenta,
                                                TipoCuentaId = x.IdTipoProducto,
                                                Cueotas = x.Cuotas,
                                            })
                                        .ToList();
                }
                else
                {
                    usuarios.Cuentas = cuentasCliente.ToList();
                }


                int cuotas = 0;
                if (usuarios.Cuentas.Count() > 0)
                {
                    try
                    {
                        cuotas = usuarios.Cuentas.OrderByDescending(c => c.FechaCreacion).First(c => c.EstadoCuentaId != 7).Cueotas;
                    }
                    catch
                    {
                        cuotas = 0;
                    }
                    if (cuotas == 0)
                    {
                        var cuentaCuotas = usuarios.Cuentas.FirstOrDefault(c => c.EstadoCuentaId != 7 && c.Cueotas > 0);
                        cuotas = cuentaCuotas == null ? 0 : cuentaCuotas.Cueotas;
                    }
                    if (cuotas == 0)
                    {
                        usuarios.AportesCategoria = this._mapper.Map<List<AportesClienteDto>>(await _clientsRepository.ObtenerAportesCliente(usuarios.IdCliente));

                        var aporteCuota = usuarios.AportesCategoria.FirstOrDefault();
                        cuotas = aporteCuota == null ? 0 : aporteCuota.APC_CUOTAS_ACUMULADAS;
                    }
                }

                usuarios.NumeroCuotas = cuotas;
            }
            catch (Exception ex)
            {
                return new Response<ClienteNominaDto> { IsSuccess = false, ReturnMessage = "Error al obtener información del cliente: " + ex.Message, Data = null };
            }

            return new Response<ClienteNominaDto> { Data = usuarios };
        }

        public async Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumer(int identificationType, string identificationNumber, IEnumerable<Claim> claims)
        {
            if (identificationType == 0)
            {
                identificationType = 1;
            }

            ClienteDto usuarios = null;
            ObtenerBuaResponse clienteIntegracionGA2 = null;

            var cliente = await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber);

            if (cliente == null)
            {
                return new Response<ClienteDto>
                {
                    IsSuccess = false,
                    ReturnMessage = "No se encontró información del cliente "
                };
            }


            usuarios = _mapper.Map<Cliente, ClienteDto>(cliente);


            usuarios.TipoVinculacion = !string.IsNullOrEmpty(usuarios.SituacionActual) ? "AFILIADO ADMINISTRACION CESANTIAS" : "";
            var catalogoValores = await _iEmbargosRequestProvider.ObtenerValoresCatalogosPorIdCatalogo(54);
            usuarios.EstadoCliente = usuarios.IdEstadoCliente == 1 ? "VIGENTE" : "ANULADO";

            usuarios.SituacionActual = catalogoValores.Data
            ?.FirstOrDefault(c => c.IdValor == usuarios.CLV_ID)?
            .Descripcion;

            AnotacionClienteDto anotacionRequest = new AnotacionClienteDto()
            {
                ClienteId = usuarios.IdCliente
            };

            var anotacionesCliente = await _clientsRepository.ObtenerAnotacionesCliente(anotacionRequest);
            usuarios.Anotaciones = _mapper.Map<List<AnotacionIntegracion>>(anotacionesCliente);

            usuarios.Cuentas = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(usuarios.IdCliente));
            var tiposDeCuentas = await _iEmbargosRequestProvider.ObtenerTiposDeCuentasGA2();
            var conceptosGA2 = await _iEmbargosRequestProvider.ObtenerConceptosGA2();
            var estadosCuentasGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeCuentasGA2();
            var estadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeClientesGA2();
            var causalesEstadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerCausalesEstadosDeClientesGA2();

            foreach (var cuenta in usuarios.Cuentas)
            {
                string tipo = cuenta.IdTct + " ";
                var tipoCuenta = tiposDeCuentas.Data.FirstOrDefault(x => x.prefijoTipoCuenta == tipo);

                if (tipoCuenta != null)
                {
                    cuenta.TipoCuenta = tipoCuenta.descripcionTipoCuenta;
                    cuenta.NumeroCuentaCompleta = $"0{tipoCuenta.prefijoTipoCuenta}{cuenta.NumeroCuenta}";
                }
            }

            foreach (var cuenta in usuarios.Cuentas)
            {
                cuenta.ConceptosSaldosBloqueos = _mapper.Map<List<ConceptosSaldosBloqueosDto>>(
                    await _clientsRepository.ObtenerConceptosSaldosBloqueos(cuenta.IdCuenta)
                ).GroupBy(x => x.IdConcepto).Select(x => x.First()).ToList();

                cuenta.Conceptos = _mapper.Map<List<ConceptoCuentaDto>>(
                    await _cuentasConceptosRepository.ObtenerConceptosCuentas(cuenta.IdCuenta)
                );
                cuenta.EstadoCuenta = estadosCuentasGA2.Data.ToList().Find(x => x.IdEstado == cuenta.IdEct).Estado;
                var movimientosConceptos = new List<MovimientosCuentaDto>();

                foreach (var concepto in cuenta.Conceptos)
                {
                    var movimientos = _mapper.Map<List<MovimientosCuentaDto>>(
                        await _cuentasConceptosRepository.ObtenerMovimientosConceptos(cuenta.IdCuenta, concepto.ConceptoId)
                    );

                    concepto.Movimientos = movimientos;
                    movimientosConceptos.AddRange(movimientos);
                    concepto.ConceptoDescripcion = conceptosGA2.Data.ToList().Find(x => x.IdConcepto == concepto.ConceptoId).Descripcion;

                    cuenta.Movimientos = new List<MovimientoDTO>();

                    foreach (var movimiento in concepto.Movimientos.OrderByDescending(x => x.FechaProceso))
                    {
                        MovimientoDTO movimientoDto = new MovimientoDTO()
                        {
                            CAT_TIPO_MOVIMIENTO = movimiento.TipoMovimiento,
                            CNC_CONCEPTO = conceptosGA2.Data.First(x => x.IdConcepto == movimiento.ConceptoId).Descripcion,
                            CNC_ID = movimiento.ConceptoId,
                            CTA_ID = cuenta.IdCuenta,
                            DCT_ID = movimiento.DocumentoId,
                            MVT_ANO_PERIODO = movimiento.PeriodoAno,
                            MVT_FECHA_MOVIMIENTO = movimiento.FechaProceso,
                            MVT_ID = movimiento.MovimientoId,
                            MVT_FECHA_PROCESO = movimiento.FechaProceso,
                            MVT_MES_PERIODO = movimiento.PeriodoAno,
                            MVT_VALOR = Convert.ToDecimal(movimiento.Valor)
                        };

                        cuenta.Movimientos.Add(movimientoDto);

                    }
                }
            }

            int cuotas = 0;
            if (usuarios.Cuentas.Count() > 0)
            {
                try
                {
                    cuotas = usuarios.Cuentas.OrderByDescending(c => c.FechaCreacion).First(c => c.IdEct != 7).Cuotas;
                }
                catch
                {
                    cuotas = 0;
                }
                if (cuotas == 0)
                {
                    var cuentaCuotas = usuarios.Cuentas.FirstOrDefault(c => c.IdEct != 7 && c.Cuotas > 0);
                    cuotas = cuentaCuotas == null ? 0 : cuentaCuotas.Cuotas;
                }
                if (cuotas == 0)
                {
                    var aporteCuota = usuarios?.AportesCategoria.FirstOrDefault();
                    cuotas = aporteCuota == null ? 0 : aporteCuota.APC_CUOTAS_ACUMULADAS;
                }
            }

            usuarios.NumeroCuotas = cuotas;
            usuarios.AportesCategoria = this._mapper.Map<List<AportesClienteDto>>(await _clientsRepository.ObtenerAportesCliente(usuarios.IdCliente));
            usuarios.EstadoCliente = estadosClientesGA2.Data.First(x => x.SecondsId == usuarios.IdEstadoCliente).Descriptions;
            usuarios.EstadosCliente = estadosClientesGA2.Data.ToList();
            usuarios.CausalesEstadoCliente = causalesEstadosClientesGA2.Data.ToList();
            if (usuarios.AportesCategoria.Count() > 0)
            {
                var aportesCategoria = usuarios.AportesCategoria.OrderByDescending(x => x.APC_FECHA_PRIMERA_CUOTA);
                if (aportesCategoria.Any())
                {
                    var aportesOrdenados = aportesCategoria.OrderBy(a => a.APC_FECHA_PRIMERA_CUOTA).ToList();
                    var primerAporte = aportesOrdenados.First();
                    var ultimoAporte = aportesOrdenados.Last();

                    usuarios.FechaPrimeraCuota = primerAporte.APC_FECHA_PRIMERA_CUOTA;
                    usuarios.FechaUltimaCuota = ultimoAporte.APC_FECHA_ULTIMA_COUTA;
                    decimal subsidiovivienda = 0;

                    foreach (var aporteCategoria in aportesCategoria)
                    {
                        var simulacionCargoCategoriaDto = new ObtenerSmlvAnualDto()
                        {
                            CTG_ID = aporteCategoria.CTG_ID,
                            SMLV_ANIO = usuarios.Regimen,
                        };

                        var subsidioCategoriaData = (await _iEmbargosRequestProvider.ObtenerSalarioMinimoAnualGA2(simulacionCargoCategoriaDto))?.Data;

                        if (subsidioCategoriaData != null)
                        {
                            decimal factorSubsidio = Math.Round(subsidioCategoriaData.SMLV_SALARIO * (decimal)subsidioCategoriaData.VLE_FACTOR_SUBS, 6);

                            if (aportesCategoria.Count() >= 2)
                            {
                                decimal valorAporte = Math.Round(factorSubsidio * aporteCategoria.APC_CUOTAS_ACUMULADAS, 2, MidpointRounding.AwayFromZero);
                                subsidiovivienda += valorAporte;
                            }
                            else
                            {
                                subsidiovivienda += Math.Round(factorSubsidio, 2, MidpointRounding.AwayFromZero);
                            }
                        }
                    }

                    if (aportesCategoria.Count() >= 2)
                    {
                        subsidiovivienda = Math.Round(subsidiovivienda / 168, 2, MidpointRounding.AwayFromZero);
                    }

                    usuarios.ValorSubsidio = Math.Round(subsidiovivienda / 100, 0, MidpointRounding.AwayFromZero) * 100;
                }

            }
            usuarios.Dependientes = _mapper.Map<List<DependienteDto>>(await _clientsRepository.ObtenerDependientes(usuarios.IdCliente));

            Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> solicitudesGA2 = await _iEmbargosRequestProvider.ObtenerSolicitudesGA2(identificationNumber);
            usuarios.Solicitudes = _mapper.Map<List<SolicitudBUADto>>(solicitudesGA2.Data);

            var solicitudesReintegroGA2 = await _iEmbargosRequestProvider.ObtenerSolicitudesReintegroGA2(identificationType, identificationNumber);
            usuarios.Solicitudes.AddRange(_mapper.Map<List<SolicitudBUADto>>(solicitudesReintegroGA2.Data));

            var embargosSinConcepto = usuarios.Embargos;

            foreach (var embargo in embargosSinConcepto)
            {
                SolicitudBUADto solicitud = new()
                {
                    NumeroSolicitud = embargo.NumeroSolicitud.ToString(),
                    Radicado = embargo.RadicadoWorkManager ?? "",
                    TipoSolicitud = "BLOQUEO Y/O EMBARGO DE CUENTA",
                    Modalidad = embargo.TipoProceso ?? "",
                    FechaInicioSolicitud = embargo.FechaRegistro,
                    FechaFinSolicitud = embargo.FechaActualizacion,
                    ValorPagado = 0,
                    EstadoSolicitud = embargo.EstadoEmbargo
                };
                usuarios.Solicitudes.Add(solicitud);
            }

            return new Response<ClienteDto> { Data = usuarios };

        }

        /// <summary>
        /// Metodo obtener info cliente por tipo y número de documento 
        /// </summary>
        /// <param name="Cliente">Modelo dto cliente</param>
        /// <author>Nicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<ClienteDto>> ObtenerClientePorTipoIdentificationYNumero(int identificationType, string identificationNumber, IEnumerable<Claim> claims)
        {
            var parametrosIntegraciones = await _iEmbargosRequestProvider.ObtenerParametrosIntegraciones();

            var status = parametrosIntegraciones.Data.Where(p => p.Integracion.Equals("Bua")).FirstOrDefault();
            if (status.Valor == "false")
            {
                var result = await ObtenerClientePorTipoIdentificationYNumer(identificationType, identificationNumber, claims);
                return result;
            }
            else
            {
                ///LLMADO NUEVO SERVICIO DE BUA
                //var response = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                ClienteDto usuarios = null;
                ObtenerBuaResponse clienteIntegracionGA2 = null;
                var listSaldos = new List<SaldoDto>();
                if (identificationType == 0)
                {
                    identificationType = 1;
                    usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber));
                    clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                    if (usuarios == null && clienteIntegracionGA2.ObtenerBUAResult == null)
                    {
                        identificationType = 4;
                        usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber));
                        clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                        if (usuarios == null && clienteIntegracionGA2.ObtenerBUAResult == null)
                        {
                            identificationType = 5;
                            usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber));
                            clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                            if (usuarios == null && clienteIntegracionGA2.ObtenerBUAResult == null)
                            {
                                identificationType = 3;
                                usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber));
                                clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                            }
                        }
                    }
                }
                else
                {
                    usuarios = this._mapper.Map<Cliente, ClienteDto>(await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(identificationType, identificationNumber));
                    if (usuarios != null)
                    {
                        if (identificationType == 9)
                        {
                            identificationType = usuarios.IdTipoIdentificacion;
                            identificationNumber = usuarios.Identificacion;
                        }
                    }
                    clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2New(identificationType, identificationNumber);
                }

                Cliente cliente = null;

                if (usuarios != null &&
                    usuarios.IdTipoIdentificacion > 0 &&
                    !string.IsNullOrWhiteSpace(usuarios.Identificacion))
                {
                    try
                    {
                        cliente = await _clientsRepository.ObtenerClientePorTipoIdentificationYNumero(
                            usuarios.IdTipoIdentificacion,
                            usuarios.Identificacion);
                    }
                    catch
                    {

                        cliente = null;
                    }

                    if (cliente != null)
                    {
                        usuarios.EmbargosAfiliado ??= new List<EmbargosAfiliados>();
                        usuarios.CausalesBloqueoAfiliado ??= new List<CausalesBloqueoAfiliados>();

                        if (clienteIntegracionGA2?.ObtenerBUAResult?.EmbargosAfiliado?.Count > 0)
                        {
                            usuarios.EmbargosAfiliado.AddRange(
                                clienteIntegracionGA2.ObtenerBUAResult.EmbargosAfiliado
                            );
                        }


                        usuarios.EstadosCausaleslCliente ??= new List<CausalEstadoClienteDto>();

                        if (clienteIntegracionGA2?.ObtenerBUAResult?.CausalesBloqueoAfiliado?.Any() == true)
                        {
                            var causalesMapeadas = clienteIntegracionGA2.ObtenerBUAResult.CausalesBloqueoAfiliado
                                .Select(x => new CausalEstadoClienteDto
                                {
                                    CausalEstadoId = 0,
                                    CausalId = x.CEC_ID,
                                    EstadoId = x.ECL_ID ?? 0,
                                    ClienteId = x.CEL_ID ?? 0,
                                    DescripcionCausal = x.CAUSAL ?? string.Empty,
                                    NombreEstado = x.ESTADO_BLOQUEO ?? string.Empty,
                                    Razon = string.Empty,
                                    Observaciones = x.OBSERVACIONES ?? string.Empty,
                                    Fecha = x.FECHA_REGISTRO ?? DateTime.MinValue,
                                    Usuario = null,
                                    FechaRetiro = x.FECHA_RETIRO,
                                    Estado = string.Equals(x.ESTADO, "ACTIVO", StringComparison.OrdinalIgnoreCase)
                                })
                                .ToList();

                            usuarios.EstadosCausaleslCliente.AddRange(causalesMapeadas);

                            var totalAgregados = causalesMapeadas.Count;
                            var totalFinal = usuarios.EstadosCausaleslCliente.Count;
                        }

                        usuarios.Telefono2 = cliente.DTL_TELEFONO2;
                        if (!string.IsNullOrWhiteSpace(clienteIntegracionGA2?.ObtenerBUAResult?.Direccion?.CorreoInstitucional))
                        {
                            usuarios.Correo = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoInstitucional;
                        }

                        if (!string.IsNullOrWhiteSpace(clienteIntegracionGA2?.ObtenerBUAResult?.Direccion?.CorreoPersonal))
                        {
                            usuarios.Correo2 = clienteIntegracionGA2.ObtenerBUAResult.Direccion.CorreoPersonal;
                        }

                        if (clienteIntegracionGA2?.ObtenerBUAResult?.TasaAporte != null)
                        {
                            usuarios.TasaAportes = clienteIntegracionGA2.ObtenerBUAResult.TasaAporte;
                        }

                        if (clienteIntegracionGA2?.ObtenerBUAResult?.TasaAporteVoluntario != null)
                        {
                            usuarios.TasaVoluntariaAportes = clienteIntegracionGA2.ObtenerBUAResult.TasaAporteVoluntario;
                        }

                    }
                }
                if (usuarios != null)
                {
                    usuarios.PrimerNombreConyuge = usuarios.PrimerNombreConyuge?.Trim().ToUpperInvariant();
                    usuarios.SegundoNombreConyuge = usuarios.SegundoNombreConyuge?.Trim().ToUpperInvariant();
                    usuarios.PrimerApellidoConyuge = usuarios.PrimerApellidoConyuge?.Trim().ToUpperInvariant();
                    usuarios.SegundoApellidoConyuge = usuarios.SegundoApellidoConyuge?.Trim().ToUpperInvariant();


                }

                int cuotas = 0;
                if (usuarios == null)
                {
                    if (clienteIntegracionGA2.ObtenerBUAResult == null)
                    {
                        return new Response<ClienteDto> { Data = null };
                    }

                    usuarios = this._mapper.Map<ClienteDto>(clienteIntegracionGA2.ObtenerBUAResult);

                }
                usuarios.Anotaciones = clienteIntegracionGA2?.ObtenerBUAResult?.Anotaciones?.OrderByDescending(x => x.FECHA_ANOTACION).ToList();
                AnotacionClienteDto anotacionRequest = new AnotacionClienteDto()
                {
                    ClienteId = usuarios.IdCliente
                };
                var Anotaciones = this._mapper.Map<List<AnotacionClienteDto>>(await _clientsRepository.ObtenerAnotacionesCliente(anotacionRequest));
                foreach (var anotacion in Anotaciones)
                {
                    AnotacionIntegracion anotacionIntegracion = new AnotacionIntegracion()
                    {
                        ANOTACION_ID = anotacion.AnotacionClienteId,
                        ANOTACION = anotacion.Anotacion,
                        FECHA_ANOTACION = anotacion.FechaAnotacion,
                        USUARIO = anotacion.Usuario
                    };
                    usuarios.Anotaciones.Add(anotacionIntegracion);
                }
                usuarios.Anotaciones = usuarios?.Anotaciones?.OrderByDescending(x => x.FECHA_ANOTACION).ToList();

                usuarios.SituacionActual = clienteIntegracionGA2?.ObtenerBUAResult?.Actividad;

                var cuentas = this._mapper.Map<List<CuentaDto>>(await _clientsRepository.ObtenerCuentas(usuarios.IdCliente));
                if (cuentas.Count() == 0)
                {
                    usuarios.Cuentas = _mapper.Map<List<CuentaDto>>(clienteIntegracionGA2.ObtenerBUAResult.Cuentas);
                }
                else
                {
                    usuarios.Cuentas = cuentas;
                }
                usuarios.AportesCategoria = this._mapper.Map<List<AportesClienteDto>>(await _clientsRepository.ObtenerAportesCliente(usuarios.IdCliente));
                usuarios.Dependientes = this._mapper.Map<List<DependienteDto>>(await _clientsRepository.ObtenerDependientes(usuarios.IdCliente));
                if (clienteIntegracionGA2?.ObtenerBUAResult?.Solicitudes != null)
                {
                    usuarios.Solicitudes ??= _mapper.Map<List<SolicitudBUADto>>(
                        clienteIntegracionGA2.ObtenerBUAResult.Solicitudes
                            .GroupBy(x => x.NRO_SOLICITUD, (key, g) => g.OrderByDescending(e => e.MONTO_TOTAL_PAGO).First()));
                }
                Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>> solicitudesGA2 = new Response<IEnumerable<RespuestaConsultarSolicitudCesantiasDto>>();
                try
                {
                    solicitudesGA2 = await _iEmbargosRequestProvider.ObtenerSolicitudesGA2(identificationNumber);
                }
                catch
                {
                    solicitudesGA2.Data = null;
                }
                var tiposIdentificacion = await _iEmbargosRequestProvider.ObtenerTiposIdentificacionGA2();
                var afiliadoDeBeneficiario = await _clientsRepository.ObtenerAfiliadoDeBeneficiario(identificationNumber);

                List<DependienteDto> listDependiente = new List<DependienteDto>();
                foreach (var dependiente in usuarios.Dependientes)
                {
                    if (dependiente.TipoIdentificacion > 0)
                    {
                        var codigo = tiposIdentificacion.Data.FirstOrDefault(x => x.Id == dependiente.TipoIdentificacion).Codigo;
                        dependiente.Identificacion = codigo + "-" + dependiente.Identificacion;
                    }
                    listDependiente.Add(dependiente);
                }

                usuarios.Dependientes = listDependiente;
                if (afiliadoDeBeneficiario != null)
                {
                    var codigo = tiposIdentificacion.Data.FirstOrDefault(x => x.Id == afiliadoDeBeneficiario.TID_ID).Codigo;
                    DependienteDto dependiente = new DependienteDto()
                    {
                        IdCliente = afiliadoDeBeneficiario.CLI_ID,
                        IdDependiente = afiliadoDeBeneficiario.DPT_ID,
                        Identificacion = codigo + "-" + afiliadoDeBeneficiario.CLI_IDENTIFICACION,
                        Nombre = afiliadoDeBeneficiario.CLI_PRIMER_NOMBRE + " " + afiliadoDeBeneficiario.CLI_SEGUNDO_NOMBRE + " " + afiliadoDeBeneficiario.CLI_PRIMER_APELLIDO + " " + afiliadoDeBeneficiario.CLI_SEGUNDO_APELLIDO,
                        Participacion = afiliadoDeBeneficiario.DPT_PARTICIPACION

                    };
                    listDependiente.Add(dependiente);
                    usuarios.Dependientes = listDependiente;
                    usuarios.EsBeneficiario = true;
                }
                var NombreTipoIdentificacion = await _iEmbargosRequestProvider.ObtenerTiposIdentificacionGA2();
                usuarios.NombreTipoIdentificacion = NombreTipoIdentificacion.Data.ToList().Find(x => x.Id == usuarios.IdTipoIdentificacion).Descripcion;

                if (solicitudesGA2.Data != null)
                {
                    foreach (var solicitud in solicitudesGA2.Data)
                    {
                        SolicitudBUADto solicitudBUADto = new SolicitudBUADto()
                        {
                            EstadoSolicitud = solicitud.EstadoSolicitud,
                            FechaInicioSolicitud = solicitud.FechaSolicitud,
                            Modalidad = solicitud.TpsSubDescripcion,
                            Radicado = solicitud.NumeroRadicadoSG,
                            NumeroSolicitud = solicitud.NumeroSolicitud.ToString(),
                            Valor = solicitud.ValorRetirar,
                            ValorPagado = solicitud.ValorRetirar,
                            DocumentoCausante = solicitud.RadicadoGestionDocumental,
                            Actividad = solicitud.TareaNombre,
                            TipoSolicitud = solicitud.ProcesoDescripcion
                        };
                        usuarios.Solicitudes.Add(solicitudBUADto);
                    }
                }
                usuarios.TipoVinculacion = !string.IsNullOrEmpty(usuarios.SituacionActual) ? "AFILIADO ADMINISTRACION CESANTIAS" : "";
                var solicitudesReintegroGA2 = await _iEmbargosRequestProvider.ObtenerSolicitudesReintegroGA2(identificationType, identificationNumber);
                if (solicitudesReintegroGA2.Data != null)
                {
                    foreach (var item in solicitudesReintegroGA2.Data)
                    {
                        var solicitud = item;
                        {
                            SolicitudBUADto solicitudBUADto = new SolicitudBUADto()
                            {
                                EstadoSolicitud = solicitud.EstadoAfiliao,
                                FechaInicioSolicitud = solicitud.FechaCreacion,
                                FechaFinSolicitud = solicitud.FechaFin,
                                Modalidad = solicitud.ModalidadReintegro,
                                Radicado = solicitud.Numero_Radicado,
                                NumeroSolicitud = solicitud.NumerSolicitud.ToString(),
                                Valor = solicitud.ValorSueldo,
                                ValorPagado = solicitud.ValorSueldo,
                                TipoSolicitud = solicitud.TipoSolicitud,
                                Actividad = solicitud.TareaNombre
                            };
                            usuarios.Solicitudes.Add(solicitudBUADto);
                        }
                    }

                }

                if (usuarios.Cuentas.Count() > 0)
                {
                    try
                    {
                        cuotas = usuarios.Cuentas.OrderByDescending(c => c.FechaCreacion).First(c => c.IdEct != 7).Cuotas;
                    }
                    catch
                    {
                        cuotas = 0;
                    }
                    if (cuotas == 0)
                    {
                        var cuentaCuotas = usuarios.Cuentas.FirstOrDefault(c => c.IdEct != 7 && c.Cuotas > 0);
                        cuotas = cuentaCuotas == null ? 0 : cuentaCuotas.Cuotas;
                    }
                    if (cuotas == 0)
                    {
                        var aporteCuota = usuarios.AportesCategoria.FirstOrDefault();
                        cuotas = aporteCuota == null ? 0 : aporteCuota.APC_CUOTAS_ACUMULADAS;
                    }
                }

                usuarios.NumeroCuotas = cuotas;
                var bloqueoDto = new BloqueoRecursosCreditoDto
                {
                    TipoIdentificacion = usuarios.IdTipoIdentificacion,
                    NumeroIdentificacion = usuarios.Identificacion
                };

                var tiposDeCuentas = await _iEmbargosRequestProvider.ObtenerTiposDeCuentasGA2();
                var conceptosGA2 = await _iEmbargosRequestProvider.ObtenerConceptosGA2();
                var estadosCuentasGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeCuentasGA2();
                var estadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeClientesGA2();
                var cuentasAfectada = await _iEmbargosRequestProvider.ConsultarBloqueoCuentaCredito(bloqueoDto);
                var causalesEstadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerCausalesEstadosDeClientesGA2();
                Response<diffgram> validacionBiometria = new Response<diffgram>();
                try
                {
                    validacionBiometria = await _iEmbargosRequestProvider.ObtenerValidacionBiometriaGA2(usuarios.Identificacion);
                }
                catch
                {
                    validacionBiometria.IsSuccess = false;
                }
                //var factoresSubsidioCategoria = await _iEmbargosRequestProvider.ObtenerFactorSubsidioCategoriaGA2(DateTime.Now.Year);
                var tiposSolicitud = await _iEmbargosRequestProvider.ObtenerTiposSolicitudGA2();

                usuarios.TiposSolicitud = tiposSolicitud.Data.ToList();

                CausalEstadoClienteDto estadosCausalesRequest = new CausalEstadoClienteDto()
                {
                    ClienteId = usuarios.IdCliente
                };
                var EstadosCausalesCliente = this._mapper.Map<List<CausalEstadoClienteDto>>(await _clientsRepository.ObtenerCausalesEstadoCliente(estadosCausalesRequest));

                foreach (var estadoCausal in EstadosCausalesCliente)
                {
                    estadoCausal.NombreEstado = estadosClientesGA2.Data.First(x => x.SecondsId == estadoCausal.EstadoId).Descriptions;
                    estadoCausal.DescripcionCausal = estadoCausal.CausalId != 0 ? causalesEstadosClientesGA2.Data.First(x => x.EstadoCausalClienteId == estadoCausal.CausalId).Causal : "";
                }
                if (EstadosCausalesCliente != null && EstadosCausalesCliente.Any())
                {
                    usuarios.EstadosCausaleslCliente.AddRange(
                        EstadosCausalesCliente
                        .OrderByDescending(x => x.Fecha)
                        .ToList()
                    );
                }

                if (validacionBiometria.IsSuccess)
                {
                    usuarios.ValidacionBiometrica = true;
                }
                else
                {
                    usuarios.ValidacionBiometrica = false;
                }
                usuarios.EstadoCliente = estadosClientesGA2.Data.First(x => x.SecondsId == usuarios.IdEstadoCliente).Descriptions;
                usuarios.EstadosCliente = estadosClientesGA2.Data.ToList();
                usuarios.CausalesEstadoCliente = causalesEstadosClientesGA2.Data.ToList();
                if (usuarios.AportesCategoria.Count() > 0)
                {
                    var aportesCategoria = usuarios.AportesCategoria.OrderByDescending(x => x.APC_FECHA_PRIMERA_CUOTA);
                    if (aportesCategoria.Any())
                    {
                        var aportesOrdenados = aportesCategoria.OrderBy(a => a.APC_FECHA_PRIMERA_CUOTA).ToList();
                        var primerAporte = aportesOrdenados.First();
                        var ultimoAporte = aportesOrdenados.Last();

                        usuarios.FechaPrimeraCuota = primerAporte.APC_FECHA_PRIMERA_CUOTA;
                        usuarios.FechaUltimaCuota = ultimoAporte.APC_FECHA_ULTIMA_COUTA;
                        decimal subsidiovivienda = 0;

                        foreach (var aporteCategoria in aportesCategoria)
                        {
                            var simulacionCargoCategoriaDto = new ObtenerSmlvAnualDto()
                            {
                                CTG_ID = aporteCategoria.CTG_ID,
                                SMLV_ANIO = usuarios.Regimen,
                            };

                            var subsidioCategoriaData = (await _iEmbargosRequestProvider.ObtenerSalarioMinimoAnualGA2(simulacionCargoCategoriaDto))?.Data;

                            if (subsidioCategoriaData != null)
                            {
                                decimal factorSubsidio = Math.Round(subsidioCategoriaData.SMLV_SALARIO * (decimal)subsidioCategoriaData.VLE_FACTOR_SUBS, 6);

                                if (aportesCategoria.Count() >= 2)
                                {
                                    decimal valorAporte = Math.Round(factorSubsidio * aporteCategoria.APC_CUOTAS_ACUMULADAS, 2, MidpointRounding.AwayFromZero);
                                    subsidiovivienda += valorAporte;
                                }
                                else
                                {
                                    subsidiovivienda += Math.Round(factorSubsidio, 2, MidpointRounding.AwayFromZero);
                                }
                            }
                        }

                        if (aportesCategoria.Count() >= 2)
                        {
                            subsidiovivienda = Math.Round(subsidiovivienda / 168, 2, MidpointRounding.AwayFromZero);
                        }

                        usuarios.ValorSubsidio = Math.Round(subsidiovivienda / 100, 0, MidpointRounding.AwayFromZero) * 100;
                    }



                }
                decimal totalValorReintegro = 0;
                double valorTotalEmbargos = 0;
                decimal totalValorBloqueo = 0;
                decimal saldoDisponible = 0;
                decimal valorTotalEmbargosBloqueos = 0;
                List<SolicitudBUADto> solicitudesBUADto = new List<SolicitudBUADto>();
                try
                {
                    usuarios.Embargos = new List<EmbargosDto>();
                    ConsultaEmbargosfiltroDto consultaEmbargosfiltroDto = new ConsultaEmbargosfiltroDto()
                    {
                        Estado = null,
                        Estados = null,
                        ClienteTipoIdentificacion = usuarios.IdTipoIdentificacion,
                        ClienteIdentificacion = usuarios.Identificacion
                    };
                    var embargos = await _iEmbargosRequestProvider.ObtenerEmbargosJuzgadosGA2(consultaEmbargosfiltroDto);
                    foreach (var embargo in embargos.Data.OrderByDescending(x => x.FechaRegistro))
                    {
                        // Saltar embargos con Remanente en true
                        if (embargo.Remanente)
                            continue;

                        var juzgado = await _iEmbargosRequestProvider.ObtenerJuzgadoPorIdGA2(embargo.JuzgadoId);
                        embargo.Juzgado = juzgado.Data != null
                            ? string.Format("{0}-{1}", juzgado.Data.CodigoInterno, embargo.Juzgado)
                            : embargo.Juzgado;

                        usuarios.Embargos.Add(embargo);
                    }


                }
                catch
                {
                    var embargo = new Embargo();
                }
                foreach (CuentaDto cuenta in usuarios.Cuentas)
                {
                    cuenta.Embargos = new();
                    IEnumerable<ConceptoCuentaDto> conceptos = _mapper.Map<IEnumerable<ConceptoCuentaDto>>(await _cuentasConceptosRepository.ObtenerConceptosCuentas(cuenta.IdCuenta));
                    List<ConceptoCuentaDto> conceptoCuentaDtos = new List<ConceptoCuentaDto>();
                    decimal totalValorReintegroCuenta = 0;
                    double valorTotalEmbargosCuenta = 0;
                    foreach (var concepto in conceptos)
                    {
                        IEnumerable<EmbargoCuentaConceptoDto> embargoCuentaConceptos = _iEmbargosRequestProvider.ObtenerEmbargoByConceptoCuentaId(concepto.Id).Result.Data;
                        var embargoCuentaConceptosFiltrados = embargoCuentaConceptos
                        .Where(ecc =>
                        {
                            var embargo = usuarios.Embargos.FirstOrDefault(x => x.Id == ecc.EmbardoId);
                            return embargo != null && embargo.Remanente == false;
                        })
                        .ToList();

                        if (embargoCuentaConceptosFiltrados.Count() > 0)
                        {



                            cuenta.Embargos = cuenta.Embargos.Concat(embargoCuentaConceptosFiltrados).ToList();
                            foreach (var item in embargoCuentaConceptos)
                            {
                                var embargoValor = usuarios.Embargos.FirstOrDefault(x => x.Id == item.EmbardoId);
                                SolicitudBUADto solicitud = new()
                                {
                                    NumeroSolicitud = embargoValor != null ? embargoValor.NumeroSolicitud.ToString() : "",
                                    Radicado = embargoValor?.RadicadoWorkManager ?? "",
                                    TipoSolicitud = "BLOQUEO Y/O EMBARGO DE CUENTA",
                                    Modalidad = item?.TipoProceso ?? "",
                                    FechaInicioSolicitud = item?.FechaInicio ?? DateTime.MinValue,
                                    FechaFinSolicitud = item != null && item.FechaFin.Equals(new DateTime(1, 1, 1)) && embargoValor != null
                                    ? embargoValor.FechaActualizacion
                                    : item?.FechaFin ?? DateTime.MinValue,
                                    ValorPagado = concepto != null && item != null ? (decimal)(concepto.saldo * item.Valor / 100) : 0,
                                    EstadoSolicitud = "PROCESADA"
                                };
                                valorTotalEmbargosCuenta += item.Valor;
                                valorTotalEmbargos += item.Valor;
                                usuarios.Solicitudes.Add(solicitud);

                                if (embargoValor != null)
                                {
                                    usuarios.Embargos.Remove(embargoValor);
                                    embargoValor.Valor = item.Valor;
                                    usuarios.Embargos.Add(embargoValor);
                                }
                            }

                        }
                        ConceptoCuentaDto conceptoCuentaDto = concepto;
                        conceptoCuentaDto.ConceptoDescripcion = conceptosGA2.Data.ToList().Find(x => x.IdConcepto == concepto.ConceptoId).Descripcion;
                        totalValorReintegro += concepto.ValorDevolver;
                        totalValorReintegroCuenta += concepto.ValorDevolver;
                        conceptoCuentaDtos.Add(conceptoCuentaDto);

                        var embargosSinConcepto = usuarios.Embargos
                            .Where(e => e.Remanente == false &&
                                        !cuenta.Embargos.Any(x => x.EmbardoId == e.Id))
                            .ToList();

                        foreach (var embargo in embargosSinConcepto)
                        {
                            SolicitudBUADto solicitud = new()
                            {
                                NumeroSolicitud = embargo.NumeroSolicitud.ToString(),
                                Radicado = embargo.RadicadoWorkManager ?? "",
                                TipoSolicitud = "BLOQUEO Y/O EMBARGO DE CUENTA",
                                Modalidad = embargo.TipoProceso ?? "",
                                FechaInicioSolicitud = embargo.FechaRegistro,
                                FechaFinSolicitud = embargo.FechaActualizacion,
                                ValorPagado = 0,
                                EstadoSolicitud = embargo.EstadoEmbargo
                            };
                            usuarios.Solicitudes.Add(solicitud);
                        }

                    }

                    try
                    {
                        string tipo = cuenta.IdTct + " ";
                        var tipoCuenta = tiposDeCuentas.Data.FirstOrDefault(x => x.prefijoTipoCuenta == tipo);
                        cuenta.TipoCuenta = tipoCuenta.descripcionTipoCuenta;
                        cuenta.NumeroCuentaCompleta = string.Format("0{0}{1}", tipoCuenta.prefijoTipoCuenta, cuenta.NumeroCuenta.ToString());
                    }
                    catch
                    {
                        cuenta.TipoCuenta = "Tipo de cuenta no registrado";
                    }
                    cuenta.Conceptos = conceptoCuentaDtos;
                    cuenta.ConceptosSaldosBloqueos = this._mapper.Map<List<ConceptosSaldosBloqueosDto>>(await _clientsRepository.ObtenerConceptosSaldosBloqueos(cuenta.IdCuenta)).GroupBy(x => x.IdConcepto).Select(x => x.First()).ToList();
                    List<ConceptosSaldosBloqueosDto> conceptosSaldos = new List<ConceptosSaldosBloqueosDto>();
                    var cuentaIntegracion = clienteIntegracionGA2?.ObtenerBUAResult?.Cuentas?.FirstOrDefault(x => x.NumeroCuenta == cuenta.NumeroCuenta.ToString());
                    decimal totalValorBloqueoCuenta = 0;
                    decimal saldoDisponibleCuenta = 0;

                    foreach (var concepto in cuenta.ConceptosSaldosBloqueos)
                    {
                        ConceptosSaldosBloqueosDto conceptosSaldosBloqueosDto = concepto;
                        conceptosSaldosBloqueosDto.Concepto = conceptosGA2.Data
                            .ToList()
                            .Find(x => x.IdConcepto == concepto.IdConcepto).Descripcion;

                        string tipo_cuenta = "";
                        decimal valorAcumulado = 0;
                        string estadoBloqueo = "";
                        string razon = "";
                        string demandante = "";
                        string solicitud = "";

                        if (cuentaIntegracion != null)
                        {
                            var conceptoIntegracion = cuentaIntegracion.ConceptosSaldosBloqueos
                                .FirstOrDefault(x => x.IdConcepto == concepto.IdConcepto);

                            if (conceptoIntegracion != null)
                            {
                                valorAcumulado = conceptoIntegracion.ValorAcumulado;
                                estadoBloqueo = conceptoIntegracion.EstadoBloqueo;
                                razon = conceptoIntegracion.Razon;
                                demandante = conceptoIntegracion.Demandante;
                                solicitud = conceptoIntegracion.Solicitud;
                                tipo_cuenta = conceptoIntegracion.Tipo_Cuenta;

                            }
                        }

                        if (cuentaIntegracion?.ConceptosSaldosBloqueos?.Count > 0)
                        {
                            concepto.NroExpd = cuentaIntegracion.ConceptosSaldosBloqueos[0].NroExpd;
                        }

                        totalValorBloqueoCuenta += valorAcumulado;
                        saldoDisponible += concepto.SaldoConceptoCuenta;
                        saldoDisponibleCuenta += concepto.SaldoConceptoCuenta;

                        conceptosSaldosBloqueosDto.ValorAcumulado = valorAcumulado;
                        conceptosSaldosBloqueosDto.EstadoBloqueo = estadoBloqueo;
                        conceptosSaldosBloqueosDto.Razon = razon;
                        conceptosSaldosBloqueosDto.Demandante = demandante;
                        conceptosSaldosBloqueosDto.solicitud = solicitud;
                        conceptosSaldosBloqueosDto.tipo_cuenta = tipo_cuenta;


                        conceptosSaldos.Add(conceptosSaldosBloqueosDto);
                        totalValorBloqueo += concepto.ValorAcumulado;
                    }
                    var tiposDeCuentasObtenidos = await _iEmbargosRequestProvider.ObtenerConceptosAfectadosSolicitudGA2(usuarios.Identificacion);

                    var dataList = cuenta.ConceptosSaldosBloqueos.ToList();
                    var tiposDeCuentasList = tiposDeCuentasObtenidos.Data.ToList();
                    if (cuenta.ConceptosSaldosBloqueos.Any())
                    {

                        foreach (var conceptoCuent in dataList)
                        {
                            var tipoCuenta = tiposDeCuentasList.FirstOrDefault(tc =>
                            tc.CTC_DESCRIPCION == conceptoCuent.Concepto);

                            if (tipoCuenta != null)
                            {
                                conceptoCuent.SaldoConceptoCuenta -= (decimal)tipoCuenta.CAC_VALOR_DESCONTADO;
                            }
                        }
                        cuenta.ConceptosSaldosBloqueos = dataList;
                    }

                    decimal totalEmbargos = 0;
                    foreach (var embargo in cuenta.Embargos)
                    {
                        var saldo = cuenta.Conceptos.Where(x => x.Id == embargo.ConceptoId).FirstOrDefault();
                        if (saldo != null)
                        {
                            totalEmbargos += Convert.ToDecimal(saldo.saldo * embargo.Valor / 100);
                        }

                    }
                    valorTotalEmbargosBloqueos += totalEmbargos;
                    cuenta.SaldoDisponible = saldoDisponibleCuenta;
                    cuenta.SaldoDisponibleEmbargos = saldoDisponibleCuenta - totalValorBloqueoCuenta - totalValorReintegroCuenta - totalEmbargos;
                    cuenta.TotalEmbargos = Convert.ToDecimal(valorTotalEmbargosCuenta);
                    cuenta.TotalBloqueos = totalValorBloqueoCuenta;
                    cuenta.TotalReintegros = totalValorReintegroCuenta;
                    cuenta.ConceptosSaldosBloqueos = conceptosSaldos.OrderByDescending(x => x.FechaBloqueo).ToList();
                    cuenta.EstadoCuenta = estadosCuentasGA2.Data.ToList().Find(x => x.IdEstado == cuenta.IdEct).Estado;
                    cuenta.Movimientos = new List<MovimientoDTO>();
                    if (cuentaIntegracion != null)
                    {
                        foreach (var movimiento in cuentaIntegracion.Movimientos.OrderByDescending(x => x.FechaProceso))
                        {
                            MovimientoDTO movimientoDto = new MovimientoDTO()
                            {
                                CAT_TIPO_MOVIMIENTO = movimiento.TipoMovimiento,
                                CNC_CONCEPTO = conceptosGA2.Data.First(x => x.IdConcepto == movimiento.IdConcepto).Descripcion,
                                CNC_ID = movimiento.IdConcepto,
                                CTA_ID = cuenta.IdCuenta,
                                DCT_ID = movimiento.Documento.IdDocumento,
                                MVT_ANO_PERIODO = movimiento.AnoPeriodo,
                                MVT_FECHA_MOVIMIENTO = movimiento.FechaProceso,
                                MVT_ID = movimiento.IdMovimiento,
                                MVT_FECHA_PROCESO = movimiento.FechaProceso,
                                MVT_MES_PERIODO = movimiento.AnoPeriodo,
                                MVT_VALOR = decimal.Parse(movimiento.Valor)
                            };

                            cuenta.Movimientos.Add(movimientoDto);

                        }

                        /// AGREGAR MOVIMIENTOS DE REINTEGROS

                        var dataLogMovimientosReintegro = await _cuentasConceptosRepository.ObtenerMovimientoReintegro();
                        var dataMovimientoCuenta = await _cuentasConceptosRepository.ObtenerMovimientosCuentaAfiliado(cuenta.IdCuenta);

                        if (dataLogMovimientosReintegro.Count() > 0 && dataMovimientoCuenta.Count() > 0)
                        {
                            var logMovimientosDict = dataLogMovimientosReintegro.ToDictionary(x => x.MovimientoId);
                            var dataMovimiento = dataMovimientoCuenta.Where(m => logMovimientosDict.ContainsKey(m.MVT_ID)).ToList();
                            foreach (var m in dataMovimiento)
                            {
                                var validateMovimiento = dataLogMovimientosReintegro.Where(x => x.MovimientoId == m.MVT_ID).FirstOrDefault();
                                if (validateMovimiento != null)
                                {
                                    MovimientoDTO movimientoDto = new MovimientoDTO()
                                    {
                                        CAT_TIPO_MOVIMIENTO = m.CAT_TIPO_MOVIMIENTO,
                                        CNC_CONCEPTO = conceptosGA2.Data.First(x => x.IdConcepto == validateMovimiento.ConceptoId).Descripcion,
                                        CNC_ID = m.CNC_ID,
                                        CTA_ID = m.CTA_ID,
                                        DCT_ID = m.DCT_ID,
                                        MVT_ANO_PERIODO = m.MVT_ANO_PERIODO,
                                        MVT_FECHA_MOVIMIENTO = m.MVT_FECHA_MOVIMIENTO,
                                        MVT_ID = m.MVT_ID,
                                        MVT_FECHA_PROCESO = m.MVT_FECHA_PROCESO,
                                        MVT_MES_PERIODO = m.MVT_ANO_PERIODO,
                                        MVT_VALOR = (decimal)m.MVT_VALOR
                                    };

                                    cuenta.Movimientos.Add(movimientoDto);
                                }
                            }
                            cuenta.Movimientos = cuenta.Movimientos.OrderByDescending(x => x.MVT_FECHA_PROCESO).ToList();
                        }
                    }
                }
                var solicitudes = usuarios.Solicitudes.GroupBy(x => x.NumeroSolicitud);

                foreach (var solicitud in solicitudes)
                {
                    var solicitudKeys = solicitud.OrderByDescending(x => x.FechaInicioSolicitud).First();
                    solicitudesBUADto.Add(solicitudKeys);
                }
                usuarios.Solicitudes = solicitudesBUADto.OrderByDescending(x => x.FechaInicioSolicitud).ToList();
                usuarios.SaldoDisponible = saldoDisponible - totalValorBloqueo - totalValorReintegro - Convert.ToDecimal(valorTotalEmbargos) - valorTotalEmbargosBloqueos;
                usuarios.SaldoDisponible = usuarios.SaldoDisponible < 0 ? 0 : usuarios.SaldoDisponible;
                ClienteDto clientePayload = this._mapper.Map<ClienteDto>(clienteIntegracionGA2?.ObtenerBUAResult);
                usuarios.Saldos = new();
                listSaldos = await BuscarSaldos(clientePayload.Cuentas);
                var listAfiliaciones = await _iEmbargosRequestProvider.ObtenerTiposAfiliacionGA2();


                usuarios.TiposVinculacion = listAfiliaciones.Data.Where(x => x.AplicaClienteTipoActor).ToList();

                foreach (var item in usuarios.Solicitudes)
                {
                    if (item.TipoSolicitud == "POSTULACIÓN FONDO SOLIDARIDAD")
                    {
                        var valiadateExistData = await _iEmbargosRequestProvider.ObtenerEstadoSolicitudFondo(item.Radicado);
                        if (valiadateExistData.Data != null)
                        {
                            item.EstadoSolicitud = valiadateExistData.Data.Estado;
                            item.Modalidad = item.Modalidad == "HÉROES" ? item.Modalidad : valiadateExistData.Data.Modalidad;
                            item.FechaFinSolicitud = valiadateExistData.Data.FechaCreacion;
                        }
                    }
                }

                foreach (var saldo in listSaldos)
                    usuarios.Saldos.Add(saldo);

                if (usuarios.CuentasHabilitadas == 0)
                {
                    return new Response<ClienteDto> { Data = usuarios, IsSuccess = true, ReturnMessage = "El afiliado no tiene cuentas vigentes" };
                }

                return new Response<ClienteDto> { Data = usuarios };
            }
        }

        /// <summary> 
        /// Metodo obtener info cliente por tipo y número de documento 
        /// </summary>
        /// <param name="identificationNumber">Identificacion del cliente</param>
        /// <param name="identificationType">Tipo identificacion</param>
        /// <param name="idCLiente"></param>
        /// <author>Erwin Pantoja España</author>
        /// <date>4/10/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<ClienteCesantiasDto>> ObtenerInformacionClientePorDocumentoYTipo(int identificationType, string identificationNumber, int idCLiente)
        {
            ClienteCesantiasDto usuarios = this._mapper.Map<ClienteCesantias, ClienteCesantiasDto>(await _clientsRepository.ObtenerInformacionClientePorDocumentoYTipo(identificationType, identificationNumber, idCLiente));
            return new Response<ClienteCesantiasDto> { Data = usuarios };
        }

        /// <summary>
        /// Metodo obtener info cliente por tipo y número de documento 
        /// </summary>
        /// <param name="Cliente">Modelo dto cliente</param>
        /// <author>Nicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<ClienteDto>> ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular(string tipoDocumento, string numeroIdentificacion, string numeroCelular, DateTime fechaExpedicion)
        {
            var a = await _clientsRepository.ObtenerClientePorTipoDocumentoNumeroFechaExpedicionCelular(
                tipoDocumento, numeroIdentificacion, numeroCelular, fechaExpedicion);
            var cliente = this._mapper.Map<ClienteDto>(a);
            return new Response<ClienteDto> { Data = cliente };
        }

        /// <summary>
        /// Metodo obtener clientes existentes y no esxistentes
        /// </summary>
        /// <param name="Cliente">Modelo dto cliente</param>
        /// <author>Nicolas Florez Sarraola</author>
        /// <date>12/03/2021</date>
        /// <returns>Modelo dto de un cliente</returns>
        public async Task<Response<IEnumerable<ClienteSinCrearDTO>>> ValidarClientes(string file)
        {
            var clientes = await _clientsRepository.ValidarNomina(file);
            var respuesta = await _clientsRepository.CrearClientesPorCargaNomina(clientes);

            return new Response<IEnumerable<ClienteSinCrearDTO>>
            {
                Data = this._mapper.Map<IEnumerable<ClienteSinCrearDTO>>(respuesta),
                ReturnMessage = $"Lista de usuarios que no están en BUA creados"
            };
        }

        public async Task<Response<ClienteDto>> RestaurarIntegracionCliente(RestaurarIntegracionClienteDTO restaurarIntegracionCliente)
        {
            ObtenerBuaResponse clienteIntegracionGA2 = await ObtenerClienteIntegracionGA2(restaurarIntegracionCliente.IdTipoIdentificacion, restaurarIntegracionCliente.NumeroIdentificacion);
            if (clienteIntegracionGA2 == null) { return new Response<ClienteDto> { Data = null }; }
            var catalogoValores = await _iEmbargosRequestProvider.ObtenerValoresCatalogosPorIdCatalogo(54);
            var tramites = await _iEmbargosRequestProvider.EliminarSolicitudes(restaurarIntegracionCliente.NumeroIdentificacion);

            var Actividad = catalogoValores.Data
            ?.FirstOrDefault(c => c.Descripcion == clienteIntegracionGA2.ObtenerBUAResult.Actividad)?
            .IdValor;


            ClienteDto cliente = this._mapper.Map<ClienteDto>(clienteIntegracionGA2.ObtenerBUAResult);
            cliente.IdCliente = restaurarIntegracionCliente.IdCliente;
            cliente.IdInformacionEconomica = restaurarIntegracionCliente.IdInformacionFinanciera;
            cliente.FechaActualizaDatosContacto = clienteIntegracionGA2.ObtenerBUAResult.Direccion.FechaActualizacion;
            cliente.IdDireccion = restaurarIntegracionCliente.IdDireccion;
            cliente.CLV_ID = Actividad;
            var clienteEntidad = _mapper.Map<Cliente>(cliente);

            var clienteRestaurado = await _clientsRepository.RestaurarIntegracionCliente(clienteEntidad);

            var nuevoCliente = _mapper.Map<ClienteDto>(clienteRestaurado);

            await InsertarIntegracionCuentas(nuevoCliente, clienteIntegracionGA2.ObtenerBUAResult);

            return new Response<ClienteDto> { Data = nuevoCliente };
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
            _logger.LogInformation("pasa por produccuin");
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
        private async Task<ObtenerBuaResponse> ObtenerClienteIntegracionGA2New(int identificationType, string identificationNumber)
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
            _logger.LogInformation("pasa por produccuin");
            responseIntegracion = await _IBuaService2BusinessLogic.ObtenerBUAAsync(identificationNumber, identificationType);

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
        private async Task<ClienteDto> InsertarIntegracionCuentas(ClienteDto cliente, ClienteIntegracionGA2DTO clienteIntegracionGA2)
        {
            if (clienteIntegracionGA2.Cuentas != null)
            {
                cliente.Cuentas = new List<CuentaDto>();
                foreach (var cuenta in clienteIntegracionGA2.Cuentas)
                {
                    CuentaDto cuentaDto = this._mapper.Map<CuentaDto>(cuenta);
                    cuentaDto.IdCliente = cliente.IdCliente;
                    cuentaDto.IdCuentaIntegracion = cuenta.IdCuenta;
                    var cuentaInserted = await _clientsRepository.InsertarCuentaIntegracion(this._mapper.Map<Domain.Entities.Cuenta>(cuentaDto));
                    cliente.Cuentas.Add(cuentaDto);
                    cliente.Cuentas.LastOrDefault().Movimientos = new List<MovimientoDTO>();
                    cliente.Cuentas.LastOrDefault().ConceptosSaldosBloqueos = new List<ConceptosSaldosBloqueosDto>();

                    foreach (var movimiento in cuenta.Movimientos)
                    {
                        MovimientoDTO movimientoDTO = this._mapper.Map<MovimientoDTO>(movimiento);
                        movimientoDTO.CTA_ID = cuentaInserted.CTA_ID;
                        this._mapper.Map<MovimientoDTO>(await _clientsRepository.InsertarMovimientoIntegracion(this._mapper.Map<Entities.Movimiento>(movimientoDTO)));
                        cliente.Cuentas.LastOrDefault().Movimientos.Add(movimientoDTO);
                    }

                    foreach (var item in cuenta.ConceptosSaldosBloqueos)
                    {
                        ConceptosSaldosBloqueosDto conceptosSaldosBloqueosDto = this._mapper.Map<ConceptosSaldosBloqueosDto>(item);
                        conceptosSaldosBloqueosDto.IdCuenta = cuentaInserted.CTA_ID;
                        this._mapper.Map<ConceptosSaldosBloqueosDto>(await _clientsRepository.InsertarConceptosSaldosBloqueos(this._mapper.Map<Entities.ConceptosSaldosBloqueos>(conceptosSaldosBloqueosDto)));
                        cliente.Cuentas.LastOrDefault().ConceptosSaldosBloqueos.Add(conceptosSaldosBloqueosDto);
                    }
                }
            }

            if (clienteIntegracionGA2.Dependiente != null)
            {
                cliente.Dependientes = new List<DependienteDto>();
                foreach (var dependiente in clienteIntegracionGA2.Dependiente)
                {
                    DependienteDto dependienteDto = this._mapper.Map<DependienteDto>(dependiente);
                    dependienteDto.IdCliente = cliente.IdCliente;
                    cliente.Dependientes.Add(this._mapper.Map<DependienteDto>(await _clientsRepository.InsertarDependienteIntegracion(_mapper.Map<Dependiente>(dependienteDto))));
                }
            }

            if (clienteIntegracionGA2.AportesCategoria != null)
            {
                cliente.AportesCategoria = new List<AportesClienteDto>();
                foreach (var aporteCategoria in clienteIntegracionGA2.AportesCategoria)
                {
                    AportesClienteDto aportesClienteDto = this._mapper.Map<AportesClienteDto>(aporteCategoria);
                    aportesClienteDto.CLI_ID = cliente.IdCliente;
                    cliente.AportesCategoria.Add(this._mapper.Map<AportesClienteDto>(await _clientsRepository.InsertarAportesIntegracion(_mapper.Map<AportesCliente>(aportesClienteDto))));
                }
            }

            return cliente;
        }

        public async Task<Response<IEnumerable<CuentaAfiliadoDTO>>> ObtenerCuentasMovimientosAfiliado(string identificacion)
        {
            var data = _mapper.Map<IEnumerable<CuentaAfiliado>, IEnumerable<CuentaAfiliadoDTO>>(
                await _clientsRepository.ObtenerCuentasMovimientosAfiliado(identificacion));


            return new Response<IEnumerable<CuentaAfiliadoDTO>> { Data = data };

        }


        public async Task<Response<CuentaDto>> ObtenerCuentaById(string request)
        {
            return new Response<CuentaDto>() { Data = _mapper.Map<CuentaDto>(await _clientsRepository.ObtenerCuentaById(request)) };
        }

        public async Task<Response<CuentaDto>> ObtenerCuentaClientePorIdentificadorNumeroCuenta(int numeroCuenta, int idCliente)
        {
            return new Response<CuentaDto>() { Data = _mapper.Map<CuentaDto>(await _clientsRepository.ObtenerCuentaClientePorIdentificadorNumeroCuenta(numeroCuenta, idCliente)) };
        }

        public async Task<Response<IEnumerable<ClienteDto>>> ObtenerClientesById(string clientesIds)
        {
            IEnumerable<ClienteDto> clientes = this._mapper.Map<IEnumerable<ClienteDto>>(await _clientsRepository.ObtenerClientesById(clientesIds));
            return new Response<IEnumerable<ClienteDto>> { Data = clientes };
        }

        public async Task<Response<IEnumerable<ClienteIdentificacionDto>>> ConsultarClientePorIdentificacion(string identificaciones)
        {
            IEnumerable<ClienteIdentificacionDto> clientes = this._mapper.Map<IEnumerable<ClienteIdentificacionDto>>(await _clientsRepository.ConsultarClientePorIdentificacion(identificaciones));
            return new Response<IEnumerable<ClienteIdentificacionDto>> { Data = clientes };
        }

        public async Task<Response<IEnumerable<CuentaDto>>> ObtenerCuentaIdCuenta(string idCuenta)
        {
            IEnumerable<CuentaDto> cuentas = this._mapper.Map<IEnumerable<CuentaDto>>(await _clientsRepository.ObtenerCuentaIdCuenta(idCuenta));
            return new Response<IEnumerable<CuentaDto>> { Data = cuentas };
        }

        private async Task<List<SaldoDto>> BuscarSaldos(string identificacion)
        {
            var saldos = _mapper.Map<IEnumerable<CuentaAfiliado>, IEnumerable<CuentaAfiliadoDTO>>(await _clientsRepository.ObtenerCuentasMovimientosAfiliado(identificacion)).ToList();
            var saldosAhorro = saldos.FindAll(x => x.IdConcepto == 1 || x.IdConcepto == 2 || x.IdConcepto == 4 || x.IdConcepto == 23);
            var saldosInteresesAhorro = saldos.FindAll(x => x.IdConcepto == 9 || x.IdConcepto == 24);
            var saldosCesantias = saldos.FindAll(x => x.IdConcepto == 3 || x.IdConcepto == 5 || x.IdConcepto == 15);
            var saldosInteresesCesantias = saldos.FindAll(x => x.IdConcepto == 10 || x.IdConcepto == 17);

            SaldoDto saldoAhorroDto = new SaldoDto();
            saldoAhorroDto.Concepto = "Ahorro";
            saldoAhorroDto.Valor = saldosAhorro.Sum(x => x.Saldo);
            SaldoDto saldoInteresesAhorroDto = new SaldoDto();
            saldoInteresesAhorroDto.Concepto = "Int Ahorros";
            saldoInteresesAhorroDto.Valor = saldosInteresesAhorro.Sum(x => x.Saldo);
            SaldoDto saldoCesantiasDto = new SaldoDto();
            saldoCesantiasDto.Concepto = "Cesantias";
            saldoCesantiasDto.Valor = saldosCesantias.Sum(x => x.Saldo);
            SaldoDto saldoAhorroCesantiasDto = new SaldoDto();
            saldoAhorroCesantiasDto.Concepto = "Int Cesantias";
            saldoAhorroCesantiasDto.Valor = saldosInteresesCesantias.Sum(x => x.Saldo);

            //valor total para llevar al componente datosPersonales de la modalidad Desafiliación con cesantías
            SaldoDto valorTotalDto = new SaldoDto();
            valorTotalDto.Concepto = "Valor Total";
            valorTotalDto.Valor = (saldoAhorroDto.Valor + saldoInteresesAhorroDto.Valor + saldoCesantiasDto.Valor + saldoAhorroCesantiasDto.Valor);

            //valor solo aportes para llevar al componente datosPersonales de la modalidad Desafiliación solo aportes
            SaldoDto valorAportesDto = new SaldoDto();
            valorAportesDto.Concepto = "Valor Aportes";
            valorAportesDto.Valor = (saldoAhorroDto.Valor + saldoInteresesAhorroDto.Valor);

            List<SaldoDto> listSaldos = new List<SaldoDto>();

            listSaldos.Add(saldoAhorroDto);
            listSaldos.Add(saldoInteresesAhorroDto);
            listSaldos.Add(saldoCesantiasDto);
            listSaldos.Add(saldoAhorroCesantiasDto);

            return listSaldos;
        }

        private async Task<List<SaldoDto>> BuscarSaldos(List<CuentaDto> cuentas)
        {
            List<ConceptosSaldosBloqueosDto> saldos = new();
            foreach (var item in cuentas)
            {
                List<ConceptosSaldosBloqueosDto> saldosEncontrados = item.ConceptosSaldosBloqueos.FindAll(x => (x.IdConcepto == 1 || x.IdConcepto == 2 ||
                                                                  x.IdConcepto == 4 || x.IdConcepto == 23 ||
                                                                  x.IdConcepto == 9 || x.IdConcepto == 24 ||
                                                                  x.IdConcepto == 3 || x.IdConcepto == 5 ||
                                                                  x.IdConcepto == 15 || x.IdConcepto == 10 ||
                                                                  x.IdConcepto == 17) && x.EstadoBloqueo != "VIGENTE");

                saldos = saldos.Concat(saldosEncontrados).ToList();
            }
            var saldosAhorro = saldos.FindAll(x => x.IdConcepto == 1 || x.IdConcepto == 2 || x.IdConcepto == 4 || x.IdConcepto == 23);
            var saldosInteresesAhorro = saldos.FindAll(x => x.IdConcepto == 9 || x.IdConcepto == 24);
            var saldosCesantias = saldos.FindAll(x => x.IdConcepto == 3 || x.IdConcepto == 5 || x.IdConcepto == 15);
            var saldosInteresesCesantias = saldos.FindAll(x => x.IdConcepto == 10 || x.IdConcepto == 17);

            SaldoDto saldoAhorroDto = new SaldoDto();
            saldoAhorroDto.Concepto = "Ahorro";
            saldoAhorroDto.Valor = saldosAhorro.Sum(x => x.SaldoConceptoCuenta);
            SaldoDto saldoInteresesAhorroDto = new SaldoDto();
            saldoInteresesAhorroDto.Concepto = "Int Ahorros";
            saldoInteresesAhorroDto.Valor = saldosInteresesAhorro.Sum(x => x.SaldoConceptoCuenta);
            SaldoDto saldoCesantiasDto = new SaldoDto();
            saldoCesantiasDto.Concepto = "Cesantias";
            saldoCesantiasDto.Valor = saldosCesantias.Sum(x => x.SaldoConceptoCuenta);
            SaldoDto saldoAhorroCesantiasDto = new SaldoDto();
            saldoAhorroCesantiasDto.Concepto = "Int Cesantias";
            saldoAhorroCesantiasDto.Valor = saldosInteresesCesantias.Sum(x => x.SaldoConceptoCuenta);

            //valor total para llevar al componente datosPersonales de la modalidad Desafiliación con cesantías
            SaldoDto valorTotalDto = new SaldoDto();
            valorTotalDto.Concepto = "Valor Total";
            valorTotalDto.Valor = (saldoAhorroDto.Valor + saldoInteresesAhorroDto.Valor + saldoCesantiasDto.Valor + saldoAhorroCesantiasDto.Valor);

            //valor solo aportes para llevar al componente datosPersonales de la modalidad Desafiliación solo aportes
            SaldoDto valorAportesDto = new SaldoDto();
            valorAportesDto.Concepto = "Valor Aportes";
            valorAportesDto.Valor = (saldoAhorroDto.Valor + saldoInteresesAhorroDto.Valor);
            List<SaldoDto> listSaldos = new List<SaldoDto>();

            listSaldos.Add(saldoAhorroDto);
            listSaldos.Add(saldoInteresesAhorroDto);
            listSaldos.Add(saldoCesantiasDto);
            listSaldos.Add(saldoAhorroCesantiasDto);

            return listSaldos;
        }

        public async Task<Response<ArchivoAplicacionPagoCSVDto>> ObtenerClienteAplicacionPagos(int identificationType, string identificationNumber, IEnumerable<Claim> claims)
        {
            ArchivoAplicacionPagoCSVDto usuarios = new ArchivoAplicacionPagoCSVDto();
            usuarios = this._mapper.Map<ArchivoAplicacionPagoCSV, ArchivoAplicacionPagoCSVDto>(await _clientsRepository.ObtenerClienteAplicacionPagos(identificationType, identificationNumber));
            return new Response<ArchivoAplicacionPagoCSVDto> { Data = usuarios };
        }

        public async Task<Response<InclusionSaldoRequestDto>> InsertarInclusionSaldos(InclusionSaldoRequestDto file)
        {
            List<InclusionSaldoRequestDto> saldosIncluidos = new();
            List<ConceptoCuenta> conceptoCuenta = (List<ConceptoCuenta>)await _clientsRepository.ObtenerCuentasConceptoPorIdentificadorCliente(file.IdCliente);
            List<int> ids = new();

            if (file.GeneraOrdenRecaudo)
            {
                ConsecutivoInclusionSaldos consecutivoInclusionSaldos = await _clientsRepository.ObtenerConsecutivoInclusionSaldos();
                file.NumeroReferencia = consecutivoInclusionSaldos.CIS_ANO.ToString() + consecutivoInclusionSaldos.CIS_MES.PadLeft(2, '0')
                    + consecutivoInclusionSaldos.CIS_DIA.PadLeft(2, '0') + consecutivoInclusionSaldos.CIS_CONSECUTIVO.PadLeft(3, '0');
            }

            foreach (var item in file.Opciones)
            {
                if (conceptoCuenta.Any(x => x.CNC_ID == item.IdConcepto))
                {
                    InclusionAportes inclusionAportes = _mapper.Map<InclusionAportes>(item);
                    if (!ids.Contains(conceptoCuenta.Find(x => x.CNC_ID == item.IdConcepto).CTA_ID))
                    {
                        file.IdCuenta = conceptoCuenta.Find(x => x.CNC_ID == item.IdConcepto).CTA_ID;
                        saldosIncluidos.Add(_mapper.Map<InclusionSaldoRequestDto>(await _clientsRepository.InsertarInclusionSaldoEncabezado(_mapper.Map<InclusionAportesRequest>(file))));
                        ids.Add(file.IdCuenta);
                    }

                    inclusionAportes.IAP_ID = saldosIncluidos.Find(x => x.IdCuenta == conceptoCuenta.Find(x => x.CNC_ID == item.IdConcepto).CTA_ID).IdInclusionSaldoEncabezado;
                    inclusionAportes = await _clientsRepository.InsertarInclusionSaldo(inclusionAportes);
                    if (file.Aprobacion && file.GeneraOrdenRecaudo)
                    {
                        await _clientsRepository.InsertarMovimientoInclusionSaldos(new MovimientosCuenta()
                        {
                            CNC_ID = inclusionAportes.CNC_ID,
                            MVT_VALOR = (double)inclusionAportes.IPD_VALOR,
                            MVT_FECHA_PROCESO = item.PeriodoHasta,
                            IPD_ID = item.IdInclusionSaldoOpciones == 0 ? inclusionAportes.IPD_ID : item.IdInclusionSaldoOpciones,
                        }, file.IdCliente);
                        await _clientsRepository.ActualizarSaldosCuentasConceptos(inclusionAportes.IPD_VALOR, inclusionAportes.CNC_ID, file.IdCliente);
                    }
                    else if (file.GeneraOrdenRecaudo)
                    {
                        await _clientsRepository.InsertarRecaudos(_mapper.Map<InclusionAportesRequest>(file), inclusionAportes);
                    }
                    else
                    {
                        await _clientsRepository.InsertarMovimientoInclusionSaldos(new MovimientosCuenta()
                        {
                            CNC_ID = inclusionAportes.CNC_ID,
                            MVT_VALOR = (double)inclusionAportes.IPD_VALOR,
                            MVT_FECHA_PROCESO = item.PeriodoHasta,
                            IPD_ID = item.IdInclusionSaldoOpciones == 0 ? inclusionAportes.IPD_ID : item.IdInclusionSaldoOpciones,
                        }, file.IdCliente);
                        await _clientsRepository.ActualizarSaldosCuentasConceptos(inclusionAportes.IPD_VALOR, inclusionAportes.CNC_ID, file.IdCliente);
                    }
                }

            }

            return new Response<InclusionSaldoRequestDto>()
            {
                Data = null
            };
        }

        public async Task<Response<InclusionSaldoRequestDto>> EliminarInclusionSaldosEncabezado(int request)
        {
            InclusionSaldoRequestDto inclusionSaldo = this._mapper.Map<InclusionSaldoRequestDto>(await _clientsRepository.EliminarInclusionSaldosEncabezado(request));
            return new Response<InclusionSaldoRequestDto> { Data = inclusionSaldo };
        }

        public async Task<Response<InclusionSaldoDto>> EliminarInclusionSaldos(int request)
        {
            InclusionSaldoDto inclusionSaldo = this._mapper.Map<InclusionSaldoDto>(await _clientsRepository.EliminarInclusionSaldos(request));
            return new Response<InclusionSaldoDto> { Data = inclusionSaldo };
        }

        public async Task<Response<ResponseDatosSimulacionParaEstudioDto>> ObtenerCuotasNumeroIdentificacion(string numeroDocumento)
        {

            ResponseDatosSimulacionParaEstudioDto result = this._mapper.Map<ResponseDatosSimulacionParaEstudioDto>(await _clientsRepository.ObtenerCuotasNumeroIdentificacion(numeroDocumento));
            return new Response<ResponseDatosSimulacionParaEstudioDto> { Data = result };
        }

        public async Task<Response<Domain.Entities.Movimiento>> InsertarMovimiento(Domain.Entities.Movimiento movimiento)
        {
            Response<Domain.Entities.Movimiento> result = new Response<Domain.Entities.Movimiento>();
            result.Data = await _clientsRepository.InsertarMovimiento(movimiento);

            return result;
        }

        public async Task<Response<ConceptoCuenta>> ActualizarSaldoDebitoCuenta(Domain.Entities.Movimiento movimiento)
        {
            Response<ConceptoCuenta> result = new Response<ConceptoCuenta>();
            result.Data = await _clientsRepository.ActualizarSaldoDebitoCuenta(movimiento);

            return result;
        }

        public async Task<Response<IEnumerable<ClienteDto>>> ObtenetClientesPorUnidadEjecutora(int idUnidadEjecutora)
        {
            return new Response<IEnumerable<ClienteDto>> { Data = this._mapper.Map<IEnumerable<ClienteDto>>(await _clientsRepository.ObtenetClientesPorUnidadEjecutora(idUnidadEjecutora)) };
        }

        public async Task<Response<Entities.Movimiento>> InsertarMovimientoAbonoIntereses(Entities.Movimiento movimiento)
        {

            Response<Domain.Entities.Movimiento> result = new Response<Domain.Entities.Movimiento>();
            result.Data = await _clientsRepository.InsertarMovimientoAbonoInteres(movimiento);

            return result;

        }

        public async Task<Response<IEnumerable<ClienteDto>>> ObtenerNombreClientePorCedulas(List<string> Identificacion)
        {
            string listaCedulas = string.Empty;

            foreach (var item in Identificacion)
            {
                listaCedulas += $"'{item}',";
            }

            listaCedulas = listaCedulas != string.Empty ? listaCedulas.Remove(listaCedulas.Length - 1) : listaCedulas;


            return new Response<IEnumerable<ClienteDto>> { Data = listaCedulas != string.Empty ? this._mapper.Map<IEnumerable<ClienteDto>>(await _clientsRepository.ObtenerNombreClientePorCedulas(listaCedulas)) : null };
        }

        public async Task<Response<ConsultarPermisoActualizacionDto>> ConsultarPermisoActualizacion(string request)
        {
            Response<ConsultarPermisoActualizacionDto> result = new Response<ConsultarPermisoActualizacionDto>();
            result.Data = await _clientsRepository.ConsultarPermisoActualizacion(request);

            return result;
        }
        public async Task<Response<ResponseInsertarPermisoActualizacionDto>> InsertarPermisoActualizacion(InsertarPermisoActualizacionDto request)
        {
            Response<ResponseInsertarPermisoActualizacionDto> result = new Response<ResponseInsertarPermisoActualizacionDto>();
            result.Data = await _clientsRepository.InsertarPermisoActualizacion(request);

            return result;
        }

        public async Task<Response<IEnumerable<ReporteAfiliadosResponseDto>>> GenerarReporteAfiliados(ReporteAfiliadosRequestDto request)
        {

            var fuerzas = await _iEmbargosRequestProvider.ObtenerFuerzasGA2();
            var unidades = await _iEmbargosRequestProvider.ObtenerUnidadesEjecutorasGA2();
            var grados = await _iEmbargosRequestProvider.ObtenerGradosGA2();
            var categorias = await _iEmbargosRequestProvider.ObtenerCategoriasGA2();
            var estadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeClientesGA2();
            var listAfiliaciones = await _iEmbargosRequestProvider.ObtenerTiposAfiliacionGA2();

            var result = new Response<IEnumerable<ReporteAfiliadosResponseDto>>();
            result.Data = await _clientsRepository.ConsultaReporteAfiliados(request);

            foreach (var item in result.Data)
            {

                item.IdFuerza = fuerzas.Data.FirstOrDefault(f => f.Id.ToString() == item.IdFuerza)?.Descripcion;
                item.IdUnidadEjecutora = unidades.Data.FirstOrDefault(u => u.Id.ToString() == item.IdUnidadEjecutora)?.Nombre;
                item.IdGrado = grados.Data.FirstOrDefault(g => g.Id.ToString() == item.IdGrado)?.Descripcion;
                item.IdCategoria = categorias.Data.FirstOrDefault(c => c.Id.ToString() == item.IdCategoria)?.Descripcion;
                item.EstadoCliente = estadosClientesGA2.Data.FirstOrDefault(c => c.FirtsId.ToString() == item.EstadoCliente)?.Descriptions;
                item.TipoVinculacion = listAfiliaciones.Data.FirstOrDefault(c => c.IdTipoActor.ToString() == item.TipoVinculacion)?.DescripcionTipoActor;
            }

            return result;
        }



        public async Task<Response<ReporteFuerzaResumenDto>> GenerarReporteAfiliadosPorFuerza(int idFuerza)
        {
            var resultados = await _clientsRepository.ConsultaReporteAfiliadosPorFuerza(idFuerza);
            var fuerzas = await _iEmbargosRequestProvider.ObtenerFuerzasGA2();
            var estadosClientesGA2 = await _iEmbargosRequestProvider.ObtenerEstadosDeClientesGA2();

            var reportePorFuerza = resultados
                .Where(x => x.ECL_ID != "TOTAL FUERZA")
                .GroupBy(x => x.FRZ_ID)
                .Select(g =>
                {
                    var fuerzaNombre = fuerzas.Data
                        .FirstOrDefault(f => f.Id.ToString() == g.Key)?.Descripcion ?? g.Key;

                    return new ReporteFuerzaAgrupadoDto
                    {
                        NombreFuerza = fuerzaNombre,
                        Estados = g.Select(e => new EstadoConteoDto
                        {
                            Estado = estadosClientesGA2.Data
                                .FirstOrDefault(c => c.FirtsId.ToString() == e.ECL_ID)?.Descriptions ?? e.ECL_ID,
                            Total = e.CLI_CONTEO_ESTADO
                        }).ToList(),
                        TotalFuerza = resultados
                            .Where(t => t.FRZ_ID == g.Key && t.ECL_ID == "TOTAL FUERZA")
                            .Select(t => t.CLI_CONTEO_ESTADO)
                            .FirstOrDefault()
                    };
                })
                .ToList();

            var conteoPorEstado = resultados
                .Where(x => x.ECL_ID != "TOTAL FUERZA")
                .GroupBy(x => x.ECL_ID)
                .Select(g => new EstadoConteoDto
                {
                    Estado = estadosClientesGA2.Data
                        .FirstOrDefault(c => c.FirtsId.ToString() == g.Key)?.Descriptions ?? g.Key,
                    Total = g.Sum(x => x.CLI_CONTEO_ESTADO)
                })
                .OrderBy(e => e.Estado)
                .ToList();

            var totalGeneral = conteoPorEstado.Sum(e => e.Total);


            var respuesta = new ReporteFuerzaResumenDto
            {
                Fuerzas = reportePorFuerza,
                ConteoPorEstado = conteoPorEstado,
                TotalGeneral = totalGeneral
            };

            return new Response<ReporteFuerzaResumenDto> { Data = respuesta };
        }



        public async Task<Response<IEnumerable<ReporteTotalesDto>>> GenerarReporteTotalesPorConcepto()
        {
            var result = await _clientsRepository.GenerarReporteTotalesPorConcepto();
            var conceptosGA2 = await _iEmbargosRequestProvider.ObtenerConceptosGA2();
            var unidades = await _iEmbargosRequestProvider.ObtenerUnidadesEjecutorasGA2();

            foreach (var item in result)
            {
                var unidad = unidades.Data.FirstOrDefault(u => u.Id == item.UEJ_ID);
                var concepto = conceptosGA2.Data.FirstOrDefault(c => c.IdConcepto == item.ConceptoID);

                item.NombreUnidadEjecutora = unidad?.Nombre;
                item.NombreConcepto = concepto?.Descripcion;
            }

            return new Response<IEnumerable<ReporteTotalesDto>>
            {
                Data = result,
                IsSuccess = true,
                ReturnMessage = "Reporte generado correctamente"
            };
        }



        public async Task<Response<IEnumerable<ClienteDto>>> GenerarReporteDatosBasicosAfiliado(ReporteDatosBasicosRequestDto request)
        {
            return new Response<IEnumerable<ClienteDto>> { Data = this._mapper.Map<IEnumerable<ClienteDto>>(await _clientsRepository.GenerarReporteDatosBasicosAfiliado(request.IdDepartamento, request.IdCiudad)) };
        }

        public async Task<Response<IEnumerable<InformacionBasicaClienteDto>>> ObtenerInformacionClientesPorIdentificacion(IEnumerable<string> documentos)
        {
            var result = await _clientsRepository.ObtenerInformacionClientesPorIdentificacion(documentos);
            return new Response<IEnumerable<InformacionBasicaClienteDto>> { Data = _mapper.Map<IEnumerable<InformacionBasicaClienteDto>>(result) };
        }

        public async Task<Response<AnotacionClienteDto>> InsertarAnotacionCliente(AnotacionClienteDto request)
        {
            var result = await _clientsRepository.InsertarAnotacionCliente(request);
            return new Response<AnotacionClienteDto> { Data = _mapper.Map<AnotacionClienteDto>(result) };
        }

        public async Task<Response<IEnumerable<AnotacionClienteDto>>> ObtenerAnotacionesCliente(AnotacionClienteDto request)
        {
            var result = await _clientsRepository.ObtenerAnotacionesCliente(request);
            return new Response<IEnumerable<AnotacionClienteDto>> { Data = _mapper.Map<IEnumerable<AnotacionClienteDto>>(result) };
        }

        public async Task<Response<IEnumerable<CausalEstadoClienteDto>>> ObtenerCausalesEstadoCliente(CausalEstadoClienteDto request)
        {
            var result = await _clientsRepository.ObtenerCausalesEstadoCliente(request);
            return new Response<IEnumerable<CausalEstadoClienteDto>> { Data = _mapper.Map<IEnumerable<CausalEstadoClienteDto>>(result) };
        }

        public async Task<Response<CausalEstadoClienteDto>> InsertarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            var result = await _clientsRepository.InsertarEstadoCausalCliente(request);
            return new Response<CausalEstadoClienteDto> { Data = _mapper.Map<CausalEstadoClienteDto>(result) };
        }

        public async Task<Response<AnotacionClienteDto>> EliminarAnotacionCliente(AnotacionClienteDto request)
        {
            var result = await _clientsRepository.EliminarAnotacionCliente(request);
            return new Response<AnotacionClienteDto> { Data = _mapper.Map<AnotacionClienteDto>(result) };
        }

        public async Task<Response<CausalEstadoClienteDto>> EliminarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            var result = await _clientsRepository.EliminarEstadoCausalCliente(request);
            return new Response<CausalEstadoClienteDto> { Data = _mapper.Map<CausalEstadoClienteDto>(result) };
        }
        public async Task<Response<CausalEstadoClienteDto>> LevantarEstadoCausalCliente(CausalEstadoClienteDto request)
        {
            var result = await _clientsRepository.LevantarEstadoCausalCliente(request);
            return new Response<CausalEstadoClienteDto>
            {
                Data = _mapper.Map<CausalEstadoClienteDto>(result)
            };
        }
    }
}

