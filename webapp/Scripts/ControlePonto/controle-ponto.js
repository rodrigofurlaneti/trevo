$(document).ready(function () {
    MakeChosenPelaClasse("ano");
    MakeChosenPelaClasse("mes");

    SupervisorAutoComplete("campo-supervisores", "campo-supervisor");
    FuncionarioAutoComplete("campo-funcionarios", "campo-funcionario");
    UnidadeAutoComplete("unidades-apoio", "unidade-apoio");
    ControlePontoDiaAutoComplete("dias", "dia", "ControlePontoSupervisor");

    FormatarCampoHora(".hora");
    ConfigTabelaGridSemFiltroSemPaginacao("#datatable_dias");
    ConfigTabelaGridSemFiltroSemPaginacao("#datatable_unidadeapoio_combo");

    $("#campo-supervisor").change(function () {
        BuscarFuncionariosDoSupervisor(this.value);
    });

    $("#folga").change(function () {
        BloquearCamposSeFolgou();
    });

    $("#falta").change(function () {
        AlternarCheckBoxesPelaFalta(this.checked);
        BloquearCamposSeFaltou();
    });

    $("#suspensao").change(function () {
        BloquearCamposSeSuspenso();
    });

    $("#atraso").change(function () {
        AlternarCheckBoxesPeloAtraso(this.checked);
    });

    $("#dia").change(function () {
        EditarControlePontoDia(this.value);
    });
});

function HabilitarCampos() {
    let faltou = $("#falta").isChecked();
    let folgou = $("#folga").isChecked();
    let suspenso = $("#suspensao").isChecked();

    if (!faltou && !folgou && !suspenso) {
        $("#folga").unDisabled();
        $("#falta").unDisabled();
        $("#entrada").unDisabled();
        $("#saida-almoco").unDisabled();
        $("#retorno-almoco").unDisabled();
        $("#saida").unDisabled();
        $("#atraso").unDisabled();
        $("#suspensao").unDisabled();
        $("#unidades-apoio").unDisabled();
        $("#unidade-apoio").unDisabled();
        $("#entrada-apoio").unDisabled();
        $("#saida-apoio").unDisabled();
    }
}

