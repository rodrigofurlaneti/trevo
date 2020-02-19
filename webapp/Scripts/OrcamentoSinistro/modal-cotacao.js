let cotacaoItens = [];
let cotacaoItemEmEdicao = {};
let cotacaoStatus = 0;

let FormaPagamento = {
    "Dinheiro": 1,
    "Boleto": 2,
    "Seguro": 3,
    "Transferencia": 4,
    "Credito": 5
};

$(document).ready(function () {
    FormatarCampos();

    $('#modal-cotacao').on('show.bs.modal', function (e) {
        $("#btn-solicitar-cotacao-disabled").hide();
        $("#btn-solicitar-cotacao").hide();

        BuscarDadosDoModal();
    })

    $("#modal-cotacao")
        .on("blur", "#cotacao-quantidade, #cotacao-valor-unitario", function () {
            CalcularValorTotal();
        })
        .on("blur", "#cotacao-quantidade-reembolso, #cotacao-valor-unitario-reembolso, #cotacao-valor-reembolso", function () {
            CalcularValorTotalReembolso();
        })
        .on("change", "#cotacao-forma-pagamento", function () {
            AlternarContainerPagamentoSeguro();
        })
        .on("change", "#cotacao-tem-seguro-reembolso", function () {
            AlternarContainerSeguroReembolso();
        });
});

function AlternarContainerPagamentoSeguro() {
    if (FormaPagamento["Seguro"] == $("#cotacao-forma-pagamento").val()) {
        $("#cotacao-quantidade").val("");
        $("#cotacao-valor-unitario").val("");
        $("#cotacao-valor-total").val("0,00");
        $("#container-se-seguro").show();
        $("#container-se-nao-seguro").hide();
    } else {
        document.getElementById("cotacao-tem-seguro-reembolso").checked = false;
        $("#cotacao-cia-seguro-reembolso").val("");
        $("#cotacao-data-prevista-reembolso").val("");
        $("#cotacao-quantidade-reembolso").val("");
        $("#cotacao-valor-unitario-reembolso").val("");
        $("#cotacao-valor-reembolso").val("");
        $("#cotacao-valor-total-reembolso").val("0,00");

        $("#container-se-seguro").hide();
        $("#container-se-nao-seguro").show();
    }
}

function AlternarContainerSeguroReembolso() {
    if (document.getElementById("cotacao-tem-seguro-reembolso").checked) {
        $("#cotacao-valor-reembolso").unDisabled();
        $("#container-se-seguro-reembolso").show();
    }
    else {
        $("#cotacao-data-prevista-reembolso").val("");
        $("#cotacao-valor-reembolso").val("");
        $("#cotacao-valor-reembolso").disabled();
        $("#container-se-seguro-reembolso").hide();
    }
}

function AlternarBotaoHistoricoData() {
    if (cotacaoItemEmEdicao.OrcamentoSinistroCotacaoHistoricoDataItens != null && cotacaoItemEmEdicao.OrcamentoSinistroCotacaoHistoricoDataItens.length > 0)
        $("#btn-historico-data").show();
    else
        $("#btn-historico-data").hide();
}

function AbrirModalHistoricoData() {
    return post("AtualizarHistoricoData", { historicoData: cotacaoItemEmEdicao.OrcamentoSinistroCotacaoHistoricoDataItens })
        .done((response) => {
            $("#lista-historico-data").empty().append(response);

            $("#modal-historico-data").modal("show");
        })
}

function AlternarBotaoPeloStatus() {
    if ((cotacaoStatus && cotacaoStatus != 3) ||
        Object.keys(cotacaoItemEmEdicao).length > 0 ||
        cotacaoItens.some(x => x.Oficina == null) ||
        cotacaoItens.length == 0) {
        $("#btn-solicitar-cotacao-disabled").show();
        $("#btn-solicitar-cotacao").hide();
    }
    else {
        $("#btn-solicitar-cotacao-disabled").hide();
        $("#btn-solicitar-cotacao").show();
    }
}

function SalvarCotacao() {
    return post("SalvarCotacao", { orcamentoSinistroId: modalOrcamentoSinistroId })
        .done((result) => $('#modal-principal').empty().append(result));
}

function BuscarDadosDoModal() {
    return post("BuscarDadosDoModal", { orcamentoSinistroId: modalOrcamentoSinistroId })
        .done((result) => {
            cotacaoItens = result.Itens;
            cotacaoStatus = result.Status;

            AlternarBotaoPeloStatus();

            $("#form-cotacao-itens").empty().append(result.Form);
            $("#lista-cotacao-itens").empty().append(result.Grid);

            FormatarCampos();
        });
}

