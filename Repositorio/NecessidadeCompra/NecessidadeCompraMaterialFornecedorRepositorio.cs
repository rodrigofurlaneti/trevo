using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class NecessidadeCompraMaterialFornecedorRepositorio : NHibRepository<NecessidadeCompraMaterialFornecedor>, INecessidadeCompraMaterialFornecedorRepositorio
    {
        public NecessidadeCompraMaterialFornecedorRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}