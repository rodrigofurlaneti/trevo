using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Linq;

namespace Dominio
{
    public interface IPrecoNotificacaoServico : IBaseServico<PrecoNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(Preco Preco, int idUsuario);
    }

    public class PrecoNotificacaoServico : BaseServico<PrecoNotificacao, IPrecoNotificacaoRepositorio>, IPrecoNotificacaoServico
    {

        private readonly IPrecoNotificacaoRepositorio _precoNotificacaoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public PrecoNotificacaoServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 IPrecoNotificacaoRepositorio PrecoNotificacaoRepositorio,
                                                 IPrecoRepositorio PrecoRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _precoNotificacaoRepositorio = PrecoNotificacaoRepositorio;
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
            var precoNotificacao = _precoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if(precoNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            precoNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            precoNotificacao.Notificacao.Status = StatusNotificacao.Aprovado;
            precoNotificacao.Preco.PrecoStatus = StatusPreco.Aprovado;
            _precoNotificacaoRepositorio.Save(precoNotificacao);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var precoNotificacao = _precoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (precoNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            precoNotificacao.Notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            precoNotificacao.Notificacao.Status = StatusNotificacao.Reprovado;
            precoNotificacao.Preco.PrecoStatus = StatusPreco.Negado;
            _precoNotificacaoRepositorio.Save(precoNotificacao);
        }

        public void Criar(Preco Preco, int idUsuario)
        {
            if (Preco.PrecoStatus == StatusPreco.Pendente)
            {
                var notificacao = new Notificacao
                {
                    Usuario = _usuarioRepositorio.GetById(idUsuario),
                    TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.TabelaPreco),
                    Status = StatusNotificacao.Aguardando,
                    Descricao = "Preco ID: " + Preco.Id.ToString(),
                    DataInsercao = DateTime.Now,
                    AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar
                };

                var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);

                var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

                var PrecoNotificacao = new PrecoNotificacao();
                PrecoNotificacao.Notificacao = notificacaoNova;

                PrecoNotificacao.Preco = Preco;

                _precoNotificacaoRepositorio.Save(PrecoNotificacao);
            }
        }
    }
}