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
    public class OficinaController : GenericController<Oficina>
    {
        private readonly IOficinaAplicacao _oficinaAplicacao;

        public List<ChaveValorViewModel> ListaTipoPessoa => _oficinaAplicacao.BuscarValoresDoEnum<TipoPessoa>().ToList();
        public List<ChaveValorViewModel> ListaTipoEndereco => _oficinaAplicacao.BuscarValoresDoEnum<TipoEndereco>().ToList();

        public OficinaController(IOficinaAplicacao oficinaAplicacao)
        {
            Aplicacao = oficinaAplicacao;
            _oficinaAplicacao = oficinaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            var oficina = _oficinaAplicacao.BuscarPorId(id);
            var oficinaViewModel = new OficinaViewModel(oficina);

            return View("Index", oficinaViewModel);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(OficinaViewModel oficinaViewModel, EnderecoViewModel endereco)
        {
            try
            {
                oficinaViewModel.Pessoa.Endereco = endereco;
                var oficina = oficinaViewModel.ToEntity();
                _oficinaAplicacao.Salvar(oficina);

                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
            }
            return View("Index", oficinaViewModel);
        }

        public ActionResult BuscarOficina()
        {
            var oficinas = _oficinaAplicacao.Buscar();
            var oficinasViewModel = oficinas.Select(x => new OficinaViewModel(x)).ToList();

            return PartialView("_Grid", oficinasViewModel);
        }
    }
}