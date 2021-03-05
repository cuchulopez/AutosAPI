using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models.DTOs
{
    public class UsuariosDTO
    {
        public string Usuario { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
