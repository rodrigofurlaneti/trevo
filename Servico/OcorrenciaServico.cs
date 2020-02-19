using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IOcorrenciaServico : IBaseServico<OcorrenciaCliente>
    {
        IList<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina);
        IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50);
        void Notificar(OcorrenciaCliente ocorrencia, int usuarioId);
    }

    public class OcorrenciaServico : BaseServico<OcorrenciaCliente, IOcorrenciaRepositorio>, IOcorrenciaServico
    {
        private readonly IOcorrenciaRepositorio _ocorrenciaRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;

        public OcorrenciaServico(IOcorrenciaRepositorio ocorrenciaRepositorio,
                                 INotificacaoRepositorio notificacaoRepositorio,
                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                 IUsuarioRepositorio usuarioRepositorio)
        {
            _ocorrenciaRepositorio = ocorrenciaRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;

            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            return _ocorrenciaRepositorio.BuscarDadosGrid(protocolo, nome, status, out quantidadeRegistros, pagina, take);
        }

        public IList<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina)
        {
            return _ocorrenciaRepositorio.BuscarPorIntervaloOrdenadoPorNome(registroInicial, registrosPorPagina);
        }

        public void Notificar(OcorrenciaCliente ocorrencia, int usuarioId)
        {
            var usuarioAprovador = _usuarioRepositorio.FirstBy(x => x.Funcionario.Id == ocorrencia.FuncionarioAtribuido.Id);
            string msgPorStatus;

            msgPorStatus = $"Ocorrência [Protocolo: {ocorrencia.NumeroProtocolo} - Funcionário: {(usuarioAprovador != null ? usuarioAprovador.Funcionario.PessoaNome : "Não Informado")}].";

            var urlPersonalizada = $"OcorrenciaCliente/Edit?id={ocorrencia.Id}";

            Notificacao notificacaoOcorrencia = _notificacaoRepositorio.FirstBy(x => x.TipoNotificacao.Entidade == Entidades.OcorrenciaAtribuida
                                                                            && x.UrlPersonalizada == urlPersonalizada);

            if (notificacaoOcorrencia == null)
            {
                notificacaoOcorrencia = new Notificacao
                {
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.OcorrenciaAtribuida),
                    Status = StatusNotificacao.Visualizacao,
                    Descricao = msgPorStatus,
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.Aviso,
                    UrlPersonalizada = urlPersonalizada
                };

            }

            notificacaoOcorrencia.Usuario = _usuarioRepositorio.GetById(usuarioId);
            notificacaoOcorrencia.DataVencimentoNotificacao = DateTime.Now.AddDays(5);
            notificacaoOcorrencia.Descricao = msgPorStatus;
            notificacaoOcorrencia.DataInsercao = DateTime.Now;

            if (usuarioAprovador != null)
            {
                notificacaoOcorrencia.NotificacaoUsuarioAprovadores = new List<NotificacaoUsuarioAprovador>
                {
                    new NotificacaoUsuarioAprovador()
                    {
                        UsuarioAprovador = usuarioAprovador,
                        Notificacao = notificacaoOcorrencia
                    }
                };
            }

            _notificacaoRepositorio.Save(notificacaoOcorrencia);
        }
    }
}
