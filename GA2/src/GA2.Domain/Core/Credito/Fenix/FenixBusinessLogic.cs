using GA2.Application.Dto;
using GA2.Domain.Core.Servicios.Fenix;
using GA2.Domain.Entities;
using GA2.Infraestructure.Repositories;
using GA2.Transversals.Commons;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GA2.Domain.Core
{
    public class FenixBusinessLogic : IFenixBusinessLogic
    {
        private readonly IFenixService _fenixService;
        private readonly ICreditoRepository _creditoRepository;

        public FenixBusinessLogic(IFenixService fenixService, ICreditoRepository creditoRepository)
        {
            _fenixService = fenixService ?? throw new ArgumentNullException(nameof(fenixService));
            _creditoRepository = creditoRepository;
        }

        public async Task<Response<diffgram>> ValidarBiometria(string request)
        {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();

            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals("FENIX") && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR)) //reemplazar por opciones fenix
            {
                var responseBiometria = await _fenixService.ValidarBiometriaAsync(new() { identificacion = int.Parse(request) });

                var s = new XMLHandler();
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(diffgram));
                    var prueba = string.Empty;
                    using (StringReader sr = new StringReader(responseBiometria.ValidarBiometriaResult.Nodes[1].ToString()))
                    {
                        prueba = sr.ReadToEnd();
                    }
                    var response = s.Deserialize<diffgram>(prueba);
                    if (response.NewDataSet == null)
                    {

                        var notificacion = new InsertarNotificacionFuncionarioExterno
                        {
                            Perfil = "ASERA_T",
                            Mensaje = "Por favor Cargar información en fenix para el afiliado de identificación: " + request,
                            Modulo = "Biometria"
                        };

                        // agregarnotificacion
                         var ResponseNotificacion = await _creditoRepository.InsertarNotificacionFuncionarioExterno(notificacion);
                       
                        

                        return new Response<diffgram>() { Data = new diffgram(), IsSuccess = false, ReturnMessage = "No hay información en Fénix para ese afiliado" };

                    }

                    //inicio seccion se hace para quemar la respuesta, para quitar esto hay que descomentar el código y quitar esta seccion
                    //diffgram response = new()
                    //{
                    //    NewDataSet = new()
                    //    {
                    //        Table1 = new()
                    //        {
                    //            A008CEDULA = 1,
                    //            A008HUELLA = 1,
                    //        }
                    //    }
                    //};
                    // fin seccion
                    return new Response<diffgram>() { Data = response, IsSuccess = true, ReturnMessage = "Biometria verificada" };
                }
                catch (Exception ex)
                {

                    var notificacion = new InsertarNotificacionFuncionarioExterno
                    {
                        Perfil = "ASERA_T",
                        Mensaje = "Por favor Cargar información en fenix para el afiliado de identificación: " + request,
                        Modulo = "Biometria"
                    };

                    // agregarnotificacion
                    var ResponseNotificacion = await _creditoRepository.InsertarNotificacionFuncionarioExterno(notificacion);


                    return new Response<diffgram>() { IsSuccess = false, ReturnMessage = "Usuario sin datos en Fenix" };
                }

            }
            else {
                return new Response<diffgram>() { IsSuccess = false, ReturnMessage = "No se pudo realizar la validacion de biometria para el documento" };
            }
        }

        public async Task<Response<string>> ValidacionBiometria(ClienteIdentificacionDto request) {
            var parametrosIntegraciones = await _creditoRepository.ObtenerParametrosIntegraciones();

            if (bool.Parse(parametrosIntegraciones.First(p => p.INTEGRACION.ToUpper().Equals("FENIX") && p.PARAMETRO.ToUpper().Equals("ESTADO")).VALOR)) //reemplazar por opciones fenix
            {
                try
                {
                    var responseBiometria = await _fenixService.ValidarBiometriaAsync(new() { identificacion = int.Parse(request.Identificacion) });
                    var s = new XMLHandler();
                    XmlSerializer ser = new XmlSerializer(typeof(diffgram));
                    var prueba = string.Empty;
                    using (StringReader sr = new StringReader(responseBiometria.ValidarBiometriaResult.Nodes[1].ToString()))
                    {
                        prueba = sr.ReadToEnd();
                    }
                    var response = s.Deserialize<diffgram>(prueba);
                    if (response.NewDataSet == null)
                    {
                        return new Response<string>() { Data = "No hay información en Fénix para ese afiliado", IsSuccess = false, ReturnMessage = "No hay información en Fénix para ese afiliado" };
                    }
                    return new Response<string>() { Data = "", IsSuccess = true, ReturnMessage = "" };
                }
                catch (Exception ex)
                {
                    return new Response<string>() { Data = $"Usuario {request.PrimerNombre} {request.SegundoNombre} {request.PrimerApellido} {request.SegundoApellido}, sin datos en Fenix", 
                        IsSuccess = false, ReturnMessage = $"Usuario {request.PrimerNombre} {request.SegundoNombre} {request.PrimerApellido} {request.SegundoApellido}, sin datos en Fenix" };
                }
            }
            else
            {
                return new Response<string>() { Data = $"No se pudo realizar la validacion de biometria para el documento {request.Identificacion}, usuario {request.PrimerNombre} {request.SegundoNombre} {request.PrimerApellido} {request.SegundoApellido}", 
                    IsSuccess = false, ReturnMessage = $"No se pudo realizar la validacion de biometria para el documento {request.Identificacion}, usuario {request.PrimerNombre} {request.SegundoNombre} {request.PrimerApellido} {request.SegundoApellido}"
                };
            }
        }
    }
}
