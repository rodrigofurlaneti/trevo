using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IParametroEquipeNotificacaoAplicacao : IBaseAplicacao<ParametroEquipeNotificacao>
    {
        List<ParametroEquipeNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(ParametroEquipeNotificacao model);
        void Reprovar(ParametroEquipeNotificacao model);
    }

    public class ParametroEquipeNotificacaoAplicacao : BaseAplicacao<ParametroEquipeNotificacao, IParametroEquipeNotificacaoServico>, IParametroEquipeNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly IParametroEquipeAplicacao _ParametroEquipeAplicacao;

        public ParametroEquipeNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, IParametroEquipeAplicacao ParametroEquipeAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _ParametroEquipeAplicacao = ParametroEquipeAplicacao;
        }

        public List<ParametroEquipeNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        {
            try
            {
                ParametroNotificacao ParametroParametroEquipeNotificacao = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Entidade == Entidades.ParametroEquipe
                                                                 && x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id)).FirstOrDefault();
                List<ParametroEquipe> ListaParametroEquipe = new List<ParametroEquipe>();
                if (ParametroParametroEquipeNotificacao != null)
                    ListaParametroEquipe = _ParametroEquipeAplicacao.BuscarPor(x => x.Notificacoes.Any(m => m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();
                List<ParametroEquipeNotificacao> ListaParametroEquipeNotificacao = new List<ParametroEquipeNotificacao>();
                foreach (var item in ListaParametroEquipe)
                {
                    foreach (var item2 in item.Notificacoes)
                    {
                        var ParametroEquipeNotif = new ParametroEquipeNotificacao()
                        {
                            ParametroEquipe = item,
                            Notificacao = item2.Notificacao
                        };
                        ListaParametroEquipeNotificacao.Add(ParametroEquipeNotif);
                    }
                }
                return ListaParametroEquipeNotificacao;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
            }
        }

        public void Aprovar(ParametroEquipeNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(ParametroEquipeNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
