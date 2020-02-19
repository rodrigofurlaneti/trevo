using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface ITabelaPrecoMensalistaNotificacaoServico : IBaseServico<TabelaPrecoMensalistaNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(TabelaPrecoMensalista TabelaPrecoMensalista, int idUsuario);
    }

    public class TabelaPrecoMensalistaNotificacaoServico : BaseServico<TabelaPrecoMensalistaNotificacao, ITabelaPrecoMensalistaNotificacaoRepositorio>, ITabelaPrecoMensalistaNotificacaoServico
    {
        private readonly ITabelaPrecoMensalistaNotificacaoRepositorio _TabelaPrecoMensalistaNotificacaoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public TabelaPrecoMensalistaNotificacaoServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 ITabelaPrecoMensalistaNotificacaoRepositorio TabelaPrecoMensalistaNotificacaoRepositorio,
                                                 ITabelaPrecoMensalistaRepositorio TabelaPrecoMensalistaRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _TabelaPrecoMensalistaNotificacaoRepositorio = TabelaPrecoMensalistaNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao)
        {

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:

                    Aprovar(idNotificacao, idUsuario);

                    break;
                case AcaoNotificacao.Reprovado:

                    Reprovar(idNotificacao, idUsuario);

                    break;
                default:
                    break;
            }
        }

        public void Aprovar(int idNotificacao, int idUsuario)
        {
            var TabelaPrecoMensalistaNotificacao = _TabelaPrecoMensalistaNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (TabelaPrecoMensalistaNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            TabelaPrecoMensalistaNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            TabelaPrecoMensalistaNotificacao.Notificacao.Status = StatusNotificacao.Aprovado;
            TabelaPrecoMensalistaNotificacao.TabelaPrecoMensalista.Status = StatusSolicitacao.Aprovado; ;
            _TabelaPrecoMensalistaNotificacaoRepositorio.Save(TabelaPrecoMensalistaNotificacao);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var TabelaPrecoMensalistaNotificacao = _TabelaPrecoMensalistaNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (TabelaPrecoMensalistaNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            TabelaPrecoMensalistaNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            TabelaPrecoMensalistaNotificacao.Notificacao.Status = StatusNotificacao.Reprovado;
            TabelaPrecoMensalistaNotificacao.TabelaPrecoMensalista.Status = StatusSolicitacao.Negado;
            _TabelaPrecoMensalistaNotificacaoRepositorio.Save(TabelaPrecoMensalistaNotificacao);
        }

        public void Criar(TabelaPrecoMensalista TabelaPrecoMensalista, int idUsuario)
        {
            if (TabelaPrecoMensalista.Status == StatusSolicitacao.Aguardando)
            {
                var notificacao = new Notificacao
                {
                    Usuario = _usuarioRepositorio.GetById(idUsuario),
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.TabelaPrecoMensalista),
                    Status = StatusNotificacao.Aguardando,
                    Descricao = "TabelaPrecoMensalista ID: " + TabelaPrecoMensalista.Id.ToString(),
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar
                };

                var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);

                var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

                var TabelaPrecoMensalistaNotificacao = new TabelaPrecoMensalistaNotificacao();
                TabelaPrecoMensalistaNotificacao.Notificacao = notificacaoNova;

                TabelaPrecoMensalistaNotificacao.TabelaPrecoMensalista = TabelaPrecoMensalista;

                _TabelaPrecoMensalistaNotificacaoRepositorio.Save(TabelaPrecoMensalistaNotificacao);
            }
        }
    }
}