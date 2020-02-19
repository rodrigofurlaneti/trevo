using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Linq;

namespace Dominio
{
    public interface IOcorrenciaFuncionarioServico : IBaseServico<OcorrenciaFuncionario>
    {

    }

    public class OcorrenciaFuncionarioServico : BaseServico<OcorrenciaFuncionario, IOcorrenciaFuncionarioRepositorio>, IOcorrenciaFuncionarioServico
    {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public OcorrenciaFuncionarioServico(IFuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public override void Salvar(OcorrenciaFuncionario ocorrenciaFuncionario)
        {
            var funcionario = _funcionarioRepositorio.GetById(ocorrenciaFuncionario.Funcionario.Id);
            funcionario.OcorrenciaFuncionario = ocorrenciaFuncionario;

            if (ocorrenciaFuncionario.OcorrenciaFuncionarioDetalhes.Any())
            {
                _funcionarioRepositorio.Save(funcionario);
            }
            else if (ocorrenciaFuncionario.Id > 0)
            {
                Repositorio.Clear();
                Repositorio.DeleteById(ocorrenciaFuncionario.Id);
            }
        }
    }
}