using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class VagaCortesiaVigencia : BaseEntity
    {
        [Required]
        public virtual Unidade Unidade { get; set; }

        public virtual DateTime DataInicio { get; set; }
        public virtual DateTime DataFim { get; set; }

        public virtual string HorarioInicio { get; set; }
        public virtual string HorarioFim { get; set; }

        public virtual VagaCortesia VagaCortesia { get; set; }

        public VagaCortesiaVigencia()
        {
        }
    }
}