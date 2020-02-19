using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class LayoutImportacaoViewModel
    {
        //Cliente
        public string CodigoDoCliente { get; set; }
        public string NomeDoCiente { get; set; }
        public string CPFDoCliente { get; set; }
        public string DataDeNascimento { get; set; }

        //Contato Cliente
        public string DDDResidencial { get; set; }
        public string FoneResidencial { get; set; }
        public string DDDCelular { get; set; }
        public string FoneCelular { get; set; }

        //Endereco Cliente
        public string TpLogradouroResidencia { get; set; }
        public string EnderecoResidencial { get; set; }
        public string ComplementoResidencial { get; set; }
        public string NumeroResidencial { get; set; }
        public string NrAptoResidencial { get; set; }
        public string ReferenciaEndereco { get; set; }
        public string CEPResidencial { get; set; }
        public string BairroResidencial { get; set; }
        public string CidadeResidencial { get; set; }
        public string UFResidencial { get; set; }

        //Endereco Trabalho Cliente
        public string TpLogradouroTrabalho { get; set; }
        public string EnderecoTrabalho { get; set; }
        public string ComplementoTrabalho { get; set; }
        public string NumeroTrabalho { get; set; }
        public string CEPTrabalho { get; set; }
        public string BairroTrabalho { get; set; }
        public string CidadeTrabalho { get; set; }
        public string UFTrabalho { get; set; }
        public string DDDTrabalho { get; set; }
        public string FoneTrabalho { get; set; }

        //Trabalho
        public string EmpresaDeTrabalho { get; set; }
        public string ProfissaoCliente { get; set; }
        public string CargoCliente { get; set; }

        //Pais
        public string NomeDoPaiDoCliente { get; set; }
        public string NomeDaMaeDoCliente { get; set; }
        public string NomeDaReferencia1doCliente { get; set; }
        public string TelefoneDaReferencia1DoCliente { get; set; }
        public string NomeDaReferencia2DoCliente { get; set; }
        public string TelefoneDaReferencia2DoCliente { get; set; }

        //Conjuge
        public string CodigoDoConjuge { get; set; }
        public string NomeDoConjuge { get; set; }
        public string CPFDoConjuge { get; set; }
        public string NomeEmpresaConjuge { get; set; }
        public string TpLogradouroDoConjuge { get; set; }
        public string EnderecoConjuge { get; set; }
        public string ComplementoConjuge { get; set; }
        public string NumeroConjuge { get; set; }
        public string CEPConjuge { get; set; }
        public string BairroConjuge { get; set; }
        public string CidadeConjuge { get; set; }
        public string UFConjuge { get; set; }
        public string DDDTrabalhoConjuge { get; set; }
        public string FoneTrabalhoConjuge { get; set; }
        public string CargoDoConjuge { get; set; }
        public string ProfissaoDoConjuge { get; set; }

        //Avalista
        public string CodigoDoAvalista { get; set; }
        public string NomeDoAvalista { get; set; }
        public string CPFDoAvalista { get; set; }
        public string EmpresaTrabalhoDoAvalista { get; set; }

        //Avalista Endereco
        public string TpLogradouroResidenciaDoAvalista { get; set; }
        public string EnderecoResidenciaDoAvalista { get; set; }
        public string ComplementoResidenciaDoAvalista { get; set; }
        public string NumeroResidenciaDoAvalista { get; set; }
        public string ReferenciaEnderecoDoAvalista { get; set; }
        public string UFResidenciaDoAvalista { get; set; }

        //Avalista Contato
        public string DDDResidenciaDoAvalista { get; set; }
        public string FoneResidenciaDoAvalista { get; set; }
        public string BairroResidenciaDoAvalista { get; set; }
        public string CidadeResidenciaDoAvalista { get; set; }

        //Avalista Endereco Trabalho
        public string TpLogradouroTrabalhoDoAvalista { get; set; }
        public string EnderecoTrabalhoDoAvalista { get; set; }
        public string ComplementoTrabalhoDoAvalista { get; set; }
        public string NumeroTrabalhoDoAvalista { get; set; }
        public string CEPTrabalhoDoAvalista { get; set; }
        public string BairroTrabalhoDoAvalista { get; set; }
        public string CidadeTrabalhoDoAvalista { get; set; }
        public string UFTrabalhoDoAvalista { get; set; }

        //Avalista Contato Trabalho
        public string DDDTrabalhoDoAvalista { get; set; }
        public string FoneTrabalhoDoAvalista { get; set; }
        public string NumeroDoTituloDoCliente { get; set; }
        public string QuantidadeDeRenegociações { get; set; }
        public string DataDaCompra { get; set; }
        public string ValorDaEntrada { get; set; }
        public string ConstNoSPC { get; set; }
        public string ParcelaDoContrato { get; set; }
        public string ValorContrato { get; set; }
        public string DataDoUltimoPagamento { get; set; }
        public string LojaDeCompra { get; set; }
        public string NomeDoAgente { get; set; }
        public string EspecieDoTitulo { get; set; }
        public string NumeroUltimaParcelaPaga { get; set; }
        public string SaldoDoContrato { get; set; }
        public string VlEntContratoReneg { get; set; }
        public string NumeroDoTituloDoCliente_1 { get; set; }
        public string NumeroDaParcela { get; set; }
        public string VencimentoDaParcela { get; set; }
        public string ValorDaParcela { get; set; }
        public string NumeroDoTituloDoCliente_2 { get; set; }
        public string DescricaoDProduto { get; set; }
        public string QuantidadeComprada { get; set; }

        public LayoutImportacaoViewModel()
        {

        }
    }
}