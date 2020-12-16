using Digital.curso.api.Models.Usuarios;

namespace Digital.curso.api.Configurations
{
    public interface IAuthenticationService
    {
        string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput);
    }
}
