using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IOcorrenciaRepositorio : IRepository<OcorrenciaCliente>
    {
        IList<OcorrenciaCliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int quantidadeRegistros);
        IList<OcorrenciaCliente> BuscarDadosGrid(string protocolo, string nome, string status, out int quantidadeRegistros, int pagina = 1, int take = 50);
    }
}
