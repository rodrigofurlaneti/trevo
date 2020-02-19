using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MaterialFornecedorRepositorio : NHibRepository<MaterialFornecedor>, IMaterialFornecedorRepositorio
    {
        public MaterialFornecedorRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}