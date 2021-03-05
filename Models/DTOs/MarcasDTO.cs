using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models.DTOs
{
    public class MarcasDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La marca es obligatoria.")]
        public string Marca { get; set; }

    }
}
