using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ChequeRecebidoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public long Numero { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpf { get; set; }
        public string Emitente { get; set; }
        public decimal Valor { get; set; }
        public string ValorFormatado { get; set; }
        public DateTime? DataDeposito { get; set; }
        public DateTime? DataProtesto { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string CartorioProtestado { get; set; }
        public StatusCheque StatusCheque { get; set; }
        public BancoViewModel Banco { get; set; }
        public ClienteViewModel Cliente { get; set; }

        public virtual IList<LancamentoCobrancaViewModel> ListaLancamentoCobranca { get; set; }


        public ChequeRecebidoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ChequeRecebidoViewModel(ChequeRecebido entity)
        {
            if (entity != null)
            {
                Id = entity.Id;

                Numero = entity.Numero;
                Agencia = entity.Agencia;
                DigitoAgencia = entity.DigitoAgencia;
                Conta = entity.Conta;
                DigitoConta = entity.DigitoConta;
                Cpf = entity.Cpf;
                Emitente = entity.Emitente;
                Valor = entity.Valor;
                DataDeposito = entity.DataDeposito;
                DataProtesto = entity.DataProtesto;
                DataDevolucao = entity.DataDevolucao;
                CartorioProtestado = entity.CartorioProtestado;
                StatusCheque = entity.StatusCheque;
                Banco = new BancoViewModel(entity.Banco);
                Cliente = new ClienteViewModel(entity.Cliente);
                DataInsercao = entity.DataInsercao;
                ListaLancamentoCobranca = entity.ListaLancamentoCobranca.Select(x => new LancamentoCobrancaViewModel(x.LancamentoCobranca)).ToList();
            }
        }

        public ChequeRecebido ToEntity()
        {
            return new ChequeRecebido
            {
                Id = Id,

                Numero = Numero,
                Agencia = Agencia,
                DigitoAgencia = DigitoAgencia,
                Conta = Conta,
                DigitoConta = DigitoConta,
                Cpf = Cpf,
                Emitente = Emitente,
                Valor = Valor,
                DataDeposito = DataDeposito,
                DataProtesto = DataProtesto,
                DataDevolucao = DataDevolucao,
                CartorioProtestado = CartorioProtestado,
                StatusCheque = StatusCheque,
                Banco = Banco.ToEntity(),
                Cliente = new Cliente { Id = Cliente.Id},
                DataInsercao = DataInsercao,
                ListaLancamentoCobranca = ListaLancamentoCobranca.Select(x => new ChequeRecebidoLancamentoCobranca { LancamentoCobranca = x.ToEntity() }).ToList()
            };
        }
    }
}
