using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IMensalistaServico : IBaseServico<Mensalista>
    {

    }
    public class MensalistaServico : BaseServico<Mensalista,IMensalistaRepositorio>,IMensalistaServico
    {

    }
}
