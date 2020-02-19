using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CotacaoRepositorio : NHibRepository<Cotacao>, ICotacaoRepositorio
    {
        public CotacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}