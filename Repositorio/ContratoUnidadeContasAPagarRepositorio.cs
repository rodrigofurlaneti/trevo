using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContratoUnidadeContasAPagarRepositorio : NHibRepository<ContratoUnidadeContasAPagar>, IContratoUnidadeContasAPagarRepositorio
    {
        public ContratoUnidadeContasAPagarRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}