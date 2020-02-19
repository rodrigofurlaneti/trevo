using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IEmpresaServico : IBaseServico<Empresa>
    {
    }

    public class EmpresaServico : BaseServico<Empresa, IEmpresaRepositorio>, IEmpresaServico
    {

    }
}