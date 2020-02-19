using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using System.Data;

namespace Portal.Controllers
{
    public class IntegracaoController : GenericController<Integracao>
    {
        private readonly IIntegracaoAplicacao _integracaoAplicacao;
        private readonly IAssessoriaAplicacao _assessoriaAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IContratoAplicacao _contratoAplicacao;
        private readonly ICredorAplicacao _credorAplicacao;
        private readonly ITipoOcorrenciaAplicacao _tipoOcorrenciaAplicacao;
        private readonly IOcorrenciaAplicacao _ocorrenciaAplicacao;

        public List<IntegracaoViewModel> ListaIntegracoes { get; private set; }
        public int PermiteGeracaoExcel { get; set; }

        public IntegracaoController(IIntegracaoAplicacao integracaoAplicacao, IAssessoriaAplicacao assessoriaAplicacao, IUsuarioAplicacao usuarioAplicacao, IContratoAplicacao contratoAplicacao,
            ICredorAplicacao credorAplicacao, ITipoOcorrenciaAplicacao tipoOcorrenciaAplicacao, IOcorrenciaAplicacao ocorrenciaAplicacao)
        {
            _integracaoAplicacao = integracaoAplicacao;
            _assessoriaAplicacao = assessoriaAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _contratoAplicacao = contratoAplicacao;
            _credorAplicacao = credorAplicacao;
            _tipoOcorrenciaAplicacao = tipoOcorrenciaAplicacao;
            _ocorrenciaAplicacao = ocorrenciaAplicacao;
        }

        public override ActionResult Index()
        {
            ListaIntegracoes = _integracaoAplicacao.Buscar().Select(x => new IntegracaoViewModel(x)).ToList();
            return View();
        }

        private Integracao GeraIntegracao(string fileName, string fileExtension, string leiaute)
        {
            string[] stringSeparators = new string[] { "_" };
            string[] result;
            result = fileName.Split(stringSeparators, StringSplitOptions.None);

            var currentUser = HttpContext.User as dynamic;
            int usuarioId = Convert.ToInt32(currentUser.UsuarioId);

            Assessoria _assessoria = _assessoriaAplicacao.BuscarPor(x => x.Codigo == result[3].ToString().Replace(fileExtension.ToString(), "")).FirstOrDefault();
            Usuario _usuario = _usuarioAplicacao.BuscarPor(x => x.Id == usuarioId).FirstOrDefault();

            var _data = result[0].ToString();
            var _hora = result[1].ToString();

            DateTime _DataArquivo = new DateTime(int.Parse(_data.Substring(0, 4)),
                                                    int.Parse(_data.Substring(4, 2)),
                                                    int.Parse(_data.Substring(6, 2)),
                                                    int.Parse(_hora.Substring(0, 2)),
                                                    int.Parse(_hora.Substring(2, 2)),
                                                    int.Parse(_hora.Substring(4, 2)));

            Integracao _objIntegracao = new Integracao();
            _objIntegracao.Assessoria = _assessoria;
            _objIntegracao.DataHoraArquivo = _DataArquivo;
            _objIntegracao.DataInsercao = DateTime.Now;
            _objIntegracao.Leiaute = (TipoLeiauteImportacao)(Convert.ToInt32(leiaute));
            _objIntegracao.Lote = 2;
            _objIntegracao.NomeArquivo = fileName;
            _objIntegracao.Status = true;
            _objIntegracao.UsuarioImportacao = _usuario;

            return _integracaoAplicacao.SalvarComRetorno(_objIntegracao);
        }

