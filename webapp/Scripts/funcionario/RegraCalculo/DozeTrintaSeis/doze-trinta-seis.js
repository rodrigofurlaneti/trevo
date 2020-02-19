function AbrirModalHistoricoDozeTrintaSeis() {
    let ehDozeTrintaSeis = $("#eh-doze-trintaseis").isChecked();
    if (!ehDozeTrintaSeis) {
        toastr.warning("A escala precisa ser do tipo 12/36", "Alerta");
        return;
    }

    $("#modal-doze-trinta-seis").modal("show");
}

function AdicionarIntervaloDozeTrintaSeis() {
    let funcionarioId = $("#hdnFuncionario").val();
    let ehDozeTrintaSeis = $("#eh-doze-trintaseis").isChecked();
    if (!ehDozeTrintaSeis) {
        toastr.warning("A escala precisa ser do tipo 12/36", "Alerta");
        return;
    }

    let dataInicial = $("#doze-trintaseis-data-inicial").val();
    let dataFinal = $("#doze-trintaseis-data-final").val();

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
            LimparCamposIntervaloDozeTrintaSeis();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloDozeTrintaSeisEmEdicacao = false;
}

function LimparCamposIntervaloDozeTrintaSeis() {
    $("#doze-trintaseis-data-inicial").val("");
    $("#doze-trintaseis-data-final").val("");
}