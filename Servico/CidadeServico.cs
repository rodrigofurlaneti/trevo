using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICidadeServico : IBaseServico<Cidade>
    {
        Cidade BuscarPelaDescricao(string descricao, string uf);
    }

    public class CidadeServico : BaseServico<Cidade, ICidadeRepositorio>, ICidadeServico
    {
        private readonly ICidadeRepositorio _cidadeRepositorio;
        private readonly IEstadoRepositorio _estadoRepositorio;

        public CidadeServico(ICidadeRepositorio cidadeRepositorio, IEstadoRepositorio estadoRepositorio)
        {
            _cidadeRepositorio = cidadeRepositorio;
            _estadoRepositorio = estadoRepositorio;
        }

        public Cidade BuscarPelaDescricao(string descricao, string uf)
        {
            if (string.IsNullOrEmpty(descricao))
                return null;

            var cidade = _cidadeRepositorio.FirstBy(x => x.Descricao == descricao);
            if (cidade != null)
                return cidade;

            if (!string.IsNullOrEmpty(uf))
            {
                var estado = _estadoRepositorio.FirstBy(x => x.Sigla.ToLower() == uf.ToLower());
                var novaCidade = new Cidade { Descricao = descricao, Estado = estado };
                _cidadeRepositorio.Save(novaCidade);
                return novaCidade;
            }
            else
            {
                throw new BusinessRuleException("Informe o estado");
            }
        }
    }
}