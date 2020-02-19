using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class HorarioUnidadeNotificacaoRepositorio : NHibRepository<HorarioUnidadeNotificacao>, IHorarioUnidadeNotificacaoRepositorio 
    {
        public HorarioUnidadeNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}