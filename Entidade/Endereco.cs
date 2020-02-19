using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Core.Exceptions;
using Entidade.Base;

namespace Entidade
{
    public class Endereco : BaseEntity
    {
        [Required, StringLength(10)]
        public virtual string Cep { get; set; }
     
        [StringLength(200)]
        public virtual string Logradouro { get; set; }
        
        [StringLength(10)]
        public virtual string Numero { get; set; }
        
        [StringLength(50)]
        public virtual string Complemento { get; set; }
        
        [StringLength(100)]
        public virtual string Bairro { get; set; }
        
        [StringLength(100)]
        public virtual string Descricao { get; set; }
        
        public virtual string Latitude { get; set; }
        
        public virtual string Longitude { get; set; }
        
        [NotExportField]
        public virtual string Tipo { get; set; }
        
        [NotExportField]
        public virtual Cidade Cidade { get; set; }
        
        [NotExportField]
        public virtual string Resumo => $"{Logradouro}, {Numero} {(!string.IsNullOrWhiteSpace(Complemento) ? " - " + Complemento : string.Empty)} - {Cep} {(Cidade != null && !string.IsNullOrWhiteSpace(Cidade.Descricao) ? " - " + Cidade.Descricao : string.Empty)}";
        public virtual string Referencia {get; set;}


        //Nao Mapear
        public virtual string CidadeNome {get; set;}
        public virtual string UF {get; set;} 

        public virtual bool Valido
        {
            get
            {
                if (Cidade == null)
                    return false;
                if (string.IsNullOrEmpty(Bairro))
                    return false;

                return true;
            }
        }
    }
}