using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface INegociacaoSeloDescontoNotificacaoAplicacao : IBaseAplicacao<NegociacaoSeloDescontoNotificacao>
    {
        List<NegociacaoSeloDescontoNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(NegociacaoSeloDescontoNotificacao model);
        void Reprovar(NegociacaoSeloDescontoNotificacao model);
    }

    public class NegociacaoSeloDescontoNotificacaoAplicacao : BaseAplicacao<NegociacaoSeloDescontoNotificacao, INegociacaoSeloDescontoNotificacaoServico>, INegociacaoSeloDescontoNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly IDescontoAplicacao _NegociacaoSeloDescontoAplicacao;

        public NegociacaoSeloDescontoNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, IDescontoAplicacao NegociacaoSeloDescontoAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _NegociacaoSeloDescontoAplicacao = NegociacaoSeloDescontoAplicacao;
        }

        public List<NegociacaoSeloDescontoNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        {
            try
            {
                ParametroNotificacao ParametroNegociacaoSeloDescontoNotificacao = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Entidade == Entidades.Desconto
                                                                 && x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id)).FirstOrDefault();
                List<Desconto> ListaNegociacaoSeloDesconto = new List<Desconto>();
                if (ParametroNegociacaoSeloDescontoNotificacao != null)
                    ListaNegociacaoSeloDesconto = _NegociacaoSeloDescontoAplicacao.BuscarPor(x => x.Notificacoes.Any(m => m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();
                List<NegociacaoSeloDescontoNotificacao> ListaNegociacaoSeloDescontoNotificacao = new List<NegociacaoSeloDescontoNotificacao>();
                foreach (var item in ListaNegociacaoSeloDesconto)
                {
                    foreach (var item2 in item.Notificacoes)
                    {
                        var NegociacaoSeloDescontoNotif = new NegociacaoSeloDescontoNotificacao()
                        {
                            //NegociacaoSeloDesconto = item, //TODO:MARCO
                            Notificacao = item2.Notificacao
                        };
                        ListaNegociacaoSeloDescontoNotificacao.Add(NegociacaoSeloDescontoNotif);
                    }
                }
                return ListaNegociacaoSeloDescontoNotificacao;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
            }
        }

        public void Aprovar(NegociacaoSeloDescontoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(NegociacaoSeloDescontoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
