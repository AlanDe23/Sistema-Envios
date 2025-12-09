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
                var cuerpo = new MailMessage(emailEmisor!, destino, asunto, mensaje) 
                {
                    IsBodyHtml = true

                };

                await stmpClient.SendMailAsync(cuerpo);

               
                        
            }


            private string GenerarCorreoBienvenida(string nombre)
            {
                return $@"
    <div style='font-family: Arial; padding: 20px; background: #f4f4f4;'>
        <div style='max-width: 500px; margin: auto; background: white; padding: 25px; border-radius: 10px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>

            <h2 style='color:#2C73D2; text-align:center;'>¡Bienvenido a Delivery Control!</h2>

            <p style='font-size: 15px; color:#333;'>
                Hola <b>{nombre}</b>,
            </p>

            <p style='font-size: 15px; color:#333;'>
                Tu cuenta ha sido creada exitosamente.  
                Ahora puedes gestionar deliverys, pedidos y Entregas desde nuestra plataforma.
            </p>

            <div style='text-align: center; margin-top: 20px;'>
                <a href='https://tusitio.com/login' 
                   style='background:#2C73D2; padding: 12px 25px; color:white; text-decoration:none;
                          font-weight:bold; border-radius:5px; display:inline-block;'>
                    Iniciar Sesión
                </a>
            </div>

            <hr style='margin-top: 30px;'>

            <p style='font-size: 12px; color:#777; text-align:center;'>
                Gracias por unirte a Delivery Control.
            </p>
        </div>
    </div>";
            }


            public async Task EnviarBienvenidaAsync(string destino, string nombre)
            {
                string asunto = "Bienvenido a Delivery Control";
                string mensaje = GenerarCorreoBienvenida(nombre);

                await EnviarCorreoAsync(destino, asunto, mensaje);
            }


            public async Task EnviarBienvenidaDeliveryAsync(string correo, string nombre, int idUsuario)
            {
                string asunto = "¡Bienvenido al Equipo de Delivery!";

                string mensaje = $@"
<div style='font-family: Arial; padding: 20px; background: #f4f4f4;'>
    <div style='max-width: 520px; margin: auto; background: white; padding: 25px; 
                border-radius: 12px; box-shadow: 0 0 12px rgba(0,0,0,0.12);'>

        <!-- Encabezado -->
        <h2 style='color:#2C73D2; text-align:center; margin-bottom: 10px;'>
            ¡Bienvenido al Equipo de Delivery!
        </h2>

        <p style='font-size: 15px; color:#333;'>
            Hola <b>{nombre}</b>,
        </p>

        <p style='font-size: 15px; color:#333;'>
            Tu cuenta como <b>Delivery</b> ha sido creada exitosamente.
        </p>

        <!-- Tarjeta con el ID -->
        <div style='background:#2C73D2; color:white; padding:15px; border-radius: 8px; 
                    text-align:center; margin: 18px 0; font-size:16px; font-weight:bold;'>
            Tu ID de Usuario: <span style='font-size: 20px;'>{idUsuario}</span>
        </div>

        <p style='font-size: 15px; color:#333;'>
            Este ID será utilizado por tu administrador para agregarte a su equipo de delivery.
            Asegúrate de guardarlo y compartirlo con tu supervisor.
        </p>

        <div style='text-align: center; margin-top: 22px;'>
            <a href='https://tusitio.com/login'
               style='background:#2C73D2; padding: 12px 25px; color:white; text-decoration:none;
                      font-weight:bold; border-radius:6px; display:inline-block;'>
                Acceder a tu Cuenta
            </a>
        </div>

        <hr style='margin-top: 30px;'>

        <p style='font-size: 12px; color:#777; text-align:center;'>
            Gracias por formar parte de Delivery Control.<br>
            ¡Estamos emocionados de tenerte en el equipo!
        </p>
    </div>
</div>";

                await EnviarCorreoAsync(correo, asunto, mensaje);
            }



        }
    }
}
