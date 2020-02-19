using Aplicacao;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Start;
using WebServices;

namespace Test.Aplicacao
{
    [TestClass]
    public class LancamentoCobrancaTests : BaseTests
    {
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;

        public LancamentoCobrancaTests()
        {
            _lancamentoCobrancaAplicacao = SimpleInjectorInitializer.Container.GetInstance<ILancamentoCobrancaAplicacao>();
        }

        [TestMethod]
        [TestCategory("Calculo")]
        public void CalculoProporcional()
        {
            var contratoMensalista = new ContratoMensalista
            {
                DataInicio = new DateTime(2019, 12, 16),
                Valor = 271.74m
            };

            //var valorSemDesconto = decimal.Round(contratoMensalista.Valor / 0.92m, 2, MidpointRounding.AwayFromZero);

            var valorMensalidade = contratoMensalista.Valor;
            var dataInicio = contratoMensalista.DataInicio;

            var daysInMonth = DateTime.DaysInMonth(dataInicio.Year, dataInicio.Month);
            var valorPorDia = decimal.Round(valorMensalidade / daysInMonth, 2, MidpointRounding.AwayFromZero);
            var primeiroDiaProporcional = 0;
            var totalDiasValorMensalidadeSemAlteracao = 0;

            var valoresPorDiaNaUnidade = new Dictionary<int, KeyValuePair<int, decimal>>();

            var unidade = new Unidade
            {
                Nome = "001",
                DiaVencimento = 20
            };
            var diaVencimento = 5;

            if (unidade.Nome == "55"
                || unidade.Nome == "055")
            {
                //valorPorDia = 8.89m;
                primeiroDiaProporcional = 4;
                totalDiasValorMensalidadeSemAlteracao = 4;
            }

            switch (diaVencimento)
            {
                case 1:
                    //valorPorDia = 8.89m;
                    primeiroDiaProporcional = daysInMonth;
                    totalDiasValorMensalidadeSemAlteracao = 3;
                    break;
                case 5:
                case 7:
                    //valorPorDia = 6.52m;
                    primeiroDiaProporcional = 4;
                    totalDiasValorMensalidadeSemAlteracao = 3;
                    break;
                default:

                    primeiroDiaProporcional = 1;
                    totalDiasValorMensalidadeSemAlteracao = 2;
                    break;
            }

            valoresPorDiaNaUnidade.Add(1, new KeyValuePair<int, decimal>(primeiroDiaProporcional, valorPorDia));

            for (int i = 2; i <= daysInMonth; i++)
            {
                var diaAnteriorLista = valoresPorDiaNaUnidade.LastOrDefault().Value.Key;
                var diaSeguinteLista = (diaAnteriorLista - 1) == 0
                                ? daysInMonth
                                : diaAnteriorLista - 1;

                var valorProporcional = ((primeiroDiaProporcional + totalDiasValorMensalidadeSemAlteracao) > daysInMonth
                                        ? totalDiasValorMensalidadeSemAlteracao
                                        : (primeiroDiaProporcional + totalDiasValorMensalidadeSemAlteracao)) >= diaSeguinteLista
                                        && diaSeguinteLista > (primeiroDiaProporcional == daysInMonth ? 1 : primeiroDiaProporcional)
                                            ? valorMensalidade
                                            : diaSeguinteLista >= unidade.DiaVencimento
                                                ? valoresPorDiaNaUnidade.LastOrDefault().Value.Value + valorPorDia
                                                : valoresPorDiaNaUnidade.LastOrDefault().Value.Value + valorPorDia;

                valoresPorDiaNaUnidade.Add(i, new KeyValuePair<int, decimal>(diaSeguinteLista, valorProporcional));
            }
            var valorDiaMensalidadeLista = valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Key == contratoMensalista.DataInicio.Day).Value.Value;
            var valorFinalMensalidade = dataInicio.Day > unidade.DiaVencimento || dataInicio.Day <= primeiroDiaProporcional ? valorMensalidade + valorDiaMensalidadeLista : valorDiaMensalidadeLista;

            var resultado = new KeyValuePair<decimal, bool>(valorFinalMensalidade, unidade.DiaVencimento <= dataInicio.Day);
            return;
        }

        [TestMethod]
        [TestCategory("Calculo")]
        public void CalculoProporcionalV2()
        {
            var contratoMensalista = new ContratoMensalista
            {
                DataInicio = new DateTime(2019, 12, 16),
                Valor = 271.74m
            };

            //var valorSemDesconto = decimal.Round(contratoMensalista.Valor / 0.92m, 2, MidpointRounding.AwayFromZero);
            var valorPorDia = decimal.Round(contratoMensalista.Valor / 30, 2, MidpointRounding.AwayFromZero);

            var valorMensalidade = contratoMensalista.Valor;
            var dataInicio = contratoMensalista.DataInicio;

            var diaDeVencimento = 1;
            var maxDiasMensalidadeLiquida = 2;
            var maxDiasProporcional = 11;

            var valoresPorDiaNaUnidade = new Dictionary<int, KeyValuePair<int, decimal>>();

            for (int i = 1; i <= 30; i++)
            {
                var diaAnteriorLista = valoresPorDiaNaUnidade.Any() ? valoresPorDiaNaUnidade.LastOrDefault().Value.Key : diaDeVencimento;
                var diaSeguinteLista = !valoresPorDiaNaUnidade.Any()
                                        ? diaDeVencimento
                                        : diaAnteriorLista == 30 
                                            ? 1 : diaAnteriorLista + 1;

                var totalDiasMes = 30 - diaSeguinteLista + 1;

                var valor = diaSeguinteLista >= diaDeVencimento && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida)
                            ? valorMensalidade
                            : diaSeguinteLista >= (diaDeVencimento + maxDiasMensalidadeLiquida)
                                && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional)
                                    ? diaDeVencimento > 1
                                        ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia
                                        : totalDiasMes * valorPorDia
                                    : diaSeguinteLista < diaDeVencimento
                                        ? valorPorDia * (diaDeVencimento - diaSeguinteLista) + valorMensalidade 
                                        : diaDeVencimento > 1 
                                            ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia + valorMensalidade
                                            : totalDiasMes * valorPorDia + valorMensalidade;
                
