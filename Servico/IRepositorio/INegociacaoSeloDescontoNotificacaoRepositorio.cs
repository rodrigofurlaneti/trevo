using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface INegociacaoSeloDescontoNotificacaoRepositorio : IRepository<NegociacaoSeloDescontoNotificacao>
    {
        void Criar(PedidoSelo pedido, int idUsuario);
    }
}