﻿$(document).ready(function () {
    FuncionarioAutoComplete();
});

function FuncionarioAutoComplete(campoId = "funcionarios", campoEscondidoId = "funcionario", campoEscondidoText = "funcionarioText") {
    let selecionou = false;
    $(`#${campoId}`).change(function () {
        if (!this.value)
            $(`#${campoEscondidoId}`).val("").change();
    });

    $(`#${campoId}`).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Funcionario/BuscarPeloNome',
                data: { nome: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $(`#${campoEscondidoId}`).val("").change();
                        $(`#${campoEscondidoText}`).val("");

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
                            return { label: item.Nome, value: item.Nome, id: item.Id };
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
            $(`#${campoEscondidoText}`).val(i.item.label);
        },
        close: function (e, i) {
            if (!selecionou) {
                $(`#${campoId}`).val("").change();
                $(`#${campoEscondidoId}`).val("").change();
                toastr.warning("Selecione uma opção da busca", "Alerta");
            }
            selecionou = false;
        },
        minLength: 3
    });
}