using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class PropostaRepositorio : NHibRepository<Proposta>, IPropostaRepositorio
    {
        public PropostaRepositorio(NHibContext context)
            : base(context)
        {
        }

        public int RetornaNumeroPropostaDisponivel()
        {
            var sql = $@"SELECT MAX(ID) FROM Proposta;";
            return Convert.ToInt32(Session.CreateSQLQuery(sql).UniqueResult()) + 1;
        }

        public IList<Proposta> BuscarPorClienteUnidade(int idCliente, int idUnidade)
        {
            return Session
                .GetListBy<Proposta>(x => x.Cliente.Id == idCliente && x.Unidade.Id == idUnidade)
                .ToList();
        }

        public bool PropostaExistente(Proposta proposta)
        {
            var propostaExistente = Session.GetItemBy<Proposta>(x =>
                x.Cliente.Id == proposta.Cliente.Id
                && x.Unidade.Id == proposta.Unidade.Id
                && (proposta.Id == 0 || x.Id != proposta.Id));

            return propostaExistente != null;
        }
    }
}