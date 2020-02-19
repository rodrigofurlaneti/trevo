var convenios = [];
var indiceConveniosAdicionados = 0;

$(document).ready(function () {
    $.ajax({
        url: '/TabelaPreco/BuscarConvenios',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            convenios = response;
        }
    });
});

function adicionarConvenio() {
   
    indiceConveniosAdicionados--;
    var convenio = {
        Id: indiceConveniosAdicionados,
        Descricao: $("#nomeConvenio").val(),
        Valor: $("#valorConvenio").val()
    }

    if (!validarConvenio(convenio))
        return;

    convenios.push(convenio);

    atualizarConvenios(convenios);
}

function validarConvenio({ Descricao, Valor }) {
    if (Descricao === 0 || Descricao === "") {
        toastr.warning("informe o convenio", "Alerta");
    }
    else if (!Valor || Valor === "") {
        toastr.warning("Informe o valor", "Alerta");
    } else {
        return true;
    }

    return false;
}

function atualizarConvenios(convenios) {
    showLoading();
    $.post("/TabelaPreco/AtualizarConvenios", { convenios })
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
                $("#lista-convenios-result").empty();
                $("#lista-convenios-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}


function removerConvenio(id) {
    convenios = convenios.filter(x => x.Id !== id);

    atualizarConvenios(convenios);
}

var convenioEmEdicao = {};
function editarConvenio(id) {
    debugger;
    if (Object.keys(convenioEmEdicao).length)
        convenios.push(convenioEmEdicao);

    convenioEmEdicao = convenios.find(x => x.Id === id);
    removerconvenio(id);

    $("#id").val(convenioEmEdicao.Id);
    $("#nomeconvenio").val(convenioEmEdicao.Descricao);
    $("#valorconvenio").val(convenioEmEdicao.Valor);
    $("#cancel").show();
}