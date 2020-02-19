using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface ITabelaPrecoMensalistaNotificacaoAplicacao : IBaseAplicacao<TabelaPrecoMensalistaNotificacao>
    {
        List<TabelaPrecoMensalistaNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(TabelaPrecoMensalistaNotificacao model);
        void Reprovar(TabelaPrecoMensalistaNotificacao model);
    }

    public class TabelaPrecoMensalistaNotificacaoAplicacao : BaseAplicacao<TabelaPrecoMensalistaNotificacao, ITabelaPrecoMensalistaNotificacaoServico>, ITabelaPrecoMensalistaNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly ITabelaPrecoMensalistaAplicacao _TabelaPrecoMensalistaAplicacao;

        public TabelaPrecoMensalistaNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, ITabelaPrecoMensalistaAplicacao TabelaPrecoMensalistaAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _TabelaPrecoMensalistaAplicacao = TabelaPrecoMensalistaAplicacao;
        }

        public List<TabelaPrecoMensalistaNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        {
            try
            {
                ParametroNotificacao ParametroTabelaPrecoMensalistaNotificacao = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Entidade == Entidades.TabelaPrecoMensalista
                                                                 && x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id)).FirstOrDefault();
                List<TabelaPrecoMensalista> ListaTabelaPrecoMensalista = new List<TabelaPrecoMensalista>();
                if (ParametroTabelaPrecoMensalistaNotificacao != null)
                    ListaTabelaPrecoMensalista = _TabelaPrecoMensalistaAplicacao.BuscarPor(x => x.Notificacoes.Any(m => m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();
                List<TabelaPrecoMensalistaNotificacao> ListaTabelaPrecoMensalistaNotificacao = new List<TabelaPrecoMensalistaNotificacao>();
                foreach (var item in ListaTabelaPrecoMensalista)
                {
                    foreach (var item2 in item.Notificacoes)
                    {
                        var TabelaPrecoMensalistaNotif = new TabelaPrecoMensalistaNotificacao()
                        {
                            TabelaPrecoMensalista = item,
                            Notificacao = item2.Notificacao
                        };
                        ListaTabelaPrecoMensalistaNotificacao.Add(TabelaPrecoMensalistaNotif);
                    }
                }
                return ListaTabelaPrecoMensalistaNotificacao;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
            }
        }

        public void Aprovar(TabelaPrecoMensalistaNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(TabelaPrecoMensalistaNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
