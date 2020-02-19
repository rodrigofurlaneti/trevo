using Dominio.IRepositorio.Base;
using Entidade;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IFuncionarioRepositorio : IRepository<Funcionario>
    {
        List<Funcionario> BuscarComDadosSimples();
    }
}