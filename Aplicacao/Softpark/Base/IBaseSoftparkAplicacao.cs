using Aplicacao.ViewModels;
using System.Collections.Generic;

namespace Aplicacao.Softpark.Base
{
    public interface IBaseSoftparkAplicacao<T> where T : IBaseSoftparkViewModel
    {
        string Tela { get; }

        string Salvar(T vm);
        void SalvarOuEditar(T vm);
        T BuscarPorId(int id);
        IEnumerable<T> Listar();
        string Editar(T vm);
        string ExcluirPorId(int id);
        TokenSoftparkViewModel BuscarToken();
    }
}
