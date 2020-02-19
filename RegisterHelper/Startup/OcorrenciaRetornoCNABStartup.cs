using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Microsoft.Practices.ServiceLocation;

namespace InitializerHelper.Startup
{
    public static class OcorrenciaRetornoCNABStartup
    {
        public static void Start()
        {
            AdicionaOcorrenciaDefault();
        }

        private static void AdicionaOcorrenciaDefault()
        {
            var _ocorrenciaRepositorio = ServiceLocator.Current.GetInstance<IOcorrenciaRetornoCNABRepositorio>();
            var codigosOcorrenciasExistentes = _ocorrenciaRepositorio.List()?.Select(x => x.Codigo)?.ToList();

            var ocorrencias = new List<OcorrenciaRetornoCNAB>();
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "00", Descricao = "Crédito ou Débito Efetivado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "01", Descricao = "Insuficiência de Fundos -Débito Não Efetuado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "02", Descricao = "Crédito ou Débito Cancelado pelo Pagador / Credor" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "03", Descricao = "Débito Autorizado pela Agência - Efetuado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AA", Descricao = "Controle Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AB", Descricao = "Tipo de Operação Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AC", Descricao = "Tipo de Serviço Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AD", Descricao = "Forma de Lançamento Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AE", Descricao = "Tipo / Número de Inscrição Inválido(gerado na crítica ou para informar rejeição) *" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AF", Descricao = "Código de Convênio Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AG", Descricao = "Agência / Conta Corrente / DV Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AH", Descricao = "Número Seqüencial do Registro no Lote Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AI", Descricao = "Código de Segmento de Detalhe Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AJ", Descricao = "Tipo de Movimento Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AK", Descricao = "Código da Câmara de Compensação do Banco do Favorecido / Depositário Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AL", Descricao = "Código do Banco do Favorecido, Instituição de Pagamento ou Depositário Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AM", Descricao = "Agência Mantenedora da Conta Corrente do Favorecido Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AN", Descricao = "Conta Corrente / DV / Conta de Pagamento do Favorecido Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AO", Descricao = "Nome do Favorecido não Informado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AP", Descricao = "Data Lançamento Inválida/ Vencimento Inválido / Data de Pagamento não permitda." });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AQ", Descricao = "Tipo / Quantidade da Moeda Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AR", Descricao = "Valor do Lançamento Inválido/ Divergente" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AS", Descricao = "Aviso ao Favorecido -Identificação Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AT", Descricao = "Tipo / Número de Inscrição do Favorecido / Contribuinte Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AU", Descricao = "Logradouro do Favorecido não Informado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AV", Descricao = "Número do Local do Favorecido não Informado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AW", Descricao = "Cidade do Favorecido não Informada" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AX", Descricao = "CEP / Complemento do Favorecido Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AY", Descricao = "Sigla do Estado do Favorecido Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "AZ", Descricao = "Código / Nome do Banco Depositário Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BA", Descricao = "Código / Nome da Agência Depositário não Informado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BB", Descricao = "Número do Documento Inválido(Seu Número)" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BC", Descricao = "Nosso Número Invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BD", Descricao = "Inclusão Efetuada com Sucesso" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BE", Descricao = "Alteração Efetuada com Sucesso" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BF", Descricao = "Exclusão Efetuada com Sucesso" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "BG", Descricao = "Agência / Conta Impedida Legalmente" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "B1", Descricao = "Bloqueado Pendente de Autorização" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "B3", Descricao = "Bloqueado pelo cliente" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "B4", Descricao = "Bloqueado pela captura de titulo da cobrança" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "B8", Descricao = "Bloqueado pela Validação de Tributos" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CA", Descricao = "Código de barras -Código do Banco Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CB", Descricao = "Código de barras -Código da Moeda Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CC", Descricao = "Código de barras -Dígito Verificador Geral Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CD", Descricao = "Código de barras -Valor do Título Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CE", Descricao = "Código de barras -Campo Livre Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CF", Descricao = "Valor do Documento / Principal / menor que o minimo Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CH", Descricao = "Valor do Desconto Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CI", Descricao = "Valor de Mora Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CJ", Descricao = "Valor da Multa Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CK", Descricao = "Valor do IR Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CL", Descricao = "Valor do ISS Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CG", Descricao = "Valor do Abatimento inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CM", Descricao = "Valor do IOF Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CN", Descricao = "Valor de Outras Deduções Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "CO", Descricao = "Valor de Outros Acréscimos Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HA", Descricao = "Lote Não Aceito" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HB", Descricao = "Inscrição da Empresa Inválida para o Contrato" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HC", Descricao = "Convênio com a Empresa Inexistente/ Inválido para o Contrato" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HD", Descricao = "Agência / Conta Corrente da Empresa Inexistente/ Inválida para o Contrato" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HE", Descricao = "Tipo de Serviço Inválido para o Contrato" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HF", Descricao = "Conta Corrente da Empresa com Saldo Insuficiente" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HG", Descricao = "Lote de Serviço fora de Seqüência" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HH", Descricao = "Lote de Serviço Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HI", Descricao = "Arquivo não aceito" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HJ", Descricao = "Tipo de Registro Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HL", Descricao = "Versão de Layout Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "HU", Descricao = "Hora de Envio Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IA", Descricao = "Pagamento exclusive em Cartório." });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IJ", Descricao = "Competência ou Período de Referencia ou Numero da Parcela invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IL", Descricao = "Codigo Pagamento / Receita não numérico ou com zeros" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IM", Descricao = "Município Invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IN", Descricao = "Numero Declaração Invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IO", Descricao = "Numero Etiqueta invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IP", Descricao = "Numero Notificação invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IQ", Descricao = "Inscrição Estadual invalida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IR", Descricao = "Divida Ativa Invalida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IS", Descricao = "Valor Honorários ou Outros Acréscimos invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IT", Descricao = "Período Apuração invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IU", Descricao = "Valor ou Percentual da Receita invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "IV", Descricao = "Numero Referencia invalida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "SC", Descricao = "Validação parcial" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "TA", Descricao = "Lote não Aceito -Totais do Lote com Diferença" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "XB", Descricao = "Número de Inscrição do Contribuinte Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "XC", Descricao = "Código do Pagamento ou Competência ou Número de Inscrição Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "XF", Descricao = "Código do Pagamento ou Competência não Numérico ou igual á zeros" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YA", Descricao = "Título não Encontrado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YB", Descricao = "Identificação Registro Opcional Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YC", Descricao = "Código Padrão Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YD", Descricao = "Código de Ocorrência Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YE", Descricao = "Complemento de Ocorrência Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "YF", Descricao = "Alegação já Informada" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZA", Descricao = "Transferência Devolvida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZB", Descricao = "Transferência mesma titularidade não permitida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZC", Descricao = "Código pagamento Tributo inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZD", Descricao = "Competência Inválida" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZE", Descricao = "Título Bloqueado na base" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZF", Descricao = "Sistema em Contingência – Titulo com valor maior que referência" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZG", Descricao = "Sistema em Contingência – Título vencido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZH", Descricao = "Sistema em contingência -Título indexado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZI", Descricao = "Beneficiário divergente" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZJ", Descricao = "Limite de pagamentos parciais excedido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZK", Descricao = "Título já liquidado" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZT", Descricao = "Valor outras entidades inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZU", Descricao = "Sistema Origem Inválido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZW", Descricao = "Banco Destino não recebe DOC" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZX", Descricao = "Banco Destino inoperante para DOC" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZY", Descricao = "Código do Histórico de Credito Invalido" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "ZV", Descricao = "Autorização iniciada no Internet Banking" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z0", Descricao = "Conta com bloqueio*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z1", Descricao = "Conta fechada.É necessário ativar a conta*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z2", Descricao = "Conta com movimento controlado*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z3", Descricao = "Conta cancelada*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z4", Descricao = "Registro inconsistente(Título) *" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z5", Descricao = "Apresentação indevida(Título) *" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z6", Descricao = "Dados do destinatário inválidos*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z7", Descricao = "Agência ou conta destinatária do crédito inválida*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z8", Descricao = "Divergência na titularidade*" });
            ocorrencias.Add(new OcorrenciaRetornoCNAB { Codigo = "Z9", Descricao = "Conta destinatária do crédito encerrada*" });

            if (codigosOcorrenciasExistentes.Count > 0)
            {
                ocorrencias.RemoveAll(x => codigosOcorrenciasExistentes.Contains(x.Codigo));
            }

            _ocorrenciaRepositorio.Save(ocorrencias);
        }
    }
}