using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Linq;

namespace Dominio
{
    public interface IBeneficioFuncionarioServico : IBaseServico<BeneficioFuncionario>
    {
    }

    public class BeneficioFuncionarioServico : BaseServico<BeneficioFuncionario, IBeneficioFuncionarioRepositorio>, IBeneficioFuncionarioServico
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public BeneficioFuncionarioServico(IFuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public override void Salvar(BeneficioFuncionario beneficioFuncionario)
        {
            var funcionario = _funcionarioRepositorio.GetById(beneficioFuncionario.Funcionario.Id);
            funcionario.BeneficioFuncionario = beneficioFuncionario;
            beneficioFuncionario.Funcionario = funcionario;

            if (beneficioFuncionario.BeneficioFuncionarioDetalhes.Any())
            {
                _funcionarioRepositorio.Save(funcionario);
            }
            else if(beneficioFuncionario.Id > 0)
            {
                Repositorio.Clear();
                Repositorio.DeleteById(beneficioFuncionario.Id);
            }
        }
    }
}