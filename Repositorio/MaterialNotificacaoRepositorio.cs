using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MaterialNotificacaoRepositorio : NHibRepository<MaterialNotificacao>, IMaterialNotificacaoRepositorio
    {
        public MaterialNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}