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
    public interface IParametroEquipeNotificacaoServico : IBaseServico<ParametroEquipeNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(ParametroEquipe parametroEquipe, int idUsuario);
    }

    public class ParametroEquipeNotificacaoServico : BaseServico<ParametroEquipeNotificacao, IParametroEquipeNotificacaoRepositorio>, IParametroEquipeNotificacaoServico
    {
        private readonly IParametroEquipeNotificacaoRepositorio _parametroEquipeNotificacaoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public ParametroEquipeNotificacaoServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 IParametroEquipeNotificacaoRepositorio parametroEquipeNotificacaoRepositorio,
                                                 IParametroEquipeRepositorio parametroEquipeRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _parametroEquipeNotificacaoRepositorio = parametroEquipeNotificacaoRepositorio;
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
            var ParametroEquipeNotificacao = _parametroEquipeNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (ParametroEquipeNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            ParametroEquipeNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            ParametroEquipeNotificacao.Notificacao.Status = StatusNotificacao.Aprovado;
            ParametroEquipeNotificacao.ParametroEquipe.Status = StatusSolicitacao.Aprovado; ;
            _parametroEquipeNotificacaoRepositorio.Save(ParametroEquipeNotificacao);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var ParametroEquipeNotificacao = _parametroEquipeNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (ParametroEquipeNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            ParametroEquipeNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            ParametroEquipeNotificacao.Notificacao.Status = StatusNotificacao.Reprovado;
            ParametroEquipeNotificacao.ParametroEquipe.Status = StatusSolicitacao.Negado;
            _parametroEquipeNotificacaoRepositorio.Save(ParametroEquipeNotificacao);
        }

        public void Criar(ParametroEquipe parametroEquipe, int idUsuario)
        {
            if (parametroEquipe.Status == StatusSolicitacao.Aguardando)
            {
                var notificacao = new Notificacao
                {
                    Usuario = _usuarioRepositorio.GetById(idUsuario),
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.ParametroEquipe),
                    Status = StatusNotificacao.Aguardando,
                    Descricao = "ParametroEquipe ID: " + parametroEquipe.Id.ToString(),
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar
                };

                var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);

                var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

                var ParametroEquipeNotificacao = new ParametroEquipeNotificacao();
                ParametroEquipeNotificacao.Notificacao = notificacaoNova;

                ParametroEquipeNotificacao.ParametroEquipe = parametroEquipe;

                _parametroEquipeNotificacaoRepositorio.Save(ParametroEquipeNotificacao);
            }
        }
    }
}