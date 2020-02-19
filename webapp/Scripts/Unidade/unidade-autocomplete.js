$(document).ready(function () {
    UnidadeAutoComplete();
});

function UnidadeAutoComplete(campoId = "unidades", campoEscondidoId = "unidade") {
    $(`#${campoId}`).change(function () {
        if (!this.value)
            $(`#${campoEscondidoId}`).val("").change();
    });

    $(`#${campoId}`).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Unidade/BuscarPeloNome',
                data: { nome: request.term },
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
            $(`#${campoEscondidoId}`).val(i.item.id).change();
        },
        minLength: 2
    });
}