function CalcularValorTotal() {
    let quantidade = $("#cotacao-quantidade").int();
    let valor = $("#cotacao-valor-unitario").decimal();
    let valorTotal = valor * quantidade;

    if (!isNaN(valorTotal)) {
        let valorComMascara = $("#cotacao-valor-total").val(valorTotal.toFixed(2)).masked();
        $("#cotacao-valor-total").val(valorComMascara);
    }
}

function CalcularValorTotalReembolso() {
    let quantidade = $("#cotacao-quantidade-reembolso").int();
    quantidade = quantidade <= 0 ? 1 : quantidade;
    let valor = $("#cotacao-valor-unitario-reembolso").decimal();
    let valorTotalReembolso = valor * quantidade;

    if (!isNaN(valorTotalReembolso)) {
        let valorComMascara = $("#cotacao-valor-total").val(valorTotalReembolso.toFixed(2)).masked();
        $("#cotacao-valor-total-reembolso").val(valorComMascara);
    }
}

function FormatarCampos() {
    FormatarNumerosInteiros("#cotacao-quantidade, #cotacao-quantidade-reembolso");
    FormatarNumerosDecimais("#cotacao-valor-total, #cotacao-valor-total-reembolso");
    FormatarCampoData("data-servico");
    FormatarCampoData("cotacao-data-prevista-reembolso");

    $("#cotacao-valor-unitario, #cotacao-valor-unitario-reembolso, #cotacao-valor-reembolso").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
}

function atualizarCotacaoItem(cotacaoItens) {
    return post("AtualizarCotacaoItens", { cotacaoItens })
        .done((response) => {
            $("#lista-cotacao-itens").empty().append(response);
        });
}

function validarCotacaoItem(cotacaoItem) {

    if (!cotacaoItem.Oficina.Id) {
        toastr.warning("Informe a Oficina", "Alerta");
        return false;
    }

    if (!cotacaoItem.Fornecedor.Id) {
        toastr.warning("Informe o Fornecedor", "Alerta");
        return false;
    }

    if (!cotacaoItem.DataServico) {
        toastr.warning("Informe a data limite para o serviço/compra", "Alerta");
        return false;
    }

    if (!cotacaoItem.PecaServico.Id) {
        toastr.warning("Informe a Peca ou Serviço", "Alerta");
        return false;
    }

    if (!cotacaoItem.PecaServico.Id) {
        toastr.warning("Informe a Peca ou Serviço", "Alerta");
        return false;
    }

    if (!cotacaoItem.FormaPagamento) {
        toastr.warning("Informe a Forma de Pagamento", "Alerta");
        return false;
    }

    if (cotacaoItem.TemSeguroReembolso && !cotacaoItem.DataReembolso) {
        toastr.warning("Informe â Data Prevista de Reembolso", "Alerta");
        return false;
    }

    if (!cotacaoItem.Quantidade && !cotacaoItem.TemSeguroReembolso) {
        toastr.warning("Informe a quantidade", "Alerta");
        return false;
    }

    if (!cotacaoItem.ValorUnitario) {
        toastr.warning("Informe o valor unitário", "Alerta");
        return false;
    }

    return true;
}

function adicionarCotacaoItem() {
    let ehFormaPagamentoSeguro = FormaPagamento["Seguro"] == $("#cotacao-forma-pagamento").val();
    let cotacaoItem = {
        Oficina: {
            Id: $("#cotacao-oficina").val(),
            Nome: $("#cotacao-oficina option:selected").text()
        },
        Fornecedor: {
            Id: $("#cotacao-fornecedor").val(),
            Nome: $("#cotacao-fornecedor option:selected").text()
        },
        PecaServico: {
            Id: $("#cotacao-peca-servico-id").val(),
            Nome: $("#cotacao-peca-servico").val()
        },
        FormaPagamento: $("#cotacao-forma-pagamento").val(),
        Quantidade: ehFormaPagamentoSeguro ? $("#cotacao-quantidade-reembolso").int() : $("#cotacao-quantidade").int(),
        ValorUnitario: ehFormaPagamentoSeguro ? $("#cotacao-valor-unitario-reembolso").valDecimal() : $("#cotacao-valor-unitario").valDecimal(),
        ValorTotal: ehFormaPagamentoSeguro ? $("#cotacao-valor-total-reembolso").valDecimal() : $("#cotacao-valor-total").valDecimal(),
        DataServico: $("#data-servico").val(),
        TemSeguroReembolso: document.getElementById("cotacao-tem-seguro-reembolso").checked,
        CiaSeguro: $("#cotacao-cia-seguro-reembolso").val(),
        DataReembolso: $("#cotacao-data-prevista-reembolso").val(),
        ValorReembolso: $("#cotacao-valor-reembolso").val(),
    }

    if (!validarCotacaoItem(cotacaoItem))
        return;

    cotacaoItens.push(cotacaoItem);

    atualizarCotacaoItem(cotacaoItens);

    limparCamposCotacao();
}

