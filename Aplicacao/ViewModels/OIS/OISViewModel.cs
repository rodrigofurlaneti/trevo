using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class OISViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string NomeCliente { get; set; }
        public string TelefoneFixo { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public List<OISContatoViewModel> OISContatos {
            get
            {
                var contatos = new List<OISContatoViewModel>();

                if (!string.IsNullOrEmpty(TelefoneFixo))
                    contatos.Add(new OISContatoViewModel { Contato = new ContatoViewModel { Tipo = TipoContato.Residencial, Numero = TelefoneFixo } });

                if (!string.IsNullOrEmpty(Celular))
                    contatos.Add(new OISContatoViewModel { Contato = new ContatoViewModel { Tipo = TipoContato.Celular, Numero = Celular } });

                if (!string.IsNullOrEmpty(Email))
                    contatos.Add(new OISContatoViewModel { Contato = new ContatoViewModel { Tipo = TipoContato.Email, Email = Email } });

                return contatos;
            }
        }
        public MarcaViewModel Marca { get; set; }
        public ModeloViewModel Modelo { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string Placa { get; set; }
        public string Cor { get; set; }
        public string Ano { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public List<OISCategoriaViewModel> OISCategorias { get; set; }

        public string Categorias
        {
            get
            {
                return string.Join("; ", OISCategorias.Select(x => x.Descricao));
            }
        }

        public string OutraCategoria { get; set; }
        public List<OISFuncionarioViewModel> OISFuncionarios { get; set; }
        public StatusSinistro StatusSinistro { get; set; }
        public string Observacao { get; set; }
        public List<OISImagemViewModel> OISImagens { get; set; }
        public List<OISImagemViewModel> ImagensParaSalvar { get; set; } = new List<OISImagemViewModel> { new OISImagemViewModel(), new OISImagemViewModel(), new OISImagemViewModel(), new OISImagemViewModel() };
    }
}