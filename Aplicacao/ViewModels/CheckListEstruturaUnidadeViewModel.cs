using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class CheckListEstruturaUnidadeViewModel
    {
        public int Id { get; set; }
        public string EstruturaGaragem { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInsercao { get; set; }

        public CheckListEstruturaUnidadeViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public CheckListEstruturaUnidadeViewModel(CheckListEstruturaUnidade checkListEstruturaUnidade)
        {
            Id = checkListEstruturaUnidade.Id;
            EstruturaGaragem = checkListEstruturaUnidade.EstruturaGaragem;
            Quantidade = checkListEstruturaUnidade.Quantidade;
            Ativo = checkListEstruturaUnidade.Ativo;
            DataInsercao = checkListEstruturaUnidade.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : checkListEstruturaUnidade.DataInsercao;
        }

        public CheckListEstruturaUnidade ToEntity()
        {
            var entidade = new CheckListEstruturaUnidade
            {
                Id = Id,
                EstruturaGaragem = EstruturaGaragem,
                Quantidade = Quantidade,
                Ativo = Ativo,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            };

            return entidade;
        }
    }
}
