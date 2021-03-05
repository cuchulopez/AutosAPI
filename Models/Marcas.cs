using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models
{
    public class Marcas
    {
        [Key]
        public int Id { get; set; }
        public string Marca { get; set; }

    }
}
