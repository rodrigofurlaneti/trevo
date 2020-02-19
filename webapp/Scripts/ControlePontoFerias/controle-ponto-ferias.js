enableMobileWidgets = true;

$(document).ready(function () {
    callbackPaginacao = BuscarFuncionarios;

    UnidadeAutoComplete("busca-unidades", "busca-unidade");
    UnidadeAutoComplete("unidades-apoio", "unidade-apoio");
    FuncionarioAutoComplete("busca-funcionarios", "busca-funcionario");
    SupervisorAutoComplete("busca-supervisores", "busca-supervisor");
    FormatarCampoHora(".hora");
    ConfigTabelaGridSemFiltroSemPaginacao("#datatable_dias");
    ConfigTabelaGridSemFiltroSemPaginacao("#datatable_unidadeapoio_combo");
    FormatarCampoDataPelaClasse("campo-data");

    let timeout = null;
    $("#lista-funcionario").on("input", ".hasinput input", function (event) {
        clearTimeout(timeout);

        timeout = setTimeout(function () {
            BuscarFuncionarios();
        }, 500);
    });

    $("#falta").change(function () {
        AlternarCheckBoxesPelaFalta(this.checked);
        BloquearCamposSeFaltou();
    });

    $("#atraso").change(function () {
        AlternarCheckBoxesPeloAtraso(this.checked);
    });
});

function AtualizarGridDiasGridControlePontoFeriasUnidadeApoio() {
    let ano = $("#ano").val();
    let mes = $("#mes").val();
    let funcionarioId = $("#campo-funcionario").val();

    return post("AtualizarGridDiasGridControlePontoFeriasUnidadeApoio", { funcionarioId, ano, mes })
        .done((response) => {
            AtualizarGrids(response);
            AtualizarDadosTotais();
        });
}

function BuscarFuncionarios(pagina = 1) {
    let busca = {
        UnidadeId: $("#busca-unidade").val(),
        SupervisorId: $("#busca-supervisor").val(),
        FuncionarioId: $("#busca-funcionario").val(),
        ColunaUnidade: $("#coluna-busca-unidade").val(),
        ColunaSupervisor: $("#coluna-busca-supervisor").val(),
        ColunaFuncionario: $("#coluna-busca-funcionario").val(),
    }

    post("BuscarFuncionarios", { pagina, busca })
        .done((response) => {
            $(".paginacao").empty().append(response.PartialPaginacao);
            $("#lista-funcionario tbody").empty().append(response.Grid);
        });
}

function EditarControlePontoFeriasDia(data) {
    return post("EditarControlePontoFeriasDia", { data })
        .done((response) => {
            $("#dia").val(response.ControleDia.Dia);
            $("#observacao").val(response.ControleDia.Observacao);
            $("#entrada").val(response.ControleDia.HorarioEntrada);
            $("#saida-almoco").val(response.ControleDia.HorarioSaidaAlmoco);
            $("#retorno-almoco").val(response.ControleDia.HorarioRetornoAlmoco);
            $("#saida").val(response.ControleDia.HorarioSaida);
            $("#falta").prop("checked", response.ControleDia.Falta);
            $("#falta-justificada").prop("checked", response.ControleDia.FaltaJustificada);
            $("#atraso").prop("checked", response.ControleDia.Atraso);
            $("#atraso-justificado").prop("checked", response.ControleDia.AtrasoJustificado);
            $("#suspensao").prop("checked", response.ControleDia.Suspensao);
            $("#atestado").prop("checked", response.ControleDia.Atestado);

            AlternarCheckBoxesPelaFalta(response.ControleDia.Falta);
            AlternarCheckBoxesPeloAtraso(response.ControleDia.Atraso);

            BloquearCamposSeFaltou();

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoFeriasUnidadeApoioPrincipal);
            AtualizarGridDiasGridControlePontoFeriasUnidadeApoio();
        });
}

function SalvarControlePontoFeriasDia() {
    let funcionarioId = $("#campo-funcionario").val();

    let dto = {
        Data: $("#dia").val(),
        Observacao: $("#observacao").val(),
        HorarioEntrada: $("#entrada").val(),
        HorarioSaidaAlmoco: $("#saida-almoco").val(),
        HorarioRetornoAlmoco: $("#retorno-almoco").val(),
        HorarioSaida: $("#saida").val(),
        Falta: $("#falta").is(":checked"),
        FaltaJustificada: $("#falta-justificada").is(":checked"),
        Atraso: $("#atraso").is(":checked"),
        AtrasoJustificado: $("#atraso-justificado").is(":checked"),
        Suspensao: $("#suspensao").is(":checked"),
        Atestado: $("#atestado").is(":checked")
    };

    if (!dto.Data) {
        toastr.warning("Selecione um dia antes de tentar salvar", "Alerta");
        return false;
    }

    return post("SalvarControlePontoFeriasDia", { funcionarioId, dto })
        .done((response) => {
            LimparCamposControlePontoFeriasDia();

            AtualizarGridDiasGridControlePontoFeriasUnidadeApoio()
                .done(() => AtualizarDadosTotais);
        });
}

