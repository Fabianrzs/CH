using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Threading.Tasks;
using System;

namespace GA2.Apis.GA2.Endpoints.Integraciones
{
    [ApiController]
    [Route("api/test/smtp")]
    public class SmtpTestController : ControllerBase
    {
        private readonly ILogger<SmtpTestController> _logger;

        public SmtpTestController(ILogger<SmtpTestController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                using var smtpClient = new SmtpClient("cpvmp-01v5", 25)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = false
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress("notificaciones@cajahonor.gov.co"),
                    Subject = "Prueba SMTP",
                    Body = "Mensaje enviado con fines de validación técnica del servicio de correo electrónico. Esta comunicación hace parte de las pruebas de integración realizadas por Grupo Kriterion y no requiere atención ni respuesta.",
                    IsBodyHtml = false
                };

                var recipients = new[]
                {
                    "desarrollok18@grupokriterion.com.co",
                    "desarrollok30@grupokriterion.com.co",
                    "pruebatres.kriterion@cajahonor.gov.co",
                    "pruebasdos.kriterion@cajahonor.onmicrosoft.com",
                    "pruebascuatro.kriterion@cajahonor.gov.co",
                    "pruebasuno.kriterion@cajahonor.gov.co",
                    "ingenieria.kriterion@cajahonor.gov.co",
                    "angel.bernal@cajahonor.gov.co",
                    "dianam.bautista@cajahonor.gov.co",
                    "luis.rivera@cajahonor.gov.co",
                    "myriam.toro@cajahonor.gov.co"
                };

                foreach (var recipient in recipients)
                {
                    mailMessage.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mailMessage);

                return Ok(new
                {
                    success = true,
                    message = "Correo enviado correctamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo SMTP");

                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    detail = ex.InnerException?.Message
                });
            }
        }
    }
}
