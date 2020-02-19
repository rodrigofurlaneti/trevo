using Aplicacao.ViewModels;

namespace Aplicacao.Softpark.ViewModels
{
    public class ContratoCondominoCarroSoftparkViewModel
    {
        public ContratoCondominoSoftparkViewModel ContratoCondutor { get; set; }
        public CarroSoftparkViewModel Carro { get; set; }

        public ContratoCondominoCarroSoftparkViewModel(ContratoCondominoSoftparkViewModel contratoCondutor, CarroSoftparkViewModel carro)
        {
            ContratoCondutor = contratoCondutor;
            Carro = carro;
        }
    }
}
