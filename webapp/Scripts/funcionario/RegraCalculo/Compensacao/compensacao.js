function AbrirModalHistoricoCompensacao() {
    let ehCompensacao = $("#eh-compensacao").isChecked();
    if (!ehCompensacao) {
        toastr.warning("A escala precisa ser do tipo Compensação", "Alerta");
        return;
    }

    $("#modal-compensacao").modal("show");
}

function AdicionarIntervaloCompensacao() {
    let funcionarioId = $("#hdnFuncionario").val();
    let ehCompensacao = $("#eh-compensacao").isChecked();
    if (!ehCompensacao) {
        toastr.warning("A escala precisa ser do tipo Compensação", "Alerta");
        return;
    }

    let dataInicial = $("#compensacao-data-inicial").val();
    let dataFinal = $("#compensacao-data-final").val();

    if (!dataInicial) {
        toastr.warning("Informe a data inicial", "Campo Obrigatório");
        return;
    }
    if (!dataFinal) {
        toastr.warning("Informe a data final", "Campo Obrigatório");
        return;
    }

    post("AdicionarIntervaloCompensacao", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            LimparCamposIntervaloCompensacao();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloCompensacaoEmEdicacao = false;
}

function LimparCamposIntervaloCompensacao() {
    $("#compensacao-data-inicial").val("");
    $("#compensacao-data-final").val("");
}