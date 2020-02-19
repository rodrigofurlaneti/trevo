using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPecaServicoServico : IBaseServico<PecaServico>
    {
    }

    public class PecaServicoServico : BaseServico<PecaServico, IPecaServicoRepositorio>, IPecaServicoServico
    {
        
    }
}