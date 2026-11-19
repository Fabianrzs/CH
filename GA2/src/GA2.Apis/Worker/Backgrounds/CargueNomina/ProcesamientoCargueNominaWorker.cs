using AutoMapper;
using Azure.Storage.Blobs.Models;
using GA2.Application.Dto;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Worker.Backgrounds.CargueNomina
{
    /// <summary>
    /// Procesamiento de cargue de nomina
    /// </summary>
    class ProcesamientoCargueNominaWorker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMapper _mapper;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaWorkerRepository _cuentaWorkerRepository;
        private readonly IClienteWorkRepository _clienteWorkerRepository;
        private const int documentoProcesar = 6;

        public ProcesamientoCargueNominaWorker(ILogger<Worker> logger,
                                               IMapper mapper,
                                               IClienteRepository clienteRepository,
                                               IMovimientoRepository movimientoRepository,
                                               ICuentaWorkerRepository cuentaWorkerRepository,
                                               IClienteWorkRepository clienteWorkerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _movimientoRepository = movimientoRepository ?? throw new ArgumentNullException(nameof(movimientoRepository));
            _cuentaWorkerRepository = cuentaWorkerRepository ?? throw new ArgumentNullException(nameof(cuentaWorkerRepository));
            _clienteWorkerRepository = clienteWorkerRepository ?? throw new ArgumentNullException(nameof(clienteWorkerRepository));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var documentos = await _movimientoRepository.ObtenerDocumentosCarga(documentoProcesar);

                foreach (var item in documentos)
                {
                    _logger.LogInformation("Worker iniciando en: {time}", DateTimeOffset.Now);
                    // obtener la cuenta storage por el nombre del archivo item.DCT_NOMBRE
                    var cuentaStorage = await _movimientoRepository.ObtenerBlobStoragePorNombre(item.DCT_NOMBRE);
                    var blob = await ObtenerDocumentoBlob(cuentaStorage);
                    StreamReader reader = new(blob.Content);
                    var componentes = DeserializarComponente(reader);
                    var rondacomponente = default(int);
                    _logger.LogInformation($"Obteniendo clientes por tipo y numero de identificacion del blob == {DateTime.UtcNow}");
                    foreach (var componente in componentes.BODY)
                    {
                        try
                        {
#if DEBUG
                            rondacomponente++;
                            Console.WriteLine($"{rondacomponente}/{componentes.BODY.Count()}");
#endif
                            //Validacion de existencia de cliente
                            var cliente = await _clienteWorkerRepository.ObtenerClientePorTipoIdentificationYNumero
                                (componente.TIPO_IDENTIFICACION, componente.IDENTIFICACION);

                            if (cliente == null)
                            {
                                _logger.LogInformation($"Cliente no registrado: {JsonConvert.SerializeObject(componente)} {DateTimeOffset.Now}");
                                //armar cliente
                                var a = await _clienteWorkerRepository.InsertarClienteNomina(componente);
                                _logger.LogInformation($"Cliente registrado con exito {DateTimeOffset.Now}");
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(exception.Message);
                        }
                    }

                    _logger.LogInformation($"Finalizacion clientes por tipo y numero de identificacion del blob == {DateTime.UtcNow}");

                    componentes.DOCUMENTO = _mapper.Map<DocumentoDto>(item);
                    // Registra Clientes Crear Cliente no existentes

                    try
                    {
                        // conciliacion de la nomina Consultas y validaciones
                        // Integracion con seven para conciliacion falta definiciaon del proceso.


                        // creacion de cuentas Crear Cuentas no existentes
                        _logger.LogInformation("Creando cuentas en : {time}", DateTimeOffset.Now);
                        //_ = await _cuentaWorkerRepository.ParametrosCreacionCuenta(componentes.BODY);
                        _ = await _cuentaWorkerRepository.CreacionCuentaCargueNomina(componentes.HEADER, componentes.BODY, componentes.END, componentes.FILENAME);
                        _logger.LogInformation("Cuentas creadas correctamente en: {time}", DateTimeOffset.Now);
                        // creacion de moviientos no existentes
                        //_logger.LogInformation("Creando movimientos en : {time}", DateTimeOffset.Now);
                        //_ = await _cuentaWorkerRepository.ParametrosCreacionMovimientos(componentes.BODY);
                        //_logger.LogInformation("Movimientos creados correctamente en: {time}", DateTimeOffset.Now);


                        /// Ejecuta la conciliacion.
                        /// 


                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }

                    break;

                    _logger.LogInformation("Eliminando archivo cargado .... en {time}", DateTimeOffset.Now);
                    _ = await EliminarDocumentoBlob(cuentaStorage);
                    _logger.LogInformation("Archivo eliminado correctamente en {time}", DateTimeOffset.Now);
                }

                // En caso de error cambiar el estado a fallido o novedad sobre el documento enviar correo y notificar.
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }

        private async Task<BlobDownloadInfo> ObtenerDocumentoBlob(CuentaStorage cuentaStorage)
        {
            try
            {
                return await _movimientoRepository.DescargarArchivoStorage(cuentaStorage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static ComponentesDTO DeserializarComponente(StreamReader reader)
        {
            try
            {
                var blobString = reader.ReadToEnd();
                ComponentesDTO componentes = JsonConvert.DeserializeObject<ComponentesDTO>(blobString);
                return componentes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<Azure.Response> EliminarDocumentoBlob(CuentaStorage cuentaStorage)
        {
            try
            {
                return await _movimientoRepository.EliminarArchivoStorage(cuentaStorage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
