using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class ContaCorrenteClienteDetalhe : BaseEntity
    {
        public virtual TipoOperacaoContaCorrente TipoOperacaoContaCorrente { get; set; }
        public DateTime DataCompetencia { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual ContratoMensalista ContratoMensalista { get; set; }
    }
}