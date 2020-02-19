using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Aplicacao.ViewModels;
using BoletoNet;
using Core.Extensions;
using Entidade.Uteis;

namespace Portal.Helpers
{
    public static class ArquivoRemessa
    {
        private static FileStream arquivoStream;

        public static string Criar(Operacao operacao, AssessoriaViewModel assessoria, List<DistribuicaoViewModel> lista)
        {
            if (lista == null) return "";

            var devedores = lista.GroupBy(x => x.Contrato.Devedor.Id)
                                .Select(grp => grp.First())
                                .Select(x => x.Contrato.Devedor)
                                .OrderBy(x => x.Id)
                                .ToList();

            foreach (var devedor in devedores)
            {
                devedor.Contratos = lista.Where(x => x.Contrato.Devedor.Id == devedor.Id).Select(x => new DevedorContratoViewModel { Contrato = x.Contrato }).ToList();
            }

            return Criar(operacao, assessoria, devedores);
        }

        /// <summary>
        /// Cria um arquivo por assessoria da lista de distribuicoes
        /// </summary>
        public static void Criar(Operacao operacao, List<DistribuicaoViewModel> lista)
        {
            var assessorias = lista.GroupBy(d => d.Assessoria).Select(x => x.Key).ToList();

            foreach (var assessoria in assessorias)
            {
                Criar(operacao, assessoria, lista);
            }
        }


