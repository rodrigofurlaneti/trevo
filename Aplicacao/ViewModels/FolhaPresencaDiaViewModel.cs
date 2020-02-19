using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class FolhaPresencaDiaViewModel
    {
        public DateTime Data { get; set; }
        public bool EhFeriado { get; set; }
        public bool EhFerias { get; set; }
        public string Assinatura {
            get {
                if (EhFerias)
                    return "FÉRIAS";

                else if (Data.ToString("dddd").ToLower() == "domingo")
                    return "DOMINGO";

                else if (EhFeriado)
                    return "FERIADO";

                return "";
            }
        }
    }
}