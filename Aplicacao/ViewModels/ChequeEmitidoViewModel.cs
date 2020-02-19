using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ChequeEmitidoViewModel
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
        public DateTime? DataEmissao { get; set; }
        public string CartorioProtestado { get; set; }
        public BancoViewModel Banco { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }

        public virtual IList<ContasAPagarViewModel> ListaContaPagar { get; set; }


        public ChequeEmitidoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ChequeEmitidoViewModel(ChequeEmitido entity)
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
                DataEmissao = entity.DataEmissao;
                CartorioProtestado = entity.CartorioProtestado;
                Banco = new BancoViewModel(entity.Banco);
                Fornecedor = new FornecedorViewModel(entity.Fornecedor);
                DataInsercao = entity.DataInsercao;
                ListaContaPagar = entity.ListaContaPagar.Select(x => new ContasAPagarViewModel(x.ContaPagar)).ToList();
            }
        }

        public ChequeEmitido ToEntity()
        {
            return new ChequeEmitido
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
                DataEmissao = DataEmissao,
                CartorioProtestado = CartorioProtestado,
                Banco = Banco.ToEntity(),
                Fornecedor = new Fornecedor { Id = Fornecedor.Id},
                DataInsercao = DataInsercao,
                ListaContaPagar = ListaContaPagar.Select(x => new ChequeEmitidoContaPagar { ContaPagar = x.ToEntity() }).ToList()
            };
        }
    }
}
