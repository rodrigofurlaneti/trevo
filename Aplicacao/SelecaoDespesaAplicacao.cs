using Aplicacao.Base;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao
{
    public interface ISelecaoDespesaAplicacao : IBaseAplicacao<SelecaoDespesa>
    {
        void RemoverPorContaAPagarId(int id);
    }
    public class SelecaoDespesaAplicacao : BaseAplicacao<SelecaoDespesa, ISelecaoDespesaServico>, ISelecaoDespesaAplicacao
    {
        public void RemoverPorContaAPagarId(int id)
        {
           
        }
    }
}
