using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContatoRepositorio : NHibRepository<Contato>, IContatoRepositorio
    {
        public ContatoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}