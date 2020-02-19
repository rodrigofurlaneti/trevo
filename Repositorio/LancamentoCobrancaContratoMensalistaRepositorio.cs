using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Linq;

namespace Repositorio
{
    public class LancamentoCobrancaContratoMensalistaRepositorio : NHibRepository<LancamentoCobrancaContratoMensalista>, ILancamentoCobrancaContratoMensalistaRepositorio
    {
        public LancamentoCobrancaContratoMensalistaRepositorio(NHibContext context)
            : base(context)
        {
        }

        public LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id)
        {
            return Session.GetListBy<LancamentoCobrancaContratoMensalista>(x => x.ContratoMensalista.Id == id)
                ?.OrderByDescending(x => x.DataInsercao)
                ?.FirstOrDefault();
        }
    }
}