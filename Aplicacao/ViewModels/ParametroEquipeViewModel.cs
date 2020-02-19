using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ParametroEquipeViewModel
    {
        public int Id { get; set; }
        public Equipe Equipe { get; set; }
        public bool Ativo { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string Usuario { get; set; }
        public IList<HorarioParametroEquipeViewModel> HorarioParametroEquipe { get; set; }

        public IList<ParametroEquipeNotificacaoViewModel> Notificacoes { get; set; }

        //private List<int> ListaIds { get; set; }
        //public List<int> ListaTipoHorario
        //{
        //    get
        //    {

        //        var retorno = new List<int>();

        //        if (PeriodosHorario != null)
        //            retorno.AddRange(PeriodosHorario.Select(x => (int)x.PeriodoHorario.TipoHorario));

        //        return ListaIds != null && ListaIds.Any()
        //                ? ListaIds
        //                : retorno;
        //    }
        //    set { ListaIds = value; }
        //}

        public ParametroEquipeViewModel()
        {

        }

        public ParametroEquipeViewModel(ParametroEquipe ParametroEquipe)
        {
            Id = ParametroEquipe.Id;
            Equipe = ParametroEquipe.Equipe;
            Ativo = ParametroEquipe.Ativo;
            Status = ParametroEquipe.Status;
            Usuario = ParametroEquipe.Usuario;
            HorarioParametroEquipe = ParametroEquipe.HorarioParametroEquipe.Select(x => new HorarioParametroEquipeViewModel(x)).ToList();
            Notificacoes = ParametroEquipe?.Notificacoes?.Select(x => new ParametroEquipeNotificacaoViewModel()).ToList();
        }

        public ParametroEquipe ToEntity()
        {
            var entidade = new ParametroEquipe
            {
                Id = Id,
                Equipe = Equipe,
                Ativo = Ativo,
                Status = Status,
                Usuario = Usuario,
                HorarioParametroEquipe = HorarioParametroEquipe?.Select(x => x.ToEntity())?.ToList() ?? new List<HorarioParametroEquipe>(),
            };

            return entidade;
        }
    }
}


