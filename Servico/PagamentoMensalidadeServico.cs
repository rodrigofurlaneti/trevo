using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{

    public interface IPagamentoMensalidadeServico : IBaseServico<PagamentoMensalidade>
    {

    }
    public class PagamentoMensalidadeServico : BaseServico<PagamentoMensalidade,IPagamentoMensalidadeRepositorio>, IPagamentoMensalidadeServico
    {

    }
}
