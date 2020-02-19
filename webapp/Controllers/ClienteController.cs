using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Core.Validators;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Portal.Models;

namespace Portal.Controllers
{
    public class ClienteController : GenericController<Cliente>
    {
        public List<ClienteViewModel> ListaClientes => _clienteAplicacao?.Buscar()?.Select(x => new ClienteViewModel())?.ToList() ?? new List<ClienteViewModel>();
        public List<Unidade> ListaUnidades => _unidadeAplicacao?.Buscar().ToList();
        private List<SelectListItem> _listMarcas { get; set; }
        public List<SelectListItem> ListaMarcas
        {
            get { return _listMarcas; }
            set { _listMarcas = value; }
        }

        public List<ContratoMensalistaViewModel> ListaContratosMensalistas { get; set; }
        public List<ContratoMensalistaViewModel> ListaContrato
        {
            get => (List<ContratoMensalistaViewModel>)Session["ListaContratos"] ?? new List<ContratoMensalistaViewModel>();
            set => Session["ListaContratos"] = value;
        }

        public bool IsEdicao = false;

        public List<TipoMensalistaViewModel> ListaTipoMensalista => AutoMapper.Mapper.Map<List<TipoMensalista>, List<TipoMensalistaViewModel>>(_tipoMensalistaAplicacao.BuscarPor(t => t.Ativo)?.ToList());

        public List<ContaCorrenteClienteDetalheViewModel> ListaContaCorrenteClienteDetalhes
        {
            get => (List<ContaCorrenteClienteDetalheViewModel>)Session["ListaContaCorrenteItemDetalhes"] ?? new List<ContaCorrenteClienteDetalheViewModel>();
            set => Session["ListaContaCorrenteItemDetalhes"] = value;
        }
        public ClienteCadastroViewModel ClienteCadastroViewModel { get; set; }

