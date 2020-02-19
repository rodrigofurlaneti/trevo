using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ILayoutAplicacao : IBaseAplicacao<Layout>
    {
        void SalvarDados(LayoutViewModel viewModel, List<LayoutFormatoViewModel> formatos);
    }

    public class LayoutAplicacao : BaseAplicacao<Layout, ILayoutServico>, ILayoutAplicacao
    {
        public void SalvarDados(LayoutViewModel viewModel, List<LayoutFormatoViewModel> formatos)
        {
            var entity = BuscarPorId(viewModel.Id) ?? new Layout();

            entity.Nome = viewModel.Nome;

            if (entity.Formatos == null || !entity.Formatos.Any())
            {
                entity.Formatos = new List<LayoutFormato>();
                foreach (var formato in formatos.Select(x => x.ToEntity()))
                {
                    entity.Formatos.Add(formato);
                }
            }
            else
            {
                //Remove
                var removerFormatos = new List<int>();
                var removerLinhas = new List<int>();
                foreach (var layoutFormato in entity.Formatos)
                {
                    if (formatos.All(x => x.Id != layoutFormato.Id))
                    {
                        removerFormatos.Add(layoutFormato.Id);
                        continue;
                    }
                    var itemFormato = formatos.FirstOrDefault(x => x.Id == layoutFormato.Id);
                    if (itemFormato == null)
                        continue;

                    removerLinhas.AddRange(from layoutCampo in layoutFormato.Linhas where itemFormato.Linhas.All(x => x.Id != layoutCampo.Id) select layoutCampo.Id);
                    foreach (var remover in removerLinhas)
                    {
                        layoutFormato.Linhas.Remove(layoutFormato.Linhas.FirstOrDefault(x => x.Id == remover));
                    }
                }
                foreach (var removerFormato in removerFormatos)
                {
                    entity.Formatos.Remove(entity.Formatos.FirstOrDefault(x => x.Id == removerFormato));
                }
                //Add or Edit
                foreach (var layoutFormato in formatos)
                {
                    if (layoutFormato.Id > 0 && entity.Formatos.Any(x => x.Id == layoutFormato.Id))
                    {
                        var item = entity.Formatos.FirstOrDefault(x => x.Id == layoutFormato.Id);
                        item.Descricao = layoutFormato.Descricao;
                        item.Formato = layoutFormato.Formato;
                        item.Delimitador = layoutFormato.Delimitador;

                        foreach (var layoutCampo in layoutFormato.Linhas)
                        {
                            if (layoutCampo.Id > 0 && item.Linhas.Any(x => x.Id == layoutCampo.Id))
                            {
                                var itemCampo = item.Linhas.FirstOrDefault(x => x.Id == layoutCampo.Id);
                                //itemCampo.Campo = layoutCampo.Campo;
                                //itemCampo.Posicao = layoutCampo.Posicao;
                                //itemCampo.Formatacao = layoutCampo.Formatacao;
                            }
                            else
                            {
                                item.Linhas.Add(layoutCampo.ToEntity());
                            }
                        }
                    }
                    else
                    {
                        entity.Formatos.Add(layoutFormato.ToEntity());
                    }
                }
            }

            Salvar(entity);
        }
    }
}