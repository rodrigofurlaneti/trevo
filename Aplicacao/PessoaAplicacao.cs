using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;
using Aplicacao.ViewModels;

namespace Aplicacao
{
    public interface IPessoaAplicacao : IBaseAplicacao<Pessoa>
    {
        bool ValidarSeCpfExiste(string cpf);
        Pessoa BuscarPorCpf(string cpf);
    }

    public class PessoaAplicacao : BaseAplicacao<Pessoa, IPessoaServico>, IPessoaAplicacao
    {
        private readonly IPessoaServico _pessoaServico;

        public PessoaAplicacao(IPessoaServico pessoaServico)
        {
            _pessoaServico = pessoaServico;
        }

        public Pessoa BuscarPorCpf(string cpf)
        {
            return _pessoaServico.PrimeiroPor(x => x.Documentos.Any(d => d.Documento.Tipo == TipoDocumento.Cpf && d.Documento.Numero == cpf));
        }

        public bool ValidarSeCpfExiste(string cpf)
        {
            var pessoa = _pessoaServico.PrimeiroPor(x => x.Documentos.Any(d => d.Documento.Tipo == TipoDocumento.Cpf && d.Documento.Numero == cpf));
            return pessoa != null;
        }
    }
}