        private Ocorrencia ProcessaLinha(string line, Integracao integracao)
        {
            var _codAssessoria = line.Substring(0, 3).Trim();
            var _CPFCNPJ = line.Substring(3, 14).Trim();
            var _contrato = line.Substring(17, 30).Trim();
            var _dataOcorrencia = line.Substring(47, 8).Trim();
            var _sequencial = line.Substring(55, 3).Trim();
            var _codOcorrencia = line.Substring(58, 10).Trim();
            var _operadorId = line.Substring(68, 9).Trim();
            var _numTelefone = line.Substring(77, 14).Trim();
            var _DDD = line.Substring(91, 2).Trim();
            var _credor = line.Substring(93, 3).Trim();
            var _comentario = line.Substring(96, 255).Trim();

            var _assessoriaEntity = _assessoriaAplicacao.BuscarPor(x => x.Codigo == _codAssessoria).FirstOrDefault();
            var _contratoEntity = _contratoAplicacao.BuscarPor(x => x.CodContrato == _contrato).FirstOrDefault();
            var _credorEntity = _credorAplicacao.BuscarPor(x => x.Codigo == Convert.ToInt32(_credor)).FirstOrDefault();
            var _tipoOcorrenciaEntity = _tipoOcorrenciaAplicacao.BuscarPor(x => x.Codigo == _codOcorrencia).FirstOrDefault();
            var _integracao = new Integracao { Id = integracao.Id };

            Ocorrencia _objInclusao = new Ocorrencia
            {
                Integracao = _integracao,
                DataInsercao = DateTime.Now,
                Assessoria = _assessoriaEntity,
                Contrato = _contratoEntity,
                Credor = _credorEntity,
                TipoOcorrencia = _tipoOcorrenciaEntity,
                Comentario = _comentario,
                CPFCNPJ = _CPFCNPJ,
                DataOcorrencia = new DateTime(int.Parse(_dataOcorrencia.Substring(0, 4)), int.Parse(_dataOcorrencia.Substring(4, 2)), int.Parse(_dataOcorrencia.Substring(6, 2))),
                DDD = int.Parse(_DDD),
                IdOperador = int.Parse(_operadorId),
                Sequencia = int.Parse(_sequencial),
                Status = true,
                Telefone = int.Parse(_numTelefone)
            };

            return _objInclusao;
        }

