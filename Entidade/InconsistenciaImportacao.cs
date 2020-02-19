using Entidade.Base;

namespace Entidade
{
    public class InconsistenciaImportacao : BaseEntity
    {
        public virtual int Line { get; set; }
        public virtual string Message { get; set; }
        public virtual string StackTrace { get; set; }
        public virtual bool IsError { get; set; }
    }
}