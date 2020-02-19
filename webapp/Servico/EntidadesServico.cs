using Aplicacao;
using Aplicacao.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Servico
{
    public static class EntidadesServico
    {

        public static List<PrecoViewModel> ListarPreco(IPrecoAplicacao precoAplicacao)
        {

            return AutoMapper.Mapper.Map<List<Entidade.Preco>, List<PrecoViewModel>>(precoAplicacao.Buscar().Where(x =>x.Ativo == true).OrderBy(x => x.Nome).ToList());


        }


        public static List<UnidadeViewModel> ListarPerfil(IPerfilAplicacao perfilAplicacao)
        {

            return AutoMapper.Mapper.Map<List<Entidade.Perfil>, List<UnidadeViewModel>>(perfilAplicacao.Buscar().OrderBy(x => x.Nome).ToList());


        }


        public static List<BancoViewModel> ListarBanco(IBancoAplicacao bancoAplicacao)
        {

            return AutoMapper.Mapper.Map<List<Entidade.Banco>, List<BancoViewModel>>(bancoAplicacao.Buscar().OrderBy(x => x.Descricao).ToList());


        }






    }
}