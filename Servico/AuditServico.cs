using Dominio.Base;
using Dominio.IRepositorio;
using Entidade.Base;

namespace Dominio
{
    public interface IAuditServico : IBaseServico<Audit>
    {
    }

    public class AuditServico : BaseServico<Audit, IAuditRepositorio>, IAuditServico
    {
    }
}