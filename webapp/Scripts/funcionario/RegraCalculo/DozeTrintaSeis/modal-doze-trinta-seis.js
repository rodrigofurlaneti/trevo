let intervaloModalDozeTrintaSeisEmEdicacao = false;

$(document).ready(function () {
    FormatarCampoDataPelaClasse("data");

    $('#modal-doze-trinta-seis').on('show.bs.modal', function (e) {
        AtualizarGridIntervaloModalDozeTrintaSeis();
    })
});

function AtualizarGridIntervaloModalDozeTrintaSeis() {
    let funcionarioId = $("#hdnFuncionario").val();

    post("AtualizarGridIntervaloDozeTrintaSeis", { funcionarioId })
        .done((response) => {
            $("#lista-intervalo-doze-trinta-seis").empty().append(response.Grid);
        });
}

function AdicionarIntervaloModalDozeTrintaSeis() {
    let funcionarioId = $("#hdnFuncionario").val();

    let dataInicial = $("#modal-doze-trintaseis-data-inicial").val();
    let dataFinal = $("#modal-doze-trintaseis-data-final").val();

    if (!dataInicial) {
        toastr.warning("Informe a data inicial", "Campo Obrigatório");
        return;
    }
    if (!dataFinal) {
        toastr.warning("Informe a data final", "Campo Obrigatório");
        return;
    }

    post("AdicionarIntervaloDozeTrintaSeis", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#lista-intervalo-doze-trinta-seis").empty().append(response.Grid);
            LimparCamposIntervaloModalDozeTrintaSeis();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloModalDozeTrintaSeisEmEdicacao = false;
}

function RemoverIntervaloModalDozeTrintaSeis(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    post("RemoverIntervaloDozeTrintaSeis", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#lista-intervalo-doze-trinta-seis").empty().append(response.Grid);
        });
}

function EditarIntervaloModalDozeTrintaSeis(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    if (intervaloModalDozeTrintaSeisEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarIntervaloDozeTrintaSeis", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#modal-doze-trintaseis-data-inicial").val(response.Item.DataInicialString);
            $("#modal-doze-trintaseis-data-final").val(response.Item.DataFinalString);
            $("#lista-intervalo-doze-trinta-seis").empty().append(response.Grid);
            intervaloModalDozeTrintaSeisEmEdicacao = true;
        });
}

function LimparCamposIntervaloModalDozeTrintaSeis() {
    $("#modal-doze-trintaseis-data-inicial").val("");
    $("#modal-doze-trintaseis-data-final").val("");
}