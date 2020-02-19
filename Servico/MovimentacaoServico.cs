using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IMovimentacaoServico : IBaseServico<Movimentacao>
    {
        List<MovimentacaoSelo> BuscarMovimentacoesSelosPelosIds(List<int> ids);
    }

    public class MovimentacaoServico : BaseServico<Movimentacao, IMovimentacaoRepositorio>, IMovimentacaoServico
    {
        private readonly IMovimentacaoSeloRepositorio _movimentacaoSeloRepositorio;

        public MovimentacaoServico(IMovimentacaoSeloRepositorio movimentacaoSeloRepositorio)
        {
            _movimentacaoSeloRepositorio = movimentacaoSeloRepositorio;
        }

        public List<MovimentacaoSelo> BuscarMovimentacoesSelosPelosIds(List<int> ids)
        {
            return _movimentacaoSeloRepositorio.ListBy(x => x.IdSoftpark.HasValue && ids.Contains(x.IdSoftpark.Value)).ToList();
        }
    }
}
