using System;

namespace Aplicacao.ViewModels
{
    public interface IBaseSoftparkViewModel
    {
        int Id { get; set; }
        DateTime DataInsercao { get; set; }
    }

    public class BaseSoftparkViewModel : IBaseSoftparkViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
    }
}
