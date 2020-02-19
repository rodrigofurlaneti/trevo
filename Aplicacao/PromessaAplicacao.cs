using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IPromessaAplicacao : IBaseAplicacao<Promessa>
    {
        IList<Promessa> PesquisaPromessa(string credor, string produto, string carteira, string dataInicio, string dataFim, TipoLote tipoLote);
        IList<Promessa> PesquisaPromessa(string credor, string produto, string carteira, string dataVencimento, TipoLote tipoLote, string nomeLote);
        string GerarArquivoGrafica(List<ConsultaBoletagemViewModel> promessaExport, string guid);
    }

    public class PromessaAplicacao : BaseAplicacao<Promessa, IPromessaServico>, IPromessaAplicacao
    {
        private readonly IPromessaServico _promessaServico;
        private readonly IParametroFaturamentoServico _parametroFaturamentoServico;

        public PromessaAplicacao(IPromessaServico promessaServico, IParametroFaturamentoServico parametroFaturamentoServico)
        {
            _promessaServico = promessaServico;
            _parametroFaturamentoServico = parametroFaturamentoServico;
        }

        public IList<Promessa> PesquisaPromessa(string credor, string produto, string carteira, string dataInicio, string dataFim, TipoLote tipoLote)
        {
            return _promessaServico.PesquisaPromessa(credor, produto, carteira, dataInicio, dataFim, tipoLote);
        }

        public IList<Promessa> PesquisaPromessa(string credor, string produto, string carteira, string dataVencimento, TipoLote tipoLote, string nomeLote)
        {
            return _promessaServico.PesquisaPromessa(credor, produto, carteira, dataVencimento, tipoLote, nomeLote);
        }

        public string GerarArquivoGrafica(List<ConsultaBoletagemViewModel> promessaExport, string guid)
        {
            if (promessaExport == null || !promessaExport.Any())
                return null;

            var fileNameAndPath = $@"{AppDomain.CurrentDomain.BaseDirectory}Content\files\arqgrf_{guid}.txt";

            var parametroFaturamento = _parametroFaturamentoServico.Buscar()?.ToList().FirstOrDefault();

            var valor = new StreamWriter(fileNameAndPath, true, System.Text.Encoding.ASCII);
            valor.WriteLine(RegArquivo.InicioDadosBoleto.ToDescription());

            foreach (var item in promessaExport)
            {
                var primeiroContrato = item?.Contrato?.FirstOrDefault();
                var devedor = item?.Devedor;
                var ultimoEndereco = devedor?.Pessoa?.Enderecos?.LastOrDefault();
                var cep = ultimoEndereco?.Cep?.ExtractNumbers() ?? string.Empty;

                //01
                valor.WriteLine(RegArquivo.IdentificaCarta.ToDescription()
                    + cep.PadRight(8, ' ')
                    + string.Empty.PadRight(8, ' ')
                    + (item?.ParcelaNossoNumero.ToString() ?? string.Empty).PadRight(15, ' ')
                    + (devedor?.Pessoa?.Nome ?? string.Empty).PadRight(40, ' ')
                    + (devedor?.Pessoa?.Cpf?.ExtractLettersAndNumbers() ?? string.Empty).PadRight(14, ' ')
                    + (ultimoEndereco?.Logradouro ?? string.Empty).PadRight(55, ' ')
                    + (ultimoEndereco?.CidadeDescricao ?? string.Empty).PadRight(20, ' ')
                    + (ultimoEndereco?.UF ?? string.Empty).PadRight(2, ' ')
                    + (((item.TipoLote == TipoLote.NovoNegocio
                            || item.TipoLote == TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.Value.ToShortDateString()
                                : item.DataVencimento.Value.ToShortDateString()) ?? string.Empty).PadRight(10, ' ')
                    + (((item.TipoLote == TipoLote.NovoNegocio
                            || item.TipoLote == TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.Value.ToShortDateString()
                                : item.DataVencimento.Value.ToShortDateString()) ?? string.Empty).PadRight(10, ' ')
                    + (primeiroContrato?.Carteira?.Produto?.Sigla ?? string.Empty).PadRight(4, ' ')
                    + (item?.Contrato?.FirstOrDefault()?.CodContrato ?? string.Empty).PadRight(20, ' ')
                    + (parametroFaturamento?.Convenio ?? string.Empty).PadRight(20, ' ')
                    + (parametroFaturamento?.Agencia ?? string.Empty).PadRight(5, ' ')
                    + (parametroFaturamento?.Conta ?? string.Empty).PadRight(8, ' ')
                    + (parametroFaturamento?.Empresa?.Nome ?? string.Empty).PadRight(300, ' ')
                    + ("08000074216" ?? string.Empty).PadRight(12, ' '));


                foreach (var contrato in item.Contrato)
                {
                    //02
                    valor.WriteLine(RegArquivo.IdentificaContratos.ToDescription()
                        + (((item.TipoLote == TipoLote.NovoNegocio
                            || item.TipoLote == TipoLote.AcordoEmAtraso)
                                ? item.DataVencimento.Value.ToShortDateString()
                                : item?.DataVencimento.Value.ToShortDateString()) ?? string.Empty).PadLeft(10, ' ')
                        + (contrato.ValorContrato.ToString("N2").ExtractNumbers() ?? string.Empty).PadLeft(10, '0')
                        + (contrato.CodContrato ?? string.Empty).PadRight(10, ' '));
                }

                if (item.TipoLote == TipoLote.NovoNegocio)
                {
                    foreach (var plano in item.PlanosPagamento.Where(x => x.Ativo))
                    {
                        //03 > exibir os planos estipulados
                        valor.WriteLine(RegArquivo.IdentificaOpcoesPagamento.ToDescription()
                            + (plano.Numero.ToString().PadLeft(2, '0')).PadLeft(2, ' ')
                            + (plano.Valor.ToString("N2").ExtractNumbers() ?? string.Empty).PadLeft(10, '0')
                            + (plano.Valor.ToString("N2").ExtractNumbers() ?? string.Empty).PadLeft(10, '0')
                            + (0).ToString().PadRight(10, '0'));
                    }
                }

                //06 > valores finais
                valor.WriteLine(RegArquivo.TotalizadorContratos.ToDescription()
                    + (item.ValorDivida.ToString("N2").ExtractNumbers() ?? string.Empty).PadLeft(10, '0')
                    + (item.ValorDivida.ToString("N2").ExtractNumbers() ?? string.Empty).PadLeft(10, '0')
                    + (0).ToString().PadLeft(10, '0'));
            }

            valor.Close();

            var texto = File.ReadAllText(fileNameAndPath);
            File.Delete(fileNameAndPath);

            return texto;
        }
    }
}