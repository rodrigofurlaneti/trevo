using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ImportacaoPagamentoController : GenericController<ImportacaoPagamento>
    {
        public List<ImportacaoPagamentoViewModel> ListaImpPagamento => RetornaListaMok();
        public SelectList ListaLayoutArquivo => TiposArquivoPagamento();

        public ImportacaoPagamentoController(IImportacaoPagamentoAplicacao importacaoPagamentoAplicacao)
        {
            Aplicacao = importacaoPagamentoAplicacao;
        }

        // GET: ImportacaoPagamento
        public override ActionResult Index()
        {
            return View();
        }

        public SelectList TiposArquivoPagamento()
        {
            return new SelectList(Enum.GetValues(typeof(LayoutArquivoPagamento)).OfType<LayoutArquivoPagamento>()
        .Select(x => new SelectListItem
        {
            Text = GetEnumDescription(x),
            Value = (Convert.ToInt32(x)).ToString()
        }), "Value", "Text");

        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        private List<ImportacaoPagamentoViewModel> RetornaListaMok()
        {
            var lista = new List<ImportacaoPagamentoViewModel>();

            lista.Add(new ImportacaoPagamentoViewModel
            {
                Arquivo = "arquivoTest.txt",
                Cedente = "123",
                DataInsercao = DateTime.Now,
                DataPagamento = DateTime.Now,
                Id = 1,
                Lote = 123,
                Pagamento = new List<PagamentoViewModel>(),
                Usuario = new UsuarioViewModel()
            });

            lista.Add(new ImportacaoPagamentoViewModel
            {
                Arquivo = "arquivoTest2.txt",
                Cedente = "321",
                DataInsercao = DateTime.Now,
                DataPagamento = DateTime.Now,
                Id = 2,
                Lote = 321,
                Pagamento = new List<PagamentoViewModel>(),
                Usuario = new UsuarioViewModel()
            });

            return lista;
        }

        public ActionResult BuscarArquivos(int cedente)
        {
            var listaArquivos = ListaImpPagamento;

            if (cedente > 0)
                listaArquivos = listaArquivos.Where(x=>x.Cedente == cedente.ToString()).ToList();

            var listaRetorno = listaArquivos.ToList();

            return PartialView("_GridDadosPagamentos", listaRetorno);
        }

    }
}