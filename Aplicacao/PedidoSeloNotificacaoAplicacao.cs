using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;
namespace Aplicacao
{
    public interface IPedidoSeloNotificacaoAplicacao : IBaseAplicacao<PedidoSeloNotificacao>
    {
        List<PedidoSeloNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(PedidoSeloNotificacao model);
        void Reprovar(PedidoSeloNotificacao model);
    }

    public class PedidoSeloNotificacaoAplicacao : BaseAplicacao<PedidoSeloNotificacao, IPedidoSeloNotificacaoServico>, IPedidoSeloNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly IPedidoSeloAplicacao _PedidoSeloAplicacao;

        public PedidoSeloNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, IPedidoSeloAplicacao PedidoSeloAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _PedidoSeloAplicacao = PedidoSeloAplicacao;
        }

        public List<PedidoSeloNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        {
            try
            {
                ParametroNotificacao ParametroPedidoSeloNotificacao = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Entidade == Entidades.PedidoSelo
                                                                 && x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id)).FirstOrDefault();
                List<PedidoSelo> ListaPedidoSelo = new List<PedidoSelo>();
                if (ParametroPedidoSeloNotificacao != null)
                    ListaPedidoSelo = _PedidoSeloAplicacao.BuscarPor(x => x.Notificacoes.Any(m => m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();
                List<PedidoSeloNotificacao> ListaPedidoSeloNotificacao = new List<PedidoSeloNotificacao>();
                foreach (var item in ListaPedidoSelo)
                {
                    foreach (var item2 in item.Notificacoes)
                    {
                        var PedidoSeloNotif = new PedidoSeloNotificacao()
                        {
                            PedidoSelo = item,
                            Notificacao = item2.Notificacao
                        };
                        ListaPedidoSeloNotificacao.Add(PedidoSeloNotif);
                    }
                }
                return ListaPedidoSeloNotificacao;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
            }
        }

        public void Aprovar(PedidoSeloNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(PedidoSeloNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
