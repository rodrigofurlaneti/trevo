using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Core.Extensions;

namespace Entidade.Uteis
{
    public class Constantes
    {
        public const int NUMERO_MAXIMO_PAGINACAO = 10;
        public const string MascaraMonetaria = "#,##0.00";
        public const string MascaraNossoNumeroDefault = "#########000";
        public const string MascaraNossoNumeroSantander = "000000000000";

        public static string FormataNossoNumeroBanco(int nossoNumero, string banco = "0")
        {
            string nossoNumeroFormatado;
            var codigoBanco = 0;
            if (int.TryParse(banco, out int result))
                codigoBanco = result;

            switch (codigoBanco)
            {
                case 33:
                    nossoNumeroFormatado = nossoNumero.ToString().PadLeftTrunc(7, '0');
                    break;
                default:
                    nossoNumeroFormatado = nossoNumero.ToString(MascaraNossoNumeroDefault);
                    break;
            }
            return nossoNumeroFormatado;
        }
    }

    public enum StatusCheckList
    {
        Aberto = 1,
        Finalizado = 2
    }
    public enum TipoEndereco
    {
        [Description("Residencial")]
        Residencial = 1,
        [Description("Comercial")]
        Comercial = 2
    }

    public enum TipoDocumento
    {
        Rg = 1,
        Cpf = 2,
        Cnpj = 3,
        Ie = 4,
        Cfm = 5,
        TituloEleitoral = 6,
        Ctps = 7,
        Pis = 8,
        Im = 9,
        Cnh = 10
    }

    public enum TipoContato
    {
        Email = 1,
        Residencial = 2,
        Celular = 3,
        Recado = 4,
        Comercial = 5,
        Fax = 6,
        OutroEmail = 7
    }

    public enum Funcao
    {
        [Description("Gerente Regional")]
        GerenteRegional = 1,
        [Description("Consultor de Negócios")]
        ConsultorNegocios = 2,
        [Description("Franqueado")]
        Franqueado = 3,
        [Description("Consultor de Venda")]
        ConsultorVenda = 4,
        [Description("Revendedor")]
        Revendedor = 5,
        [Description("Trade Marketing")]
        TradeMarketing = 6,
        [Description("Comunicação")]
        Comunicacao = 7,
        [Description("Criação de Arte")]
        CriacaoArte = 8,
        [Description("Lead Revendedor")]
        LeadRevendedor = 9,
        [Description("Supervisor")]
        Supervisor = 10,
        [Description("Responsável Por Máquina de Cartão")]
        Responsavel = 11
    }

    public enum TipoCampanha
    {
        [Description("A5")]
        A5 = 1,
        [Description("Adesivo de Parede")]
        AdesivoParede = 2,
        [Description("Banner")]
        Banner = 3,
        [Description("Cardápio")]
        Cardapio = 4,
        [Description("Cartão Fidelidade")]
        CartaoFidelidade = 5,
        [Description("Cavalete")]
        Cavalete = 6,
        [Description("Checkout")]
        Checkout = 7,
        [Description("Display")]
        Display = 8,
        [Description("Faixa Degustação")]
        FaixaDegustacao = 9,
        [Description("Folder")]
        Folder = 10,
        [Description("Gráfico Vitrine")]
        GraficoVitrine = 11,
        [Description("Outdoor")]
        Outdoor = 12,
        [Description("Panfleto")]
        Panfleto = 13,
        [Description("Precificador")]
        Precificador = 14,
        [Description("Quadro")]
        Quadro = 15,
        [Description("Régua")]
        Regua = 16,
        [Description("Voucher")]
        Voucher = 17,
        [Description("Outro")]
        Outro = 18
    }

    public enum DestaqueCampanha
    {
        [Description("Campanha")]
        Campanha = 1,
        [Description("Inauguração")]
        Inauguracao = 2,
        [Description("Institucional")]
        Institucional = 3,
        [Description("Promoção")]
        Promocao = 4,
        [Description("Sem Destaque")]
        SemDestaque = 5,
        [Description("Venda Corporativa")]
        VendaCorporativa = 6,
        [Description("Venda Direta")]
        VendaDireta = 7
    }

    public enum Entidades
    {        
        [Description("Tabela de Preço")]
        TabelaPreco = 1,
        [Description("Horário de Unidade")]
        HorarioUnidade = 2,
        [Description("Parâmetro de Equipe")]
        ParametroEquipe = 3,
        [Description("Desconto")]
        Desconto = 4,
        [Description("Pedido de Selo")]
        PedidoSelo = 5,
        [Description("Tabela Preço Mensalista")]
        TabelaPrecoMensalista = 6,
        [Description("Tabela Preço Avulso")]
        TabelaPrecoAvulso = 7,
        [Description("Pedido Locação")]
        PedidoLocacao = 8,
        [Description("Material")]
        Material = 9,
        [Description("Necessidade de Compra")]
        NecessidadeCompra = 10,
        [Description("Cotação")]
        Cotacao = 11,
        [Description("Pedido de Compra")]
        PedidoCompra = 12,
        [Description("Contas a Pagar")]
        ContasAPagar = 13,
        [Description("Abertura OIS")]
        OIS = 14,
        [Description("Orçamento Sinistro")]
        OrcamentoSinistro = 15,
        [Description("Orçamento Sinistro Cotação")]
        OrcamentoSinistroCotacao = 16,
        [Description("Lancamento de Cobrança")]
        LancamentoCobranca = 17,
        [Description("Retirada do Cofre")]
        RetiradaCofre = 18,
        [Description("Contrato Mensalista")]
        ContratoMensalista = 19,
        [Description("Desbloqueio de Referência")]
        DesbloqueioReferencia = 20,
        [Description("Baixa Manual")]
        BaixaManual = 21,
        [Description("Leitura CNAB")]
        LeituraCNAB = 22,
        [Description("Ocorrência atribuída")]
        OcorrenciaAtribuida = 23
    }

    public enum ExibicaoArte
    {
        [Description("Frente")]
        Frente = 1,
        [Description("Frente e Verso")]
        FrenteVerso = 2,
        [Description("Verso")]
        Verso = 3
    }

    public enum UsoLoja
    {
        [Description("Externo")]
        Externo = 1,
        [Description("Interno")]
        Interno = 2
    }

    public enum ExibirPlanta
    {
        [Description("Sim")]
        Sim = 1,
        [Description("Nao")]
        Nao = 0
    }

    public enum ExibirFotoLocal
    {
        [Description("Sim")]
        Sim = 1,
        [Description("Nao")]
        Nao = 0
    }

	public enum StatusFuncionario
	{
        [Display(Name = "Ativo")]
        Ativo = 1,
        [Display(Name = "Inativo")]
        Inativo = 0
    }

    public enum TipoEscalaFuncionario
    {
        [Display(Name = "Nenhum")]
        Nenhum,
        [Display(Name = "DozeTrintaSeis")]
        DozeTrintaSeis,
        [Display(Name = "Compensação")]
        Compensacao,
        [Display(Name = "Noturno")]
        Noturno
    }

