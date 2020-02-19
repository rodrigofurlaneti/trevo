using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IDepartamentoServico : IBaseServico<Departamento>
    {
    }

    public class DepartamentoServico : BaseServico<Departamento, IDepartamentoRepositorio>, IDepartamentoServico
    {
    }
}