        [HttpPost]
        public JsonResult Importar(int Leiaute)
        {
            try
            {
                var postedFile = Request.Files["file"];
                var leiaute = Request["leiaute"];
                var fileExtension = Path.GetExtension(postedFile.FileName)?.ToLower();
                var fileName = Path.GetFileName(postedFile.FileName);
                var fileBytes = new byte[postedFile.ContentLength];
                postedFile.InputStream.Read(fileBytes, 0, fileBytes.Length);

                if (TipoLeiauteImportacao.ArquivoOcorrencia == ((TipoLeiauteImportacao)Convert.ToInt32(leiaute)))
                {
                    Integracao _objInserido = GeraIntegracao(fileName, fileExtension, leiaute);

                    var json = new StringBuilder();
                    using (var reader = new StreamReader(new MemoryStream(fileBytes), Encoding.GetEncoding("iso-8859-1")))
                    {
                        var lista = new List<string>();
                        string line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            var _ocorrencia = ProcessaLinha(line, _objInserido);
                            _ocorrenciaAplicacao.Salvar(_ocorrencia);
                        }
                    }
                }

                return Json(new Resultado<ArquivoImportacaoViewModel>()
                {
                    Sucesso = true,
                    TipoModal = TipoModal.Success.ToDescription(),
                    Titulo = "Importação de arquivo",
                    Mensagem = "Importação realizada com sucesso."
                });
             }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<ArquivoImportacaoViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Importação de arquivo",
                    Mensagem = br.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new Resultado<ArquivoImportacaoViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Importação de arquivo",
                    Mensagem = ex.Message
                });
            }
        }

        [CheckSessionOut]
        public void ExportToExcel(int leiaute)
        {
            try
            {
                List<IntegracaoViewModel> lista = _integracaoAplicacao.Buscar().Select(x => new IntegracaoViewModel(x)).ToList();
                DateTime _date = DateTime.Now;
                string _formatDate = _date.Year.ToString() + _date.Month.ToString().PadLeft(2, '0') + _date.Day.ToString().PadLeft(2, '0');
                var _arquivo = "integracoes_" + _formatDate;

                Download.Criar(Grid(lista), _arquivo, ContentType.xls);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção",
                    br.Message,
                    TipoModal.Warning);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção",
                    "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal.Danger);
            }

        }

        [CheckSessionOut]
        public void ReportOcorrenciasToExcel(DateTime periodoInicio, DateTime periodoFim, int leiaute, int assessoria, int? lote)
        {
            try
            {
                List<OcorrenciaViewModel> lista = _ocorrenciaAplicacao.BuscarPor(x => x.Integracao.Leiaute == (TipoLeiauteImportacao)leiaute && x.Assessoria.Id == assessoria).Select(x => new OcorrenciaViewModel(x)).ToList();
                lista = lista.Where(x => x.DataOcorrencia >= periodoInicio && x.DataOcorrencia <= periodoFim).ToList();

                if (lote != null && lote != 0)
                {
                    lista = lista.Where(x => x.Integracao.Lote == Convert.ToInt32(lote)).ToList();
                }

                DateTime _date = DateTime.Now;
                string _formatDate = _date.Year.ToString() + _date.Month.ToString().PadLeft(2, '0') + _date.Day.ToString().PadLeft(2, '0');
                var _arquivo = "ocorrencias_" + _formatDate;

                Download.Criar(GridOcorrencia(lista), _arquivo, ContentType.xls);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção",
                    br.Message,
                    TipoModal.Warning);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção",
                    "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal.Danger);
            }

        }

        private object Grid(List<IntegracaoViewModel> lista)
        {
            List<object> _lista = new List<object>();

            foreach (var item in lista)
            {
                object _objinsert = new
                {
                    LOTE = item.Lote.ToString(),
                    ASSESSORIA = item.Assessoria?.Codigo ?? "",
                    ARQUIVO = item.NomeArquivo?.ToString() ?? "",
                    DATA = item.DataHoraArquivo.ToShortDateString(),
                    HORA = item.DataHoraArquivo.ToShortTimeString(),
                    USUARIO = item.UsuarioImportacao.Pessoa.Nome,
                    IMPORTACAO = item.DataInsercao.ToShortDateString() + " " + item.DataInsercao.ToShortTimeString()
                };

                _lista.Add(_objinsert);
            }

            return _lista;
        }

        private object GridOcorrencia(List<OcorrenciaViewModel> lista)
        {
            List<object> _lista = new List<object>();

            foreach (var item in lista)
            {
                object _objinsert = new
                {
                    ASSESSORIA = item.Assessoria.Descricao,
                    CPF_CNPJ = item.CPFCNPJ,
                    CONTRATO = item.Contrato.CodContrato,
                    DATA_ATIVIDADE = item.DataOcorrencia.ToShortDateString(),
                    SEQUENCIAL = item.Sequencia.ToString(),
                    COD_OCORRENCIA = item.TipoOcorrencia.Codigo,
                    DESCRICAO_OCORRENCIA = item.TipoOcorrencia.Descricao,
                    ID_OPERADOR = item.IdOperador.ToString(),
                    NUMERO_TELEFONICO = item.Telefone.ToString(),
                    DDD = item.DDD.ToString(),
                    CREDOR = item.Credor.Codigo.ToString(),
                    COMENTARIOS = item.Comentario
                };

                _lista.Add(_objinsert);
            }

            return _lista;
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult Pesquisar(int leiaute)
        {
            List<IntegracaoViewModel> _retorno = new List<IntegracaoViewModel>();

            try
            {
                List<Integracao> _lista = _integracaoAplicacao.BuscarPor(x => x.Leiaute == (TipoLeiauteImportacao)leiaute).ToList();
                _retorno = _lista.Select(y => new IntegracaoViewModel(y)).ToList();

                if (_retorno.Count > 0)
                    PermiteGeracaoExcel = 1;
                else
                    PermiteGeracaoExcel = 0;

            }
            catch (Exception ex)
            {
                return Json(new Models.Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            ListaIntegracoes = _retorno;

            return PartialView("_ListaIntegracoes", _retorno);
        }

        public JsonResult CarregarAssessorias(int leiaute)
        {
            var lista = new List<Assessoria>
            {
                new Assessoria()
                {
                    Descricao = @"Selecione uma Assessoria.",
                    Id = 0
                }
            };

            var busca = _integracaoAplicacao.BuscarPor(x => x.Leiaute == ((TipoLeiauteImportacao)Convert.ToInt32(leiaute))).ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Id", "Descricao"));

            foreach (var item in busca)
            {
                if(!lista.Any( x => x.RazaoSocial == item.Assessoria.RazaoSocial))
                    lista.Add(item.Assessoria);
            }

            return Json(new SelectList(lista, "Id", "Descricao", 0));
        }

        public JsonResult CarregarLotes(int leiaute, int assessoria)
        {

            var lista = new List<SelectListItem>
            {
                new SelectListItem(){
                    Text = @"Selecione um Lote.",
                    Value = "0"
                }
            };

            var busca = _integracaoAplicacao.BuscarPor(x => x.Leiaute == ((TipoLeiauteImportacao)Convert.ToInt32(leiaute)) && x.Assessoria.Id == assessoria).ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Value", "Text"));

            foreach (var item in busca)
            {
                var _objLista = new SelectListItem();
                _objLista.Text = item.Lote.ToString() + " - " + item.NomeArquivo;
                _objLista.Value = item.Id.ToString();
                lista.Add(_objLista);
            }

            return Json(new SelectList(lista, "Value", "Text", 0));
        }
    }
}