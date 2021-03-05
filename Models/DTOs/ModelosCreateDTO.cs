﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Models.DTOs
{
    public class ModelosCreateDTO
    {

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        public string Modelo { get; set; }
        public string RutaImagen { get; set; }
        public IFormFile Foto { get; set; }
	
        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        public string Descripcion { get; set; }

        public int MarcaId { get; set; }
    }
}