    public enum SituacaoSageFuncionario
    {
        [Display(Name = "Ativo")]
        Ativo,
        [Display(Name = "Demitido")]
        Demitido,
        [Display(Name = "Afastado")]
        Afastado
    }

    public enum StatusHistorico
    {
        [Description("Informação do Usuário")]
        InformacaoUsuario = 0,

        //Pedido de Criação de Arte
        [Description("Salvar Rascunho")]
        SalvarRascunho = 1,
        [Description("Novo Pedido")]
        NovoPedido = 2,
        [Description("Em Análise pelo Consultor de Negócio")]
        EmAnaliseConsultorNegocio = 3,
        [Description("Aprovado pelo Consultor de Negócio")]
        AprovadoConsultorNegocio = 4,
        [Description("Em Análise por Trade Mkt")]
        EmAnaliseMkt = 5,
        [Description("Em Análise por Comunicação")]
        EmAnaliseComunicacao = 6,
        [Description("Aprovado por Trade Mkt")]
        AprovadoMkt = 7,
        [Description("Aprovado por Comunicação")]
        AprovadoComunicacao = 8,
        [Description("Pedido Enviado para Criação")]
        PedidoEnviadoCriacao = 9,
        [Description("Cancelado")]
        Cancelado = 10,

        //Criacao Arte
        [Description("Arte em Desenvolvimento pela Criação")]
        ArteDesenvolvimentoCriacao = 11,
        [Description("Arte em Aprovação pela Comunicação")]
        ArteEmAprovacaoComunicacao = 12,
        [Description("Arte Aprovada pela Comunicação")]
        ArteAprovadaComunicacao = 13,
        [Description("Arte em Aprovação pelo Trade Mkt")]
        ArteEmAprovacaoMkt = 14,
        [Description("Arte Aprovada pelo Trade Mkt")]
        ArteAprovadaMkt = 15,
        [Description("Arte em Aprovação pelo Consultor de Negócio")]
        ArteEmAprovacaoConsultorNegocio = 16,
        [Description("Arte Aprovada pelo Consultor de Negócio")]
        ArteAprovadaConsultorNegocio = 17,
        [Description("Pendente de Finalização da Arte Final")]
        PendenteFinalizacaoArteFinal = 18,
        [Description("Arte Finalizada")]
        ArteFinalizada = 19,
        [Description("Não Aprovar")]
        NaoAprovar = 20
    }

    public enum AcaoPermissao
    {
        Novo,
        Incluir,
        Alterar,
        Excluir,
        Cancelar,
        NaoAprovar,
        Aprovar,
        IniciarAtividade,
        Reavaliar,
        EnviarParaAnalise,
        Upload,
        Download,
        Finalizar
    }

    public enum TipoArquivo
    {
        [Description("Arquivo")]
        Arquivo = 1,
        [Description("Thumbnail")]
        Thumbnail = 2,
        [Description("Foto")]
        Foto = 3,
        [Description("CNAB Retorno")]
        CNABRetorno = 4,
    }

    public enum TipoPrograma
    {
        [Description("Meu Show"), Display(Name = "Meu Show")]
        MeuShow = 1,
        [Description("Vip Show"), Display(Name = "Vip Show")]
        VipShow = 2,
        [Description("Pic Show"), Display(Name = "Pic Show")]
        PicShow = 3
    }

