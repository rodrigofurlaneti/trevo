using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MaquinaCartaoRepositorio : NHibRepository<MaquinaCartao>,IMaquinaCartaoRepositorio
    {
        public MaquinaCartaoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
