using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IUsuarioAplicacao : IBaseAplicacao<Usuario>
    {
        Usuario ValidarLogin(string cpf, string senha);
        Usuario RetornarPorCPF(string cpf);
        void RecuperarSenha(string cpf, string template, string url);
        void TrocarSenha(string cpf, string senha, string senhaConfirmacao);
        void PrimeiroLoginRealizado(string cpf);
        List<Perfil> BuscarPerfis();
    }

    public class UsuarioAplicacao : BaseAplicacao<Usuario, IUsuarioServico>, IUsuarioAplicacao
    {
        private readonly IPerfilAplicacao _perfilAplicacao;
        private readonly IOperadorSoftparkAplicacao _operadorSoftparkAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public UsuarioAplicacao(IPerfilAplicacao perfilAplicacao, IOperadorSoftparkAplicacao operadorSoftparkAplicacao, IUnidadeAplicacao unidadeAplicacao)
        {
            _perfilAplicacao = perfilAplicacao;
            _operadorSoftparkAplicacao = operadorSoftparkAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        public Usuario ValidarLogin(string cpf, string senha)
        {
            return Servico.ToCast<IUsuarioServico>().ValidarLogin(cpf, senha);
        }

        public void RecuperarSenha(string cpf, string template, string url)
        {
            Servico.ToCast<IUsuarioServico>().RecuperarSenha(cpf, template, url);
        }

        public void TrocarSenha(string cpf, string senha, string senhaConfirmacao)
        {
            Servico.ToCast<IUsuarioServico>().TrocarSenha(cpf, senha, senhaConfirmacao);
        }

        public void PrimeiroLoginRealizado(string cpf)
        {
            Servico.ToCast<IUsuarioServico>().PrimeiroLoginRealizado(cpf);
        }

        public List<Perfil> BuscarPerfis()
        {
            return _perfilAplicacao.Buscar().ToList();
        }

        public new void Salvar(Usuario entity)
        {
            Servico.Salvar(entity);
            
            if (entity.TemAcessoAoPDV)
            {
                var usuario = Servico.BuscarPorId(entity.Id);
                usuario.Unidade = _unidadeAplicacao.BuscarPorId(usuario.Unidade.Id);

                var operador = new OperadorSoftparkViewModel(usuario);
                _operadorSoftparkAplicacao.Salvar(operador);
            }
        }

        public Usuario RetornarPorCPF(string cpf)
        {
            return Servico.ToCast<IUsuarioServico>().RetornarPorCPF(cpf);
        }

        public override void ExcluirPorId(int id)
        {
            base.ExcluirPorId(id);

            _operadorSoftparkAplicacao.ExcluirPorId(id);
        }
    }
}