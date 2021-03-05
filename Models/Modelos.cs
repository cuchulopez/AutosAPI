using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models
{
    public class Modelos
    {
        [Key]
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string RutaImagen { get; set; }
        public string Descripcion { get; set; }

        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public Marcas Marcas { get; set; }

    }
}
