using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IMaquinaCartaoServico : IBaseServico<MaquinaCartao>
    {

    }

    public class MaquinaCartaoServico : BaseServico<MaquinaCartao,IMaquinaCartaoRepositorio>, IMaquinaCartaoServico
    {
    }
}
