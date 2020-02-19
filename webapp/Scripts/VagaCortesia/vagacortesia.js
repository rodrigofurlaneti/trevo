var listahorarioparametro = [];
var items = [];

$(document).ready(function () {

    $("#todos").change(function () {
        HabilitaDesabilita($(this));
    });

    FormatarCampos();

    if (isEdit()) {
        showLoading();
        var id = location.pathname.split('/').pop();

        $.post(`/VagaCortesia/BuscarDadosDosGrids/${id}`)
            .done((response) => {
                items = response.items;
            })
            .always(() => { hideLoading(); });
    }

    ClienteAutoComplete("clientes", "cliente", "clienteText", VerificarSeJaTemCadastro);
});

function VerificarSeJaTemCadastro(clienteId) {
    if (clienteId) {
        return get(`VerificarSeJaTemCadastro?clienteId=${clienteId}`)
            .done((response) => {
                if (response.JaExiste === true) {
                    let id = location.pathname.split("/").pop();

                    if (isEdit() && id === response.VagaCortesiaId === id)
                        return;

                    let mensagem = `Já existe cadastro para esse cliente. <a href="/vagacortesia/edit/${response.VagaCortesiaId}">Clique aqui para editar</a>`;
                    openCustomModal(null, null, "warning", "Alerta", mensagem, false, null, null);
                }
            });
    }
    return;
}

function HabilitaDesabilita(x) {
    var valor = false;
    if ($(x).is(':checked'))
        valor = true;
    else
        valor = false;

    $('input[name="selecionalinha"]').prop("checked", valor)

    for (var i = 0; i < items.length; i++) {
        items[i].Selecionado = valor;
    }
}

function DeleteSelecionados() {
    $("#tablevigencia tbody").find('tr').each(function (rowIndex, r) {

        var id = $(this).find('input').attr('data-id');

        for (var i = 0; i < items.length; i++) {
            if (items[i].Id === ConvertToFloat(id)) {
                if (items[i].Selecionado) {
                    items = items.filter(x => x.Id !== items[i].Id);
                }
            }
        }
    });

    atualizarItems(items);
}

function AdicionaSelecionado(x, id) {
    var valor = $(x).is(':checked');

    for (var i = 0; i < items.length; i++) {
        if (items[i].Id === id) {
            items[i].Selecionado = valor;
        }
    }

    atualizarItems(items);
}

function validarItens(cliente) {

    if (cliente == null || cliente == '' || cliente == undefined || cliente == '') {
        toastr.error('Selecione um Cliente!', 'Selecione um Cliente!');
        return false;
    }

    return true;
}

function validarSubItens(unidade) {

    if (unidade <= 0) {
        return false;
    }

    return true;
}

function SalvarDados() {

    var listahorarioparametro = [];
    var Id = $("#hdnVagaCortesia").val();

    var cliente = $("#cliente").val();

    if (!validarItens(cliente))
        return;

    var model = {
        Id: Id,
        Cliente: {
            Id: cliente
        }
    }

    $.ajax({
        url: "/VagaCortesia/SalvarDados",
        type: "POST",
        data: { model },
        success: function (result) {
            hideLoading();
            openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () {
                if (result.Sucesso) {
                    window.location.href = "/VagaCortesia/Index/";
                }
            });
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function FormatarCampos() {
    MakeChosen("unidade");
    FormatarCampoHora(".hora")
    FormatarCampoData("datainicio")
    FormatarCampoData("datafim")
}

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
        Unidade: {
            Id: $("#unidade option:selected").val(),
            Nome: $("#unidade option:selected").text()
        },
        DataInicio: $("#datainicio").val(),
        DataFim: $("#datafim").val(),
        HorarioInicio: $("#horarioinicio").val(),
        HorarioFim: $("#horariofim").val()
    }

    if (item.Unidade.Id === "0" || item.Unidade.Id === "" || item.Unidade.Id === undefined) {
        toastr.warning("Selecione uma Unidade", "Alerta");
        return false;
    }
    else if (!item.DataInicio) {
        toastr.warning("Informe uma data inicio", "Alerta");
        return false;
    }
    else if (!item.HorarioInicio) {
        toastr.warning("Informe um horario inicio", "Alerta");
        return false;
    }
    else if (!item.DataFim) {
        toastr.warning("Informe uma data fim", "Alerta");
        return false;
    }
    else if (!item.HorarioFim) {
        toastr.warning("Informe um horario fim", "Alerta");
        return false;
    }

    if (novo)
        indiceitemsAdicionados--;

    items.push(item);

    atualizarItems(items);

    LimpaCamposItens();
}

function LimpaCamposItens() {
    $("#unidade").val("0").trigger("chosen:updated");
    $("#datainicio").val('');
    $("#datafim").val('');
    $("#horarioinicio").val('');
    $("#horariofim").val('');
    itemEmEdicao = {};
    $("#cancel").hide();
}

function cancelarEdicaoDeItem() {
    items.push(itemEmEdicao);

    atualizarItems(items);

    LimpaCamposItens();
}


function atualizarItems(items) {
    showLoading();
    $.post("/VagaCortesia/atualizarItems", { items })
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
    $("#unidade").val(itemEmEdicao.Unidade.Id);

    $("#datainicio").val(itemEmEdicao.DataInicio);
    $("#datafim").val(itemEmEdicao.DataFim);
    $("#horarioinicio").val(itemEmEdicao.HorarioInicio);
    $("#horariofim").val(itemEmEdicao.HorarioFim);

    selecionarTipoDeitem(itemEmEdicao.Unidade.Id);

    $("#cancel").show();
}

function selecionarTipoDeitem(indice) {
    $("#unidade_chosen .chosen-results .result-selected").removeClass("result-selected");
    $(`#unidade_chosen .chosen-results .active-result[data-option-array-index='${indice}']`).addClass("result-selected");
    $("#unidade_chosen .chosen-single span").text($(`#unidade option[value='${indice}']`).text());
    $("#unidade").val(indice);
}
