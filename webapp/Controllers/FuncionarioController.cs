using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class FuncionarioController : GenericController<Funcionario>
    {
        public List<FuncionarioViewModel> ListaFuncionarios => AutoMapper.Mapper.Map<List<Funcionario>, List<FuncionarioViewModel>>(_funcionarioAplicacao.Buscar().ToList());

        public List<FuncionarioViewModel> ListaSupervisor => _funcionarioAplicacao.BuscarPor(x => x.Cargo.Nome == CargoFuncionario.Supervisor.ToDescription()).Select(x => new FuncionarioViewModel(x)).ToList();

        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListaUnidade().Select(x => new UnidadeViewModel(x)).ToList();

        public List<CargoViewModel> ListaCargo => _cargoAplicacao.ListarCargo().Select(x => new CargoViewModel(x)).ToList();

        public List<ItemFuncionarioDetalheViewModel> ListaFuncionarioItens
        {
            get => (List<ItemFuncionarioDetalheViewModel>)Session["ListaFuncionarioItens"] ?? new List<ItemFuncionarioDetalheViewModel>();
            set => Session["ListaFuncionarioItens"] = value;
        }

        public List<BeneficioFuncionarioDetalheViewModel> ListaBeneficioFuncionarioDetalhes
        {
            get => (List<BeneficioFuncionarioDetalheViewModel>)Session["ListaBeneficioItemDetalhes"] ?? new List<BeneficioFuncionarioDetalheViewModel>();
            set => Session["ListaBeneficioItemDetalhes"] = value;
        }

        public List<OcorrenciaFuncionarioDetalheViewModel> ListaOcorrenciaFuncionarioDetalhe
        {
            get => (List<OcorrenciaFuncionarioDetalheViewModel>)Session["ListaOcorrenciaFuncionarioDetalhe"] ?? new List<OcorrenciaFuncionarioDetalheViewModel>();
            set => Session["ListaOcorrenciaFuncionarioDetalhe"] = value;
        }

        public bool IsEdicao = false;

        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly IPessoaAplicacao _pessoaAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ICargoAplicacao _cargoAplicacao;
        private readonly IMaterialAplicacao _materialAplicacao;
        private readonly ITipoBeneficioAplicacao _tipoBeneficioAplicacao;
        private readonly ITipoOcorrenciaAplicacao _tipoOcorrenciaAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IControlePontoAplicacao _controlePontoAplicacao;
        private readonly IPlanoCarreiraAplicacao _planoCarreiraAplicacao;

        public FuncionarioController(
            IFuncionarioAplicacao funcionarioAplicacao,
            IEmpresaAplicacao empresaAplicacao,
            IPessoaAplicacao pessoaAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            ICargoAplicacao cargoAplicacao,
            IMaterialAplicacao materialAplicacao
            , ITipoBeneficioAplicacao tipoBeneficioAplicacao
            , ITipoOcorrenciaAplicacao tipoOcorrenciaAplicacao
            , IUsuarioAplicacao usuarioAplicacao
            , IControlePontoAplicacao controlePontoAplicacao
            , IPlanoCarreiraAplicacao planoCarreiraAplicacao
        )
        {
            Aplicacao = funcionarioAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _empresaAplicacao = empresaAplicacao;
            _pessoaAplicacao = pessoaAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _cargoAplicacao = cargoAplicacao;
            _materialAplicacao = materialAplicacao;
            _tipoBeneficioAplicacao = tipoBeneficioAplicacao;
            _tipoOcorrenciaAplicacao = tipoOcorrenciaAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _controlePontoAplicacao = controlePontoAplicacao;
            _planoCarreiraAplicacao = planoCarreiraAplicacao;

            ViewBag.ListaMaterial = _materialAplicacao.Buscar().ToList();
            ViewBag.ListaTipoBeneficio = _tipoBeneficioAplicacao.BuscarPor(x => x.Ativo);
            ViewBag.ListaTipoOcorrencia = _tipoOcorrenciaAplicacao.Buscar().ToList();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaFuncionarioItens = new List<ItemFuncionarioDetalheViewModel>();
            ListaBeneficioFuncionarioDetalhes = new List<BeneficioFuncionarioDetalheViewModel>();
            ListaOcorrenciaFuncionarioDetalhe = new List<OcorrenciaFuncionarioDetalheViewModel>();
            Session["contatos"] = new List<ContatoViewModel>();
            Session["ItensSelecionados"] = new List<ItemFuncionarioDetalheViewModel>();

            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(
            FuncionarioViewModel model,
            ItemFuncionarioViewModel itemFuncionario,
            BeneficioFuncionarioViewModel beneficioFuncionario,
            OcorrenciaFuncionarioViewModel ocorrenciaFuncionario)
        {
            try
            {
                _funcionarioAplicacao.ValidarTipoEscala(model.Id);
                Session["ItensSelecionados"] = new List<ItemFuncionarioDetalheViewModel>();

                var contatos = !string.IsNullOrEmpty(Session["contatos"]?.ToString()) ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();
                var pessoa = _pessoaAplicacao.BuscarPorId(model.Pessoa.Id);
                var supervisor = _funcionarioAplicacao.BuscarPorId(model?.Supervisor?.Id ?? 0);

                itemFuncionario.ItemFuncionariosDetalhes = ListaFuncionarioItens;
                beneficioFuncionario.BeneficioFuncionarioDetalhes = ListaBeneficioFuncionarioDetalhes;
                ocorrenciaFuncionario.OcorrenciaFuncionarioDetalhes = ListaOcorrenciaFuncionarioDetalhe;

                model.ItemFuncionario = itemFuncionario;
                model.BeneficioFuncionario = beneficioFuncionario;
                model.OcorrenciaFuncionario = ocorrenciaFuncionario;

                model.Pessoa.Contatos = contatos;

                var funcionario = model.ToEntity(pessoa, supervisor);

                if (funcionario.OcorrenciaFuncionario != null)
                    funcionario.OcorrenciaFuncionario.UsuarioResponsavel = _usuarioAplicacao.BuscarPorId(UsuarioLogado.UsuarioId);

                //TODO: Remover o codigo abaixo, apos identificar o porque esta sendo criado um rg em branco.
                var rg = funcionario.Pessoa.Documentos.FirstOrDefault(x => x.Documento.Tipo == TipoDocumento.Rg);
                funcionario.Pessoa.Documentos.Remove(rg);

                var pessoaCpf = _pessoaAplicacao.BuscarPorCpf(funcionario.Pessoa.DocumentoCpf);

                if (pessoaCpf != null && (funcionario.Pessoa.Id == 0) || (pessoaCpf != null && funcionario.Pessoa.Id != pessoaCpf.Id))
                {
                    CriarDadosModalAviso("CPF já cadastrado para outro funcionário");
                    return View("Index", model);
                }

                _funcionarioAplicacao.Salvar(funcionario);

                ModelState.Clear();

                Session["contatos"] = new List<ContatoViewModel>();

                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
                return View("Index", model);
            }
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(id);
            var funcionarioVM = new FuncionarioViewModel(funcionario);

            if (funcionarioVM.BeneficioFuncionario != null && funcionarioVM.BeneficioFuncionario.BeneficioFuncionarioDetalhes != null)
            {
                foreach (var item in funcionarioVM.BeneficioFuncionario.BeneficioFuncionarioDetalhes)
                {
                    item.Valor = _planoCarreiraAplicacao.BuscarValorPeloPeriodo(item.TipoBeneficio, id);
                }
            }

            Session["contatos"] = funcionarioVM.Pessoa.Contatos;
            Session["ItensSelecionados"] = new List<ItemFuncionarioDetalheViewModel>();
            ListaFuncionarioItens = funcionarioVM.ItemFuncionario?.ItemFuncionariosDetalhes;
            ListaBeneficioFuncionarioDetalhes = funcionarioVM.BeneficioFuncionario?.BeneficioFuncionarioDetalhes;
            ListaOcorrenciaFuncionarioDetalhe = funcionarioVM.OcorrenciaFuncionario?.OcorrenciaFuncionarioDetalhes;
            funcionarioVM.ControlePontoDiaFaltas = Mapper.Map<List<ControlePontoDiaViewModel>>(_controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == id)?.ControlePontoDias?.Where(x => x.Falta)?.ToList()) ?? new List<ControlePontoDiaViewModel>();

            return View("Index", funcionarioVM);
        }

        public ActionResult BuscarFuncionarios(string cpf, string nome, int pagina = 1)
        {
            var funcionarios = new List<Funcionario>();
            PaginacaoGenericaViewModel paginacao;

            if (string.IsNullOrEmpty(cpf) && string.IsNullOrEmpty(nome))
            {
                var quantidadeFuncionarios = _funcionarioAplicacao.Contar();
                paginacao = new PaginacaoGenericaViewModel(50, pagina, quantidadeFuncionarios);
                funcionarios = _funcionarioAplicacao.BuscarPorIntervaloOrdernadoPorAlias(paginacao.RegistroInicial, paginacao.RegistrosPorPagina, "Pessoa.Nome").ToList();
            }
            else
            {
                var predicate = PredicateBuilder.True<Funcionario>();

                if (!string.IsNullOrEmpty(cpf))
                    predicate = predicate.And(x => x.Pessoa.Documentos.Any(c => c.Documento.Numero == cpf));

                if (!string.IsNullOrEmpty(nome))
                    predicate = predicate.And(x => x.Pessoa.Nome.Contains(nome));

                funcionarios = _funcionarioAplicacao.BuscarPor(predicate).ToList();
                paginacao = new PaginacaoGenericaViewModel(50, pagina, funcionarios.Count);

                funcionarios = funcionarios.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }

            ViewBag.Paginacao = paginacao;
            var funcionariosVM = funcionarios.Select(x => new FuncionarioViewModel(x)).ToList();

            return PartialView("_GridFuncionarios", funcionariosVM);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarPeloNome(string nome)
        {
            var lista = _funcionarioAplicacao.BuscarPor(x => x.Pessoa.Nome.Contains(nome.ToLower()));

            return Json(lista.Select(c => new
            {
                c.Id,
                Nome = c.Pessoa.Nome
            }));
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarSupervisoresPeloNome(string nome)
        {
            var funcionariosComSupervisor = _funcionarioAplicacao.BuscarPor(x => x.Supervisor != null);
            var supervisores = funcionariosComSupervisor.Select(x => x.Supervisor).Distinct().ToList();
            supervisores = supervisores.Where(x => x.Pessoa.Nome.ToLower().Contains(nome.ToLower())).ToList();

            return Json(supervisores.Select(c => new
            {
                c.Id,
                Nome = c.Pessoa.Nome
            }));
        }

        public PartialViewResult EditarObservacaoDia(int funcionarioId, int diaId, string observacao)
        {
            var controlePonto = _controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);
            var controlePontoDia = controlePonto.ControlePontoDias.FirstOrDefault(x => x.Id == diaId);
            controlePontoDia.Observacao = observacao;

            _controlePontoAplicacao.Salvar(controlePonto);
            var faltas = controlePonto.ControlePontoDias.Where(x => x.Falta);
            var faltasVM = Mapper.Map<List<ControlePontoDiaViewModel>>(faltas);

            return PartialView("_GridFaltas", faltasVM);
        }

        public JsonResult BuscarDataAdmissao(int? funcionarioId)
        {
            if (!funcionarioId.HasValue)
                return Json(string.Empty);

            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId.Value);

            return Json(funcionario.DataAdmissao?.ToShortDateString() ?? string.Empty);
        }

        public void AtualizarTipoEscala(int funcionarioId, TipoEscalaFuncionario tipoEscala)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            funcionario.TipoEscala = tipoEscala;
            _funcionarioAplicacao.Salvar(funcionario);
        }

        #region IntervaloDozeTrintaSeis
        public ActionResult AtualizarGridIntervaloDozeTrintaSeis(int funcionarioId)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloDozeTrintaSeis", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult AdicionarIntervaloDozeTrintaSeis(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            funcionario.ListaIntervaloDozeTrintaSeis = funcionario.ListaIntervaloDozeTrintaSeis ?? new List<FuncionarioIntervaloDozeTrintaSeis>();
            funcionario.ListaIntervaloDozeTrintaSeis.Add(new FuncionarioIntervaloDozeTrintaSeis
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal
            });
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloDozeTrintaSeis", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverIntervaloDozeTrintaSeis(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloDozeTrintaSeis.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloDozeTrintaSeis.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloDozeTrintaSeis", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarIntervaloDozeTrintaSeis(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloDozeTrintaSeis.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloDozeTrintaSeis.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloDozeTrintaSeisViewModel>>(funcionario.ListaIntervaloDozeTrintaSeis);
            var itemVM = Mapper.Map<FuncionarioIntervaloDozeTrintaSeisViewModel>(item);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloDozeTrintaSeis", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(itemVM)
            });
        }
        #endregion

        #region IntervaloCompensacao
        public ActionResult AtualizarGridIntervaloCompensacao(int funcionarioId)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloCompensacao", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult AdicionarIntervaloCompensacao(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            funcionario.ListaIntervaloCompensacao = funcionario.ListaIntervaloCompensacao ?? new List<FuncionarioIntervaloCompensacao>();
            funcionario.ListaIntervaloCompensacao.Add(new FuncionarioIntervaloCompensacao
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal
            });
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloCompensacao", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverIntervaloCompensacao(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloCompensacao.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloCompensacao.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloCompensacao", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarIntervaloCompensacao(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloCompensacao.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloCompensacao.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloCompensacaoViewModel>>(funcionario.ListaIntervaloCompensacao);
            var itemVM = Mapper.Map<FuncionarioIntervaloCompensacaoViewModel>(item);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloCompensacao", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(itemVM)
            });
        }
        #endregion

        #region IntervaloNoturno
        public ActionResult AtualizarGridIntervaloNoturno(int funcionarioId)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloNoturno", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult AdicionarIntervaloNoturno(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            funcionario.ListaIntervaloNoturno = funcionario.ListaIntervaloNoturno ?? new List<FuncionarioIntervaloNoturno>();
            funcionario.ListaIntervaloNoturno.Add(new FuncionarioIntervaloNoturno
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal
            });
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloNoturno", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverIntervaloNoturno(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloNoturno.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloNoturno.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloNoturno", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarIntervaloNoturno(int funcionarioId, DateTime dataInicial, DateTime dataFinal)
        {
            var funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            var item = funcionario.ListaIntervaloNoturno.FirstOrDefault(x => x.DataInicial.Date == dataInicial.Date && x.DataFinal.Date == dataFinal.Date);
            funcionario.ListaIntervaloNoturno.Remove(item);
            _funcionarioAplicacao.Salvar(funcionario);

            var listaIntervaloVM = Mapper.Map<List<FuncionarioIntervaloNoturnoViewModel>>(funcionario.ListaIntervaloNoturno);
            var itemVM = Mapper.Map<FuncionarioIntervaloNoturnoViewModel>(item);

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridIntervaloNoturno", listaIntervaloVM);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(itemVM)
            });
        }
        #endregion
    }
}