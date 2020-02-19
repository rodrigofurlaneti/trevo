let materialFornecedores = [];
let materialFornecedorEmEdicao = {};
let indiceMaterialFornecedoresAdicionados = 0;
let modalNecessidadeCompraId = 0;

$(document).ready(function () {
    BuscarNecessidadeCompras()
        .done(() => {
            if (isEdit()) {
                post("BuscarNecessidadeCompraMaterialFornecedores")
                    .done((result) => materialFornecedores = result);
            } else if (location.pathname.includes("NecessidadeCompra/Cotacao")) {
                AbrirModalCotacao(location.pathname.split("/").pop());
            }
        });

    ConfigTabelaGridSemCampoFiltroPrincipal();

    FormatarCampoData("data-notificacao-validade");
    FormatarNumerosInteiros("#quantidade");

    $("#material").change(function () {
        $("#quantidade, #fornecedor").val("");

        BuscarFornecedoresDoMaterial($("#material").val());
    })

    $("#necessidade-compra-form").submit(function (e) {
        if (!ValidarCampos())
            e.preventDefault();
    });
});

function BuscarFornecedoresDoMaterial(materialId) {
    if (!materialId)
        materialId = 0;

    return post("BuscarFornecedoresDoMaterial", { materialId })
        .done((response) => {
            $("#combo-fornecedores").empty().append(response);
        });
}

function BuscarNecessidadeCompras() {
    return post("BuscarNecessidadeCompras")
        .done((result) => $("#lista-necessidade-compra").empty().append(result));
}

function ValidarCampos() {
    if (!$(".item-material-fornecedor").length) {
        toastr.warning("Adicione pelo menos 1 item no grid", "Alerta");
        return false;
    }

    if (!$("#data-notificacao-validade").val()) {
        toastr.warning("Informe a data de validade da notificação", "Alerta");
        return false;
    }

    return true;
}

function AbrirModalCotacao(necessidadeCompraId) {
    modalNecessidadeCompraId = necessidadeCompraId;
    $("#modal-cotacao").modal("show");
}


function atualizarMaterialFornecedor(materialFornecedores) {
    return post("AtualizarMaterialFornecedores", { materialFornecedores })
        .done((response) => {
            $("#lista-material-fornecedores").empty().append(response);
        });
}

function validarMaterialFornecedor({ Fornecedor, Material, Quantidade }) {
    if (!Material.Id) {
        toastr.warning("Selecione um Material", "Alerta");
        return false;
    }

    if (!Quantidade) {
        toastr.warning("Informe a quantidade", "Alerta");
        return false;
    }

    if (!Fornecedor.Id) {
        toastr.warning("Selecione um Fornecedor", "Alerta");
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
        Material: {
            Id: $("#material").val(),
            Nome: $("#material option:selected").text()
        },
        Quantidade: $("#quantidade").val(),
    }

    if (!validarMaterialFornecedor(materialFornecedor))
        return;

    materialFornecedores.push(materialFornecedor);

    atualizarMaterialFornecedor(materialFornecedores);

    limparCampos();
}

function removerMaterialFornecedor(id) {
    materialFornecedores = materialFornecedores.filter(x => x.Id != id);

    return atualizarMaterialFornecedor(materialFornecedores);
}

function editarMaterialFornecedor(id) {
    if (Object.keys(materialFornecedorEmEdicao).length)
        materialFornecedores.push(materialFornecedorEmEdicao);

    materialFornecedorEmEdicao = materialFornecedores.find(x => x.Id == id);
    removerMaterialFornecedor(id);

    $("#fornecedor").val(materialFornecedorEmEdicao.Fornecedor.Id);
    $("#quantidade").val(materialFornecedorEmEdicao.Quantidade);
    $("#material").val(materialFornecedorEmEdicao.Material.Id);

    $("#cancelar-edicao-material-forcedor").show();
}

function cancelarEdicaoDeMaterialFornecedor() {
    materialFornecedores.push(materialFornecedorEmEdicao);

    atualizarMaterialFornecedor(materialFornecedores);

    limparCampos();
}

function limparCampos() {
    materialFornecedorEmEdicao = {};
    $('#fornecedor').val("");
    $('#cancelar-edicao-material-forcedor').hide();
}