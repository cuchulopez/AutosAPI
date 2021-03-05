using _1eraAPI.Models;
using _1eraAPI.Models.DTOs;
using _1eraAPI.Repo.IRepo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Controllers
{
    [Authorize]
    [Route("api_autos/Modelos")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIModelos")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class ModelosController : ControllerBase
    {
        private readonly IModeloRepo _mdRepo; 
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ModelosController(IModeloRepo mdRepo, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _mdRepo = mdRepo;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }/// <summary>
        /// Obtener todos los modelos de autos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<ModelosDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetModelos()
        {
            var listaModelo = _mdRepo.GetModelos();
            var listaModeloDTO = new List<ModelosDTO>();

            foreach(var lista in listaModelo)
            {
                listaModeloDTO.Add(_mapper.Map<ModelosDTO>(lista));

            }


            return Ok(listaModeloDTO);
        }
        /// <summary>
        /// Obtener modelo por ID
        /// </summary>
        /// <param name="modeloId">Id del modelo de auto</param>
        /// <returns></returns>
        [HttpGet("{modeloId:int}", Name = "GetModelo")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MarcasDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetModelo(int modeloId)
        {
            var itemModelo = _mdRepo.GetModelo(modeloId);
            if (itemModelo == null)
            {
                return NotFound();
            }
            var itemModeloDTO = _mapper.Map<ModelosDTO>(itemModelo);
            return Ok(itemModeloDTO);
        }

        /// <summary>
        /// Obtener modelos por marca
        /// </summary>
        /// <param name="MarcaId"></param>
        /// <returns></returns>
        [HttpGet("GetModeloPorMarca/{MarcaId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ModelosDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetModeloPorMarca(int MarcaId)
        {
            var listaModelos = _mdRepo.GetModelosPorMarca(MarcaId);
            if (listaModelos== null)
            {
                return NotFound();
            }
            var itemModelo = new List<ModelosDTO>();
            foreach(var item in listaModelos)
            {
                itemModelo.Add(_mapper.Map<ModelosDTO>(item));
            }
            return Ok(itemModelo);
        }

        /// <summary>
        /// Buscar mediante una palabra
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet("Buscar")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ModelosDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _mdRepo.BuscarModelo(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
        }

        /// <summary>
        /// Crear un modelo
        /// </summary>
        /// <param name="ModelosDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ModelosDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearModelo([FromForm] ModelosCreateDTO ModelosDTO)
        {
            if (ModelosDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_mdRepo.ExisteModelo(ModelosDTO.Modelo))
            {
                ModelState.AddModelError("", "El modelo ya existe");
                return StatusCode(404,ModelState);
            }

            /* Subir Fotos */
            var archivo = ModelosDTO.Foto;
            string rutaPpal = _hostingEnvironment.WebRootPath;
            var archivos = HttpContext.Request.Form.Files;

            if (archivo.Length >0)
            {
                var nombreFoto = Guid.NewGuid().ToString();
                var subidas = Path.Combine(rutaPpal, @"img_modelos");
                var extension = Path.GetExtension(archivos[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
                {
                    archivos[0].CopyTo(fileStreams);
                }
                ModelosDTO.RutaImagen = @"\img_modelos\" + nombreFoto + extension;
            }

            var modelo = _mapper.Map<Modelos>(ModelosDTO);

            if (!_mdRepo.CrearModelo(modelo))
            {
                ModelState.AddModelError("", $"No se pudo guardar el modelo {modelo.Modelo}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetModelo", new { modeloId = modelo.Id}, modelo );
        }


        /// <summary>
        /// Modificar un modelo
        /// </summary>
        /// <param name="modeloId">Id del modelo de auto</param>
        /// <param name="modeloDTO">Campos para modificar</param>
        /// <returns></returns>
        [HttpPatch("{modeloId:int}", Name = "ActualizarModelo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarModelo(int modeloId, [FromBody] ModelosDTO modeloDTO)
        {
            if (modeloDTO == null || modeloId != modeloDTO.Id)
            {
                return BadRequest(ModelState);
            }
            var modelo = _mapper.Map<Modelos>(modeloDTO);
            if (!_mdRepo.ActualizarModelo(modelo))
            {
                ModelState.AddModelError("", $"No se pudo actualizar el modelo {modelo.Modelo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Eliminar un modelo
        /// </summary>
        /// <param name="modeloId"></param>
        /// <returns></returns>
        [HttpDelete("{modeloId:int}", Name = "BorrarModelo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarModelo(int modeloId)
        {
            // var marca = _mapper.Map<Modelos>(marcaDTO);
            if (!_mdRepo.ExisteModelo(modeloId))
            {
                return NotFound();
            }

            var modelo = _mdRepo.GetModelo(modeloId);

            if (!_mdRepo.BorrarModelo(modelo))
            {
                ModelState.AddModelError("", $"No se pudo borrar el modelo {modelo.Modelo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
