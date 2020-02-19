using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class SeloClienteRepositorio : NHibRepository<SeloCliente>, ISeloClienteRepositorio
    {
        public SeloClienteRepositorio(NHibContext context)
           : base(context)
        {
        }
    }
}
