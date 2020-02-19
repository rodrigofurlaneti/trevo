using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface ITabelaPrecoAvulsoNotificacaoAplicacao : IBaseAplicacao<TabelaPrecoAvulsoNotificacao>
    {
        List<TabelaPrecoAvulsoNotificacao> RetornaNotificacoesPendentes(Usuario usuario);
        void Aprovar(TabelaPrecoAvulsoNotificacao model);
        void Reprovar(TabelaPrecoAvulsoNotificacao model);
    }

    public class TabelaPrecoAvulsoNotificacaoAplicacao : BaseAplicacao<TabelaPrecoAvulsoNotificacao, ITabelaPrecoAvulsoNotificacaoServico>, ITabelaPrecoAvulsoNotificacaoAplicacao
    {
        private readonly IParametroNotificacaoAplicacao _parametroNotificacaoAplicacao;
        private readonly ITabelaPrecoAvulsoAplicacao _tabelaPrecoAvulsoAplicacao;

        public TabelaPrecoAvulsoNotificacaoAplicacao(
            IParametroNotificacaoAplicacao parametroNotificacaoAplicacao, 
            ITabelaPrecoAvulsoAplicacao tabelaPrecoAvulsoAplicacao)
        {
            _parametroNotificacaoAplicacao = parametroNotificacaoAplicacao;
            _tabelaPrecoAvulsoAplicacao = tabelaPrecoAvulsoAplicacao;
        }

        public List<TabelaPrecoAvulsoNotificacao> RetornaNotificacoesPendentes(Usuario usuario)
        {
            try
            {
                var parametroTabelaPrecoAvulsoNotificacao = _parametroNotificacaoAplicacao
                    .BuscarPor(x => 
                        x.TipoNotificacao.Entidade == Entidades.TabelaPrecoAvulso && 
                        x.Aprovadores.Any(m => m.Usuario.Id == usuario.Id))
                    .FirstOrDefault();

                var listaTabelaPrecoAvulso = new List<TabelaPrecoAvulso>();
                if (parametroTabelaPrecoAvulsoNotificacao != null)
                    listaTabelaPrecoAvulso = _tabelaPrecoAvulsoAplicacao.BuscarPor(x => x.Notificacoes.Any(m => m.Notificacao.Status == StatusNotificacao.Aguardando)).ToList();

                var listaTabelaPrecoAvulsoNotificacao = new List<TabelaPrecoAvulsoNotificacao>();

                foreach (var item in listaTabelaPrecoAvulso)
                {
                    foreach (var item2 in item.Notificacoes)
                    {
                        var TabelaPrecoAvulsoNotif = new TabelaPrecoAvulsoNotificacao()
                        {
                            TabelaPrecoAvulso = item,
                            Notificacao = item2.Notificacao
                        };
                        listaTabelaPrecoAvulsoNotificacao.Add(TabelaPrecoAvulsoNotif);
                    }
                }
                return listaTabelaPrecoAvulsoNotificacao;
            }
            catch (Exception ex)
            {
                throw new System.Exception("Erro ao buscar as notificações pendentes de Preço : " + ex.Message + "trace: " + ex.StackTrace);
            }
        }

        public void Aprovar(TabelaPrecoAvulsoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Aprovado;
            Salvar(model);
        }

        public void Reprovar(TabelaPrecoAvulsoNotificacao model)
        {
            model.Notificacao.Status = StatusNotificacao.Reprovado;
            Salvar(model);
        }
    }
}