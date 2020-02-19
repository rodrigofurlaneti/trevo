using System.Collections.Generic;
using System.Linq;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface IFilialServico : IBaseServico<Filial>
    {
    }

    public class FilialServico : BaseServico<Filial, IFilialRepositorio>, IFilialServico
    {

    }
}
