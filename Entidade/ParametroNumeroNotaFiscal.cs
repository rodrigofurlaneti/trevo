using Entidade.Base;

namespace Entidade
{
    public class ParametroNumeroNotaFiscal :BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual string ValorMaximoNota { get; set; }
        public virtual string ValorMaximoNotaDia { get; set; }
    }
}
