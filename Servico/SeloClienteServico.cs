using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public interface ISeloClienteServico : IBaseServico<SeloCliente>
    {
    }
    public class SeloClienteServico : BaseServico<SeloCliente, ISeloClienteRepositorio>, ISeloClienteServico
    {
        private readonly ISeloClienteRepositorio _seloClienteRepositorio;

        public SeloClienteServico(ISeloClienteRepositorio seloClienteRepositorio)
        {
            _seloClienteRepositorio = seloClienteRepositorio;
        }
    }
}
