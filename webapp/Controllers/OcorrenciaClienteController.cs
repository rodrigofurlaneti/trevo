using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Helpers;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    
    public class OcorrenciaClienteController : GenericController<OcorrenciaCliente>
    {       

        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IOcorrenciaAplicacao _ocorrenciaClienteAplicacao;

        public OcorrenciaClienteController(IOcorrenciaAplicacao ocorrenciaAplicacao, IFuncionarioAplicacao funcionarioAplicacao)
        {
            _ocorrenciaClienteAplicacao = ocorrenciaAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
        }

        public List<OcorrenciaClienteViewModel> ListaOcorrencia
        {
            get => (List<OcorrenciaClienteViewModel>)Session["ListaOcorrencia"] ?? new List<OcorrenciaClienteViewModel>();
            set => Session["ListaOcorrencia"] = value;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View(IndexPadrao());
        }

        private OcorrenciaClienteViewModel IndexPadrao()
        {
            Session["ListaOcorrencia"] = new List<OcorrenciaClienteViewModel>();
            OcorrenciaClienteViewModel ocorrencia = new OcorrenciaClienteViewModel();
            ocorrencia.GerarProtocolo();
            ocorrencia.GerarValoresPadrao();
            ocorrencia.Veiculo = new VeiculoViewModel();
            return ocorrencia;
        }

        [CheckSessionOut]
        [HttpPost, ValidateInput(false)]
        public ActionResult SalvarDadosOcorrencia(string jsonModel)
        {
            if (!string.IsNullOrEmpty(jsonModel))
            {
                var format = "dd/MM/yyyy"; // your datetime format
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                var model = JsonConvert.DeserializeObject<OcorrenciaClienteViewModel>(jsonModel, dateTimeConverter);

                try
                {
                    var ocorrenciaClienteEntity = model.ToEntity();
                    ocorrenciaClienteEntity.FuncionarioAtribuido = _funcionarioAplicacao.BuscarPorId(model.FuncionarioAtribuido.Pessoa.Id);

                    if (ocorrenciaClienteEntity.Veiculo != null && ocorrenciaClienteEntity.Veiculo.Id <= 0)
                    {
                        ocorrenciaClienteEntity.Veiculo = null;
                    }

                    if (ocorrenciaClienteEntity.Unidade != null && ocorrenciaClienteEntity.Unidade.Id <= 0)
                    {
                        ocorrenciaClienteEntity.Unidade = null;
                    }

                    var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                    _ocorrenciaClienteAplicacao.SalvarDadosOcorrenciaComNotificacao(ocorrenciaClienteEntity, usuarioLogadoCurrent.UsuarioId);
                    ModelState.Clear();

                    DadosModal = new DadosModal
                    {
                        Titulo = "Sucesso",
                        Mensagem = "Registro salvo com sucesso",
                        TipoModal = TipoModal.Success
                    };
                }
                catch (BusinessRuleException br)
                {
                    DadosModal = new DadosModal
                    {
                        Titulo = "Atenção",
                        Mensagem = br.Message,
                        TipoModal = TipoModal.Warning
                    };
                    return View("Index", model);
                }
                catch (Exception ex)
                {
                    DadosModal = new DadosModal
                    {
                        Titulo = "Atenção",
                        Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                        TipoModal = TipoModal.Danger
                    };
                    return View("Index", model);
                }
            }

            return Json(new Resultado<object>()
            {
                Titulo = "Sucesso",
                Mensagem = "Registro salvo com sucesso",
                TipoModal = TipoModal.Success.GetDescription()
            });

            //return View("Index", IndexPadrao());
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult AdicionarOcorrencia(string jsonOcorrencias)
        {
            if (!string.IsNullOrEmpty(jsonOcorrencias))
            {
                try
                {
                    var format = "dd/MM/yyyy"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

                    var ocorrencias = JsonConvert.DeserializeObject<List<OcorrenciaClienteViewModel>>(jsonOcorrencias, dateTimeConverter);
                    ListaOcorrencia = ocorrencias;

                    var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrencias", ListaOcorrencia);
                    return Json(new
                    {
                        Grid = grid,
                    });
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            return View();
        }

        
        [HttpGet]
        public JsonResult BuscarOcorrenciasPorClienteId()
        {

            var ocorrenciasViewModel = !string.IsNullOrEmpty(Session["ListaOcorrencia"]?.ToString()) ? (List<OcorrenciaClienteViewModel>)Session["ListaOcorrencia"] : new List<OcorrenciaClienteViewModel>();

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue,
                Data = new
                {
                    ocorrenciaLista = ocorrenciasViewModel
                }
            };
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult EditarOcorrencia(int ocorrenciaId)
        {
            var item = ListaOcorrencia.FirstOrDefault(x => x.Id == ocorrenciaId);
            var ListaOcorrenciaAUX = ListaOcorrencia;

            ListaOcorrenciaAUX.Remove(item);
            ListaOcorrencia = ListaOcorrenciaAUX;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrencias", ListaOcorrencia);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }
        public TJson RemoverLoopDoJson<TJson>(TJson obj)
        {
            var objJson = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            var objConvertido = JsonConvert.DeserializeObject<TJson>(objJson);
            return objConvertido;
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult BuscarOcorrenciasCliente(string protocolo, string nome, string status, int pagina = 1)
        {
            var documentoFormatado = string.Empty;
            var grid = string.Empty;
            var resultado = new Resultado<GridOcorrenciaClienteViewModel>();
            var ocorrenciasCliente = new List<OcorrenciaCliente>();
            var clienteViewModel = new List<GridOcorrenciaClienteViewModel>();
            PaginacaoGenericaViewModel paginacao = new PaginacaoGenericaViewModel();
            var take = 50;

            var quantidadeRegistros = 0;
            ocorrenciasCliente = _ocorrenciaClienteAplicacao.BuscarDadosGrid(protocolo, nome, status, out quantidadeRegistros, pagina, take)?.ToList();
            paginacao = new PaginacaoGenericaViewModel(take, pagina, quantidadeRegistros);

            clienteViewModel = ocorrenciasCliente?.Select(x => new GridOcorrenciaClienteViewModel
            {
                Id = x.Id,
                Nome = string.IsNullOrEmpty(x.Cliente.NomeFantasia) ? string.IsNullOrEmpty(x.Cliente.RazaoSocial) ? x.Cliente.Pessoa.Nome : x.Cliente.RazaoSocial : x.Cliente.NomeFantasia,
                Protocolo = x.NumeroProtocolo,
                Status = x.StatusOcorrencia
            })?
            .OrderBy(x => x.Nome)?
            .ToList();

            ViewBag.Paginacao = paginacao;

            grid = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrenciasCliente", clienteViewModel);
            return new JsonResult { Data = new { documentoFormatado, grid, resultado } };
        }

        public override ActionResult Edit(int id)
        {
            var ocorrenciaEntity = _ocorrenciaClienteAplicacao.BuscarPorId(id);
            var ocorrencia = new OcorrenciaClienteViewModel(ocorrenciaEntity);
            
            return View("Index", ocorrencia);
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

            return View("Index", IndexPadrao());
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            ViewBag.PrintFlag = false;
            _ocorrenciaClienteAplicacao.ExcluirPorId(id);

            ModelState.Clear();
            GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);

            return View("Index", IndexPadrao());
        }

        [CheckSessionOut]
        public ActionResult ExecutarModal(int id)
        {
            try
            {
                return PartialView("_ModalCampos", IndexPadrao());
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.GetDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}