using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class LayoutViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public List<LayoutFormatoViewModel> Formatos { get; set; }
        public int QuantidadeFormatos => Formatos == null || !Formatos.Any() ? 0 : Formatos.Count;
        public List<LayoutViewModel> LayoutViewModelList(IList<Layout> layouts) => layouts.Select(layout => new LayoutViewModel(layout)).ToList();

        public LayoutViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LayoutViewModel(Layout layout)
        {
            Id = layout?.Id ?? 0;
            DataInsercao = layout?.DataInsercao ?? DateTime.Now;
            Nome = layout?.Nome;
            Formatos = new LayoutFormatoViewModel().LayoutFormatoViewModelList(layout?.Formatos?.ToList() ?? new List<LayoutFormato>());
        }        

        public Layout ToEntity() => new Layout
        {
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Id = Id,
            Nome = Nome,
            Formatos = this?.Formatos?.Select(x => x.ToEntity()).ToList()
        };
    }
}