function removerCotacaoItem(pecaServicoId) {
    cotacaoItens = cotacaoItens.filter(x => x.PecaServico.Id != pecaServicoId);

    AlternarBotaoPeloStatus();

    return atualizarCotacaoItem(cotacaoItens);
}

function editarCotacaoItem(pecaServicoId) {
    if (Object.keys(cotacaoItemEmEdicao).length)
        cotacaoItens.push(cotacaoItemEmEdicao);

    cotacaoItemEmEdicao = cotacaoItens.find(x => x.PecaServico.Id == pecaServicoId);
    removerCotacaoItem(pecaServicoId);

    if (cotacaoItemEmEdicao.Oficina) {
        $("#cotacao-oficina").val(cotacaoItemEmEdicao.Oficina.Id);
    }

    if (cotacaoItemEmEdicao.Fornecedor) {
        $("#cotacao-fornecedor").val(cotacaoItemEmEdicao.Fornecedor.Id);
    }

    let ehFormaPagamentoSeguro = FormaPagamento["Seguro"] == cotacaoItemEmEdicao.FormaPagamento;

    $("#cotacao-forma-pagamento").val(cotacaoItemEmEdicao.FormaPagamento);
    $("#cotacao-peca-servico-id").val(cotacaoItemEmEdicao.PecaServico.Id);
    $("#cotacao-peca-servico").val(cotacaoItemEmEdicao.PecaServico.Nome);
    $("#data-servico").val(cotacaoItemEmEdicao.DataServico);
    $("#cotacao-cia-seguro-reembolso").val(cotacaoItemEmEdicao.CiaSeguro);
    $("#cotacao-data-prevista-reembolso").val(cotacaoItemEmEdicao.DataReembolso);
    document.getElementById("cotacao-tem-seguro-reembolso").checked = cotacaoItemEmEdicao.TemSeguroReembolso;

    if (ehFormaPagamentoSeguro) {
        $("#cotacao-quantidade-reembolso").val(cotacaoItemEmEdicao.Quantidade);
        $("#cotacao-valor-unitario-reembolso").val(cotacaoItemEmEdicao.ValorUnitario);
        $("#cotacao-valor-reembolso").val(cotacaoItemEmEdicao.ValorReembolso);
        $("#cotacao-valor-total-reembolso").val($("#cotacao-valor-total-reembolso").val(cotacaoItemEmEdicao.ValorTotal).masked());
    } else {
        $("#cotacao-quantidade").val(cotacaoItemEmEdicao.Quantidade);
        $("#cotacao-valor-unitario").val(cotacaoItemEmEdicao.ValorUnitario);
        $("#cotacao-valor-total").val($("#cotacao-valor-total").val(cotacaoItemEmEdicao.ValorTotal).masked());
    }

    AlternarBotaoPeloStatus();
    AlternarBotaoHistoricoData();
    AlternarContainerPagamentoSeguro();
    AlternarContainerSeguroReembolso();
    $("#adicionar-cotacao-item").show();
    $("#cancelar-edicao-cotacao-item").show();
}

function cancelarEdicaoDeCotacaoItem() {
    cotacaoItens.push(cotacaoItemEmEdicao);

    atualizarCotacaoItem(cotacaoItens);

    limparCamposCotacao();
}

function limparCamposCotacao() {
    cotacaoItemEmEdicao = {};
    $("#cotacao-oficina").val("");
    $("#cotacao-peca-servico").val("");
    $("#cotacao-fornecedor").val("");
    $("#cotacao-forma-pagamento").val("");
    $("#cotacao-quantidade").val("");
    $("#cotacao-valor-unitario").val("");
    $("#cotacao-valor-total").val("");
    $("#data-servico").val("");
    document.getElementById("cotacao-tem-seguro-reembolso").checked = false;
    $("#cotacao-cia-seguro-reembolso").val("");
    $("#cotacao-data-prevista-reembolso").val("");
    $("#cotacao-quantidade-reembolso").val("");
    $("#cotacao-valor-unitario-reembolso").val("");
    $("#cotacao-valor-reembolso").val("");
    $("#cotacao-valor-total-reembolso").val("");

    AlternarBotaoPeloStatus();
    AlternarBotaoHistoricoData();
    AlternarContainerPagamentoSeguro();
    AlternarContainerSeguroReembolso();

    $("#btn-historico-data").hide();
    $("#adicionar-cotacao-item").hide();
    $('#cancelar-edicao-cotacao-item').hide();
}