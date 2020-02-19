using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Core.Validators;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Models;

namespace Portal.Controllers
{
    public class ContratoMensalistaController : GenericController<ContratoMensalista>
    {
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly ITipoMensalistaAplicacao _tipoMensalistaAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IContratoMensalistaAplicacao _contratoMensalistaAplicacao;
        private readonly IContratoMensalistaVeiculoAplicacao _contratoMensalistaVeiculoAplicacao;
        private readonly IClienteVeiculoAplicacao _clienteVeiculoAplicacao;
        private readonly IVeiculoAplicacao _veiculoAplicacao;
        private readonly ITabelaPrecoMensalistaAplicacao _tabelaPrecoMensalistaAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public ContratoMensalistaController(IContratoMensalistaAplicacao contratoMensalistaAplicacao,
                                            IClienteAplicacao clienteAplicacao,
                                            ITipoMensalistaAplicacao tipoMensalistaAplicacao,
                                            IUnidadeAplicacao unidadeAplicacao,
                                            IContratoMensalistaVeiculoAplicacao contratoMensalistaVeiculoAplicacao,
                                            IClienteVeiculoAplicacao clienteVeiculoAplicacao,
                                            IVeiculoAplicacao veiculoAplicacao,
                                            ITabelaPrecoMensalistaAplicacao tabelaPrecoMensalistaAplicacao,
                                            IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
                                            INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            _contratoMensalistaAplicacao = contratoMensalistaAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _tipoMensalistaAplicacao = tipoMensalistaAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _contratoMensalistaAplicacao = contratoMensalistaAplicacao;
            _contratoMensalistaVeiculoAplicacao = contratoMensalistaVeiculoAplicacao;
            _clienteVeiculoAplicacao = clienteVeiculoAplicacao;
            _veiculoAplicacao = veiculoAplicacao;
            _tabelaPrecoMensalistaAplicacao = tabelaPrecoMensalistaAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
        }

        public List<Unidade> ListaUnidade => _unidadeAplicacao.ListarOrdenadoSimplificado()?.ToList();
        public List<TipoMensalistaViewModel> ListaTipoMensalista => AutoMapper.Mapper.Map<List<TipoMensalista>, List<TipoMensalistaViewModel>>(_tipoMensalistaAplicacao.BuscarPor(t => t.Ativo)?.ToList());
        public List<VeiculoViewModel> ListaVeiculos { get; set; }
        public List<ContratoMensalistaVeiculoViewModel> ListaContratoMensalistaVeiculos { get; set; }
        public List<TabelaPrecoMensalista> ListaTabelaPrecoMensalista { get; set; }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            ViewBag.PrintFlag = false;
            Session["VeiculosAdicionados"] = null;
            return View();
        }

        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            ViewBag.PrintFlag = true;
            var contratoMensalistaDM = _contratoMensalistaAplicacao.BuscarPorId(id);

            var contratoMensalistaVM = new ContratoMensalistaViewModel(contratoMensalistaDM);

