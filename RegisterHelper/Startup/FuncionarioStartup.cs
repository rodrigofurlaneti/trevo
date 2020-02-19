using System.Collections.Generic;
using Aplicacao;
using Entidade;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class FuncionarioStartup
    {
        #region Private Members
        #endregion

        #region Public Members

        public static void Start()
        {
            AdicionaFuncionarioRoot();
        }

        private static void AdicionaFuncionarioRoot()
        {
            var _pessoaAplicacao = ServiceLocator.Current.GetInstance<IPessoaAplicacao>();
            var _funcionarioAplicacao = ServiceLocator.Current.GetInstance<IFuncionarioAplicacao>();
            var _cargoAplicacao = ServiceLocator.Current.GetInstance<ICargoAplicacao>();
            var funcionarioRoot = _funcionarioAplicacao.PrimeiroPor(x => x.Pessoa.Nome.Equals("Administrador"));

            if (funcionarioRoot == null)
            {
                var pessoaRoot = _pessoaAplicacao.PrimeiroPor(x => x.Nome.Equals("Administrador"));
                var cargo = _cargoAplicacao.PrimeiroPor(x => true);

                funcionarioRoot = new Funcionario
                {
                    Pessoa = pessoaRoot,
                    Cargo = cargo,
                    Status = Entidade.Uteis.StatusFuncionario.Ativo
                };
                _funcionarioAplicacao.Salvar(funcionarioRoot);
            }
        }

        #endregion
    }
}