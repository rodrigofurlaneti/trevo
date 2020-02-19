using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IEmissaoSeloRepositorio : IRepository<EmissaoSelo>
    {
        List<int> RetornaListaIdPedidoSeloDasEmissoesDeSelo(List<int> listaIdPedido);
        int RetornaProximoNumeroLote();
        IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(StatusSelo? status, int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao);
    }
}