using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Entidade.Base;

namespace Entidade
{
    public class PessoaEndereco : BaseEntity
    {
        public virtual Endereco Endereco { get; set; }
        public int Pessoa { get; set; }
    }
}