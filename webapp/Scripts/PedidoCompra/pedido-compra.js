$(document).ready(function () {
    MakeChosen("forma-pagamento", null, "100%");
    MakeChosen("tipo-pagamento", null, "100%");
    MakeChosen("cotacao", null, "100%");

    DesabilitarEstoqueOuUnidade();
    FormatarEstoqueEUnidade();
    BuscarPedidoCompra();

    $("#estoque, #unidade").change(function () {
        DesabilitarEstoqueOuUnidade();
        FormatarEstoqueEUnidade();
    });

    $("#cotacao").change(function () {
        let cotacaoId = $(this).val();

        if (!cotacaoId) {
            $("#lista-cotacao-material-fornecedores").empty();
        } else {
            post("AtualizarCotacaoMaterialFornecedores", { cotacaoId })
                .done(result => $("#lista-cotacao-material-fornecedores").empty().append(result));
        }
    });


    $("#pedido-compra-form").submit(function (e) {
        if (!ValidarForm(this.id))
            e.preventDefault();

        else if (!ValidarCampos(this.id))
            e.preventDefault();
    });
});

function DesabilitarEstoqueOuUnidade() {
    $("#unidade").unDisabled();
    $("#estoque").unDisabled();

    if ($("#unidade").val()) {
        $("#estoque").disabled();
    }

    if ($("#estoque").val())
        $("#unidade").disabled();
}

function FormatarEstoqueEUnidade(){
    MakeChosen("estoque", null, "100%");
    MakeChosen("unidade", null, "100%");
}

function ValidarCampos() {
    if (!$("#unidade").val() && !$("#estoque").val()) {
        toastr.warning(`É obrigatório a Unidade ou o Estoque`, "Alerta");
        return false;
    }

    if (!$(".select-item:checked").length) {
        toastr.warning(`Selecione pelo menos 1 item`, "Alerta");
        return false;
    }

    return true;
}

function BuscarPedidoCompra() {
    return post("BuscarPedidoCompra")
        .done((result) => {
            $("#lista-pedido-compra").empty().append(result);
            ConfigTabelaGridSemCampoFiltroPrincipal("#lista-pedido-compra #datatable_fixed_column");
        });
}

function AlterarStatusDoItem(el, id) {
    return post("AlterarStatusDoItem", { id });
}