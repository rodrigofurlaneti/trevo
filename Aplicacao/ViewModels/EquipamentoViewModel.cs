using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class EquipamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public EquipamentoViewModel() { }

        public EquipamentoViewModel(Equipamento equipamento)
        {
            Id = equipamento.Id;
            DataInsercao = equipamento.DataInsercao;
            Codigo = equipamento.Codigo;
            Descricao = equipamento.Descricao;
        }

        public Equipamento ToEntity()
        {
            return new Equipamento
            {
                Id= Id,
                DataInsercao = DataInsercao,
                Codigo = Codigo,
                Descricao = Descricao
            };
        }

        public EquipamentoViewModel ToViewModel(Equipamento equipamento)
        {
            return new EquipamentoViewModel
            {
                Id = equipamento.Id,
                DataInsercao = equipamento.DataInsercao,
                Codigo = equipamento.Codigo,
                Descricao = equipamento.Descricao
            };
        }
    }
}
