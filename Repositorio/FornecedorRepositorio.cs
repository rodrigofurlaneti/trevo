using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class FornecedorRepositorio : NHibRepository<Fornecedor>, IFornecedorRepositorio
    {
        public FornecedorRepositorio(NHibContext context) : base(context)
        {
        }
    }
}
