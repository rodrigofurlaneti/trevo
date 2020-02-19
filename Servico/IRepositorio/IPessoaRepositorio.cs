using Entidade;
using Dominio.IRepositorio.Base;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IPessoaRepositorio : IRepository<Pessoa>
    {
        List<Pessoa> PesquisarComFiltro(string nome, string CPF);
        List<Pessoa> PesquisarComFiltroNaoDevedores(string nome, string CPF);
    }
}
