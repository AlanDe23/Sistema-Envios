using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace Envios.Application.Service
{
    using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    namespace Envios.Application.Services
    {
        public class EmailService
        {

            private readonly IConfiguration _configuration;


            public EmailService(IConfiguration configuration)
            {
                this._configuration = configuration;
            }



            public async Task EnviarCorreoAsync(string destino, string asunto, string mensaje)
            {
                var emailEmisor = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:Email");
                var password = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:Password");
                var host = _configuration.GetValue<string>("CONFIGURACIONES_EMAIL:Host");   
                var puerto = _configuration.GetValue<int>("CONFIGURACIONES_EMAIL:Puerto");

                var stmpClient = new SmtpClient(host, puerto);
                stmpClient.EnableSsl = true;
                stmpClient.UseDefaultCredentials = false;


                stmpClient.Credentials = new NetworkCredential(emailEmisor, password);  
                var cuerpo = new MailMessage(emailEmisor! ,destino , asunto, mensaje);
                await stmpClient.SendMailAsync(cuerpo);  



            }
        }
    }
}
