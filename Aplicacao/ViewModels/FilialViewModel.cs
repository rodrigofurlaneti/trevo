using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aplicacao.ViewModels
{
    public class FilialViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string RazaoSocial { get; set; }
        public int CodigoTipoFilial { get; set; }
        public int CodigoEmpresa { get; set; }
        public EnderecoViewModel Endereco { get; set; }
        public List<ContatoViewModel> Contatos { get; set; }
        public  EmpresaViewModel Empresa { get; set; }
        public  TipoFilialViewModel TipoFilial { get; set; }

        public FilialViewModel()
        {
            this.Endereco = new EnderecoViewModel();
            this.Contatos = new List<ContatoViewModel>();
            this.Empresa = new EmpresaViewModel();
            this.TipoFilial = new TipoFilialViewModel();
        }

        public FilialViewModel(Filial filial)
        {
            this.Id = filial?.Id ?? 0;
            this.DataInsercao = filial?.DataInsercao ?? DateTime.Now;
            this.Descricao = filial?.Descricao;
            this.InscricaoEstadual = filial?.InscricaoEstadual;
            this.RazaoSocial = filial?.RazaoSocial;
            this.CNPJ = filial?.CNPJ;
            this.Endereco = new EnderecoViewModel(filial?.Endereco);
            this.Contatos = ContatoViewModel.ContatoViewModelList(filial?.Contatos.Select(x => x.Contato).ToList() ?? new List<Contato>());
            this.Empresa = AutoMapper.Mapper.Map<Empresa, EmpresaViewModel>(filial?.Empresa);  //new EmpresaViewModel(filial?.Empresa);
            this.TipoFilial = new TipoFilialViewModel(filial?.TipoFilial);
            this.CodigoEmpresa = filial?.Empresa?.Id ?? 0;
            this.CodigoTipoFilial = filial?.TipoFilial?.Id ?? 0;
        }

        public Filial ToEntity() => new Filial()
        {
            Id = this.Id,
            CNPJ = this.CNPJ,
            Contatos = this?.Contatos?.Select(x => new FilialContato() {Contato = x.ToEntity()}).ToList(),
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Descricao = this.Descricao,
            Endereco = this?.Endereco.ToEntity(),
            RazaoSocial = this?.RazaoSocial,
            InscricaoEstadual = this?.InscricaoEstadual,
            Empresa = this?.CodigoEmpresa > 0 ? new Empresa() { Id = this.CodigoEmpresa} : null,
            TipoFilial = this?.CodigoTipoFilial > 0 ? new TipoFilial() { Id = this.CodigoTipoFilial} : null
        };
    }
}
