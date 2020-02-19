using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Attributes;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;

namespace Portal.Controllers
{
    public class LayoutController : GenericController<Layout>
    {
        public List<LayoutViewModel> ListaLayouts { get; set; }
        public SelectList ListaCamposDisponiveis
        {
            get { return (SelectList)Session["ListaCamposDisponiveis"] ?? new SelectList(null); }
            set { Session["ListaCamposDisponiveis"] = value; }
        }
        public IEnumerable<ChaveValorViewModel> ListaFormatos { get; set; }

        public List<LayoutLinhaViewModel> LinhasClientSession
        {
            get { return (List<LayoutLinhaViewModel>)Session["ListaLinhasClientSession"] ?? new List<LayoutLinhaViewModel>(); }
            set { Session["ListaLinhasClientSession"] = value; }
        }

        public List<LayoutFormatoViewModel> FormatosClientSession
        {
            get { return (List<LayoutFormatoViewModel>)Session["ListaFormatosClientSession"] ?? new List<LayoutFormatoViewModel>(); }
            set { Session["ListaFormatosClientSession"] = value; }
        }
        public bool ApenasVisualizacao { get; set; }

        private readonly ILayoutAplicacao _layoutAplicacao;

        public LayoutController(ILayoutAplicacao layoutAplicacao)
        {
            Aplicacao = layoutAplicacao;
            _layoutAplicacao = layoutAplicacao;

            ListaFormatos = Enum.GetValues(typeof(FormatoExportacao)).Cast<FormatoExportacao>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
        }

        [CheckSessionOut]
        public ActionResult Exportacao()
        {
            ListaCamposDisponiveis = null;
            LinhasClientSession = null;
            FormatosClientSession = null;

            ListaLayouts = _layoutAplicacao.Buscar()?.Select(x => new LayoutViewModel(x))?.ToList() ?? new List<LayoutViewModel>();
            ListaCamposDisponiveis = GetCamposDisponiveisExportacaoSelectList();

            return View("Exportacao", new LayoutViewModel());
        }

        [CheckSessionOut]
        public JsonResult SuggestionLayout(string param, bool exact)
        {
            return Json(_layoutAplicacao.BuscarPor(x => !exact && x.Nome.Contains(param) || x.Nome == param).Select(x => new LayoutViewModel(x)));
        }

