using Entidade;
using System;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class PessoaEnderecoViewModel
    {
        public EnderecoViewModel Endereco {get; set;}

        public PessoaEnderecoViewModel()
        {
            Endereco = new EnderecoViewModel();
        }

        public PessoaEnderecoViewModel(PessoaEndereco entity)
        {
            Endereco = new EnderecoViewModel(entity.Endereco);
        }

        public PessoaEndereco ToEntity() => new PessoaEndereco
        {
            DataInsercao = DateTime.Now,
            Endereco = this.Endereco.ToEntity()
        };
    }
}