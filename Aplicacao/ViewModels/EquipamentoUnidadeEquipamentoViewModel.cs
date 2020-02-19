using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class EquipamentoUnidadeEquipamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool Ativo { get; set; }
        public int Quantidade { get; set; }
        public int EquipamentoUnidade { get; set; }
        public EquipamentoViewModel Equipamento { get; set; }
        public DateTime UltimaConferencia { get; set; }

        public EquipamentoUnidadeEquipamentoViewModel() { }

        public EquipamentoUnidadeEquipamentoViewModel(EquipamentoUnidadeEquipamento equipamentoUnidadeEquipamento)
        {
            Id = equipamentoUnidadeEquipamento.Id;
            DataInsercao = equipamentoUnidadeEquipamento.DataInsercao;
            Ativo = equipamentoUnidadeEquipamento.Ativo;
            Quantidade = equipamentoUnidadeEquipamento.Quantidade;
            EquipamentoUnidade = equipamentoUnidadeEquipamento.EquipamentoUnidade;
            Equipamento = new EquipamentoViewModel(equipamentoUnidadeEquipamento?.Equipamento ?? new Equipamento());
        }

        public EquipamentoUnidadeEquipamento ToEntity()
        {
            return new EquipamentoUnidadeEquipamento
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Ativo = Ativo,
                Quantidade = Quantidade,
                EquipamentoUnidade = EquipamentoUnidade,
                Equipamento = Equipamento?.ToEntity()
            };
        }

        public EquipamentoUnidadeEquipamentoViewModel ToViewModel(EquipamentoUnidadeEquipamento equipamentoUnidadeEquipamento)
        {
            return new EquipamentoUnidadeEquipamentoViewModel
            {
                Id = equipamentoUnidadeEquipamento.Id,
                DataInsercao = equipamentoUnidadeEquipamento.DataInsercao,
                Ativo = equipamentoUnidadeEquipamento.Ativo,
                Quantidade = equipamentoUnidadeEquipamento.Quantidade,
                EquipamentoUnidade = equipamentoUnidadeEquipamento.EquipamentoUnidade,
                Equipamento = new EquipamentoViewModel(equipamentoUnidadeEquipamento?.Equipamento ?? new Equipamento())
            };
        }

    }
}
