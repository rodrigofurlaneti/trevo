using Aplicacao.Softpark.Base;
using Aplicacao.ViewModels;
using Entidade;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Aplicacao
{
    public interface ICondutorSoftparkAplicacao : IBaseSoftparkAplicacao<CondutorSoftparkViewModel>
    {
    }

    public class CondutorSoftparkAplicacao : BaseSoftparkAplicacao<CondutorSoftparkViewModel>, ICondutorSoftparkAplicacao
    {
        public override string Tela => "Condutor";
    }
}
