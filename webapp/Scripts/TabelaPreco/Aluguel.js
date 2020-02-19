var alugueis = [];
var indiceAlugueisAdicionados = 0;

$(document).ready(function () {


    $.ajax({
        url: '/TabelaPreco/BuscarAlugueis',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            alugueis = response;
        }
    });
});
function adicionarAluguel() {

    indiceAlugueisAdicionados--;
    var aluguel = {
        Id: indiceAlugueisAdicionados,
        Descricao: $("#nomeAluguel").val(),
        Valor: $("#valorAluguel").val()
    }

    if (!validarAluguel(aluguel))
        return;

    alugueis.push(aluguel);

    atualizarAlugueis(alugueis);

}

function validarAluguel({ Descricao, Valor }) {
    if (Descricao === 0 || Descricao === "") {
        toastr.warning("informe o aluguel", "Alerta");
    }
    else if (!Valor || Valor === "") {
        toastr.warning("Informe o valor", "Alerta");
    } else {
        return true;
    }

    return false;
}

function atualizarAlugueis(alugueis) {
    showLoading();
    $.post("/TabelaPreco/AtualizarAlugueis", { alugueis })
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
                $("#lista-alugueis-result").empty();
                $("#lista-alugueis-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}


function removerAluguel(id) {
    alugueis = alugueis.filter(x => x.Id !== id);

    atualizarAlugueis(alugueis);
}

var aluguelEmEdicao = {};
function editarAluguel(id) {
    debugger;
    if (Object.keys(aluguelEmEdicao).length)
        alugueis.push(aluguelEmEdicao);

    aluguelEmEdicao = alugueis.find(x => x.Id === id);
    removeraluguel(id);

    $("#id").val(aluguelEmEdicao.Id);
    $("#nomealuguel").val(aluguelEmEdicao.Descricao);
    $("#valoraluguel").val(aluguelEmEdicao.Valor);
    $("#cancel").show();
}