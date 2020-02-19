using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class TipoEquipeViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }

        public TipoEquipeViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public TipoEquipeViewModel(TipoEquipe tipoequipe)
        {
            Id = tipoequipe.Id;
            Descricao = tipoequipe.Descricao;
            DataInsercao = tipoequipe.DataInsercao;
        }

        public TipoEquipe ToEntity()
        {
            var entidade = new TipoEquipe
            {
                Id = Id,
                Descricao = Descricao,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            };

            return entidade;
        }
    }
}
