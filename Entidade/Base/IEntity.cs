using System;

namespace Entidade.Base
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime DataInsercao { get; set; }
    }

    public interface IAudit { }
}