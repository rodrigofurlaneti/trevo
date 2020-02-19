using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ParametrosLayoutViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public CarteiraViewModel Carteira { get; set; }
        //public ParametrosCarteiraViewModel ParametrosCarteira { get; set; }
        public LayoutViewModel Layout { get; set; }
        public FormatoCarga FormatoCarga { get; set; }
        public List<ParametrosLayoutViewModel> ParametrosProdutoViewModelList(IList<ParametrosLayout> parametrosProdutos) => parametrosProdutos.Select(parametrosProduto => new ParametrosLayoutViewModel(parametrosProduto)).ToList();

        public ParametrosLayoutViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ParametrosLayoutViewModel(ParametrosLayout parametrosLayout)
        {
            Id = parametrosLayout?.Id ?? 0;
            Carteira = new CarteiraViewModel(parametrosLayout?.Carteira);
            //ParametrosCarteira = new ParametrosCarteiraViewModel(parametrosLayout?.ParametrosCarteira);
            Layout = new LayoutViewModel(parametrosLayout?.Layout);
            FormatoCarga = parametrosLayout?.FormatoCarga ?? FormatoCarga.ParcSaldoUnico;
            DataInsercao = parametrosLayout?.DataInsercao ?? DateTime.Now;
        }

        public ParametrosLayout ToEntity() => new ParametrosLayout
        {
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Carteira = Carteira?.ToEntity(),
            //ParametrosCarteira = ParametrosCarteira?.ToEntity(),
            Layout = Layout?.ToEntity(),
            Id = Id,
            FormatoCarga = FormatoCarga
        };
    }
}