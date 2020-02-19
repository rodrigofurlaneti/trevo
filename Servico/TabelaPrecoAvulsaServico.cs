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

    public interface ITabelaPrecoAvulsaServico : IBaseServico<TabelaPrecoAvulsa>
    {
    }

    public class TabelaPrecoAvulsaServico : BaseServico<TabelaPrecoAvulsa, ITabelaPrecoAvulsaRepositorio>, ITabelaPrecoAvulsaServico
    {

    }
}
