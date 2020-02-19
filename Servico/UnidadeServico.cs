using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IUnidadeServico: IBaseServico<Unidade>
    {
        IList<Unidade> BuscarPorConvenio(int idConvenio);
        IList<Unidade> ListaUnidade(int? idConvenio = null);
        List<Unidade> ListarOrdenadoSimplificado();
    }

    public class UnidadeServico : BaseServico<Unidade, IUnidadeRepositorio>, IUnidadeServico
    {
        public readonly IUnidadeRepositorio _unidadeRepositorio;

        public UnidadeServico(IUnidadeRepositorio unidadeRepositorio)
        {
            _unidadeRepositorio = unidadeRepositorio;
        }

        public IList<Unidade> BuscarPorConvenio(int idConvenio)
        {
            if (idConvenio == 0)
                return new List<Unidade>();

            return _unidadeRepositorio.BuscarPorConvenio(idConvenio);
        }

        public IList<Unidade> ListaUnidade(int? idConvenio = null)
        {
            if (idConvenio.HasValue)
                return BuscarPorConvenio(idConvenio.Value);

            return _unidadeRepositorio.List();
        }

        public override void ExcluirPorId(int id)
        {
            var unidade = _unidadeRepositorio.GetById(id);
            unidade.CheckListAtividade = null;
            _unidadeRepositorio.Save(unidade);

            unidade = _unidadeRepositorio.GetById(id);

            _unidadeRepositorio.Delete(unidade);
        }

        public List<Unidade> ListarOrdenadoSimplificado()
        {
            return _unidadeRepositorio.ListarOrdenadoSimplificado();
        }
    }
}