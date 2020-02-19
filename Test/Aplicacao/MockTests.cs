using Aplicacao.ViewModels;
using Entidade;
using System;
using System.Collections.Generic;

namespace Test.Aplicacao
{
    public class MockTests
    {
        public static EstacionamentoSoftparkViewModel Estacionamento(int id)
        {
            return new EstacionamentoSoftparkViewModel
            {
                Nome = "Frios Mogiano",
                Endereco = "Rua Rosa Pardana, 23  - 69309-514  - Boa Vista",
                Complemento = null,
                Cep = "69309514",
                Telefone = "",
                Dizeres = "",
                Horario = "06:00 - 19:00",
                Codigo = id.ToString(),
                RazaoSocial = "Frios Mogiano",
                InAtivo = true,
                DataInsercao = DateTime.Now,
                UserName = "",
                DiaPgtoMensal = 0,
                JurosMulta = 0,
                JurosMora = 0,
                Ccm = "null",
                Cnpj = "18.214.802/0001-60",
                CaminhoLogoTipo = "",
                CaminhoLogoEmpresa = "",
                ToleranciaMulta = 0,
                Id = id
            };
        }

        public static TabelaPrecoMensalSoftparkViewModel TabelaPrecoMensal(int tabelaPrecoMensalId)
        {
            var tabelaPrecoMensal = new TabelaPrecoMensalSoftparkViewModel();
            tabelaPrecoMensal.Id = tabelaPrecoMensalId;

            var estacionamentos = new List<TabelaPrecoMensalEstacionamentoSoftparkViewModel>();
            for (int i = 1; i <= 3; i++)
            {
                var id = 999 * i;
                var estacionamento = Estacionamento(id);
                estacionamentos.Add(TabelaPrecoMensalEstacionamento(id, tabelaPrecoMensal, estacionamento));
            }

            tabelaPrecoMensal.Periodo = "string";
            tabelaPrecoMensal.HoraInicial = 5;
            tabelaPrecoMensal.HoraFinal = 10;
            tabelaPrecoMensal.MinutoInicial = 20;
            tabelaPrecoMensal.MinutoFinal = 30;
            tabelaPrecoMensal.InSegunda = 0;
            tabelaPrecoMensal.InTerca = 0;
            tabelaPrecoMensal.InQuarta = 0;
            tabelaPrecoMensal.InQuinta = 0;
            tabelaPrecoMensal.InSexta = 0;
            tabelaPrecoMensal.InSabado = 0;
            tabelaPrecoMensal.InDomingo = 0;
            tabelaPrecoMensal.InFeriado = 0;
            tabelaPrecoMensal.Valor = 50;
            tabelaPrecoMensal.DataInsercao = DateTime.Now;

            return tabelaPrecoMensal;
        }

        public static TabelaPrecoMensalEstacionamentoSoftparkViewModel TabelaPrecoMensalEstacionamento(int id, TabelaPrecoMensalSoftparkViewModel tabelaPrecoMensal, EstacionamentoSoftparkViewModel estacionamento)
        {
            return new TabelaPrecoMensalEstacionamentoSoftparkViewModel
            {
                Id = id,
                TabelaPrecoMensalId = tabelaPrecoMensal.Id,
                TabelaPrecoMensal = tabelaPrecoMensal,
                EstacionamentoId = estacionamento.Id,
                Estacionamento = estacionamento
            };
        }

        public static TabelaPrecoSoftparkViewModel TabelaPreco(int tabelaPrecoId)
        {
            var tabelaPreco = new TabelaPrecoSoftparkViewModel();
            tabelaPreco.Id = tabelaPrecoId;

            var estacionamentos = new List<TabelaPrecoEstacionamentoSoftparkViewModel>();
            for (int i = 1; i <= 3; i++)
            {
                var id = 999 * i;
                var estacionamento = Estacionamento(id);
                estacionamentos.Add(TabelaPrecoEstacionamento(id, tabelaPreco, estacionamento));
            }

            var items = new List<TabelaPrecoItemSoftparkViewModel>();
            for (int i = 1; i <= 3; i++)
            {
                var id = 999 * i;
                items.Add(TabelaPrecoItem(id, 1));
            }

            var configuracaoDiaria = new List<TabelaPrecoConfiguracaoDiariaSoftparkViewModel>();
            for (int i = 1; i <= 3; i++)
            {
                var id = 999 * i;
                configuracaoDiaria.Add(TabelaPrecoConfiguracaoDiaria(id, 1));
            }

            tabelaPreco.Numero = 1;
            tabelaPreco.Nome = "Nome da Tabela Preco";
            tabelaPreco.ToleranciaInicial = 10;
            tabelaPreco.Items = items;
            tabelaPreco.ConfiguracaoDiaria = configuracaoDiaria;
            tabelaPreco.DataInsercao = DateTime.Now;
            tabelaPreco.TabelaPrecoEstacionamento = estacionamentos;

            return tabelaPreco;
        }

        public static TabelaPrecoConfiguracaoDiariaSoftparkViewModel TabelaPrecoConfiguracaoDiaria(int id, int tabelaPrecoId)
        {
            return new TabelaPrecoConfiguracaoDiariaSoftparkViewModel
            {
                Id = id,
                HoraInicial = 10,
                HoraFinal = 12,
                TabelaPrecoId = tabelaPrecoId,
                DataInsercao = DateTime.Now,
            };
        }

        public static TabelaPrecoEstacionamentoSoftparkViewModel TabelaPrecoEstacionamento(int id, TabelaPrecoSoftparkViewModel tabelaPreco, EstacionamentoSoftparkViewModel estacionamento)
        {
            return new TabelaPrecoEstacionamentoSoftparkViewModel
            {
                Id = id,
                TabelaPrecoId = tabelaPreco.Id,
                TabelaPreco = tabelaPreco,
                EstacionamentoId = estacionamento.Id,
                Estacionamento = estacionamento,
                DataInsercao = DateTime.Now
            };
        }

        public static TabelaPrecoItemSoftparkViewModel TabelaPrecoItem(int id, int tabelaPrecoId)
        {
            return new TabelaPrecoItemSoftparkViewModel
            {
                Id = id,
                Hora = 10,
                Minuto = 20,
                Valor = 30,
                TabelaPrecoId = tabelaPrecoId,
                DataInsercao = DateTime.Now,
            };
        }

        public static OperadorSoftparkViewModel Operador(int unidadeId)
        {
            return new OperadorSoftparkViewModel()
            {
                Login = "string",
                Nome = "string",
                Senha = "string",
                Ativo = true,
                AlterSenha = 1,
                AbrirCaixa = true,
                Matricula = "string",
                Estacionamento = Estacionamento(unidadeId),
                OperadorPerfil = "Administrador",
                Id = 10,
                DataInsercao = DateTime.Now
            };
        }

        public static PagamentoMensalidadeSoftparkViewModel PagamentoMensalidade(int unidadeId)
        {
            return new PagamentoMensalidadeSoftparkViewModel
            {
                Id = 1,
                DataInsercao = DateTime.Now
            };
        }
    }
}
