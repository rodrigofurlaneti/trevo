using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using Entidade.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.ViewModels
{
    public class ContatoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public TipoContato Tipo { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string NomeRecado { get; set; }
        public int Ordem { get; set; }
        
        //Mapeamento para Importacao Apenas
        public string DDD {get; set;}
        public string Numero { get; set; }

        public ContatoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ContatoViewModel(Contato contato)
        {
            Id = contato?.Id ?? 0;
            Email = contato?.Email;
            Celular = contato?.Tipo == TipoContato.Celular || contato?.Tipo == TipoContato.Recado ? contato.Numero : "";
            Telefone = contato?.Tipo == TipoContato.Residencial || contato?.Tipo == TipoContato.Comercial || contato?.Tipo == TipoContato.Fax ? contato.Numero : "";
            Tipo = contato?.Tipo ?? 0;
            Numero = contato?.Numero ?? string.Empty;
            DataInsercao = contato?.DataInsercao ?? DateTime.Now;
        }

        public static List<ContatoViewModel> ContatoViewModelList(IList<Contato> contatos)
        {
            var contatosVm = new List<ContatoViewModel>();
            if (contatos == null || contatos.Count <= 0) return contatosVm;

            contatosVm.AddRange(contatos.Select(contato => new ContatoViewModel(contato)));
            return contatosVm;
        }

        public Contato ToEntity() => new Contato
        {
            Id = Id,
            DataInsercao = DateTime.Now,
            Numero = this.RetornarNumero(),
            Tipo = this.Tipo,
            Email = this.Email,
            Celular = this.Celular
        };
        
        //ajuste Marco. Ele verifica o campo Numero. Porem, a informacao vem nos campos 'Celular' ou 'Telefone'
        private string RetornarNumero()
        {
            if (this.Tipo == TipoContato.Residencial || this.Tipo == TipoContato.Celular || this.Tipo == TipoContato.Comercial || this.Tipo == TipoContato.Recado || this.Tipo == TipoContato.Fax)
            {
                var campoContato = RetornaCampoComValor();
                if (!string.IsNullOrEmpty(this.DDD) || !string.IsNullOrEmpty(campoContato))
                    return string.Concat(this.DDD, " ", campoContato).Trim();
            }

            return null;
        }

        public string RetornaCampoComValor()
        {
            if (!string.IsNullOrEmpty(this.Celular))
                return this.Celular;

            if (!string.IsNullOrEmpty(this.Telefone))
                return this.Telefone;

            if (!string.IsNullOrEmpty(this.Numero))
                return this.Numero;

            return "";

        }
    }
}