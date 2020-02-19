using Entidade.Uteis;

namespace Entidade
{
    public class Fornecedor : Pessoa
    {
        public virtual string NomeFantasia { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual bool ReceberCotacaoPorEmail { get; set; }

        public virtual Banco Banco { get; set; }
        public virtual string Agencia { get; set; }
        public virtual string DigitoAgencia { get; set; }
        public virtual string Conta { get; set; }
        public virtual string DigitoConta { get; set; }
        public virtual string CPFCNPJ { get; set; }
        public virtual string Beneficiario { get; set; }

        public virtual string Descricao
        {
            get
            {
                return string.IsNullOrEmpty(NomeFantasia) ? Nome : NomeFantasia;
            }
        }
    }
}
