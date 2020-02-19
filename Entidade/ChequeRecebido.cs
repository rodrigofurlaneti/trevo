using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ChequeRecebido :  BaseEntity
    {
        public ChequeRecebido()
        {
            DataInsercao = DateTime.Now;
        }
        [Required(ErrorMessage = "*")]
        public virtual long Numero { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Agencia { get; set; }

        public virtual string DigitoAgencia { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Conta { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string DigitoConta { get; set; }

        public virtual string Cpf { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Emitente { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual decimal Valor { get; set; }

        public virtual DateTime? DataDeposito { get; set; }

        public virtual DateTime? DataProtesto { get; set; }

        public virtual DateTime? DataDevolucao { get; set; }

        public virtual string CartorioProtestado { get; set; }

        public virtual StatusCheque StatusCheque { get; set; }
        public virtual Cliente Cliente { get; set; }

        public virtual IList<ChequeRecebidoLancamentoCobranca> ListaLancamentoCobranca { get; set; }

        public virtual Banco Banco { get; set; }
    }
}
