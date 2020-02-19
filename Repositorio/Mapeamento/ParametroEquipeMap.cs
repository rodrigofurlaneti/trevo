using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ParametroEquipeMap : ClassMap<ParametroEquipe>
    {
        public ParametroEquipeMap()
        {
            Table("ParametroEquipe");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Ativo).Column("Ativo").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();
            Map(x => x.Usuario).Column("Usuario").Nullable();
            References(x => x.Equipe).Column("Equipe");
            HasMany(x => x.HorarioParametroEquipe).Cascade.All();

            HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }
}
