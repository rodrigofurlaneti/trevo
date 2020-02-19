using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContaCorrenteClienteRepositorio : NHibRepository<ContaCorrenteCliente>, IContaCorrenteClienteRepositorio
    {
        public ContaCorrenteClienteRepositorio(NHibContext context)
            : base(context)
        {
        }

        public void DeleteOrphan()
        {
            var orphans = ListBy(x => x.Cliente == null);

            foreach (var orphan in orphans)
            {
                Delete(orphan);
            }
        }
    }
}