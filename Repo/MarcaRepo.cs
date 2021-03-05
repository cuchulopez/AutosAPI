using _1eraAPI.Data;
using _1eraAPI.Models;
using _1eraAPI.Repo.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo
{
    public class MarcaRepo : IMarcaRepo
    {
        private readonly AppDbContext _db;
        public MarcaRepo(AppDbContext db)
        {
            _db = db;

        }
        public bool ActualizarMarca(Marcas marcas)
        {
            _db.Marcas.Update(marcas);
            // _db.Marcas.Update(marca);
            return Guardar();
        }

        public bool BorrarMarca(Marcas marcas)
        {
            _db.Marcas.Remove(marcas);
            return Guardar();
        }

        public bool CrearMarca(Marcas marcas)
        {
            _db.Marcas.Add(marcas);
            return Guardar();
        }

        public bool ExisteMarca(string marca)
        {
            bool valor = _db.Marcas.Any(m => m.Marca.ToLower().Trim() == marca.ToLower().Trim());
            return valor;
        }

        public bool ExisteMarca(int Id)
        {
            return _db.Marcas.Any(m => m.Id == Id);
        }
        public Marcas GetMarca(int MarcaId)
        {
            return _db.Marcas.FirstOrDefault(m => m.Id == MarcaId);
        }
        public ICollection<Marcas> GetMarcas()
        {
            return _db.Marcas.OrderBy(m => m.Marca).ToList();
        }


        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
