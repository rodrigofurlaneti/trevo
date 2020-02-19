using Aplicacao.ViewModels;
using System;
using System.Collections.Generic;

namespace Portal.Helpers
{
    public static class ParametroNegociacao
    {
        public static IList<LimiteDescontoViewModel> ListaParametroNegociacaoLimiteDesconto(List<string> tiposServico)
        {
            var limitesDesconto = new List<LimiteDescontoViewModel>();
            foreach(var tipoServico in tiposServico)
            {
                var limiteDesconto = new LimiteDescontoViewModel();
                limiteDesconto.TipoServico = (Entidade.Uteis.TipoServico)Enum.Parse(typeof(Entidade.Uteis.TipoServico), tipoServico);
                limitesDesconto.Add(limiteDesconto);
            }

            return limitesDesconto;
        }

        internal static void ListaParametroNegociacaoLimiteDesconto(ref List<LimiteDescontoViewModel> limitesDesconto, List<string> tiposServico)
        {
            foreach (var tipoServico in tiposServico)
            {
                if(limitesDesconto.Exists(obj=> obj.TipoServico.ToString() == tipoServico))
                {
                    continue;
                }
                var limiteDesconto = new LimiteDescontoViewModel();
                limiteDesconto.TipoServico = (Entidade.Uteis.TipoServico)Enum.Parse(typeof(Entidade.Uteis.TipoServico), tipoServico);
                limitesDesconto.Add(limiteDesconto);
            }
        }
    }
}