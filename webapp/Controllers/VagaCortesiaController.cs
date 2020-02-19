using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    public class VagaCortesiaController : GenericController<VagaCortesia>
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IVagaCortesiaVigenciaAplicacao _VagaCortesiaVigenciaAplicacao;
        private readonly IVagaCortesiaAplicacao _VagaCortesiaAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;

        public VagaCortesiaController(IVagaCortesiaAplicacao VagaCortesiaAplicacao,
                                         IUnidadeAplicacao unidadeAplicacao,
                                         IVagaCortesiaVigenciaAplicacao VagaCortesiaVigenciaAplicacao,
                                         IClienteAplicacao clienteAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            Aplicacao = VagaCortesiaAplicacao;
            _VagaCortesiaVigenciaAplicacao = VagaCortesiaVigenciaAplicacao;
            _VagaCortesiaAplicacao = VagaCortesiaAplicacao;
            _clienteAplicacao = clienteAplicacao;
        }

        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadas();
        public List<VagaCortesiaViewModel> ListaVagaCortesia => Aplicacao?.Buscar()?.Select(x => new VagaCortesiaViewModel(x)).ToList() ?? new List<VagaCortesiaViewModel>();

        // GET: VagaCortesia
        public override ActionResult Index()
        {
            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public new JsonResult SalvarDados(VagaCortesiaViewModel model)
        {
            try
            {

                var items = !string.IsNullOrEmpty(Session["Itens"]?.ToString()) ? (List<VagaCortesiaVigenciaViewModel>)Session["Itens"] : new List<VagaCortesiaVigenciaViewModel>(); ;

                if(items == null || items.Count <= 0)
                {
                    return Json(new Resultado<VagaCortesiaViewModel>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Warning.ToDescription(),
                        Titulo = "Salvar Alterações",
                        Mensagem = "Nenhum item foi adicionado"
                    });
                }

                if (model.VagaCortesiaVigencia != null && model.VagaCortesiaVigencia.Count > 0)
                    model.VagaCortesiaVigencia.Clear();
                else
                    model.VagaCortesiaVigencia = new List<VagaCortesiaVigenciaViewModel>();

                foreach (var item in items)
                {
                    model.VagaCortesiaVigencia.Add(item);
                }

                _VagaCortesiaAplicacao.Salvar(model);

                ModelState.Clear();

                return Json(new Resultado<VagaCortesiaViewModel>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = "Registro Salvo com Sucesso"
                });
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<VagaCortesiaViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = br.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new Resultado<VagaCortesiaViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = ex.Message
                });
            }
        }

        public override ActionResult Edit(int id)
        {
            var cliente = new VagaCortesiaViewModel(Aplicacao.BuscarPorId(id));

            Session["Itens"] = cliente.VagaCortesiaVigencia;

            return View("Index", cliente);
        }

        [CheckSessionOut]
        public ActionResult Atualizaritems(List<VagaCortesiaVigenciaViewModel> items)
        {
            Session["Itens"] = items;
            return PartialView("~/Views/VagaCortesia/_GridVigencia.cshtml", items);
        }

        public JsonResult BuscarDadosDosGrids(int id)
        {
            var cliente = new VagaCortesiaViewModel(Aplicacao.BuscarPorId(id));

            cliente.VagaCortesiaVigencia.ToList().ForEach(x =>
            {
                var unidade = new Unidade { Id = x.Unidade.Id, Nome = x.Unidade.Nome };
                x.Unidade = unidade;
            });

            Session["Itens"] = cliente.VagaCortesiaVigencia.ToList();

            return Json(
                new
                {
                    items = cliente?.VagaCortesiaVigencia,
                }
            );
        }

        public JsonResult VerificarSeJaTemCadastro(int clienteId)
        {
            var vagaCortesia = _VagaCortesiaAplicacao.PrimeiroPor(x => x.Cliente.Id == clienteId);

            return Json(new
            {
                JaExiste = vagaCortesia != null,
                VagaCortesiaId = vagaCortesia?.Id
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
