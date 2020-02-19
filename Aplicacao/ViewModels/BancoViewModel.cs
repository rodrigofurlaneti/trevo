using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class BancoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string CodigoBanco { get; set; }

        public virtual string CodigoDescricao { get { return CodigoBanco + " - " + Descricao; } }

        public BancoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public BancoViewModel(Banco banco)
        {
            this.Id = banco?.Id ?? 0;
            this.DataInsercao = banco?.DataInsercao ?? DateTime.Now;
            this.Descricao = banco?.Descricao;
            this.CodigoBanco = banco?.CodigoBanco;
        }

        public Banco ToEntity() => new Banco()
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Descricao = this.Descricao,
            CodigoBanco = this.CodigoBanco
        };
    }
}
