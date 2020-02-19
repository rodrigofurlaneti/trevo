using Entidade.Base;
using System;

namespace Entidade
{
    public class HorarioPreco : BaseEntity
    {
        public virtual string Horario { get; set; }
        public virtual decimal Valor { get; set; }
    }
}
