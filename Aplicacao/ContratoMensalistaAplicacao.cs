using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContratoMensalistaAplicacao : IBaseAplicacao<ContratoMensalista>
    {
        void Salvar(ContratoMensalista entity, bool validar, bool salvarCondutorSoftpark = true);
        List<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros);
        List<ContratoMensalista> BuscarPorCliente(int idCliente);
        void SalvarCondutorSoftPark(ContratoMensalista item);
    }

    public class ContratoMensalistaAplicacao : BaseAplicacao<ContratoMensalista, IContratoMensalistaServico>, IContratoMensalistaAplicacao
    {
        private readonly IContratoMensalistaServico _contratoMensalistaServico;
        private readonly ICondutorSoftparkAplicacao _condutorSoftparkAplicacao;
        private readonly IClienteCondominoServico _condominoServico;

        public ContratoMensalistaAplicacao(
            IContratoMensalistaServico contratoMensalistaServico, 
            ICondutorSoftparkAplicacao condutorSoftparkAplicacao,
            IClienteCondominoServico condominoServico)
        {
            _contratoMensalistaServico = contratoMensalistaServico;
            _condutorSoftparkAplicacao = condutorSoftparkAplicacao;
            _condominoServico = condominoServico;
        }

        public List<ContratoMensalista> BuscarPorIntervaloOrdenadoPeloNomeDoCliente(int registroInicial, int quantidadeRegistros)
        {
           return _contratoMensalistaServico.BuscarPorIntervaloOrdenadoPeloNomeDoCliente(registroInicial, quantidadeRegistros);
        }

        public List<ContratoMensalista> ObterDadosContrato(int idContratoMensalista)
        {
            return new List<ContratoMensalista>();
        }

        public void Salvar(ContratoMensalista item, bool validar, bool salvarCondutorSoftpark = true)
        {
            _contratoMensalistaServico.Salvar(item, validar);

            if (!salvarCondutorSoftpark)
                return;

            var contratoMensalista = _contratoMensalistaServico.BuscarPorId(item.Id);
            if (contratoMensalista == null) new ContratoMensalista();
            var contratosCondomino = _condominoServico.BuscarPor(x => x.Cliente.Id == contratoMensalista.Cliente.Id);
            var contratos = _contratoMensalistaServico.BuscarPor(x => x.Cliente.Id == item.Cliente.Id);
            var condutor = new CondutorSoftparkViewModel(contratoMensalista.Cliente, contratosCondomino, contratos);

            _condutorSoftparkAplicacao.Salvar(condutor);
        }

        public void SalvarCondutorSoftPark(ContratoMensalista item)
        {
            var contratoMensalista = _contratoMensalistaServico.BuscarPorId(item.Id);
            if (contratoMensalista == null) new ContratoMensalista();
            var contratosCondomino = _condominoServico.BuscarPor(x => x.Cliente.Id == contratoMensalista.Cliente.Id);
            var contratos = _contratoMensalistaServico.BuscarPor(x => x.Cliente.Id == item.Cliente.Id);
            var condutor = new CondutorSoftparkViewModel(contratoMensalista.Cliente, contratosCondomino, contratos);

            _condutorSoftparkAplicacao.Salvar(condutor);
        }

        public List<ContratoMensalista> BuscarPorCliente(int idCliente)
        {
            return _contratoMensalistaServico.BuscarPorCliente(idCliente)?.OrderBy(x=>x.Unidade.Nome)?.ToList();
        }
    }
}