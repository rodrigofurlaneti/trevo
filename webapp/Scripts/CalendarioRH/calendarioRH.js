let calendarioRHUnidadeEmEdicao = false;

$(document).ready(function () {
    FormatarCampoDataPelaClasse("campo-data");
    BuscarCalendarioRH();

    $("#todas-unidades").change(function () {
        if (this.checked)
            RemoverTodasCalendarioRHUnidade();
    });

    $("#calendarioRH-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
        if (!$(".item-calendarioRH-unidade").length && !$("#todas-unidades").isChecked()) {
            toastr.warning("Adicione pelo menos uma unidade ou marque o check de Todas Unidades", "Alerta");
            return false;
        }
    });
});

function RemoverTodasCalendarioRHUnidade() {
    return post("RemoverTodasCalendarioRHUnidade").done((response) => $("#lista-calendarioRH-unidade").empty().append(response.Grid));
}

function BuscarCalendarioRH() {
    return get("BuscarCalendarioRH", "CalendarioRH")
        .done((response) => {
            $("#lista-calendarioRH").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function AdicionarCalendarioRHUnidade() {
    let unidadeId = $("#unidade").val();

    if ($("#todas-unidades").isChecked()) {
        toastr.warning("Não é possível adicionar pois o campo Todas as Unidades já está marcado", "Alerta");
        return;
    }

    if (!unidadeId) {
        toastr.warning("Informe a Unidade", "Campo Obrigatório");
        return;
    }

    post("AdicionarCalendarioRHUnidade", { unidadeId }, "CalendarioRH")
        .done((response) => {
            $("#lista-calendarioRH-unidade").empty().append(response.Grid);
            LimparCamposCalendarioRH();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    calendarioRHUnidadeEmEdicao = false;
}

function RemoverCalendarioRHUnidade(unidadeId) {
    post("RemoverCalendarioRHUnidade", { unidadeId }, "CalendarioRH")
        .done((response) => {
            $("#lista-calendarioRH-unidade").empty().append(response.Grid);
        });
}

function EditarCalendarioRHUnidade(unidadeId) {
    if (calendarioRHUnidadeEmEdicao) {
        toastr.warning("Já possui uma unidade em edição", "Alerta");
        return;
    }

    post("EditarCalendarioRHUnidade", { unidadeId }, "CalendarioRH")
        .done((response) => {
            $("#lista-calendarioRH-unidade").empty().append(response.Grid);
            $("#unidades").val(response.CalendarioRHUnidade.Unidade.Nome);
            $("#unidade").val(response.CalendarioRHUnidade.Unidade.Id);
            calendarioRHUnidadeEmEdicao = true;
        });
}

function LimparCamposCalendarioRH() {
    $("#unidades").val("");
    $("#unidade").val("");
}