using Dapper;
using GA2.Domain.Entities;
using GA2.Domain.Entities.Correo;
using GA2.Infraestructure.Data;
using GA2.Infraestructure.Data.Credito;
using GA2.Infraestructure.Repositories;
using GA2.Infraestructure.Repositories.EmailNotificacion;
using GA2.Transversals.Commons;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


namespace GA2.Infraestructure.Repositories
{
    public class EmailNotificacionRepository
    : DapperGenericRepository, IEmailNotificacionRepository
    {
        private readonly IConfiguration _configuration;

        public EmailNotificacionRepository(
            IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarAsync(Email request)
        {
            // OBTENER RUTA BASE
            string rutaBase =
                _configuration["CarpetaBlogStorage:CarpetaServidorStorage"] + "\\template-email";

            // OBTENER RUTA DEL MODULO
            string rutaModulo =
                Path.Combine(
                    rutaBase,
                    request.Modulo);

            // CONSTRUIR RUTAS
            string rutaHtml =
                Path.Combine(
                    rutaModulo,
                    request.RutaHtml);

            string rutaImagen =
                Path.Combine(
                    rutaModulo,
                    request.RutaImagenEncabezado);

            // LEER HTML
            string html =
                await File.ReadAllTextAsync(
                    rutaHtml);

            // LEER IMAGEN
            byte[] imageBytes =
                await File.ReadAllBytesAsync(
                    rutaImagen);

           
            // CONVERTIR IMAGEN A BASE64
            string base64Image =
                    Convert.ToBase64String(
                        imageBytes);
           

            // FORMATO IMAGEN HTML
            string imageSrc =
                $"data:image/png;base64,{base64Image}";

            // REEMPLAZAR IMAGEN
            html = html.Replace(
                "{{ImagenEncabezado}}",
                imageSrc);

            // REEMPLAZAR VARIABLES
            foreach (var item in request.Parametros)
            {
                html = html.Replace(
                    $"{{{{{item.Key}}}}}",
                    item.Value);
            }

            using MailMessage correo =
                new MailMessage();

            correo.From =
                new MailAddress(
                    _configuration["SMTP:Usuario"],
                    _configuration["SMTP:NombreRemitente"]);

            // DESTINATARIOS
            foreach (var item in request.Destinatarios)
            {
                correo.To.Add(item);
            }

            // ADJUNTOS OPCIONALES
            if (!request.EnvioBase64)
            {
                if (request.Adjuntos != null &&
               request.Adjuntos.Any())
                {
                    foreach (var archivo in request.Adjuntos)
                    {
                        if (File.Exists(archivo))
                        {
                            correo.Attachments.Add(new Attachment(archivo));
                        }
                    }
                }
            }
            else
            {
                byte[] archivoBytes = Convert.FromBase64String(request.AdjuntoBase64);

                MemoryStream stream = new MemoryStream(archivoBytes);
                string nombreDocumento = request.NombreDocumentoAdjunto == null ? "Documento" : request.NombreDocumentoAdjunto; 
                Attachment attachment = new Attachment(
                    stream,
                    nombreDocumento+".pdf",
                    "application/pdf");

                correo.Attachments.Add(attachment);
            }
           

            correo.Subject = request.Asunto;

            correo.Body = html;

            correo.IsBodyHtml = true;

            using SmtpClient smtp =
                new SmtpClient
                {
                    Host =
                        _configuration["SMTP:Host"],

                    Port =
                        int.Parse(
                            _configuration["SMTP:Puerto"]),

                    EnableSsl =
                        bool.Parse(
                            _configuration["SMTP:HabilitarSsl"]),

                    UseDefaultCredentials =
                        bool.Parse(
                            _configuration["SMTP:UsarCredencialesPorDefecto"]),

                    DeliveryMethod =
                        SmtpDeliveryMethod.Network,

                    Timeout =
                        int.Parse(
                            _configuration["SMTP:Timeout"])
                };

            // CONFIGURAR CREDENCIALES SOLO SI EXISTEN
            string usuario =
                _configuration["SMTP:Usuario"];

            string clave =
                _configuration["SMTP:Clave"];

            if (!string.IsNullOrWhiteSpace(usuario) &&
                !string.IsNullOrWhiteSpace(clave))
            {
                smtp.Credentials =
                    new NetworkCredential(
                        usuario,
                        clave);
            }

            try
            {
                await smtp.SendMailAsync(correo);

                await AuditoriaEmailPeticion(
                    request,
                    true,
                    "ENVIADO",
                    null);
            }
            catch (Exception ex)
            {
                await AuditoriaEmailPeticion(
                    request,
                    false,
                    "ERROR",
                    ex.ToString());

                throw;
            }
        }

        public async Task<bool> AuditoriaEmailPeticion(
        Email request,
        bool enviado,
        string estado,
        string error)
        {
            DynamicParameters parameters = new();

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_ASUNTO),
                request.Asunto);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_DESTINATARIOS),
                string.Join(";", request.Destinatarios));

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_MODULO),
                request.Modulo);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_RUTA_HTML),
                request.RutaHtml);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_ESTADO),
                estado);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_FUE_EXITOSO),
                enviado);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_MENSAJE_ERROR),
                error);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_USUARIO_CREACION),
                request.UsuarioCreacion);

            parameters.Add(
                HelpersEnums.GetEnumDescription(
                    EnumInsertarAuditoriaEmail.EMA_IP_ORIGEN),
                request.IpOrigen);

            return await GetAsyncFirst<bool>(HelperDBParameters.BuilderFunction(HelperDBParameters.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }

}

    //public async Task EnviarAsyncTest(Email request)
    //{
    //    // LEER HTML
    //    string html =
    //        await File.ReadAllTextAsync(
    //            request.RutaHtml);

    //    // LEER CSS
    //    string css =
    //        await File.ReadAllTextAsync(
    //            request.RutaCss);

    //    // INYECTAR CSS
    //    html = html.Replace(
    //        "{{STYLE}}",
    //        css);

    //    // LEER IMAGEN
    //    byte[] imageBytes =
    //        await File.ReadAllBytesAsync(
    //            request.RutaImagenEncabezado);

    //    // CONVERTIR BASE64
    //    string base64Image =
    //        Convert.ToBase64String(imageBytes);

    //    // FORMATO HTML
    //    string imageSrc =
    //        $"data:image/png;base64,{base64Image}";

    //    // REEMPLAZAR IMAGEN
    //    html = html.Replace(
    //        "{{ImagenEncabezado}}",
    //        imageSrc);

    //    // REEMPLAZAR VARIABLES
    //    foreach (var item in request.Parametros)
    //    {
    //        html = html.Replace(
    //            $"{{{{{item.Key}}}}}",
    //            item.Value);
    //    }

    //    using MailMessage correo =
    //        new MailMessage();

    //    correo.From = new MailAddress(
    //        "notificaciones@cajahonor.gov.co",
    //        "CajaHonor");

    //    foreach (var item in request.Destinatarios)
    //    {
    //        correo.To.Add(item);
    //    }
    //    //correo.Attachments.Add(new Attachment())
    //    correo.Subject = request.Asunto;
    //    correo.Body = html;
    //    correo.IsBodyHtml = true;

    //    using SmtpClient smtp =
    //        new SmtpClient
    //        {
    //            Host = "cpvmp-01v5",

    //            Port = 25,

    //            EnableSsl = false,

    //            UseDefaultCredentials = false,

    //            DeliveryMethod = SmtpDeliveryMethod.Network,

    //            Timeout = 30000
    //        };

    //    await smtp.SendMailAsync(correo);
    //}


public class SmtpSettings
{
    public string Host { get; set; }
    public int Puerto { get; set; }
    public bool HabilitarSsl { get; set; }
    public bool UsarCredencialesPorDefecto { get; set; }
    public string Usuario { get; set; }
    public string Clave { get; set; }
    public string NombreRemitente { get; set; }
    public int Timeout { get; set; }
}