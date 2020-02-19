namespace Aplicacao.ViewModels
{
    public class PaginacaoGenericaViewModel
    {
        public int PaginaAtual { get; set; }
        public int QuantidadeDePaginas { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int RegistroInicial { get; set; }
        public int RegistroFinal { get; set; }
        public int TotalRegistros { get; set; }

        public PaginacaoGenericaViewModel()
        {
            PaginaAtual = 1;
            QuantidadeDePaginas = 1;
        }

        public PaginacaoGenericaViewModel(int registrosPorPagina, int pagina, int totalRegistros)
        {
            RegistrosPorPagina = registrosPorPagina;
            RegistroInicial = (RegistrosPorPagina * (pagina - 1));
            RegistroFinal = RegistroInicial + registrosPorPagina;
            TotalRegistros = totalRegistros;
            PaginaAtual = pagina;
            QuantidadeDePaginas = totalRegistros / registrosPorPagina;

            var resto = totalRegistros % registrosPorPagina;
            if (resto > 0)
            {
                QuantidadeDePaginas++;

                if (RegistroFinal > totalRegistros)
                    RegistroFinal = RegistroInicial + resto;
            }
        }
    }
}
