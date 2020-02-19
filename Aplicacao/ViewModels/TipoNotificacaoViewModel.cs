using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoNotificacaoViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInsercao { get; set; }
        public Entidades Entidade { get; set; }

        public TipoNotificacaoViewModel()
        {
            this.DataInsercao = DateTime.Now;
        }

        public TipoNotificacaoViewModel(TipoNotificacao tipoNotificacao)
        {
            this.Id = tipoNotificacao.Id;
            this.Descricao = tipoNotificacao.Descricao;
            this.DataInsercao = tipoNotificacao?.DataInsercao ?? DateTime.Now;
            this.Entidade = tipoNotificacao.Entidade;
        }

        public TipoNotificacao ToEntity() => new TipoNotificacao()
        {
            Id = this.Id,
            Descricao = this.Descricao,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Entidade = this.Entidade
        };

    }
}