        [CheckSessionOut]
        public JsonResult CarregarLayout(int id)
        {
            var message = string.Empty;
            var tipo = TipoModal.Success;
            var divGridLinhas = string.Empty;
            var divGridFormato = string.Empty;

            try
            {
                LinhasClientSession = null;
                FormatosClientSession = null;
                var layoutViewModel = new LayoutViewModel(_layoutAplicacao.BuscarPorId(id));
                Session["ListaFormatosClientSession"] = layoutViewModel.Formatos;

                divGridLinhas = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
                divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);

                message = $"Os dados do layout {layoutViewModel.Nome}, foram carregados na tela!";
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }
            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divGridLinhas, divGridFormato }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult AddCampoExportacao(string campo, string conteudo, string codLinha)
        {
            var linha = LinhasClientSession.FirstOrDefault(x => x.CodigoLinha == codLinha);

            if (linha == null)
                return new JsonResult { Data = new { message = "Não foi encontrada a linha!", ExibirBotaoExcluir = false, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
            
            if (linha.Campos == null)
                linha.Campos = new List<LayoutCampoViewModel>();

            if (campo != "Fixo")
            {
                if (linha.Campos.Any(x => x.Campo == campo))
                    return new JsonResult { Data = new { message = "Já existe este campo na listagem!", ExibirBotaoExcluir = linha.Campos.Count > 0, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
            }
            else
            {
                if (linha.Campos.Any(x => x.Campo == campo && x.Conteudo == conteudo))
                    return new JsonResult { Data = new { message = "Já existe este campo na listagem!", ExibirBotaoExcluir = linha.Campos.Count > 0, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
            }
            linha.Campos.Add(new LayoutCampoViewModel { Campo = campo, Conteudo = conteudo, CodigoLinha = codLinha });

            var divGrid = RazorHelper.RenderRazorViewToString(ControllerContext, "GridCamposArquivo", linha.Campos);
            return new JsonResult { Data = new { message = "", ExibirBotaoExcluir = linha.Campos.Count > 0, divGrid }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult AddLinhaExportacao(string codLinha, string tipoLinha)
        {
            if (LinhasClientSession.Any(x => x.CodigoLinha == codLinha && x.TipoLinha == tipoLinha))
                return new JsonResult { Data = new { message = "Já existe este campo na listagem!", ExibirBotaoExcluir = LinhasClientSession.Count > 0, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

            if (Session["ListaLinhasClientSession"] == null)
            {
                Session["ListaLinhasClientSession"] = new List<LayoutLinhaViewModel>
                {
                    new LayoutLinhaViewModel {CodigoLinha = codLinha, TipoLinha= tipoLinha}
                };
            }
            else
                ((List<LayoutLinhaViewModel>)Session["ListaLinhasClientSession"]).Add(new LayoutLinhaViewModel { CodigoLinha = codLinha, TipoLinha = tipoLinha });

            var divGrid = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
            return new JsonResult { Data = new { message = "", ExibirBotaoExcluir = LinhasClientSession.Count > 0, divGrid }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult DeleteCampoExportacao(string listaCampo, string codLinha)
        {
            var linha = LinhasClientSession.FirstOrDefault(x => x.CodigoLinha == codLinha);

            var listaCamposExclusao = string.IsNullOrEmpty(listaCampo)
                ? null
                : JsonConvert.DeserializeObject<List<LayoutCampoViewModel>>(listaCampo)?.Select(x => x.Campo)?.ToList() ?? new List<string>();

            if (listaCamposExclusao == null || !listaCamposExclusao.Any())
                return new JsonResult { Data = new { message = "Selecione um ou mais itens para remover da listagem!", ExibirBotaoExcluir = linha.Campos.Count > 0, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };


            foreach (var item in listaCamposExclusao)
            {
                if (linha.Campos.Any(x => x.Campo == item))
                    linha.Campos.Remove(linha.Campos.FirstOrDefault(x => x.Campo == item));
            }

            var divGrid = RazorHelper.RenderRazorViewToString(ControllerContext, "GridCamposArquivo", linha.Campos);
            return new JsonResult { Data = new { message = "", ExibirBotaoExcluir = linha.Campos.Count > 0, divGrid }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }
        [CheckSessionOut]
        public JsonResult ChangeCampoExportacao(string id,
            string campo,
            string inicio,
            string final,
            string tamanho,
            string preenchimento,
            string formatacao,
            string direcao,
            string conteudo,
            string codLinha)
        {
            var message = string.Empty;
            Direcao dir;
            TipoValidacao form;

            Enum.TryParse(direcao, out dir);
            Enum.TryParse(formatacao, out form);
            var campos = LinhasClientSession.FirstOrDefault(x => x.CodigoLinha == codLinha).Campos;

            if (campos.All(x => x.Campo != campo))
                return new JsonResult { Data = new { message = "Não encontrado o registro para alteração!", ExibirBotaoExcluir = campos.Count > 0, divGrid = string.Empty }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

            if (campos.Any(x => x.Campo == campo))
            {
                var item = campos.FirstOrDefault(x => x.Campo == campo);

                if (campo == "Fixo")
                    item = campos.FirstOrDefault(x => x.Campo == campo && x.Conteudo == conteudo);

                if (item != null)
                {
                    item.PosicaoInicio = Convert.ToInt32(inicio);
                    item.PosicaoFim = Convert.ToInt32(final);
                    item.Tamanho = Convert.ToInt32(tamanho);
                    item.Preenchimento = preenchimento;
                    item.Direcao = dir;
                    item.Formatacao = form;
                }
            }

            var divGrid = RazorHelper.RenderRazorViewToString(ControllerContext, "GridCamposArquivo", campos);
            return new JsonResult { Data = new { message, ExibirBotaoExcluir = campos.Count > 0, divGrid }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult IncluirFormato(LayoutFormatoViewModel formato)
        {
            if (Session["ListaFormatosClientSession"] == null)
                Session["ListaFormatosClientSession"] = new List<LayoutFormatoViewModel>();

            if (FormatosClientSession.Any(x => x.Descricao == formato.Descricao))
            {
                return new JsonResult
                {
                    Data = new
                    {
                        message = "Já existe um Formato de Arquivo com a mesma descrição!",
                        divGridCampos = "",
                        divGridFormato = ""
                    },
                    ContentType = "application/json",
                    MaxJsonLength = int.MaxValue
                };
            }

            formato.Linhas = LinhasClientSession;
            ((List<LayoutFormatoViewModel>)Session["ListaFormatosClientSession"]).Add(formato);

            LinhasClientSession = null;

            var divGridCampos = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
            var divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);
            return new JsonResult { Data = new { message = "", divGridCampos, divGridFormato }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }
        [CheckSessionOut]
        public JsonResult AtualizarFormato(LayoutFormatoViewModel formato)
        {
            if (Session["ListaFormatosClientSession"] == null)
                Session["ListaFormatosClientSession"] = new List<LayoutFormatoViewModel>();

            if (FormatosClientSession.Any(x => x.Descricao == formato.Descricao))
            {
                var item = FormatosClientSession.FirstOrDefault(x => x.Descricao == formato.Descricao);
                item.Formato = formato.Formato;
                item.Delimitador = formato.Delimitador;

                if (item.Linhas == null)
                    item.Linhas = new List<LayoutLinhaViewModel>();

                foreach (var itemCampo in LinhasClientSession)
                {
                    if (item.Linhas.Any(x => x.CodigoLinha == itemCampo.CodigoLinha))
                    {
                        var alterarCampo = item.Linhas.FirstOrDefault(x => x.CodigoLinha == itemCampo.CodigoLinha);
                        alterarCampo.CodigoLinha = itemCampo.CodigoLinha;
                        alterarCampo.TipoLinha = itemCampo.TipoLinha;
                    }
                    else
                        item.Linhas.Add(itemCampo);
                }
            }
            else
            {
                formato.Linhas = LinhasClientSession;
                ((List<LayoutFormatoViewModel>)Session["ListaFormatosClientSession"]).Add(formato);
            }

            LinhasClientSession = null;

            var divGridCampos = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
            var divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);
            return new JsonResult { Data = new { message = "", divGridCampos, divGridFormato }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult VisualizarFormato(string id, string descricao)
        {
            ApenasVisualizacao = true;
            var viewModel = FormatosClientSession.FirstOrDefault(x => !string.IsNullOrEmpty(id) && Convert.ToInt32(id) > 0 ? x.Id == Convert.ToInt32(id) : x.Descricao == descricao);
            Session["ListaLinhasClientSession"] = viewModel?.Linhas ?? new List<LayoutLinhaViewModel>();

            var divGridCampos = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
            var divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);
            return new JsonResult { Data = new { message = "", divGridCampos, divGridFormato, Formato = viewModel }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }
        [CheckSessionOut]
        public JsonResult EditarFormato(string id, string descricao)
        {
            var viewModel = FormatosClientSession.FirstOrDefault(x => !string.IsNullOrEmpty(id) && Convert.ToInt32(id) > 0 ? x.Id == Convert.ToInt32(id) : x.Descricao == descricao);
            Session["ListaLinhasClientSession"] = viewModel?.Linhas ?? new List<LayoutLinhaViewModel>();

            var divGridCampos = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
            return new JsonResult { Data = new { message = "", divGridCampos, Formato = viewModel }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }
        [CheckSessionOut]
        public JsonResult DeletarFormato(string id, string descricao, bool limparGridCampos)
        {
            if (FormatosClientSession.Any(x => string.IsNullOrEmpty(id) ? x.Descricao == descricao : x.Id == Convert.ToInt32(id)))
                FormatosClientSession.Remove(FormatosClientSession.FirstOrDefault(x => string.IsNullOrEmpty(id) ? x.Descricao == descricao : x.Id == Convert.ToInt32(id)));

            var divGridCampos = string.Empty;
            var divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);
            return new JsonResult { Data = new { message = "", divGridCampos, divGridFormato }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public ActionResult EditExportacao(int id)
        {
            var viewModel = new LayoutViewModel();
            try
            {
                viewModel = new LayoutViewModel(_layoutAplicacao.BuscarPorId(id) ?? new Layout());
                ListaLayouts = _layoutAplicacao.Buscar()?.Select(x => new LayoutViewModel(x))?.ToList() ?? new List<LayoutViewModel>();
                ListaCamposDisponiveis = GetCamposDisponiveisExportacaoSelectList();
                Session["ListaFormatosClientSession"] = viewModel?.Formatos ?? new List<LayoutFormatoViewModel>();
                Session["ListaCamposClientSession"] = null;
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", $"Ocorreu um erro: {ex.Message}", TipoModal.Danger);
            }
            return View("Exportacao", viewModel);
        }
        [CheckSessionOut]
        public JsonResult DeletarExportacao(int id)
        {
            string message;
            var tipo = TipoModal.Success;
            var divGridExportacao = string.Empty;
            try
            {
                _layoutAplicacao.ExcluirPorId(id);

                message = "Layout de Exportação Removido com Sucesso!";

                ListaLayouts = _layoutAplicacao.Buscar()?.Select(x => new LayoutViewModel(x))?.ToList() ?? new List<LayoutViewModel>();
                divGridExportacao = RazorHelper.RenderRazorViewToString(ControllerContext, "GridExportacao", ListaLayouts);
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }
            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divGridExportacao }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public JsonResult SalvarDadosExportacao(LayoutViewModel viewModel)
        {
            string message;
            var tipo = TipoModal.Success;
            var divGridCampos = string.Empty;
            var divGridFormato = string.Empty;
            var divGridExportacao = string.Empty;
            try
            {
                _layoutAplicacao.SalvarDados(viewModel, FormatosClientSession);

                message = "Layout de Exportação Armazenado com Sucesso!";

                LinhasClientSession = null;
                FormatosClientSession = null;
                ListaLayouts = _layoutAplicacao.Buscar()?.Select(x => new LayoutViewModel(x))?.ToList() ?? new List<LayoutViewModel>();

                divGridCampos = RazorHelper.RenderRazorViewToString(ControllerContext, "GridLinhasArquivo", LinhasClientSession);
                divGridFormato = RazorHelper.RenderRazorViewToString(ControllerContext, "GridFormato", FormatosClientSession);
                divGridExportacao = RazorHelper.RenderRazorViewToString(ControllerContext, "GridExportacao", ListaLayouts);
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }
            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divGridCampos, divGridFormato, divGridExportacao }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        private Dictionary<string, string> CamposDisponiveisExportacao()
        {
            var listCampos = new Dictionary<string, string>();

            var classesExportar = new List<object>
            {
                new Pessoa(),
                new Endereco(),
                new Cidade(),
                new Estado(),
                new Carteira()
            };
            foreach (var itemClass in classesExportar)
            {
                RetornaPropriedadesExportarCampo(itemClass, ref listCampos);
            }
            return listCampos;
        }

        private void RetornaPropriedadesExportarCampo(object obj, ref Dictionary<string, string> campos)
        {
            var props = obj.GetType().GetProperties(BindingFlags.FlattenHierarchy |
                          BindingFlags.Instance |
                          BindingFlags.Public)?
                          .OrderBy(x => x.Name);
            var entityName = obj.GetType().ToString().Split('.').LastOrDefault();

            if (props != null)
                foreach (var prop in props)
                {
                    var attrs = prop.GetCustomAttributes(true);
                    if ((!attrs.Any() || !attrs.All(x => x is NotExportField))
                        && campos.All(x => x.Value != entityName && x.Key != prop.Name))
                        campos.Add($"{entityName}.{prop.Name}", prop.PropertyType.Name);
                }
        }

        private SelectList GetCamposDisponiveisExportacaoSelectList()
        {
            var campos = CamposDisponiveisExportacao();
            var lista = campos.Select(campo => new ChaveValorCategoriaViewModel
            {
                Id = campo.Key,
                Descricao = campo.Key.Split('.').LastOrDefault(),
                Categoria = campo.Key.Split('.').FirstOrDefault()
            }).ToList();

            return new SelectList(lista, "Id", "Descricao", "Categoria", 0);
        }
    }
}