        public List<FeriasClienteViewModel> ListaFerias
        {
            get => (List<FeriasClienteViewModel>)Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] ?? new List<FeriasClienteViewModel>();
            set => Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = value;
        }

        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly IPessoaAplicacao _pessoaAplicacao;
        private readonly ICidadeAplicacao _cidadeAplicacao;
        private readonly IMarcaAplicacao _marcaAplicacao;
        private readonly IModeloAplicacao _modeloAplicacao;
        private readonly IVeiculoAplicacao _veiculoAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IContratoMensalistaAplicacao _contratoMensalistaAplicacao;
        private readonly IContaCorrenteClienteAplicacao _contaCorrenteClienteAplicacao;
        private readonly ITipoMensalistaAplicacao _tipoMensalistaAplicacao;
        private readonly IOcorrenciaAplicacao _ocorrenciaAplicacao;
        private readonly IFeriasClienteAplicacao _feriasClienteAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public ClienteController(IClienteAplicacao clienteAplicacao
                                , IEmpresaAplicacao empresaAplicacao
                                , IPessoaAplicacao pessoaAplicacao
                                , ICidadeAplicacao cidadeAplicacao
                                , IMarcaAplicacao marcaAplicacao
                                , IModeloAplicacao modeloAplicacao
                                , IVeiculoAplicacao veiculoAplicacao
                                , IUnidadeAplicacao unidadeAplicacao
                                , IContratoMensalistaAplicacao contratoMensalistaAplicacao
                                , IContaCorrenteClienteAplicacao contaCorrenteClienteAplicacao
                                , ITipoMensalistaAplicacao tipoMensalistaAplicacao
                                , IOcorrenciaAplicacao ocorrenciaAplicacao
                                , IFeriasClienteAplicacao feriasClienteAplicacao
                                , IFuncionarioAplicacao funcionarioAplicacao
                                )
        {
            Aplicacao = clienteAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _empresaAplicacao = empresaAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
            _cidadeAplicacao = cidadeAplicacao;
            _marcaAplicacao = marcaAplicacao;
            _modeloAplicacao = modeloAplicacao;
            _veiculoAplicacao = veiculoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _contratoMensalistaAplicacao = contratoMensalistaAplicacao;
            _contaCorrenteClienteAplicacao = contaCorrenteClienteAplicacao;
            _tipoMensalistaAplicacao = tipoMensalistaAplicacao;
            _ocorrenciaAplicacao = ocorrenciaAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _feriasClienteAplicacao = feriasClienteAplicacao;

            ViewBag.TipoPessoaSelectList = new SelectList(
                Enum.GetValues(typeof(TipoPessoa)).Cast<TipoPessoa>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");

            ViewBag.ListaTipoOperacaoContaCorrente = Aplicacao?.BuscarValoresDoEnum<TipoOperacaoContaCorrente>();
            ViewBag.ListaMeses = Aplicacao?.BuscarValoresDoEnum<Mes>();
            ViewBag.ListaContratos = new List<ContaCorrenteCliente>();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            IsEdicao = false;
            Session["ClienteEnderecos"] = "";
            Session["contatos"] = "";
            Session["Veiculo"] = null;
            ListaContratosMensalistas = new List<ContratoMensalistaViewModel>();
            ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
            Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = new List<FeriasClienteViewModel>();
            Session["ListaOcorrencia"] = new List<OcorrenciaClienteViewModel>(); 

            return View("Index");
        }

        public JsonResult SuggestionPerson(string param, bool exact)
        {
            return Json(_clienteAplicacao.BuscarPor(x => !exact && x.Pessoa.Nome.Contains(param) || x.Pessoa.Nome == param).Select(x => new ClienteViewModel()));
        }

        [CheckSessionOut]
        [HttpPost, ValidateInput(false)]
        public ActionResult SalvarDados(ClienteViewModel model, ContaCorrenteClienteViewModel contaCorrenteClienteViewModel)
        {
            return SalvarDadosCliente(model, contaCorrenteClienteViewModel, "Index");
        }

        public override ActionResult Edit(int id)
        {
            var clienteEntity = _clienteAplicacao.BuscarPorId(id);
            var cliente = new ClienteViewModel(clienteEntity);

            ViewBag.TipoPessoaSelectList = new SelectList(
                Enum.GetValues(typeof(TipoPessoa)).Cast<TipoPessoa>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao",
                (int)cliente.TipoPessoa);

            Session["ClienteEnderecos"] = cliente.Pessoa.Enderecos;
            Session["contatos"] = cliente.Pessoa.Contatos;
            Session["Veiculos"] = cliente.Veiculos;
            Session["ListaOcorrencia"] = _ocorrenciaAplicacao.BuscarPor(x => x.Cliente.Id == cliente.Id).Select(x => AutoMapper.Mapper.Map<OcorrenciaCliente, OcorrenciaClienteViewModel>(x)).ToList();
            var lojasPorFuncao = _empresaAplicacao.Buscar().Select(x => AutoMapper.Mapper.Map<Empresa, EmpresaViewModel>(x)).ToList();
            var lojasSession = new List<EmpresaViewModel>();

            foreach (var lojaFuncao in lojasPorFuncao)
                if (lojasSession.All(x => x.Id != lojaFuncao.Id))
                    lojasSession.Add(lojaFuncao);

            Session["LojasSelecionadas"] = lojasSession;

            cliente.ContaCorrenteCliente = new ContaCorrenteClienteViewModel(_contaCorrenteClienteAplicacao.BuscarPor(x => x.Cliente.Id == cliente.Id)?.FirstOrDefault() ?? new ContaCorrenteCliente());
            ListaContaCorrenteClienteDetalhes = cliente.ContaCorrenteCliente?.ContaCorrenteClienteDetalhes;

            ListaContratosMensalistas = _contratoMensalistaAplicacao.BuscarPorCliente(id)?.Select(x => new ContratoMensalistaViewModel(x))?.OrderBy(x => x.Unidade.Nome)?.ToList() ?? new List<ContratoMensalistaViewModel>();
            if (!ReferenceEquals(ListaContratosMensalistas, null))
            {
                ViewBag.ListaContratos = ListaContratosMensalistas.Select(e => new ChaveValorViewModel { Id = (int)e.Id, Descricao = e.NumeroContrato.ToString() });
            }
            
            Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = _feriasClienteAplicacao.BuscarPor(x => x.Cliente.Id == id)?.Select(x => new FeriasClienteViewModel(x))?.ToList();
            BuscarSelo(cliente);

            return View("Index", cliente);
        }

        private void BuscarSelo(ClienteViewModel cliente)
        {
            var seloClienteEntity = _clienteAplicacao.BuscarSeloCliente(cliente.Id);
            cliente.SeloCliente = ReferenceEquals(seloClienteEntity, null) ? new SeloClienteViewModel() : new SeloClienteViewModel(seloClienteEntity);
        }

        public JsonResult BuscarDadosDosGrids(int id)
        {
            var cliente = new ClienteViewModel(_clienteAplicacao.BuscarPorId(id));

            Session["Veiculo"] = cliente?.Veiculos.Select(x => x.Veiculo).ToList();

            return Json(
                new
                {
                    enderecos = cliente?.Pessoa.Enderecos,
                    contatos = cliente?.Pessoa.Contatos,
                    veiculos = cliente?.Veiculos.Select(x => x.Veiculo).ToList()
                }
            );
        }

        public JsonResult BuscarDadosDosGridsEmSessao()
        {
            var enderecos = !string.IsNullOrEmpty(Session["ClienteEnderecos"]?.ToString()) ? (List<EnderecoViewModel>)Session["ClienteEnderecos"] : new List<EnderecoViewModel>(); ;
            var contatos = !string.IsNullOrEmpty(Session["contatos"]?.ToString()) ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();
            var veiculos = !string.IsNullOrEmpty(Session["Veiculo"]?.ToString()) ? (List<VeiculoViewModel>)Session["Veiculo"] : new List<VeiculoViewModel>();

            return Json(
                new
                {
                    enderecos,
                    contatos,
                    veiculos,
                }
            );
        }

        [CheckSessionOut]
        public ActionResult AdicionarEndereco(string json)
        {
            if (string.IsNullOrEmpty(json)) return PartialView("_GridEnderecos");

            var enderecos = new List<EnderecoViewModel>();
            enderecos.AddRange(JsonConvert.DeserializeObject<List<EnderecoViewModel>>(json));

            Session["ClienteEnderecos"] = enderecos;

            return PartialView("_GridEnderecos", enderecos);
        }

        [CheckSessionOut]
        public ActionResult AtualizarEnderecos(List<EnderecoViewModel> enderecos)
        {
            Session["ClienteEnderecos"] = enderecos;

            return PartialView("_GridEnderecos", enderecos);
        }

        public ActionResult BuscarClientes(string documento, string nome, string contrato, int pagina = 1)
        {
            var documentoFormatado = string.Empty;
            var grid = string.Empty;
            var resultado = new Resultado<GridClienteViewModel>();
            var clientes = new List<Cliente>();
            var clienteViewModel = new List<GridClienteViewModel>();
            PaginacaoGenericaViewModel paginacao = new PaginacaoGenericaViewModel();
            var take = 50;

            if (!string.IsNullOrEmpty(documento) && !Validators.IsValid(documento))
            {
                resultado = new Resultado<GridClienteViewModel>
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Busca de Clientes",
                    Mensagem = "CPF/CNPJ Inválido!"
                };

                paginacao = new PaginacaoGenericaViewModel(take, 1, 0);
                ViewBag.Paginacao = paginacao;
                grid = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridClientes", clienteViewModel);
                return new JsonResult { Data = new { documentoFormatado, grid, resultado } };
            }

            if (string.IsNullOrEmpty(documento) && string.IsNullOrEmpty(nome) && string.IsNullOrEmpty(contrato))
            {
                var quantidadeFuncionarios = _clienteAplicacao.Contar();
                paginacao = new PaginacaoGenericaViewModel(take, pagina, quantidadeFuncionarios);
                clientes = _clienteAplicacao.BuscarPorIntervaloOrdenadoPorNome(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
            else
            {
                var quantidadeRegistros = 0;
                clientes = _clienteAplicacao.BuscarDadosGrid(documento, nome, contrato, out quantidadeRegistros, pagina, take)?.ToList();
                paginacao = new PaginacaoGenericaViewModel(take, pagina, quantidadeRegistros);
            }

            clienteViewModel = clientes?.Select(x => new GridClienteViewModel
            {
                Id = x.Id,
                Nome = string.IsNullOrEmpty(x.NomeFantasia) ? string.IsNullOrEmpty(x.RazaoSocial) ? x.Pessoa.Nome : x.RazaoSocial : x.NomeFantasia,
                Documento = string.IsNullOrEmpty(x.Pessoa.DocumentoCpf) ? x.Pessoa.DocumentoCnpj : x.Pessoa.DocumentoCpf
            })?
            .OrderBy(x => x.Nome)?
            .ToList();

            ViewBag.Paginacao = paginacao;

            grid = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridClientes", clienteViewModel);
            return new JsonResult { Data = new { documentoFormatado, grid, resultado } };
        }

        public JsonResult CarregarMarcas()
        {
            var lista = new List<Marca>
            {
                new Marca()
                {
                    Nome = @"Selecione um marca.",
                    Id = 0
                }
            };

            var busca = _marcaAplicacao.Buscar().ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Id", "Nome"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Nome", 0));
        }

