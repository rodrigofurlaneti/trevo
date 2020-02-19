using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entidade
{
    public class HorarioUnidade : BaseEntity
    {
        [Required(ErrorMessage = "Nome é requerido!")]
        public virtual string Nome { get; set; }
        public virtual bool Fixo { get; set; }
        public virtual DateTime DataValidade { get; set; }
        public virtual TipoHorario TipoHorario { get; set; }
        public virtual bool Feriados { get; set; }
        public virtual StatusHorario Status  { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual IList<HorarioUnidadePeriodoHorario> PeriodosHorario { get; set; }

        public virtual List<int> ListaTipoHorario
        {
            get
            {
                var retorno = new List<int>();

                if (PeriodosHorario != null)
                    retorno.AddRange(PeriodosHorario.Select(x=> (int)x.PeriodoHorario.TipoHorario));

                return ListaIds != null && ListaIds.Any()
                    ? ListaIds
                    : retorno;
            }
            set { ListaIds = value; }
        }

        private List<int> ListaIds { get; set; }

        public HorarioUnidade()
        {
            DataValidade = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}