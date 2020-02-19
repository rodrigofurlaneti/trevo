using Aplicacao.Softpark.Base;
using Aplicacao.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Aplicacao
{
    public interface IOperadorSoftparkAplicacao : IBaseSoftparkAplicacao<OperadorSoftparkViewModel>
    {
    }

    public class OperadorSoftparkAplicacao : BaseSoftparkAplicacao<OperadorSoftparkViewModel>, IOperadorSoftparkAplicacao
    {
        public override string Tela => "Operador";
    }
}
