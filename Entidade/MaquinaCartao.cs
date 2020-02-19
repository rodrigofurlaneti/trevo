using Entidade.Base;

namespace Entidade
{
    public class MaquinaCartao : BaseEntity
    {
        public virtual int NumeroMaquina { get; set; }
        public virtual string Observacao { get; set; }
        public virtual string MarcaMaquina { get; set; }
        public virtual Documento CNPJ { get; set; }
        public virtual Funcionario Responsavel { get; set; }
    }
}
