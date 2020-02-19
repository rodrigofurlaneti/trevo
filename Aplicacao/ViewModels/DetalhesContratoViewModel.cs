using System;
using System.Collections.Generic;
using Core.Extensions;

namespace Aplicacao.ViewModels
{
	public class DetalhesContratoViewModel
	{
		public string NomeDevedor { get; set; }
		public string CPFDevedor { get; set; }
		public bool? NegativacaoSPC { get; set; }
		public string Bens { get; set; }
		public DateTime? DataPagamento { get; set; }
		public string LojaCompra { get; set; }
		public int NumeroDaUltimaParcelaPaga { get; set; }
		public string DataPagamentoFormatada => DataPagamento == null ? string.Empty : DataPagamento?.ToShortDateString();
		public string CPFDevedorFormatado => CPFDevedor.FormatCpfCnpj();
		public string ConstaSPC => NegativacaoSPC == null ? "Sem info" : NegativacaoSPC.Value ? "Negativado" : "Nada Consta";
	}
}