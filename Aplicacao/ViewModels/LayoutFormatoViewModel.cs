using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LayoutFormatoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Descricao { get; set; }
        public FormatoExportacao Formato { get; set; }
        public string Delimitador { get; set; }
        public LayoutViewModel Layout { get; set; }
        
        public List<LayoutLinhaViewModel> Linhas { get; set; }

        public LayoutFormatoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LayoutFormatoViewModel(LayoutFormato layout)
        {
            Id = layout?.Id ?? 0;
            Descricao = layout?.Descricao;
            DataInsercao = layout?.DataInsercao ?? DateTime.Now;
            Formato = layout?.Formato ?? FormatoExportacao.Excel;
            Delimitador = layout?.Delimitador;
            Linhas = new LayoutLinhaViewModel().LayoutLinhaViewModelList(layout?.Linhas?.ToList() ?? new List<LayoutLinha>());
        }

        public List<LayoutFormatoViewModel> LayoutFormatoViewModelList(IList<LayoutFormato> layouts)
        {
            return layouts.Select(layout => new LayoutFormatoViewModel(layout)).ToList();
        }

        public LayoutFormato ToEntity() => new LayoutFormato
        {
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Id = this.Id,
            Descricao = this.Descricao,
            Formato = this.Formato,
            Delimitador = this.Delimitador,
            Linhas = this?.Linhas?.Select(x => x.ToEntity()).ToList(),
            //Layout = new Layout { Id = Layout?.Id??0 }
        };
    }
}