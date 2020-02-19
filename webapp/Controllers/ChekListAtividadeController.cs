using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Portal.Controllers
{
    public class CheckListAtividadeController : GenericController<CheckListAtividade>
    {
        public List<FuncionarioViewModel> ListaFuncionario => _funcionarioAplicacao.Buscar().Select(x => new FuncionarioViewModel(x)).ToList();

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public List<CheckListAtividade> ListaTiposAtividade => Aplicacao?.Buscar()?.ToList() ?? new List<CheckListAtividade>();

        private readonly ITipoAtividadeAplicacao _tipoAtividadeAplicacao;

        private readonly IEnderecoAplicacao _enderecoAplicacao;



        private readonly ITipoAtividadeAplicacao _marcaAplicacao;

        public readonly ICheckListAtividadeTipoAtividadeAplicacao _checkListAtividadeTipoAtividadeAplicacao;

        public ICheckListAtividadeAplicacao _checkListAtividadeAplicacao;

        public CheckListAtividadeController(ICheckListAtividadeAplicacao CheckListAtividadeAplicacao, 
                                            ITipoAtividadeAplicacao tipoAtividadeAplicacao,
                                            ITipoAtividadeAplicacao marcaAplicacao, 
                                            ICheckListAtividadeTipoAtividadeAplicacao checkListAtividadeTipoAtividadeAplicacao,
                                            IUnidadeAplicacao unidadeAplicacao,
                                            IFuncionarioAplicacao funcionarioAplicacao,
                                            IEnderecoAplicacao enderecoAplicacao)
        {
            Aplicacao = CheckListAtividadeAplicacao;
            _tipoAtividadeAplicacao = tipoAtividadeAplicacao;
            _marcaAplicacao = marcaAplicacao;
            _checkListAtividadeTipoAtividadeAplicacao = checkListAtividadeTipoAtividadeAplicacao;
            _checkListAtividadeAplicacao = CheckListAtividadeAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _enderecoAplicacao = enderecoAplicacao;
        }


        // GET: RetiradaCofre
        public override ActionResult Index()
        {
            ViewBag.TipoAtividadeSelectList = new SelectList(
                _tipoAtividadeAplicacao.Buscar() ?? new List<TipoAtividade>(),
                "Id",
                "Descricao");

            Session["TiposAtividade"] = null;

            return View();
        }

        public JsonResult SuggestionPerson(string param, bool exact)
        {
            return Json(_funcionarioAplicacao.BuscarPor(x => !exact && x.Pessoa.Nome.Contains(param) || x.Pessoa.Nome == param).Select(x => new FuncionarioViewModel()));
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(CheckListAtividadeViewModel model)
        {
            try
            {

                var entity = Aplicacao.BuscarPorId(model.Id) ?? new CheckListAtividade();

                //ajustando propriedades
                entity.Descricao = model.Descricao;
                entity.DataInsercao = model.DataInsercao;
                entity.Ativo = model.Ativo;
                entity.Id = model.Id;
                entity.Responsavel = _funcionarioAplicacao.BuscarPorId(model.Responsavel.Id);
                entity.Usuario = model.Usuario;

                //entity.Usuario = ??

                //ajustando lista de Tipo Atividades
                if (Session["TiposAtividade"] != null)
                {
                    var tipoAtividadeViewModels = (List<TipoAtividadeViewModel>)Session["TiposAtividade"];

                    var CheckListAtividadeTipoAtividades = new List<CheckListAtividadeTipoAtividade>();
                    foreach (var TipoAtividade in tipoAtividadeViewModels)
                    {
                        var CheckListAtividadeTipoAtividade = new CheckListAtividadeTipoAtividade();
                        CheckListAtividadeTipoAtividade.CheckListAtividade = entity.Id;
                        CheckListAtividadeTipoAtividade.TipoAtividade = new TipoAtividade();
                        CheckListAtividadeTipoAtividade.TipoAtividade = TipoAtividade.ToEntity();
                        CheckListAtividadeTipoAtividades.Add(CheckListAtividadeTipoAtividade);
                    }

                    entity.TiposAtividade = CheckListAtividadeTipoAtividades; 
                }

                Aplicacao.Salvar(entity);

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
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var cliente = new CheckListAtividadeViewModel(Aplicacao.BuscarPorId(id));

            Session["TiposAtividade"] = Helpers.TipoAtividade.RetornaTipoAtividades(cliente.TiposAtividade.ToList());

            return View("Index", cliente);
        }

        public JsonResult BuscarDadosDosGrids(int id)
        {
            var checkListAtividade = new CheckListAtividadeViewModel(_checkListAtividadeAplicacao.BuscarPorId(id));

            var atividades = checkListAtividade?.TiposAtividade.Select(x => x.TipoAtividade).ToList();
            Session["TiposAtividade"] = atividades;

            return Json(
                new
                {
                    Atividades = atividades
                }
            );
        }

        [HttpPost]
        public void AdicionaTipoAtividade(string index, string TipoAtividade)
        {
            if (!string.IsNullOrEmpty(TipoAtividade))
            {

                var TipoAtividades = new List<TipoAtividadeViewModel>();
                if (Session["TiposAtividade"] != null)
                {
                    TipoAtividades = (List<TipoAtividadeViewModel>)Session["TiposAtividade"];
                }

                if (int.Parse(index) < 0)
                {
                    TipoAtividades.AddRange(JsonConvert.DeserializeObject<List<TipoAtividadeViewModel>>(TipoAtividade));
                }
                else
                {
                    TipoAtividades[int.Parse(index)] = JsonConvert.DeserializeObject<List<TipoAtividadeViewModel>>(TipoAtividade).First();
                }
                Session["TiposAtividade"] = TipoAtividades;
            }
        }

        [HttpPost]
        public void AtualizaTipoAtividades(string jsonTipoAtividades)
        {
            if (!string.IsNullOrEmpty(jsonTipoAtividades))
            {
                var TipoAtividades = JsonConvert.DeserializeObject<List<TipoAtividadeViewModel>>(jsonTipoAtividades);

                Session["TiposAtividade"] = TipoAtividades;
            }
        }

        [HttpPost]
        public void RemoveTipoAtividadeSessao(string index)
        {

            var TipoAtividades = new List<TipoAtividadeViewModel>();
            if (Session["TiposAtividade"] != null)
            {
                TipoAtividades = (List<TipoAtividadeViewModel>)Session["TiposAtividade"];
                TipoAtividades.RemoveAt(int.Parse(index));
                Session["TiposAtividade"] = TipoAtividades;
            }
        }

        [HttpPost]
        public JsonResult ListaAtividades(string CheckListAtividade)
        {
            var TipoAtividades = new List<TipoAtividadeViewModel>();

            if (!string.IsNullOrEmpty(CheckListAtividade))
            {
                var CheckListAtividades = new List<CheckListAtividadeViewModel>();
                CheckListAtividades.AddRange(JsonConvert.DeserializeObject<List<CheckListAtividadeViewModel>>(CheckListAtividade));

                var cliente = new CheckListAtividadeViewModel(Aplicacao.BuscarPorId(CheckListAtividades.First().Id));


                if (cliente != null && cliente.TiposAtividade != null)
                {
                    TipoAtividades = Helpers.TipoAtividade.RetornaTipoAtividades(cliente.TiposAtividade.ToList());
                } 
            }

            return Json(TipoAtividades);
        }

        public JsonResult ListaTipoAtividade()
        {
            var Marcas = _tipoAtividadeAplicacao.Buscar().Select(x => new TipoAtividadeViewModel(x)).ToList();
            return Json(Marcas);
        }

        public JsonResult CarregarMarcas()
        {
            var lista = new List<TipoAtividadeViewModel>
            {
                new TipoAtividadeViewModel()
                {
                    Descricao = @"Selecione um marca.",
                    Id = 0
                }
            };

            var busca = _tipoAtividadeAplicacao.Buscar().Select(x => new TipoAtividadeViewModel(x)).ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Id", "Descricao"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Descricao", 0));
        }
    }
}