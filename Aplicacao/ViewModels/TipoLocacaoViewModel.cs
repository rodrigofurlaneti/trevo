using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class TipoLocacaoViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInsercao { get; set; }

        public TipoLocacaoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TipoLocacaoViewModel(TipoLocacao TipoLocacao)
        {
            Id = TipoLocacao.Id;
            Descricao = TipoLocacao.Descricao;
            DataInsercao = TipoLocacao.DataInsercao;
        }

        public TipoLocacao ToEntity()
        {
            var entidade = new TipoLocacao
            {
                Id = Id,
                Descricao = Descricao,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            };

            return entidade;
        }
    }
}
