using _1eraAPI.Models;
using _1eraAPI.Models.DTOs;
using _1eraAPI.Repo.IRepo;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace _1eraAPI.Controllers
{
    [Authorize]
    [Route("api_autos/Usuarios")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIUsuarios")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UsuariosController(IUsuarioRepo userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        /// <summary>
        /// Obtener todos los usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<UsuariosDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _userRepo.GetUsuarios();
            var listaUsuariosDTO = new List<UsuariosDTO>();

            foreach(var lista in listaUsuarios)
            {
                listaUsuariosDTO.Add(_mapper.Map<UsuariosDTO>(lista));

            }


            return Ok(listaUsuariosDTO);
        }

        /// <summary>
        /// Obtener usuario por Id.
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        [HttpGet("{usuarioId:int}", Name = "GetUsuario")]
        [ProducesResponseType(200, Type = typeof(UsuariosDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itemUsuario = _userRepo.GetUsuario(usuarioId);
            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDTO = _mapper.Map<UsuariosDTO>(itemUsuario);
            return Ok(itemUsuarioDTO);
        }

        /// <summary>
        /// Registrar un usuario
        /// </summary>
        /// <param name="usuariosAuthDTO">Campos requeridos para registrar un usuario</param>
        /// <returns></returns>
        [HttpPost("Registro")]
        [AllowAnonymous]
        [ProducesResponseType(201, Type = typeof(UsuariosDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Registro(UsuariosAuthDTO usuariosAuthDTO)
        {
            usuariosAuthDTO.Usuario = usuariosAuthDTO.Usuario.ToLower();
            if ( _userRepo.ExisteUsuario(usuariosAuthDTO.Usuario))
            {
                return BadRequest("El usuario ya existe.");
            }

            var usuarioACrear = new Usuarios
            {
                Usuario = usuariosAuthDTO.Usuario
            };

            var usuarioCreado = _userRepo.Registro(usuarioACrear, usuariosAuthDTO.Password);
            return Ok(usuarioCreado);
        }
        /// <summary>
        /// Login de usuario
        /// </summary>
        /// <param name="usuarioAuthLoginDTO">Campos requeridos para login</param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(202, Type = typeof(UsuariosDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(UsuarioAuthLoginDTO usuarioAuthLoginDTO)
        {
            var usuarioDesdeRepo = _userRepo.Login(usuarioAuthLoginDTO.Usuario, usuarioAuthLoginDTO.Password);
            if (usuarioDesdeRepo == null)
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDesdeRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioDesdeRepo.Usuario.ToString())
            };

            // Creacion del Token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credenciales
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new
                {
                    token = tokenHandler.WriteToken(token)
                }
            );


        }

    }
}
