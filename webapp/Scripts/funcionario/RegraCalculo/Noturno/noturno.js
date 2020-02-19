function AbrirModalHistoricoNoturno() {
    let ehNoturno = $("#eh-noturno").isChecked();
    if (!ehNoturno) {
        toastr.warning("A escala precisa ser do tipo Noturno", "Alerta");
        return;
    }

    $("#modal-noturno").modal("show");
}

function AdicionarIntervaloNoturno() {
    let funcionarioId = $("#hdnFuncionario").val();
    let ehNoturno = $("#eh-noturno").isChecked();
    if (!ehNoturno) {
        toastr.warning("A escala precisa ser do tipo Noturno", "Alerta");
        return;
    }

    let dataInicial = $("#noturno-data-inicial").val();
    let dataFinal = $("#noturno-data-final").val();

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
            LimparCamposIntervaloNoturno();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloNoturnoEmEdicacao = false;
}

function LimparCamposIntervaloNoturno() {
    $("#noturno-data-inicial").val("");
    $("#noturno-data-final").val("");
}