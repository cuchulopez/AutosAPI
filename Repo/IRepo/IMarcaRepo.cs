using _1eraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo.IRepo
{
    public interface IMarcaRepo
    {
        ICollection<Marcas> GetMarcas();
        Marcas GetMarca(int MarcaId);
        bool ExisteMarca(string Marca);
        bool ExisteMarca(int Id);
        bool CrearMarca(Marcas marcas);
        bool ActualizarMarca(Marcas marcas);
        bool BorrarMarca(Marcas marcas);
        bool Guardar();
    }
}
