using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IFornecedorAplicacao : IBaseAplicacao<Fornecedor>
    {
    }

    public class FornecedorAplicacao : BaseAplicacao<Fornecedor, IFornecedorServico>, IFornecedorAplicacao
    {
        private readonly IFornecedorServico _fornecedorServico;
        private readonly ICidadeServico _cidadeServico;

        public FornecedorAplicacao(ICidadeServico cidadeServico, IFornecedorServico fornecedorServico)
        {
            _cidadeServico = cidadeServico;
            _fornecedorServico = fornecedorServico;
        }

        public new void Salvar(Fornecedor entity)
        {
            if (!ObjetoValido(entity))
                throw new BusinessRuleException("Objeto Invalido!");

            foreach (var pessoaEndereco in entity.Enderecos)
            {
                pessoaEndereco.Endereco.Cidade = _cidadeServico.BuscarPelaDescricao(pessoaEndereco.Endereco?.Cidade?.Descricao, pessoaEndereco.Endereco?.Cidade?.Estado?.Descricao);
            }

            var retorno = BuscarPorId(entity.Id) ?? entity;
            retorno.Id = entity.Id;
            retorno.TipoPessoa = entity.TipoPessoa;
            retorno.Contatos = entity.Contatos;
            retorno.DataInsercao = entity.DataInsercao;
            retorno.RazaoSocial = entity.RazaoSocial;
            retorno.Nome = entity.Nome;
            retorno.NomeFantasia = entity.NomeFantasia;
            retorno.DocumentoCpf = entity.DocumentoCpf;
            retorno.DocumentoRg = entity.DocumentoRg;
            retorno.DocumentoCnpj = entity.DocumentoCnpj;
            retorno.DocumentoIe = entity.DocumentoIe;
            retorno.DocumentoIm = entity.DocumentoIm;
            retorno.ReceberCotacaoPorEmail = entity.ReceberCotacaoPorEmail;
            retorno.Enderecos = entity.Enderecos;
            retorno.Contatos = entity.Contatos?.Where(x => !string.IsNullOrEmpty(x.Contato.Email) || !string.IsNullOrEmpty(x.Contato.Numero))?.ToList();
            retorno.Banco = entity.Banco;
            retorno.Agencia = entity.Agencia;
            retorno.DigitoAgencia = entity.DigitoAgencia;
            retorno.Conta = entity.Conta;
            retorno.DigitoConta = entity.DigitoConta;
            retorno.CPFCNPJ = entity.CPFCNPJ;
            retorno.Beneficiario = entity.Beneficiario;

            Servico.Salvar(retorno);
        }

        private static bool ObjetoValido(Fornecedor entity)
        {
            if (entity.TipoPessoa == TipoPessoa.Fisica)
            {
                if (string.IsNullOrEmpty(entity.DocumentoCpf))
                    throw new BusinessRuleException("Informe o CPF!");

                if (string.IsNullOrEmpty(entity.Nome))
                    throw new BusinessRuleException("Informe o Nome do Fornecedor!");

            }

            if (entity.TipoPessoa == TipoPessoa.Juridica)
            {
                if (string.IsNullOrEmpty(entity.DocumentoCnpj))
                    throw new BusinessRuleException("Informe o CNPJ!");

                if (string.IsNullOrEmpty(entity.RazaoSocial))
                    throw new BusinessRuleException("Informe a Razão Social do Fornecedor!");
            }

            if (!entity.Enderecos.Any())
                throw new BusinessRuleException("Informe o Endereco do Fornecedor!");

            if (!entity.Contatos.Any())
                throw new BusinessRuleException("Informe ao menos um Contato o Fornecedor!");

            if (entity.ReceberCotacaoPorEmail && !entity.Contatos.Any(x => x.Contato.Tipo == TipoContato.Email))
                throw new BusinessRuleException("É preciso informar um email para receber a cotação!");

            return true;
        }

    }
}
