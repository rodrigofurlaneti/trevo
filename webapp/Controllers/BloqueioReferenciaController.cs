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
    public class BloqueioReferenciaController : GenericController<BloqueioReferencia>
    {
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;

        public BloqueioReferenciaController(IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao)
        {
            Aplicacao = bloqueioReferenciaAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(BloqueioReferenciaViewModel bloqueioReferenciaVM)
        {
            try
            {
                var objSalvar = bloqueioReferenciaVM.Id > 0
                    ? Aplicacao.BuscarPorId(bloqueioReferenciaVM.Id)
                    : AutoMapper.Mapper.Map<BloqueioReferenciaViewModel, BloqueioReferencia>(bloqueioReferenciaVM);

                if (objSalvar.Id > 0)
                    objSalvar.Ativo = bloqueioReferenciaVM.Ativo;
                else
                {
                    var retorno = Aplicacao.PrimeiroPor(x => x.DataMesAnoReferencia == bloqueioReferenciaVM.DataMesAnoReferencia);
                    if (retorno != null && retorno.Id > 0)
                        throw new BusinessRuleException("Não pode criar um novo registro com a mesma data de referência!");
                }

                Aplicacao.Salvar(objSalvar);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", bloqueioReferenciaVM);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", bloqueioReferenciaVM);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var bloqueioReferencia = new BloqueioReferenciaViewModel(Aplicacao.BuscarPorId(id));

            return View("Index", bloqueioReferencia);
        }

        public ActionResult BuscarDados()
        {
            var bloqueioReferenciaVM = AutoMapper.Mapper.Map<List<BloqueioReferencia>, List<BloqueioReferenciaViewModel>>(Aplicacao.Buscar().ToList());

            return PartialView("_GridBloqueioReferencia", bloqueioReferenciaVM);
        }

        [CheckSessionOut]
        [HttpPost]
        public JsonResult VerificarLiberacao(DadosValidacaoNotificacaoDesbloqueioReferenciaModal model)
        {
            var msg = string.Empty;
            var resultado = new Models.Resultado<BloqueioReferenciaViewModel>();
            try
            {
                TempData[$"LiberacaoBloqueioReferencia_{model.EntidadeRegistro}"] = model;

                var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(model.IdNotificacao, model.IdRegistro, model.EntidadeRegistro, model.DataReferencia, new Usuario { Id = UsuarioLogado.UsuarioId });
                model.IdNotificacao = retorno.Key;
                model.StatusDesbloqueioLiberacao = retorno.Value;

                TempData[$"LiberacaoBloqueioReferencia_{model.EntidadeRegistro}"] = model;
                TempData.Keep();
                
                switch (retorno.Value)
                {
                    case StatusDesbloqueioLiberacao.Aguardando:
                        msg = "Aguardando aprovação...";
                        break;
                    case StatusDesbloqueioLiberacao.Aprovado:
                        msg = "Liberado desbloqueio da referência.";
                        break;
                    case StatusDesbloqueioLiberacao.Negado:
                        msg = "Não liberado ou não possui permissão.";
                        break;
                    case StatusDesbloqueioLiberacao.Utilizado:
                        msg = "Liberação já utilizada, solicite uma nova liberação!";
                        break;
                    default:
                        throw new BusinessRuleException($"Não definido uma mensagem para o status recebido [{retorno.Value.ToDescription()}]!");
                }

                var tipoModel = TipoModal.Success;
                switch (retorno.Value)
                {
                    case StatusDesbloqueioLiberacao.Aguardando:
                        tipoModel = TipoModal.Warning;
                        break;
                    case StatusDesbloqueioLiberacao.Aprovado:
                        tipoModel = TipoModal.Success;
                        break;
                    case StatusDesbloqueioLiberacao.Negado:
                        tipoModel = TipoModal.Danger;
                        break;
                    case StatusDesbloqueioLiberacao.Utilizado:
                    default:
                        tipoModel = TipoModal.Info;
                        break;
                }

                resultado = new Models.Resultado<BloqueioReferenciaViewModel>
                {
                    Sucesso = retorno.Value == StatusDesbloqueioLiberacao.Aprovado,
                    TipoModal = tipoModel.ToDescription(),
                    Titulo = "Verificar Liberação de Bloqueio de Referência",
                    Mensagem = msg
                };
            }
            catch (Exception ex)
            {
                resultado = new Models.Resultado<BloqueioReferenciaViewModel>
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Verificar Liberação de Bloqueio de Referência",
                    Mensagem = ex.Message
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    resultado,
                    Liberacao = model,
                    Status = model.StatusDesbloqueioLiberacao.ToDescription()
                }
            };
        }

        [CheckSessionOut]
        [HttpPost]
        public JsonResult NovaDeliberacao(DadosValidacaoNotificacaoDesbloqueioReferenciaModal model)
        {
            var msg = string.Empty;
            var resultado = new Models.Resultado<BloqueioReferenciaViewModel>();
            try
            {
                TempData[$"LiberacaoBloqueioReferencia_{model.EntidadeRegistro}"] = model;

                var retorno = _bloqueioReferenciaAplicacao.GerarNovaRequisicao(model.IdRegistro, model.EntidadeRegistro, model.DataReferencia, new Usuario { Id = UsuarioLogado.UsuarioId });
                model.IdNotificacao = retorno.Key;
                model.StatusDesbloqueioLiberacao = retorno.Value;

                TempData[$"LiberacaoBloqueioReferencia_{model.EntidadeRegistro}"] = model;
                TempData.Keep();

                msg = "Criado nova solicitação para liberação de referência!";

                var tipoModel = TipoModal.Success;
                switch (retorno.Value)
                {
                    case StatusDesbloqueioLiberacao.Aguardando:
                        tipoModel = TipoModal.Warning;
                        break;
                    case StatusDesbloqueioLiberacao.Aprovado:
                        tipoModel = TipoModal.Success;
                        break;
                    case StatusDesbloqueioLiberacao.Negado:
                        tipoModel = TipoModal.Danger;
                        break;
                    case StatusDesbloqueioLiberacao.Utilizado:
                    default:
                        tipoModel = TipoModal.Info;
                        break;
                }

                resultado = new Models.Resultado<BloqueioReferenciaViewModel>
                {
                    Sucesso = retorno.Value == StatusDesbloqueioLiberacao.Aprovado,
                    TipoModal = tipoModel.ToDescription(),
                    Titulo = "Requisição p/ Liberação de Bloqueio de Referência",
                    Mensagem = msg
                };
            }
            catch (Exception ex)
            {
                resultado = new Models.Resultado<BloqueioReferenciaViewModel>
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Requisição p/ Liberação de Bloqueio de Referência",
                    Mensagem = ex.Message
                };
            }
            return new JsonResult
            {
                Data = new
                {
                    resultado,
                    Liberacao = model,
                    Status = model.StatusDesbloqueioLiberacao.ToDescription()
                }
            };
        }
    }
}
