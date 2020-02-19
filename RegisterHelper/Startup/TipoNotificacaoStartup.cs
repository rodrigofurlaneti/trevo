using System;
using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class TipoNotificacaoStartup
    {
        public static void Start()
        {
            AdicionaTipoNotificacaoDefault();
        }

        private static void AdicionaTipoNotificacaoDefault()
        {
            var tipoNotificacaoAplicacao = ServiceLocator.Current.GetInstance<ITipoNotificacaoAplicacao>();
            var tiposNotifacoes = tipoNotificacaoAplicacao.BuscarValoresDoEnum<Entidades>();

            var tiposNotificacoesParaSalvar = new List<TipoNotificacao>();
            foreach (var tipoNotificacao in tiposNotifacoes)
            {
                if(tipoNotificacaoAplicacao.PrimeiroPor(x => (int)x.Entidade == tipoNotificacao.Id) == null)
                {
                    var tipoNotificacaoParaSalvar = new TipoNotificacao
                    {
                        Descricao = tipoNotificacao.Descricao,
                        Entidade = (Entidades)tipoNotificacao.Id
                    };

                    tiposNotificacoesParaSalvar.Add(tipoNotificacaoParaSalvar);
                }
            }

            if (tiposNotificacoesParaSalvar.Count > 0)
                tipoNotificacaoAplicacao.Salvar(tiposNotificacoesParaSalvar);
        }
    }
}