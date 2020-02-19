using Core.Extensions;
using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class ItemFuncionarioRepositorio : NHibRepository<ItemFuncionario>, IItemFuncionarioRepositorio
    {
        private readonly IEstoqueMaterialRepositorio _estoqueMaterialRepositorio;

        public ItemFuncionarioRepositorio(
            NHibContext context,
            IEstoqueMaterialRepositorio estoqueMaterialRepositorio
            )
            : base(context)
        {
            _estoqueMaterialRepositorio = estoqueMaterialRepositorio;
        }

        public void AtualizarEstoque(ItemFuncionario itemFuncionario, ItemFuncionario itemFuncionarioAntigo)
        {
            var listaEstoqueMaterial = new List<EstoqueMaterial>();

            if (itemFuncionarioAntigo != null && itemFuncionarioAntigo.Id > 0)
            {
                foreach (var item in itemFuncionarioAntigo.ItemFuncionariosDetalhes)
                {
                    var estoqueMaterial = _estoqueMaterialRepositorio.GetById(item.EstoqueMaterial.Id);
                    estoqueMaterial.Material.DarEntrada(item.Quantidade, false);
                    estoqueMaterial.DarEntrada(item.Quantidade, item.EstoqueMaterial.Preco);
                    listaEstoqueMaterial.Add(estoqueMaterial);
                }
            }

            foreach (var item in itemFuncionario?.ItemFuncionariosDetalhes)
            {
                var estoqueMaterial = listaEstoqueMaterial.FirstOrDefault(x => x.Id == item.EstoqueMaterial.Id);

                if(estoqueMaterial == null)
                {
                    estoqueMaterial = _estoqueMaterialRepositorio.GetById(item.EstoqueMaterial.Id);
                    listaEstoqueMaterial.Add(estoqueMaterial);
                }

                estoqueMaterial.Material.DarSaida(item.Quantidade, false);
                estoqueMaterial.DarSaida(item.Quantidade);
                item.EstoqueMaterial = estoqueMaterial;
            }

            _estoqueMaterialRepositorio.Save(listaEstoqueMaterial);
        }

        public void DeleteOrphan()
        {
            var orphans = ListBy(x => x.Funcionario == null);

            foreach (var orphan in orphans)
            {
                Delete(orphan);
            }
        }
    }
}