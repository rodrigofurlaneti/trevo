using Core.Extensions;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class OISCategoriaViewModel
    {
        public int OisId { get; set; }
        public TipoOISCategoria TipoCategoria { get; set; }
        public string OutraCategoria { get; set; }
        public string Descricao {
            get
            {
                if (TipoCategoria == TipoOISCategoria.Outros)
                    return OutraCategoria;

                return TipoCategoria.ToDescription();
            }
        }
    }
}