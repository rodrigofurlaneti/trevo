using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using Portal.Models;

namespace Portal.Controllers
{
    public class NegativacaoController : GenericController<Negativacao>
    {
        //private readonly IHistoricoArquivoAplicacao _historicoArquivoAplicacao;
        private readonly IHistoricoNegativacaoAplicacao _historicoNegativacaoAplicacao;
        private readonly IRetornoNegativacaoAplicacao _retornoNegativacaoAplicacao;
        //private readonly IContratoAplicacao _contratoAplicacao;

        public SelectList ListaDestino { get; private set; }
        public SelectList ListaSituacao { get; private set; }

        public NegativacaoController(INegativacaoAplicacao negativacaoAplicacao,
            //IHistoricoArquivoAplicacao historicoArquivoAplicacao,
            IHistoricoNegativacaoAplicacao historicoNegativacaoAplicacao
            //, IRetornoNegativacaoAplicacao retornoNegativacaoAplicacao,
            //IContratoAplicacao contratoAplicacao
            )

        {
            Aplicacao = negativacaoAplicacao;
            //_historicoArquivoAplicacao = historicoArquivoAplicacao;
            _historicoNegativacaoAplicacao = historicoNegativacaoAplicacao;
            //_retornoNegativacaoAplicacao = retornoNegativacaoAplicacao;
            //_contratoAplicacao = contratoAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaDestino = CarregarDestinos();
            ListaSituacao = CarregarSituacao();

            ModelState.Clear();
            return View("Index");
        }

        //[CheckSessionOut]
        //public ActionResult Pesquisar(string json)
        //{
        //    try
        //    {
        //        ListaDestino = CarregarDestinos();
        //        ListaSituacao = CarregarSituacao();

        //        if (string.IsNullOrEmpty(json))
        //            throw new BusinessRuleException("É necessário informar um período para a consulta.");

        //        var filtros = JsonConvert.DeserializeObject<FiltroNegativacaoViewModel>(json,
        //            new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy" });

        //        if (filtros.DataDe == DateTime.MinValue)
        //            throw new BusinessRuleException("Informe uma data de início válida!");

        //        if (filtros.DataAte == DateTime.MinValue)
        //            throw new BusinessRuleException("Informe uma data final válida!");

        //        var lista = _historicoArquivoAplicacao.BuscarPor(x => x.DataInsercao >= filtros.DataDe
        //                                                              && x.DataInsercao <= filtros.DataAte.AddHours(23)
        //                                                                  .AddMinutes(59).AddSeconds(59));

        //        if (filtros.CodigoDestino > 0)
        //            lista = lista.Where(x => (int)x.TipoArquivo == filtros.CodigoDestino).ToList();

        //        if (filtros.CodigoSituacao > 0)
        //            lista = lista.Where(x => (int)x.StatusArquivoSpcCartorio == filtros.CodigoSituacao).ToList();

        //        var model = lista.Select(x => new Negativacao()
        //        {
        //            Id = x.Id,
        //            Data = x.DataInsercao,
        //            NomeArquivo = x.NomeArquivo,
        //            TipoArquivo = x.TipoArquivo,
        //            Destino = x.TipoArquivo.ToDescription(),
        //            Situacao = x.StatusArquivoSpcCartorio.ToDescription()
        //        }).ToList();

        //        return PartialView("_Grid", model);
        //    }
        //    catch (BusinessRuleException br)
        //    {
        //        return Json(new Resultado<object>()
        //        {
        //            Sucesso = false,
        //            TipoModal = TipoModal.Warning.ToDescription(),
        //            Titulo = "Atenção",
        //            Mensagem = br.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new Resultado<object>()
        //        {
        //            Sucesso = false,
        //            TipoModal = TipoModal.Danger.ToDescription(),
        //            Titulo = "Atenção",
        //            Mensagem = new BusinessRuleException(ex.Message).Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[CheckSessionOut]
        //public ActionResult AlterarStatus(int idArquivo, int status)
        //{
        //    try
        //    {
        //        var historico = _historicoArquivoAplicacao.BuscarPorId(idArquivo);
        //        historico.StatusArquivoSpcCartorio = (StatusArquivoSpcCartorio)status;

