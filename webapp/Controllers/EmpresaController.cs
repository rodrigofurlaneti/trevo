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
    public class EmpresaController : GenericController<Empresa>
    {
        private ContatoController _contato => new ContatoController();
        private readonly IGrupoAplicacao _grupoAplicacao;
        public List<Empresa> ListaEmpresas => Aplicacao?.Buscar()?.ToList() ?? new List<Empresa>();
        public List<GrupoViewModel> ListaGrupos => _grupoAplicacao?.Buscar()?.Select(x=> new GrupoViewModel(x)).ToList();
        public readonly ICidadeAplicacao _cidadeAplicacao;

        public EmpresaController(IEmpresaAplicacao empresaAplicacao, IGrupoAplicacao grupoAplicacao, ICidadeAplicacao cidadeAplicacao)
        {
            Aplicacao = empresaAplicacao;
            _grupoAplicacao = grupoAplicacao;
            _cidadeAplicacao = cidadeAplicacao;
        }

		[CheckSessionOut]
	    [HttpPost]
	    public ActionResult SalvarDados(EmpresaViewModel empresa, EnderecoViewModel endereco)
	    {
		    try
		    {
			    empresa.Endereco = endereco;
                if (empresa.Endereco.Cidade != null)
                    empresa.Endereco.Cidade = new CidadeViewModel(_cidadeAplicacao.BuscarPor(x => x.Descricao == endereco.Cidade.Descricao).FirstOrDefault());

			    empresa.Contatos = Session["contatos"] != null ? (List<ContatoViewModel>)Session["contatos"] : new List<ContatoViewModel>();
			    Aplicacao.Salvar(empresa.ToEntity());

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
		    var loja = Aplicacao.BuscarPorId(id);
		    Session["contatos"] = ContatoViewModel.ContatoViewModelList(loja.Contatos.Select(x => x.Contato).ToList());
		    return View("Index", new EmpresaViewModel(loja));
	    }
	}
}