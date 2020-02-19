using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BoletoNet;
using Core.Exceptions;
using Entidade;

namespace Portal.Helpers
{
    public static class ArquivoRetorno
    {
        public static KeyValuePair<LeituraCNAB, List<DetalheRetorno>> LeituraCNAB400(byte[] arquivo, ContaFinanceira contaFinanceira)
        {
            if (arquivo == null)
                throw new BusinessRuleException("Arquivo de Retorno Nulo/Vazio!");

            var fileStr = string.Empty;
            using (StreamReader reader = new StreamReader(new MemoryStream(arquivo), Encoding.UTF8))
            {
                fileStr = reader.ReadToEnd();
            }
            if (string.IsNullOrEmpty(fileStr))
                throw new BusinessRuleException("Arquivo de Retorno Vazio!");

            var banco = new BoletoNet.Banco(Convert.ToInt32(contaFinanceira.Banco.CodigoBanco));
            var retornoCNAB400 = new ArquivoRetornoCNAB400();
            using (StreamReader reader = new StreamReader(new MemoryStream(arquivo), Encoding.UTF8))
            {
                retornoCNAB400.LerArquivoRetorno(banco, reader.BaseStream);
            }

            if (retornoCNAB400.ListaDetalhe == null || !retornoCNAB400.ListaDetalhe.Any())
                throw new BusinessRuleException("Detalhes não identificados no arquivo");

            var leitura = new LeituraCNAB
            {
                DataInsercao = DateTime.Now
            };
            return RetornaDadosLeituraCNAB(leitura, retornoCNAB400);
        }

        private static KeyValuePair<LeituraCNAB, List<DetalheRetorno>> RetornaDadosLeituraCNAB(LeituraCNAB leitura, ArquivoRetornoCNAB400 retornoCNAB400)
        {
            leitura.CodigoBanco = retornoCNAB400?.HeaderRetorno?.CodigoBanco.ToString();
            leitura.Agencia = retornoCNAB400?.HeaderRetorno?.Agencia.ToString();
            leitura.Conta = retornoCNAB400?.HeaderRetorno?.Conta.ToString();
            leitura.DACConta = retornoCNAB400?.HeaderRetorno?.DACConta.ToString();
            leitura.DataGeracao = retornoCNAB400?.HeaderRetorno?.DataGeracao ?? DateTime.Now;
            leitura.DataCredito = retornoCNAB400?.HeaderRetorno?.DataCredito ?? DateTime.Now;
            leitura.NumeroCNAB = retornoCNAB400?.HeaderRetorno?.NumeroSequencialArquivoRetorno.ToString();
            leitura.ValorTotal = retornoCNAB400?.ListaDetalhe?.Sum(x => x.ValorPago) ?? 0;

            return new KeyValuePair<LeituraCNAB, List<DetalheRetorno>>(leitura, retornoCNAB400.ListaDetalhe);
        }
    }
}