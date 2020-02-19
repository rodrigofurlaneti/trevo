using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class TipoAtividadeViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }

        public TipoAtividadeViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TipoAtividadeViewModel(TipoAtividade tipoAtividade)
        {
            Id = tipoAtividade.Id;
            Descricao = tipoAtividade.Descricao;
            Ativo = tipoAtividade.Ativo;
            DataInsercao = tipoAtividade.DataInsercao;
        }

        public TipoAtividade ToEntity()
        {
            var entidade = new TipoAtividade
            {
                Id = Id,
                Descricao = Descricao,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                Ativo = Ativo
            };

            return entidade;
        }
    }
}
