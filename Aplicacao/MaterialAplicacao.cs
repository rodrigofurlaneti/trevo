using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IMaterialAplicacao : IBaseAplicacao<Material>
    {
        void SalvarDados(MaterialViewModel materialViewModel);
        List<EstoqueMaterialViewModel> BuscarEstoqueMateriaisPeloMaterialId(int materialId);
        List<MaterialHistoricoViewModel> BuscarMaterialHistoricoPeloMaterialId(int materialId);
        EstoqueMaterialViewModel BuscarEstoqueMaterial(int materialId, int estoqueId);
    }

    public class MaterialAplicacao : BaseAplicacao<Material, IMaterialServico>, IMaterialAplicacao
    {
        private readonly IMaterialServico _materialServico;

        public MaterialAplicacao(IMaterialServico materialServico)
        {
            _materialServico = materialServico;
        }

        public List<EstoqueMaterialViewModel> BuscarEstoqueMateriaisPeloMaterialId(int materialId)
        {
            return Mapper.Map<List<EstoqueMaterialViewModel>>(_materialServico.BuscarEstoqueMateriaisPeloMaterialId(materialId));
        }

        public List<MaterialHistoricoViewModel> BuscarMaterialHistoricoPeloMaterialId(int materialId)
        {
            return Mapper.Map<List<MaterialHistoricoViewModel>>(_materialServico.BuscarMaterialHistoricoPeloMaterialId(materialId));
        }

        public EstoqueMaterialViewModel BuscarEstoqueMaterial(int materialId, int estoqueId)
        {
            return Mapper.Map<EstoqueMaterialViewModel>(_materialServico.BuscarEstoqueMaterial(materialId, estoqueId));
        }

        public void SalvarDados(MaterialViewModel materialViewModel)
        {
            var material = Mapper.Map<Material>(materialViewModel);
            _materialServico.Salvar(material);
        }
    }
}