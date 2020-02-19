let intervaloModalCompensacaoEmEdicacao = false;

$(document).ready(function () {
    FormatarCampoDataPelaClasse("data");

    $('#modal-compensacao').on('show.bs.modal', function (e) {
        AtualizarGridIntervaloModalCompensacao();
    })
});

function AtualizarGridIntervaloModalCompensacao() {
    let funcionarioId = $("#hdnFuncionario").val();

    post("AtualizarGridIntervaloCompensacao", { funcionarioId })
        .done((response) => {
            $("#lista-intervalo-compensacao").empty().append(response.Grid);
        });
}

function AdicionarIntervaloModalCompensacao() {
    let funcionarioId = $("#hdnFuncionario").val();

    let dataInicial = $("#modal-compensacao-data-inicial").val();
    let dataFinal = $("#modal-compensacao-data-final").val();

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
            $("#lista-intervalo-compensacao").empty().append(response.Grid);
            LimparCamposIntervaloModalCompensacao();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    intervaloModalCompensacaoEmEdicacao = false;
}

function RemoverIntervaloModalCompensacao(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    post("RemoverIntervaloCompensacao", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#lista-intervalo-compensacao").empty().append(response.Grid);
        });
}

function EditarIntervaloModalCompensacao(dataInicial, dataFinal) {
    let funcionarioId = $("#hdnFuncionario").val();

    if (intervaloModalCompensacaoEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarIntervaloCompensacao", { funcionarioId, dataInicial, dataFinal })
        .done((response) => {
            $("#modal-compensacao-data-inicial").val(response.Item.DataInicialString);
            $("#modal-compensacao-data-final").val(response.Item.DataFinalString);
            $("#lista-intervalo-compensacao").empty().append(response.Grid);
            intervaloModalCompensacaoEmEdicacao = true;
        });
}

function LimparCamposIntervaloModalCompensacao() {
    $("#modal-compensacao-data-inicial").val("");
    $("#modal-compensacao-data-final").val("");
}