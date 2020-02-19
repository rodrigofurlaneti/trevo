using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class EstruturaUnidadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public int Quantidade { get; set; }
        public bool SolicitarCompra { get; set; }
        public int Unidade { get; set; }
        public bool Checkado { get; set; }
        public EstruturaGaragemViewModel EstruturaGaragem { get; set; }

        public EstruturaUnidadeViewModel()
        {

        }

        public EstruturaUnidadeViewModel(EstruturaUnidade estruturaUnidade)
        {
            Id = estruturaUnidade.Id;
            DataInsercao = estruturaUnidade.DataInsercao;
            Quantidade = estruturaUnidade.Quantidade;
            SolicitarCompra = estruturaUnidade.SolicitarCompra;
            EstruturaGaragem = new EstruturaGaragemViewModel(estruturaUnidade?.EstruturaGaragem ?? new EstruturaGaragem());
        }

        public EstruturaUnidade ToEntity()
        {
            return new EstruturaUnidade
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Quantidade = Quantidade,
                SolicitarCompra = SolicitarCompra,
                EstruturaGaragem = EstruturaGaragem?.ToEntity()
            };
        }

        public EstruturaUnidadeViewModel ToViewModel(EstruturaUnidade estruturaUnidade)
        {
            return new EstruturaUnidadeViewModel
            {
                Id = estruturaUnidade.Id,
                DataInsercao = estruturaUnidade.DataInsercao,
                Quantidade = estruturaUnidade.Quantidade,
                SolicitarCompra = estruturaUnidade.SolicitarCompra,
                EstruturaGaragem = new EstruturaGaragemViewModel(estruturaUnidade?.EstruturaGaragem ?? new EstruturaGaragem())
            };
           
        }

    }
}
