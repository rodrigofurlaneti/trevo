using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPermissaoAplicacao : IBaseAplicacao<Permissao>
    {
        IList<Permissao> BuscarPermissoes();
    }

    public class PermissaoAplicacao : BaseAplicacao<Permissao, IPermissaoServico>, IPermissaoAplicacao
    {
        public IList<Permissao> BuscarPermissoes()
        {
            return BuscarPor(x => !x.Regra.Equals("root"));
        }
    }
}