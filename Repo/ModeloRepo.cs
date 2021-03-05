using _1eraAPI.Data;
using _1eraAPI.Models;
using _1eraAPI.Repo.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo
{
    public class ModeloRepo : IModeloRepo
    {
        private readonly AppDbContext _db;
        public ModeloRepo(AppDbContext db)
        {
            _db = db;

        }
        public bool ActualizarModelo(Modelos Modelo)
        {
            _db.Modelos.Update(Modelo);
            return Guardar();
        }

        public bool BorrarModelo(Modelos Modelo)
        {
            _db.Modelos.Remove(Modelo);
            return Guardar();
        }

        public IEnumerable<Modelos> BuscarModelo(string Nombre)
        {
            IQueryable<Modelos> query = _db.Modelos;

            if (!string.IsNullOrEmpty(Nombre))
            {
                query = query.Where(e => e.Modelo.Contains(Nombre) || e.Descripcion.Contains(Nombre));
            }
            return query.ToList();
        }

        public bool CrearModelo(Modelos Modelo)
        {
            _db.Modelos.Add(Modelo);
            return Guardar();
        }

        public bool ExisteModelo(string Modelo)
        {
            bool valor = _db.Modelos.Any(c => c.Modelo.ToLower().Trim() == Modelo.ToLower().Trim());
            return valor;
        }

        public bool ExisteModelo(int Id)
        {
            return _db.Modelos.Any(c => c.Id == Id);
        }

        public Modelos GetModelo(int ModeloId)
        {
            return _db.Modelos.FirstOrDefault(m => m.Id == ModeloId);
        }

        public ICollection<Modelos> GetModelos()
        {
            return _db.Modelos.OrderBy(c => c.Modelo).ToList();

        }

        public ICollection<Modelos> GetModelosPorMarca(int MarId)
        {
            return _db.Modelos.Include(ma => ma.Marcas).Where(ma => ma.MarcaId == MarId).ToList();
        }

        public bool Guardar()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
