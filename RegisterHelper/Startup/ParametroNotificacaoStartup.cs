using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class ParametroNotificacaoStartup
    {
        public static void Start()
        {
            AdicionaParametroNotificacaoDefault();
        }

        private static void AdicionaParametroNotificacaoDefault()
        {
            var _tipoNotificacaoAplicacao = ServiceLocator.Current.GetInstance<ITipoNotificacaoAplicacao>();
            var _parametroNotificacaoAplicacao = ServiceLocator.Current.GetInstance<IParametroNotificacaoAplicacao>();
            var _usuarioAplicacao = ServiceLocator.Current.GetInstance<IUsuarioAplicacao>();

            var usuario = _usuarioAplicacao.BuscarPorId(1);
            var tipoNotificacoes = _tipoNotificacaoAplicacao.Buscar().ToList();
            var parametroNotificacoes = _parametroNotificacaoAplicacao.Buscar().ToList();

            var parametrosNotificacoesParaSalvar = new List<ParametroNotificacao>();
            foreach (var tipoNotificacao in tipoNotificacoes)
            {
                if(!parametroNotificacoes.Any(x => x.TipoNotificacao.Id == tipoNotificacao.Id))
                {
                    var parametroNotificacaoParaSalvar = new ParametroNotificacao
                    {
                        DataInsercao = DateTime.Now,
                        TipoNotificacao = tipoNotificacao,
                        Aprovadores = new List<ParametroNotificacaoUsuario>()
                        {
                            new ParametroNotificacaoUsuario
                            {
                                DataInsercao = DateTime.Now,
                                Usuario = usuario
                            }
                        }
                    };

                    parametrosNotificacoesParaSalvar.Add(parametroNotificacaoParaSalvar);
                }
            }

            if (parametrosNotificacoesParaSalvar.Count > 0)
                _parametroNotificacaoAplicacao.Salvar(parametrosNotificacoesParaSalvar);
        }
    }
}