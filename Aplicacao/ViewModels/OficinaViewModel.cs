using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class OficinaViewModel
    {
        private bool PessoaFisica => TipoPessoa.Fisica == TipoPessoa;
        private bool PessoaJuridica => TipoPessoa.Juridica == TipoPessoa;

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public PessoaViewModel Pessoa { get; set; }
        public string DocumentoCpf => Pessoa?.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string NomeFantasia { get; set; }
        public string Nome { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public TipoPessoa TipoPessoa { get; set; }

        public bool IndicadaPeloCliente { get; set; }
        public string NomeCliente { get; set; }

        public string CelularClienteDescricao { get; set; }

        public string ContatosOficina {
            get {
                var contatosOficina = new List<string>();

                if (Pessoa == null || Pessoa.Contatos == null)
                    return string.Empty;

                var telefone = Pessoa.Contatos.FirstOrDefault(x => x.Tipo == TipoContato.Residencial);
                if (telefone != null)
                    contatosOficina.Add(telefone.Telefone);

                var celular = Pessoa.Contatos.FirstOrDefault(x => x.Tipo == TipoContato.Celular);
                if (celular != null)
                    contatosOficina.Add(celular.Celular);

                return string.Join(" / ", contatosOficina);
            }
        }

        public ContatoViewModel CelularOficina
        {
            get
            {
                if(!string.IsNullOrEmpty(CelularClienteDescricao))
                    return new ContatoViewModel { Tipo = TipoContato.Celular, Numero = CelularClienteDescricao };

                return null;
            }
        }

        public string Descricao
        {
            get
            {
                if (PessoaFisica)
                    return Pessoa?.Nome ?? string.Empty;
                else if (PessoaJuridica)
                    return NomeFantasia ?? string.Empty;

                return string.Empty;
            }
        }

        public string DescricaoDocumento
        {
            get
            {
                if (PessoaFisica)
                    return Pessoa?.Cpf ?? string.Empty;
                else if (PessoaJuridica)
                    return Cnpj ?? string.Empty;

                return string.Empty;
            }
        }

        public OficinaViewModel()
        {
            Pessoa = new PessoaViewModel();
        }

        public OficinaViewModel(Oficina oficina)
        {
            if (oficina != null)
            {
                Id = oficina.Id;
                DataInsercao = oficina.Pessoa.DataInsercao;
                Pessoa = new PessoaViewModel(oficina?.Pessoa ?? new Pessoa());
                NomeFantasia = oficina?.NomeFantasia;
                IndicadaPeloCliente = oficina?.IndicadaPeloCliente ?? false;
                NomeCliente = oficina?.NomeCliente;
                CelularClienteDescricao = oficina.CelularCliente?.Numero;
                Nome = !string.IsNullOrEmpty(oficina.Pessoa?.Nome) ? oficina.Pessoa?.Nome : oficina.NomeFantasia;
                RazaoSocial = oficina?.RazaoSocial;
                TipoPessoa = oficina.TipoPessoa;
                Cnpj = oficina.Pessoa.DocumentoCnpj;
                InscricaoEstadual = oficina.Pessoa.DocumentoIe;
                InscricaoMunicipal = oficina.Pessoa.DocumentoIm;
                TipoPessoa = oficina.TipoPessoa;

                if (Pessoa != null && Pessoa.Enderecos != null && Pessoa.Enderecos.Any())
                {
                    Pessoa.Endereco = Pessoa.Enderecos.FirstOrDefault();
                    Pessoa.Endereco.Estado = Pessoa.Endereco.Cidade.Estado.Sigla;
                }
            }
        }

        public Oficina ToEntity()
        {
            var oficina = new Oficina();

            oficina.Id = Id;
            oficina.DataInsercao = DateTime.Now;
            oficina.Pessoa = Pessoa?.ToEntity();
            oficina.NomeFantasia = this.NomeFantasia;
            oficina.IndicadaPeloCliente = this.IndicadaPeloCliente;
            oficina.NomeCliente = this.NomeCliente;
            oficina.CelularCliente = this.CelularOficina?.ToEntity();
            oficina.RazaoSocial = this.RazaoSocial;
            oficina.Pessoa.DocumentoCpf = this.Pessoa.Cpf;
            oficina.Pessoa.DocumentoRg = this.Pessoa.Rg;
            oficina.Pessoa.DocumentoCnpj = this.Cnpj;
            oficina.Pessoa.DocumentoIe = this.InscricaoEstadual;
            oficina.Pessoa.DocumentoIm = this.InscricaoMunicipal;
            oficina.TipoPessoa = this.TipoPessoa;

            return oficina;
        }
    }
}