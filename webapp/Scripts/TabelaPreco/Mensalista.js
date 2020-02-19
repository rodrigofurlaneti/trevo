var mensalistas = [];
var indiceMensalistasAdicionados = 0;
var idMensalistaInEdit;

$(document).ready(function () {

    $.ajax({
        url: '/TabelaPreco/BuscarMensalistas',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            mensalistas = response;
        }
    });

   

});




function adicionarMensalista() {
   
    indiceMensalistasAdicionados--;
    var mensalista = {
        Id: indiceMensalistasAdicionados,
        Descricao: $(".nomeMensalista").val(),
        Valor: $(".valorMensalista").val()
    }

    if (!validarMensalista(mensalista))
        return;

    mensalistas.push(mensalista);

    atualizarMensalistas(mensalistas);

}

function validarMensalista({ Descricao, Valor }) {
    if (Descricao === 0 || Descricao === "") {
        toastr.warning("informe o Mensalista", "Alerta");
    }
    else if (!Valor || Valor === "") {
        toastr.warning("Informe o valor", "Alerta");
    } else {
        return true;
    }

    return false;
}

function atualizarMensalistas(mensalistas) {
    showLoading();

    $.post("/TabelaPreco/AtualizarMensalistas", { mensalistas })
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
                $("#lista-mensalistas-result").empty();
                $("#lista-mensalistas-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });

    $('#valorMensalista').maskMoney({ decimal: ',', thousands: '.', precision: 2 });
}


function removerMensalista(id) {
    mensalistas = mensalistas.filter(x => x.Id !== id);

    atualizarMensalistas(mensalistas);
}

var mensalistaEmEdicao = {};

function editarMensalista(id) {
    if (Object.keys(mensalistaEmEdicao).length)
        mensalistas.push(mensalistaEmEdicao);

    mensalistaEmEdicao = mensalistas.find(x => x.Id === id);
    removerMensalista(id);

    $("#id").val(mensalistaEmEdicao.Id);
    $("#nomeMensalista").val(mensalistaEmEdicao.Descricao);
    $("#valorMensalista").val(mensalistaEmEdicao.Valor);
    $("#cancel").show();
}
