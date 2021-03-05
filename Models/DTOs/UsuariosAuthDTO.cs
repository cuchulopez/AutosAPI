using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models.DTOs
{
    public class UsuariosAuthDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(10,MinimumLength = 4, ErrorMessage = "Longitud inválida ( Min: 4; Máx:10).") ]
        public string Password { get; set; }
    }
}
