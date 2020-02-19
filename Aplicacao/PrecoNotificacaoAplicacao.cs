using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IPrecoNotificacaoAplicacao : IBaseAplicacao<PrecoNotificacao>
    {
        //List<PrecoNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(PrecoNotificacao model);
        void Reprovar(PrecoNotificacao model);
    }

    public class PrecoNotificacaoAplicacao : BaseAplicacao<PrecoNotificacao, IPrecoNotificacaoServico>, IPrecoNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly IPrecoAplicacao _precoAplicacao;

        public PrecoNotificacaoAplicacao(IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, IPrecoAplicacao precoAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _precoAplicacao = precoAplicacao;
        }

        //public List<PrecoNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        //{
        //    try
        //    {
        //        ParametroNotificacao ParametroPrecoNotificacao = _parametroNotificacaoAplicacao.BuscarPor(x => x.TipoNotificacao.Entidade == Entidades.TabelaPreco
        //                                                         && x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id)).FirstOrDefault();
        //        List<Preco> ListaPreco = new List<Preco>();
        //        if (ParametroPrecoNotificacao != null)
        //            ListaPreco = _precoAplicacao.BuscarPor(x=>x.Notificacoes.Any(m=>m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();
        //        List<PrecoNotificacao> ListaPrecoNotificacao = new List<PrecoNotificacao>();
        //        foreach (var item in ListaPreco)
        //        {
        //            foreach (var item2 in item.Notificacoes)
        //            {
        //                var precoNotif = new PrecoNotificacao()
        //                {
        //                    Preco = item,
        //                    Notificacao = item2.Notificacao
        //                };
        //                ListaPrecoNotificacao.Add(precoNotif);
        //            }
        //        }
        //        return ListaPrecoNotificacao;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
        //    }
        //}

        public void Aprovar(PrecoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(PrecoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}
