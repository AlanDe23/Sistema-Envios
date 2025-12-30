using Envios.Application.DTOs.Carnet;
using Envios.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Envios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {

        public readonly SuscripcionService _suscripcionService;

        public WebhooksController(SuscripcionService suscripcionService)
        {
            _suscripcionService = suscripcionService;
        }


        [HttpPost("webhooks/carnet")]
        public async Task<IActionResult> Recibir([FromBody] CarnetWebhookDto dto)
        {
            await _suscripcionService.ProcesarWebhookCarnet(dto);
            return Ok((new { message = "Webhook procesado correctamente" }));
        }

    }
}

