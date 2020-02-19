using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Funcionamento : BaseEntity
    {
        public Funcionamento()
        {
            HorariosPrecos = new List<HorarioPreco>();
        }
        public virtual string Nome { get; set; }
        public virtual int? CodFuncionamento { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual IList<HorarioPreco> HorariosPrecos { get; set; }
    }
}
