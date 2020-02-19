using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Extensions;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;

namespace Repositorio
{
    public class PessoaRepositorio : NHibRepository<Pessoa>, IPessoaRepositorio
    {

        #region Base Itens

        private string[] ColunasInformacoesBase =
        {
            /*Pessoa*/
            "Pessoa_Id", "Pessoa_Nome", "Pessoa_Sexo", "Pessoa_DataNascimento", "Pessoa_DataInsercao", "Pessoa_Funcao",
            "Pessoa_Indicacao", "Pessoa_Programa",
            "Pessoa_Endereco_Id", "Pessoa_StatusLead", "Pessoa_DataAtualizacao", "Pessoa_TreinamentoBasico",
            "Pessoa_RegulamentoAceito", "Pessoa_AdesaoTurbo_Id",
            "Pessoa_IndicacaoCPFCNPJ", "Pessoa_CPFCNPJIdentificado", "Pessoa_Observacao", "Pessoa_RazaoRecusa_Id", "Pessoa_DataDesligamento",

            /*Loja*/
            "Pessoa", "Loja_Id", "Loja_Nome", "Loja_CNPJ", "Loja_DataInsercao", "Loja_InscricaoEstadual", "Loja_RazaoSocial",
            "Loja_Endereco_Id", "Loja_GrupoLoja_Id", "Loja_PossuiPicShowTurbo",

            /*Regional*/
            "Regional_Id", "Regional_Nome",

            /*RG*/
            "Documento_Id (RG)", "RG_Tipo", "RG_Numero",

            /*CPF*/
            "Documento_Id (CPF)", "CPF_Tipo", "CPF_Numero",

            /*CNPJ*/
            "Documento_Id (CNPJ)", "CNPJ_Tipo", "CNPJ_Numero",

            /*Endereco*/
            "Endereco_Id", "Endereco_Cep", "Endereco_Tipo", "Endereco_Logradouro", "Endereco_Numero",
            "Endereco_Complemento", "Endereco_Bairro", "Endereco_Descricao", "Endereco_Latitude", "Endereco_Longitude",
            "Endereco_DataInsercao",

            /*Cidade*/
            "Cidade_Id", "Cidade_Descricao", "Cidade_DataInsercao",

            /*Estado*/
            "Estado_Id", "Estado_Descricao", "Estado_Sigla", "Estado_DataInsercao", "Estado_AssociaMaterialCampanha",

            /*Pais*/
            "Pais_Id",

            /*Email*/
            "Contato_Id (Email)", "Contato_Tipo (Email)", "Email",

            /*Residencial*/
            "Contato_Id (Residencial)", "Contato_Tipo (Residencial)", "Contato_Numero (Residencial)",

            /*Celular*/
            "Contato_Id (Celular)", "Contato_Tipo (Celular)", "Contato_Numero (Celular)"
        };

        #endregion
        public PessoaRepositorio(NHibContext context)
            : base(context)
        {
        }

