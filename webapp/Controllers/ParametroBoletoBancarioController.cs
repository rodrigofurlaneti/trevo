using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ParametroBoletoBancarioController : GenericController<ParametroBoletoBancario>
    {
        private readonly IParametroBoletoBancarioAplicacao _parametroBoletoBancarioAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        private List<ParametroBoletoBancarioViewModel> ListaParametroBoletoBancarioViewModels
        {
            get => (List<ParametroBoletoBancarioViewModel>)Session["ListaParametroBoletoBancarioViewModels"] ?? new List<ParametroBoletoBancarioViewModel>();
            set => Session["ListaParametroBoletoBancarioViewModels"] = value;
        }

        public List<ParametroBoletoBancarioDescritivoViewModel> ListaDescritivo
        {
            get => (List<ParametroBoletoBancarioDescritivoViewModel>)Session["ListaDescritivo"] ?? new List<ParametroBoletoBancarioDescritivoViewModel>();
            set => Session["ListaDescritivo"] = value;
        }
        public List<ChaveValorViewModel> ListaTipoServico => Aplicacao?.BuscarValoresDoEnum<TipoServico>().ToList();

        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListaUnidade().Select(x => new UnidadeViewModel(x)).ToList();

        public ParametroBoletoBancarioController(
            IParametroBoletoBancarioAplicacao parametroBoletoBancarioAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            IUsuarioAplicacao usuarioAplicacao)
        {
            Aplicacao = parametroBoletoBancarioAplicacao;
            _parametroBoletoBancarioAplicacao = parametroBoletoBancarioAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaDescritivo = new List<ParametroBoletoBancarioDescritivoViewModel>();

            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ParametroBoletoBancarioViewModel model)
        {
            try
            {
                if (ListaParametroBoletoBancarioViewModels.Any(x => x.TipoServico == model.TipoServico 
                                                                && (x.Unidade != null && x.Unidade.Id != model.Unidade.Id)) && model.Id == 0)
                    CriarDadosModalErro($"Já existe um registro para o tipo: {model.TipoServico.ToString()}");
                else
                {
                    model.ParametroBoletoBancarioDescritivos = ListaDescritivo;
                    if (model.Unidade.Id == 0)
                        model.Unidade = null;

                    var parametroBoletoBancario = Mapper.Map<ParametroBoletoBancario>(model);

                    _parametroBoletoBancarioAplicacao.Salvar(parametroBoletoBancario);

                    ModelState.Clear();

                    CriarDadosModalSucesso("Registro salvo com sucesso");
                }
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
                return View("Index", model);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var parametroBoletoBancario = _parametroBoletoBancarioAplicacao.BuscarPorId(id);
            var vm = Mapper.Map<ParametroBoletoBancarioViewModel>(parametroBoletoBancario);
            ListaDescritivo = vm.ParametroBoletoBancarioDescritivos;
            return View("Index", vm);
        }

        public ActionResult AdicionarDescritivo(string descritivo)
        {
            var usuarioVM = Mapper.Map<UsuarioViewModel>(_usuarioAplicacao.BuscarPorId(UsuarioLogado.UsuarioId));
            var descritivoBoleto = new ParametroBoletoBancarioDescritivoViewModel(descritivo, usuarioVM);

            var listaDescritivo = ListaDescritivo;

            listaDescritivo.Add(descritivoBoleto);
            ListaDescritivo = listaDescritivo;

            return PartialView("_GridDescritivos", ListaDescritivo);
        }

        public ActionResult RemoverDescritivo(string descritivo)
        {
            var descritivoBoleto = ListaDescritivo.FirstOrDefault(x => x.Descritivo == descritivo);
            var listaDescritivo = ListaDescritivo;

            listaDescritivo.Remove(descritivoBoleto);
            ListaDescritivo = listaDescritivo;

            return PartialView("_GridDescritivos", ListaDescritivo);
        }

        public ActionResult BuscarParametroBoletoBancario()
        {
            var parametros = _parametroBoletoBancarioAplicacao.Buscar();
            ListaParametroBoletoBancarioViewModels = Mapper.Map<List<ParametroBoletoBancarioViewModel>>(parametros);
            return PartialView("_Grid", ListaParametroBoletoBancarioViewModels);
        }
    }
}
