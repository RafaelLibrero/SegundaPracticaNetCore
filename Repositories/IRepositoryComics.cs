using SegundaPracticaNetCore.Models;

namespace SegundaPracticaNetCore.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        Comic FindComic(int id);
        void InsertarComicProcedure(string nombre, string imagen, string descripcion);
        void InsertarComicLambda(string nombre, string imagen, string descripcion);
        void DeleteComic(int id);
    }
}
