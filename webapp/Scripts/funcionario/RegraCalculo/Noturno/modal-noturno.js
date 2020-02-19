let intervaloModalNoturnoEmEdicacao = false;

$(document).ready(function () {
    FormatarCampoDataPelaClasse("data");

    $('#modal-noturno').on('show.bs.modal', function (e) {
        AtualizarGridIntervaloModalNoturno();
    })
});

function AtualizarGridIntervaloModalNoturno() {
    let funcionarioId = $("#hdnFuncionario").val();

    post("AtualizarGridIntervaloNoturno", { funcionarioId })
        .done((response) => {
            $("#lista-intervalo-noturno").empty().append(response.Grid);
        });
}

function AdicionarIntervaloModalNoturno() {
    let funcionarioId = $("#hdnFuncionario").val();

    let dataInicial = $("#modal-noturno-data-inicial").val();
    let dataFinal = $("#modal-noturno-data-final").val();

    if (!dataInicial) {
        toastr.warning("Informe a data inicial", "Campo Obrigatório");
        return;
    }
    if (!dataFinal) {
        toastr.warning("Informe a data final", "Campo Obrigatório");
        return;
    }

    post("AdicionarIntervaloNoturno", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#lista-intervalo-noturno").empty().append(response.Grid);
            LimparCamposIntervaloModalNoturno();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloModalNoturnoEmEdicacao = false;
}

function RemoverIntervaloModalNoturno(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    post("RemoverIntervaloNoturno", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#lista-intervalo-noturno").empty().append(response.Grid);
        });
}

function EditarIntervaloModalNoturno(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    if (intervaloModalNoturnoEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarIntervaloNoturno", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#modal-noturno-data-inicial").val(response.Item.DataInicialString);
            $("#modal-noturno-data-final").val(response.Item.DataFinalString);
            $("#lista-intervalo-noturno").empty().append(response.Grid);
            intervaloModalNoturnoEmEdicacao = true;
        });
}

function LimparCamposIntervaloModalNoturno() {
    $("#modal-noturno-data-inicial").val("");
    $("#modal-noturno-data-final").val("");
}