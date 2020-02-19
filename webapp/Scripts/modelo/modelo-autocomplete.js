
function ModeloAutoComplete(campoId = "cliente-cadastro-modelo", campoEscondidoId = "modelo-id", campoEscondidoText = "modelo-text", callBackFunction = null) {
    $(`#${campoId}`).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Modelo/BuscarModeloPeloNome',
                data: { nome: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $(`#${campoEscondidoId}`).val("");
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
                            return { label: item.Descricao, value: item.Descricao, id: item.Id };
                        }));
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                    $(`#${campoEscondidoId}`).val("");
                    $(`#${campoEscondidoText}`).val("");
                },
                failure: function (response) {
                    console.log(response.responseText);
                    $(`#${campoEscondidoId}`).val("");
                    $(`#${campoEscondidoText}`).val("");
                }
            });
        },
        select: function (e, i) {
            $(`#${campoEscondidoId}`).val(i.item.id);
            $(`#${campoEscondidoText}`).val(i.item.label);
        },
        minLength: 3
    }).change(function () {
        var autoTexto = $(`#${campoId}`).val();
        var texto = $(`#${campoEscondidoText}`).val();
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            //$(`#${campoId}`).val(""); //autocomplete
            $(`#${campoEscondidoId}`).val(""); //hiddenId
            $(`#${campoEscondidoText}`).val(""); //hiddenText
        }
        if (callBackFunction !== null)
            callBackFunction();
    });
}