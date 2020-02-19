using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class TabelaPrecoAvulso : BaseEntity, IAudit
    {
        public virtual string Nome { get; set; }
        public virtual int Numero { get; set; }
        public virtual int TempoToleranciaPagamento { get; set; }
        public virtual int TempoToleranciaDesistencia { get; set; }
        public virtual StatusSolicitacao Status { get; set; }
        public virtual bool HoraAdicional { get; set; }
        public virtual bool Padrao { get; set; }
        public virtual int QuantidadeHoraAdicional { get; set; }
        public virtual decimal ValorHoraAdicional { get; set; }
        public virtual string DescricaoHoraValor { get; set; }
        public virtual string HoraInicioVigencia { get; set; }
        public virtual string HoraFimVigencia { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual IList<TabelaPrecoAvulsoPeriodo> ListaPeriodo { get; set; }
        public virtual IList<TabelaPrecoAvulsoHoraValor> ListaHoraValor { get; set; }
        public virtual IList<TabelaPrecoAvulsoUnidade> ListaUnidade { get; set; }

        public virtual IList<TabelaPrecoAvulsoNotificacao> Notificacoes { get; set; }
    }
}