using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ParametrizacaoLocacaoController : GenericController<ParametrizacaoLocacao>
    {
        public List<ParametrizacaoLocacao> ListaParametrizacaoLocacao => _parametrizacaoLocacaoAplicacao?.Buscar()?.ToList() ?? new List<ParametrizacaoLocacao>();

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoLocacaoAplicacao _tipoLocacaoAplicacao;
        private readonly IParametrizacaoLocacaoAplicacao _parametrizacaoLocacaoAplicacao;



        public ParametrizacaoLocacaoController(IParametrizacaoLocacaoAplicacao ParametrizacaoLocacaoAplicacao,
                                               IUnidadeAplicacao unidadeAplicacao,
                                               ITipoLocacaoAplicacao tipoLocacaoAplicacao)
        {
            Aplicacao = ParametrizacaoLocacaoAplicacao;
            _parametrizacaoLocacaoAplicacao = ParametrizacaoLocacaoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _tipoLocacaoAplicacao = tipoLocacaoAplicacao;
        }

        public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.Buscar().ToList());
        public List<TipoLocacaoViewModel> ListaTipoLocacao => AutoMapper.Mapper.Map<List<TipoLocacao>, List<TipoLocacaoViewModel>>(_tipoLocacaoAplicacao.Buscar().ToList());

        [HttpPost]
        public ActionResult AtualizaUnidades(int tipolocacaoid)
        {

            var listaTipoLocacao = Aplicacao.BuscarPor(x => x.TipoLocacao.Id == Convert.ToInt32(tipolocacaoid));

            var unidadesExistentes = listaTipoLocacao.Select(x => x.Unidade.Id.ToString()).ToArray();

            var lista = ListaUnidade.Where(x => !unidadesExistentes.Contains(x.Id.ToString())).Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Nome }).ToList();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ParametrizacaoLocacaoViewModel entity)
        {
            try
            {
                _parametrizacaoLocacaoAplicacao.SalvarDados(entity);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (Exception br)
            {
                ModelState.Clear();
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", entity);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var objParametrizacao = new ParametrizacaoLocacaoViewModel(Aplicacao.BuscarPorId(id));


            return View("Index", objParametrizacao);
        }
    }
}