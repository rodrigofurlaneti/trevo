using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Core.Extensions;
using System;
using Core.Exceptions;

namespace Aplicacao
{
    public interface IClienteAplicacao : IBaseAplicacao<Cliente>
    {
        Cliente BuscarPorCpf(string cpf);
        Cliente BuscarPorCnpj(string cnpj);
        List<ChaveValorViewModel> BuscarClientesDaUnidade(int unidadeId);
        CondutorSoftparkViewModel ConverterClienteParaCondutor(Cliente cliente);
        void SalvarClienteCondutor(Cliente cliente);
        List<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina);
        IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50);
        SeloCliente BuscarSeloCliente(int cliente_Id);
        void Salvar(Cliente cliente, int usuarioId);
    }

    public class ClienteAplicacao : BaseAplicacao<Cliente, IClienteServico>, IClienteAplicacao
    {
        private readonly IClienteServico _clienteServico;
        private readonly ICidadeAplicacao _cidadeAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ICondutorSoftparkAplicacao _condutorSoftparkAplicacao;
        private readonly IClienteCondominoServico _condominoServico;
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly IContratoMensalistaServico _contratoMensalistaServico;
        private readonly IContaCorrenteClienteServico _contaCorrenteClienteServico;
        private readonly ISeloClienteServico _seloClienteServico;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IOcorrenciaAplicacao _ocorrenciaAplicacao;
        private readonly IVeiculoAplicacao _veiculoAplicacao;

        public ClienteAplicacao(
            IClienteServico clienteServico,
            ICidadeAplicacao cidadeAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            ICondutorSoftparkAplicacao condutorSoftparkAplicacao,
            IClienteCondominoServico condominoServico,
            ILancamentoCobrancaServico lancamentoCobrancaServico,
            IContratoMensalistaServico contratoMensalistaServico,
            IContaCorrenteClienteServico contaCorrenteClienteServico,
            ISeloClienteServico seloClienteServico,
            IFuncionarioAplicacao funcionarioAplicacao,
            IOcorrenciaAplicacao ocorrenciaAplicacao,
            IVeiculoAplicacao veiculoAplicacao)
        {
            _clienteServico = clienteServico;
            _cidadeAplicacao = cidadeAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _condutorSoftparkAplicacao = condutorSoftparkAplicacao;
            _condominoServico = condominoServico;
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _contratoMensalistaServico = contratoMensalistaServico;
            _contaCorrenteClienteServico = contaCorrenteClienteServico;
            _seloClienteServico = seloClienteServico;
            _funcionarioAplicacao = funcionarioAplicacao;
            _ocorrenciaAplicacao = ocorrenciaAplicacao;
            _veiculoAplicacao = veiculoAplicacao;
        }

        public List<ChaveValorViewModel> BuscarClientesDaUnidade(int unidadeId)
        {
            var clientes = _clienteServico.BuscarPor(x => x.Unidades.Any(u => u.Unidade.Id == unidadeId));
            return clientes.Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.NomeExibicao
            }).ToList() ?? new List<ChaveValorViewModel>();
        }

        public Cliente BuscarPorCpf(string cpf)
        {
            return _clienteServico.BuscarPor(x => x.Pessoa != null && x.Pessoa.Documentos.Any(d => d.Documento.Tipo == TipoDocumento.Cpf && (d.Documento.Numero == cpf.RemoveSpecialCharacters() || d.Documento.Numero == cpf))).FirstOrDefault();
        }
        public Cliente BuscarPorCnpj(string cnpj)
        {
            return _clienteServico.BuscarPor(x => x.Pessoa != null && x.Pessoa.Documentos.Any(d => d.Documento.Tipo == TipoDocumento.Cnpj && (d.Documento.Numero == cnpj.RemoveSpecialCharacters() || d.Documento.Numero == cnpj))).FirstOrDefault();
        }

        public new void Salvar(Cliente cliente, int usuarioId)
        {
            var retorno = BuscarPorId(cliente.Id) ?? cliente;

            if (retorno.Unidades == null)
                retorno.Unidades = new List<ClienteUnidade>();

            retorno.NomeFantasia = cliente.NomeFantasia;

            var unidades = _unidadeAplicacao.Buscar().ToList();
            retorno.Unidades.Clear();
            foreach (var idMenu in cliente.ListaUnidade)
            {
                retorno.Unidades.Add(new ClienteUnidade { Unidade = unidades.FirstOrDefault(x => x.Id == idMenu) });
            }

            retorno.Id = cliente.Id;
            retorno.TipoPessoa = cliente.TipoPessoa;
            retorno.DataInsercao = cliente.DataInsercao;

            if (cliente.TipoPessoa == TipoPessoa.Juridica)
            {
                retorno.RazaoSocial = string.IsNullOrEmpty(cliente.RazaoSocial) ? cliente.NomeFantasia : cliente.RazaoSocial;
                retorno.NomeFantasia = cliente.NomeFantasia;
                retorno.Pessoa.DocumentoCnpj = cliente.Pessoa.DocumentoCnpj;
                retorno.Pessoa.DocumentoIe = cliente.Pessoa.DocumentoIe;
                retorno.Pessoa.DocumentoIm = cliente.Pessoa.DocumentoIm;
                LimparDadosPessoaFisica(ref retorno);
            }
            else if (cliente.TipoPessoa == TipoPessoa.Fisica)
            {
                retorno.Pessoa.Nome = cliente.Pessoa.Nome;
                retorno.Pessoa.Sexo = cliente.Pessoa.Sexo;
                retorno.Pessoa.DataNascimento = cliente.Pessoa.DataNascimento;
                retorno.Pessoa.DocumentoCpf = cliente.Pessoa.DocumentoCpf;
                retorno.Pessoa.DocumentoRg = cliente.Pessoa.DocumentoRg;
                LimparDadosPessoaJuridica(ref retorno);
            }

            retorno.NomeConvenio = cliente.NomeConvenio;
            retorno.Observacao = cliente.Observacao;

            retorno.Pessoa.Contatos = cliente.Pessoa.Contatos;
            retorno.Pessoa.Enderecos = cliente.Pessoa.Enderecos;
            retorno.Veiculos = cliente.Veiculos;
            retorno.Pessoa.Contatos = cliente.Pessoa.Contatos?.Where(x => !string.IsNullOrEmpty(x.Contato.Email) || !string.IsNullOrEmpty(x.Contato.Numero))?.ToList();
            retorno.NotaFiscalSemDesconto = cliente.NotaFiscalSemDesconto;
            retorno.ExigeNotaFiscal = cliente.ExigeNotaFiscal;

            foreach (var pessoaEndereco in retorno.Pessoa.Enderecos)
            {
                pessoaEndereco.Endereco.Cidade = pessoaEndereco.Endereco.Cidade == null
                    ? null
                    : _cidadeAplicacao.BuscarPor(endereco => endereco.Descricao == pessoaEndereco.Endereco.Cidade.Descricao).FirstOrDefault();
            }

            Servico.Salvar(retorno);

            SalvarContaCorrente(cliente, retorno);
            SalvarSelos(cliente);
            SalvarOcorrencias(cliente, usuarioId);

            try
            {
                var condutor = this.ConverterClienteParaCondutor(retorno);

                if ((condutor.Contratos != null && condutor.Contratos.Count > 0) || (condutor.ContratosCondomino != null && condutor.ContratosCondomino.Count > 0))
                {
                    _condutorSoftparkAplicacao.Salvar(condutor);
                }
            }
            catch (SoftparkIntegrationException ex)
            {
                throw new SoftparkIntegrationException("Registro salvo com sucesso!<br/>Porém encontramos um erro na integração com o PDV.<br/>Erro: " + ex.Message);
            }
        }

        private void SalvarContaCorrente(Cliente cliente, Cliente retorno)
        {
            if (cliente.ContaCorrenteCliente == null)
                cliente.ContaCorrenteCliente = new ContaCorrenteCliente { Cliente = retorno };
            else
                cliente.ContaCorrenteCliente.Cliente = retorno;

            var contaCorrenteEntity = _contaCorrenteClienteServico.PrimeiroPor(x => x.Cliente.Id == retorno.Id) ?? cliente.ContaCorrenteCliente;
            contaCorrenteEntity.ContaCorrenteClienteDetalhes = cliente.ContaCorrenteCliente.ContaCorrenteClienteDetalhes?.Select(x => x)?.ToList() ?? new List<ContaCorrenteClienteDetalhe>();

            _contaCorrenteClienteServico.Salvar(contaCorrenteEntity);
        }

        private void SalvarSelos(Cliente cliente)
        {
            if (!ReferenceEquals(cliente.SeloCliente, null))
            {
                cliente.SeloCliente.Cliente = new Cliente() { Id = cliente.Id };
                _seloClienteServico.Salvar(cliente.SeloCliente);
            }
        }

        private void SalvarOcorrencias(Cliente cliente, int usuarioLogadoCurrent)
        {
            var clienteBase = BuscarPorId(cliente.Id);
            var clienteVeiculosSalvos = clienteBase.Veiculos;

            foreach (var ocorrencia in cliente.Ocorrencias)
            {
                ocorrencia.Id = ocorrencia.Id < 0 ? 0 : ocorrencia.Id;   
                ocorrencia.Cliente = new Cliente { Id = cliente.Id };
                if (ocorrencia.Veiculo != null)
                {
                    var clienteVeiculo = BuscarPor(x=>x.Id == cliente.Id).Select(s=>s.Veiculos.FirstOrDefault(f=>f.Veiculo.Placa == ocorrencia.Veiculo.Placa)).FirstOrDefault();
                    if (!ReferenceEquals(clienteVeiculo, null))
                    {
                        ocorrencia.Veiculo = clienteVeiculo.Veiculo;
                    }
                }

                var entityOcorrencia = AutoMapper.Mapper.Map<OcorrenciaCliente>(ocorrencia);

                entityOcorrencia.FuncionarioAtribuido = _funcionarioAplicacao.BuscarPorId(ocorrencia.FuncionarioAtribuido.Pessoa.Id) ?? null;
                entityOcorrencia.Cliente = clienteBase;
                entityOcorrencia.Unidade = entityOcorrencia.Unidade != null && entityOcorrencia.Unidade.Id > 0 ? _unidadeAplicacao.BuscarPorId(entityOcorrencia.Unidade.Id) ?? null : null;
                entityOcorrencia.Veiculo = entityOcorrencia.Veiculo != null && entityOcorrencia.Veiculo.Id > 0 ? _veiculoAplicacao.BuscarPorId(entityOcorrencia.Veiculo.Id) ?? null : null;

                _ocorrenciaAplicacao.SalvarDadosOcorrenciaComNotificacao(entityOcorrencia, usuarioLogadoCurrent);
            }
        }

        private void LimparDadosPessoaFisica(ref Cliente cliente)
        {
            cliente.Pessoa.Nome = string.Empty;
            cliente.Pessoa.DataNascimento = new DateTime?();
            cliente.Pessoa.Documentos.Remove(cliente.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf));
            cliente.Pessoa.Documentos.Remove(cliente.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == TipoDocumento.Rg));
            cliente.Pessoa.Sexo = string.Empty;
        }
        private void LimparDadosPessoaJuridica(ref Cliente cliente)
        {
            cliente.RazaoSocial = string.Empty;
            cliente.NomeFantasia = string.Empty;
            cliente.Pessoa.Documentos.Remove(cliente.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == TipoDocumento.Cnpj));
            cliente.Pessoa.Documentos.Remove(cliente.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == TipoDocumento.Ie));
            cliente.Pessoa.Documentos.Remove(cliente.Pessoa.Documentos.FirstOrDefault(x => x.Tipo == TipoDocumento.Im));
        }

        public void SalvarClienteCondutor(Cliente cliente)
        {
            _clienteServico.Salvar(cliente);
        }

        public CondutorSoftparkViewModel ConverterClienteParaCondutor(Cliente cliente)
        {
            var contratosMensalistas = _contratoMensalistaServico.BuscarPor(x => x.Cliente.Id == cliente.Id);

            var clienteCondomino = _condominoServico.BuscarPor(x => x.Cliente.Id == cliente.Id);

            var condutor = new CondutorSoftparkViewModel(cliente, clienteCondomino, contratosMensalistas);

            return condutor;
        }

        private int BuscarNumeroVagasCondimino(int pessoaId)
        {
            return _condominoServico.PrimeiroPor(x => x.Cliente.Pessoa.Id == pessoaId)?.NumeroVagas ?? 0;
        }

        private DateTime? BuscarDataUltimoPagamento(string cpf)
        {
            cpf = cpf.FormatCpfCnpj();

            var lancamentoCobrancas = _lancamentoCobrancaServico
                                         .BuscarPor(x => x.Cliente.Pessoa.Documentos
                                                           .Any(d => d.Documento.Tipo == TipoDocumento.Cpf && d.Documento.Numero == cpf)).ToList();

            lancamentoCobrancas = lancamentoCobrancas.Where(x => x.Recebimento != null && x.Recebimento.Pagamentos != null && x.Recebimento.Pagamentos.Any()).ToList();
            return lancamentoCobrancas != null && lancamentoCobrancas.Any() ?
                        lancamentoCobrancas.SelectMany(x => x.Recebimento.Pagamentos)?.Max(x => x.DataPagamento) : null;
        }

        public List<Cliente> BuscarPorIntervaloOrdenadoPorNome(int registroInicial, int registrosPorPagina)
        {
            return _clienteServico.BuscarPorIntervaloOrdenadoPorNome(registroInicial, registrosPorPagina).ToList();
        }

        public IList<Cliente> BuscarDadosGrid(string documento, string nome, string contrato, out int quantidadeRegistros, int pagina = 1, int take = 50)
        {
            return _clienteServico.BuscarDadosGrid(documento, nome, contrato, out quantidadeRegistros, pagina, take);
        }

        public SeloCliente BuscarSeloCliente(int cliente_Id)
        {
            var seloCliente = _seloClienteServico.BuscarPor(s => s.Cliente.Id == cliente_Id).FirstOrDefault();
            return seloCliente;
        }
    }
}