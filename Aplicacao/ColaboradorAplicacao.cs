using Aplicacao.Base;
using Dominio;
using Entidade;


namespace Aplicacao
{
    public interface IColaboradorAplicacao : IBaseAplicacao<Colaborador>
    {

    }


    public class ColaboradorAplicacao : BaseAplicacao<Colaborador, IColaboradorServico>, IColaboradorAplicacao
    {

    }
}
