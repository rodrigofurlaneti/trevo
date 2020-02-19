using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoBeneficioRepositorio : NHibRepository<TipoBeneficio>, ITipoBeneficioRepositorio
    {
        public TipoBeneficioRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}