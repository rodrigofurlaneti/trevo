let itemEmEdicacao = false;

$(document).ready(function () {
    FuncionarioAutoComplete("responsaveis-entrega", "responsavel-entrega");
    FuncionarioAutoComplete("responsaveis-devolucao", "responsavel-devolucao");
    FormatarCampoDataPelaClasse("campo-data");
    MakeChosen("material");
    MakeChosen("estoque");
    FormatarReal("valmoney");
    FormatarNumerosDecimais("#valor-total");

    $("#quantidade, #valor").blur(function () {
        CalcularValorTotal();
    });

    $("#material").change(function () {
        BuscarEstoqueDoMaterial(this.value);
    });

    $("#campos-item-funcionario").on("change", "#estoque", function () {
        if(this.value)
            BuscarPrecoDoMaterialNoEstoque($("#material").val(), this.value);
    });
});

function BuscarEstoqueDoMaterial(materialId) {
    return post("BuscarEstoqueDoMaterial", { materialId }, "ItemFuncionario")
        .done((response) => {
            $("#select-estoque").empty().append(response);
            MakeChosen("estoque");
        });
}

function BuscarPrecoDoMaterialNoEstoque(materialId, estoqueId) {
    return get(`BuscarPrecoDoEstoque?materialId=${materialId}&estoqueId=${estoqueId}`, "Material")
        .done((response) => {
            $("#valor").val(response.Preco);
        })
        .fail((error) => {
            $("#valor").val("0,00");
            openCustomModal(null, null, "warning", "Alerta", error.statusText);
        })
        .always(() => CalcularValorTotal());
}

function Imprimir() {
    return ArmazenarDadosImpressao()
        .done(() => {
            document.getElementById("botao-impressao").click();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });
}

function ArmazenarDadosImpressao() {
    let funcionarioId = $("#campo-funcionario").val();
    let dataEntrega = $("#data-entrega").val();
    let dataDevolucao = $("#data-devolucao").val();

    return post("ArmazenarDadosImpressao", { funcionarioId, dataEntrega, dataDevolucao }, "ItemFuncionario");
}

function AltenarSelecionarTudo(el) {
    let selecionado = el.checked;

    if (selecionado)
        $(".item-checkbox").prop("checked", true);
    else
        $(".item-checkbox").prop("checked", false);

    return post("AltenarSelecionarTudo", { selecionado }, "ItemFuncionario");
}

function AlternarItemSelecionado(materialId) {
    return post("AlternarItemSelecionado", { materialId }, "ItemFuncionario");
}

function AdicionarFuncionarioItem() {
    if (location.pathname.toLowerCase().includes("itemfuncionario") && !$("#campo-funcionario").val()) {
        toastr.warning("Informe o Funcionário antes de adicionar um registro", "Alerta");
        return false;
    }

    let materialId = $("#material").val();
    let estoqueId = $("#estoque").val();
    let valor = $("#valor").val();
    let quantidade = $("#quantidade").val();
    let valorTotal = $("#valor-total").val();

    if (!materialId) {
        toastr.warning("Informe o Item", "Campo Obrigatório");
        return;
    }
    if (!estoqueId) {
        toastr.warning("Informe o Estoque", "Campo Obrigatório");
        return;
    }
    if (!valor || valor == '0,00') {
        toastr.warning("Selecione um item com valor", "Campo Obrigatório");
        return;
    }
    if (!quantidade) {
        toastr.warning("Informe a Quantidade", "Campo Obrigatório");
        return;
    }

    post("AdicionarFuncionarioItem", { materialId, estoqueId, valor, quantidade, valorTotal }, "ItemFuncionario")
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            LimparCamposItemFuncionario();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    itemEmEdicacao = false;
}

function RemoverFuncionarioItem(estoqueMaterialId) {
    post("RemoverFuncionarioItem", { estoqueMaterialId }, "ItemFuncionario")
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            if ($("#lista-item .item").length == 0) {
                $("#data-entrega, #responsaveis-entrega, #data-devolucao, #responsaveis-devolucao, #responsavel-entrega, #responsavel-devolucao").val("");
            }
        });
}

function EditarFuncionarioItem(estoqueMaterialId) {
    if (itemEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarFuncionarioItem", { estoqueMaterialId }, "ItemFuncionario")
        .done((response) => {
            BuscarEstoqueDoMaterial(response.Item.Material.Id)
                .done(() => {
                    $("#lista-item").empty().append(response.Grid);
                    $("#material").val(response.Item.Material.Id);
                    $("#estoque").val(response.Item.EstoqueMaterial.Estoque.Id);
                    $("#valor").val(response.Item.Valor);
                    $("#quantidade").val(response.Item.Quantidade);
                    $("#valor-total").val(response.Item.ValorTotal);
                    MakeChosen("material");
                    MakeChosen("estoque");
                    itemEmEdicacao = true;
                });
        });
}

function CalcularValorTotal() {
    let quantidade = $("#quantidade").int();
    let valor = $("#valor").decimal();
    let valorTotal = valor * quantidade;

    if (!isNaN(valorTotal)) {
        let valorComMascara = $("#valor-total").val(valorTotal.toFixed(2)).masked();
        $("#valor-total").val(valorComMascara);
    }
}

function LimparCamposItemFuncionario() {
    $("#material").val("");
    $("#estoque").empty().append("<option value=''>Selecione um...</option>").val("");
    $("#valor").val("0,00");
    $("#quantidade").val("");
    $("#valor-total").val("0,00");
    MakeChosen("material");
    MakeChosen("estoque");
}