function EditarControlePontoFeriasUnidadeApoio(unidadeId) {
    let funcionarioId = $("#campo-funcionario").val();
    let data = $("#dia").val();

    return post("EditarControlePontoFeriasUnidadeApoio", { funcionarioId, data, unidadeId })
        .done((response) => {
            $("#unidades-apoio").val(response.ControlePontoFeriasUnidadeApoio.Unidade.Nome);
            $("#unidade-apoio").val(response.ControlePontoFeriasUnidadeApoio.Unidade.Id);
            $("#entrada-apoio").val(response.ControlePontoFeriasUnidadeApoio.HorarioEntrada);
            $("#saida-apoio").val(response.ControlePontoFeriasUnidadeApoio.HorarioSaida);

            if (response.ControlePontoFeriasUnidadeApoio.TipoHoraExtra == 65)
                $("#unidade-apoio-he-sessenta-cinto").prop("checked", true);
            else
                $("#unidade-apoio-he-cem").prop("checked", true);

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoFeriasUnidadeApoioPrincipal);

            AtualizarDadosTotais();
        });
}

function SalvarControlePontoFeriasUnidadeApoio() {
    let unidadeId = $("#unidade-apoio").val();
    let funcionarioId = $("#campo-funcionario").val();

    if (!unidadeId) {
        toastr.warning("Informe a Unidade", "Campo Obrigatório");
        return false;
    }

    let dto = {
        Unidade: {
            Id: unidadeId
        },
        HorarioEntrada: $("#entrada-apoio").val(),
        HorarioSaida: $("#saida-apoio").val(),
        TipoHoraExtra: $("input[name=HE]:checked").val(),
        Data: $("#dia").val(),
    };

    return post("SalvarControlePontoFeriasUnidadeApoio", { funcionarioId, dto })
        .done((response) => {
            LimparCamposControlePontoFeriasUnidadeApoio();

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoFeriasUnidadeApoioPrincipal);

            AtualizarDadosTotais();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });
}

function LimparCamposControlePontoDia() {
    $("#dia").val("");
    $("#observacao").val("");
    $("#entrada").val("");
    $("#saida-almoco").val("");
    $("#retorno-almoco").val("");
    $("#saida").val("");
    $("#falta").prop("checked", false);
    $("#falta-justificada").prop("checked", false);
    $("#atraso").prop("checked", false);
    $("#atraso-justificado").prop("checked", false);
    $("#suspensao").prop("checked", false);
    $("#atestado").prop("checked", false);
    $("#lista-unidade-apoio-principal tbody").empty();
    HabilitarCampos();
}

function BloquearCamposSeFaltou() {
    let faltou = $("#falta").isChecked();

    if (faltou) {
        $("#entrada").disabled().val("");
        $("#saida-almoco").disabled().val("");
        $("#retorno-almoco").disabled().val("");
        $("#saida").disabled().val("");
        $("#atraso").disabled().prop("checked", false);
        $("#atraso-justificado").disabled().prop("checked", false);
        $("#suspensao").disabled().prop("checked", false);
        $("#unidades-apoio").disabled().val("");
        $("#unidade-apoio").disabled().val("");
        $("#entrada-apoio").disabled().val("");
        $("#saida-apoio").disabled().val("");
    } else {
        HabilitarCampos();
    }
}

function HabilitarCampos() {
    $("#entrada").unDisabled();
    $("#saida-almoco").unDisabled();
    $("#retorno-almoco").unDisabled();
    $("#saida").unDisabled();
    $("#atraso").unDisabled();
    $("#atraso-justificado").unDisabled();
    $("#suspensao").unDisabled();
    $("#unidades-apoio").unDisabled();
    $("#unidade-apoio").unDisabled();
    $("#entrada-apoio").unDisabled();
    $("#saida-apoio").unDisabled();
}

function LimparCamposControlePontoFeriasUnidadeApoio() {
    $("#unidades-apoio").val("");
    $("#unidade-apoio").val("");
    $("#entrada-apoio").val("");
    $("#saida-apoio").val("");
    $("#unidade-apoio-he-sessenta-cinto").prop("checked", true);
}

function AtualizarGrids(response) {
    if (response.GridDias) {
        $("#lista-dias").empty().append(response.GridDias);
        ConfigTabelaGridSemFiltroSemPaginacao("#datatable_dias");
    }
    if (response.GridControlePontoFeriasUnidadeApoioCombo) {
        $("#lista-unidade-apoio-combo").empty().append(response.GridControlePontoFeriasUnidadeApoioCombo);
        ConfigTabelaGridSemFiltroSemPaginacao("#datatable_unidadeapoio_combo");
    }
}

function AlternarCheckBoxesPelaFalta(checked) {
    if (checked) {
        $("#falta-justificada").unDisabled();
        $("#atestado").unDisabled();
    }
    else {
        $("#falta-justificada").unChecked().disabled();

        if (!$("#atraso").isChecked()) {
            $("#atestado").unChecked().disabled();
        }
    }
};

function AlternarCheckBoxesPeloAtraso(checked) {
    if (checked) {
        $("#atraso-justificado").unDisabled();
        $("#atestado").unDisabled();
    }
    else {
        $("#atraso-justificado").unChecked().disabled();

        if (!$("#falta").isChecked()) {
            $("#atestado").unChecked().disabled();
        }
    }
};
