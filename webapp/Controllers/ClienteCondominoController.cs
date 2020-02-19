using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace Portal.Controllers
{
    public class ClienteCondominoController : GenericController<ClienteCondomino>
    {
        private readonly IClienteCondominoAplicacao _clienteCondominoAplicacao;
        private readonly IUnidadeCondominoAplicacao _unidadeCondominoAplicacao;
        private readonly IMarcaAplicacao _marcaAplicacao;
        private readonly IModeloAplicacao _modeloAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IVeiculoAplicacao _veiculoAplicacao;

        public List<ClienteCondomino> ListaUnidadesCondomino => Aplicacao?.Buscar()?.ToList() ?? new List<ClienteCondomino>();
        public List<UnidadeCondominoViewModel> ListaUnidade => Mapper.Map<List<UnidadeCondomino>, List<UnidadeCondominoViewModel>>(_unidadeCondominoAplicacao.Buscar().ToList());
        public List<VeiculoViewModel> ListaVeiculos { get; set; }

        public List<VeiculoViewModel> ListaVeiculosAdicionados
        {
            get => (List<VeiculoViewModel>)Session["VeiculosAdicionados"] ?? new List<VeiculoViewModel>();
            set => Session["VeiculosAdicionados"] = value;
        }

        public ClienteCondominoController(IClienteCondominoAplicacao clienteCondominoAplicacao,
                                          IUnidadeCondominoAplicacao unidadeCondominoAplicacao,
                                          IMarcaAplicacao marcaAplicacao,
                                          IModeloAplicacao modeloAplicacao,
                                          IVeiculoAplicacao veiculoAplicacao,
                                          IClienteAplicacao clienteAplicacao)
        {
            Aplicacao = clienteCondominoAplicacao;
            _clienteCondominoAplicacao = clienteCondominoAplicacao;
            _unidadeCondominoAplicacao = unidadeCondominoAplicacao;
            _marcaAplicacao = marcaAplicacao;
            _modeloAplicacao = modeloAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _veiculoAplicacao = veiculoAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            ListaVeiculosAdicionados = null;
            return View();
        }

        public override ActionResult Edit(int id)
        {
            var clienteCondomino = Aplicacao.BuscarPorId(id);
            var clienteCondominoViewModel = Mapper.Map<ClienteCondominoViewModel>(clienteCondomino);
            ListaVeiculos = new List<VeiculoViewModel>();
            clienteCondominoViewModel.Veiculos = clienteCondominoViewModel.CondominoVeiculos.Select(x => x.Veiculo).ToList();
            ListaVeiculosAdicionados = clienteCondominoViewModel.Veiculos;
            return View("Index", clienteCondominoViewModel);
        }

        public override ActionResult Salvar(ClienteCondomino model)
        {
            var veiculos = ListaVeiculosAdicionados;
            model.CondominoVeiculos = model.CondominoVeiculos ?? new List<CondominoVeiculo>();

            foreach (var veiculo in veiculos)
            {
                if (!model.CondominoVeiculos.Any(x => x.Veiculo != null && x.Veiculo.Id == veiculo.Id))
                {
                    model.CondominoVeiculos.Add(new CondominoVeiculo
                    {
                        Condomino = model,
                        Veiculo = veiculo.ToEntity()
                    });
                }
            }

            return base.Salvar(model);
        }

        public JsonResult BuscarVeiculos(int idCliente)
        {
            ViewBag.PrintFlag = false;
            var cliente =
              _clienteAplicacao.BuscarPorId(idCliente);

            List<ClienteVeiculo> clienteVeiculos = new List<ClienteVeiculo>(cliente.Veiculos);
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
                    if (veiculo.Id > 0)
                    {
                        VeiculoViewModel item = new VeiculoViewModel();
                        item = Mapper.Map<Veiculo, VeiculoViewModel>(_veiculoAplicacao.BuscarPorId(veiculo.Id));
                        if (item != null)
                        {
                            ListaVeiculos.Add(item);
                        }
                    }
                }
            }

            ListaVeiculosAdicionados = ListaVeiculos;
            return PartialView("_GridVeiculos", ListaVeiculos);
        }

        public JsonResult BuscarClienteVeiculos()
        {
            return Json(ListaVeiculosAdicionados, JsonRequestBehavior.AllowGet);
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
    }
}