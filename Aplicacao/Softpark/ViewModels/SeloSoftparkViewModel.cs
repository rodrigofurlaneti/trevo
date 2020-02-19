using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a Selo
    /// </summary>
    public class SeloSoftparkViewModel : BaseSoftparkViewModel
    {
        public bool Ativo => EmissaoSelo?.StatusSelo == StatusSelo.Ativo;
        public int Sequencial { get; set; }
        public DateTime? Validade { get; set; }
        public decimal Valor { get; set; }
        public ParametroSelo ParametroSelo { get; set; }
        public EmissaoSeloSoftparkViewModel EmissaoSelo { get; set; }

        public SeloSoftparkViewModel()
        {
        }

        public SeloSoftparkViewModel(Selo selo)
        {
            Id = selo.Id;
            DataInsercao = selo.DataInsercao;
            Sequencial = selo.Sequencial;
            Validade = selo.Validade;
            Valor = selo.Valor;
            ParametroSelo = selo.EmissaoSelo.PedidoSelo.TipoSelo.ParametroSelo;
            EmissaoSelo = new EmissaoSeloSoftparkViewModel(selo.EmissaoSelo);
        }

        public Selo ToEntity()
        {
            return new Selo
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Sequencial = Sequencial,
                Validade = Validade,
                Valor = Valor
            };
        }
    }
}
