using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IFornecedorServico : IBaseServico<Fornecedor>
    {
    }
    
    public class FornecedorServico : BaseServico<Fornecedor, IFornecedorRepositorio>, IFornecedorServico
    {

    }
}
