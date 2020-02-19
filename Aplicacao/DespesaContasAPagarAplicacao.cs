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
    public interface IDespesaContasAPagarAplicacao : IBaseAplicacao<DespesaContasAPagar>
    {
        void RemoverPorSelecaoDespesaId(int id);
        void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar);
    }
    public class DespesaContasAPagarAplicacao : BaseAplicacao<DespesaContasAPagar, IDespesaContasAPagarServico>, IDespesaContasAPagarAplicacao
    {
        private readonly IDespesaContasAPagarServico _DespesaContasAPagarServico;
        public DespesaContasAPagarAplicacao(IDespesaContasAPagarServico despesaContasAPagarServico)
        {
            _DespesaContasAPagarServico = despesaContasAPagarServico;
        }

        public void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar)
        {
            _DespesaContasAPagarServico.RemoverPorContasAPagar(contasAPagar);
        }

        public void RemoverPorSelecaoDespesaId(int id)
        {
            _DespesaContasAPagarServico.RemoverPorSelecaoDespesaId((id));
        }
    }
}