function LimparCamposControlePontoDia() {
    $("#data-admissao").val("");
    $("#dias").val("");
    $("#dia").val("");
    $("#observacao").val("");
    $("#entrada").val("");
    $("#saida-almoco").val("");
    $("#retorno-almoco").val("");
    $("#saida").val("");
    $("#folga").prop("checked", false);
    $("#falta").prop("checked", false);
    $("#falta-justificada").disabled().prop("checked", false);
    $("#atraso").prop("checked", false);
    $("#atraso-justificado").disabled().prop("checked", false);
    $("#suspensao").prop("checked", false);
    $("#atestado").disabled().prop("checked", false);
    $("#lista-unidade-apoio-principal tbody").empty();
    HabilitarCampos();
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

function BloquearCamposSeFaltou() {
    let faltou = $("#falta").isChecked();

    if (faltou) {
        $("#entrada").disabled().val("");
        $("#saida-almoco").disabled().val("");
        $("#retorno-almoco").disabled().val("");
        $("#saida").disabled().val("");
        $("#folga").disabled().prop("checked", false);
        $("#atraso").disabled().prop("checked", false);
        $("#atraso-justificado").disabled().prop("checked", false);
        $("#suspensao").disabled().prop("checked", false);
        $("#unidades-apoio").disabled().val("");
        $("#unidade-apoio").disabled().val("");
        $("#entrada-apoio").disabled().val("");
        $("#saida-apoio").disabled().val("");
    }

    HabilitarCampos();
}

function BloquearCamposSeFolgou() {
    let folgou = $("#folga").isChecked();

    if (folgou) {
        $("#entrada").disabled().val("");
        $("#saida-almoco").disabled().val("");
        $("#retorno-almoco").disabled().val("");
        $("#saida").disabled().val("");
        $("#falta").disabled().unChecked();
        $("#atraso").disabled().unChecked();
        $("#atraso-justificado").disabled().unChecked();
        $("#suspensao").disabled().unChecked();
    }

    HabilitarCampos();
}

function BloquearCamposSeSuspenso() {
    let suspenso = $("#suspensao").isChecked();

    if (suspenso) {
        $("#entrada").disabled().val("");
        $("#saida-almoco").disabled().val("");
        $("#retorno-almoco").disabled().val("");
        $("#saida").disabled().val("");
        $("#folga").disabled().unChecked();
        $("#atraso").disabled().unChecked();
        $("#atraso-justificado").disabled().unChecked();
        $("#falta").disabled().unChecked();
        $("#falta-justificada").disabled().unChecked();
        $("#atestado").disabled().unChecked();
        $("#unidades-apoio").disabled().val("");
        $("#unidade-apoio").disabled().val("");
        $("#entrada-apoio").disabled().val("");
        $("#saida-apoio").disabled().val("");
    }

    HabilitarCampos();
}

function ControlePontoDiaAutoComplete(campoId, campoEscondidoId, controller) {
    let selecionou = false;
    $(`#${campoId}`).change(function () {
        if (!this.value)
            $(`#${campoEscondidoId}`).val("").change();
    });

    let url = `/${controller}/BuscarDias`;
    $(`#${campoId}`).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                data: { dia: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $(`#${campoEscondidoId}`).val("").change();

                        var result = [
                            {
                                label: 'Nenhum resultado encontrado',
                                value: response.term
                            }
                        ];
                        response(result);
                    }
                    else {
                        response($.map(data, function (item) {
                            return { label: item.Dia, value: item.Dia, id: item.Dia };
                        }))
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                },
                failure: function (response) {
                    console.log(response.responseText);
                }
            });
        },
        select: function (e, i) {
            selecionou = true;
            $(`#${campoEscondidoId}`).val(i.item.id).change();
        },
        close: function (e, i) {
            if (!selecionou) {
                $(`#${campoId}`).val("").change();
                $(`#${campoEscondidoId}`).val("").change();
                toastr.warning("Selecione uma opção da busca", "Alerta");
            }
            selecionou = false;
        },
        minLength: 1
    });
}

function BuscarDataAdmissao(funcionarioId) {
    return post("BuscarDataAdmissao", { funcionarioId }, "Funcionario")
        .done((response) => {
            $("#data-admissao").val(response);
        });
}

function BuscarFuncionariosDoSupervisor(supervisorId) {
    return post("BuscarFuncionariosDoSupervisor", { supervisorId })
        .done((response) => {
            $("#select-funcionarios").empty().append(response);

            if (supervisorId) {
                MakeChosen("campo-funcionario");
            } else {
                FuncionarioAutoComplete("campo-funcionarios", "campo-funcionario");
            }
        });
}

function SalvarControlePontoDia() {
    let funcionarioId = $("#campo-funcionario").val();

    let dto = {
        Data: $("#dia").val(),
        Observacao: $("#observacao").val(),
        HorarioEntrada: $("#entrada").val(),
        HorarioSaidaAlmoco: $("#saida-almoco").val(),
        HorarioRetornoAlmoco: $("#retorno-almoco").val(),
        HorarioSaida: $("#saida").val(),
        Folga: $("#folga").is(":checked"),
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

    return post("SalvarControlePontoDia", { funcionarioId, dto })
        .done(() => {
            LimparCamposControlePontoDia();

            AtualizarGridDiasGridControlePontoUnidadeApoio()
                .done(() => {
                    if (location.pathname.toLowerCase().includes("controlepontorh"))
                        AtualizarDadosTotais()
                });
        });
}

function EditarControlePontoDia(data) {
    LimparCamposControlePontoDia();

    return post("EditarControlePontoDia", { data })
        .done((response) => {
            $("#dias").val(response.ControleDia.Dia);
            $("#dia").val(response.ControleDia.Dia);
            $("#observacao").val(response.ControleDia.Observacao);
            $("#entrada").val(response.ControleDia.HorarioEntrada);
            $("#saida-almoco").val(response.ControleDia.HorarioSaidaAlmoco);
            $("#retorno-almoco").val(response.ControleDia.HorarioRetornoAlmoco);
            $("#saida").val(response.ControleDia.HorarioSaida);
            $("#folga").prop("checked", response.ControleDia.Folga);
            $("#falta").prop("checked", response.ControleDia.Falta);
            $("#falta-justificada").prop("checked", response.ControleDia.FaltaJustificada);
            $("#atraso").prop("checked", response.ControleDia.Atraso);
            $("#atraso-justificado").prop("checked", response.ControleDia.AtrasoJustificado);
            $("#suspensao").prop("checked", response.ControleDia.Suspensao);
            $("#atestado").prop("checked", response.ControleDia.Atestado);

            AlternarCheckBoxesPelaFalta(response.ControleDia.Falta);
            AlternarCheckBoxesPeloAtraso(response.ControleDia.Atraso);

            BloquearCamposSeFolgou();
            BloquearCamposSeFaltou();
            BloquearCamposSeSuspenso();

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoUnidadeApoioPrincipal);
            AtualizarGridDiasGridControlePontoUnidadeApoio();
        });
}

