using _1eraAPI.Data;
using _1eraAPI.Models;
using _1eraAPI.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly AppDbContext _db;
        public UsuarioRepo(AppDbContext db)
        {
            _db = db;

        }
        public bool ExisteUsuario(string usuario)
        {
            if (_db.Usuarios.Any(x => x.Usuario == usuario))
            {
                return true;
            }
            return false;
        }
        
        public Usuarios GetUsuario(int UsuarioId)
        {
            return _db.Usuarios.FirstOrDefault(c => c.Id == UsuarioId);
        }

        public ICollection<Usuarios> GetUsuarios()
        {
            return _db.Usuarios.OrderBy(c => c.Usuario).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public Usuarios Login(string usuario, string password)
        {
            var user = _db.Usuarios.FirstOrDefault(x => x.Usuario == usuario);
            if (user == null)
            {
                return null;
            }
            if (!VerificarPasswordHash(password, user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public Usuarios Registro(Usuarios usuario, string password)
        {
            byte[] passwordHash, passwordSalt;

            CrearPasswordHash(password, out passwordHash, out passwordSalt);
            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _db.Usuarios.Add(usuario);
            Guardar();
            return usuario;
        }



        private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var pass = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = pass.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i< hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var pass = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = pass.Key;
                passwordHash = pass.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }


    }
}
