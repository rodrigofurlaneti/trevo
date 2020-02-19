using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IClienteRepositorio : IRepository<Cliente>
    {
        IList<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int quantidadeRegistros);
        IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50);
    }
}