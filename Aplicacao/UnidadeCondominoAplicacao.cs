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
    public interface IUnidadeCondominoAplicacao : IBaseAplicacao<UnidadeCondomino>
    { }

    public class UnidadeCondominoAplicacao : BaseAplicacao<UnidadeCondomino, IUnidadeCondominoServico>, IUnidadeCondominoAplicacao
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IUnidadeCondominoServico _unidadeCondominoServico;

        public UnidadeCondominoAplicacao(IUnidadeAplicacao unidadeAplicacao,
                                         IUnidadeCondominoServico unidadeCondominoServico)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _unidadeCondominoServico = unidadeCondominoServico;
        }

        public new void Salvar(UnidadeCondomino entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            int vagasAntes = 0;
            int vagasAtual = 0;

            var UnidadeCondomino = BuscarPorId(entity.Id) ?? entity;
            UnidadeCondomino.Id = entity.Id;
            UnidadeCondomino.Unidade = entity.Unidade;

            if(UnidadeCondomino.Id != 0)
            {
                vagasAtual = entity.NumeroVagas;
                vagasAntes = UnidadeCondomino.NumeroVagas; 

                if(vagasAtual > vagasAntes)
                {
                    var novasVagas = vagasAtual - vagasAntes;

                    UnidadeCondomino.NumeroVagasRestantes = UnidadeCondomino.NumeroVagasRestantes + novasVagas;
                }
                else if (vagasAtual < vagasAntes)
                {
                    var novasVagas = vagasAntes - vagasAtual;

                    if(novasVagas > UnidadeCondomino.NumeroVagasRestantes)
                    {
                        throw new BusinessRuleException("Não é possível diminuir para esta quantidade, pois há vagas ocupadas!");
                    }

                    UnidadeCondomino.NumeroVagasRestantes = UnidadeCondomino.NumeroVagasRestantes - novasVagas;
                }
            }

            UnidadeCondomino.NumeroVagas = entity.NumeroVagas;
            UnidadeCondomino.DataInsercao = entity.DataInsercao;

            if(UnidadeCondomino.Id == 0)
            {
                UnidadeCondomino.NumeroVagasRestantes = UnidadeCondomino.NumeroVagas;
            }

            Servico.Salvar(UnidadeCondomino);
        }

        public bool ObjetoValido(UnidadeCondomino entity)
        {
            if (entity.NumeroVagas <= 0)
                throw new BusinessRuleException("Informe um numero válido de vagas!");

            if (entity.Unidade == null || entity.Unidade.Id ==0)
                throw new BusinessRuleException("Informe uma Unidade!");

            if (entity.Id == 0)
            {
                var UnidadeCondomino = BuscarPor(x => x.Unidade.Id == entity.Unidade.Id);

                if (UnidadeCondomino.Any())
                {
                    var unidadeCondominoExistente = UnidadeCondomino.FirstOrDefault();
                    throw new BusinessRuleException($"A Unidade {unidadeCondominoExistente.Unidade.Nome} já possui cadastro de Vagas Condomino definida!");
                } 
            }

            var unidade = _unidadeAplicacao.BuscarPorId(entity.Unidade.Id);

            if (entity.NumeroVagas > unidade.NumeroVaga)
            {
                //throw new BusinessRuleException($"A Unidade possui apenas {unidade.NumeroVaga} vagas disponíveis !");
                throw new BusinessRuleException($"Numero Total de Vagas nao pode ser maior do que cadastrado em unidade");
            }

            return true;
        }

        public new void ExcluirPorId(int id)
        {
            _unidadeCondominoServico.ExcluirPorId(id);
        }
    }
}