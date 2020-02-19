using Entidade.Base;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class PeriodoHorario : BaseEntity
    {
        public virtual int TipoHorario { get; set; }
        public virtual string Periodo { get; set; }
        public virtual string Inicio { get; set; }
        public virtual string Fim { get; set; }
    }
}