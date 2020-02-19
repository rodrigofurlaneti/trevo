using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IDescontoAplicacao : IBaseAplicacao<Desconto>
    {
        List<DescontoViewModel> ListarOrdenado();
    }
    public class DescontoAplicacao : BaseAplicacao<Desconto, IDescontoServico>, IDescontoAplicacao
    {
        public List<DescontoViewModel> ListarOrdenado()
        {
            return AutoMapper.Mapper.Map<List<DescontoViewModel>>(Servico.Buscar().OrderBy(x => x.Descricao).ToList());
        }
    }
}

