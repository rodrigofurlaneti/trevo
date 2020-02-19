using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Linq;

namespace Dominio
{
    public interface INotificacaoDesbloqueioReferenciaServico : IBaseServico<NotificacaoDesbloqueioReferencia>
    {
        void Criar(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, int idUsuario, string nomeArquivo = null, string msgErro = null, string urlPersonalizada = null);
        void AtualizarStatus(int idNotificacao, int idUsuarioAcao, AcaoNotificacao acao);
        void ConsumirLiberacao(int idNotificacao, bool utilizado = false);
    }

    public class NotificacaoDesbloqueioReferenciaServico : BaseServico<NotificacaoDesbloqueioReferencia, INotificacaoDesbloqueioReferenciaRepositorio>, INotificacaoDesbloqueioReferenciaServico
    {
        private readonly INotificacaoDesbloqueioReferenciaRepositorio _notificacaoDesbloqueioReferenciaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly ILeituraCNABServico _leituraCNABServico;

        public NotificacaoDesbloqueioReferenciaServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 INotificacaoDesbloqueioReferenciaRepositorio notificacaoDesbloqueioReferenciaRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio,
                                                 ILeituraCNABServico leituraCNABServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoDesbloqueioReferenciaRepositorio = notificacaoDesbloqueioReferenciaRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _leituraCNABServico = leituraCNABServico;
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
            var notificacao = _notificacaoRepositorio.GetById(idNotificacao);

            if (notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }
            else if (notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Aprovar' novamente!");
            }

            var notificacaoBloq = _notificacaoDesbloqueioReferenciaRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);
            notificacaoBloq.StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aprovado;

            if (notificacaoBloq.EntidadeRegistro == Entidades.LeituraCNAB)
            {
                var leituraCnab = _leituraCNABServico.BuscarPor(x => x.NomeArquivo == notificacaoBloq.NomeArquivoCNABAssociado)?.FirstOrDefault();
                var dadosLeitura = Helpers.ArquivoRetorno.LeituraCNAB400(leituraCnab.Arquivo, leituraCnab.ContaFinanceira);
                dadosLeitura.Value.RemoveAll(x => Convert.ToInt32(x.NossoNumero) != notificacaoBloq.IdRegistro);
                _leituraCNABServico.ProcessarLeitura(dadosLeitura.Key, dadosLeitura.Value, idUsuario, false, notificacaoBloq);
            }

            notificacao.Status = StatusNotificacao.Aprovado;
            notificacao.DataAprovacao = DateTime.Now;
            notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            _notificacaoRepositorio.Save(notificacao);

            _notificacaoDesbloqueioReferenciaRepositorio.Save(notificacaoBloq);

            if (notificacaoBloq.EntidadeRegistro == Entidades.LeituraCNAB)
                ConsumirLiberacao(notificacao.Id);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var notificacao = _notificacaoRepositorio.GetById(idNotificacao);

            if (notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }
            else if (notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Reprovar' novamente!");
            }

            notificacao.Status = StatusNotificacao.Reprovado;
            notificacao.DataAprovacao = DateTime.Now;
            notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            _notificacaoRepositorio.Save(notificacao);
            var notificacaoBloq = _notificacaoDesbloqueioReferenciaRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);
            notificacaoBloq.StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Negado;
            _notificacaoDesbloqueioReferenciaRepositorio.Save(notificacaoBloq);
            //Atualmente nenhuma decisão é tomada quando é reprovado, por isso é dado como utilizado logo em seguida que é reprovado.
            ConsumirLiberacao(notificacao.Id);
        }

        public void Criar(int idRegistro, Entidades entidadeRegistro, DateTime dataReferencia, int idUsuario, string nomeArquivo = null, string msgErro = null, string urlPersonalizada = null)
        {
            var usuario = _usuarioRepositorio.GetById(idUsuario);
            var notificacao = new Notificacao
            {
                Usuario = usuario,
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.DesbloqueioReferencia),
                Status = StatusNotificacao.Aguardando,
                Descricao = $"O solicitante [{usuario.Funcionario.PessoaNome}] requer o desbloqueio de referência{(entidadeRegistro != Entidades.BaixaManual ? $" [Ref.{dataReferencia.ToString("yyyy/MM")}] " : " ") }do registro [{(idRegistro > 0 || (idRegistro > 0 && entidadeRegistro != Entidades.BaixaManual) ? $"{entidadeRegistro.ToDescription()}: Id - {idRegistro.ToString()}" : entidadeRegistro.ToDescription())}]",
                DataInsercao = DateTime.Now,
                DataVencimentoNotificacao = DateTime.Now.AddDays(1),
                AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar,
                UrlPersonalizada = urlPersonalizada
            };

            var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);
            var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

            var notificacaoDesbloqueioReferencia = new NotificacaoDesbloqueioReferencia
            {
                IdRegistro = idRegistro,
                EntidadeRegistro = entidadeRegistro,
                DataReferencia = dataReferencia,
                LiberacaoUtilizada = false,
                StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando,
                Notificacao = notificacaoNova,
                NomeArquivoCNABAssociado = nomeArquivo
            };

            _notificacaoDesbloqueioReferenciaRepositorio.Save(notificacaoDesbloqueioReferencia);
        }

        public void ConsumirLiberacao(int idNotificacao, bool utilizado = false)
        {
            var notificacaoBloq = _notificacaoDesbloqueioReferenciaRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);
            if (notificacaoBloq != null)
            {
                notificacaoBloq.StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Utilizado;
                notificacaoBloq.LiberacaoUtilizada = utilizado;
                _notificacaoDesbloqueioReferenciaRepositorio.Save(notificacaoBloq);
            }
        }
    }
}