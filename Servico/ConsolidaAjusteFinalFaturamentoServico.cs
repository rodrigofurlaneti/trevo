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
    public interface IConsolidaAjusteFinalFaturamentoServico : IBaseServico<ConsolidaAjusteFinalFaturamento>
    {

    }
    public class ConsolidaAjusteFinalFaturamentoServico : BaseServico<ConsolidaAjusteFinalFaturamento, IConsolidaAjusteFinalFaturamentoRepositorio>, IConsolidaAjusteFinalFaturamentoServico
    {

    }
}
