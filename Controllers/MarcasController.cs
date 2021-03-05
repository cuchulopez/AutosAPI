using _1eraAPI.Models;
using _1eraAPI.Models.DTOs;
using _1eraAPI.Repo.IRepo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Controllers
{
    [Authorize]
    [Route("api_autos/Marcas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIMarcas")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MarcasController : ControllerBase
    {
        private readonly IMarcaRepo _ctRepo;
        private readonly IMapper _mapper;
        public MarcasController(IMarcaRepo ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las Marcas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MarcasDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetMarcas()
        {
            var listaMarca = _ctRepo.GetMarcas();
            var listaMarcaDTO = new List<MarcasDTO>();

            foreach(var lista in listaMarca)
            {
                listaMarcaDTO.Add(_mapper.Map<MarcasDTO>(lista));

            }


            return Ok(listaMarcaDTO);
        }

        /// <summary>
        /// Obtener marca por Id
        /// </summary>
        /// <param name="marcaId">Id de la marca</param>
        /// <returns></returns>
        [HttpGet("{marcaId:int}", Name = "GetMarca")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(MarcasDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetMarca(int marcaId)
        {
            var itemMarca = _ctRepo.GetMarca(marcaId);
            if (itemMarca == null)
            {
                return NotFound();
            }
            var itemMarcaDTO = _mapper.Map<MarcasDTO>(itemMarca);
            return Ok(itemMarcaDTO);
        }

        /// <summary>
        /// Crear una marca
        /// </summary>
        /// <param name="marcasDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MarcasDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearMarca([FromBody] MarcasDTO marcasDTO)
        {
            if (marcasDTO == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExisteMarca(marcasDTO.Marca))
            {
                ModelState.AddModelError("", "La marca ya existe");
                return StatusCode(404,ModelState);
            }
            var marca = _mapper.Map<Marcas>(marcasDTO);

            if (!_ctRepo.CrearMarca(marca))
            {
                ModelState.AddModelError("", $"No se pudo guardar la marca {marca.Marca}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMarca", new { marcaId = marca.Id}, marca );
        }

        /// <summary>
        /// Actualizar una Marca existente
        /// </summary>
        /// <param name="marcaId"></param>
        /// <param name="marcaDTO"></param>
        /// <returns></returns>
        [HttpPatch("{marcaId:int}", Name = "ActualizarMarca")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarMarca(int marcaId, [FromBody] MarcasDTO marcaDTO)
        {
            if (marcaDTO == null || marcaId != marcaDTO.Id)
            {
                return BadRequest(ModelState);
            }
            var marca = _mapper.Map<Marcas>(marcaDTO);
            if (!_ctRepo.ActualizarMarca(marca))
            {
                ModelState.AddModelError("", $"No se pudo actualizar la marca {marca.Marca}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Borrar una marca
        /// </summary>
        /// <param name="marcaId"></param>
        /// <returns></returns>
        [HttpDelete("{marcaId:int}", Name = "BorrarMarca")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarMarca(int marcaId)
        {
            // var marca = _mapper.Map<Marcas>(marcaDTO);
            if (!_ctRepo.ExisteMarca(marcaId))
            {
                return NotFound();
            }

            var marca = _ctRepo.GetMarca(marcaId);

            if (!_ctRepo.BorrarMarca(marca))
            {
                ModelState.AddModelError("", $"No se pudo borrar la marca {marca.Marca}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
