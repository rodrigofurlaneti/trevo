using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class MovimentacaoMap : ClassMap<Movimentacao>
    {
        public MovimentacaoMap()
        {
            Table("Movimentacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.IdSoftpark).Column("IdSoftpark");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.NumFechamento, "NumFechamento");
            Map(x => x.NumTerminal, "NumTerminal");
            Map(x => x.DataAbertura, "DataAbertura");
            Map(x => x.DataFechamento, "DataFechamento");
            Map(x => x.Ticket, "Ticket");
            Map(x => x.Placa, "Placa");
            Map(x => x.DataEntrada, "DataEntrada");
            Map(x => x.DataSaida, "DataSaida");
            Map(x => x.ValorCobrado, "ValorCobrado");
            Map(x => x.DescontoUtilizado, "DescontoUtilizado");
            Map(x => x.ValorDesconto, "ValorDesconto");
            Map(x => x.TipoCliente, "TipoCliente");
            Map(x => x.NumeroContrato, "NumeroContrato");
            Map(x => x.VagaIsenta, "VagaIsenta");

            References(x => x.Unidade, "Unidade_Id");
            References(x => x.Usuario, "Usuario_Id");
            References(x => x.Cliente, "Cliente_Id");

            HasMany(x => x.MovimentacaoSelo).Cascade.All();
        }
    }
}
