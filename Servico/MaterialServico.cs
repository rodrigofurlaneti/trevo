using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IMaterialServico : IBaseServico<Material>
    {
        List<EstoqueMaterial> BuscarEstoqueMateriaisPeloMaterialId(int materialId);
        List<MaterialHistorico> BuscarMaterialHistoricoPeloMaterialId(int materialId);
        void SalvarMaterialNotificacao(Material material, Notificacao notificacao);
	    void SalvarMaterialHistorico(MaterialHistorico materialHistorico);
		Notificacao SalvarNotificacaoComRetorno(Material material, Usuario usuario, string descricaoAuxiliar = "");
        void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao);
        EstoqueMaterial BuscarEstoqueMaterial(int materialId, int estoqueId);
    }

    public class MaterialServico : BaseServico<Material, IMaterialRepositorio>, IMaterialServico
    {
        private readonly IMaterialRepositorio _materialRepositorio;
        private readonly IEstoqueMaterialRepositorio _estoqueMaterialRepositorio;
        private readonly IMaterialHistoricoRepositorio _materialHistoricoRepositorio;
        private readonly IMaterialNotificacaoRepositorio _materialNotificacaoRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IMaterialFornecedorRepositorio _materialFornecedorRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public MaterialServico(
            IMaterialRepositorio materialRepositorio,
            IEstoqueMaterialRepositorio estoqueMaterialRepositorio,
            IMaterialHistoricoRepositorio materialHistoricoRepositorio,
            IMaterialNotificacaoRepositorio materialNotificacaoRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            IMaterialFornecedorRepositorio materialFornecedorRepositorio,
            INotificacaoRepositorio notificacaoRepositorio
            )
        {
            _materialRepositorio = materialRepositorio;
            _estoqueMaterialRepositorio = estoqueMaterialRepositorio;
            _materialHistoricoRepositorio = materialHistoricoRepositorio;
            _materialNotificacaoRepositorio = materialNotificacaoRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _materialFornecedorRepositorio = materialFornecedorRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public List<EstoqueMaterial> BuscarEstoqueMateriaisPeloMaterialId(int materialId)
        {
            return _estoqueMaterialRepositorio.ListBy(x => x.Material.Id == materialId).ToList();
        }

        public List<MaterialHistorico> BuscarMaterialHistoricoPeloMaterialId(int materialId)
        {
            return _materialHistoricoRepositorio.ListBy(x => x.Material.Id == materialId).ToList();
        }

        public override void Salvar(Material material)
        {
            material.MaterialNotificacaos = material.MaterialNotificacaos ?? new List<MaterialNotificacao>();

            if (material.Id > 0)
                material.MaterialNotificacaos = _materialRepositorio.GetById(material.Id).MaterialNotificacaos;

            material.AdicionarMaterialAoMaterialFornecedor();
            base.Salvar(material);

            DeletarMateriaisFornecedoresSemMateriais();
        }

        public void SalvarMaterialNotificacao(Material material, Notificacao notificacao)
        {
            var materialNotificacao = new MaterialNotificacao
            {
                Material = material,
                Notificacao = notificacao
            };

            _materialNotificacaoRepositorio.Save(materialNotificacao);
        }

        public void SalvarMaterialHistorico(MaterialHistorico materialHistorico)
        {
            _materialHistoricoRepositorio.Save(materialHistorico);
        }

        public Notificacao SalvarNotificacaoComRetorno(Material material, Usuario usuario, string descricaoAuxiliar = "")
        {
            var enumerador = (Entidades)Enum.Parse(typeof(Entidades), nameof(Material));
            var notificacao = new Notificacao
            {
                Usuario = usuario,
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == enumerador),
                Status = StatusNotificacao.Aguardando,
                Descricao = $"Estoque Baixo - O estoque minimo do material {material.Nome} é de {material.EstoqueMinimo}" +
                            $" e ele possui apenas {material.QuantidadeTotalEstoque} em estoque.",
                DataInsercao = DateTime.Now,
                AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar,
                DataVencimentoNotificacao = null
            };
            var notificacaoId = _notificacaoRepositorio.SaveAndReturn(notificacao);

            return _notificacaoRepositorio.GetById(notificacaoId);
        }

        public void AtualizarStatus(int notificacaoId, Usuario usuario, AcaoNotificacao acao)
        {
            var materialNotificacao = _materialNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == notificacaoId);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    materialNotificacao.Aprovar(usuario);
                    break;
                case AcaoNotificacao.Reprovado:
                    materialNotificacao.Reprovar(usuario);
                    break;
                default:
                    break;
            }

            _materialNotificacaoRepositorio.Save(materialNotificacao);
        }

        private void DeletarMateriaisFornecedoresSemMateriais()
        {
            var materiaisFornecedores = _materialFornecedorRepositorio.ListBy(x => x.Material == null);

            foreach (var item in materiaisFornecedores)
            {
                _materialFornecedorRepositorio.Delete(item);
            }
        }

        public EstoqueMaterial BuscarEstoqueMaterial(int materialId, int estoqueId)
        {
            return _estoqueMaterialRepositorio.FirstBy(x => x.Material.Id == materialId && x.Estoque.Id == estoqueId);
        }
    }
}