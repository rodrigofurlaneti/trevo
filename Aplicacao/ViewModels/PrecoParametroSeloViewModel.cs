using Entidade;

namespace Aplicacao.ViewModels
{
    public class PrecoParametroSeloViewModel
    {
        public int Id { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public TipoSelo TipoPreco { get; set; }
        public string DescontoTabelaPreco { get; set; }
        public string DescontoCustoTabelaPreco { get; set; }
        public string DescontoMaximoValor { get; set; }
        public PerfilViewModel Perfil { get; set; }
	    public bool TodasUnidades { get; set; }
    }
}