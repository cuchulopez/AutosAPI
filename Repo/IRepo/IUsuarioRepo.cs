using _1eraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo.IRepo
{
    public interface IUsuarioRepo
    {
        ICollection<Usuarios> GetUsuarios();
        Usuarios GetUsuario(int UsuarioId);
        bool ExisteUsuario(string usuario);
        Usuarios Registro(Usuarios Usuario, string Password);
        Usuarios Login(string Usuario, string Password);
        bool Guardar();
    }
}
