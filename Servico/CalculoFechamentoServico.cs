using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface ICalculoFechamentoServico : IBaseServico<CalculoFechamento>
    {

    }
    public class CalculoFechamentoServico : BaseServico<CalculoFechamento, ICalculoFechamentoRepositorio>, ICalculoFechamentoServico
    {
    }
}
