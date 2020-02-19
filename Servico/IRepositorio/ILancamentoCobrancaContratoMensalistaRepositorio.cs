using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface ILancamentoCobrancaContratoMensalistaRepositorio : IRepository<LancamentoCobrancaContratoMensalista>
    {
        LancamentoCobrancaContratoMensalista RetornaUltimoLancamentoCobrancaPor(int id);
    }
}