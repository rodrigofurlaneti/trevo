using Entidade.Base;

namespace Entidade
{
    public class OISFuncionario
    {
        public virtual OIS OIS { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}