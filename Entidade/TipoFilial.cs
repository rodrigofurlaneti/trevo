using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TipoFilial : IEntity
    {
        public  virtual int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public  virtual  string Nome { get; set; }

        public virtual DateTime DataInsercao { get; set; }
    }
}
