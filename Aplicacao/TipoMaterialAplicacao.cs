using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface ITipoMaterialAplicacao : IBaseAplicacao<TipoMaterial>
    {
        List<TipoMaterialViewModel> BuscarOrdenados();
    }

    public class TipoMaterialAplicacao : BaseAplicacao<TipoMaterial, ITipoMaterialServico>, ITipoMaterialAplicacao
    {
        public List<TipoMaterialViewModel> BuscarOrdenados()
        {
            return Mapper.Map<List<TipoMaterialViewModel>>(Servico.Buscar());
        }
    }
}