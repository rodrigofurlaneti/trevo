using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Helpers;
using Portal.Models;

namespace Portal.Controllers
{
    public class UploadController : Controller
    {
        private Dictionary<string, string> ListaArquivosUploaded =>
            (Dictionary<string, string>)Session["ArquivosUploaded"] ?? new Dictionary<string, string>();

        private readonly IContratoAplicacao _contratoAplicacao;
        private readonly IRetornoNegativacaoAplicacao _retornoNegativacaoAplicacao;
        private readonly IHistoricoNegativacaoAplicacao _historicoNegativacaoAplicacao;
        private readonly IHistoricoArquivoAplicacao _historicoArquivoAplicacao;

        public UploadController(IContratoAplicacao contratoAplicacao,
            IRetornoNegativacaoAplicacao retornoNegativacaoAplicacao,
            IHistoricoNegativacaoAplicacao historicoNegativacaoAplicacao,
            IHistoricoArquivoAplicacao historicoArquivoAplicacao)
        {
            _contratoAplicacao = contratoAplicacao;
            _retornoNegativacaoAplicacao = retornoNegativacaoAplicacao;
            _historicoNegativacaoAplicacao = historicoNegativacaoAplicacao;
            _historicoArquivoAplicacao = historicoArquivoAplicacao;
        }

        [CheckSessionOut]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [CheckSessionOut]
        public void UploadArquivo()
        {
            if (Request.Files["file"] != null)
                RegistrarUpload(TipoArquivo.Arquivo, Request.Files["file"]);
        }

        [CheckSessionOut]
        public ActionResult Processar()
        {
            try
            {
                if (ListaArquivosUploaded == null || ListaArquivosUploaded.Count == 0)
                    throw new BusinessRuleException("Selecione um arquivo para realizar o processamento do mesmo.");

                var codigoRetornoHeader = 0;
                var codigoRetornoConsumidor = 0;
                var dataMovimentacao = DateTime.MinValue;
                var lista = new List<HistoricoNegativacao>();

                foreach (var upload in ListaArquivosUploaded)
                {
                    var fileStream = new FileStream(upload.Value, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string line;
                        HistoricoArquivo arquivo = null;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            line = $" {line}";
                            if (line.Substring(1, 2) == "00")
                            {
                                if (line.Substring(3, 7) != "RETORNO")
                                    throw new BusinessRuleException("Este não é um arquivo de retorno!!!");

                                var data = line.Substring(10, 8);
                                dataMovimentacao =
                                    Convert.ToDateTime(
                                        $"{data.Substring(0, 2)}/{data.Substring(2, 2)}/{data.Substring(4, 4)}", new CultureInfo("pt-br"));

                                codigoRetornoHeader = int.Parse(line.Substring(375, 10));
                                var nameSplit1 = fileStream.Name.Split('\\');
                                var nameSplit2 = nameSplit1.LastOrDefault()?.Split('.');
                                arquivo = _historicoArquivoAplicacao
                                    .BuscarPor(x => x.NomeArquivo == nameSplit2[0])
                                    .FirstOrDefault();

                                if (arquivo == null) continue;

                                arquivo.StatusArquivoSpcCartorio = StatusArquivoSpcCartorio.RetornoProcessadoSpc;
                                _historicoArquivoAplicacao.Salvar(arquivo);
                            }

                            if (line.Substring(1, 2) == "01")
                                codigoRetornoConsumidor = int.Parse(line.Substring(375, 10));

                            if (line.Substring(1, 2) != "02") continue;

                            var codigoOperacao = line.Substring(19, 1);
                            var codigoContrato = line.Substring(50, 30);
                            var codigoRetorno = line.Substring(375, 10);
                            if (string.IsNullOrEmpty(codigoRetorno)) continue;

                            if (arquivo == null)
                                arquivo = _historicoArquivoAplicacao
                                    .BuscarPor(x => x.NomeArquivo == fileStream.Name)
                                    .FirstOrDefault();

                            var contrato = _contratoAplicacao
                                .BuscarPor(x => x.CodContrato == codigoContrato)
                                .FirstOrDefault();

                            if (codigoOperacao == "E")
                            {
                                contrato.Devedor.Pessoa.NegativacaoSPC = false;
                                _contratoAplicacao.Salvar(contrato);
                            }

                            var status =
                                _retornoNegativacaoAplicacao
                                    .BuscarPor(x => x.Codigo == Convert.ToInt32(codigoRetorno))
                                    .FirstOrDefault();

                            var historico = new HistoricoNegativacao()
                            {
                                DataSolicitacao = dataMovimentacao,
                                Destino = HistoricoNegativacaoDestino.SPC,
                                DataEfetivacao = DateTime.Now,
                                TipoOperacao =
                                    codigoOperacao == "I"
                                        ? TipoOperacaoNegativacao.Inclusao
                                        : TipoOperacaoNegativacao.Exclusao,
                                Arquivo = arquivo,
                                Contrato = contrato,
                                Status = status
                            };

                            lista.Add(historico);

                            if (codigoRetornoHeader > 0)
                            {
                                historico.Status = _retornoNegativacaoAplicacao
                                                       .BuscarPor(x => x.Codigo == codigoRetornoHeader)
                                                       .FirstOrDefault();
                                lista.Add(historico);
                            }

                            if (codigoRetornoConsumidor > 0)
                            {
                                historico.Status = _retornoNegativacaoAplicacao
                                                       .BuscarPor(x => x.Codigo == codigoRetornoConsumidor)
                                                       .FirstOrDefault();
                                lista.Add(historico);
                            }

                            _historicoNegativacaoAplicacao.Salvar(lista);
                        }
                    }
                }

                return Json(new Resultado<object>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Uploado de arquivo",
                    Mensagem = ListaArquivosUploaded.Count > 1
                        ? "Os arquivos foram processados com sucesso."
                        : "O arquivo foi processado com sucesso."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = br.Message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = new BusinessRuleException(ex.Message).Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private void RegistrarUpload(TipoArquivo tipoArquivo, HttpPostedFileBase file)
        {
            try
            {
                var pathFile = Path.Combine(PathArquivo.ReturnPath(tipoArquivo), file.FileName);
                file.SaveAs(pathFile);

                var listaArquivos = ListaArquivosUploaded;
                listaArquivos.Add(tipoArquivo.ToString(), pathFile);
                Session["ArquivosUploaded"] = listaArquivos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}