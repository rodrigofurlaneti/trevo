using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LayoutCampoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Conteudo { get; set; }
        public string Campo { get; set; }
        public int PosicaoInicio { get; set; }
        public int PosicaoFim { get; set; }
        public int Tamanho { get; set; }
        public TipoValidacao Formatacao { get; set; }
        public string Preenchimento { get; set; }
        public Direcao Direcao { get; set; }
        public LayoutLinhaViewModel LayoutFormato { get; set; }

        //propriedade auxiliar
        public string CodigoLinha { get; set; }

        public LayoutCampoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LayoutCampoViewModel(LayoutCampo layout)
        {
            Id = layout?.Id ?? 0;
            DataInsercao = layout?.DataInsercao ?? DateTime.Now;
            Conteudo = layout?.Conteudo;
            Campo = layout?.Campo;
            PosicaoInicio = layout?.PosicaoInicio ?? 0;
            PosicaoFim = layout?.PosicaoFim ?? 0;
            Tamanho = layout?.Tamanho ?? 0;
            Formatacao = layout?.Formatacao ?? TipoValidacao.Alfanumerico;
            Preenchimento = layout?.Preenchimento;
            Direcao = layout?.Direcao ?? Direcao.Right;
        }

        public List<LayoutCampoViewModel> LayoutCampoViewModelList(IList<LayoutCampo> layouts)
        {
            return layouts.Select(layout => new LayoutCampoViewModel(layout)).ToList();
        }

        public LayoutCampo ToEntity() => new LayoutCampo
        {
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Id = Id,
            Conteudo = this.Conteudo,
            Campo = this.Campo,
            PosicaoInicio = this.PosicaoInicio,
            PosicaoFim = this.PosicaoFim,
            Tamanho = this.Tamanho,
            Formatacao = this.Formatacao,
            Preenchimento = this.Preenchimento,
            Direcao = this.Direcao,
            //LayoutFormato = new LayoutFormato { Id = LayoutFormato?.Id ?? 0 }
        };
    }
}