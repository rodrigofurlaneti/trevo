using Aplicacao.ViewModels;

namespace Aplicacao.Softpark.ViewModels
{
    public class ContratoMensalistaCarroSoftparkViewModel
    {
        public ContratoSoftparkViewModel ContratoCondutor { get; set; }
        public CarroSoftparkViewModel Carro { get; set; }

        public ContratoMensalistaCarroSoftparkViewModel(ContratoSoftparkViewModel contratoCondutor, CarroSoftparkViewModel carro)
        {
            ContratoCondutor = contratoCondutor;
            Carro = carro;
        }
    }
}
