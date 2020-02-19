using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IPedidoSeloNotificacaoRepositorio : IRepository<PedidoSeloNotificacao>
    {
        void Criar(PedidoSelo pedido, int idUsuario);
        void CriarNotificacaoBloqueio(PedidoSelo pedido, int idUsuario);
        PedidoSelo BuscarPorIdNotificacao(int idNotificacao);
    }
}