        public JsonResult CarregarModelos()
        {
            var lista = new List<Modelo>
            {
                new Modelo()
                {
                    Descricao = @"Selecione um Modelo.",
                    Id = 0
                }
            };

            var busca = _modeloAplicacao.Buscar().ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Id", "Descricao"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Descricao", 0));
        }

        public JsonResult BuscarCPF(string cpf)
        {
            var clienteDM = _clienteAplicacao.BuscarPorCpf(cpf);
            return Json(clienteDM?.Id ?? 0);
        }

        public JsonResult BuscarCNPJ(string cnpj)
        {
            var clienteDM = _clienteAplicacao.BuscarPorCnpj(cnpj);
            return Json(clienteDM?.Id ?? 0);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarClientePeloNome(string nome)
        {
            var lista = _clienteAplicacao.BuscarPor(c => c.Pessoa.Nome.Contains(nome) || c.NomeFantasia.Contains(nome));

            return Json(lista.Select(c => new
            {
                c.Id,
                Nome = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
            }));
        }

        [CheckSessionOut]
        public ActionResult CarregarContratosMensalistas(string documento)
        {
            var retorno = new List<ContratoMensalistaViewModel>();
            if (string.IsNullOrEmpty(documento))
                return PartialView("_GridContratosMensalistas", retorno);

            var cliente = documento.ExtractNumbers().Length <= 11 ? _clienteAplicacao.BuscarPorCpf(documento) : _clienteAplicacao.BuscarPorCnpj(documento);

            retorno = _contratoMensalistaAplicacao.BuscarPorCliente(cliente?.Id ?? 0)?.Select(x => new ContratoMensalistaViewModel(x))?.OrderBy(x => x.Unidade.Nome)?.ToList() ?? new List<ContratoMensalistaViewModel>();
            return PartialView("_GridContratosMensalistas", retorno);
        }

        [CheckSessionOut]
        public ActionResult Cadastrar()
        {
            ModelState.Clear();

            IsEdicao = false;
            Session["ClienteEnderecos"] = "";
            Session["contatos"] = "";
            Session["VeiculosAdicionados"] = null;
            Session["Veiculo"] = null;
            Session["ListaOcorrencia"] = new List<OcorrenciaClienteViewModel>();
            ListaContratosMensalistas = new List<ContratoMensalistaViewModel>();
            ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
            ClienteCadastroViewModel = new ClienteCadastroViewModel();
            ClienteCadastroViewModel.Ocorrencia.Veiculo = new VeiculoViewModel();

            return View("Cadastrar", ClienteCadastroViewModel);
        }

        [HttpPost]
        public void AdicionaVeiculoCadatroCliente(string index, string veiculo)
        {
            if (!string.IsNullOrEmpty(veiculo))
            {

                var veiculos = new List<VeiculoViewModel>();
                if (Session["Veiculo"] != null)
                {
                    veiculos = (List<VeiculoViewModel>)Session["Veiculo"];
                }

                if (int.Parse(index) < 0)
                {
                    veiculos.AddRange(JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo));
                }
                else
                {
                    veiculos[int.Parse(index)] = JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo).First();
                }
                Session["Veiculo"] = veiculos;
            }
        }

        public ActionResult SalvarDadosCadastroCliente(ClienteCadastroViewModel model, ContaCorrenteClienteViewModel contaCorrenteClienteViewModel, ClienteViewModel clienteViewModel)
        {
            try
            {
                var cliente = _clienteAplicacao.BuscarPorCpf(model.Cliente.Pessoa.Cpf);
                if (cliente != null)
                {
                    model.Cliente.Id = cliente.Id;
                    model.ClienteId = cliente.Id;
                }

                if (model.ClienteId == 0)
                {
                    Validacao(model);

                    AdicionarModeloMarcaVeiculo(model);
                    AdicionarContatos(model);
                    AdicionarEnderecos(model);
                    //AdicionarContaCorrente(model, contaCorrenteClienteViewModel);
                    AdicionarSelo(model.Cliente, clienteViewModel); 
                    
                    cliente = SalvarCliente(model.Cliente);
                }

                //if (model.Cliente.ContaCorrenteCliente == null)
                //    cliente.ContaCorrenteCliente = new ContaCorrenteCliente { Cliente = cliente };
                //else
                //{
                //    var contaEntity = model.Cliente.ContaCorrenteCliente.ToEntity();
                //    cliente.ContaCorrenteCliente = contaEntity;
                //    cliente.ContaCorrenteCliente.Cliente = cliente;
                //}
                    
                model.Cliente.Id = model.ClienteId = cliente.Id = (model.ClienteId == 0 ? cliente.Id : model.ClienteId);
                SalvarContratoMensalista(_clienteAplicacao.BuscarPorId(model.ClienteId), model.ContratoMensalista.PagamentoCadastro);

                ModelState.Clear();
                LimparSession();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success,
                    RedirectUrl = "Cadastrar"
                };
            }
            catch (SoftparkIntegrationException siex)
            {
                LimparSession();

                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + siex.Message,
                    TipoModal = TipoModal.Danger,
                    RedirectUrl = "Cadastrar"
                };
                return View("Cadastrar", model);
            }
            catch (MonthlyContractPaymentException br)
            {
                CriarModalConfirmacao($"O cliente foi salvo, porém os dados de Contrato precisam de uma definição.<br/><br/>{br.Message}", "Pagamento - Contrato Mensalista", null, "Sim, desejo!", model.Cliente.Id, "", "EfetuarPagamentoCadastro();");
                return View("Cadastrar", model);
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Cadastrar", model);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Cadastrar", model);
            }

            return View("Cadastrar");
        }

        public JsonResult BuscarClienteVeiculos()
        {
            var veiculos = (List<VeiculoViewModel>)Session["VeiculosAdicionados"];
            return Json(veiculos, JsonRequestBehavior.AllowGet);
        }

        private void AdicionarSelo(ClienteViewModel model, ClienteViewModel clienteViewModel)
        {
            model.SeloCliente = clienteViewModel.SeloCliente;
            model.NomeConvenio = clienteViewModel.NomeConvenio;
        }

        private Cliente SalvarCliente(ClienteViewModel model)
        {
            var clienteEntity = model.ToEntity();
            var ocorrencias = !string.IsNullOrEmpty(Session["ListaOcorrencia"]?.ToString()) ? (List<OcorrenciaClienteViewModel>)Session["ListaOcorrencia"] : new List<OcorrenciaClienteViewModel>();
            clienteEntity.Ocorrencias = new List<OcorrenciaCliente>();
            foreach (var ocorrencia in ocorrencias)
            {
                clienteEntity.Ocorrencias.Add(ocorrencia.ToEntity());
            }
            var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
            _clienteAplicacao.Salvar(clienteEntity, usuarioLogadoCurrent.UsuarioId);
            Cliente cliente = _clienteAplicacao.BuscarPorId(clienteEntity.Id);
            return cliente;
        }

        private void SalvarContratoMensalista(Cliente cliente, bool pagamentoCadastro)
        {
            var contratos = !string.IsNullOrEmpty(Session["ListaContratos"]?.ToString()) ? (List<ContratoMensalistaViewModel>)Session["ListaContratos"] : new List<ContratoMensalistaViewModel>();
            ListaContratosMensalistas = contratos;

            var contratoMensalistaEntity = contratos?.Select(x => x.ToEntity())?.ToList() ?? new List<ContratoMensalista>();

            foreach (var contrato in contratoMensalistaEntity)
            {
                contrato.Id = 0;
                contrato.Cliente = cliente;
                contrato.TipoMensalista = _tipoMensalistaAplicacao.BuscarPorId(contrato.TipoMensalista.Id);
                contrato.PagamentoCadastro = pagamentoCadastro;
                foreach (var contratoVeiculo in contrato.Veiculos)
                {
                    var clienteVeiculo = _clienteAplicacao.BuscarPor(x => x.Id == cliente.Id).Select(s => s.Veiculos.FirstOrDefault(f => f.Veiculo.Placa == contratoVeiculo.Veiculo.Placa)).FirstOrDefault();
                    contratoVeiculo.Veiculo = clienteVeiculo.Veiculo;
                    //contratoVeiculo.Veiculo = cliente.Veiculos.FirstOrDefault(x => x.Veiculo.Placa == contratoVeiculo.Veiculo.Placa).Veiculo;
                    contratoVeiculo.ContratoMensalista = contrato;
                }
                _contratoMensalistaAplicacao.Salvar(contrato, false, false);
            }

            SalvarContaCorrenteCliente(cliente);
            _contaCorrenteClienteAplicacao.Salvar(cliente.ContaCorrenteCliente);

            //SalvarCondutorSoftPark
            foreach (var contrato in contratoMensalistaEntity)
            {
                _contratoMensalistaAplicacao.SalvarCondutorSoftPark(contrato);
            }
        }

        private void SalvarContaCorrenteCliente(Cliente cliente)
        {
            ContaCorrenteClienteViewModel contaCorrente = new ContaCorrenteClienteViewModel();
            contaCorrente.ContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes?.Select(x => x)?.ToList() ?? new List<ContaCorrenteClienteDetalheViewModel>();

            cliente.ContaCorrenteCliente = contaCorrente.ToEntity();
            cliente.ContaCorrenteCliente.Cliente = cliente;

            var contaCorrenteEntity = _contaCorrenteClienteAplicacao.PrimeiroPor(x => x.Cliente.Id == cliente.Id) ?? cliente.ContaCorrenteCliente;
            contaCorrenteEntity.ContaCorrenteClienteDetalhes = cliente.ContaCorrenteCliente.ContaCorrenteClienteDetalhes?.Select(x => x)?.ToList() ?? new List<ContaCorrenteClienteDetalhe>();

            foreach (var conta in contaCorrenteEntity.ContaCorrenteClienteDetalhes)
            {
                ContratoMensalista contratoMensalista;
                if (conta.ContratoMensalista.NumeroContrato > 0)
                {
                    contratoMensalista = _contratoMensalistaAplicacao.BuscarPor(x => x.Cliente.Id == cliente.Id && x.NumeroContrato == conta.ContratoMensalista.NumeroContrato).FirstOrDefault();
                }
                else
                {
                    contratoMensalista = _contratoMensalistaAplicacao.BuscarPor(x => x.Cliente.Id == cliente.Id && x.Id == conta.ContratoMensalista.Id).FirstOrDefault();
                }

                if (!ReferenceEquals(contratoMensalista, null))
                {
                    conta.ContratoMensalista = new ContratoMensalista() { Id = contratoMensalista.Id };
                }
            }

            cliente.ContaCorrenteCliente = contaCorrenteEntity;

            //_contaCorrenteClienteAplicacao.Salvar(cliente.ContaCorrenteCliente);
        }

        private void Validacao(ClienteCadastroViewModel model)
        {
            if (model.Cliente.Id <= 0 && model.Cliente.TipoPessoa == TipoPessoa.Fisica)
            {
                if (string.IsNullOrEmpty(model.Cliente.Pessoa.Cpf)
                    || !Validators.IsValid(model.Cliente.Pessoa.Cpf))
                    throw new BusinessRuleException("Informe um CPF para prosseguir com o salvamento do cadastro!");
            }
            else if (model.Cliente.Id <= 0)
            {
                if (string.IsNullOrEmpty(model.Cliente.Cnpj)
                    || !Validators.IsValid(model.Cliente.Cnpj))
                    throw new BusinessRuleException("Informe um CNPJ para prosseguir com o salvamento do cadastro!");
            }

            foreach (var contrato in ListaContrato)
            {
                if (contrato.NumeroVagas <= 0)
                    throw new BusinessRuleException("É necessário informar o número de vagas para prosseguir!");

                var listContrato = _contratoMensalistaAplicacao.BuscarPor(x => x.NumeroContrato == contrato.NumeroContrato);
                if ((contrato.Id <= 0 && listContrato != null && listContrato.Any())
                            || (contrato.Id > 0 && listContrato != null && listContrato.Any() && listContrato.Count(x => x.Id != contrato.Id) > 0))
                    throw new BusinessRuleException("O código de contrato já está em uso, verifique no sistema e/ou entre em contato com o suporte!");

            }
        }

        private void LimparSession()
        {
            ListaContratosMensalistas = new List<ContratoMensalistaViewModel>();
            ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
            Session["ClienteEnderecos"] = new List<EnderecoViewModel>();
            Session["contatos"] = new List<ContatoViewModel>();
            Session["Veiculo"] = new List<VeiculoViewModel>();
            Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = new List<FeriasClienteViewModel>();
            Session["ListaOcorrencia"] = new List<OcorrenciaClienteViewModel>();
            ClienteCadastroViewModel = new ClienteCadastroViewModel();
            ClienteCadastroViewModel.Ocorrencia.Veiculo = new VeiculoViewModel();
        }

        private void AdicionarContaCorrente(ClienteCadastroViewModel model, ContaCorrenteClienteViewModel contaCorrenteClienteViewModel)
        {
            model.Cliente.ContaCorrenteCliente = contaCorrenteClienteViewModel;
            model.Cliente.ContaCorrenteCliente.ContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes?.Select(x => x)?.ToList() ?? new List<ContaCorrenteClienteDetalheViewModel>();
        }

        private void AdicionarContatos(ClienteCadastroViewModel model)
        {
            var contatos = new List<ContatoViewModel>();
            if (!model.Contato.Telefone.IsNullOrEmpyOrWhiteSpace())
            {
                if (model.Cliente.TipoPessoa == TipoPessoa.Fisica)
                {
                    contatos.Add(new ContatoViewModel() { Id = 0, Tipo = TipoContato.Residencial, Telefone = model.Contato.Telefone });
                }
                else
                {
                    contatos.Add(new ContatoViewModel() { Id = 0, Tipo = TipoContato.Comercial, Telefone = model.Contato.Telefone });
                }
            }

            if (!model.Contato.Celular.IsNullOrEmpyOrWhiteSpace())
            {
                contatos.Add(new ContatoViewModel() { Id = 0, Tipo = TipoContato.Celular, Telefone = model.Contato.Celular });
            }

            if (!model.Contato.Telefone.IsNullOrEmpyOrWhiteSpace())
            {
                contatos.Add(new ContatoViewModel() { Id = 0, Tipo = TipoContato.Email, Email = model.Contato.Email });
            }

            Helpers.Contato.DefineNumero(contatos);
            model.Cliente.Pessoa.Contatos = contatos;
        }

        private void AdicionarEnderecos(ClienteCadastroViewModel model)
        {
            var enderecos = new List<EnderecoViewModel>();
            if (model.EnderecoComercial.Cep != null)
            {
                model.EnderecoComercial.Id = 0;
                enderecos.Add(model.EnderecoComercial);
            }
            if (model.EnderecoResidencial.Cep != null)
            {
                model.EnderecoResidencial.Id = 0;
                enderecos.Add(model.EnderecoResidencial);
            }
            model.Cliente.Pessoa.Enderecos.Clear();
            model.Cliente.Pessoa.Enderecos.AddRange(enderecos);
        }

        private void AdicionarModeloMarcaVeiculo(ClienteCadastroViewModel model)
        {
            var veiculos = !string.IsNullOrEmpty(Session["Veiculo"]?.ToString()) ? (List<VeiculoViewModel>)Session["Veiculo"] : new List<VeiculoViewModel>();
            foreach (var veiculo in veiculos)
            {
                var entityVeiculo = AutoMapper.Mapper.Map<Veiculo>(veiculo);
                entityVeiculo.Id = 0;

                if (entityVeiculo.Modelo.Marca.Id <= 0)
                {
                    entityVeiculo.Modelo.Marca = _marcaAplicacao.SalvarComRetorno(entityVeiculo.Modelo.Marca);
                    veiculo.Modelo.Marca.Id = entityVeiculo.Modelo.Marca.Id;
                }

                if (entityVeiculo.Modelo.Id <= 0)
                {
                    entityVeiculo.Modelo = _modeloAplicacao.SalvarComRetorno(entityVeiculo.Modelo);
                    veiculo.Modelo.Id = entityVeiculo.Modelo.Id;
                }
            }

            model.Cliente.Veiculos = Helpers.Veiculo.RetornaClienteVeiculos(model.Cliente.Id, veiculos);
        }

        private void SalvarOcorrencias(Cliente cliente)
        {
            var ocorrenciasViewModel = !string.IsNullOrEmpty(Session["ListaOcorrencia"]?.ToString()) ? (List<OcorrenciaClienteViewModel>)Session["ListaOcorrencia"] : new List<OcorrenciaClienteViewModel>();
            var clienteBase = _clienteAplicacao.BuscarPorId(cliente.Id);
            
            var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;

            foreach (var ocorrencia in ocorrenciasViewModel)
            {
                ocorrencia.Id = 0;
                ocorrencia.Cliente = new ClienteViewModel { Id = cliente.Id };
                if (ocorrencia.Veiculo != null) {
                    var clienteVeiculosSalvos = _clienteAplicacao.BuscarPor(x=>x.Id == cliente.Id).Select(s=>s.Veiculos.FirstOrDefault(f=>f.Veiculo.Placa == ocorrencia.Veiculo.Placa)).FirstOrDefault();

                    //ocorrencia.Veiculo.Id = clienteVeiculosSalvos?.FirstOrDefault(x => x.Veiculo.Placa == ocorrencia.Veiculo.Placa)?.Veiculo != null
                    //                        ? clienteVeiculosSalvos.FirstOrDefault(x => x.Veiculo.Placa == ocorrencia.Veiculo.Placa).Veiculo.Id ?? 0
                    //                        : ocorrencia.Veiculo.Id;

                    //if (clienteVeiculosSalvos != null && clienteVeiculosSalvos.FirstOrDefault(x => x.Veiculo.Placa == ocorrencia.Veiculo.Placa) != null)
                    //{
                    //    ocorrencia.Veiculo.Id = clienteVeiculosSalvos.FirstOrDefault(x => x.Veiculo.Placa == ocorrencia.Veiculo.Placa).Veiculo.Id;
                    //}

                    if (clienteVeiculosSalvos != null)
                    {
                        ocorrencia.Veiculo.Id = clienteVeiculosSalvos.Veiculo.Id;
                    }
                }

                var entityOcorrencia = AutoMapper.Mapper.Map<OcorrenciaCliente>(ocorrencia);

                entityOcorrencia.FuncionarioAtribuido = _funcionarioAplicacao.BuscarPorId(ocorrencia.FuncionarioAtribuido.Pessoa.Id) ?? null;
                entityOcorrencia.Cliente = clienteBase;
                entityOcorrencia.Unidade = entityOcorrencia.Unidade != null && entityOcorrencia.Unidade.Id > 0 ? _unidadeAplicacao.BuscarPorId(entityOcorrencia.Unidade.Id) ?? null : null;
                entityOcorrencia.Veiculo = entityOcorrencia.Veiculo != null && entityOcorrencia.Veiculo.Id > 0 ? _veiculoAplicacao.BuscarPorId(entityOcorrencia.Veiculo.Id) ?? null : null;

                _ocorrenciaAplicacao.SalvarDadosOcorrenciaComNotificacao(entityOcorrencia, usuarioLogadoCurrent.UsuarioId);
            }
        }

        private void SalvarContaCorrente(Cliente cliente)
        {
            if (cliente.ContaCorrenteCliente == null)
                cliente.ContaCorrenteCliente = new ContaCorrenteCliente { Cliente = _clienteAplicacao.BuscarPorId(cliente.Id) };
            else
                cliente.ContaCorrenteCliente.Cliente = cliente;

            var contaCorrenteEntity = _contaCorrenteClienteAplicacao.PrimeiroPor(x => x.Cliente.Id == cliente.Id) ?? cliente.ContaCorrenteCliente;
            contaCorrenteEntity.ContaCorrenteClienteDetalhes = cliente.ContaCorrenteCliente.ContaCorrenteClienteDetalhes?.Select(x => x)?.ToList() ?? new List<ContaCorrenteClienteDetalhe>();

            _contaCorrenteClienteAplicacao.Salvar(contaCorrenteEntity);
        }

        private void SalvarFerias(Cliente cliente)
        {
            foreach (var ferias in ListaFerias.Where(x => x.Id == 0 || x.IsEdited))
            {
                foreach (var item in ferias.ListaDataCompetenciaPeriodo)
                {
                    if (_feriasClienteAplicacao.VerificaExistenciaBoletoMensalista(ferias.Cliente.Id, item)
                    && ListaFerias.Any(x => x.Id == 0))
                    {
                        throw new BusinessRuleException($"Não é possível incluir/editar/excluir as férias do cliente pois, já existe um boleto para a competência [{item.ToShortDateString()}]");
                    }
                }
            }

            _feriasClienteAplicacao.SalvarFeriasCliente(ListaFerias, cliente);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult AdicionarContaCorrenteClienteCadastro(int tipoOperacaoId, string valor, DateTime dataCompetencia, int contratoId, int numeroContrato)
        {
            if (ListaContaCorrenteClienteDetalhes.Any(x => (int)x.TipoOperacaoContaCorrente == tipoOperacaoId))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionado informações para este Tipo Benefício");

            var tipoBeneficioVM = (TipoOperacaoContaCorrente)tipoOperacaoId;

            var contaCorrenteCliente = new ContaCorrenteClienteDetalheViewModel(tipoBeneficioVM, dataCompetencia, valor, contratoId, numeroContrato);
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Add(contaCorrenteCliente);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_ContaCorrenteGridDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult EditarContaCorrenteCliente(int tipoBeneficioId)
        {
            var item = ListaContaCorrenteClienteDetalhes.FirstOrDefault(x => (int)x.TipoOperacaoContaCorrente == tipoBeneficioId);
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Remove(item);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_ContaCorrenteGridDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        private ActionResult SalvarDadosCliente(ClienteViewModel model, ContaCorrenteClienteViewModel contaCorrenteClienteViewModel, string view)
        {
            try
            {
                if (model.Id <= 0 && model.TipoPessoa == TipoPessoa.Fisica)
                {
                    if (string.IsNullOrEmpty(model.Pessoa.Cpf)
                        || !Validators.IsValid(model.Pessoa.Cpf))
                        throw new BusinessRuleException("Informe um CPF para prosseguir com o salvamento do cadastro!");
                }
                else if (model.Id <= 0)
                {
                    if (string.IsNullOrEmpty(model.Cnpj)
                        || !Validators.IsValid(model.Cnpj))
                        throw new BusinessRuleException("Informe um CNPJ para prosseguir com o salvamento do cadastro!");
                }

                var enderecos = !string.IsNullOrEmpty(Session["ClienteEnderecos"]?.ToString()) ? (List<EnderecoViewModel>)Session["ClienteEnderecos"] : new List<EnderecoViewModel>();
                var contatos = !string.IsNullOrEmpty(Session["contatos"]?.ToString()) ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();
                var veiculos = !string.IsNullOrEmpty(Session["Veiculo"]?.ToString()) ? (List<VeiculoViewModel>)Session["Veiculo"] : new List<VeiculoViewModel>();
                Helpers.Contato.DefineNumero(contatos);

                var pessoaBase = _pessoaAplicacao.BuscarPorId(model.Pessoa?.Id ?? 0);

                model.Pessoa.Contatos = contatos;

                model.Pessoa.Enderecos.Clear();
                model.Pessoa.Enderecos.AddRange(enderecos);

                model.Veiculos = Helpers.Veiculo.RetornaClienteVeiculos(model.Id, veiculos);
                
                var clienteEntity = model.ToEntity();
                SalvarFerias(clienteEntity);
                SalvarContaCorrenteCliente(clienteEntity);
                var ocorrencias = !string.IsNullOrEmpty(Session["ListaOcorrencia"]?.ToString()) ? (List<OcorrenciaClienteViewModel>)Session["ListaOcorrencia"] : new List<OcorrenciaClienteViewModel>();
                clienteEntity.Ocorrencias = new List<OcorrenciaCliente>();
                foreach (var ocorrencia in ocorrencias)
                {
                    clienteEntity.Ocorrencias.Add(ocorrencia.ToEntity());
                }
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _clienteAplicacao.Salvar(clienteEntity, usuarioLogadoCurrent.UsuarioId);
               
                //_clienteAplicacao.Salvar(clienteEntity);
                //SalvarOcorrencias(clienteEntity);
                
                ModelState.Clear();

                ListaContratosMensalistas = new List<ContratoMensalistaViewModel>();
                ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
                Session["ClienteEnderecos"] = new List<EnderecoViewModel>();
                Session["contatos"] = new List<ContatoViewModel>();
                Session["Veiculo"] = new List<VeiculoViewModel>();
                Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = new List<FeriasClienteViewModel>();
                Session["ListaOcorrencia"] = new List<OcorrenciaClienteViewModel>();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (SoftparkIntegrationException siex)
            {
                //model = new ClienteViewModel();
                //ListaContratosMensalistas = new List<ContratoMensalistaViewModel>();
                //ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
                //Session["ClienteEnderecos"] = new List<EnderecoViewModel>();
                //Session["contatos"] = new List<ContatoViewModel>();
                //Session["Veiculo"] = new List<VeiculoViewModel>();
                //Session[$"ListaFerias_{UsuarioLogado.UsuarioId}"] = new List<FeriasClienteViewModel>();

                DadosModal = new DadosModal
                {
                    Titulo = "Atenção - Softpark Integration",
                    Mensagem = "Ocorreu um erro ao salvar, carregue o cadastro novamente. <br/><br/>" + siex.Message,
                    TipoModal = TipoModal.Danger,
                    RedirectUrl = $"/{ControllerName}/Edit/{model.Id}"
                };
                return View(view, model);
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View(view, model);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View(view, model);
            }
            return View(view);
        }

        public JsonResult CarregarUnidades(List<int> unidadesId)
        {
            var lista = new List<UnidadeViewModel>
            {
                new UnidadeViewModel
                {
                    Nome = @"Selecione uma unidade.",
                    Id = 0
                }
            };

            var unidades = _unidadeAplicacao.BuscarPor(x => unidadesId.Contains(x.Id));
            if (!unidades.Any()) return Json(new SelectList(lista, "Id", "Nome"));

            foreach (var unid in unidades)
            {
                lista.Add(new UnidadeViewModel(unid));
            }

            return Json(new SelectList(lista, "Id", "Nome"));
        }

        public ActionResult AdicionarContrato(string jsonContratos)
        {
            if (!string.IsNullOrEmpty(jsonContratos))
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                var contratos = JsonConvert.DeserializeObject<List<ContratoMensalistaViewModel>>(jsonContratos, dateTimeConverter);
                ListaContrato = contratos;

                var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContratoMensalista", ListaContrato);
                return Json(new
                {
                    Grid = grid,
                });
            }

            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult EditarContrato(int contratoId)
        {
            var item = ListaContrato.FirstOrDefault(x => x.Id == contratoId);
            var ListaContratoAUX = ListaContrato;

            ListaContratoAUX.Remove(item);
            ListaContrato = ListaContratoAUX;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContratoMensalista", ListaContrato);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        public ActionResult RemoverContaCorrenteCliente(int tipoBeneficioId)
        {
            var item = ListaContaCorrenteClienteDetalhes.FirstOrDefault(x => (int)x.TipoOperacaoContaCorrente == tipoBeneficioId);
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Remove(item);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_ContaCorrenteGridDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        #region Férias Cliente
        public JsonResult BuscaContratosCliente(int idCliente)
        {
            var contratos = _contratoMensalistaAplicacao.BuscarPorCliente(idCliente)?.ToList() ?? new List<ContratoMensalista>();
            var listaContratos = contratos?.Select(x => new ContratoMensalistaViewModel { Id = x.Id, NumeroContrato = x.NumeroContrato, NumeroVagas = x.NumeroVagas })?.ToList() ?? new List<ContratoMensalistaViewModel>();

            return Json(new
            {
                Contratos = listaContratos,
                TotalVagas = contratos?.Sum(x => x.NumeroVagas) ?? 0
            });
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult AdicionarFerias(string jsonDados)
        {
            var grid = string.Empty;

            if (!string.IsNullOrEmpty(jsonDados))
            {
                try
                {
                    var format = "dd/MM/yyyy"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                    var dados = JsonConvert.DeserializeObject<FeriasClienteViewModel>(jsonDados, dateTimeConverter);

                    dados.UsuarioCadastro = dados.UsuarioCadastro == null || dados.UsuarioCadastro.Id == 0 ? new UsuarioViewModel { Id = UsuarioLogado.UsuarioId } : dados.UsuarioCadastro;

                    foreach (var item in ListaFerias)
                    {
                        if (item.DataInicio <= dados.DataInicio && item.DataFim >= dados.DataInicio
                            || item.DataInicio <= dados.DataFim && item.DataFim >= dados.DataFim)
                        {
                            return Json(new Resultado<object>()
                            {
                                Sucesso = false,
                                TipoModal = TipoModal.Warning.ToDescription(),
                                Titulo = "Atenção",
                                Mensagem = $"Não é possível cadastrar as férias entre períodos já informados! Verifique as férias cadastradas na listagem para um novo preenchimento."
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    foreach (var data in dados.ListaDataCompetenciaPeriodo)
                    {
                        if (_feriasClienteAplicacao.VerificaExistenciaBoletoMensalista(dados.Cliente.Id, data))
                        {
                            return Json(new Resultado<object>()
                            {
                                Sucesso = false,
                                TipoModal = TipoModal.Danger.ToDescription(),
                                Titulo = "Atenção",
                                Mensagem = $"Não é possível cadastrar as férias do cliente pois, já existe um boleto para a competência [{data.ToShortDateString()}]"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var ferias = ListaFerias;
                    ferias.Add(dados);
                    ListaFerias = ferias;

                    grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridFerias", ListaFerias);

                    return Json(new
                    {
                        Grid = grid
                    });
                }
                catch (Exception ex)
                {
                    return Json(new Resultado<object>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Danger.ToDescription(),
                        Titulo = "Atenção",
                        Mensagem = "Ocorreu um erro ao adicionar as férias: " + ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult EditarFerias(int index, int idFerias)
        {
            var grid = string.Empty;
            var item = idFerias > 0 ? ListaFerias.FirstOrDefault(x => x.Id == idFerias) : ListaFerias[index];
            var ListaFeriasAUX = ListaFerias;

            foreach (var data in item.ListaDataCompetenciaPeriodo)
            {
                if (_feriasClienteAplicacao.VerificaExistenciaBoletoMensalista(item.Cliente.Id, data))
                {
                    return Json(new Resultado<object>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Danger.ToDescription(),
                        Titulo = "Atenção",
                        Mensagem = $"Não é possível editar as férias do cliente pois, já existe um boleto para a competência [{data.ToShortDateString()}]"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            ListaFeriasAUX.Remove(item);
            ListaFerias = ListaFeriasAUX;

            item.IsEdited = true;

            grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridFerias", ListaFerias);

            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult ExcluirFerias(int index, int idFerias)
        {
            var grid = string.Empty;
            var item = idFerias > 0 ? ListaFerias.FirstOrDefault(x => x.Id == idFerias) : ListaFerias[index];
            var ListaFeriasAUX = ListaFerias;

            foreach (var data in item.ListaDataCompetenciaPeriodo)
            {
                if (_feriasClienteAplicacao.VerificaExistenciaBoletoMensalista(item.Cliente.Id, data))
                {
                    return Json(new Resultado<object>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Danger.ToDescription(),
                        Titulo = "Atenção",
                        Mensagem = $"Não é possível remover as férias do cliente pois, já existe um boleto para a competência [{data.ToShortDateString()}]"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            ListaFeriasAUX.Remove(item);
            ListaFerias = ListaFeriasAUX;

            grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridFerias", ListaFerias);

            return Json(new
            {
                Grid = grid
            });
        }
        #endregion
    }
}