using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IFuncionarioAplicacao : IBaseAplicacao<Funcionario>
    {
        Funcionario BuscarPorCpf(string cpf);
        List<Funcionario> BuscarComDadosSimples();
        void ValidarTipoEscala(int id);
    }

    public class FuncionarioAplicacao : BaseAplicacao<Funcionario, IFuncionarioServico>, IFuncionarioAplicacao
    {
        private readonly IFuncionarioServico _funcionarioServico;

        public FuncionarioAplicacao(IFuncionarioServico funcionarioServico)
        {
            _funcionarioServico = funcionarioServico;
        }

        public Funcionario BuscarPorCpf(string cpf)
        {
            return _funcionarioServico.BuscarPor(x => x.Pessoa.Documentos.Any(d => d.Documento.Tipo == TipoDocumento.Cpf && d.Documento.Numero == cpf)).FirstOrDefault();
        }

        public new void Salvar(Funcionario item)
        {
            Servico.Salvar(item);
        }

        public List<Funcionario> BuscarComDadosSimples()
        {
            return _funcionarioServico.BuscarComDadosSimples();
        }

        public void ValidarTipoEscala(int id)
        {
            var funcionario = _funcionarioServico.BuscarPorId(id);
            funcionario.ValidarTipoEscala();
        }
    }
}