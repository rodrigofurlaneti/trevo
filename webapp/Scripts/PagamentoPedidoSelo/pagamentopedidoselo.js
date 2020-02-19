var listahorarioparametro = [];

$(document).ready(function () {


    MakeChosen("tiposelo");
    MakeChosen("unidade");
    MakeChosen("convenio");
    MakeChosen("tipopagamento");


    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/pagamentopedidoselo/BuscarCliente',
                data: { descricao: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#cliente").val("");

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
                            return { label: item.Descricao, value: item.descricao, id: item.Id };
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
            $("#cliente").val(i.item.id);
        },
        minLength: 3
    });

    MetodoUtil();
});

function FiltrarPedidos() {
    let filtro = {};
    let unidadeId = $("#unidade").val();
    let clienteId = $("#cliente").val();
    let tiposeloId = $("#tiposelo").val();
    let convenioId = $("#convenio").val();
    let tipoPagamentoId = $("#tipopagamento").val();

    if (!unidadeId) {
        toastr.error('Selecione uma Unidade!', 'Informe a Unidade!');
        return false;
    }

    if (!cliente) {
        toastr.error('Selecione um Cliente!', 'Informe um Cliente!');
        return false;
    }

    if (unidadeId)
        filtro.Unidade = { Id: unidadeId };

    if (clienteId)
        filtro.Cliente = { Id: clienteId };

    if (tiposeloId)
        filtro.TipoSelo = { Id: tiposeloId };

    if (convenioId)
        filtro.Convenio = { Id: convenioId };

    if (tipoPagamentoId)
        filtro.TiposPagamento = tipoPagamentoId;

    $.ajax({
        url: "/PagamentoPedidoSelo/Filtrar",
        type: "POST",
        data: { filtro },
        success: function (result) {
            hideLoading();
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                hideLoading();

                $("#lista-pedido-result").empty();
                $("#lista-pedido-result").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        error: function (response) {
            hideLoading();
        },
        complete: function () {
            hideLoading();
            MetodoUtil();
        }
    });
}
