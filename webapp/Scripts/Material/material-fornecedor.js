var materialFornecedores = [];
var materialFornecedorEmEdicao = {};
var indiceMaterialFornecedoresAdicionados = 0;

$(document).ready(function () {
    if (isEdit() || isSave()) {
        post("BuscarMaterialFornecedores")
            .done((result) => {
                materialFornecedores = result;
            })
    }

    $("#fornecedor").change(function () {
        ChecarSerFornecedorTemEmail();
    });

    $("#eh-personalizado").change(function () {
        if ($("#eh-personalizado").is(":checked"))
            $("#quantidade-para-pedido-automatico").val("");

        mostrarQuantidadeSeForPersonalizado();
    });
});

function ChecarSerFornecedorTemEmail() {
    return post("ChecarSerFornecedorTemEmail", { id: $("#fornecedor").val() })
        .done((temEmail) => {
            if (!temEmail) {
                toastr.warning("Fornecedor não possui email", "Alerta", { timeOut: 1500 });
                $('#eh-personalizado').prop("checked", false);
                $("#container-eh-personalizado").hide();
                $("#container-quantidade-para-pedido-automatico").hide();
                $("#quantidade-para-pedido-automatico").val("");
            } else {
                $("#container-eh-personalizado").show();
                $("#container-quantidade-para-pedido-automatico").hide();
            }
        });
}

function mostrarQuantidadeSeForPersonalizado() {
    if ($("#eh-personalizado").is(":checked"))
        $("#container-quantidade-para-pedido-automatico").show();
    else
        $("#container-quantidade-para-pedido-automatico").hide();
}

function atualizarMaterialFornecedor(materialFornecedores) {
    return post("AtualizarMaterialFornecedores", { materialFornecedores })
        .done((response) => {
            $("#lista-material-fornecedores").empty().append(response);
        });
}

function validarMaterialFornecedor({ Fornecedor, EhPersonalizado, QuantidadeParaPedidoAutomatico }) {
    if (!Fornecedor.Id) {
        toastr.warning("Selecione um Fornecedor", "Alerta");
        return false;
    }

    if (EhPersonalizado && !QuantidadeParaPedidoAutomatico) {
        toastr.warning("Informe a quantidade para pedido automático", "Alerta");
        return false;
    }

    return true;
}

function adicionarMaterialFornecedor() {
    indiceMaterialFornecedoresAdicionados--;

    var materialFornecedor = {
        Id: Object.keys(materialFornecedorEmEdicao).length > 0 ? materialFornecedorEmEdicao.Id : indiceMaterialFornecedoresAdicionados,
        Fornecedor: {
            Id: $("#fornecedor").val(),
            Nome: $("#fornecedor option:selected").text()
        },  
        EhPersonalizado: $("#eh-personalizado").is(":checked"),
        QuantidadeParaPedidoAutomatico: $("#quantidade-para-pedido-automatico").val(),
    }

    if (!validarMaterialFornecedor(materialFornecedor))
        return;

    if (materialFornecedor.EhPersonalizado) {
        materialFornecedores.forEach((item, index) => item.EhPersonalizado = false);
    }

    materialFornecedores.push(materialFornecedor);

    atualizarMaterialFornecedor(materialFornecedores);

    clearFields();
}

function removerMaterialFornecedor(id) {
    materialFornecedores = materialFornecedores.filter(x => x.Id != id);

    return atualizarMaterialFornecedor(materialFornecedores);
}

function editarMaterialFornecedor(id) {
    if (Object.keys(materialFornecedorEmEdicao).length)
        materialFornecedores.push(materialFornecedorEmEdicao);

    materialFornecedorEmEdicao = materialFornecedores.find(x => x.Id == id);
    removerMaterialFornecedor(id)
        .done(() => {
            ChecarSerFornecedorTemEmail()
                .done(() => {
                    mostrarQuantidadeSeForPersonalizado();
                });
        });

    $("#fornecedor").val(materialFornecedorEmEdicao.Fornecedor.Id);
    $("#eh-personalizado").prop("checked", materialFornecedorEmEdicao.EhPersonalizado);
    $("#quantidade-para-pedido-automatico").val(materialFornecedorEmEdicao.QuantidadeParaPedidoAutomatico);

    $("#cancelar-edicao-material-forcedor").show();
}

function cancelarEdicaoDeMaterialFornecedor() {
    materialFornecedores.push(materialFornecedorEmEdicao);

    atualizarMaterialFornecedor(materialFornecedores);

    clearFields();
}

function clearFields() {
    materialFornecedorEmEdicao = {};
    $('#fornecedor').val("");
    $('#eh-personalizado').prop("checked", false);
    $('#quantidade-para-pedido-automatico').val("");
    $('#cancelar-edicao-material-forcedor').hide();
    mostrarQuantidadeSeForPersonalizado();
}