using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Filial : PessoaJuridica
    {
        public virtual IList<FilialContato> Contatos { get; set; }

        public  virtual Empresa Empresa { get; set; }

        public virtual TipoFilial TipoFilial { get; set; }
    }
}
