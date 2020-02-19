using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ControlePontoDia : BaseEntity
    {
        public virtual DateTime Data { get; set; }
        public virtual bool Folga { get; set; }
        public virtual bool Falta { get; set; }
        public virtual bool Atraso { get; set; }
        public virtual bool Suspensao { get; set; }
        public virtual bool Atestado { get; set; }
        public virtual bool FaltaJustificada { get; set; }
        public virtual bool AtrasoJustificado { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string HorarioEntrada { get; set; }
        public virtual string HorarioSaidaAlmoco { get; set; }
        public virtual string HorarioRetornoAlmoco { get; set; }
        public virtual string HorarioSaida { get; set; }
        public virtual TimeSpan HorasDiaTime { get; set; }
        public virtual string HorasDia { get; set; }
        public virtual string HoraExtra { get; set; }
        public virtual string HoraAtraso { get; set; }
        public virtual string AdicionalNoturno { get; set; }
        public virtual IList<ControlePontoUnidadeApoio> UnidadesApoio { get; set; }
    }
}