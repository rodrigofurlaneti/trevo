using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class CalculoDetalhe : BaseEntity
    {
        public virtual int FaixaAtrasoDe { get; set; }
        public virtual int FaixaAtrasoAte { get; set; }
        public virtual decimal ValPrincipalDe { get; set; }
        public virtual decimal ValPrincipalAte { get; set; }
        public virtual decimal ValEntradaDe { get; set; }
        public virtual decimal ValEntradaAte { get; set; }
        public virtual decimal TaxaAdm { get; set; }
        public virtual decimal TaxaExtra { get; set; }
        public virtual decimal DescPrincipal { get; set; }
        public virtual decimal DescJuros { get; set; }
        public virtual decimal Multa { get; set; }
        public virtual decimal Iof { get; set; }
        public virtual decimal Outros { get; set; }
        public virtual bool ParcelaIguais { get; set; }
        public virtual int QtdRenovacoes { get; set; }
        public virtual bool AutEspecial { get; set; }
        public virtual TipoJuros TipoJuros { get; set; }

    }
}