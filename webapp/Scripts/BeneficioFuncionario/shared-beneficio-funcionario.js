let beneficioFuncionarioEmEdicacao = false;

$(document).ready(function () {
    MakeChosen("beneficio-funcionario-tipo-beneficio");
    FormatarReal("valmoney");

    $("#beneficio-funcionario-tipo-beneficio").change(function () {
        DesabilitarValorSeForPlanoCarreira();
    });

    if ($("#guidSession").val() === undefined || $("#guidSession").val() === null || $("#guidSession").val() === "")
    $("#guidSession").val(generateUUID());
});

function DesabilitarValorSeForPlanoCarreira() {
    let planoCarreiraSelecionado = $("#beneficio-funcionario-tipo-beneficio option:selected").text() === "Plano de Carreira";
    if (planoCarreiraSelecionado) {
        $("#beneficio-funcionario-valor").val("0,00");
        $("#beneficio-funcionario-valor").readonly();
    } else {
        $("#beneficio-funcionario-valor").unReadonly();
    }
}

function AdicionarBeneficioFuncionario() {
    let funcionarioId = $("#beneficio-funcionario-funcionario").val();

    if (location.pathname.toLowerCase().includes("beneficiofuncionario") && !$("#beneficio-funcionario-funcionario").val()) {
        toastr.warning("Informe o Funcionário antes de adicionar um registro", "Alerta");
        return false;
    }

    let planoCarreiraSelecionado = $("#beneficio-funcionario-tipo-beneficio option:selected").text() === "Plano de Carreira";
    let tipoBeneficioId = $("#beneficio-funcionario-tipo-beneficio").val();
    let valor = $("#beneficio-funcionario-valor").val();

    if (!tipoBeneficioId) {
        toastr.warning("Informe o Tipo de Benefício", "Campo Obrigatório");
        return;
    }
    if (!planoCarreiraSelecionado && (!valor || valor === '0,00')) {
        toastr.warning("Informe o valor", "Campo Obrigatório");
        return;
    }

    var guidSession = $("#guidSession").val();

    post("AdicionarBeneficioFuncionario", { funcionarioId, tipoBeneficioId, valor, guidSession }, "BeneficioFuncionario")
        .done((response) => {
            $("#lista-beneficio-funcionario-detalhe").empty().append(response.Grid);
            LimparCamposBeneficioFuncionario();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    beneficioFuncionarioEmEdicacao = false;
}

function RemoverBeneficioFuncionario(tipoBeneficioId) {
    var guidSession = $("#guidSession").val();

    post("RemoverBeneficioFuncionario", { tipoBeneficioId, guidSession }, "BeneficioFuncionario")
        .done((response) => {
            $("#lista-beneficio-funcionario-detalhe").empty().append(response.Grid);
        });
}

function EditarBeneficioFuncionario(tipoBeneficioId) {
    if (beneficioFuncionarioEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    var guidSession = $("#guidSession").val();

    post("EditarBeneficioFuncionario", { tipoBeneficioId, guidSession }, "BeneficioFuncionario")
        .done((response) => {
            $("#lista-beneficio-funcionario-detalhe").empty().append(response.Grid);
            $("#beneficio-funcionario-tipo-beneficio").val(response.Item.TipoBeneficio.Id);
            $("#beneficio-funcionario-valor").val(response.Item.Valor);
            MakeChosen("beneficio-funcionario-tipo-beneficio");
            beneficioFuncionarioEmEdicacao = true;
            DesabilitarValorSeForPlanoCarreira();
        });
}

function LimparCamposBeneficioFuncionario() {
    $("#beneficio-funcionario-tipo-beneficio").val("");
    $("#beneficio-funcionario-valor").val("0,00");
    MakeChosen("beneficio-funcionario-tipo-beneficio");
}