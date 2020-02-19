using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IClienteServico : IBaseServico<Cliente>
    {
        IList<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina);
        IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50);
    }

    public class ClienteServico : BaseServico<Cliente, IClienteRepositorio>, IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteServico(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            return _clienteRepositorio.BuscarDadosGrid(documento, nome, contrato, out quantidadeRegistros, pagina, take);
        }

        public IList<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina)
        {
            return _clienteRepositorio.BuscarPorIntervaloOrdenadoPorNome(registroInicial, registrosPorPagina);
        }
    }
}