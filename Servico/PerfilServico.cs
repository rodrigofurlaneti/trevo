using System.Linq;
using Core;
using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPerfilServico : IBaseServico<Perfil>
    {
        void SalvarEAtualizarCache(Perfil perfil, int[] permissoes);
        void ExcluirEAtualizarCache(Perfil perfil);
        void ExcluirEAtualizarCache(int idPerfil);
        bool VerificaPerfilJaAssociado(int idPerfil);
    }

    public class PerfilServico : BaseServico<Perfil, IPerfilRepositorio>, IPerfilServico
    {
        public void ExcluirEAtualizarCache(Perfil perfil)
        {
            perfil = BuscarPorId(perfil.Id);

            if (VerificaPerfilJaAssociado(perfil.Id))
                throw new BusinessRuleException("Perfil já está sendo utilizado!");

            Excluir(perfil);
            //
            CacheLayer.Clear(perfil.Id.ToString());
        }
        public void ExcluirEAtualizarCache(int idPerfil)
        {
            var perfil = BuscarPorId(idPerfil);

            if (VerificaPerfilJaAssociado(perfil.Id))
                throw new BusinessRuleException("Perfil já está sendo utilizado!");

            ExcluirPorId(perfil.Id);
            //
            CacheLayer.Clear(perfil.Id.ToString());
        }

        public void SalvarEAtualizarCache(Perfil perfil, int[] permissoes)
        {
            perfil.Permissoes.Clear();
            if (permissoes != null)
            {
                foreach (var permissao in permissoes)
                    perfil.Permissoes.Add(new Permissao { Id = permissao });
            }
            //
            Salvar(perfil);
            perfil = BuscarPorId(perfil.Id);
            //
            CacheLayer.Clear(perfil.Id.ToString());
            CacheLayer.Add(perfil.Id.ToString(), perfil.Permissoes.Select(x => x.Regra));
        }

        public bool VerificaPerfilJaAssociado(int idPerfil)
        {
            if (idPerfil <= 0)
                return false;

            var perfil = Repositorio.FirstBy(x => x.Id == idPerfil);

            return perfil?.Menus != null && perfil.Menus.Any() 
                //|| perfil?.Permissoes != null && perfil.Permissoes.Any()   Por enquanto não será algo para validar por não existir cadastro
                || perfil?.Usuarios != null && perfil.Usuarios.Any();
        }
    }
}