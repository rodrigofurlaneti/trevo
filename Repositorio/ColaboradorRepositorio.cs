using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;


namespace Repositorio
{
    public class ColaboradorRepositorio : NHibRepository<Colaborador>, IColaboradorRepositorio
    {
        public ColaboradorRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
