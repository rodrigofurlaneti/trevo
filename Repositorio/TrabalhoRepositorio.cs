using System.Collections.Generic;
using System.Linq;
using Dominio;
using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TrabalhoRepositorio : NHibRepository<Trabalho>, ITrabalhoRepositorio
    {
        public TrabalhoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}