        public List<Pessoa> PesquisarComFiltro(string nome, string CPF)
        {
            var sql = new StringBuilder();

           sql.Append(@"    SELECT * 
                            FROM
                                (SELECT
    	                            P.ID, 
    	                            P.NOME,
    	                            REPLACE(REPLACE(REPLACE((SELECT DISTINCT NUMERO FROM DOCUMENTO D INNER JOIN PESSOADOCUMENTO PD ON D.ID = PD.DOCUMENTO_ID WHERE D.TIPO = 2 AND PD.PESSOA = P.ID ), '-', '' ), '.', '' ), '/', '' ) AS CPF, 
    	                            (SELECT NUMERO FROM DOCUMENTO D INNER JOIN PESSOADOCUMENTO PD ON D.ID = PD.DOCUMENTO_ID WHERE D.TIPO = 1 AND PD.PESSOA = P.ID ) AS RG, 
    	                            P.SEXO, 
    	                            P.DATANASCIMENTO, 
    	                            E.CEP, 
    	                            E.LOGRADOURO,
    	                            E.NUMERO AS NUMEROENDERECO, 
    	                            E.COMPLEMENTO, 
    	                            E.BAIRRO, 
    	                            C.DESCRICAO AS CIDADEDESCRICAO, 
    	                            EST.SIGLA
                                FROM PESSOA AS P
                                LEFT JOIN PESSOAENDERECO AS PE ON PE.PESSOA = P.ID
                                LEFT JOIN ENDERECO AS E ON E.ID = PE.ENDERECO_ID
                                LEFT JOIN CIDADE AS C ON C.ID = E.CIDADE_ID
                                LEFT JOIN ESTADO AS EST ON EST.ID = C.ESTADO_ID) AS Pessoas 
                            WHERE 1=1");

            if (!string.IsNullOrEmpty(nome))
                sql.Append($" AND NOME like '%{nome}%'");

            if (!string.IsNullOrEmpty(CPF))
                sql.Append($" AND CPF like '%{CPF.Replace(".", "").Replace("-", "")}%'");

            var query = Session.CreateSQLQuery(sql.ToString());

            return ConverterResultadoPesquisaEmObjeto(query.List())?.ToList() ?? new List<Pessoa>();
        }
        
        public List<Pessoa> PesquisarComFiltroNaoDevedores(string nome, string CPF)
        {
            var sql = new StringBuilder();

           sql.Append($@"    SELECT * 
                            FROM
                                (SELECT DISTINCT 
                                    P.ID, 
                                    P.NOME,
                                    REPLACE(REPLACE(REPLACE((SELECT {(IsMySql() ? "DISTINCT" : "TOP 1")} NUMERO FROM DOCUMENTO D INNER JOIN PESSOADOCUMENTO PD ON D.ID = PD.DOCUMENTO_ID WHERE D.TIPO = 2 AND PD.PESSOA = P.ID {(IsMySql() ? "LIMIT 1" : string.Empty)}), '-', '' ), '.', '' ), '/', '' ) AS CPF,
                                    (SELECT {(IsMySql() ? "DISTINCT" : "TOP 1")} NUMERO FROM DOCUMENTO D INNER JOIN PESSOADOCUMENTO PD ON D.ID = PD.DOCUMENTO_ID WHERE D.TIPO = 1 AND PD.PESSOA = P.ID {(IsMySql() ? "LIMIT 1" : string.Empty)}) AS RG, 
                                    P.SEXO, 
                                    P.DATANASCIMENTO, 
                                    E.CEP, 
                                    E.LOGRADOURO,
                                    E.NUMERO AS NUMEROENDERECO, 
                                    E.COMPLEMENTO, 
                                    E.BAIRRO, 
                                    C.DESCRICAO AS CIDADEDESCRICAO, 
                                    EST.SIGLA,
                                    P.ATIVO
                                FROM PESSOA AS P
                                    LEFT JOIN PESSOAENDERECO AS PE ON PE.PESSOA = P.ID
                                    LEFT JOIN ENDERECO AS E ON E.ID = PE.ENDERECO_ID
                                    LEFT JOIN CIDADE AS C ON C.ID = E.CIDADE_ID
                                    LEFT JOIN ESTADO AS EST ON EST.ID = C.ESTADO_ID
                                WHERE 1=1 AND P.ID NOT IN (SELECT PESSOA  FROM DEVEDOR)) AS Pessoas
                            WHERE 1=1");

            if (!string.IsNullOrEmpty(nome))
                sql.Append($" AND NOME like '%{nome}%'");

            if (!string.IsNullOrEmpty(CPF))
                sql.Append($" AND CPF like '%{CPF.Replace(".", "").Replace("-", "")}%'");

            var query = Session.CreateSQLQuery(sql.ToString());

            return ConverterResultadoPesquisaEmObjeto(query.List())?.ToList() ?? new List<Pessoa>();
        }

        public IList<Pessoa> ConverterResultadoPesquisaEmObjeto(IList results)
        {
            var pessoas = new List<Pessoa>();
            foreach (object[] p in results)
            {
                var itemP = new Pessoa
                {
                    Id = p.Length > 0 && p[0] != null ? Convert.ToInt32(p[0] ?? 0) : 0,
                    Nome = p.Length > 1 && p[1] != null ? p[1]?.ToString() ?? string.Empty : string.Empty,
                    Sexo = p.Length > 4 && p[4] != null ? p[4]?.ToString() ?? string.Empty : string.Empty,
                    DataNascimento = p.Length > 5 && p[5] != null ? DateTime.Parse(p[5]?.ToString()) : DateTime.Now.Date,
                        
                    Documentos = new List<PessoaDocumento> {
                        new PessoaDocumento { Documento =
                            new Documento(TipoDocumento.Cpf, p.Length > 2 && p[2] != null ? p[2]?.ToString() ?? string.Empty : string.Empty, DateTime.Now)
                        },
                        new PessoaDocumento { Documento =
                            new Documento(TipoDocumento.Rg, p.Length > 3 && p[3] != null ? p[3]?.ToString() ?? string.Empty : string.Empty, DateTime.Now)
                        }
                    },
                    Enderecos = new List<PessoaEndereco>
                    {
                        new PessoaEndereco { Endereco = 
                            new Endereco
                            {
                                Cep = p.Length > 6 && p[6] != null ? p[6]?.ToString() ?? string.Empty : string.Empty,
                                Logradouro = p.Length > 7 && p[7] != null ? p[7]?.ToString() : string.Empty,
                                Numero = p.Length > 8 && p[8] != null ? p[8]?.ToString() : string.Empty,
                                Complemento = p.Length > 9 && p[9] != null ? p[9]?.ToString() : string.Empty,
                                Bairro = p.Length > 10 && p[10] != null ? p[10]?.ToString() : string.Empty,
                                Cidade = new Cidade
                                {
                                    Descricao = p.Length > 11 && p[11] != null ? p[11]?.ToString() : string.Empty,
                                    Estado = new Estado
                                    {
                                        Sigla = p.Length > 12 && p[12] != null ? p[12]?.ToString() : string.Empty
                                    }
                                }
                            }
                        }
                    },
                    Ativo = p.Length > 13 && p[13] != null ? Convert.ToBoolean(p[13]?.ToString()) : false
                };
                pessoas.Add(itemP);
            }

            return pessoas;
        }
    }
}
