using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class ChequeEmitidoContaPagarViewModel
    {
        public int Id { get; set; }
        public ContasAPagarViewModel ContaPagar { get; set; }
        public ChequeEmitidoViewModel ChequeEmitido { get; set; }

        public ChequeEmitidoContaPagarViewModel()
        {
            
        }

        public ChequeEmitidoContaPagarViewModel(ChequeEmitidoContaPagar entity)
        {
            if (entity != null)
            {
                Id = entity.Id;
                ContaPagar = new ContasAPagarViewModel(entity.ContaPagar);
                ChequeEmitido = new ChequeEmitidoViewModel(entity.ChequeEmitido);
            }
        }

        public ChequeEmitidoContaPagar ToEntity()
        {
            return new ChequeEmitidoContaPagar
            {
                Id = Id,
                ContaPagar = ContaPagar.ToEntity(),
                ChequeEmitido = ChequeEmitido.ToEntity()
            };
        }
    }
}