            ListaVeiculos = new List<VeiculoViewModel>();
            Session["VeiculosAdicionados"] = contratoMensalistaVM.Veiculos.ToList();
            return View("Index", contratoMensalistaVM);
        }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            try
            {
                ViewBag.PrintFlag = false;
                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
            }

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            ViewBag.PrintFlag = false;
            _contratoMensalistaAplicacao.ExcluirPorId(id);

            ModelState.Clear();
            GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);

            return View("Index");
        }

        public ActionResult FiltrarCampos(string contrato, string nome, int pagina = 1)
        {
            var grid = string.Empty;
            var resultado = new Resultado<ContratoMensalistaViewModel>();
            var lista = new List<ContratoMensalista>();
            PaginacaoGenericaViewModel paginacao = new PaginacaoGenericaViewModel();

            if (string.IsNullOrEmpty(contrato) && string.IsNullOrEmpty(nome))
            {
                var quantidadeFuncionarios = _contratoMensalistaAplicacao.Contar();
                paginacao = new PaginacaoGenericaViewModel(50, pagina, quantidadeFuncionarios);

                lista = _contratoMensalistaAplicacao.BuscarPorIntervaloOrdenadoPeloNomeDoCliente(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
            else if (!string.IsNullOrEmpty(nome) && Validators.IsValid(contrato))
            {
                lista = _contratoMensalistaAplicacao.BuscarPor(x => x.Cliente.Pessoa.Nome.Contains(nome) || x.Cliente.NomeFantasia.Contains(nome) &&
                                                                    x.NumeroContrato.ToString() == contrato).ToList();

                paginacao = new PaginacaoGenericaViewModel(50, pagina, lista.Count);
                lista = lista.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
            else if (!string.IsNullOrEmpty(nome))
            {
                lista = _contratoMensalistaAplicacao.BuscarPor(x => x.Cliente.Pessoa.Nome.Contains(nome) || x.Cliente.NomeFantasia.Contains(nome)).ToList();

                paginacao = new PaginacaoGenericaViewModel(50, pagina, lista.Count);
                lista = lista.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
            else if (!string.IsNullOrEmpty(contrato))
            {
                lista = _contratoMensalistaAplicacao.BuscarPor(x => x.NumeroContrato.ToString() == contrato).ToList();

                paginacao = new PaginacaoGenericaViewModel(50, pagina, lista.Count);
                lista = lista.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
            else
            {
                resultado = new Resultado<ContratoMensalistaViewModel>
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Filtro de Registros",
                    Mensagem = $"Contrato {contrato} Não Encontrado!"
                };
            }

            var dadosListaViewModel = AutoMapper.Mapper.Map<List<ContratoMensalistaViewModel>>(lista)
                                        .OrderBy(x => TipoPessoa.Fisica == x.Cliente.TipoPessoa ? x.Cliente.Pessoa.Nome : x.Cliente.NomeFantasia)?
                                        .ToList();

            ViewBag.Paginacao = paginacao;

            grid = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContratoMensalista", dadosListaViewModel);
            return new JsonResult { Data = new { grid, resultado } };
        }

        public ActionResult ImprimirContrato(int contratoMensalistaID)
        {
            var obj = _contratoMensalistaAplicacao.BuscarPorId(contratoMensalistaID);

            var objviewmodel = new ContratoMensalistaViewModel(obj);
            return View("_ImprimirContrato", objviewmodel);
        }

        public JsonResult BuscarVeiculos(int idCliente)
        {
            ViewBag.PrintFlag = false;
            var cliente =
              _clienteAplicacao.BuscarPorId(idCliente);

            List<ClienteVeiculo> clienteVeiculos = new List<ClienteVeiculo>(cliente?.Veiculos ?? new List<ClienteVeiculo>());
            var list = new ClienteVeiculoViewModel().ListaVeiculos(clienteVeiculos);
            List<ClienteVeiculoViewModel> clienteVeiculosVM = new List<ClienteVeiculoViewModel>(list);

            var veiculos = Helpers.Veiculo.RetornaVeiculos(clienteVeiculosVM);

            return Json(veiculos, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AtualizarVeiculos(List<VeiculoViewModel> veiculos)
        {
            ViewBag.PrintFlag = false;

            ListaVeiculos = new List<VeiculoViewModel>();

            if (veiculos != null)
            {
                foreach (var veiculo in veiculos)
                {
                    var item = new VeiculoViewModel
                    {
                        Id = veiculo?.Id ?? 0,
                        Placa = veiculo?.Placa,
                        Modelo = veiculo?.Modelo,
                        Cor = veiculo?.Cor,
                        Ano = veiculo?.Ano
                    };

                    item = veiculo?.Id > 0 ? AutoMapper.Mapper.Map<Veiculo, VeiculoViewModel>(_veiculoAplicacao.BuscarPorId(veiculo?.Id??0)) : item;
                    if (item != null)
                        ListaVeiculos.Add(item);
                }
            }

            Session["VeiculosAdicionados"] = ListaVeiculos;
            return PartialView("_GridVeiculos", ListaVeiculos);
        }

        public JsonResult BuscarClienteVeiculos()
        {
            var veiculos = (List<VeiculoViewModel>)Session["VeiculosAdicionados"];
            return Json(veiculos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CarregarUnidades(int clienteId)
        {
            var lista = new List<UnidadeViewModel>
            {
                new UnidadeViewModel
                {
                    Nome = @"Selecione uma unidade.",
                    Id = 0
                }
            };

            var busca = _clienteAplicacao.BuscarPor(x => x.Id == clienteId)?.SelectMany(c => c.Unidades)?.Select(uc => new UnidadeViewModel(uc.Unidade))?.Distinct()?.OrderBy(x => x.Nome)?.ToList();

            if (!busca.Any()) return Json(new SelectList(lista, "Id", "Nome"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Nome"));
        }

        public JsonResult CarregarTabelaPrecoMensalista(int unidadeId)
        {
            var lista = new List<TabelaPrecoMensalista>
            {
                new TabelaPrecoMensalista
                {
                    Nome = @"Selecione uma tabela.",
                    Id = 0
                }
            };

            var busca = _tabelaPrecoMensalistaAplicacao.BuscarPor(x => x.TabelaPrecoUnidade.Count(y => y.Unidade.Id == unidadeId) > 0).ToList();

            if (!busca.Any()) return Json(new SelectList(lista, "Id", "Nome"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Nome"));
        }

        [HttpPost]
        public JsonResult VerificacaoBloqueioReferencia(ContratoMensalistaViewModel contratoMensalista)
        {
            var divModalBloq = string.Empty;
            var data = contratoMensalista.DataVencimento;
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {
                var contratoEntity = _contratoMensalistaAplicacao.BuscarPorId(contratoMensalista.Id);

                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado
                    && (contratoMensalista.Id == 0 
                        || (contratoEntity.Id > 0 && contratoMensalista.Id > 0 
                            && (contratoMensalista.NumeroVagas != contratoEntity.NumeroVagas || Convert.ToDecimal(contratoMensalista.Valor) != contratoEntity.Valor))))
                {
                    var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao, contratoMensalista.Id, Entidades.ContratoMensalista, data, new Usuario { Id = UsuarioLogado.UsuarioId });
                    var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                    {
                        IdNotificacao = retorno.Key,
                        StatusDesbloqueioLiberacao = retorno.Value,
                        IdRegistro = contratoMensalista.Id,
                        EntidadeRegistro = Entidades.ContratoMensalista,
                        DataReferencia = data,
                        UsuarioLogadoId = UsuarioLogado.UsuarioId,
                        LiberacaoUtilizada = false
                    };
                    TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}"] = modelLiberacao;
                    TempData.Keep();

                    if (retorno.Value != StatusDesbloqueioLiberacao.Aprovado)
                    {
                        divModalBloq = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalLiberacaoBloqueioReferencia", modelLiberacao);
                        throw new BlockedReferenceDateException();
                    }
                }
            }
            catch (BlockedReferenceDateException)
            {
                DadosValidacaoDesbloqueioReferenciaModal = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                {
                    Titulo = "Atenção",
                    TipoModal = TipoModal.Info
                };
                return new JsonResult
                {
                    Data = new
                    {
                        Bloqueio = true,
                        Modal = divModalBloq,
                        Liberacao = liberacao,
                        Status = liberacao.StatusDesbloqueioLiberacao.ToDescription()
                    }
                };
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult
            {
                Data = new
                {
                    Bloqueio = false,
                    Modal = divModalBloq,
                    Liberacao = liberacao,
                    Status = liberacao.StatusDesbloqueioLiberacao.ToDescription()
                }
            };
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ContratoMensalistaViewModel contratoMensalista)
        {
            try
            {
                var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}"]
                                : null;

                var cliente = _clienteAplicacao.BuscarPorId(contratoMensalista.Cliente.Id);
                contratoMensalista.Cliente = new ClienteViewModel(cliente);
                contratoMensalista.UnidadeSelecionada = contratoMensalista.Unidade?.Id.ToString() ?? string.Empty;

                if (contratoMensalista.NumeroVagas <= 0)
                    throw new BusinessRuleException("É necessário informar o número de vagas para prosseguir!");

                var listaContrato = _contratoMensalistaAplicacao.BuscarPor(x => x.NumeroContrato == contratoMensalista.NumeroContrato);
                if ((contratoMensalista.Id <= 0 && listaContrato != null && listaContrato.Any())
                            || (contratoMensalista.Id > 0 && listaContrato != null && listaContrato.Any() && listaContrato.Count(x => x.Id != contratoMensalista.Id) > 0))
                    throw new BusinessRuleException("O código de contrato já está em uso, verifique no sistema e/ou entre em contato com o suporte!");

                ViewBag.PrintFlag = false;
                
                var contratoMensalistaEntity = AutoMapper.Mapper.Map<ContratoMensalista>(contratoMensalista);

                ListaVeiculos = (List<VeiculoViewModel>)Session["VeiculosAdicionados"];
                if (ListaVeiculos != null)
                {
                    ListaContratoMensalistaVeiculos = new List<ContratoMensalistaVeiculoViewModel>();
                    foreach (var veiculoVM in ListaVeiculos)
                    {
                        ContratoMensalistaVeiculoViewModel contratoMensVeiculo = new ContratoMensalistaVeiculoViewModel
                        {
                            Contrato = contratoMensalista,
                            Veiculo = veiculoVM
                        };
                        ListaContratoMensalistaVeiculos.Add(contratoMensVeiculo);
                    }
                    contratoMensalistaEntity.Veiculos = AutoMapper.Mapper.Map<List<ContratoMensalistaVeiculoViewModel>, List<ContratoMensalistaVeiculo>>(ListaContratoMensalistaVeiculos);
                }
                if (contratoMensalistaEntity.DataFim == DateTime.MinValue)
                {
                    contratoMensalistaEntity.DataFim = null;
                }
                contratoMensalistaEntity.Cliente = cliente;
                _contratoMensalistaAplicacao.Salvar(contratoMensalistaEntity, true);

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.ContratoMensalista && liberacao.IdRegistro == contratoMensalista.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.ContratoMensalista}");
                }

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (SoftparkIntegrationException sf)
            {
                CriarModalAvisoComRetornoParaIndex(sf.Message);
            }
            catch (MonthlyContractPaymentException br)
            {
                CriarModalConfirmacao(br.Message, "Pagamento - Contrato Mensalista", null, "Sim, desejo!", contratoMensalista.Id, null, "EfetuarPagamentoCadastro();");
                return View("Index", contratoMensalista);
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", contratoMensalista);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
                return View("Index", contratoMensalista);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarCliente(string descricao)
        {
            var lista = _clienteAplicacao.BuscarPor(c => c.Pessoa.Nome.Contains(descricao) || c.NomeFantasia.Contains(descricao));

            return Json(lista.Select(c => new
            {
                c.Id,
                Descricao = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
            }));
        }

        [HttpPost]
        public JsonResult VerificarSeNumeroContratoExiste(int idContrato, string codigoContrato)
        {
            var resultado = new Resultado<ContratoMensalistaViewModel>();
            var existe = false;

            try
            {
                var listaContrato = _contratoMensalistaAplicacao.BuscarPor(x => x.NumeroContrato == Convert.ToInt64(codigoContrato));
                existe = (idContrato <= 0 && listaContrato != null && listaContrato.Any())
                            || (idContrato > 0 && listaContrato != null && listaContrato.Any() && listaContrato.Count(x => x.Id != idContrato) > 0);
            }
            catch (Exception ex)
            {
                resultado = new Resultado<ContratoMensalistaViewModel>
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Filtro de Registros",
                    Mensagem = $"Contrato {codigoContrato} Não Encontrado! <br> Erro: [{ex.Message}]."
                };
            }
            return new JsonResult { Data = new { existe, resultado } };
        }

        #region NaoUsados
        [HttpPost]
        public void RemoveVeiculo(string index)
        {
            if (Session["VeiculosAdicionados"] != null)
            {
                var veiculos = (List<VeiculoViewModel>)Session["VeiculosAdicionados"];
                veiculos.RemoveAt(int.Parse(index));
                Session["VeiculosAdicionados"] = veiculos;
            }
        }
        [HttpPost]
        public void AdicionaVeiculo(string index, string veiculo)
        {
            if (!string.IsNullOrEmpty(veiculo))
            {
                var veiculos = new List<VeiculoViewModel>();
                if (Session["VeiculosAdicionados"] != null)
                {
                    veiculos = (List<VeiculoViewModel>)Session["VeiculosAdicionados"];
                }

                if (int.Parse(index) < 0)
                {
                    veiculos.AddRange(JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo));
                }
                else
                {
                    veiculos[int.Parse(index)] = JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo).First();
                }
                Session["VeiculosAdicionados"] = veiculos;
            }
        }

        //public void AlimentarViewBag()
        //{
        //    var listaCliente = AutoMapper.Mapper.Map<List<Cliente>, List<ClienteViewModel>>(clienteAplicacao.Buscar().OrderBy(x => TipoPessoa.Fisica == x.TipoPessoa ? x.Pessoa.Nome : x.NomeFantasia).ToList());
        //    ViewBag.ListaClientes = listaCliente;
        //}
        #endregion
    }
}