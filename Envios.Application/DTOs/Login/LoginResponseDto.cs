

namespace Envios.Application.DTOs.Login
{
    public class LoginResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }

        public int? IdDelivery { get; set; }
        public List<SucursalLoginDto> Sucursales { get; set; }
    }
}
