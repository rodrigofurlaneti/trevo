using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Oficina : BaseEntity
    {
        public virtual Pessoa Pessoa { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual bool IndicadaPeloCliente { get; set; }
        public virtual string NomeCliente { get; set; }
        public virtual Contato CelularCliente { get; set; }
        public virtual string Nome => !string.IsNullOrEmpty(Pessoa?.Nome) ? Pessoa?.Nome : NomeFantasia;
    }
}