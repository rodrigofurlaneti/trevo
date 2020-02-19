using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class EmissaoSeloRepositorio : NHibRepository<EmissaoSelo>, IEmissaoSeloRepositorio
    {
        public EmissaoSeloRepositorio(NHibContext context) 
            : base(context)
        {

        }

        public List<int> RetornaListaIdPedidoSeloDasEmissoesDeSelo(List<int> listaIdPedido)
        {
            //return Session.GetList<EmissaoSelo>().Select(x => x.PedidoSelo.Id).ToList();

            if (listaIdPedido == null || !listaIdPedido.Any())
                throw new ArgumentNullException("Informe uma lista de Ids de Pedido.");

            var sql = new StringBuilder();

            sql.Append(" SELECT DISTINCT p.Id FROM EmissaoSelo es ");
            sql.Append("    JOIN es.PedidoSelo p ");
            sql.Append($"   WHERE p.Id in ({string.Join(",", listaIdPedido)})");

            var query = Session.CreateQuery(sql.ToString());

            return query.List<int>()?.ToList();
        }

        public int RetornaProximoNumeroLote()
        {
            var sql = $@"SELECT MAX(NumeroLote) FROM EmissaoSelo;";
            var valor = Session.CreateSQLQuery(sql).UniqueResult();

            if (valor != null)
                return Convert.ToInt32(valor) + 1;

            return 1;
        }

        public IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(StatusSelo? status, int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao)
        {
            var sql = new StringBuilder();

            var colunas = new List<string> {
                "ems.Id EmissaoId", "ems.DataInsercao EmissaoDataInsercao", "ems.StatusSelo", "ems.NumeroLote", "ems.Validade", "ems.EntregaRealizada", "ems.DataEntrega",
                "ps.Id PedidoSeloId", "ps.TiposPagamentos", "ps.ValidadePedido", "ps.Quantidade", "ps.DiasVencimento", "ps.TipoPedidoSelo", "ps.DataVencimento PedidoSeloDataVencimento", "ps.Convenio ConvenioId",
                "ts.Id TipoSeloId", "ts.Nome TipoSeloNome", "ts.Valor TipoSeloValor", "ts.Ativo TipoSeloAtivo", "ts.ParametroSelo TipoSeloParametroSelo", "ts.ComValidade TipoSeloComValidade", "ts.PagarHorasAdicionais TipoSeloPagarHorasAdicionais",
                "c.Id ClienteId", "c.RazaoSocial", "c.NomeFantasia", "c.Pessoa PessoaId", "p.Nome NomePessoa",
                "u.Id UnidadeId", "u.Codigo CodigoUnidade", "u.Nome NomeUnidade",
                "e.Id IdEndereco", "e.Cep", "e.Logradouro", "e.Numero NumeroEndereco", "e.Complemento", "e.Bairro",
                "ue.Id IdUnidadeEndereco", "ue.Cep UnidadeCep", "ue.Logradouro UnidadeLogradouro", "ue.Numero UnidadeNumeroEndereco", "ue.Complemento UnidadeComplemento", "ue.Bairro UnidadeBairro",
                "e.Cidade_id", "ci.Descricao Cidade", "es.Id IdEstado", "es.Descricao Estado", "es.Sigla EstadoSigla",
                "docCPF.IdCPF", "docCPF.CPF", "docCNPJ.IdCNPJ", "docCNPJ.CNPJ"
            };

            sql.Append($"SELECT {string.Join(", ", colunas.ToArray())} ");
            
            sql.Append("FROM EmissaoSelo ems (NOLOCK) ");
            sql.Append("INNER JOIN PedidoSelo ps (NOLOCK) on ps.Id = ems.PedidoSelo ");
            sql.Append("INNER JOIN TipoSelo ts (NOLOCK) on ts.Id = ps.TipoSelo ");
            sql.Append("INNER JOIN Unidade u (NOLOCK) on u.Id = ps.Unidade ");
            sql.Append("LEFT JOIN Endereco ue (NOLOCK) on ue.Id = u.Endereco ");
            sql.Append("INNER JOIN Cliente c (NOLOCK) on c.Id = ps.Cliente ");
            sql.Append("LEFT JOIN Pessoa p (NOLOCK) on p.Id = c.Pessoa ");
            sql.Append("LEFT JOIN PessoaEndereco pe (NOLOCK) on pe.Pessoa = p.Id ");
            sql.Append("LEFT JOIN Endereco e (NOLOCK) on e.Id = pe.Endereco_id ");
            sql.Append("LEFT JOIN Cidade ci (NOLOCK) on ci.Id = e.Cidade_id ");
            sql.Append("LEFT JOIN Estado es (NOLOCK) on es.Id = ci.Estado_id ");
            
            sql.Append("OUTER APPLY ( ");
            sql.Append("    SELECT TOP 1 d.Id IdCPF, d.Numero CPF ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK) ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 2 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero ");
            sql.Append("    ORDER BY d.Id desc ");
            sql.Append(" ) docCPF ");

            sql.Append("OUTER APPLY ( ");
            sql.Append("    SELECT TOP 1 d.Id IdCNPJ, d.Numero CNPJ ");
            sql.Append("    FROM PessoaDocumento pd (NOLOCK) ");
            sql.Append("    INNER JOIN Documento d (NOLOCK) on d.Id = pd.Documento_id ");
            sql.Append("    WHERE d.Numero is not null and d.Tipo = 3 and pd.Pessoa = p.Id ");
            sql.Append("    GROUP BY d.Id, d.Numero ");
            sql.Append("    ORDER BY d.Id desc ");
            sql.Append(" ) docCNPJ ");
            
            sql.Append("WHERE 1=1 ");

            if (idEmissao != null && idEmissao != 0)
                sql.Append($" AND e.Id = {idEmissao} ");
            else
            {
                if (idUnidade != null && idUnidade != 0)
                    sql.Append($" AND u.Id = {idUnidade} ");

                if (idCliente != null && idCliente != 0)
                    sql.Append($" AND c.Id = {idCliente} ");

                if (status != null)
                    sql.Append($" AND e.StatusSelo = {(int)status.Value} ");

                if (idTipoSelo != null && idTipoSelo != 0)
                    sql.Append($" AND ts.Id = {idTipoSelo} ");
            }

            sql.Append($" GROUP BY {string.Join(",", colunas.Select(x => (x.Contains(" ") ? x.Split(' ').First() : x)).ToList())} ");

            var query = Session.CreateSQLQuery(sql.ToString());

            return ConverterResultadoPesquisaEmObjetoSimples(query.List(), colunas)?.ToList() ?? new List<EmissaoSelo>();
        }

        public IList<EmissaoSelo> ConverterResultadoPesquisaEmObjetoSimples(IList results, List<string> colunas)
        {
            var lista = new List<EmissaoSelo>();
            foreach (object[] p in results)
            {
                if (lista.Any(x => x.Id == Convert.ToInt32(p[colunas.IndexOf("e.Id EmissaoId")].ToString())))
                    continue;

                var item = new EmissaoSelo
                {
                    Id = Convert.ToInt32(p[colunas.IndexOf("e.Id EmissaoId")].ToString()),
                    DataInsercao = p[colunas.IndexOf("e.DataInsercao EmissaoDataInsercao")]?.ToString() == null ? DateTime.Now : Convert.ToDateTime(p[colunas.IndexOf("e.DataInsercao EmissaoDataInsercao")].ToString()),
                    StatusSelo = (StatusSelo)Convert.ToInt32(p[colunas.IndexOf("e.StatusSelo")].ToString()),
                    NumeroLote = p[colunas.IndexOf("e.NumeroLote")]?.ToString() ?? string.Empty,
                    Validade = p[colunas.IndexOf("e.Validade")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : Convert.ToDateTime(p[colunas.IndexOf("e.Validade")].ToString()),
                    EntregaRealizada = p[colunas.IndexOf("e.EntregaRealizada")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("e.EntregaRealizada")]?.ToString()) : false,
                    DataEntrega = p[colunas.IndexOf("e.DataEntrega")]?.ToString() == null ? new DateTime?() : Convert.ToDateTime(p[colunas.IndexOf("e.DataEntrega")].ToString()),
                    
                    PedidoSelo = new PedidoSelo
                    {
                        Id = p[colunas.IndexOf("ps.Id PedidoSeloId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ps.Id PedidoSeloId")].ToString()),
                        TiposPagamento = (TipoPagamentoSelo)Convert.ToInt32(p[colunas.IndexOf("ps.TiposPagamentos")].ToString()),
                        ValidadePedido = p[colunas.IndexOf("ps.ValidadePedido")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : Convert.ToDateTime(p[colunas.IndexOf("ps.ValidadePedido")].ToString()),
                        Quantidade = p[colunas.IndexOf("ps.Quantidade")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ps.Quantidade")].ToString()),
                        DiasVencimento = p[colunas.IndexOf("ps.DiasVencimento")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ps.DiasVencimento")].ToString()),
                        TipoPedidoSelo = (TipoPedidoSelo)Convert.ToInt32(p[colunas.IndexOf("ps.TipoPedidoSelo")].ToString()),
                        DataVencimento = p[colunas.IndexOf("ps.DataVencimento PedidoSeloDataVencimento")]?.ToString() == null ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : Convert.ToDateTime(p[colunas.IndexOf("ps.DataVencimento PedidoSeloDataVencimento")].ToString()),
                        Convenio = new Convenio
                        {
                            Id = p[colunas.IndexOf("ps.Convenio ConvenioId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ps.Convenio ConvenioId")].ToString())
                        },
                        TipoSelo = new TipoSelo
                        {
                            Id = p[colunas.IndexOf("ts.Id TipoSeloId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ts.Id TipoSeloId")].ToString()),
                            Nome = p[colunas.IndexOf("ts.Nome TipoSeloNome")]?.ToString() ?? string.Empty,
                            Valor = p[colunas.IndexOf("ts.Valor TipoSeloValor")]?.ToString() == null ? 0 : Convert.ToDecimal(p[colunas.IndexOf("ts.Valor TipoSeloValor")].ToString()),
                            Ativo = p[colunas.IndexOf("ts.Ativo TipoSeloAtivo")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("ts.Ativo TipoSeloAtivo")]?.ToString()) : false,
                            ParametroSelo = (ParametroSelo)Convert.ToInt32(p[colunas.IndexOf("ts.ParametroSelo TipoSeloParametroSelo")].ToString()),
                            ComValidade = p[colunas.IndexOf("ts.ComValidade TipoSeloComValidade")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("ts.ComValidade TipoSeloComValidade")]?.ToString()) : false,
                            PagarHorasAdicionais = p[colunas.IndexOf("ts.PagarHorasAdicionais TipoSeloPagarHorasAdicionais")]?.ToString() != null ? Convert.ToBoolean(p[colunas.IndexOf("ts.PagarHorasAdicionais TipoSeloPagarHorasAdicionais")]?.ToString()) : false
                        },
                        Unidade = new Unidade
                        {
                            Id = p[colunas.IndexOf("u.Id UnidadeId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("u.Id UnidadeId")].ToString()),
                            Codigo = p[colunas.IndexOf("u.Codigo CodigoUnidade")]?.ToString() ?? string.Empty,
                            Nome = p[colunas.IndexOf("u.Nome NomeUnidade")]?.ToString() ?? string.Empty,
                            Endereco = new Endereco
                            {
                                Id = p[colunas.IndexOf("ue.Id IdUnidadeEndereco")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("ue.Id IdUnidadeEndereco")].ToString()),
                                Cep = p[colunas.IndexOf("ue.Cep UnidadeCep")]?.ToString() ?? string.Empty,
                                Logradouro = p[colunas.IndexOf("ue.Logradouro UnidadeLogradouro")]?.ToString() ?? string.Empty,
                                Numero = p[colunas.IndexOf("ue.Numero UnidadeNumeroEndereco")]?.ToString() ?? string.Empty,
                                Complemento = p[colunas.IndexOf("ue.Complemento UnidadeComplemento")]?.ToString() ?? string.Empty,
                                Bairro = p[colunas.IndexOf("ue.Bairro UnidadeBairro")]?.ToString() ?? string.Empty
                            }
                        },
                        Cliente = new Cliente
                        {
                            Id = p[colunas.IndexOf("c.Id ClienteId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Id ClienteId")].ToString()),
                            RazaoSocial = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() ?? string.Empty,
                            NomeFantasia = p[colunas.IndexOf("c.NomeFantasia")]?.ToString() ?? string.Empty,
                            TipoPessoa = p[colunas.IndexOf("c.RazaoSocial")]?.ToString() != null || p[colunas.IndexOf("c.NomeFantasia")]?.ToString() != null ? TipoPessoa.Juridica : TipoPessoa.Fisica,
                            Pessoa = new Pessoa
                            {
                                Id = p[colunas.IndexOf("c.Pessoa PessoaId")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("c.Pessoa PessoaId")].ToString()),
                                Nome = p[colunas.IndexOf("p.Nome NomePessoa")]?.ToString() ?? string.Empty,

                                Documentos = new List<PessoaDocumento>
                            {
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cpf,
                                    Documento = new Documento(TipoDocumento.Cpf,
                                                                p[colunas.IndexOf("docCPF.CPF")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("docCPF.IdCPF")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("docCPF.IdCPF")].ToString()),
                                                                null, null, null, false)
                                },
                                new PessoaDocumento
                                {
                                    Tipo = TipoDocumento.Cnpj,
                                    Documento = new Documento(TipoDocumento.Cnpj,
                                                                p[colunas.IndexOf("docCNPJ.CNPJ")]?.ToString() ?? string.Empty,
                                                                DateTime.MinValue,
                                                                p[colunas.IndexOf("docCNPJ.IdCNPJ")]?.ToString() == null ? 0 : Convert.ToInt32(p[colunas.IndexOf("docCNPJ.IdCNPJ")].ToString()),
                                                                null, null, null, false)
                                }
                            }
                            }
                        }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}