using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IUnidadeCondominoServico : IBaseServico<UnidadeCondomino>
    {
    }

    public class UnidadeCondominoServico : BaseServico<UnidadeCondomino, IUnidadeCondominoRepositorio>, IUnidadeCondominoServico
    {

        private readonly IClienteCondominoRepositorio _clienteCondominoRepositorio;

        public UnidadeCondominoServico(IClienteCondominoRepositorio clienteCondominoRepositorio)
        {
            _clienteCondominoRepositorio = clienteCondominoRepositorio;
        }

        public new void ExcluirPorId(int id)
        {
            var objClienteCondomino = _clienteCondominoRepositorio.ListBy(x => x.Unidade.Id == id);

            if (objClienteCondomino != null && objClienteCondomino.Count > 0)
            {
                throw new BusinessRuleException("Existem Clientes Condômino alocados nestas vagas!");
            }

            Repositorio.DeleteById(id);
        }
    }
}