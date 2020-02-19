using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class EquipamentoUnidade : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual bool GerarNotificacao { get; set; }
        public virtual string Observacao { get; set; }
        public virtual bool ConferenciaConcluida { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual PeriodoDiasEquipamentoUnidade PeriodoEquipamentoUnidade { get; set; }
        public virtual DateTime? UltimaConferencia { get; set; }
        public virtual string Usuario { get; set; }

        //public virtual IList<EquipamentoUnidadeEquipamento> EquipamentosUnidadeEquipamento { get; set; }
    }
}
