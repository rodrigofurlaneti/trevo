using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Portal.Controllers
{
    public class VeiculoController :  GenericController<Veiculo>
    {
        public readonly IClienteVeiculoAplicacao clienteVeiculoAplicacao;
        public readonly IVeiculoAplicacao veiculoAplicacao;
        public readonly IMarcaAplicacao marcaAplicacao;
        public readonly IModeloAplicacao modeloAplicacao;
  

        public List<MarcaViewModel> Marcas { get; set; }
        public List<ModeloViewModel> Modelos { get; set; }

        public VeiculoController(IVeiculoAplicacao veiculoAplicacao
                                , IClienteVeiculoAplicacao clienteVeiculoAplicacao
                                , IMarcaAplicacao marcaAplicacao
                                , IModeloAplicacao modeloAplicacao)
        {
            Aplicacao = veiculoAplicacao;
            this.veiculoAplicacao = veiculoAplicacao;
            this.marcaAplicacao = marcaAplicacao;
            this.modeloAplicacao = modeloAplicacao;
            this.clienteVeiculoAplicacao = clienteVeiculoAplicacao;
        }
        //GET: Veiculo
       [CheckSessionOut]
        public override ActionResult Index()
        {
            Marcas = new List<MarcaViewModel>();
            Modelos = new List<ModeloViewModel>();
            ViewBag.TipoVeiculoSelectList = new SelectList(
                System.Enum.GetValues(typeof(TipoVeiculo)).Cast<TipoVeiculo>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(), "Id", "Descricao");

            ModelState.Clear();
            Marcas = new List<MarcaViewModel>();
            Modelos = new List<ModeloViewModel>();
           
            Marcas = marcaAplicacao.Buscar().Select(x => new MarcaViewModel(x)).ToList();
            Modelos = modeloAplicacao.Buscar().Select(x => new ModeloViewModel(x)).ToList();
            //ModelState.Clear();
            return View("Index");
        }

        public JsonResult CarregarTipos()
        {
            var tipos = EnumHelper.GetSelectList(typeof(TipoVeiculo));
            return Json(new SelectList(tipos, "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaModelo(string marca)
        {
            if (!string.IsNullOrEmpty(marca))
            {
               
                var marcas = new List<MarcaViewModel>();
                marcas.AddRange(JsonConvert.DeserializeObject<List<MarcaViewModel>>(marca));

                Modelos = modeloAplicacao.BuscarPor(x => x.Marca.Id == marcas.First().Id).Select(x => new ModeloViewModel(x)).ToList();
            }
                
            return Json(Modelos);
        }

        [HttpPost]
        public JsonResult ListaMarca(int? jsonModeloId)
        {   
            if (jsonModeloId == null)
            {
                Marcas = marcaAplicacao.Buscar().Select(x => new MarcaViewModel(x)).ToList();
                return Json(Marcas);
            }

            MarcaViewModel response = new MarcaViewModel();
            if (jsonModeloId > 0)
            {
                var modelo = modeloAplicacao.BuscarPorId(jsonModeloId??0);
                response.Id = modelo.Marca.Id;
                response.Nome = modelo.Marca.Nome;
            }
            return Json(response);
        }

        [HttpPost]
        public void AdicionaVeiculo(string index,string veiculo)
        {
            if (!string.IsNullOrEmpty(veiculo))
            {

                var veiculos = new List<VeiculoViewModel>();
                if(Session["Veiculo"] != null)
                {
                    veiculos = (List<VeiculoViewModel>)Session["Veiculo"];
                }

                if(int.Parse(index) < 0)
                {
                    veiculos.AddRange(JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo));
                }
                else
                {
                    veiculos[int.Parse(index)] = JsonConvert.DeserializeObject<List<VeiculoViewModel>>(veiculo).First();
                }
                Session["Veiculo"] = veiculos;
            }
        }

        [HttpPost]
        public void AtualizaVeiculos(string jsonVeiculos)
       {
            if (!string.IsNullOrEmpty(jsonVeiculos))
            {
                var veiculos = JsonConvert.DeserializeObject<List<VeiculoViewModel>>(jsonVeiculos);
                Session["Veiculo"] = veiculos;
            }
        }

        [HttpPost]
        public void RemoveVeiculoSessao(string index)
        {            
            if (Session["Veiculo"] != null)
            {
                var veiculos = (List<VeiculoViewModel>)Session["Veiculo"];
                veiculos.RemoveAt(int.Parse(index));
                Session["Veiculo"] = veiculos;
            }
        }

        [HttpPost]
        public JsonResult ListaVeiculo(string cliente)
        {
            var veiculos = new List<VeiculoViewModel>();
            Session["Veiculo"] = null;
            if (!string.IsNullOrEmpty(cliente))
            {
                var clientes = new List<ClienteViewModel>();
                clientes.AddRange(JsonConvert.DeserializeObject<List<ClienteViewModel>>(cliente));

                var clienteVeiculos = 
                    clienteVeiculoAplicacao
                    .BuscarPor(veiculo => veiculo.Cliente == clientes.First().Id)
                    .Select(x => new ClienteVeiculoViewModel(x)).ToList(); ;
                veiculos = Helpers.Veiculo.RetornaVeiculos(clienteVeiculos);
            }

            Session["Veiculo"] = veiculos;
            return Json(Modelos);
        }

    }
}