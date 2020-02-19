using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepository<Usuario>
    {
        Usuario ValidarLogin(string cpf, string senha);
        Usuario RetornarPorCPF(string cpf);
    }
}
