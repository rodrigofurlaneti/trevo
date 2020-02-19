using Entidade;

namespace Aplicacao.ViewModels
{
    public class DepartamentoFuncionarioViewModel
    {
        public virtual DepartamentoViewModel Departamento { get; set; }
        public virtual FuncionarioViewModel Funcionario { get; set; }

        public DepartamentoFuncionarioViewModel()
        {
        }

        public DepartamentoFuncionarioViewModel(DepartamentoFuncionario departamentoFuncionario)
        {
            Funcionario = new FuncionarioViewModel(departamentoFuncionario.Funcionario);
        }

        public DepartamentoFuncionario ToEntity() => new DepartamentoFuncionario
        {
            Funcionario = this.Funcionario.ToEntity()
        };
    }
}