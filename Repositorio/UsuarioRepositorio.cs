using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Linq;

namespace Repositorio
{
    public class UsuarioRepositorio : NHibRepository<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(NHibContext context)
            : base(context)
        {
        }

        public Usuario RetornarPorCPF(string cpf)
        {
            var cpfSemFormatacao = cpf.Replace("-", "").Replace(".", "").Replace("/", "");
            return FirstBy(x => x.Funcionario.Pessoa.Documentos
                .Any(z => z.Documento.Tipo == Entidade.Uteis.TipoDocumento.Cpf && 
                    (z.Documento.Numero.Trim() == cpfSemFormatacao.Trim() || z.Documento.Numero.Trim() == cpf.Trim())));
        }

        public Usuario ValidarLogin(string cpf, string senha)
        {
            var cpfSemFormatacao = cpf.Replace("-", "").Replace(".", "").Replace("/", "");
            return FirstBy(x => x.Funcionario.Pessoa.Documentos
                .Any(z=> z.Documento.Tipo == Entidade.Uteis.TipoDocumento.Cpf && 
                (z.Documento.Numero.Trim() == cpfSemFormatacao.Trim() || z.Documento.Numero.Trim() == cpf.Trim())) && x.Senha.Equals(senha));
        }
    }
}
