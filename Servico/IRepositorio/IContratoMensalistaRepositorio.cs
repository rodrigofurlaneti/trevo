using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IContratoMensalistaRepositorio : IRepository<ContratoMensalista>
    {
        void Salvar(ContratoMensalista contratoMensalista);
        IList<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros);
        IList<ContratoMensalista> BuscarPorCliente(int idCliente);
    }
}
