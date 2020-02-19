using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;

namespace Portal.Controllers
{
    public class FilialController : GenericController<Filial>
    {
        private ContatoController _contato => new ContatoController();
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly ITipoFilialAplicacao _tipoFilialAplicacao;
        public List<Filial> ListaFiliais => Aplicacao?.Buscar()?.ToList() ?? new List<Filial>();
        public List<TipoFilialViewModel> ListaTipoFiliais => _tipoFilialAplicacao?.Buscar()?.Select(x => new TipoFilialViewModel(x)).ToList();

        public List<EmpresaViewModel> ListaEmpresas =>
            _empresaAplicacao?.Buscar()?.Select(x => AutoMapper.Mapper.Map<Empresa, EmpresaViewModel>(x)).ToList();
        public FilialController(IFilialAplicacao filialAplicacao, IEmpresaAplicacao empresaAplicacao, ITipoFilialAplicacao tipoFilialAplicacao)
        {
            Aplicacao = filialAplicacao;
            _empresaAplicacao = empresaAplicacao;
            _tipoFilialAplicacao = tipoFilialAplicacao;
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(FilialViewModel filial, EnderecoViewModel endereco)
        {
            try
            {
                filial.Endereco = endereco;
                filial.Contatos = Session["contatos"] != null
                    ? (List<ContatoViewModel>) Session["contatos"]
                    : new List<ContatoViewModel>();
                Aplicacao.Salvar(filial.ToEntity());

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

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var filial = Aplicacao.BuscarPorId(id);
            Session["contatos"] = ContatoViewModel.ContatoViewModelList(filial.Contatos.Select(x => x.Contato).ToList());
            return View("Index", new FilialViewModel(filial));
        }
    }
}