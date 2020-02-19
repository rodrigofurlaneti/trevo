using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
namespace Dominio
{
    public interface IColaboradorServico : IBaseServico<Colaborador>
    {

    }
    public class ColaboradorServico : BaseServico<Colaborador,IColaboradorRepositorio>,IColaboradorServico
    {

    }
}
