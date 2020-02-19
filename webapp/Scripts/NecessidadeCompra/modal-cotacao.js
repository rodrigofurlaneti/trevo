let cotacaoMaterialFornecedores = [];
let cotacaoMaterialFornecedorEmEdicao = {};
let indiceCotacaoMaterialFornecedoresAdicionados = 0;
let podeSolicitar = true;

$(document).ready(function () {
    FormatarNumerosInteiros("#cotacao-quantidade");
    FormatarNumerosDecimais("#cotacao-valor-total");

    $('#modal-cotacao').on('show.bs.modal', function (e) {
        limparCamposCotacao();

        BuscarDadosDoModal();
    })

    $("#cotacao-quantidade, #cotacao-valor").blur(function () {
        CalcularValorTotal();
    });

    $("#cotacao-valor").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
});

function AlternarBotaoSolicitacao() {
    if (!podeSolicitar ||
        Object.keys(cotacaoMaterialFornecedorEmEdicao).length > 0 ||
        cotacaoMaterialFornecedores.some(x => !x.Valor) ||
        cotacaoMaterialFornecedores.length == 0) {
        $("#btn-solicitar-cotacao-disabled").show();
        $("#btn-solicitar-cotacao").hide();
    }
    else {
        $("#btn-solicitar-cotacao-disabled").hide();
        $("#btn-solicitar-cotacao").show();
    }
}

function SalvarCotacao() {
    return post("SalvarCotacao", { necessidadeCompraId: modalNecessidadeCompraId })
        .done((result) => $('#modal-principal').empty().append(result));
}

function BuscarDadosDoModal() {
    return post("BuscarDadosDoModal", { necessidadeCompraId: modalNecessidadeCompraId })
        .done((response) => {
            cotacaoMaterialFornecedores = response.MaterialFornecedores;
            podeSolicitar = response.PodeSolicitar;

            AlternarBotaoSolicitacao();

            $("#lista-cotacao-material-fornecedores").empty().append(response.Grid)
        });
}

function CalcularValorTotal() {
    let quantidade = $("#cotacao-quantidade").int();
    let valor = $("#cotacao-valor").decimal();
    let valorTotal = valor * quantidade;

    if (!isNaN(valorTotal)) {
        let valorComMascara = $("#cotacao-valor-total").val(valorTotal.toFixed(2)).masked();
        $("#cotacao-valor-total").val(valorComMascara);
    }
}

function atualizarCotacaoMaterialFornecedor(cotacaoMaterialFornecedores) {
    return post("AtualizarCotacaoMaterialFornecedores", { cotacaoMaterialFornecedores })
        .done((response) => {
            $("#lista-cotacao-material-fornecedores").empty().append(response);
        });
}

function validarCotacaoMaterialFornecedor({ Quantidade, Valor }) {
    if (!Quantidade) {
        toastr.warning("Informe a quantidade", "Alerta");
        return false;
    }

    if (!Valor || Valor == "0,00") {
        toastr.warning("Informe o valor unitário", "Alerta");
        return false;
    }

    return true;
}

function adicionarCotacaoMaterialFornecedor() {
    indiceCotacaoMaterialFornecedoresAdicionados--;

    var cotacaoMaterialFornecedor = {
        Id: Object.keys(cotacaoMaterialFornecedorEmEdicao).length > 0 ? cotacaoMaterialFornecedorEmEdicao.Id : indiceCotacaoMaterialFornecedoresAdicionados,
        Fornecedor: cotacaoMaterialFornecedorEmEdicao.Fornecedor,
        Material: cotacaoMaterialFornecedorEmEdicao.Material,
        Valor: $("#cotacao-valor").valDecimal(),
        ValorTotal: $("#cotacao-valor-total").valDecimal(),
        Quantidade: $("#cotacao-quantidade").val()
    }

    if (!validarCotacaoMaterialFornecedor(cotacaoMaterialFornecedor))
        return;

    cotacaoMaterialFornecedores.push(cotacaoMaterialFornecedor);

    atualizarCotacaoMaterialFornecedor(cotacaoMaterialFornecedores);

    limparCamposCotacao();
}

function removerCotacaoMaterialFornecedor(id) {
    cotacaoMaterialFornecedores = cotacaoMaterialFornecedores.filter(x => x.Id != id);

    AlternarBotaoSolicitacao();
    return atualizarCotacaoMaterialFornecedor(cotacaoMaterialFornecedores);
}

function editarCotacaoMaterialFornecedor(id) {
    if (Object.keys(cotacaoMaterialFornecedorEmEdicao).length)
        cotacaoMaterialFornecedores.push(cotacaoMaterialFornecedorEmEdicao);

    cotacaoMaterialFornecedorEmEdicao = cotacaoMaterialFornecedores.find(x => x.Id == id);
    removerCotacaoMaterialFornecedor(id);

    $("#cotacao-fornecedor").val(cotacaoMaterialFornecedorEmEdicao.Fornecedor.Nome);
    $("#cotacao-quantidade").val(cotacaoMaterialFornecedorEmEdicao.Quantidade);
    $("#cotacao-material").val(cotacaoMaterialFornecedorEmEdicao.Material.Nome);
    $("#cotacao-valor").val(cotacaoMaterialFornecedorEmEdicao.Valor);
    $("#cotacao-valor-total").val($("#cotacao-valor-total").val(cotacaoMaterialFornecedorEmEdicao.ValorTotal).masked());

    AlternarBotaoSolicitacao();
    $("#adicionar-cotacao-material-forcedor").show();
    $("#cancelar-edicao-cotacao-material-forcedor").show();
}

function cancelarEdicaoDeCotacaoMaterialFornecedor() {
    cotacaoMaterialFornecedores.push(cotacaoMaterialFornecedorEmEdicao);

    atualizarCotacaoMaterialFornecedor(cotacaoMaterialFornecedores);

    limparCamposCotacao();
}

function limparCamposCotacao() {
    cotacaoMaterialFornecedorEmEdicao = {};
    AlternarBotaoSolicitacao();
    $("#cotacao-fornecedor").val("");
    $("#cotacao-quantidade").val("");
    $("#cotacao-material").val("");
    $("#cotacao-valor").val("");
    $("#cotacao-valor-total").val("");
    $("#adicionar-cotacao-material-forcedor").hide();
    $('#cancelar-edicao-cotacao-material-forcedor').hide();
}