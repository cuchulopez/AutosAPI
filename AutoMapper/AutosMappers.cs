using _1eraAPI.Models;
using _1eraAPI.Models.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.AutoMapper
{
    public class AutosMappers : Profile
    {
        public AutosMappers()
        {
            CreateMap<Marcas, MarcasDTO>().ReverseMap();
            CreateMap<Modelos, ModelosDTO>().ReverseMap();
            CreateMap<Modelos, ModelosCreateDTO>().ReverseMap();
            CreateMap<Modelos, ModelosUpdateDTO>().ReverseMap();
            CreateMap<Usuarios, UsuariosDTO>().ReverseMap();
        }
    }
}
