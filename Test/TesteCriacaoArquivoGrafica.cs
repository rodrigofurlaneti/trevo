using System.IO;
using System.ComponentModel;
using Core.Extensions;
using System.Linq;
using System.Collections.Generic;
using Aplicacao;

namespace Test
{
    public enum RegArquivo
    {
        [Description("00")]
        InicioDadosBoleto = 0,
        [Description("01")]
        IdentificaCarta = 1,
        [Description("02")]
        IdentificaContratos = 2,
        [Description("03")]
        IdentificaOpcoesPagamento = 3,
        [Description("04")]
        Produto = 4,
        [Description("05")]
        Parcela = 5,
        [Description("06")]
        TotalizadorContratos = 6,
        [Description("07")]
        ContratosV1 = 7,
        [Description("08")]
        IdentificaOpcoesPagamentoV2 = 8,
        [Description("09")]
        ContratosV2 = 9,
        [Description("10")]
        Instrucoes = 10
    }

    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TesteCriacaoArquivoGrafica
    {
        private readonly IPromessaAplicacao _promessaAplicacao;
        private readonly IParametroFaturamentoAplicacao _parametroFaturamentoAplicacao;

        public TesteCriacaoArquivoGrafica(IPromessaAplicacao promessaAplicacao, IParametroFaturamentoAplicacao parametroFaturamentoAplicacao)
        {
            _promessaAplicacao = promessaAplicacao;
            _parametroFaturamentoAplicacao = parametroFaturamentoAplicacao;
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void TestMethod1()
        {
            var fileNameAndPath = "c:\arquivoTeste.txt";
            if (File.Exists(fileNameAndPath))
                File.Delete(fileNameAndPath);

            var promessaExport = _promessaAplicacao.Buscar()?.ToList() ?? new List<Entidade.Promessa>();
            var parametroFaturamento = _parametroFaturamentoAplicacao.Buscar()?.ToList().FirstOrDefault();

            var valor = new StreamWriter(fileNameAndPath, true, System.Text.Encoding.ASCII);

            if (promessaExport?.Any() ?? false)
                return;

            valor.Write(RegArquivo.InicioDadosBoleto.ToDescription());

            foreach (var item in promessaExport)
            {
                var primeiroContrato = item?.Contratos?.FirstOrDefault();
                var devedor = primeiroContrato?.Contrato?.Devedor;
                var ultimoEndereco = devedor?.Pessoa?.Enderecos?.LastOrDefault();
                var cep = ultimoEndereco?.Endereco?.Cep?.ExtractNumbers() ?? string.Empty;

                //01
                valor.WriteLine(RegArquivo.IdentificaCarta.ToDescription()
                    + cep.PadRight(8)
                    + string.Empty.PadRight(8)
                    + (item.Parcelas?.FirstOrDefault()?.NossoNumero.ToString() ?? string.Empty).PadRight(15)
                    + (devedor?.Pessoa?.Nome ?? string.Empty).PadRight(40)
                    + (devedor?.Pessoa?.DocumentoCpf?.ExtractLettersAndNumbers() ?? string.Empty).PadRight(14)
                    + (ultimoEndereco?.Endereco?.Logradouro ?? string.Empty).PadRight(55)
                    + (ultimoEndereco?.Endereco?.CidadeNome ?? string.Empty).PadRight(20)
                    + (ultimoEndereco?.Endereco?.UF ?? string.Empty).PadRight(2)
                    + (((item.TipoLote == Entidade.Uteis.TipoLote.NovoNegocio
                            || item.TipoLote == Entidade.Uteis.TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.ToShortDateString()
                                : item.Parcelas?.FirstOrDefault()?.DataVencimento.ToShortDateString()) ?? string.Empty).PadRight(10) //vencimento da parcela exibida no grid conforme pesquisa, falta corrigir
                    + (((item.TipoLote == Entidade.Uteis.TipoLote.NovoNegocio
                            || item.TipoLote == Entidade.Uteis.TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.ToShortDateString()
                                : item.Parcelas?.FirstOrDefault()?.DataVencimento.ToShortDateString()) ?? string.Empty).PadRight(10) //vencimento da parcela exibida no grid conforme pesquisa, falta corrigir
                    + (primeiroContrato?.Contrato?.Carteira?.Produto?.Sigla ?? string.Empty).PadRight(4)
                    + (item?.Contratos?.FirstOrDefault()?.Contrato?.CodContrato ?? string.Empty).PadRight(20)
                    + (parametroFaturamento?.Convenio ?? string.Empty).PadRight(20)
                    + (parametroFaturamento?.Agencia ?? string.Empty).PadRight(5)
                    + (parametroFaturamento?.Conta ?? string.Empty).PadRight(8)
                    + (parametroFaturamento?.Empresa?.Nome ?? string.Empty).PadRight(300)
                    + ("08000074216" ?? string.Empty).PadRight(12));


                foreach (var contrato in item.Contratos)
                {
                    //02
                    valor.WriteLine(RegArquivo.IdentificaContratos.ToDescription()
                        + (((item.TipoLote == Entidade.Uteis.TipoLote.NovoNegocio
                            || item.TipoLote == Entidade.Uteis.TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.ToShortDateString()
                                : item.Parcelas?.FirstOrDefault()?.DataVencimento.ToShortDateString()) ?? string.Empty).PadRight(10)
                        + (contrato.Contrato?.ValorContrato.ToString("N2").ExtractNumbers() ?? string.Empty).PadRight(10, '0')
                        + (contrato.Contrato?.CodContrato ?? string.Empty).PadRight(10));
                }

                if (item.TipoLote == Entidade.Uteis.TipoLote.NovoNegocio)
                {
                    foreach (var plano in item.PlanosDePagamento.Where(x => x.Ativo))
                    {
                        //03 > exibir os planos estipulados
                        valor.WriteLine(RegArquivo.IdentificaOpcoesPagamento.ToDescription()
                            + (plano.Numero.ToString().PadLeft(2, '0')).PadRight(2)
                            + (plano.Valor.ToString("N2").ExtractNumbers() ?? string.Empty).PadRight(10, '0')
                            + (plano.Valor.ToString("N2").ExtractNumbers() ?? string.Empty).PadRight(10, '0')
                            + (0).ToString().PadRight(10, '0'));
                    }
                }

                //06 > valores finais
                valor.WriteLine(RegArquivo.TotalizadorContratos.ToDescription()
                    + (item.Contratos.Sum(x => x.Contrato?.ValorContrato ?? 0).ToString("N2").ExtractNumbers() ?? string.Empty).PadRight(10, '0')
                    + (item.Contratos.Sum(x => x.Contrato?.ValorContrato ?? 0).ToString("N2").ExtractNumbers() ?? string.Empty).PadRight(10, '0')
                    + (0).ToString().PadRight(10, '0'));
            }

            valor.Close();
        }
    }
}
