using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Collections.Generic;

namespace Dominio
{
    public interface IPropostaServico : IBaseServico<Proposta>
    {
        int RetornaNumeroPropostaDisponivel();
        IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade);
    }

    public class PropostaServico : BaseServico<Proposta, IPropostaRepositorio>, IPropostaServico
    {
        private readonly IPropostaRepositorio _propostaRepositorio;

        public PropostaServico(IPropostaRepositorio propostaRepositorio)
        {
            _propostaRepositorio = propostaRepositorio;
        }

        public int RetornaNumeroPropostaDisponivel()
        {
            return _propostaRepositorio.RetornaNumeroPropostaDisponivel();
        }

        public IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade)
        {
            if (idCliente == 0 || idUnidade == 0)
                return new List<Proposta>();

            return _propostaRepositorio.BuscarPorClienteUnidade(idCliente, idUnidade);
        }

        public override void Salvar(Proposta proposta)
        {
            if (_propostaRepositorio.PropostaExistente(proposta))
                throw new BusinessRuleException("Já existe uma proposta para esse Cliente e Filial");

            _propostaRepositorio.Save(proposta);
        }
    }
}