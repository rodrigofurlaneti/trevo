using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ParametroNumeroNotaFiscalController : GenericController<ParametroNumeroNotaFiscal>
    {

        private readonly IParametroNumeroNotaFiscalAplicacao  numeroNotaFiscalAplicacao;
        private readonly IUnidadeAplicacao unidadeAplicacao;

        public ParametroNumeroNotaFiscalController(IParametroNumeroNotaFiscalAplicacao numeroNotaFiscalAplicacao, IUnidadeAplicacao unidadeAplicacao)
        {
            this.numeroNotaFiscalAplicacao = numeroNotaFiscalAplicacao;
            this.unidadeAplicacao = unidadeAplicacao;
           

        }

        //public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.Buscar().Where(x => x.Precos != null).ToList());

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ViewBag.ListaUnidade = ListarUnidade();
            return View("Index");
        }

        private List<UnidadeViewModel> ListarUnidade()
        {
            return AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.Buscar().ToList());
        }

        public JsonResult BuscarParametroNFUnidade(int idUnidade)
        {

            var parametroNumeroNotaFiscalVM = AutoMapper.Mapper.Map<ParametroNumeroNotaFiscal, 
                ParametroNumeroNotaFiscalViewModel>(numeroNotaFiscalAplicacao.BuscarPor(n => n.Unidade.Id == idUnidade).FirstOrDefault());


            string message;
            var tipo = TipoModal.Success;
            var valorMaximoNota = string.Empty;
            var valorMaximoNotaDia = string.Empty;
            var id = 0;
            try
            {
                message = "Busca feita com sucesso!";
                if(parametroNumeroNotaFiscalVM != null)
                {
                    id = parametroNumeroNotaFiscalVM.Id;
                    valorMaximoNota = parametroNumeroNotaFiscalVM.ValorMaximoNota;
                    valorMaximoNotaDia = parametroNumeroNotaFiscalVM.ValorMaximoNotaDia;

                }
               
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(),valorMaximoNota,valorMaximoNotaDia,id }, ContentType = "application/json", MaxJsonLength = int.MaxValue };



        }


        public ActionResult SalvarDados(ParametroNumeroNotaFiscalViewModel parametro)
        {
            try
            {

                var parametroNumeroNotaFiscal = AutoMapper.Mapper.Map<ParametroNumeroNotaFiscalViewModel, ParametroNumeroNotaFiscal>(parametro);

                numeroNotaFiscalAplicacao.Salvar(parametroNumeroNotaFiscal);
               

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

            ViewBag.ListaUnidade = ListarUnidade();

            return View("Index");
        }




    }
}