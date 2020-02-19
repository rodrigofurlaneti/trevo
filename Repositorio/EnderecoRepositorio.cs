using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EnderecoRepositorio : NHibRepository<Endereco>, IEnderecoRepositorio
    {
        public EnderecoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}