
using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class NotificacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public UsuarioViewModel Aprovador { get; set; }
        public DateTime DataAprovacao { get; set; }
        public StatusNotificacao Status { get; set; }
        public TipoNotificacaoViewModel TipoNotificacao { get; set; }
        public string Descricao { get; set; }

        public DateTime? DataVencimentoNotificacao { get; set; }
        public TipoAcaoNotificacao AcaoNotificacao { get; set; }

        public NotificacaoViewModel()
        {

        }

        public NotificacaoViewModel(Notificacao notificacao)
        {
            Id = notificacao?.Id ?? 0;
            DataInsercao = notificacao?.DataInsercao ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            Usuario = notificacao?.Usuario == null ? new UsuarioViewModel() : new UsuarioViewModel(notificacao?.Usuario);
            Aprovador = notificacao?.Aprovador == null ? new UsuarioViewModel() : new UsuarioViewModel(notificacao?.Aprovador);
            DataAprovacao = notificacao?.DataAprovacao ?? System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            Status = notificacao?.Status ?? StatusNotificacao.Aguardando;
            TipoNotificacao = notificacao?.TipoNotificacao == null ? new TipoNotificacaoViewModel() : new TipoNotificacaoViewModel(notificacao?.TipoNotificacao);
            Descricao = notificacao?.Descricao ?? string.Empty;
            DataVencimentoNotificacao = notificacao?.DataVencimentoNotificacao;
            AcaoNotificacao = notificacao?.AcaoNotificacao ?? TipoAcaoNotificacao.AprovarReprovar;
        }

        public Notificacao ToEntity() => new Notificacao
        {
            Id = Id,
            DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Usuario = Usuario.ToEntity(),
            Aprovador = Aprovador.ToEntity(),
            Status = Status,
            TipoNotificacao = TipoNotificacao.ToEntity(),
            Descricao = Descricao,
            DataVencimentoNotificacao = DataVencimentoNotificacao,
            AcaoNotificacao = AcaoNotificacao
        };
    }
}
