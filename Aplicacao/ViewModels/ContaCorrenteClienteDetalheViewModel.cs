using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class ContaCorrenteClienteDetalheViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataCompetencia { get; set; }
        public string DataCompetenciaStr
        {
            get
            {
                return DataCompetencia.ToShortDateString();
            }
        }
        public TipoOperacaoContaCorrente TipoOperacaoContaCorrente { get; set; }
        public string Valor { get; set; }

        public ContratoMensalistaViewModel ContratoMensalista { get; set; }

        public ContaCorrenteClienteDetalheViewModel()
        {
            ContratoMensalista = new ContratoMensalistaViewModel();
        }

        public ContaCorrenteClienteDetalheViewModel(TipoOperacaoContaCorrente tipoOperacao, DateTime dataCompetencia, string valor, int contratoId, int numeroContrato)
        {
            DataInsercao = DateTime.Now;
            DataCompetencia = dataCompetencia;
            TipoOperacaoContaCorrente = tipoOperacao;
            Valor = valor;
            ContratoMensalista = new ContratoMensalistaViewModel() { Id = contratoId, NumeroContrato = numeroContrato };
        }

        public ContaCorrenteClienteDetalheViewModel(ContaCorrenteClienteDetalhe obj)
        {
            Id = obj.Id;
            DataInsercao = obj.DataInsercao;
            DataCompetencia = obj.DataCompetencia;
            TipoOperacaoContaCorrente = obj.TipoOperacaoContaCorrente;
            Valor = obj.Valor.ToString("N2");
            ContratoMensalista = obj.ContratoMensalista != null ? new ContratoMensalistaViewModel() { Id = obj.ContratoMensalista.Id, NumeroContrato = obj.ContratoMensalista.NumeroContrato } : new ContratoMensalistaViewModel();
        }

        public ContaCorrenteClienteDetalhe ToEntity()
        {
            var obj = new ContaCorrenteClienteDetalhe
            {
                Id = Id,
                DataInsercao = DataInsercao,
                DataCompetencia = DataCompetencia,
                TipoOperacaoContaCorrente = TipoOperacaoContaCorrente,
                Valor = Convert.ToDecimal(Valor),
                ContratoMensalista = ContratoMensalista.ToEntity()
            };
            return obj;
        }       
    }
}