function AtualizarGrids(response) {
    if (response.GridDias) {
        $("#lista-dias").empty().append(response.GridDias);
        ConfigTabelaGridSemFiltroSemPaginacao("#datatable_dias");
    }
    if (response.GridControlePontoUnidadeApoioCombo) {
        $("#lista-unidade-apoio-combo").empty().append(response.GridControlePontoUnidadeApoioCombo);
        ConfigTabelaGridSemFiltroSemPaginacao("#datatable_unidadeapoio_combo");
    }
}

function AtualizarGridDiasGridControlePontoUnidadeApoio() {
    let ano = $("#ano").val();
    let mes = $("#mes").val();
    let funcionarioId = $("#campo-funcionario").val();

    return post("AtualizarGridDiasGridControlePontoUnidadeApoio", { funcionarioId, ano, mes })
        .done((response) => {
            AtualizarGrids(response);
        });
}

function LimparCamposControlePontoUnidadeApoio() {
    $("#unidades-apoio").val("");
    $("#unidade-apoio").val("");
    $("#entrada-apoio").val("");
    $("#saida-apoio").val("");
    $("#unidade-apoio-he-sessenta-cinto").prop("checked", true);
}

function SalvarControlePontoUnidadeApoio() {
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

    return post("SalvarControlePontoUnidadeApoio", { funcionarioId, dto })
        .done((response) => {
            LimparCamposControlePontoUnidadeApoio();

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoUnidadeApoioPrincipal);
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });
}

function EditarControlePontoUnidadeApoio(unidadeId) {
    let funcionarioId = $("#campo-funcionario").val();
    let data = $("#dia").val();

    return post("EditarControlePontoUnidadeApoio", { funcionarioId, data, unidadeId })
        .done((response) => {
            $("#unidades-apoio").val(response.ControlePontoUnidadeApoio.Unidade.Nome);
            $("#unidade-apoio").val(response.ControlePontoUnidadeApoio.Unidade.Id);
            $("#entrada-apoio").val(response.ControlePontoUnidadeApoio.HorarioEntrada);
            $("#saida-apoio").val(response.ControlePontoUnidadeApoio.HorarioSaida);

            if (response.ControlePontoUnidadeApoio.TipoHoraExtra == 65)
                $("#unidade-apoio-he-sessenta-cinto").prop("checked", true);
            else
                $("#unidade-apoio-he-cem").prop("checked", true);

            $("#lista-unidade-apoio-principal").empty().append(response.GridControlePontoUnidadeApoioPrincipal);
        });
}

function ImprimirLista() {
    let supervisorId = $("#campo-supervisor").val();

    if (!supervisorId) {
        toastr.warning("Informe o supervisor", "Alerta");
        return;
    }

    let a = document.createElement("a");
    a.target = "_blank";
    a.href = `/ControlePonto/Impressao?supervisorId=${supervisorId}`;
    a.click();
}