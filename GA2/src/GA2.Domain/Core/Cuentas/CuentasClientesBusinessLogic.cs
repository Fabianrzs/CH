using AutoMapper;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Application.Dto.Catalogs;
using GA2.Application.Main;
using GA2.Application.Main.ProviderRequest;
using GA2.Domain.Core.Reintegros;
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
    public class CuentasClientesBusinessLogic : ICuentasClientesBusinessLogic
    {

        private readonly IMapper _mapper;
        private readonly IClientRequestProvider _iClientRequestProvider;
        private readonly ICuentasClientesRepository _cuentasClientesRepository;
        private readonly IEmbargosRepository _juzgadosRepository;
        private readonly IEmbargosProvider _iEmbargosRequestProvider;
        private readonly IReintegrosRepository _reintegrosRepository;
        private readonly ICatalogosRepository _catalogosRepository;

        public CuentasClientesBusinessLogic(IMapper mapper, IClientRequestProvider iClientRequestProvider, ICuentasClientesRepository cuentasClientesRepository, IEmbargosProvider embargosProvider
            , IReintegrosRepository reintegrosRepository, ICatalogosRepository catalogosRepository, IEmbargosRepository juzgadosRepository)
        {
            _mapper = mapper;
            _iClientRequestProvider = iClientRequestProvider;
            _cuentasClientesRepository = cuentasClientesRepository;
            _iEmbargosRequestProvider = embargosProvider;
            _reintegrosRepository = reintegrosRepository;
            _catalogosRepository = catalogosRepository;
            _juzgadosRepository = juzgadosRepository;
        }

        public async Task<Response<InfoClienteDto>> ObtenerInformacionCliente(int tpo_identificacion, string num_identificacion)
        {
            Response<InfoClienteDto> clienteDto = await _iClientRequestProvider.ObtenerInfoCliente(tpo_identificacion, num_identificacion);
            Response<InfoClienteDto> respuesta = new Response<InfoClienteDto>();

            if (clienteDto.Data != null)
            {
                InfoDetalleCliente detalleCliente = await _cuentasClientesRepository.ObtenerDetalleInfoCliente(_mapper.Map<InfoCliente>(clienteDto.Data));
                InfoClienteDto cliente = clienteDto.Data;
                clienteDto.Data.Cuentas.ForEach(cuenta =>
                {
                    if (cuenta.ListaEmbargos != null)
                    {
                        cuenta.ListaEmbargos = cuenta.ListaEmbargos
                            .Where(embargo => embargo.Remanente != true)
                            .ToList();
                    }
                });


                IEnumerable<TipoCuentaCDto> tipoCuentaCDto = this._mapper.Map<IEnumerable<TipoCuentaCDto>>(await _cuentasClientesRepository.ObtenerTipoCuentaC());
                var estadosCuentas = await _catalogosRepository.ObtenerEctEstadoCuenta();
                IEnumerable<TipoAfiliacionDto> modalidadesAfiliacion = this._mapper.Map<IEnumerable<TipoAfiliacionDto>>(await _catalogosRepository.ObtenerTiposAfiliacion());
                IEnumerable<PorcentajeDescuentoDto> porcentajeDescuentos = this._mapper.Map<IEnumerable<PorcentajeDescuentoDto>>(await _cuentasClientesRepository.ObtenerPorcentajesDescuento());
                IEnumerable<CausalEstadoCuentaDto> causalEstadoCuentas = this._mapper.Map<IEnumerable<CausalEstadoCuentaDto>>(await _cuentasClientesRepository.ObtenerCausalEstadoCuenta());
                IEnumerable<CategoriaDto> categorias = this._mapper.Map<IEnumerable<CategoriaDto>>(await this._cuentasClientesRepository.ObtenerCategorias());
                IEnumerable<DescripcionConcepto> conceptos = await this._cuentasClientesRepository.ObtenerConceptos();

                cliente.Aportes = cliente.Aportes.Join(categorias,
                         aporte => aporte.CTG_ID,
                         categoria => categoria.Id,
                         (aporte, categoria) => new AportesClienteDto
                         {
                             CLI_ID = aporte.CLI_ID,
                             CTG_ID = aporte.CTG_ID,
                             APC_CUOTAS_ACUMULADAS = aporte.APC_CUOTAS_ACUMULADAS,
                             APC_FECHA_ULTIMA_COUTA = aporte.APC_FECHA_ULTIMA_COUTA,
                             APC_FECHA_PRIMERA_CUOTA = aporte.APC_FECHA_PRIMERA_CUOTA,
                             CTG_DESCRIPCION = categoria.Descripcion
                         }
                    ).ToList();

                cliente.Cuentas.ForEach(x =>
                {
                    x.Conceptos.ForEach(async y =>
                    {
                        var descripcion = conceptos.Where(find => find.CNC_ID == y.ConceptoId).FirstOrDefault().CNC_DESCRIPCION;
                        y.DescripcionConcepto = descripcion ?? "";
                        var ValorEmbargo = await _reintegrosRepository.GetAfiliadoByNumeroDocumento(num_identificacion, tpo_identificacion);
                        if (ValorEmbargo is not null)
                        {
                            if (ValorEmbargo.TipoValor == EnumsTipoValorReintegro.Valor)
                            {
                                y.ValorDevolver = ValorEmbargo.ValorSueldo;
                            }
                            else
                            {
                                if (ValorEmbargo.TipoValor == EnumsTipoValorReintegro.Porcenjate)
                                {
                                    y.ValorDevolver = ValorEmbargo.ValorSueldoPorcentaje;

                                }
                            }
                        }
                    });

                    x.Conceptos = x.Conceptos.OrderBy(z => z.DescripcionConcepto).ToList();
                });


                cliente.Categoria = detalleCliente.CATEGORIA;
                cliente.Grado = detalleCliente.GRADO;
                cliente.Fuerza = detalleCliente.FUERZA;
                cliente.TipoIdentificacion = detalleCliente.TIPO_IDENTIFICACION;
                cliente.UnidadEjecutora = detalleCliente.UNIDADE_EJECUTORA;
                cliente.SiglaUnidadEjecutora = detalleCliente.SIGLA_UNIDAD_EJECUTORA;
                cliente.TipoAfiliacion = detalleCliente.TIPO_AFILIACION;
                cliente.causalEstadoCuenta = causalEstadoCuentas.ToList<CausalEstadoCuentaDto>();
                cliente.tipoCuentaC = tipoCuentaCDto.ToList<TipoCuentaCDto>();
                cliente.tipoCuentaC = cliente.tipoCuentaC.Where(x => x.IdTipoCuentaC != 7).ToList();

                cliente.porcentajeDescuentos = porcentajeDescuentos.Where(x => x.ProductoDescuento.Equals("AVAC")).OrderBy(x => x.DescripcionPorcentajeDescuento).ToList();
                List<Application.Dto.Catalogs.EstadoCuentaDto> estadosList = new List<Application.Dto.Catalogs.EstadoCuentaDto>();
                foreach (var estado in estadosCuentas)
                {
                    Application.Dto.Catalogs.EstadoCuentaDto estadoCuentaDto = new Application.Dto.Catalogs.EstadoCuentaDto
                    {
                        IdEstado = estado.ECT_ID,
                        Estado = estado.ECT_DESCRIPCION
                    };
                    if (!estadoCuentaDto.Estado.Contains("SUSPENDIDA"))
                        estadosList.Add(estadoCuentaDto);
                }
                cliente.estadosCuenta = estadosList;
                cliente.modalidadesAfiliacion = modalidadesAfiliacion.ToList();
                return new Response<InfoClienteDto> { Data = cliente };

            }
            else
            {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "El cliente no existe";
                return respuesta;

            }


        }

        public async Task<Response<IEnumerable<CuentaPorConceptoDto>>> ObtenerConceptoDeCuentas(int idCuenta)
        {
            Response<IEnumerable<CuentaPorConceptoDto>> listaConceptoCuenta = await _iClientRequestProvider.obtenerCuentaConcepto(idCuenta);
            Response<IEnumerable<CuentaPorConceptoDto>> respuesta = new Response<IEnumerable<CuentaPorConceptoDto>>();

            if (listaConceptoCuenta.Data != null)
            {
                IEnumerable<DescripcionConceptoDto> listConceptosGa2 = this._mapper.Map<IEnumerable<DescripcionConceptoDto>>(await _cuentasClientesRepository.ObtenerConceptos());

                IEnumerable<CuentaPorConceptoDto> returnListaConceptoCuenta = listaConceptoCuenta.Data.Join(
                      listConceptosGa2,
                      concepto => concepto.IdConcepto,
                      conceptoGa2 => conceptoGa2.IdConcepto,
                       (concepto, conceptoGa2) => new CuentaPorConceptoDto
                       {
                           IdConceptoCuenta = concepto.IdConceptoCuenta,
                           IdCuenta = concepto.IdCuenta,
                           IdConcepto = concepto.IdConcepto,
                           DescripcionConcepto = conceptoGa2.DescripcionConcepto,
                           ConceptoSaldoCuenta = concepto.ConceptoSaldoCuenta,
                           ConceptoFechaUltimoAporte = concepto.ConceptoFechaUltimoAporte,
                           ConceptoIngreso = concepto.ConceptoIngreso,
                           ConceptoSva = concepto.ConceptoSva,
                           cuentaNumero = concepto.cuentaNumero,
                           CuentaFechaCreación = concepto.CuentaFechaCreación,
                           CuentaPrimerAporte = concepto.CuentaPrimerAporte,
                           CuentaSaldo = concepto.CuentaSaldo,
                           CuentaSaldoCanje = concepto.CuentaSaldoCanje,
                           CuentaSaldoDisponible = concepto.CuentaSaldoDisponible,
                           CuentaCuotas = concepto.CuentaCuotas
                       }
                       );



                return new Response<IEnumerable<CuentaPorConceptoDto>> { Data = returnListaConceptoCuenta };

            }
            else
            {

                respuesta.IsSuccess = false;
                respuesta.ReturnMessage = "No existen conceptos";
                return respuesta;

            }


        }

        public async Task<Response<InfoClienteDto>> ObtenerCuentasConceptosPorTipoNumeroIdentificacion(int tpo_identificacion, string num_identificacion)
        {
            Response<InfoClienteDto> clienteDto = await _iClientRequestProvider.ObtenerCuentasConceptosPorTipoNumeroIdentificacion(tpo_identificacion, num_identificacion);
            IEnumerable<DescripcionConceptoDto> listConceptosGa2 = this._mapper.Map<IEnumerable<DescripcionConceptoDto>>(await _cuentasClientesRepository.ObtenerConceptos());
            IEnumerable<EmbargosDto> embargos = _mapper.Map<IEnumerable<EmbargosDto>>(await this._juzgadosRepository.ObtenerEmbargoByIdentificacion(num_identificacion));
            var estadosCuentasGA2 = await _juzgadosRepository.ObtenerEctEstadoCuenta();

            if (!clienteDto.IsSuccess || clienteDto.Data == null)
            {
                return clienteDto;
            }

            List<CuentaClienteDto> lista = new List<CuentaClienteDto>();

            foreach (CuentaClienteDto cuentas in clienteDto.Data.Cuentas)
            {
                // Filtrar conceptos eliminando duplicados por ConceptoId
                var ConceptosDistinct = cuentas.Conceptos
                    .GroupBy(concepto => concepto.ConceptoId) // Agrupa por ConceptoId
                    .Select(group => group.First()) // Selecciona el primer elemento de cada grupo
                    .ToList();

                // Asignar descripciones a los conceptos
                foreach (ConceptoCuentaDto concepto in ConceptosDistinct)
                {
                    var descri = listConceptosGa2.FirstOrDefault(n => n.IdConcepto == concepto.ConceptoId);
                    if (descri != null)
                    {
                        concepto.ConceptoDescripcion = descri.DescripcionConcepto;
                    }
                }

                // Asignar descripción de estado de cuenta
                foreach (var item in estadosCuentasGA2)
                {
                    if (cuentas.EstadoCuentaId == item.ECT_ID)
                    {
                        cuentas.EstadoCuenta = item.ECT_DESCRIPCION;
                    }
                }

                // Asignar los conceptos procesados a la cuenta
                cuentas.Conceptos = ConceptosDistinct;

                // Agregar la cuenta procesada a la lista final
                lista.Add(cuentas);
            }

            clienteDto.Data.Cuentas = lista;
            return clienteDto;
        }


    }
}
