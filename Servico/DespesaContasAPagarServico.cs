using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IDespesaContasAPagarServico : IBaseServico<DespesaContasAPagar>
    {
        void RemoverPorSelecaoDespesaId(int id);
        void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar);
    }

    public class DespesaContasAPagarServico : BaseServico<DespesaContasAPagar, IDespesaContasAPagarRepositorio>,IDespesaContasAPagarServico
    {
        private readonly IDespesaContasAPagarRepositorio _DespesaContasAPagarRepositorio;

        public DespesaContasAPagarServico(IDespesaContasAPagarRepositorio despesaContasAPagarRepositorio)
        {
            _DespesaContasAPagarRepositorio = despesaContasAPagarRepositorio;
        }

        public void RemoverPorContasAPagar(List<ContasAPagar> contasAPagar)
        {
            _DespesaContasAPagarRepositorio.RemoverPorContasAPagar(contasAPagar);
        }

        public void RemoverPorSelecaoDespesaId(int id)
        {
            _DespesaContasAPagarRepositorio.RemoverPorSelecaoDespesaId(id);
        }
    }
}

