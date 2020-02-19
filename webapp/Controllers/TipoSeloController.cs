using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class TipoSeloController : GenericController<TipoSelo>
    {
        //ITipoSeloAplicacao Aplicacao;
        public TipoSeloController(ITipoSeloAplicacao Aplicacao)
        {
            this.Aplicacao = Aplicacao;

            ViewBag.ListaParametroSelo = new SelectList(
               Enum.GetValues(typeof(ParametroSelo)).Cast<ParametroSelo>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
               "Id",
               "Descricao");
        }

        public List<TipoSelo> ListaTipoSelo => Aplicacao?.Buscar()?.ToList() ?? new List<TipoSelo>();
        
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult BuscarTipoSelos()
        {
            var tipoSelos = AutoMapper.Mapper.Map<List<TipoSelo>, List<TipoSeloViewModel>>(ListaTipoSelo);
            foreach (var tipoSelo in tipoSelos)
                tipoSelo.ParametroSeloFormatado = tipoSelo.ParametroSelo.ToDescription();

            return PartialView("_GridTipoSelos", tipoSelos);
        }
        
        public ActionResult SalvarDados(TipoSeloViewModel TipoSeloVM)
        {
            if (TipoSeloVM.Valor == null)
                TipoSeloVM.Valor = "0";

            var valor = Convert.ToDecimal(TipoSeloVM.Valor.Replace(".", ""));
            
            var TipoSeloDM = AutoMapper.Mapper.Map<TipoSeloViewModel, TipoSelo>(TipoSeloVM);

            Aplicacao.Salvar(TipoSeloDM);

            ModelState.Clear();

            if(TipoSeloDM.ParametroSelo == ParametroSelo.Monetario || TipoSeloDM.ParametroSelo == ParametroSelo.Percentual)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Alerta",
                    Mensagem = "Registro salvo com sucesso mas é preciso parametrizar o valor do selo.",
                    TipoModal = TipoModal.Warning
                };
            }
            else
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            

            return View("Index");
        }
        
        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            var tipoSeloDM = Aplicacao.BuscarPorId(id);
            var tipoSeloVM = AutoMapper.Mapper.Map<TipoSelo, TipoSeloViewModel> (tipoSeloDM);
            tipoSeloVM.Valor = tipoSeloDM.Valor.ToString("C2").Replace("R$",string.Empty).Replace(" ", string.Empty);
            return View("Index", tipoSeloVM);
        }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
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
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index");
            }
            return View("Index");
        }

        [CheckSessionOut]
        public override  ActionResult ConfirmarDelete(int id)
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
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index");
            }
            
            return View("Index");
        }
    }
}