namespace Entidade
{
    public class DepartamentoFuncionario
    {
        public virtual Departamento Departamento { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}