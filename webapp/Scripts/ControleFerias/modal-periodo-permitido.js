let periodoPermitidoEmEdicacao = false;

$(document).ready(function () {
    FormatarCampoDataPelaClasse("data");

    $('#modal-periodo-permitido').on('show.bs.modal', function (e) {
        AtualizarGridPeriodoPermitido();
    })
});

function AtualizarGridPeriodoPermitido() {
    post("AtualizarGridPeriodoPermitido", { controleFeriasId: controleFeriasIdModal })
        .done((response) => {
            $("#lista-periodo-permitido").empty().append(response.Grid);
        });
}

function AdicionarPeriodoPermitido() {
    let dataDe = $("#modal-periodo-permitido-data-inicial").val();
    let dataAte = $("#modal-periodo-permitido-data-final").val();

    if (!dataDe) {
        toastr.warning("Informe a data inicial", "Campo Obrigatório");
        return;
    }
    if (!dataAte) {
        toastr.warning("Informe a data final", "Campo Obrigatório");
        return;
    }

    post("AdicionarPeriodoPermitido", { controleFeriasId: controleFeriasIdModal, dataDe, dataAte })
        .done((response) => {
            $("#lista-periodo-permitido").empty().append(response.Grid);
            LimparCamposPeriodoPermitido();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    periodoPermitidoEmEdicacao = false;
}

function RemoverPeriodoPermitido(dataDe, dataAte) {
    post("RemoverPeriodoPermitido", { controleFeriasId: controleFeriasIdModal, dataDe, dataAte })
        .done((response) => {
            $("#lista-periodo-permitido").empty().append(response.Grid);
        });
}

function EditarPeriodoPermitido(dataDe, dataAte) {
    if (periodoPermitidoEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarPeriodoPermitido", { controleFeriasId: controleFeriasIdModal, dataDe, dataAte })
        .done((response) => {
            $("#modal-periodo-permitido-data-inicial").val(response.Item.DataDeString);
            $("#modal-periodo-permitido-data-final").val(response.Item.DataAteString);
            $("#lista-periodo-permitido").empty().append(response.Grid);
            periodoPermitidoEmEdicacao = true;
        });
}

function LimparCamposPeriodoPermitido() {
    $("#modal-periodo-permitido-data-inicial").val("");
    $("#modal-periodo-permitido-data-final").val("");
}