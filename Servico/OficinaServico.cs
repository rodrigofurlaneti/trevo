using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System.Linq;

namespace Dominio
{
    public interface IOficinaServico : IBaseServico<Oficina>
    {
    }

    public class OficinaServico : BaseServico<Oficina, IOficinaRepositorio>, IOficinaServico
    {
        private readonly IOficinaRepositorio _oficinaRepositorio;
        private readonly ICidadeRepositorio _cidadeRepositorio;
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;

        public OficinaServico(
            IOficinaRepositorio oficinaRepositorio
            , ICidadeRepositorio cidadeRepositorio
            , IContatoRepositorio contatoRepositorio
            , IPessoaRepositorio pessoaRepositorio
            )
        {
            _oficinaRepositorio = oficinaRepositorio;
            _cidadeRepositorio = cidadeRepositorio;
            _contatoRepositorio = contatoRepositorio;
            _pessoaRepositorio = pessoaRepositorio;
        }

        private void AdicionarEnderecos(Oficina oficina)
        {
            foreach (var pessoaEndereco in oficina.Pessoa.Enderecos)
            {
                pessoaEndereco.Endereco.Cidade = pessoaEndereco.Endereco.Cidade == null
                    ? null
                    : _cidadeRepositorio.FirstBy(x => x.Descricao == pessoaEndereco.Endereco.Cidade.Descricao);
            }
        }

        private void AdicionarContatos(Oficina oficina)
        {
            if (oficina.CelularCliente != null)
            {
                var contatoExistente = _contatoRepositorio.FirstBy(x => x.Numero == oficina.CelularCliente.Numero);

                if (contatoExistente != null)
                    oficina.CelularCliente.Id = contatoExistente.Id;
                else
                    _contatoRepositorio.Save(oficina.CelularCliente);
            }

            if (oficina.Pessoa.Contatos != null && oficina.Pessoa.Contatos.Any())
            {
                var oficinaTelefones = oficina.Pessoa.Contatos.Where(x => x.Contato.Tipo == TipoContato.Residencial || x.Contato.Tipo == TipoContato.Celular).Select(x => x.Contato).ToList();

                foreach (var item in oficinaTelefones)
                {
                    var contatoExistente = _contatoRepositorio.FirstBy(x => x.Numero == item.Numero);
                    item.Id = contatoExistente != null ? contatoExistente.Id : item.Id;
                }

                var oisEmail = oficina.Pessoa.Contatos.FirstOrDefault(x => x.Contato.Tipo == TipoContato.Email)?.Contato;
                if (oisEmail != null)
                {
                    var emailExistente = _contatoRepositorio.FirstBy(x => x.Email == oisEmail.Email);
                    oisEmail.Id = emailExistente != null ? emailExistente.Id : oisEmail.Id;
                }

                var contatosParaSalvar = oficina.Pessoa.Contatos.Where(x => x.Contato.Id <= 0).Select(x => x.Contato).ToList();
                if (contatosParaSalvar != null && contatosParaSalvar.Any())
                    _contatoRepositorio.Save(contatosParaSalvar);
            }
        }

        private void AdicionarPessoa(Oficina oficina)
        {
            if (oficina.Pessoa != null)
            {
                var cpf = oficina.Pessoa.DocumentoCpf;
                var pessoaExistente = _pessoaRepositorio.FirstBy(x => x.Documentos
                                      .Any(d => d.Documento.Tipo == TipoDocumento.Cpf && d.Documento.Numero == cpf));

                oficina.Pessoa.Id = pessoaExistente != null ? pessoaExistente.Id : 0;
                _pessoaRepositorio.Save(oficina.Pessoa);
            }
        }

        public override void Salvar(Oficina oficina)
        {
            AdicionarEnderecos(oficina);
            AdicionarContatos(oficina);
            AdicionarPessoa(oficina);

            base.Salvar(oficina);
        }
    }
}