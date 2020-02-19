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
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ConvenioController : GenericController<Convenio>
    {
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IConvenioUnidadeAplicacao convenioUnidadeAplicacao;
        private readonly ITipoSeloAplicacao _tipoSeloAplicacao;
        private readonly IConvenioAplicacao _convenioAplicacao;
        private readonly IClienteAplicacao clienteAplicacao;
        private readonly IPrecoParametroSeloAplicacao _precoParametroSeloAplicacao;

        public ConvenioController(
            IUnidadeAplicacao unidadeAplicacao,
            IConvenioUnidadeAplicacao convenioUnidadeAplicacao,
            ITipoSeloAplicacao tipoSeloAplicacao,
            IConvenioAplicacao convenioAplicacao,
            IClienteAplicacao clienteAplicacao,
            IPrecoParametroSeloAplicacao precoParametroSeloAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            this.convenioUnidadeAplicacao = convenioUnidadeAplicacao;
            _tipoSeloAplicacao = tipoSeloAplicacao;
            _convenioAplicacao = convenioAplicacao;
            this.clienteAplicacao = clienteAplicacao;
            _precoParametroSeloAplicacao = precoParametroSeloAplicacao;
        }

        public List<Convenio> ListaConvenios => _convenioAplicacao?.Buscar()?.Where(x => x.ConvenioUnidades != null).ToList() ?? new List<Convenio>();
        public List<SelectListItem> ListaUnidades => _unidadeAplicacao?.Buscar()?.ToList().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() }).ToList();
        public List<SelectListItem> ListaTipoSelos => _tipoSeloAplicacao?.Buscar()?.ToList().Select(x => new SelectListItem { Text = x.Nome, Value = x.Id.ToString() }).ToList();
        
        public override ActionResult Index()
        {
            Session["ConveniosUnidades"] = null;
            return View();
        }

        public JsonResult VerificarTipoSelo(int idTipoSelo)
        {
            var tipoSeloDM = _tipoSeloAplicacao.BuscarPorId(idTipoSelo);
            var precoParametroSelo = _precoParametroSeloAplicacao.PrimeiroPor(x => x.TipoPreco != null && x.TipoPreco.Id == tipoSeloDM.Id);
            return Json(new {
                ParametroSelo = tipoSeloDM.ParametroSelo.ToString(),
                TemPrecoParametroSelo = precoParametroSelo != null ? true : false,
                tipoSeloDM.Valor
            });
        }

        public ActionResult ObterConvenioUnidades()
        {
            var conveniosUnidadesVM = (List<ConvenioUnidadeViewModel>)Session["ConveniosUnidades"];
            return PartialView("_GridConvenioUnidade", conveniosUnidadesVM);
        }

        public ActionResult AdicionarConvenioUnidade(int idUnidade, int idTipoSelo, string valor)
        {
            var conveniosUnidadesVM = (List<ConvenioUnidadeViewModel>)Session["ConveniosUnidades"];

            var unidadeVM = AutoMapper.Mapper.Map<Unidade, UnidadeViewModel>(_unidadeAplicacao.BuscarPorId(idUnidade));
            var tipoSeloVM = AutoMapper.Mapper.Map<TipoSelo, TipoSeloViewModel>(_tipoSeloAplicacao.BuscarPorId(idTipoSelo));

            if (conveniosUnidadesVM == null)
                conveniosUnidadesVM = new List<ConvenioUnidadeViewModel>();

            //verificação adicionada a pedido da GTE-1778 - inicio
            if(conveniosUnidadesVM.Where(x => x.Unidade.Id == unidadeVM.Id && x.TipoSelo.Id == tipoSeloVM.Id).Any())
            {
                return Json(new
                {
                    Sucesso = true,
                    Unidade = unidadeVM.Nome,
                    TipoSelo = tipoSeloVM.Nome,
                    Mensagem = "Item já foi adicionado"
                }, JsonRequestBehavior.AllowGet);
            }
            //verificação adicionada a pedido da GTE-1778 - fim

            var convenioUnidadeDM = new ConvenioUnidadeViewModel()
            {
                IdTeste = conveniosUnidadesVM.Count + 1,
                Unidade = unidadeVM,
                TipoSelo = tipoSeloVM,
                Valor = Convert.ToDecimal(valor)
            };

            conveniosUnidadesVM.Add(convenioUnidadeDM);
            Session["ConveniosUnidades"] = conveniosUnidadesVM;

            //List<ConvenioUnidadeViewModel> conveniosUnidadesVM = AutoMapper.Mapper.Map<List<ConvenioUnidade>, List<ConvenioUnidadeViewModel>>(conveniosUnidadesDM);

            return PartialView("_GridConvenioUnidade", conveniosUnidadesVM);
        }

        public ActionResult RemoverConvenioUnidade(int idConvenioUnidade)
        {
            var conveniosUnidadesVM = (List<ConvenioUnidadeViewModel>)Session["ConveniosUnidades"];
            var convenioUnidadeVM = conveniosUnidadesVM.Find(x => x.IdTeste == idConvenioUnidade);
            conveniosUnidadesVM.Remove(convenioUnidadeVM);
            Session["ConveniosUnidades"] = conveniosUnidadesVM;
            return PartialView("_GridConvenioUnidade", conveniosUnidadesVM);
        }

        public ActionResult SalvarDados(ConvenioViewModel convenio)
        {
            var conveniosUnidadesVM = (List<ConvenioUnidadeViewModel>)Session["ConveniosUnidades"] ?? new List<ConvenioUnidadeViewModel>();
            var convenioClientesVM = (List<ClienteViewModel>)Session["ConvenioClientes"] ?? new List<ClienteViewModel>();
            try
            {
                if (conveniosUnidadesVM == null || !conveniosUnidadesVM.Any())
                {
                    throw new BusinessRuleException("Adicione ao menos um item a Lista de Unidades x Tipos de Selo!");
                }

                if (convenioClientesVM == null || !convenioClientesVM.Any())
                {
                    throw new BusinessRuleException("Adicione ao menos um item a Lista de Clientes x Convênio!");
                }


                var convenioUnidadesListVM = new List<ConvenioUnidadesViewModel>();
                var convenioUnidadesListDM = new List<ConvenioUnidades>();
                
                foreach (var item in conveniosUnidadesVM)
                {
                    var convenioUnidade = new ConvenioUnidade
                    {
                        DataInsercao = DateTime.Now,
                        Id = item.Id,
                        TipoSelo = new TipoSelo { Id = item.TipoSelo.Id },
                        Unidade = new Unidade { Id = item.Unidade.Id },
                        Valor = item.Valor
                    };

                    var convenioUnidades = new ConvenioUnidades();
                    convenioUnidades.ConvenioUnidade = convenioUnidade;
                    convenioUnidadesListDM.Add(convenioUnidades);
                }

                var convenioDM = AutoMapper.Mapper.Map<ConvenioViewModel, Convenio>(convenio);
                convenioDM.ConvenioUnidades = convenioUnidadesListDM;

                convenioDM.Clientes = convenioClientesVM.Select(cli => cli.ToEntity()).ToList();

                _convenioAplicacao.Salvar(convenioDM);
                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
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
            //SetarViewBag();

            convenio.ConvenioCliente = convenioClientesVM;
            convenio.ConvenioUnidade = conveniosUnidadesVM;

            return View("Index", convenio);
        }


        public override ActionResult Edit(int id)
        {
            //SetarViewBag();
            var convenioDM = _convenioAplicacao.BuscarPorId(id);
            var convenioVM = AutoMapper.Mapper.Map<Convenio, ConvenioViewModel>(convenioDM);

            var conveniosUnidadesViewModel = new List<ConvenioUnidadeViewModel>();

            foreach (var convenioUnidade in convenioDM.ConvenioUnidades)
            {
                var convenioUnidadeViewModel = new ConvenioUnidadeViewModel();

                convenioUnidadeViewModel = AutoMapper.Mapper.Map<ConvenioUnidade, ConvenioUnidadeViewModel>(convenioUnidade.ConvenioUnidade);
                conveniosUnidadesViewModel.Add(convenioUnidadeViewModel);
            }
            convenioVM.ConvenioUnidade = conveniosUnidadesViewModel;

            int i = 0;

            foreach (var convenioUnidade in convenioVM.ConvenioUnidade)
            {
                convenioUnidade.IdTeste = i + 1;
            }

            Session["ConveniosUnidades"] = convenioVM.ConvenioUnidade;
            
            convenioVM.ConvenioCliente = convenioDM.Clientes.Select(cli => new ClienteViewModel(cli)).ToList();
            Session["ConvenioClientes"] = convenioVM.ConvenioCliente;

            return View("Index", convenioVM);
        }

        public ActionResult BuscarConvenios()
        {
            return PartialView("_GridConvenio", ListaConvenios);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarCliente(string descricao, int? unidadeId)
        {
            var lista = clienteAplicacao.BuscarPor(c => 
                (c.Pessoa.Nome.Contains(descricao) || c.NomeFantasia.Contains(descricao)) 
                && (unidadeId == null || c.Unidades.Any(u => u.Unidade.Id == unidadeId)));

            return Json(lista.Select(c => new
            {
                c.Id,
                Descricao = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
            }));
        }

        public ActionResult ObterClientes()
        {
            var convenioClientesVM = (List<ClienteViewModel>)Session["ConvenioClientes"] ?? new List<ClienteViewModel>();
            return PartialView("_GridConvenioClientes", convenioClientesVM);
        }

        public ActionResult AdicionarCliente(int idCliente)
        {
            var cliente = clienteAplicacao.BuscarPorId(idCliente);

            var convenioClientesVM = (List<ClienteViewModel>)Session["ConvenioClientes"] ?? new List<ClienteViewModel>();
            convenioClientesVM.Add(new ClienteViewModel(cliente));

            Session["ConvenioClientes"] = convenioClientesVM;

            return PartialView("_GridConvenioClientes", convenioClientesVM);
        }

        public ActionResult RemoverCliente(int idCliente)
        {
            var convenioClientesVM = (List<ClienteViewModel>)Session["ConvenioClientes"] ?? new List<ClienteViewModel>();
            var clienteVM = convenioClientesVM.FirstOrDefault(c => c.Id == idCliente);
            convenioClientesVM.Remove(clienteVM);

            Session["ConvenioClientes"] = convenioClientesVM;

            return PartialView("_GridConvenioClientes", convenioClientesVM);
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            try
            {
                _convenioAplicacao.ExcluirPorId(id);

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
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index");
            }


            return View("Index");
        }
    }
}