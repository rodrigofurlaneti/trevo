using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoCompraCotacaoMaterialFornecedorRepositorio : NHibRepository<PedidoCompraCotacaoMaterialFornecedor>, IPedidoCompraCotacaoMaterialFornecedorRepositorio
    {
        public PedidoCompraCotacaoMaterialFornecedorRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}