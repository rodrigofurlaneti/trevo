using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class EmpresaViewModel
    {
	    public int Id { get; set; }
	    public DateTime DataInsercao { get; set; }
	    public string Descricao { get; set; }
	    public string CNPJ { get; set; }
	    public string InscricaoEstadual { get; set; }
	    public string InscricaoMunicipal { get; set; }
	    public string RazaoSocial { get; set; }
	    public int CodigoGrupo { get; set; }
	    public EnderecoViewModel Endereco { get; set; }
	    public List<ContatoViewModel> Contatos { get; set; }
		public GrupoViewModel Grupo{ get; set; }

		public EmpresaViewModel()
	    {
		    this.Endereco = new EnderecoViewModel();
		    this.Contatos = new List<ContatoViewModel>();
		    this.Grupo = new GrupoViewModel();
	    }

	    public EmpresaViewModel(Empresa empresa)
	    {
		    this.Id = empresa?.Id ?? 0;
		    this.DataInsercao = empresa?.DataInsercao ?? DateTime.Now;
		    this.Descricao = empresa?.Descricao;
		    this.InscricaoEstadual = empresa?.InscricaoEstadual;
		    this.RazaoSocial = empresa?.RazaoSocial;
		    this.CNPJ = empresa?.CNPJ;
		    this.Endereco = new EnderecoViewModel(empresa?.Endereco);
		    this.Contatos = ContatoViewModel.ContatoViewModelList(empresa?.Contatos?.Select(x => x.Contato).ToList() ?? new List<Contato>());
		    this.Grupo = new GrupoViewModel(empresa?.Grupo);
		    this.CodigoGrupo = empresa?.Grupo?.Id ?? 0;
	    }

	    public Empresa ToEntity() => new Empresa()
	    {
		    Id = this.Id,
		    CNPJ = this.CNPJ,
		    Contatos = this?.Contatos?.Select(x => new EmpresaContato() { Contato = x.ToEntity() }).ToList(),
		    DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
		    Descricao = this.Descricao,
		    Endereco = this?.Endereco.ToEntity(),
		    RazaoSocial = this?.RazaoSocial,
		    InscricaoEstadual = this?.InscricaoEstadual,
		    Grupo = this?.CodigoGrupo > 0 ? new Grupo() { Id = this.CodigoGrupo } : null
	    };
	}
}