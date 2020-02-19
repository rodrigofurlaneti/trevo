using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class EstruturaGaragemViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }

        public EstruturaGaragemViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public EstruturaGaragemViewModel(EstruturaGaragem estruturaGaragem)
        {
            Id = estruturaGaragem.Id;
            Descricao = estruturaGaragem.Descricao;
            DataInsercao = estruturaGaragem.DataInsercao;
        }

        public EstruturaGaragem ToEntity()
        {
            var entidade = new EstruturaGaragem
            {
                Id = Id,
                Descricao = Descricao,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            };

            return entidade;
        }
    }
}
