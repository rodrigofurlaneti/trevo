using System.Collections.Generic;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Core.Exceptions;
using System.Linq;
using Aplicacao.ViewModels;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IContratoUnidadeAplicacao : IBaseAplicacao<ContratoUnidade>
    { }

    public class ContratoUnidadeAplicacao : BaseAplicacao<ContratoUnidade, IContratoUnidadeServico>, IContratoUnidadeAplicacao
    {
        public void CriaAtualizaContrato(ContratoUnidadeViewModel viewModel)
        {

        }
    }
}