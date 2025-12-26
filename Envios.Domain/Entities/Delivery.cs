using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Envios.Domain.Entities
{
    public class Delivery
    {

        [Key]
        public int IdDelivery { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }

        public string Nombre { get; set; }
        public bool Estado { get; set; } = true;

        public decimal BalanceAcumulado { get; set; } = 0;

        public bool Activo { get; set; } = false;

        // Relación con Usuario
        public Usuario Usuario { get; set; }
        [JsonIgnore]
        public BalanceAdmin  Balance { get; set; }



        public int IdSucursal { get; set; }
        public Sucursal Sucursal { get; set; }
        public bool IsDeleted { get; set; }
    }
}



   
