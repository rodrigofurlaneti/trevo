using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IPedidoLocacaoRepositorio : IRepository<PedidoLocacao>
    {
        IList<PedidoLocacao> ListarPedidoLocacaoFiltro(int? idUnidade, int? idTipoLocacao);
    }
}