        //        _historicoArquivoAplicacao.Salvar(historico);

        //        var novoStatus = _retornoNegativacaoAplicacao.BuscarPor(x => x.Destino == HistoricoNegativacaoDestino.Cartorio && x.Codigo == status).FirstOrDefault();
        //        var listaNegativacao = _historicoNegativacaoAplicacao.BuscarPor(x => x.Arquivo.Id == idArquivo);

        //        foreach (var historicoNegativacao in listaNegativacao)
        //        {
        //            historicoNegativacao.DataEfetivacao = DateTime.Now;
        //            historicoNegativacao.Status = novoStatus;
        //            _historicoNegativacaoAplicacao.Salvar(historicoNegativacao);

        //            if ((StatusArquivoSpcCartorio)status != StatusArquivoSpcCartorio.RetornoProcessadoCart) continue;

        //            var contrato = _contratoAplicacao.BuscarPorId(historicoNegativacao.Contrato.Id);
        //            contrato.NegativacaoCartorio = ((StatusArquivoSpcCartorio)status == StatusArquivoSpcCartorio.AguardandoRetornoCart) ? true : false;
        //            _contratoAplicacao.Salvar(contrato);
        //        }

        //        return Json(new Resultado<object>()
        //        {
        //            Sucesso = true,
        //            TipoModal = TipoModal.Success.ToDescription(),
        //            Titulo = "Alterar situação do arquivo",
        //            Mensagem = "Alteração realizada com sucesso."
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (BusinessRuleException br)
        //    {
        //        return Json(new Resultado<object>()
        //        {
        //            Sucesso = false,
        //            TipoModal = TipoModal.Warning.ToDescription(),
        //            Titulo = "Atenção",
        //            Mensagem = br.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new Resultado<object>()
        //        {
        //            Sucesso = false,
        //            TipoModal = TipoModal.Danger.ToDescription(),
        //            Titulo = "Atenção",
        //            Mensagem = new BusinessRuleException(ex.Message).Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        private static SelectList CarregarDestinos()
        {
            var lista = new List<ListItem>
            {
                new ListItem
                {
                    Text = @"Selecione um...",
                    Value = 0.ToString()
                },
                new ListItem
                {
                    Text = TipoArquivoSpcCartorio.Cartorio.ToDescription(),
                    Value = ((int) TipoArquivoSpcCartorio.Cartorio).ToString()
                },
                new ListItem
                {
                    Text = TipoArquivoSpcCartorio.SPC.ToDescription(),
                    Value = ((int) TipoArquivoSpcCartorio.SPC).ToString()
                }
            };

            return new SelectList(lista, "Value", "Text");
        }

        private static SelectList CarregarSituacao()
        {
            var lista = new List<ListItem>
            {
                new ListItem
                {
                    Text = @"Selecione um...",
                    Value = 0.ToString()
                },
                new ListItem
                {
                    Text = StatusArquivoSpcCartorio.AguardandoRetornoSpc.ToDescription(),
                    Value = ((int) StatusArquivoSpcCartorio.AguardandoRetornoSpc).ToString()
                },
                new ListItem
                {
                    Text = StatusArquivoSpcCartorio.RetornoProcessadoSpc.ToDescription(),
                    Value = ((int) StatusArquivoSpcCartorio.RetornoProcessadoSpc).ToString()
                },
                new ListItem
                {
                    Text = StatusArquivoSpcCartorio.AguardandoRetornoCart.ToDescription(),
                    Value = ((int) StatusArquivoSpcCartorio.AguardandoRetornoCart).ToString()
                },
                new ListItem
                {
                    Text = StatusArquivoSpcCartorio.RetornoProcessadoCart.ToDescription(),
                    Value = ((int) StatusArquivoSpcCartorio.RetornoProcessadoCart).ToString()
                },
                new ListItem
                {
                    Text = StatusArquivoSpcCartorio.CanceladoCart.ToDescription(),
                    Value = ((int) StatusArquivoSpcCartorio.CanceladoCart).ToString()
                }
            };

            return new SelectList(lista, "Value", "Text");
        }
    }
}