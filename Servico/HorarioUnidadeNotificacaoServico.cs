using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Linq;

namespace Dominio
{
    public interface IHorarioUnidadeNotificacaoServico : IBaseServico<HorarioUnidadeNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(HorarioUnidade HorarioUnidade, int idUsuario);
    }

    public class HorarioUnidadeNotificacaoServico : BaseServico<HorarioUnidadeNotificacao, IHorarioUnidadeNotificacaoRepositorio>, IHorarioUnidadeNotificacaoServico
    {
        private readonly IHorarioUnidadeNotificacaoRepositorio _horarioUnidadeNotificacaoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IHorarioUnidadeRepositorio _horarioUnidadeRepositorio;

        public HorarioUnidadeNotificacaoServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 IHorarioUnidadeNotificacaoRepositorio horarioUnidadeNotificacaoRepositorio,
                                                 IHorarioUnidadeRepositorio horarioUnidadeRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _horarioUnidadeNotificacaoRepositorio = horarioUnidadeNotificacaoRepositorio;
            _horarioUnidadeRepositorio = horarioUnidadeRepositorio;
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
            var HorarioUnidadeNotificacao = _horarioUnidadeNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (HorarioUnidadeNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            var notificacao = _notificacaoRepositorio.GetById(HorarioUnidadeNotificacao.Notificacao.Id);
            notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            notificacao.Status = StatusNotificacao.Aprovado;
            _notificacaoRepositorio.Save(notificacao);

            var horarioUnidade = _horarioUnidadeRepositorio.GetById(HorarioUnidadeNotificacao.HorarioUnidade.Id);
            horarioUnidade.Status = StatusHorario.Aprovado;
            _horarioUnidadeRepositorio.Save(horarioUnidade);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var HorarioUnidadeNotificacao = _horarioUnidadeNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (HorarioUnidadeNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            var notificacao = _notificacaoRepositorio.GetById(HorarioUnidadeNotificacao.Notificacao.Id);
            notificacao.Status = StatusNotificacao.Reprovado;
            _notificacaoRepositorio.Save(notificacao);

            var horarioUnidade = _horarioUnidadeRepositorio.GetById(HorarioUnidadeNotificacao.HorarioUnidade.Id);
            horarioUnidade.Status = StatusHorario.Negado;
            _horarioUnidadeRepositorio.Save(horarioUnidade);
        }

        public void Criar(HorarioUnidade HorarioUnidade, int idUsuario)
        {
            if (HorarioUnidade.Status == StatusHorario.Aguardando)
            {
                var notificacao = new Notificacao
                {
                    Usuario = _usuarioRepositorio.GetById(idUsuario),
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.HorarioUnidade),
                    Status = StatusNotificacao.Aguardando,
                    Descricao = "HorarioUnidade ID: " + HorarioUnidade.Id.ToString(),
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar
                };

                var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);

                var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

                var HorarioUnidadeNotificacao = new HorarioUnidadeNotificacao
                {
                    Notificacao = notificacaoNova,
                    HorarioUnidade = HorarioUnidade
                };

                _horarioUnidadeNotificacaoRepositorio.Save(HorarioUnidadeNotificacao);
            }
        }
    }
}