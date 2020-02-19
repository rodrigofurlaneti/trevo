using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IPropostaRepositorio : IRepository<Proposta>
    {
        int RetornaNumeroPropostaDisponivel();
        IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade);
        bool PropostaExistente(Proposta proposta);
    }
}