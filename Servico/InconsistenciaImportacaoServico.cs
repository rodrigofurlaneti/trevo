using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IInconsistenciaImportacaoServico : IBaseServico<InconsistenciaImportacao>
    {
        void Clear();
    }

    public class InconsistenciaImportacaoServico : BaseServico<InconsistenciaImportacao, IInconsistenciaImportacaoRepositorio>, IInconsistenciaImportacaoServico
    {
        private readonly IInconsistenciaImportacaoRepositorio _inconsistenciaImportacaoRepositorio;
        
        public InconsistenciaImportacaoServico(IInconsistenciaImportacaoRepositorio inconsistenciaImportacaoRepositorio)
        {
            _inconsistenciaImportacaoRepositorio = inconsistenciaImportacaoRepositorio;
        }
    
        public void Clear()
        {
            _inconsistenciaImportacaoRepositorio.Clear();
        }
    }
}