using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;

namespace Dominio
{
    public interface ITabelaPrecoAvulsoNotificacaoServico : IBaseServico<TabelaPrecoAvulsoNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(TabelaPrecoAvulso tabelaPrecoAvulso, int idUsuario);
    }

    public class TabelaPrecoAvulsoNotificacaoServico : BaseServico<TabelaPrecoAvulsoNotificacao, ITabelaPrecoAvulsoNotificacaoRepositorio>, ITabelaPrecoAvulsoNotificacaoServico
    {
        private readonly ITabelaPrecoAvulsoNotificacaoRepositorio _tabelaPrecoAvulsoNotificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public TabelaPrecoAvulsoNotificacaoServico(
            IUsuarioRepositorio usuarioRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            ITabelaPrecoAvulsoNotificacaoRepositorio tabelaPrecoAvulsoNotificacaoRepositorio,
            ITabelaPrecoAvulsoRepositorio tabelaPrecoAvulsoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _tabelaPrecoAvulsoNotificacaoRepositorio = tabelaPrecoAvulsoNotificacaoRepositorio;
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
            var tabelaPrecoAvulsoNotificacao = _tabelaPrecoAvulsoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (tabelaPrecoAvulsoNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");

            tabelaPrecoAvulsoNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            tabelaPrecoAvulsoNotificacao.Notificacao.Status = StatusNotificacao.Aprovado;
            tabelaPrecoAvulsoNotificacao.TabelaPrecoAvulso.Status = StatusSolicitacao.Aprovado;
            _tabelaPrecoAvulsoNotificacaoRepositorio.Save(tabelaPrecoAvulsoNotificacao);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var tabelaPrecoAvulsoNotificacao = _tabelaPrecoAvulsoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (tabelaPrecoAvulsoNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");

            tabelaPrecoAvulsoNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            tabelaPrecoAvulsoNotificacao.Notificacao.Status = StatusNotificacao.Reprovado;
            tabelaPrecoAvulsoNotificacao.TabelaPrecoAvulso.Status = StatusSolicitacao.Negado;
            _tabelaPrecoAvulsoNotificacaoRepositorio.Save(tabelaPrecoAvulsoNotificacao);
        }

        public void Criar(TabelaPrecoAvulso tabelaPrecoAvulso, int idUsuario)
        {
            if (tabelaPrecoAvulso.Status == StatusSolicitacao.Aguardando)
            {
                var notificacao = new Notificacao
                {
                    Usuario = _usuarioRepositorio.GetById(idUsuario),
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.TabelaPrecoAvulso),
                    Status = StatusNotificacao.Aguardando,
                    Descricao = Entidades.TabelaPrecoAvulso.ToDescription() + " ID: " + tabelaPrecoAvulso.Id.ToString(),
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar
                };

                var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);
                var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);
                var tabelaPrecoAvulsoNotificacao = new TabelaPrecoAvulsoNotificacao
                {
                    Notificacao = notificacaoNova,
                    TabelaPrecoAvulso = tabelaPrecoAvulso
                };

                _tabelaPrecoAvulsoNotificacaoRepositorio.Save(tabelaPrecoAvulsoNotificacao);
            }
        }
    }
}