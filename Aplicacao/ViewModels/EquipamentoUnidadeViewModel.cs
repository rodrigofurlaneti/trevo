using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class EquipamentoUnidadeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool GerarNotificacao { get; set; }
        public string Observacao { get; set; }
        public bool ConferenciaConcluida { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public PeriodoDiasEquipamentoUnidade PeriodoEquipamentoUnidade { get; set; }
        public DateTime? UltimaConferencia { get; set; }
        public string Usuario { get; set; }

        //public IList<EquipamentoUnidadeEquipamentoViewModel> EquipamentosUnidadeEquipamento { get; set; }

        public EquipamentoUnidadeViewModel()
        {
            DataInsercao = new DateTime();
        }

        public EquipamentoUnidadeViewModel(EquipamentoUnidade equipamentoUnidade)
        {
            Id = equipamentoUnidade.Id;
            DataInsercao = equipamentoUnidade?.DataInsercao ?? DateTime.Now;
            GerarNotificacao = equipamentoUnidade.GerarNotificacao;
            Observacao = equipamentoUnidade.Observacao;
            ConferenciaConcluida = equipamentoUnidade.ConferenciaConcluida;
            Unidade = new UnidadeViewModel(equipamentoUnidade?.Unidade ?? new Unidade());
            PeriodoEquipamentoUnidade = equipamentoUnidade.PeriodoEquipamentoUnidade;
            UltimaConferencia = equipamentoUnidade.UltimaConferencia;
            Usuario = equipamentoUnidade.Usuario;
            //EquipamentosUnidadeEquipamento = equipamentoUnidade.EquipamentosUnidadeEquipamento.Select(x => new EquipamentoUnidadeEquipamentoViewModel(x)).ToList() ?? new List<EquipamentoUnidadeEquipamentoViewModel>();
        }

        public EquipamentoUnidade ToEntity()
        {
            return new EquipamentoUnidade
            {
                Id = Id,
                DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                GerarNotificacao = GerarNotificacao,
                Observacao = Observacao,
                ConferenciaConcluida = ConferenciaConcluida,
                Unidade = Unidade?.ToEntity(),
                PeriodoEquipamentoUnidade = PeriodoEquipamentoUnidade,
                UltimaConferencia = UltimaConferencia,
                Usuario = Usuario
            //EquipamentosUnidadeEquipamento = EquipamentosUnidadeEquipamento.Select(x => x.ToEntity()).ToList() ?? new List<EquipamentoUnidadeEquipamento>()
        };
        }

        public EquipamentoUnidadeViewModel ToViewModel(EquipamentoUnidade equipamentoUnidade)
        {
            return new EquipamentoUnidadeViewModel
            {
                Id = equipamentoUnidade.Id,
                DataInsercao = equipamentoUnidade.DataInsercao,
                GerarNotificacao = equipamentoUnidade.GerarNotificacao,
                Observacao = equipamentoUnidade.Observacao,
                ConferenciaConcluida = equipamentoUnidade.ConferenciaConcluida,
                Unidade = new UnidadeViewModel(equipamentoUnidade?.Unidade ?? new Unidade()),
                PeriodoEquipamentoUnidade = equipamentoUnidade.PeriodoEquipamentoUnidade,
                UltimaConferencia = equipamentoUnidade.UltimaConferencia,
                Usuario = equipamentoUnidade.Usuario
            //EquipamentosUnidadeEquipamento = equipamentoUnidade.EquipamentosUnidadeEquipamento.Select(x => new EquipamentoUnidadeEquipamentoViewModel(x)).ToList() ?? new List<EquipamentoUnidadeEquipamentoViewModel>()
        };
           
        }
    }
}
