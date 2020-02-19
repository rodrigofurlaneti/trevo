using Entidade;
using Entidade.Uteis;
namespace Aplicacao.ViewModels
{
    public class UnidadeFuncionarioViewModel
    {
        public int Unidade { get; set; }
        public MaquinaCartaoViewModel MaquinaCartao { get; set; }
        public Funcao Funcao { get; set; }
        public FuncionarioViewModel Funcionario { get; set; }
        
        public UnidadeFuncionarioViewModel()
        {

        }

        public UnidadeFuncionarioViewModel(UnidadeFuncionario unidadeFuncionario)
        {
            Unidade = unidadeFuncionario.Unidade;
            MaquinaCartao = new MaquinaCartaoViewModel(unidadeFuncionario?.MaquinaCartao ?? new MaquinaCartao());
            Funcao = unidadeFuncionario.Funcao;
            Funcionario = new FuncionarioViewModel(unidadeFuncionario?.Funcionario ?? new Funcionario());
        }

        public UnidadeFuncionario ToEntity()
        {
            return new UnidadeFuncionario
            {
                Unidade = Unidade,
                MaquinaCartao = MaquinaCartao?.ToEntity(),
                Funcao = Funcao,
                Funcionario = Funcionario?.ToEntity()
            };
        }

        public UnidadeFuncionarioViewModel ToViewModel(UnidadeFuncionario unidadeFuncionario)
        {
            return new UnidadeFuncionarioViewModel
            {
                Unidade = unidadeFuncionario.Unidade,
                MaquinaCartao =new MaquinaCartaoViewModel(unidadeFuncionario?.MaquinaCartao ?? new MaquinaCartao()),
                Funcao = unidadeFuncionario.Funcao,
                Funcionario = new FuncionarioViewModel(unidadeFuncionario?.Funcionario ?? new Funcionario())
            };
        }
    }
}
