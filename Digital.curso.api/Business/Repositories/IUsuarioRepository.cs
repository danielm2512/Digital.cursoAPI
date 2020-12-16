using Digital.curso.api.Business.Entities;

namespace Digital.curso.api.Business.Repositories
{
    public interface IUsuarioRepository
    {
        void Adicionar(Usuario usuario);
        void Commit();
    }
}
