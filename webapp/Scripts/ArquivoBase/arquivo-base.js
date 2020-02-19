let itemEmEdicacao = false;

$(document).ready(function () {
    BuscarArquivoBase();

    $("#arquivo-base-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
    });
});

function BuscarArquivoBase() {
    return get("BuscarArquivoBase", "ArquivoBase")
        .done((response) => {
            $("#lista-arquivo-base").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
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

    return post("ArmazenarDadosImpressao", { funcionarioId, dataEntrega, dataDevolucao }, "ArquivoBase");
}

function AltenarSelecionarTudo(el) {
    let selecionado = el.checked;

    if (selecionado)
        $(".item-checkbox").prop("checked", true);
    else
        $(".item-checkbox").prop("checked", false);

    return post("AltenarSelecionarTudo", { selecionado }, "ArquivoBase");
}

function AlternarItemSelecionado(materialId) {
    return post("AlternarItemSelecionado", { materialId }, "ArquivoBase");
}

function AdicionarFuncionarioItem() {
    let materialId = $("#material").val();
    let valor = $("#valor").val();
    let quantidade = $("#quantidade").val();
    let valorTotal = $("#valor-total").val();

    if (!materialId) {
        toastr.warning("Informe o Item", "Campo Obrigatório");
        return;
    }
    if (!valor || valor == '0,00') {
        toastr.warning("Informe o valor", "Campo Obrigatório");
        return;
    }
    if (!quantidade) {
        toastr.warning("Informe a Quantidade", "Campo Obrigatório");
        return;
    }

    post("AdicionarFuncionarioItem", { materialId, valor, quantidade, valorTotal }, "ArquivoBase")
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            LimparCampos();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    itemEmEdicacao = false;
}

function RemoverFuncionarioItem(materialId) {
    post("RemoverFuncionarioItem", { materialId }, "ArquivoBase")
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
        });
}

function EditarFuncionarioItem(materialId) {
    if (itemEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarFuncionarioItem", { materialId }, "ArquivoBase")
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            $("#material").val(response.Item.Material.Id);
            $("#valor").val(response.Item.Valor);
            $("#quantidade").val(response.Item.Quantidade);
            $("#valor-total").val(response.Item.ValorTotal);
            MakeChosen("material");
            itemEmEdicacao = true;
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

function LimparCampos() {
    $("#material").val("");
    $("#valor").val("");
    $("#quantidade").val("");
    $("#valor-total").val("0,00");
    MakeChosen("material");
}