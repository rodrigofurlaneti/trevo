using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IHorarioPrecoServico : IBaseServico<HorarioPreco>
    {

    }
    public class HorarioPrecoServico : BaseServico<HorarioPreco,IHorarioPrecoRepositorio>,IHorarioPrecoServico
    {
    }
}
