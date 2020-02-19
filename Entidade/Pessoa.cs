using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Pessoa : BaseEntity, IAudit
    {
        [Required]
        public virtual string Nome { get; set; }
        public virtual string Sexo { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual Trabalho Trabalho { get; set; }
        public virtual IList<PessoaEndereco> Enderecos { get; set; }
        public virtual IList<PessoaDocumento> Documentos { get; set; }
        public virtual IList<PessoaContato> Contatos { get; set; }
        public virtual IList<PessoaLoja> Lojas { get; set; }
        public virtual bool Ativo { get; set; }

        public Pessoa()
        {
            Documentos = new List<PessoaDocumento>();
            Enderecos = new List<PessoaEndereco>();
            Contatos = new List<PessoaContato>();
            DataNascimento = DateTime.Now.AddYears(-100);
            Trabalho = new Trabalho();
        }

        //Não mapear campos abaixo
        public virtual string DocumentoCnpjOuCpf
        {
            get
            {
                var cnpj = GetDocumento(TipoDocumento.Cnpj);
                if (!string.IsNullOrEmpty(cnpj))
                    return cnpj;
                return DocumentoCpf;
            }
        }

        public virtual string DocumentoCnh
        {
            get { return GetDocumento(TipoDocumento.Cnh); }
            set { SetDocumento(TipoDocumento.Cnh, value); }
        }

        public virtual string DocumentoCpf
        {
            get { return GetDocumento(TipoDocumento.Cpf); }
            set { SetDocumento(TipoDocumento.Cpf, value); }
        }

        public virtual string DocumentoCnpj
        {
            get { return GetDocumento(TipoDocumento.Cnpj); }
            set { SetDocumento(TipoDocumento.Cnpj, value); }
        }

        public virtual string ContatoCelular
        {
            get { return GetContato(TipoContato.Celular); }
            set { SetContato(TipoContato.Celular, value); }
        }

        public virtual string ContatoTelefone
        {
            get { return GetContato(TipoContato.Residencial); }
            set { SetContato(TipoContato.Residencial, value); }
        }

        public virtual string ContatoEmail
        {
            get { return GetContato(TipoContato.Email); }
            set { SetContato(TipoContato.Email, value); }
        }

        public virtual string DocumentoRg
        {
            get { return GetDocumento(TipoDocumento.Rg); }
            set { SetDocumento(TipoDocumento.Rg, value); }
        }

        public virtual string DocumentoIe
        {
            get { return GetDocumento(TipoDocumento.Ie); }
            set { SetDocumento(TipoDocumento.Ie, value); }
        }

        public virtual string DocumentoIm
        {
            get { return GetDocumento(TipoDocumento.Im); }
            set { SetDocumento(TipoDocumento.Im, value); }
        }

        private string GetContato(TipoContato tipo)
        {
            var contato = Contatos?.FirstOrDefault(x => x.Contato.Tipo == tipo)?.Contato;
            if (contato != null)
            {

                if (tipo == TipoContato.Email || tipo == TipoContato.OutroEmail)
                    return contato.Email;
                else if (tipo == TipoContato.Recado)
                    return contato.NomeRecado;
                else
                    return contato.Numero;
            }

            return string.Empty;
        }

        private string GetDocumento(TipoDocumento tipo) => Documentos?.FirstOrDefault(x => x.Documento.Tipo == tipo)?.Documento?.Numero;

        private void SetContato(TipoContato tipo, dynamic value)
        {
            if ((Contatos == null || !Contatos.Any()) && value != null)
            {
                switch (tipo)
                {
                    case TipoContato.Recado:
                        Contatos = new List<PessoaContato> { new PessoaContato { Contato = new Contato { Tipo = tipo, NomeRecado = value } } };
                        break;
                    case TipoContato.Residencial:
                    case TipoContato.Celular:

                    case TipoContato.Comercial:
                    case TipoContato.Fax:
                        Contatos = new List<PessoaContato> { new PessoaContato { Contato = new Contato { Tipo = tipo, Numero = value } } };
                        break;
                    case TipoContato.Email:
                    case TipoContato.OutroEmail:
                        Contatos = new List<PessoaContato> { new PessoaContato { Contato = new Contato { Tipo = tipo, Email = value } } };
                        break;
                    //case TipoContato.Padrao:
                    //    Contatos = new List<PessoaContato> { new PessoaContato { Contato = new Contato { Tipo = tipo, Email = value } } };
                    //    break;
                    default:
                        break;
                }
            }

            //if ((ContatoEmail == null && tipo == TipoContato.Email) && value != null)
            //    Contatos.Add(new PessoaContato { Contato = new Contato { Tipo = tipo, Email = value } });

            //if ((ContatoTelefone == null || ContatoCelular == null) && tipo != TipoContato.Email)
            //    Contatos.Add(new PessoaContato { Contato = new Contato { Tipo = tipo, Numero = value } });

            //if (Contatos.All(x => x.Contato.Tipo != tipo))
            //    return;

            var contato = Contatos != null && Contatos.Any() ? Contatos?.FirstOrDefault(x => x.Contato != null && x.Contato.Tipo == tipo)?.Contato : null;
            if (contato != null)
            {
                if (tipo == TipoContato.Email || tipo == TipoContato.OutroEmail)
                    contato.Email = value;
                else if (tipo == TipoContato.Recado)
                    contato.NomeRecado = value;
                else
                    contato.Numero = value;
            }
        }

        private void SetDocumento(TipoDocumento tipo, dynamic value)
        {
            if (value != null)
            {
                if (Documentos == null || !Documentos.Any())
                    Documentos = new List<PessoaDocumento>();

                if (!Documentos.Any(x => x.Documento.Tipo == tipo && x.Documento?.Numero == value))
                    Documentos.Add(new PessoaDocumento { Documento = new Documento(tipo, value, DateTime.Now) });

                if (Documentos.All(x => x.Documento.Tipo != tipo))
                    return;

                var pessoaDocumento = Documentos.FirstOrDefault(x => x.Documento.Tipo == tipo);
                var antigoDocumento = pessoaDocumento.Documento;
                var novoDocumento = Documento.NovoDocumento(antigoDocumento, value);
            }
        }
    }
}