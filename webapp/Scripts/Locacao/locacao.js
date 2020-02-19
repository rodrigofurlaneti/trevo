var listahorarioparametro = [];
var items = [];

$(document).ready(function () {

    MakeChosen("unidade", null, "100%");
    MakeChosen("tipolocacao", null, "100%");

    MetodoUtil();

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    if (location.search.toLowerCase().includes("pedidoid")) {
        let pedidoId = obterParametroDaUrl("pedidoId");
        ExecutarModal(pedidoId);
    }
});

function InativarPedido(id) {

    $.ajax({
        url: "/Locacao/InativarPedido",
        type: "POST",
        data: { id },
        success: function (result) {
            if (typeof (result) === "object") {

                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                openCustomModal(null,
                    null,
                    "success",
                    "Sucesso",
                    "Sucesso ao inativar",
                    false,
                    null,
                    function () {
                        $("#lista-pedidos").empty();
                        $("#lista-pedidos").append(result);
                    });

            }
        },
        error: function (error) {

            $("#lista-pedidos").empty();
            $("#lista-pedidos").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function Pesquisar() {
    //debugger;
    var unidade = $("#unidade").val();
    var tipolocacao = $("#tipolocacao").val();

    var filtro = {
        Unidade: { Id: unidade },
        TipoLocacao: { Id: tipolocacao }
    };

    $.ajax({
        url: "/Locacao/Pesquisar",
        type: "POST",
        dataType: "json",
        data: {
            filtro: filtro
        },
        success: function (result) {
            if (typeof (result) === "object") {
                
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                
                $("#lista-pedidos").empty();
                $("#lista-pedidos").append(result);
            }
        },
        error: function (error) {
            
            $("#lista-pedidos").empty();
            $("#lista-pedidos").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            
            MetodoUtil();
            hideLoading();
        }
    });
}

function ConfirmarAlteracao() {
    var id = $("#idemissaoselo").val();
    var reajuste = $("#reajuste").val();
    var valorreajuste = $("#valorreajuste").val();
    var vigencia = $("#vigencia").val();

    var model = {
        Id: id,
        DataReajuste: reajuste,
        ValorReajuste: valorreajuste,
        DataVigenciaFim: vigencia
    }

    return post("ConfirmarAlteracao", { model })
        .done((response) => {
            Pesquisar();
            ObterNotificacoes();
        });
}

function ExecutarModal(id) {

    var idsLancamentosCobranca = id

    $.ajax({
        url: "/Locacao/ExecutarModal",
        type: "POST",
        dataType: "json",
        data: { idsLancamentosCobranca },
        success: function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#modalBodyPagamento").empty();
                $("#modalBodyPagamento").append(result);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            }
            hideLoading();
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalBodyPagamento").append(error.responseText);
            $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            
        }
    });
}

function ExecutarPagamento() {

    var id = $("#executarpagamento").val();



    $.ajax({
        url: "/contaPagar/ExecutarPagamento",
        type: "POST",
        async: true,
        dataType: "json",
        data: {
            id: id
        },
        success: function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#lista-pedidos").empty();
                $("#lista-pedidos").append(result);
            }
        },
        error: function (error) {
            hideLoading();
            $("#lista-pedidos").empty();
            $("#lista-pedidos").append(error.responseText);

        },
        beforeSend: function () {
        },
        complete: function () {
            MetodoUtil();
        }
    });

}

function disabledChosen(id) {
    $("#" + id).prop('disabled', true).trigger('chosen:updated').prop('disabled', false);
}

function makeReadonly(id) {
    $("#" + id).prop('readonly', true);
}

//subitens 

var indiceitemsAdicionados = 0;

function adicionarItem() {

    var usarId = 0;
    var novo = true;
    if (Object.keys(itemEmEdicao).length) {
        novo = false;
        usarId = itemEmEdicao.Id;
    }
    else
        usarId = indiceitemsAdicionados;

    var item = {
        Id: usarId,
        Descricao: $("#descricao").val(),
        Valor: $("#valoritem").val()
    }

    if (!item.Descricao) {
        toastr.warning("Informe uma Descrição", "Alerta");
        return false;
    }
    else if (!item.Valor) {
        toastr.warning("Informe um Valor", "Alerta");
        return false;
    }

    if (novo)
        indiceitemsAdicionados--;

    items.push(item);

    atualizarItems(items);

    LimpaCamposItens();
}

function LimpaCamposItens() {
    $("#descricao").val('');
    $("#valoritem").val('');
    itemEmEdicao = {};
    $("#cancel").hide();
}

function cancelarEdicaoDeItem() {
    items.push(itemEmEdicao);

    atualizarItems(items);

    LimpaCamposItens();
}

function ativar(id) {
    let item = items.find(x => x.Id == id);

    if (item.Ativo) {
        toastr.error("Este item já está ativo", "Item ativo");
        return;
    }
    
    item.Ativo = true;

    atualizarItems(items);

    toastr.success('Item ativado. Clique \'Solicitar Aprovação\' para confirmar', 'Sucesso!');
}

function inativar(id) {
    let item = items.find(x => x.Id == id);

    if (!item.Ativo) {
        toastr.error("Este item já está inativo", "Item inativo");
        return;
    }

    item.Ativo = false;

    atualizarItems(items);

    toastr.warning('Item inativado. Clique \'Solicitar Aprovação\' para confirmar', 'Sucesso!');
}


function atualizarItems(items) {
    showLoading();

    for (var i = 0; i < items.length; i++) {
        var valorAux = items[i].Valor.toString();

        if (valorAux != '' || valorAux != undefined || valorAux != null) {
            items[i].Valor = valorAux.replace('.', ',');
        }
    }

    $.post("/Locacao/atualizarItems", { items })
        .done((response) => {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#lista-item-result").empty();
                $("#lista-item-result").append(response);

            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function removerItem(id) {
    items = items.filter(x => x.Id !== id);

    atualizarItems(items);
}


var itemEmEdicao = {};
function editarItem(id) {
    if (Object.keys(itemEmEdicao).length)
        items.push(itemEmEdicao);

    itemEmEdicao = items.find(x => x.Id === id);
    removerItem(id);

    $("#id").val(itemEmEdicao.Id);

    var valorString = itemEmEdicao.Valor.toString();

    if (itemEmEdicao.Valor != '' || itemEmEdicao.Valor != undefined || itemEmEdicao.Valor != null)
        $("#valoritem").val(valorString.replace('.', ','));

    $("#descricao").val(itemEmEdicao.Descricao);

    $("#cancel").show();
}






