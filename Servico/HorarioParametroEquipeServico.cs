using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IHorarioParametroEquipeServico : IBaseServico<HorarioParametroEquipe>
    {
    }

    public class HorarioParametroEquipeServico : BaseServico<HorarioParametroEquipe, IHorarioParametroEquipeRepositorio>, IHorarioParametroEquipeServico
    {
    }
}