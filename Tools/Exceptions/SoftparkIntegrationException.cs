using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class SoftparkIntegrationException : Exception
    {
        public SoftparkIntegrationException()
        {
            Message = "Operação realizada com sucesso, porém ocorreu um erro na integração com o softpark. Por favor contate o suporte.";
        }

        public SoftparkIntegrationException(string message) : base(message)
        {
            Message = message;
        }

        public override string Message { get; }
    }
}