        public static string Criar(Operacao operacao, AssessoriaViewModel assessoria, IEnumerable<DevedorViewModel> lista)
        {
            if (operacao == Operacao.Nenhuma) return "";

            if (lista == null) return "";

            var dataHoje = DateTime.Now;

            //Nomenclatura: AAAAMMDD_HHMM_remessa_NumAssessoria_out.txt (Exemplo: 20120522_0421_remessa_3001_out.txt)
            var nomeArquivo = string.Join("_", dataHoje.ToString("yyyyMMdd"), dataHoje.ToString("HHmm"), "remessa", assessoria.Id.ToString().PadLeft(4, '0'), "out.txt");

            var path = Path.Combine(PathArquivo.ReturnPath(Entidade.Uteis.TipoArquivo.Arquivo), nomeArquivo);

            if (File.Exists(path))
                File.Delete(path);

            arquivoStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

            var texto = new StringBuilder();

            texto.Append("0");//Header
            texto.Append(RetornaTextoArquivo(assessoria.Id.ToString(), 15, Direcao.Right));
            texto.Append(dataHoje.ToString("ddMMyyyy")).Append(dataHoje.ToString("HHmmss"));

            foreach (var devedor in lista)
            {
                texto.AppendLine();
                texto.Append("2");//Dados do cliente
                texto.Append(operacao.ToDescription());
                texto.Append(RetornaTextoArquivo(devedor.Id.ToString(), 10, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Nome, 50, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Cpf, 15, Direcao.Right));

                if(devedor.Pessoa.DataNascimento.HasValue)
                    texto.Append(RetornaTextoArquivo(devedor.Pessoa.DataNascimento.Value.ToString("ddMMyyyy"), 8, Direcao.Right));

                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Contatos?.FirstOrDefault()?.Telefone ?? string.Empty, 12, Direcao.Right, '0'));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Contatos?.FirstOrDefault()?.Celular ?? string.Empty, 12, Direcao.Right, '0'));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Logradouro ?? string.Empty, 72, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Complemento ?? string.Empty, 40, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Numero ?? string.Empty, 10, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Resumo ?? string.Empty, 100, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Cep ?? string.Empty, 8, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Bairro ?? string.Empty, 30, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.CidadeDescricao ?? string.Empty, 30, Direcao.Right));
                texto.Append(RetornaTextoArquivo(devedor.Pessoa.Enderecos?.FirstOrDefault()?.Estado ?? string.Empty, 2, Direcao.Right));

                var contratos = devedor?.Contratos ?? new List<DevedorContratoViewModel>();
                foreach (var contrato in contratos)
                {
                    texto.AppendLine();
                    texto.Append("3"); //Contrato
                    texto.Append(operacao.ToDescription());
                    texto.Append(RetornaTextoArquivo(devedor.Id.ToString(), 25, Direcao.Right));
                    texto.Append(RetornaTextoArquivo("00", 2, Direcao.Right));
                    texto.Append(RetornaTextoArquivo(contrato.Contrato.DataVencimento.ToString("ddMMyyyy"), 8, Direcao.Right));
                    texto.Append(RetornaTextoArquivo(contrato.Contrato.ValorPago.ToString(), 12, Direcao.Left, '0'));
                    texto.Append(RetornaTextoArquivo("0", 1, Direcao.Right, '0'));
                    texto.Append(RetornaTextoArquivo(contrato.Contrato.Parcelas.Count.ToString(), 3, Direcao.Left, '0'));
                    texto.Append(RetornaTextoArquivo(contrato.Contrato.ValorContrato.ToString(), 12, Direcao.Left, '0'));
                    texto.Append(RetornaTextoArquivo(contrato.Contrato.DataPagamento.ToString().Replace("/", ""), 8, Direcao.Right));

                    var parcelas = contrato.Contrato.Parcelas ?? new List<ParcelaViewModel>();
                    foreach (var parcela in parcelas)
                    {
                        texto.AppendLine();
                        texto.Append("4"); //Parcelas
                        texto.Append(operacao.ToDescription());
                        texto.Append(RetornaTextoArquivo(parcela.Id.ToString(), 25, Direcao.Right));
                        texto.Append(RetornaTextoArquivo(parcela.NumParcela.ToString(), 3, Direcao.Left, '0'));
                        texto.Append(RetornaTextoArquivo(parcela.DataVencimento.ToString("ddMMyyyy"), 8, Direcao.Right));
                        texto.Append(RetornaTextoArquivo(parcela.ValorParcela.ToString(), 12, Direcao.Left, '0'));
                    }

                    var produtos = contrato.Contrato.Bens ?? new List<BemViewModel>();
                    foreach (var produto in produtos)
                    {
                        texto.AppendLine();
                        texto.Append("6"); //Produtos contrato
                        texto.Append(operacao.ToDescription());
                        texto.Append(RetornaTextoArquivo(produto.Id.ToString(), 25, Direcao.Right));
                        texto.Append(RetornaTextoArquivo(produto.Descricao, 50, Direcao.Right));
                        texto.Append(RetornaTextoArquivo("01", 4, Direcao.Right));
                    }
                }
            }
            texto.AppendLine();
            texto.Append("9"); //Trailler
            texto.Append(dataHoje.ToString("ddMMyyyy")).Append(dataHoje.ToString("HHmmss"));
            texto.Append(RetornaTextoArquivo(texto.Length.ToString(), 8, Direcao.Left, '0'));
            var info = new UTF8Encoding(true).GetBytes(texto.ToString());

            arquivoStream.Write(info, 0, info.Length);

            return path;
        }

        public static FileInfo CriarSpc(OperacaoSpc operacao, List<ContratoViewModel> lista, ParametroFaturamentoViewModel parametro, string nomeArquivo)
        {
            if (lista == null) return null;

            var dataHoje = DateTime.Now;

            var path = Path.Combine(PathArquivo.ReturnPath(Entidade.Uteis.TipoArquivo.Arquivo), nomeArquivo);

            var arquivoInfo = new FileInfo(path);//new FileStream(path, FileMode.Create, FileAccess.Write);

            var texto = new StringBuilder();

            int linha = 1;

            texto.Append(RetornaTextoArquivo("00REMESSA", 9, Direcao.Right));//Header
            texto.Append(RetornaTextoArquivo(dataHoje.ToString("ddMMyyyy"), 8, Direcao.Right));
            texto.Append(RetornaTextoArquivo("1", 8, Direcao.Left, '0'));
            texto.Append(RetornaTextoArquivo("15000", 5, Direcao.Left, '0'));
            texto.Append(RetornaTextoArquivo("1", 8, Direcao.Left, '0'));
            texto.Append(RetornaTextoArquivo(dataHoje.ToString("yyyyMMdd"), 8, Direcao.Right));
            texto.Append(RetornaTextoArquivo("", 321, Direcao.Right));
            texto.Append(RetornaTextoArquivo("SPC", 5, Direcao.Right));
            texto.Append(RetornaTextoArquivo("12", 12, Direcao.Right));
            texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

            for (int i = 0; i < lista.Count; i++)
            {
                var contrato = lista[i];
                linha = linha + 1;
                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("01", 2, Direcao.Right));//Consumidor
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf != null ? "2" : "1", 1, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf, 15, Direcao.Right));
                texto.Append(RetornaTextoArquivo(operacao.ToDescription(), 1, Direcao.Right));
                texto.Append(RetornaTextoArquivo("C", 1, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.DataVencimento.ToString("ddMMyyyy") ?? "", 8, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.DataVencimento.ToString("ddMMyyyy") ?? "", 8, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.ValorContrato.ToString("C").Replace(",", "").Replace(".", ""), 13, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo(contrato.CodContrato ?? "", 30, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Carteira.CodigoAssociado.ToString(), 8, Direcao.Right));
                texto.Append(RetornaTextoArquivo("", 2, Direcao.Right));
                texto.Append(RetornaTextoArquivo("1", 3, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf != null ? "2" : "1", 1, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf, 15, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo("N", 3, Direcao.Right));
                texto.Append(RetornaTextoArquivo("", 265, Direcao.Right));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("02", 2, Direcao.Right));//SPC
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Cep ?? "", 8, Direcao.Right, '0'));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Nome ?? "", 45, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf != null ? "2" : "1", 1, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Cpf ?? "", 15, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Rg ?? "", 20, Direcao.Right));

                if(contrato.Devedor.Pessoa.DataNascimento.HasValue)
                    texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.DataNascimento.Value.ToString("ddMMyyyy"), 8, Direcao.Right));

                texto.Append(RetornaTextoArquivo("", 45, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Logradouro ?? "", 50, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Numero ?? "", 5, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Complemento ?? "", 30, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Cep ?? "", 8, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.CidadeDescricao ?? "", 30, Direcao.Right));
                texto.Append(RetornaTextoArquivo(contrato.Devedor.Pessoa.Enderecos.FirstOrDefault()?.Estado ?? "", 2, Direcao.Right));
                texto.Append(RetornaTextoArquivo("", 90, Direcao.Right));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("10", 2, Direcao.Right));//Boleto linha 1
                texto.Append(RetornaTextoArquivo(parametro.Banco.CodigoBanco, 4, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo(parametro.Banco.Descricao, 30, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("10490904089099020004135363874104172970000201304", 60, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("10491729700002013040904090990200043536387410", 44, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("PAGÁVEL PREFERENCIALMENTE NAS CASAS LOTÉRICAS ATÉ O VALOR LIMITE", 84, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(contrato.Parcelas.Count().ToString(), 7, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(contrato.DataVencimento.ToShortDateString(), 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("HOEPERS RECUPERADORA DE CRÉDITOS SA", 84, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 49, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                var bolt = new BoletoNet.Boleto();


                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("11", 2, Direcao.Right));//Boleto linha 2
                texto.Append(RetornaTextoArquivo("1851 090409/0", 19, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(DateTime.Now.ToShortDateString(), 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("9900035363874", 13, Direcao.Right, ' '));//NumeroDocumento
                texto.Append(RetornaTextoArquivo("R$", 4, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("N", 3, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(DateTime.Now.ToShortDateString(), 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 25, Direcao.Right, ' '));//NossoNumero
                texto.Append(RetornaTextoArquivo("", 4, Direcao.Right, ' '));//Uso do Banco
                texto.Append(RetornaTextoArquivo(parametro.Carteira, 8, Direcao.Right, ' '));//Carteira
                texto.Append(RetornaTextoArquivo("R$", 10, Direcao.Right, ' '));//Moeda
                texto.Append(RetornaTextoArquivo("", 5, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(contrato.ValorContrato.ToString(), 14, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo(contrato.ValorParcela.ToString(), 14, Direcao.Left, '0'));
                texto.Append(RetornaTextoArquivo("REF CONTR 6043234000046145 - PREST (S)  1 a 1/1 | À VISTA", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 37, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("12", 2, Direcao.Right));//Boleto linha 3
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 78, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("13", 2, Direcao.Right));//Boleto linha 4
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 78, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

                linha = linha + 1;

                texto.AppendLine();
                texto.Append(RetornaTextoArquivo("14", 2, Direcao.Right));//Boleto linha 5
                texto.Append(RetornaTextoArquivo("", 98, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 274, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo("", 10, Direcao.Right, ' '));
                texto.Append(RetornaTextoArquivo(linha.ToString(), 6, Direcao.Left, '0'));

            }
            texto.AppendLine();
            texto.Append(RetornaTextoArquivo("99", 2, Direcao.Right)); //Trailler
            texto.Append(RetornaTextoArquivo(lista.Count.ToString(), 6, Direcao.Left, '0'));
            texto.Append(RetornaTextoArquivo("", 366, Direcao.Right));
            texto.Append(RetornaTextoArquivo("", 10, Direcao.Right));
            texto.Append(RetornaTextoArquivo(lista.Count.ToString(), 6, Direcao.Left, '0'));

            //var info = new UTF8Encoding(true).GetBytes(texto.ToString());

            using (StreamWriter writer = arquivoInfo.CreateText())
            {
                writer.Write(texto);
            }

            return arquivoInfo;
        }

        private static string RetornaTextoArquivo(string texto, int tamanho, Direcao direcao, char separador = ' ')
        {
            string retorno;
            var tamanhoTotal = tamanho - texto.Length;

            if (tamanhoTotal > 0)
            {
                retorno = direcao == Direcao.Right ? texto.PadRight(tamanhoTotal, separador) : texto.PadLeft(tamanhoTotal, separador);
            }
            else
            {
                retorno = texto.Substring(tamanhoTotal * -1);
            }

            return retorno;
        }

        public static byte[] ArquivoBancoBoletagem(List<Entidade.Promessa> listaPromessas, Entidade.ParametroFaturamento parametro)
        {
            var boletos = new Boletos();
            var cedente = new Cedente();
            var aqvRemessa = new BoletoNet.ArquivoRemessa(BoletoNet.TipoArquivo.CNAB400);
            var banco = new Banco(Convert.ToInt32(parametro.Banco.CodigoBanco));

            cedente.Carteira = parametro?.Carteira;
            cedente.ContaBancaria = new ContaBancaria(parametro?.Agencia, parametro?.DigitoAgencia, parametro?.Conta, parametro?.DigitoConta);
            cedente.Convenio = Convert.ToInt64(parametro?.Convenio ?? "0");
            cedente.CPFCNPJ = parametro?.Empresa?.DocumentoCnpj ?? string.Empty;
            cedente.Nome = parametro?.Empresa?.RazaoSocial ?? string.Empty;

            foreach (var item in listaPromessas)
            {
                var primeiraParcelaBoletar = item.Parcelas.FirstOrDefault(x => x.DataVencimento > DateTime.Now);
                var devedor = item.Contratos.FirstOrDefault()?.Contrato?.Devedor;
                var endereco = devedor?.Pessoa?.Enderecos?.LastOrDefault()?.Endereco ?? new Entidade.Endereco();
                var sacado = new Sacado(devedor?.DocumentoCpf, devedor?.Pessoa?.Nome);
                var boleto = new Boleto(item.Parcelas.FirstOrDefault(x => x.DataVencimento > DateTime.Now).DataVencimento,
                    primeiraParcelaBoletar.Valor,
                    parametro?.Carteira,
                    "11" + primeiraParcelaBoletar.Id.ToString().PadLeft(15, '0'),
                    cedente);
                boleto.NumeroDocumento = boleto.NossoNumero;
                boleto.DataDocumento = DateTime.Now;
                boleto.Sacado = sacado;

                primeiraParcelaBoletar.NossoNumero = boleto.NossoNumero;
                primeiraParcelaBoletar.NumeroDocumento = boleto.NumeroDocumento;

                if (boleto.Sacado.Endereco == null)
                    boleto.Sacado.Endereco = new Endereco();
                boleto.Sacado.Endereco.End = endereco?.Logradouro;
                boleto.Sacado.Endereco.Bairro = endereco?.Bairro;
                boleto.Sacado.Endereco.Cidade = endereco?.Cidade?.Descricao;
                boleto.Sacado.Endereco.CEP = endereco?.Cep;
                boleto.Sacado.Endereco.UF = endereco?.Cidade?.Estado?.Sigla;
                boleto.Sacado.Endereco.Email = devedor?.ContatoEmail;

                boleto.NumeroParcela = item.Parcelas.ToList().FindIndex(x => x.DataVencimento > DateTime.Now) + 1;

                var instrucao = new Instrucao_Caixa();
                instrucao.Descricao = "Não Receber Após o Vencimento";

                boleto.Instrucoes.Add(instrucao);
                var especie = new EspecieDocumento_Caixa("11");
                boleto.EspecieDocumento = especie;

                boleto.Remessa = new Remessa();
                boleto.Remessa.TipoDocumento = "2";

                boletos.Add(boleto);

            }
            var mem = new MemoryStream();
            aqvRemessa.GerarArquivoRemessa(parametro.Convenio, banco, cedente, boletos, mem, Convert.ToInt32(parametro.CodigoTransmissao));

            byte[] bytesInStream = mem.ToArray();
            mem.Close();

            return bytesInStream;
        }

        public static void Dispose()
        {
            arquivoStream.Dispose();
        }

        public enum Operacao
        {
            [Description("N")]
            Nenhuma,
            [Description("I")]
            Inclusao,
            [Description("A")]
            Atualizacao,
            [Description("R")]
            Retirada
        }

        public enum OperacaoSpc
        {
            [Description("N")]
            Nenhuma,
            [Description("I")]
            Inclusao,
            [Description("E")]
            Exclusao
        }
    }
}