    public enum StatusLead
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Recusado")]
        Recusado = 3,
    }

    public enum TipoCategoria
    {
        [Description("Produtividade"), Display(Name = "Produtividade")]
        Produtividade = 1,
        [Description("Frequência"), Display(Name = "Frequência")]
        Atividade = 2,
        [Description("Indicação"), Display(Name = "Indicação")]
        Indicacao = 3,
        [Description("Atingimento da Meta"), Display(Name = "Atingimento da Meta")]
        Meta = 4,
        [Description("Venda nos Canais"), Display(Name = "Venda nos Canais")]
        VendaCanal = 5,
        [Description("Treinamentos"), Display(Name = "Treinamentos")]
        Treinamento = 6,
        [Description("Avaliação de Postura"), Display(Name = "Avaliação de Postura")]
        Postura = 7,
        [Description("PEF"), Display(Name = "PEF")]
        Pef = 8
    }

    public enum TipoRanking
    {
        Semanal = 1,
        Quinzenal = 2,
        Mensal = 3,
        Semestral = 4,
        Anual = 5
    }

    public enum ChecarVigencia
    {
        SemChecagem = 0,
        Ok = 1,
        Alerta = 2,
        Vencido = 3
    }

    public enum FormatoExportacao
    {
        [Description("Excel")]
        Excel = 1,
        [Description("Delimitador")]
        Delimitador = 2,
        [Description("Posicional (TXT)")]
        Posicional = 3
    }

    public enum FormatoCarga
    {
        [Description("Parcela com Saldo Unico"), Display(Name = "Parcela Saldo Unico")]
        ParcSaldoUnico = 1,
        [Description("Parcela a Parcela"), Display(Name = "Parcela a Parcela")]
        ParcParcela = 2,
        [Description("Parcela Incremental"), Display(Name = "Parcela Incremental")]
        ParcIncremental = 3
    }

    public enum StatusNecessidadeCompra
    {
        [Description("Necessidade de Compra")]
        NecessidadeCompra,
        [Description("Aguardando Cotação")]
        AguardandoCotacao,
        [Description("Aguardando Aprovação da Cotação")]
        AguardandoAprovacaoCotacao,
        [Description("Aguardando Pedido")]
        AguardandoPedido,
        [Description("Aguardando Entrega do Pedido")]
        AguardandoEntregaPedido,
        [Description("Entrega Realizada")]
        EntregaRealizada,
        [Description("Reprovada")]
        Reprovado,
        [Description("Cotação Reprovada")]
        CotacaoReprovada
    }

    public enum FormaPagamentoPedidoCompra
    {
        [Description("Cheque")]
        Cheque,
        [Description("Dinheiro")]
        Dinheiro,
        [Description("Cartão")]
        Cartao,
        [Description("Boleto")]
        Boleto
    }

    public enum TipoPagamentoPedidoCompra
    {
        [Description("Pós")]
        Pos,
        [Description("Pré")]
        Pre
    }

    public enum StatusPedidoCompra
    {
        [Description("Nova Emissão")]
        NovaEmissao,
        [Description("Aguardando Aprovação do Pedido")]
        AguardandoAprovacaoPedido,
        [Description("Aguardando Entrega Pedido")]
        AguardandoEntregaPedido,
        [Description("Entrega Realizada")]
        EntregaRealizada,
        [Description("Pedido Reprovado")]
        PedidoReprovado
    }

    public enum StatusCotacao
    {
        [Description("Aguardando Aprovação")]
        AguardandoAprovacao,
        [Description("Aprovado")]
        Aprovado,
        [Description("Reprovado")]
        Reprovado
    }

    public enum FormatoArquivo
    {
        [Description("")]
        Nenhum = 0,
        [Description("Excel"), Display(Name = "Excel")]
        Excel = 1,
        [Description("Delimitador")]
        Delimitador = 2,
        [Description("Posicional")]
        Posicional = 3,
        [Description("Json")]
        Json = 4,
        //[Description("CSV"), Display(Name = "CSV (Parcela Unica)")]
        //CSV = 5,
        //[Description("CSV Enriquecimento"), Display(Name = "CSV (Enriquecimento - Dados do Contrato)")]
        //CSVEnriquecimentoContrato = 6,
        //[Description("CSV Enriquecimento"), Display(Name = "CSV (Enriquecimento - Dados de Contato e Endereco)")]
        //CSVEnriquecimentoContatoEndereco = 7
    }

    public enum TipoValidacao
    {
        [Description("Não Validar"), Display(Name = "Não Validar")]
        NaoValidar = 0,
        [Description("Somente Letras"), Display(Name = "Somente Letras")]
        SomenteLetras = 1,
        [Description("Somente Numeros"), Display(Name = "Somente Numeros")]
        SomenteNumeros = 2,
        [Description("Alfanumerico"), Display(Name = "Alfanumerico")]
        Alfanumerico = 3,
        [Description("Somente Data"), Display(Name = "Somente Data")]
        SomenteData = 4,
        [Description("Data e Hora"), Display(Name = "Data e Hora")]
        DataHora = 5,
        [Description("Somente Hora"), Display(Name = "Somente Hora")]
        Hora = 6
    }

    public enum TipoDado
    {
        Nenhum = 0,
        Alfanumerico = 1,
        Numerico = 2,
        Monetario = 3,
        Data = 4
    }

    public enum TipoOperacao
    {
        INCLUSAO = 1,
        ATUALIZACAO = 2,
        RETIRADA = 3
    }

    public enum TipoLinha
    {
        [Description("Header")]
        Header = 0,
        [Description("Devedor")]
        DadosCliente = 2,
        [Description("Contrato")]
        RegistroContrato = 3,
        [Description("Parcela")]
        RegistroParcela = 4,
        [Description("Bem")]
        RegistroBem = 6,
        [Description("Header")]
        Trailer = 9
    }

    public enum StatusDistribuicao
    {
        [Description("Disponível")]
        Disponivel = 0,
        [Description("Reservado")]
        Reservado = 1,
        [Description("Em cobrança")]
        EmCobranca = 2,
        [Description("Inibir cobrança")]
        InibirCobranca = 3,
        [Description("Liquidado")]
        Liquidado = 4
    }

    public enum Genero
    {
        Selecione = 0,
        Masculino = 1,
        Feminino = 2
    }

    public enum ComSemCobranca
    {
        SemCobranca = 0,
        Todos = 1
    }

    public enum FiltroNasc
    {
        SemFiltroNasc = 0,
        ComFiltroNasc = 1
    }

    public enum FiltroEnd
    {
        SemFiltroEnd = 0,
        ComFiltroEnd = 1
    }

    public enum StatusParametro
    {
        [Description("Inativo")]
        Inativo = 0,
        [Description("Ativo")]
        Ativo = 1
    }

    public enum TipoJuros
    {
        [Description("Simples")]
        Simples = 0,
        [Description("Composto")]
        Composto = 1
    }

    public enum TipoValor
    {
        [Description("Percentual")]
        Percentual = 1,
        [Description("Monetario")]
        Monetario = 2
    }

    public enum TipoDescontoAcrescimo
    {
        [Description("S/ Desc. ou Acrésc.")]
        SemDescontoSemAcrescimo = 0,
        [Description("Desconto")]
        Desconto = 1,
        [Description("Acréscimo")]
        Acrescimo = 2
    }

    public enum TipoCobrancaPagamentoExtra
    {
        [Description("Multa")]
        Multa = 1,
        [Description("Juros")]
        Juros = 2
    }

    public enum StatusCampanha
    {
        [Description("Inativa")]
        Inativo = 0,
        [Description("Ativa")]
        Ativo = 1
    }

    public enum TipoCalculo
    {
        [Description("Parametro")]
        Parametro = 1,
        [Description("Campanha")]
        Campanha = 2
    }

    public enum Direcao
    {
        Left,
        Right
    }

    public enum TipoLote
    {
        [Description("Novo Negócio")]
        NovoNegocio = 1,
        [Description("Acordo em Atraso")]
        AcordoEmAtraso = 2,
        [Description("Acordo em Dia")]
        AcordoEmDia = 3
    }

    public enum StatusPagamento
    {
        [Description("Em Aberto")]
        EmAberto = 1,
        [Description("Vencido")]
        Vencido = 2,
        [Description("Cancelado")]
        Cancelado = 3,
        [Description("Pago")]
        Pago = 4,
    }

    public enum OpcoesAgrupaContratos
    {
        [Description("Sim")]
        Sim = 1,
        [Description("Não")]
        Não = 0
    }

    public enum TipoLayoutArquivoBoletagem
    {
        [Description("Arquivo Gráfica")]
        ArquivoGrafica = 0,
        [Description("Arquivo Boletar")]
        ArquivoBoletar = 1
    }

    public enum RegArquivo
    {
        [Description("00")]
        InicioDadosBoleto = 0,
        [Description("01")]
        IdentificaCarta = 1,
        [Description("02")]
        IdentificaContratos = 2,
        [Description("03")]
        IdentificaOpcoesPagamento = 3,
        [Description("04")]
        Produto = 4,
        [Description("05")]
        Parcela = 5,
        [Description("06")]
        TotalizadorContratos = 6
    }

    public enum ContentType
    {
        [Description("application/ms-excel")]
        xls,
        [Description("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        xlsx,
        [Description("text/plain")]
        txt,
        [Description("application/pdf")]
        pdf
    }

    public enum TipoLogico
    {
        Igual = 1,
        Maior = 2,
        MaiorIgual = 3,
        Menor = 4,
        MenorIgual = 5,
        Diferente = 6
    }


    public enum TipoLeiauteImportacao
    {
        [Display(Name = "Arquivo de Ocorrências")]
        [Description("Arquivo de Ocorrências")]
        ArquivoOcorrencia = 1,
        [Display(Name = "Arquivo de Promessa de Pagamento")]
        [Description("Arquivo de Promessa de Pagamento")]
        ArquivoPromessaPagamento = 2,
        [Display(Name = "Arquivo de Repasse de Valores")]
        [Description("Arquivo de Repasse de Valores")]
        ArquivoRepasseValores = 3
    }

    public enum LayoutArquivoPagamento
    {
        [Display(Name = "Arquivo CNAB Caixa")]
        [Description("Arquivo CNAB Caixa")]
        CnabCaixa = 1,
        [Display(Name = "Arquivo CNAB Itau")]
        [Description("Arquivo CNAB Itau")]
        CnabItau = 2,
    }

    //Enumeradores TREVO

    public enum CargoFuncionario
    {
        [Description("Supervisor")]
        Supervisor = 1,
        [Description("Encarregado")]
        Encarregado = 2
    }

    public enum TipoComunicacao
    {
        [Display(Name = "Boleto")]
        [Description("Boleto")]
        Boleto = 1,
        [Display(Name = "Cadastro Mensalista")]
        [Description("Cadastro Mensalista")]
        CadastroMensalista = 2
    }

    public enum CanalComunicacao
    {
        [Display(Name = "E-mail")]
        [Description("Email")]
        Email = 1,
        [Display(Name = "Site")]
        [Description("Site")]
        Site,
        [Display(Name = "Impresso")]
        [Description("Impresso")]
        Impresso
    }

    public enum MeioPagamento
    {
        [Display(Name = "Dinheiro")]
        [Description("Dinheiro")]
        Dinheiro = 1,
        [Display(Name = "Cartao de Credito")]
        [Description("CC")]
        CartaoCredito = 2,
        [Display(Name = "Cartao de Debito")]
        [Description("CB")]
        CartaoDebito = 3,
        [Display(Name = "Boleto")]
        [Description("Boleto")]
        Boleto = 4,
        [Display(Name = "Transferencia Bancaria")]
        [Description("Transferencia")]
        Transferencia = 5,
        [Display(Name = "Deposito")]
        [Description("Deposito")]
        Deposito = 6,
        [Display(Name = "Cheque")]
        [Description("Cheque")]
        Cheque = 2,
    }

    public enum TipoVeiculo
    {
        [Display(Name = "Carro")]
        [Description("Carro")]
        Carro = 1,
        [Display(Name = "Motocicleta")]
        [Description("Motocicleta")]
        Motocicleta = 2,
        [Display(Name = "Caminhão")]
        [Description("Caminhão")]
        Caminhao = 3,
        [Display(Name = "Van")]
        [Description("Van")]
        Van = 4,
        [Display(Name = "Ônibus")]
        [Description("Ônibus")]
        Onibus = 5,
        [Display(Name = "FoodTruck")]
        [Description("FoodTruck")]
        FoodTruck = 6,
    }

    public enum StatusLancamentoCobranca
    {
        [Description("Novo")]
        Novo = 0,
        [Description("Em Aberto")]
        EmAberto = 1,
        [Description("Vencido")]
        Vencido = 2,
        [Description("Recebido")]
        Recebido = 3,
        [Description("Cheque Pré-Datado")]
        Cheque = 4,
        [Description("Cancelado")]
        Cancelado = 5,
        [Description("Depósito")]
        Deposito = 6,
        [Description("Estornado")]
        Estornado = 7,
        [Description("Pago")]
        Pago = 8,
        [Description("Divergente")]
        Divergente = 9,
        [Description("Erro CNAB")]
        ErroCNAB = 10,
        [Description("A Cancelar")]
        ACancelar = 11
    }

    public enum StatusRecebimento
    {
        [Description("Estornado")]
        Estornado = 1
    }

    public enum FormaPagamento
    {
        [Description("Cheque")]
        Cheque = 1,
        [Description("Dinheiro")]
        Dinheiro = 2,
        [Description("Cartão de Crédito")]
        Credito = 3,
        [Description("Depósito")]
        Deposito = 4,
        [Description("TED (CNAB)")]
        Ted = 5,
        [Description("DOC (CNAB)")]
        Doc = 6,
        [Description("Boleto (Título Bancário) (CNAB)")]
        Boleto = 7,
        [Description("Boleto (Concessionárias) (CNAB)")]
        BoletoConcessionaria = 14,
        [Description("Débito Automático")]
        DebitoAutomatico = 8,
        [Description("Cartão de Débito")]
        Debito = 9,
        [Description("Seguro")]
        Seguro = 10,
        [Description("DDA")]
        DDA = 11,
        [Description("Imposto sem código de barras (Tributos / Impostos)")]
        ImpostoSemCodigo = 12,
        [Description("Imposto com código de barras (Tributos / Impostos) (CNAB)")]
        ImpostoComCodigo = 13
    }

    public enum StatusVaga
    {
        [Description("Utilizada")]
        Utilizada = 1,
        [Description("Não Utilizada")]
        NaoUtilizada = 2
    }

    public enum TipoServico
    {
        [Description("Mensalista")]
        Mensalista = 1,
        [Description("Convenio")]
        Convenio = 2,
        //[Description("Aluguel")]
        //Aluguel = 3,
        [Description("Evento")]
        Evento = 4,
        [Description("Avulso")]
        Avulso = 5,
        [Description("Locação")]
        Locacao = 6,
        [Description("Seguro Reembolso")]
        SeguroReembolso = 7,
        [Description("Outros")]
        Outros = 8,
        [Description("Cartão de Acesso")]
        CartaoAcesso = 9
    }

    public enum TipoGeracao
    {
        [Description("Impressao")]
        Impressao = 1,
        [Description("E-mail")]
        Email = 2,
        [Description("Portal")]
        Portal = 3
    }

    public enum TipoCobranca
    {
        [Description("Unica")]
        Unica = 1,
        [Description("Mensal")]
        Mensal = 2,
        [Description("Pré-pago")]
        Prepago = 3,
        [Description("Pós-pago")]
        Pospago = 4
    }

    public enum TipoContaPagamento
    {
        [Description("Água")]
        Agua = 1,
        [Description("Luz")]
        Luz = 2,
        [Description("Sinistros")]
        Sinistro = 3,
        [Description("Aluguel Garagem")]
        AluguelGaragem = 4,
        [Description("Lavagem Garagem")]
        LavagemGaragem = 5,
        [Description("Telefonia")]
        Telefonia = 6,
        [Description("Internet")]
        Internet = 7,
        [Description("Outros")]
        Etc = 8
    }

    public enum TipoPessoa
    {
        [Description("Pessoa Física")]
        Fisica = 1,
        [Description("Pessoa Jurídica")]
        Juridica = 2
    }

    public enum StatusContasAPagar
    {
        [Description("Em Aberto")]
        EmAberto = 1,
        [Description("Paga")]
        Paga = 2,
        [Description("Vencida")]
        Vencida = 3,
        [Description("Solicitada Retirada Cofre")]
        RetiradaCofre = 4,
        [Description("Pendente de Pagamento")]
        PendentePagamento = 5,
        [Description("Cancelado")]
        Cancelado = 6,
        [Description("Negada")]
        Negada = 7,
        [Description("Pendente de Aprovação")]
        PendenteAprovacao = 8,
        [Description("Enviado ao Banco")]
        EnviadoAoBanco = 9,
    }

    public enum StatusRetiradaCofre
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Cancelado")]
        Cancelado = 2,
        [Description("Retirado")]
        Retirado = 3
    }

    public enum StatusPreco
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Negado")]
        Negado = 2,
        [Description("Aprovado")]
        Aprovado = 3
    }

    public enum StatusPagamentoReembolso
    {
        [Description("Pendente")]
        Pendente = 1,
        [Description("Negado")]
        Negado = 2,
        [Description("Aprovado")]
        Aprovado = 3
    }


    public enum TipoHorario
    {
        [Description("Segunda à Sexta")]
        SegundaASexta = 1,
        [Description("Sábado")]
        Sabado = 2,
        [Description("Domingo")]
        Domingo = 3,
        [Description("Feriado")]
        Feriado = 4
    }

    public enum StatusHorario
    {
        [Description("Aguardando Aprovação")]
        Aguardando = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Negado")]
        Negado = 3
    }

    public enum StatusCheque
    {
        [Description("Devolvido")]
        Devolvido = 1,
        [Description("Debitado")]
        Debitado = 2,
        [Description("Não Depositado")]
        NaoDepositado = 3,
        [Description("Protestado")]
        Protestado = 4,
        [Description("À cobrar - escritório")]
        ACobrarEscritorio,
        [Description("Com supervisor")]
        ComSupervisor,
        [Description("Recebido na garagem")]
        RecebidoNaGaragem,
        [Description("Reapresentado")]
        Reapresentado,
        [Description("Recebido escritório")]
        RecebidoEscritório
    }

    public enum IndiceReajuste
    {
        [Description("INPC")]
        INPC = 1,
        [Description("IPC")]
        IPC = 2,
        [Description("IGD-DI")]
        IGD_DI = 3,
        [Description("IGP-M")]
        IGP_M = 4,
        [Description("IPCA")]
        IPCA = 5,
        [Description("valor fixo")]
        ValorFixo = 5
    }

    public enum TipoContrato
    {
        [Description("Compra")]
        Compra = 1,
        [Description("Aluguel")]
        Aluguel = 2,
        [Description("Parceria")]
        Parceria = 3
    }

    public enum PeriodoPreco
    {
        [Description("Segunda á Sexta")]
        SegASexta = 1,
        [Description("Sábado")]
        Sabado = 2,
        [Description("Domingo")]
        Domingo = 3,
        [Description("Feriado")]
        Feriado = 4,
    }

    public enum PeriodoDia
    {
        [Description("Manhã")]
        Manha = 1,
        [Description("Tarde")]
        Tarde = 2,
        [Description("Noite")]
        Noite = 3,
        [Description("Integral")]
        Integral = 4,
        [Description("Madrugada")]
        Madrugada = 5,
        [Description("Personalizado")]
        Personalizado = 6
    }

    public enum PeriodoDiasEquipamentoUnidade
    {
        [Description("a cada 7 dias")]
        aCada7Dias = 1,
        [Description("a cada 10 dias")]
        aCada10Dias = 2,
        [Description("a cada 20 dias")]
        aCada20Dias = 3
    }

    public enum TiposUnidades
    {
        [Description("Garagem")]
        Garagem = 1,
        [Description("Prédio Comercial")]
        PredioComercial = 2,
        [Description("Condominio")]
        Condomio = 3
    }


    public enum TiposPagamentos
    {
        [Description("Cartão de Crédito")]
        cartaoCredito = 1,
        [Description("Cartão de Débito")]
        cartaoDebito = 2,
        [Description("Dinheiro")]
        dinheiro = 3,
        [Description("Cheque")]
        cheque = 4,
        [Description("TED")]
        ted = 5,
        [Description("Depósito")]
        deposito = 6,
        [Description("Boleto")]
        boleto = 7
    }

    public enum StatusSolicitacao
    {
        [Description("Aguardando Aprovação")]
        Aguardando = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Negado")]
        Negado = 3
    }

    public enum StatusNotificacao
    {
        [Description("Aguardando Aprovação")]
        Aguardando = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Reprovado")]
        Reprovado = 3,
        [Description("Visualização")]
        Visualizacao = 4
    }

    public enum StatusPrevioNotificacao
    {
        Aprovar, Reprovar
    }

    public enum StatusDesbloqueioLiberacao
    {
        [Description("Aguardando Aprovação")]
        Aguardando = 1,
        [Description("Aprovado")]
        Aprovado = 2,
        [Description("Negado")]
        Negado = 3,
        [Description("Utilizado")]
        Utilizado = 4
    }

    public enum TipoOpreracao
    {
        [Description("Troca")]
        troca = 1,
        [Description("Alocação")]
        alocacao = 2

    }

    public enum Mes
    {
        [Description("Janeiro")]
        Janeiro = 1,
        [Description("Fevereiro")]
        Fevereiro = 2,
        [Description("Março")]
        Março = 3,
        [Description("Abril")]
        Abril = 4,
        [Description("Maio")]
        Maio = 5,
        [Description("Junho")]
        Junho = 6,
        [Description("Julho")]
        Julho = 7,
        [Description("Agosto")]
        Agosto = 8,
        [Description("Setembro")]
        Setembro = 9,
        [Description("Outubro")]
        Outubro = 10,
        [Description("Novembro")]
        Novembro = 11,
        [Description("Dezembro")]
        Dezembro = 12
    }

    public enum TipoDocumentoConta
    {
        [Description("Recibo")]
        Recibo = 1,
        [Description("Nota Fiscal")]
        NotaFiscal = 2,
        [Description("Cupom Fiscal")]
        CupomFiscal = 3
    }

    public enum StatusEmissao
    {
        [Description("Movimento Pendente")]
        MovimentoPendente = 1,
        [Description("Enviado Prefeitura")]
        EnviadoPrefeitura = 2,
        [Description("Liberado para Envio")]
        Liberado = 3

    }

    public enum StatusSelo
    {
        [Description("Ativo")]
        Ativo = 1,
        [Description("Cancelado Lote")]
        CanceladoLote = 2,
        [Description("Excluido Lote")]
        ExcluidoLote = 3,
        [Description("Bloqueado")]
        Bloqueado = 4,
        [Description("Aguardando Desbloqueio")]
        AguardandoDesbloqueio = 5
    }

    public enum ParametroSelo
    {
        [Description("Monetário")]
        Monetario = 1,
        [Description("Percentual")]
        Percentual = 2,
        [Description("Hora Adicional")]
        HoraAdicional = 3,
        [Description("Primeira Linha")]
        HoraInicial = 4,
        [Description("Personalizado")]
        Personalizado = 5,
        //[Description("Qtd. Horas")]
        //Hora = 4,
    }


    public enum TipoDesconto
    {
        [Description("Monetário")]
        Monetario = 1,
        [Description("Percentual")]
        Percentual = 2
    }

    public enum TipoPedidoSelo
    {
        [Description("Bloqueio")]
        Bloqueio = 1,
        [Description("Desbloqueio")]
        Desbloqueio = 2,
        [Description("Nova Emissão")]
        Emissao = 3
    }

    public enum TipoPagamentoSelo
    {
        [Description("Pré-pago")]
        Prepago = 1,
        [Description("Pós-pago")]
        Pospago = 2
    }

    public enum StatusPedidoSelo
    {
        //[Description("Rascunho")]
        //Rascunho = 1,
        [Description("Pendente de Aprovação do Desconto")]
        PendenteAprovacaoDesconto = 2,
        [Description("Desconto Aprovado")]
        DescontoAprovado = 3,
        [Description("Desconto Reprovado")]
        DescontoReprovado = 4,
        [Description("Pendente de Aprovação do Pedido")]
        PendenteAprovacaoPedido = 5,
        //[Description("Pedido Aprovado")]
        //PedidoAprovado = 6,
        //[Description("Pedido Reprovado")]
        //PedidoReprovado = 7,
        [Description("Pendente de Aprovação do Cliente")]
        PendenteAprovacaoCliente = 8,
        [Description("Aprovado pelo Cliente")]
        AprovadoPeloCliente = 9,
        [Description("Reprovado pelo Cliente")]
        ReprovadoPeloCliente = 10,
        [Description("Cancelado")]
        Cancelado = 11
    }

    public enum TipoReajuste
    {
        [Description("Monetário")]
        Monetario = 1,
        [Description("Percentual")]
        Percentual = 2
    }

    public enum PrazoReajuste
    {
        [Description("Mensal")]
        Mensal = 1,
        [Description("Bimestral")]
        Bimestral = 2,
        [Description("Trimestral")]
        Trimestral = 3,
        [Description("Semestral")]
        Semestral = 6,
        [Description("Anual")]
        Anual = 12,
    }

    public enum CodigoBancos
    {
        BancodoBrasil = 1,
        Banrisul = 41,
        Basa = 3,
        Bradesco = 237,
        BRB = 70,
        Caixa = 104,
        HSBC = 399,
        Itau = 341,
        Real = 356,
        Safra = 422,
        Santander = 33,
        Sicoob = 756,
        Sicred = 748,
        Sudameris = 347,
        //Unibanco = 409,
        Semear = 743
    }

    public enum AcaoEstoqueManual
    {
        [Description("Entrada")]
        Entrada = 1,
        [Description("Saída")]
        Saida = 2,
        [Description("Inventário")]
        Inventario = 3
    }

    public enum AcaoNotificacao
    {
        [Description("Aprovado")]
        Aprovado = 1,
        [Description("Reprovado")]
        Reprovado = 2,
        [Description("Informações")]
        Informcacoes = 3
    }

    public enum AcaoWorkflowPedido
    {
        [Description("Aprovar")]
        Aprovar = 1,
        [Description("Reprovar")]
        Reprovar = 2,
        [Description("Cancelar")]
        Cancelar = 3,
    }

    public enum TipoAcaoNotificacao
    {
        [Description("Aprovar e Reprovar")]
        AprovarReprovar = 1,
        [Description("Aviso")]
        Aviso = 2
    }

    public enum TipoEnvioEmailPedidoSelo
    {
        [Description("Email com proposta")]
        Proposta = 1,
        [Description("Email com boleto")]
        Boleto = 2,
        [Description("Proposta recusada pelo cliente")]
        PropostaRecusada = 3,
    }

    //public enum TelaOrigemNotificacao
    //{
    //    [Description("TabelaPreco")]
    //    TabelaPreco = 1,
    //    [Description("ParametroEquipe")]
    //    ParametroEquipe = 2,
    //    [Description("HorarioUnidade")]
    //    HorarioUnidade = 3
    //}

    public enum TipoPeriodo
    {
        [Description("Domingo")]
        Domingo = 1,
        [Description("Segunda")]
        Segunda = 2,
        [Description("Terça")]
        Terca = 3,
        [Description("Quarta")]
        Quarta = 4,
        [Description("Quinta")]
        Quinta = 5,
        [Description("Sexta")]
        Sexta = 6,
        [Description("Sábado")]
        Sabado = 7,
        [Description("Evento")]
        Evento = 8,
        [Description("Feriado")]
        Feriado = 9,
    }

    public enum StatusItem
    {
        [Description("Ativo")]
        Ativo = 0,
        [Description("Inativo")]
        Inativo = 1

    }

    public enum OperadorPerfilSoftpark
    {
        [Description("Administrador")]
        Administrador = 1,
        [Description("Operador")]
        Operador = 2,
        [Description("Supervisor")]
        Supervisor = 3
    }

    public enum TipoOISCategoria
    {
        [Description("Roubo")]
        Roubo = 1,
        [Description("Furto")]
        Furto,
        [Description("Quebra de peça")]
        QuebraDePeca,
        [Description("Colisão")]
        Colisao,
        [Description("Incidente")]
        Incidente,
        [Description("Outros")]
        Outros
    }

    public enum StatusSinistro
    {
        [Description("Recebido")]
        Recebido = 1,
        [Description("Em Negociação")]
        EmNegociacao,
        [Description("Finalizado")]
        Finalizado,
        [Description("Entre Outros")]
        EntreOutros
    }

    public enum FormaPagamentoOrcamentoSinistroCotacaoItem
    {
        [Description("Dinheiro")]
        Dinheiro = 1,
        [Description("Boleto")]
        Boleto,
        [Description("Seguro")]
        Seguro,
        [Description("Transferência")]
        Transferencia,
        [Description("Crédito")]
        Credito
    }

    public enum StatusOrcamentoSinistro
    {
        [Description("Aguardando Aprovação")]
        AguardandoAprovacao = 1,
        [Description("Aprovado")]
        Aprovado,
        [Description("Negado")]
        Negado,
        [Description("Aguardando Aprovação Orçamento")]
        AguardandoAprovacaoOrcamento,
        [Description("Orçamento Aprovado")]
        OrcamentoAprovado,
        [Description("Orçamento Negado")]
        OrcamentoNegado,
        [Description("Cancelado")]
        Cancelado
    }

    public enum StatusOrcamentoSinistroCotacao
    {
        [Description("Aguardando Aprovação")]
        AguardandoAprovacao = 1,
        [Description("Aprovado")]
        Aprovado,
        [Description("Negado")]
        Negado
    }

    public enum StatusCompraServico
    {
        [Description("Em Aberto")]
        EmAberto,
        [Description("Entregue para o cliente")]
        EntregueParaCliente,
        [Description("Concluído")]
        Concluido
    }

    public enum AcaoRetiradaCofre
    {
        [Description("Aprovar")]
        Aprovar = 1,
        [Description("Negar")]
        Negar
    }

    public enum TipoFiltroGeracaoCNAB
    {
        [Description("Não Gerados")]
        NaoGerados = 0,
        [Description("Gerados")]
        Gerados = 1
    }

    public enum TipoJurosContaPagar
    {
        [Description("Percentual")]
        Percentual = 1,
        [Description("Monetário")]
        Monetario = 2
    }

    public enum TipoMultaContaPagar
    {
        [Description("Percentual")]
        Percentual = 1,
        [Description("Monetário")]
        Monetario = 2
    }

    public enum TipoOcorrenciaCNAB
    {
        ENTRADA = 01,
        BAIXA = 02,
        PRORROGAÇÃODEVENCIMENTO = 06
    }

    public enum TipoHoraExtra
    {
        [Description("65%")]
        SessentaCinco = 65,
        [Description("100%")]
        Cem = 100
    }

    public enum TipoRelatorioFinanceiro
    {
        [Description("Pagamentos Em Aberto")]
        PagamentosEmAberto = 1,
        [Description("Pagamentos Efetuados")]
        PagamentosEfetuados = 2,
        //[Description("Lançamentos Em Aberto (Excel)")]
        //LancamentosCobrancaEmAberto = 3,
        [Description("Pagamentos Efetuados (Excel)")]
        PagamentosEfetuadosExcel = 4,
        //[Description("Selos - Em Aberto")]
        //SelosEmAberto = 5,
        [Description("Pagamentos Efetuados Convênio (Excel)")]
        SelosPagos = 6,
        [Description("Pagamentos Em Aberto e Efetuados")]
        PagamentosEmAbertoEfetuados = 7,
        [Description("Lançamentos Pagos Divergentes")]
        LancamentosPagosDivergentes = 8,
        [Description("Lançamentos de Clientes")]
        LancamentosClientes = 9,
        [Description("Pagamentos Efetuados Conferência")]
        PagamentosEfetuadosConferencia = 10,
    }

    public enum TipoOcorrenciaRetorno
    {
        [Description("Entrada Confirmada")]
        EntradaConfirmada = 02,                                                   //'02' = Entrada Confirmada
        [Description("Entrada Rejeitada")]
        EntradaRejeitada = 03,                                                    //'03' = Entrada Rejeitada
        [Description("Transferência de Carteira/Entrada")]
        TransferenciaDeCarteiraEntrada = 04,                                      //'04' = Transferência de Carteira/Entrada
        [Description("Transferência de Carteira/Baixa")]
        TransferenciaDeCarteiraBaixa = 05,                                        //'05' = Transferência de Carteira/Baixa
        [Description("Liquidação")]
        Liquidacao = 06,                                                          //'06' = Liquidação
        [Description("Confirmação do Recebimento da Instrução de Desconto")]
        ConfirmacaoDoRecebimentoDaInstrucaoDeDesconto = 07,                       //'07' = Confirmação do Recebimento da Instrução de Desconto
        [Description("Confirmação do Recebimento do Cancelamento do Desconto")]
        ConfirmacaoDoRecebimentoDoCancelamentoDoDesconto = 08,                    //'08' = Confirmação do Recebimento do Cancelamento do Desconto
        [Description("Baixa")]
        Baixa = 09,                                                               //'09' = Baixa
        [Description("Títulos em Carteira (Em Ser)")]
        TitulosEmCarteira = 11,                                                   //'11' = Títulos em Carteira (Em Ser)
        [Description("Confirmação Recebimento Instrução de Abatimento")]
        ConfirmacaoRecebimentoInstrucaoDeAbatimento = 12,                         //'12' = Confirmação Recebimento Instrução de Abatimento
        [Description("Confirmação Recebimento Instrução de Cancelamento Abatimento")]
        ConfirmacaoRecebimentoInstrucaoDeCancelamentoAbatimento = 13,             //'13' = Confirmação Recebimento Instrução de Cancelamento Abatimento
        [Description("Confirmação Recebimento Instrução Alteração de Vencimento")]
        ConfirmacaoRecebimentoInstrucaoAlteracaoDeVencimento = 14,                //'14' = Confirmação Recebimento Instrução Alteração de Vencimento
        [Description("Franco de Pagamento")]
        FrancoDePagamento = 15,                                                   //'15' = Franco de Pagamento
        [Description("Liquidação Após Baixa ou Liquidação Título Não Registrado")]
        LiquidacaoAposBaixaOuLiquidacaoTituloNaoRegistrado = 17,                  //'17' = Liquidação Após Baixa ou Liquidação Título Não Registrado
        [Description("Confirmação Recebimento Instrução de Protesto")]
        ConfirmacaoRecebimentoInstrucaoDeProtesto = 19,                           //'19' = Confirmação Recebimento Instrução de Protesto
        [Description("Confirmação Recebimento Instrução de Sustação/Cancelamento de Protesto")]
        ConfirmacaoRecebimentoInstrucaoDeSustacaoCancelamentoDeProtesto = 20,     //'20' = Confirmação Recebimento Instrução de Sustação/Cancelamento de Protesto
        [Description("Remessa a Cartório (Aponte em Cartório)")]
        RemessaACartorio = 23,                                                    //'23' = Remessa a Cartório (Aponte em Cartório)
        [Description("Retirada de Cartório e Manutenção em Carteira")]
        RetiradaDeCartorioEManutencaoEmCarteira = 24,                             //'24' = Retirada de Cartório e Manutenção em Carteira
        [Description("Protestado e Baixado (Baixa por Ter Sido Protestado)")]
        ProtestadoEBaixado = 25,                                                  //'25' = Protestado e Baixado (Baixa por Ter Sido Protestado)
        [Description("Instrução Rejeitada")]
        InstrucaoRejeitada = 26,                                                  //'26' = Instrução Rejeitada
        [Description("Confirmação do Pedido de Alteração de Outros Dados")]
        ConfirmacaoDoPedidoDeAlteracaoDeOutrosDados = 27,                         //'27' = Confirmação do Pedido de Alteração de Outros Dados
        [Description("Débito de Tarifas/Custas")]
        DebitoDeTarifasCustas = 28,                                               //'28' = Débito de Tarifas/Custas
        [Description("Ocorrências do Pagador")]
        OcorrenciasDoPagador = 29,                                                //'29' = Ocorrências do Pagador
        [Description("Alteração de Dados Rejeitada")]
        AlteracaoDeDadosRejeitada = 30,                                           //'30' = Alteração de Dados Rejeitada
        [Description("Confirmação da Alteração dos Dados do Rateio de Crédito")]
        ConfirmacaoDaAlteracaoDosDadosDoRateioDeCredito = 33,                     //'33' = Confirmação da Alteração dos Dados do Rateio de Crédito
        [Description(" Confirmação do Cancelamento dos Dados do Rateio de Crédito")]
        ConfirmacaoDoCancelamentoDosDadosDoRateioDeCredito = 34,                  //'34' = Confirmação do Cancelamento dos Dados do Rateio de Crédito
        [Description("Confirmação do Desagendamento do Débito Automático")]
        ConfirmacaoDoDesagendamentoDoDebitoAutomatico = 35,                       //'35' = Confirmação do Desagendamento do Débito Automático
        [Description("Confirmação de envio de e-mail/SMS")]
        ConfirmacaoDeEnvioDeEmailSMS = 36,                                        //'36' = Confirmação de envio de e-mail/SMS
        [Description("Envio de e-mail/SMS rejeitado")]
        EnvioDeEmailSMSRejeitado = 37,                                            //'37' = Envio de e-mail/SMS rejeitado
        [Description("Confirmação de alteração do Prazo Limite de Recebimento")]
        ConfirmacaoDeAlteracaoDoPrazoLimiteDeRecebimento = 38,                    //'38' = Confirmação de alteração do Prazo Limite de Recebimento
        [Description("Confirmação de Dispensa de Prazo Limite de Recebimento")]
        ConfirmacaoDeDispensaDePrazoLimiteDeRecebimento = 39,                     //'39' = Confirmação de Dispensa de Prazo Limite de Recebimento
        [Description("Confirmação da alteração do número do título dado pelo Beneficiário")]
        ConfirmacaoDaAlteracaoDonumeroDoTitulodadoPeloBeneficiário = 40,          //'40' = Confirmação da alteração do número do título dado pelo Beneficiário
        [Description("Confirmação da alteração do número controle do Participante")]
        ConfirmacaoDaAlteracaoDonumeroControleDoParticipante = 41,                //'41' = Confirmação da alteração do número controle do Participante
        [Description("Confirmação da alteração dos dados do Pagador")]
        ConfirmacaoDaAlteracaoDosdadosDoPagador = 42,                             //'42' = Confirmação da alteração dos dados do Pagador
        [Description("Confirmação da alteração dos dados do Sacador/Avalista")]
        ConfirmacaoDaAlteracaoDosdadosDoSacadorAvalista = 43,                     //'43' = Confirmação da alteração dos dados do Sacador/Avalista
        [Description("Título pago com cheque devolvido")]
        TitulopagoComChequeDevolvido = 44,                                        //'44' = Título pago com cheque devolvido
        [Description("Título pago com cheque compensado")]
        TitulopagoComChequeCompensado = 45,                                       //'45' = Título pago com cheque compensado
        [Description("Instrução para cancelar protesto confirmada")]
        InstrucaoParacancelarProtestoConfirmada = 46,                             //'46' = Instrução para cancelar protesto confirmada
        [Description("Instrução para protesto para fins falimentares confirmada")]
        InstrucaoParaprotestoParafinsFalimentaresConfirmada = 47,                 //'47' = Instrução para protesto para fins falimentares confirmada
        [Description("Confirmação de instrução de transferência de carteira/modalidade de cobrança")]
        ConfirmacaoDeinstrucaoDetransferenciaDecarteiraModalidadeDecobrança = 48, //'48' = Confirmação de instrução de transferência de carteira/modalidade de cobrança
        [Description("Alteração de contrato de cobrança")]
        AlteracaoDecontratoDecobranca = 49,                                       //'49' = Alteração de contrato de cobrança
        [Description("Título pago com cheque pendente de liquidação")]
        TitulopagoComchequePendenteDeliquidacao = 50,                             //'50' = Título pago com cheque pendente de liquidação
        [Description("Título DDA reconhecido pelo Pagador")]
        TituloDDAreconhecidoPeloPagador = 51,                                     //'51' = Título DDA reconhecido pelo Pagador
        [Description("Título DDA não reconhecido pelo Pagador")]
        TituloDDANaoReconhecidoPeloPagador = 52,                                  //'52' = Título DDA não reconhecido pelo Pagador
        [Description("Título DDA recusado pela CIP")]
        TituloDDArecusadoPelaCIP = 53,                                            //'53' = Título DDA recusado pela CIP
        [Description("Confirmação da Instrução de Baixa de Título Negativado sem Protesto")]
        ConfirmacaoDaInstrucaoDeBaixaDeTituloNegativadoSemProtesto = 54,          //'54' = Confirmação da Instrução de Baixa de Título Negativado sem Protesto
        [Description("Confirmação de Pedido de Dispensa de Multa")]
        ConfirmacaoDePedidoDeDispensaDeMulta = 55,                                //'55' = Confirmação de Pedido de Dispensa de Multa
        [Description("Confirmação do Pedido de Cobrança de Multa")]
        ConfirmacaoDoPedidoDeCobrancaDeMulta = 56,                                //'56' = Confirmação do Pedido de Cobrança de Multa
        [Description("  Confirmação do Pedido de Alteração de Cobrança de Juros")]
        ConfirmacaoDoPedidoDeAlteracaoDeCobrancaDeJuros = 57,                     //'57' = Confirmação do Pedido de Alteração de Cobrança de Juros
        [Description("Confirmação do Pedido de Alteração do Valor/Data de Desconto")]
        ConfirmacaoDoPedidoDeAlteracaoDoValorDataDeDesconto = 58,                 //'58' = Confirmação do Pedido de Alteração do Valor/Data de Desconto
        [Description("Confirmação do Pedido de Alteração do Beneficiário do Título")]
        ConfirmacaoDoPedidoDeAlteracaoDoBeneficiarioDoTítulo = 59,                //'59' = Confirmação do Pedido de Alteração do Beneficiário do Título
        [Description("Confirmação do Pedido de Dispensa de Juros de Mora")]
        ConfirmacaoDoPedidoDeDispensaDeJurosDeMora = 60,                          //'60' = Confirmação do Pedido de Dispensa de Juros de Mora
        [Description("Confirmação de Alteração do Valor Nominal do Título")]
        ConfirmacaoDeAlteracaoDoValorNominalDoTitulo = 61,                        //'61' = Confirmação de Alteração do Valor Nominal do Título
        [Description("Título Sustado Judicialmente")]
        TituloSustadoJudicialmente = 63                                           //'63' = Título Sustado Judicialmente
    }

    public enum TipoOperacaoContaCorrente
    {
        [Description("Acréscimo")]
        Acrescimo = 1,
        [Description("Decréscimo")]
        Decrescimo = 2,
    }

    public enum TipoNatureza
    {
        [Description("Reclamação")]
        Reclamacao = 1,
        [Description("Sugestão")]
        Sugestao = 2,
        [Description("Elogio")]
        Elogio = 3,
    }

    public enum TipoOrigem
    {
        [Description("Telefone")]
        Telefone = 1,
        [Description("E-mail")]
        Email = 2,
        [Description("Supervisor")]
        Supervisor = 3,
        [Description("Funcionário")]
        Funcionario = 4,
        [Description("Outros")]
        Outros = 5,
    }

    public enum TipoPrioridade
    {
        [Description("Baixa")]
        Baixa = 1,
        [Description("Média")]
        Media = 2,
        [Description("Alta")]
        Alta = 3,
    }

    public enum StatusOcorrencia
    {
        [Description("Novo")]
        Novo = 1,
        [Description("Atribuído")]
        Atribuido = 2,
        [Description("Encerrado")]
        Encerrado = 3,
        [Description("Rejeitado")]
        Rejeitado = 4
    }

    public enum StatusPMS
    {
        [Description("Valor Líquido")]
        ValorLiquido = 1,
        [Description("Proporcional")]
        Proporcional = 2,
        [Description("Proporcional e Mês Seguinte")]
        ProporcionalMesSeguinte = 3
        //[Description("Mês Seguinte")]
        //MesSeguinte = 4
    }

    public static class StaticHelper
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}