using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Aplicacao
{

    public interface IChequeRecebidoAplicacao : IBaseAplicacao<ChequeRecebido>
    {
        IList<LancamentoCobrancaViewModel> BuscarLancamentosPorCliente(int idCliente, int idCheque = 0);
        IList<ChaveValorViewModel> ListaStatusCheque();
        IList<ChequeRecebidoViewModel> BuscarDadosSimples();
    }

    public class ChequeRecebidoAplicacao : BaseAplicacao<ChequeRecebido, IChequeRecebidoServico>, IChequeRecebidoAplicacao
    {
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IChequeRecebidoServico _chequeRecebidoServico;

        public ChequeRecebidoAplicacao(ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
                                        IChequeRecebidoServico chequeRecebidoServico)
        {
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _chequeRecebidoServico = chequeRecebidoServico;
        }

        public IList<LancamentoCobrancaViewModel> BuscarLancamentosPorCliente(int idCliente, int idCheque = 0)
        {
            var lamcamentos = _lancamentoCobrancaAplicacao.BuscarLancamentosPorCliente(idCliente);

            var chequesrecebidos = BuscarPor(x => x.Cliente.Id == idCliente);

            if (idCheque != 0)
            {
                chequesrecebidos = chequesrecebidos.Where(x => x.Id != idCheque).ToList();
            }

            //lancamentos recebidos
            var lancamentosRecebidos = new List<LancamentoCobranca>();
            foreach (var cheque in chequesrecebidos)
            {
                foreach (var lancamento in cheque.ListaLancamentoCobranca)
                {
                    lancamentosRecebidos.Add(lancamento.LancamentoCobranca);
                }
            }

            var result = lamcamentos.Where(p => !lancamentosRecebidos.Any(p2 => p2.Id == p.Id));

            return result != null && result.Any() ? result.Select(x => new LancamentoCobrancaViewModel(x)).ToList() : new List<LancamentoCobrancaViewModel>();
        }

        public IList<ChaveValorViewModel> ListaStatusCheque()
        {
            return Enum.GetValues(typeof(StatusCheque))
                .Cast<StatusCheque>()
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();
        }

        public IList<ChequeRecebidoViewModel> BuscarDadosSimples()
        {
            return _chequeRecebidoServico.BuscarDadosSimples()?.Select(x => new ChequeRecebidoViewModel(x))?.ToList();
        }
    }
}
