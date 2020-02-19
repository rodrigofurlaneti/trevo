using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IClienteCondominoAplicacao : IBaseAplicacao<ClienteCondomino>
    { }

    public class ClienteCondominoAplicacao : BaseAplicacao<ClienteCondomino, IClienteCondominoServico>, IClienteCondominoAplicacao
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IUnidadeCondominoAplicacao _unidadeCondominoAplicacao;
        private readonly IClienteCondominoServico _clienteCondominoServico;
        private readonly ICondutorSoftparkAplicacao _condutorSoftparkAplicacao;
        private readonly IContratoMensalistaServico _contratoMensalistaServico;

        public ClienteCondominoAplicacao(IUnidadeAplicacao unidadeAplicacao,
                                         IUnidadeCondominoAplicacao unidadeCondominoAplicacao,
                                         IClienteAplicacao clienteAplicacao,
                                         IClienteCondominoServico clienteCondominoServico,
                                         ICondutorSoftparkAplicacao condutorSoftparkAplicacao,
                                         IContratoMensalistaServico contratoMensalistaServico)
        {
            _unidadeCondominoAplicacao = unidadeCondominoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _clienteCondominoServico = clienteCondominoServico;
            _condutorSoftparkAplicacao = condutorSoftparkAplicacao;
            _contratoMensalistaServico = contratoMensalistaServico;
        }

        public new void Salvar(ClienteCondomino entity)
        {

            if (entity.Cliente == null || entity.Cliente.Id == 0)
                throw new BusinessRuleException("Informe um Cliente!");

            if (entity.Unidade == null || entity.Unidade.Id == 0)
                throw new BusinessRuleException("Informe uma Unidade!");

            if (entity.NumeroVagas <= 0)
                throw new BusinessRuleException("Informe um numero válido de vagas!");

            var unidadeCondomino = _unidadeCondominoAplicacao.BuscarPor(x => x.Unidade.Id == entity.Unidade.Id).FirstOrDefault();

            var clienteCondomino = BuscarPorId(entity.Id) ?? entity;

            if (entity.Id == 0)
            {
                var objVerificaReincidencia = BuscarPor(x => x.Cliente.Id == entity.Cliente.Id && x.Unidade.Id == entity.Unidade.Id);

                if (objVerificaReincidencia.Any())
                {
                    throw new BusinessRuleException($"Este Cliente já possui quantidade de vagas cadastradas para esta Unidade !");
                }

                if (unidadeCondomino == null || entity.NumeroVagas > unidadeCondomino.NumeroVagasRestantes)
                {
                    throw new BusinessRuleException($"A Unidade possui apenas {unidadeCondomino.NumeroVagasRestantes} vagas restantes para Condômino !");
                }

                unidadeCondomino.NumeroVagasRestantes = unidadeCondomino.NumeroVagasRestantes - entity.NumeroVagas;
            }
            else
            {
                unidadeCondomino.NumeroVagasRestantes = unidadeCondomino.NumeroVagasRestantes + clienteCondomino.NumeroVagas;

                if (entity.NumeroVagas > unidadeCondomino.NumeroVagasRestantes)
                {
                    throw new BusinessRuleException($"A Unidade possui apenas {unidadeCondomino.NumeroVagasRestantes} vagas restantes para Condômino !");
                }

                unidadeCondomino.NumeroVagasRestantes = unidadeCondomino.NumeroVagasRestantes - entity.NumeroVagas;
            }

            clienteCondomino.Id = entity.Id;
            clienteCondomino.Cliente = _clienteAplicacao.BuscarPorId(entity.Cliente.Id);
            clienteCondomino.Unidade = unidadeCondomino;
            clienteCondomino.NumeroVagas = entity.NumeroVagas;
            clienteCondomino.DataInsercao = entity.DataInsercao;
            clienteCondomino.CondominoVeiculos = entity.CondominoVeiculos;
            clienteCondomino.Frota = entity.Frota;

            Servico.Salvar(clienteCondomino);

            var contratosCondomino = Servico.BuscarPor(x => x.Cliente.Id == clienteCondomino.Cliente.Id);
            var contratosMensalista = _contratoMensalistaServico.BuscarPor(x => x.Cliente.Id == clienteCondomino.Cliente.Id);

            var condutor = new CondutorSoftparkViewModel(clienteCondomino.Cliente, contratosCondomino, contratosMensalista);

            _condutorSoftparkAplicacao.Salvar(condutor);
        }

        public new void ExcluirPorId(int id)
        {

            var objClienteCondomino = _clienteCondominoServico.BuscarPorId(id);

            var objUnidadeCondomino = objClienteCondomino.Unidade;

            objUnidadeCondomino.NumeroVagasRestantes = objUnidadeCondomino.NumeroVagasRestantes + objClienteCondomino.NumeroVagas;

            //atualiza NumeroVagasRestantes das vagas excluídas
            _unidadeCondominoAplicacao.Salvar(objUnidadeCondomino);

            //exclui vagas Cliente Condomino
            _clienteCondominoServico.ExcluirPorId(id);
        }
    }
}