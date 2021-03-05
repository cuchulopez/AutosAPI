using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models.DTOs
{
    public class UsuarioAuthLoginDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
    }
}