                valoresPorDiaNaUnidade.Add(i, new KeyValuePair<int, decimal>(diaSeguinteLista, valor));
            }

            var valorFinalMensalidade = valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Key == contratoMensalista.DataInicio.Day).Value.Value;

            var data2 = DateTime.Parse("05-12-2019 00:00:00");
            var data1 = DateTime.Parse("29-12-2019 00:00:00");
            var monthDiff = GetMonthDifference(data2, data1);

            
var resultado = new KeyValuePair<decimal, bool>(valorFinalMensalidade, (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional) <= dataInicio.Day);
            return;
        }

        [TestMethod]
        [TestCategory("Calculo")]
        public void CalculoProporcionalV3()
        {
            var contratoMensalista = new ContratoMensalista
            {
                DataInicio = new DateTime(2019, 12, 16),
                Valor = 217.39m
            };

            var diaInicio = contratoMensalista.DataInicio.Day == 31 ? 30 : contratoMensalista.DataInicio.Day;

            var valorMensalidadeComDesconto = contratoMensalista.Valor - (contratoMensalista.Valor * 0.08m); //decimal.Round(contratoMensalista.Valor / 0.92m, 2, MidpointRounding.AwayFromZero);
            //Valor por Dia é sobre a Mensalidade Sem Desconto!
            var valorPorDia = contratoMensalista.Valor / 30;

            var diaDeVencimento = 1; //_unidadeRepositorio.GetById(contratoMensalista.Unidade.Id)?.DiaVencimento ?? 1;
            var maxDiasMensalidadeLiquida = 2;
            var maxDiasProporcional = 11;

            var valoresPorDiaNaUnidade = new Dictionary<int, PMSVO>();

            for (int i = 1; i <= 30; i++)
            {
                var diaAnteriorLista = valoresPorDiaNaUnidade.Any() ? valoresPorDiaNaUnidade.LastOrDefault().Value.Dia : diaDeVencimento;
                var diaSeguinteLista = !valoresPorDiaNaUnidade.Any()
                                        ? diaDeVencimento
                                        : diaAnteriorLista == 30
                                            ? 1 : diaAnteriorLista + 1;

                var totalDiasMes = 30 - diaSeguinteLista + 1;
                
                var valorMensa = 0m;
                var valorDia = 0m;
                var statusPms = StatusPMS.ValorLiquido;
                if (diaSeguinteLista >= diaDeVencimento && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida))
                {
                    valorMensa = valorMensalidadeComDesconto;
                    statusPms = StatusPMS.ValorLiquido;
                }
                else if (diaSeguinteLista >= (diaDeVencimento + maxDiasMensalidadeLiquida) && diaSeguinteLista <= (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional))
                {
                    valorDia = diaDeVencimento > 1
                                ? (totalDiasMes + diaDeVencimento - 1) * valorPorDia
                                : totalDiasMes * valorPorDia;
                    statusPms = StatusPMS.Proporcional;
                }
                else if (diaSeguinteLista < diaDeVencimento)
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = valorPorDia * (diaDeVencimento - diaSeguinteLista);
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                }
                else if (diaDeVencimento > 1)
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = (totalDiasMes + diaDeVencimento - 1) * valorPorDia;
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                }
                else
                {
                    valorMensa = valorMensalidadeComDesconto;
                    valorDia = totalDiasMes * valorPorDia;
                    statusPms = StatusPMS.ProporcionalMesSeguinte;
                };


                valoresPorDiaNaUnidade.Add(i, new PMSVO(diaSeguinteLista, decimal.Round(valorMensa, 2, MidpointRounding.AwayFromZero), decimal.Round(valorDia, 2, MidpointRounding.AwayFromZero), statusPms));
            }

            var valorFinalMensalidade = valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Dia == contratoMensalista.DataInicio.Day).Value.ValorResultadoFinal;

            //var resultado = new KeyValuePair<KeyValuePair<decimal, decimal>, bool>(valorFinalMensalidade, (diaDeVencimento + maxDiasMensalidadeLiquida + maxDiasProporcional) <= diaInicio);
            //return new KeyValuePair<KeyValuePair<decimal, decimal>, bool>(valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Key == contratoMensalista.DataInicio.Day).Value.Value, diaDeVencimento <= diaInicio);

            var result = valoresPorDiaNaUnidade.FirstOrDefault(x => x.Value.Dia == contratoMensalista.DataInicio.Day).Value;
        }

        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}