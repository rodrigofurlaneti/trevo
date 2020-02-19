using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class BeneficioFuncionarioRepositorio : NHibRepository<BeneficioFuncionario>, IBeneficioFuncionarioRepositorio
    {
        public BeneficioFuncionarioRepositorio(NHibContext context)
            : base(context)
        {
        }

        public void DeleteOrphan()
        {
            var orphans = ListBy(x => x.Funcionario == null);

            foreach (var orphan in orphans)
            {
                Delete(orphan);
            }
        }
    }
}