using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class UnidadeFuncionario : BaseEntity
    {
        public virtual int Unidade { get; set; }
        public virtual MaquinaCartao MaquinaCartao { get; set; }
        public virtual Funcao Funcao { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
