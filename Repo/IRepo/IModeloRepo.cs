using _1eraAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1eraAPI.Repo.IRepo
{
    public interface IModeloRepo
    {
        ICollection<Modelos> GetModelos();
        ICollection<Modelos> GetModelosPorMarca(int MarcaId);
        Modelos GetModelo(int ModeloId);
        bool ExisteModelo(string Modelo);
        IEnumerable<Modelos> BuscarModelo(string Nombre);
        bool ExisteModelo(int Id);
        bool CrearModelo(Modelos Modelo);
        bool ActualizarModelo(Modelos Modelo);
        bool BorrarModelo(Modelos Modelo);
        bool Guardar();
    }
}
