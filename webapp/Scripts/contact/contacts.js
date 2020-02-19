var contatos = [];
var contatoEmEdicao = {};
var indiceContatosAdicionados = 0;

$(document).ready(function () {
    $("#telephone").mask("(99) 9999-9999");
    $("#cellphone").mask("(99) 99999-9999");
});

var idContactInEdit;

$("#tipoContato").on('change', function () {
    var tipoContato = $("#tipoContato").val();

    $("#telephone").val("");
    $("#cellphone").val("");
    $("#email").val("");

    switch (tipoContato) {
        case '0':
            $("#telephonebloco").hide();
            $("#cellphonebloco").hide();
            $("#emailbloco").hide();
            break;
        case '1':
            $("#telephonebloco").hide();
            $("#cellphonebloco").hide();
            $("#emailbloco").show();
            break;
        case '2':
            $("#telephonebloco").show();
            $("#cellphonebloco").hide();
            $("#emailbloco").hide();
            break;
        case '3':
            $("#telephonebloco").hide();
            $("#cellphonebloco").show();
            $("#emailbloco").hide();
            break;
        case '4':
            $("#telephonebloco").show();
            $("#cellphonebloco").show();
            $("#emailbloco").hide();

            break;
        case '5':
            $("#telephonebloco").show();
            $("#cellphonebloco").show();
            $("#emailbloco").hide();

            break;
        case '6':
            $("#telephonebloco").show();
            $("#cellphonebloco").hide();
            $("#emailbloco").hide();
            break;
        case '7':
            $("#telephonebloco").hide();
            $("#cellphonebloco").hide();
            $("#emailbloco").show();
            break;
        case '8':
            $("#telephonebloco").show();
            $("#cellphonebloco").show();
            $("#emailbloco").show();
            break;
        default:
            $("#telephonebloco").show();
            $("#cellphonebloco").show();
            $("#emailbloco").show();
            break;
    }
});

function buscarContatos() {
    return get("BuscarContatos", "Contato")
        .done((response) => contatos = response);
}

function adicionarContato() {
    indiceContatosAdicionados--;
    var tipo = $("#tipoContato").val();

    var contato = {
        Id: indiceContatosAdicionados,
        Email: $("#email").val(),
        Telefone: $("#telephone").val(),
        Celular: $("#cellphone").val(),
        Tipo: tipo,
        TipoTexto: $(`#tipoContato option[value=${tipo}]`).text()
    };

    if (!validarContato(contato))
        return;

    contatos.push(contato);

    atualizarContatos(contatos);

    clearFields();
}

function editarContato(id) {
    if (Object.keys(contatoEmEdicao).length)
        contatos.push(contatoEmEdicao);

    contatoEmEdicao = contatos.find(x => x.Id === id);
    removerContato(id);

    $("#email").val(contatoEmEdicao.Email);
    $("#telephone").val(contatoEmEdicao.Telefone);
    $("#cellphone").val(contatoEmEdicao.Celular);

    selecionarTipoDeContato(contatoEmEdicao.Tipo);

    $("#contato-cancelar-edicao").show();
}

function removerContato(id) {
    contatos = contatos.filter(x => x.Id !== id);

    atualizarContatos(contatos);
}

function cancelarEdicaoDeContato() {
    contatos.push(contatoEmEdicao);

    atualizarContatos(contatos);

    clearFields();
}

function atualizarContatos(contatos) {
    showLoading();
    $.post("/Contato/AtualizarContatos", { contatos })
        .done((response) => {
            if (typeof response === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#lista-contatos-grid").empty();
                $("#lista-contatos-grid").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function validarContato({ Email, Telefone, Celular, Tipo, TipoTexto }) {
    if (Tipo === 0) {
        toastr.warning("Selecione um Tipo de Contato", "Alerta");
        return false;
    }

    switch (TipoTexto) {
        case "Email":
        case "OutroEmail":
            if (!Email) {
                toastr.warning("Informe o E-mail", "Alerta");
                return false;
            }
            break;
        case "Residencial":
        case "Fax":
            if (!Telefone) {
                toastr.warning("Informe o Telefone", "Alerta");
                return false;
            }
            break;
        case "Celular":
            if (!Celular) {
                toastr.warning("Informe o Celular", "Alerta");
                return false;
            }
            break;
        case "Recado":
        case "Comercial":
            if (!Telefone && !Celular) {
                toastr.warning("Informe o Telefone ou o Celular", "Alerta");
                return false;
            }
            break;
        case "Padrão":
            break;
        default:
            return false;
    }

    return true;
}

function selecionarTipoDeContato(indice) {
    $("#tipoContato_chosen .chosen-results .result-selected").removeClass("result-selected");
    $(`#tipoContato_chosen .chosen-results .active-result[data-option-array-index='${indice}']`).addClass("result-selected");
    $("#tipoContato_chosen .chosen-single span").text($(`#tipoContato option[value='${indice}']`).text());
    $("#tipoContato").val(indice).change();
}

function getSelectedText(elementId) {
    var elt = document.getElementById(elementId);

    if (elt.selectedIndex === -1)
        return null;

    return elt.options[elt.selectedIndex].text;
}

function removeHiddenItems() {
    var hiddenItems = $('#data_table_contacts tbody tr:hidden');
    if (hiddenItems) hiddenItems.remove();
}

function clearFields() {
    idContactInEdit = 0;
    contatoEmEdicao = {};
    document.getElementById('email').value = "";
    document.getElementById('telephone').value = "";
    document.getElementById('cellphone').value = "";
    $("#telephone").val("");
    $("#cellphone").val("");
    $("#email").val("");
    $('#contato-cancelar-edicao').hide();
}

function VisibilidadeMsgSemContatos() {
    var totalContatos = $("#data_table_contacts tbody tr").length;
    var totalContatosInvisiveis = $("#data_table_contacts tbody tr:hidden").length;
    if (totalContatos === 0 || totalContatos === totalContatosInvisiveis)
        $('#data_table_contacts tbody').append("<tr class='odd'><td valign='top' colspan='2' class='dataTables_empty'>Nenhum registro encontrado</td></tr>");
    else
        $('#data_table_contacts tbody tr.odd').remove();
}