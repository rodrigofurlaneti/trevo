using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class EquipeMap : ClassMap<Equipe>
    {
        public EquipeMap()
        {
            Table("Equipe");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Ativo).Column("Ativo");
            Map(x => x.Datafim).Column("Datafim");
            References(x => x.Unidade).Column("Unidade");
            References(x => x.TipoEquipe).Column("TipoEquipe");
            References(x => x.HorarioTrabalho).Column("HorarioTrabalho");
            Map(x => x.TipoHorario).Column("TipoHorario");
            References(x => x.Encarregado).Column("Encarregado");
            References(x => x.Supervisor).Column("Supervisor");
            HasMany(x => x.ParametrosEquipe).Cascade.All();

            //HasMany(x => x.Colaboradores)
            //   .Table("EquipeColaborador")
            //   .KeyColumn("Equipe")
            //   .Component(m =>
            //   {
            //       m.References(x => x.Colaborador).Cascade.All();
            //   }).Cascade.AllDeleteOrphan();

            HasMany(x => x.Colaboradores)
             .Table("EquipeColaborador")
             .KeyColumn("Equipe")
             .Component(m =>
             {
                 m.References(x => x.Colaborador).Cascade.All();
             }).Cascade.All();
        }
    }
}
