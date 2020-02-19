using Aplicacao.Base;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao
{
    public interface IMovimentacaoAplicacao : IBaseAplicacao<Movimentacao>
    {
        List<MovimentacaoSelo> BuscarMovimentacoesSelosPelosIds(List<int> ids);
    }

    public class MovimentacaoAplicacao : BaseAplicacao<Movimentacao, IMovimentacaoServico>, IMovimentacaoAplicacao
    {
        private readonly IMovimentacaoServico _movimentacaoServico;

        public MovimentacaoAplicacao(IMovimentacaoServico movimentacaoServico)
        {
            _movimentacaoServico = movimentacaoServico;
        }

        public List<MovimentacaoSelo> BuscarMovimentacoesSelosPelosIds(List<int> ids)
        {
            return _movimentacaoServico.BuscarMovimentacoesSelosPelosIds(ids);
        }
    }
}
