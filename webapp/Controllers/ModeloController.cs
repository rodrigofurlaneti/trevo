using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;

namespace Portal.Controllers
{
    public class ModeloController : GenericController<Modelo>
    {
        private List<SelectListItem> _listMarcaes = new List<SelectListItem>();
        public List<SelectListItem> ListaMarcaes
        {
            get { return _listMarcaes; }
            set { _listMarcaes = value; }
        }
        public IEnumerable<Modelo> ListaModelos => Aplicacao?.Buscar()?.ToList() ?? new List<Modelo>();
        private readonly IMarcaAplicacao _marcaAplicacao;

        public ModeloController(IModeloAplicacao modeloAplicacao, IMarcaAplicacao marcaAplicacao)
        {
            Aplicacao = modeloAplicacao;
            _marcaAplicacao = marcaAplicacao;
        }

        [HttpGet]
        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View("Index");
        }

        public JsonResult CarregarMarcaes()
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

        [CheckSessionOut]
        [HttpPost]
        public ActionResult Gravar(ModeloViewModel model)
        {
            try
            {
                model.Marca =new MarcaViewModel(_marcaAplicacao.BuscarPorId(model.Marca.Id));
                Aplicacao.Salvar(model.ToEntity());

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
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
            }
            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult Editar(int id)
        {
            var model = new ModeloViewModel(Aplicacao.BuscarPorId(id));

            return View("Index", model);
        }

        [CheckSessionOut]
        public ActionResult Excluir(int id)
        {
            try
            {
                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                return View("Index");
            }

            ModelState.Clear();

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            try
            {
                Aplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                return View("Index");
            }

            ModelState.Clear();
            return View("Index");
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarModeloPeloNome(string nome)
        {
            JsonResult result = new JsonResult();

            if (!(string.IsNullOrEmpty(nome) || string.IsNullOrWhiteSpace(nome)))
            {
                var filtro = Aplicacao.BuscarPor(w => w.Descricao.Contains(nome));
                if (!ReferenceEquals(filtro, null))
                {
                    if (filtro.Count == 0)
                    {
                        var modelo = new Modelo();
                        modelo.Descricao = nome;
                        modelo.Id = -1;
                        filtro.Add(modelo);
                    }

                    result = Json(filtro.Select(c => new
                    {
                        c.Id,
                        c.Descricao
                    }));
                }
            }
            return result;
        }
    }
}