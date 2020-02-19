using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class BlockedReferenceDateException : Exception
    {
        public BlockedReferenceDateException() : base()
        {
        }

        public BlockedReferenceDateException(string message) : base(message)
        {
            Message = FriendlyMessage(message);
        }

        public BlockedReferenceDateException(string message, Exception inner) : base(message, inner)
        {
            Message = FriendlyMessage(message, inner);
        }

        protected BlockedReferenceDateException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }

        public override string Message { get; }

        private static string FriendlyMessage(string message, Exception inner = null)
        {
            var errors = Errors();
            foreach (var error in errors.Keys)
            {
                if (message.ToLower().Contains(error.ToLower()))
                {
                    return errors[error];
                }
            }
            if (inner == null) return message;

            foreach (var error in errors.Keys)
            {
                if (inner.Message.ToLower().Contains(error.ToLower()))
                {
                    return errors[error];
                }
            }
            return message;
        }

        private static Dictionary<string, string> Errors()
        {
            var errors = new Dictionary<string, string>
            {
                {"FOREIGN KEY", "Não é possível Excluir, pois esse item possui registros associados a ele."},
                {"The DELETE statement conflicted with the REFERENCE constraint", "Não é possível Excluir, pois esse item possui registros associados a ele."},
                {"COULD NOT DELETE", "Não é possível Excluir, pois esse item possui registros associados a ele."},
                {"DUPLICATE KEY", "Já existe um registro com as mesmas informação deste item."},
                {"Error converting value {null} to type 'System.DateTime'.", "Preencha o campo data."},
            };

            return errors;
        }
    }
}