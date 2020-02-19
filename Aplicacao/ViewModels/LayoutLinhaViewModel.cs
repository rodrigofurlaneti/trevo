using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LayoutLinhaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string TipoLinha { get; set; }
        public string CodigoLinha { get; set; }
        public IList<LayoutCampoViewModel> Campos { get; set; }
        public LayoutFormato LayoutFormato { get; set; }
        public int QuantidadeColunas => Campos == null || !Campos.Any() ? 0 : Campos.Count;
        
        public LayoutLinhaViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LayoutLinhaViewModel(LayoutLinha layout)
        {
            Id = layout?.Id ?? 0;
            DataInsercao = layout?.DataInsercao ?? DateTime.Now;
            TipoLinha = layout?.TipoLinha;
            CodigoLinha = layout?.CodigoLinha;
            Campos = new LayoutCampoViewModel().LayoutCampoViewModelList(layout?.Campos?.ToList() ?? new List<LayoutCampo>());
        }

        public List<LayoutLinhaViewModel> LayoutLinhaViewModelList(IList<LayoutLinha> layouts)
        {
            return layouts.Select(layout => new LayoutLinhaViewModel(layout)).ToList();
        }

        public LayoutLinha ToEntity() => new LayoutLinha
        {
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Id = this.Id,
            TipoLinha = this.TipoLinha,
            CodigoLinha = this?.CodigoLinha,
            Campos = this?.Campos.Select(x=>x.ToEntity()).ToList()
            //Layout = new Layout { Id = Layout?.Id??0 }
        };
    }
}