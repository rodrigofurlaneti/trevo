using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{

    public interface IChequeRecebidoServico : IBaseServico<ChequeRecebido>
    {
        IList<ChequeRecebido> BuscarDadosSimples();
    }

    public class ChequeRecebidoServico : BaseServico<ChequeRecebido,IChequeRecebidoRepositorio>, IChequeRecebidoServico
    {
        private readonly IChequeRecebidoRepositorio _chequeRecebidoRepositorio;

        public ChequeRecebidoServico(IChequeRecebidoRepositorio chequeRecebidoRepositorio)
        {
            _chequeRecebidoRepositorio = chequeRecebidoRepositorio;
        }

        public IList<ChequeRecebido> BuscarDadosSimples()
        {
            return _chequeRecebidoRepositorio.BuscarDadosSimples();
        }
    }
}
