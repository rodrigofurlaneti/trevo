using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoBeneficioServico : IBaseServico<TipoBeneficio>
    {
    }

    public class TipoBeneficioServico : BaseServico<TipoBeneficio, ITipoBeneficioRepositorio>, ITipoBeneficioServico
    {
    }
}