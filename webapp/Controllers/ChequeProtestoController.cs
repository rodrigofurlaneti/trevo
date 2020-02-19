using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ChequeProtestoController : GenericController<Cheque>
    {
        public List<ChequeRecebidoViewModel> ListaChequeProtestos => Mapper.Map<List<ChequeRecebido>, List<ChequeRecebidoViewModel>>(Aplicacao.Buscar().ToList());

        public List<ClienteViewModel> ListaCliente
        {
            get { return (List<ClienteViewModel>)Session["ListaCliente"] ?? new List<ClienteViewModel>(); }
            set { Session["ListaCliente"] = value; }
        }

        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IChequeRecebidoAplicacao Aplicacao;

        public ChequeProtestoController(IChequeRecebidoAplicacao marcaAplicacao,
                                         IClienteAplicacao clienteAplicacao)
        {
            Aplicacao = marcaAplicacao;
            _clienteAplicacao = clienteAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            ListaCliente = _clienteAplicacao.Buscar().Select(x => new ClienteViewModel(x)).ToList();

            foreach (var cliente in ListaCliente)
            {
                if (cliente.Pessoa.Nome == null)
                {
                    cliente.Pessoa.Nome = cliente.NomeFantasia;
                }
            }

            ViewBag.ListaCliente = ListaCliente;

            return View("Index");
        }

        public ActionResult BuscarChequeProtestos(ChequeRecebidoViewModel filtro)
        {
            var model = new List<ChequeRecebidoViewModel>();

            var chequesProtestados = Aplicacao.BuscarPor(x => x.Cliente.Id == filtro.Cliente.Id);

            if (Convert.ToInt16(filtro.StatusCheque) != 0)
            {
                chequesProtestados = chequesProtestados.Where(x => x.StatusCheque == filtro.StatusCheque).ToList();
            }

            if (filtro.Valor != 0)
            {
                chequesProtestados = chequesProtestados.Where(x => x.Valor == Convert.ToDecimal(filtro.Valor)).ToList();
            }

            if (filtro.Numero != 0)
            {
                chequesProtestados = chequesProtestados.Where(x => x.Numero == filtro.Numero).ToList();
            }

            if (filtro.DataProtesto != null)
            {
                chequesProtestados = chequesProtestados.Where(x => x.DataProtesto == filtro.DataProtesto).ToList();
            }

            if (!string.IsNullOrEmpty(filtro.CartorioProtestado))
            {
                chequesProtestados = chequesProtestados.Where(x => x.CartorioProtestado.Contains(filtro.CartorioProtestado)).ToList();
            }

            model = Mapper.Map<List<ChequeRecebido>,List<ChequeRecebidoViewModel>>(chequesProtestados.ToList());

            foreach (var models in model)
            {
                if (models.Cliente.Pessoa.Nome == null)
                {
                    models.Cliente.Pessoa.Nome = models.Cliente.NomeFantasia;
                }
            }

            return PartialView("_GridChequeProtesto", model);

        }

        public ActionResult CarregaChequeProtestos()
        {
            var ChequeProtestos = new List<ChequeViewModel>();

            return PartialView("_GridChequeProtesto", ChequeProtestos);
        }

        public ActionResult Informacoes()
        {
            return PartialView("_ModalInformacoes");
        }

        public JsonResult AlterarStatusCheque(List<ChequeRecebidoViewModel> listmodel, string cartorioprotestado, DateTime? dataprotesto)
        {

            try
            {
                foreach (var model in listmodel)
                {
                    var entity = Aplicacao.BuscarPorId(model.Id);
                    entity.StatusCheque = StatusCheque.Protestado;
                    entity.CartorioProtestado = cartorioprotestado;

                    entity.DataProtesto = DateTime.Now.Date;

                    if (dataprotesto != null)
                    {
                        entity.DataProtesto = dataprotesto;
                    }

                    Aplicacao.Salvar(entity); 
                }

                ModelState.Clear();

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger,
                    Titulo = "Erro",
                    Mensagem = ex.Message,
                    status = new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message),
                });
            }

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Solitações efetuadas com sucesso"),
            });
        }

        [HttpPost]
        public JsonResult PesquisaEmitente(string Prefix)
        {

            var retorno = Aplicacao.BuscarPor(x => x.Emitente.Contains(Prefix));


            var customers = (from valores in retorno
                             select new
                             {
                                 label = valores.Emitente,
                                 val = valores.Emitente
                             }).ToList();


            return Json(customers, JsonRequestBehavior.AllowGet);

        }

    }
}
