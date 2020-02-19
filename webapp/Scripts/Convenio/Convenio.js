$(document).ready(function () {
    $(document).on('click', '#data_table_address tr.line-blacklist', function (e) {
        //console.log('click', '#data_table_address tr.line-blacklist', this, e);
        if ($(this).is('tr')) {
            $('#data_table_address tr.line-blacklist.selected').removeClass('selected');
            $(this).addClass('selected');
            var unidadeId = $(this).find('#hdnUnidadeId').val();
            $('#fieldset_clientes').data({ unidadeId: unidadeId });
        }
    });

    function getUnidadeId() {
        return ($('#fieldset_clientes').data() || {}).unidadeId;
    }

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Convenio/BuscarCliente',
                data: { descricao: request.term, unidadeId: getUnidadeId() },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#clienteText").val("");
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
                            return { label: item.Descricao, value: item.Descricao, id: item.Id };
                        }));
                    }
                },
                error: function (response) {
                    $("#cliente").val(""); //hiddenId
                    $("#clienteText").val(""); //hiddenText
                },
                failure: function (response) {
                    $("#cliente").val(""); //hiddenId
                    $("#clienteText").val(""); //hiddenText
                }
            });
        },
        select: function (e, i) {
            $("#clienteText").val(i.item.label);
            $("#cliente").val(i.item.id);
        },
        minLength: 3
    }).change(function () {
        var autoTexto = $("#clientes").val();
        var texto = $("#clienteText").val();
        if (autoTexto !== texto || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {
            $("#clientes").val(""); //autocomplete
            $("#cliente").val(""); //hiddenId
            $("#clienteText").val(""); //hiddenText
        }
    });


    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#Status").prop("checked", true);

    $("#ConvenioForm").submit(function (e) {
        if (ValidarCamposPrincipal()) {
            return true;
        }


        return false;
    });

    $("#TipoSeloId").change(function () {
        var idTipoSelo = $("#TipoSeloId").val();

        $.ajax({
            url: '/Convenio/VerificarTipoSelo',
            type: 'POST',
            dataType: 'json',
            data: { idTipoSelo: idTipoSelo },
            success: function (response) {
                if (response.ParametroSelo === "Personalizado") {
                    $("#convenioUnidadeValor").val("0,00");
                    $("#convenioUnidadeValor").prop("disabled", false);
                }
                else if (response.ParametroSelo === "Monetario" || response.ParametroSelo === "Percentual") {
                    if (!response.TemPrecoParametroSelo) {
                        openCustomModal(null, null, "warning", "Alerta", "É preciso parametrizar o valor do selo.", null, null, null);
                        $("#convenioUnidadeValor").val("");
                        return;
                    }

                    $("#convenioUnidadeValor").val(response.Valor.toString().replace(".", ","));
                    $("#convenioUnidadeValor").prop("disabled", true);
                }
                else {
                    $("#convenioUnidadeValor").val("0,00");
                    $("#convenioUnidadeValor").prop("disabled", true);
                }
            },

            complete: function () {
                hideLoading();
            }
        });

    });
    buscarConvenios();

    formartarDropDowLists();
});

function formartarDropDowLists() {

    MakeChosen("UnidadeID");
    MakeChosen("TipoSeloId");
}

function ObterConvenioClientes() {
    $('#lista-unidadeCliente-result').empty();

    showLoading();
    $.post("/Convenio/ObterClientes", { })
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {

                }
                else {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }
            } else {
                $("#lista-unidadeCliente-result").empty();
                $("#lista-unidadeCliente-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => {
            $('#unidade_clientes_row').show();
            hideLoading();
        });
}

function ValidaCliente() {
    if ($("#cliente").val() === "") {
        toastr.error("O campo \"Cliente\" é obrigatório");
        return false;
    }

    return true;
}

function AdicionarConvenioCliente() {
    if (!ValidaCliente()) {
        return true;
    }
    
    var idCliente = $("#cliente").val();

    showLoading();
    $.post("/Convenio/AdicionarCliente", { idCliente: idCliente })
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {
                    
                }
                else {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }
            } else {
                $("#clientes").val(""); //autocomplete
                $("#cliente").val(""); //hiddenId
                $("#clienteText").val(""); //hiddenText

                $("#lista-convenioCliente-result").empty();
                $("#lista-convenioCliente-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => {
            hideLoading();
        });
}

function RemoverConvenioCliente(idCliente) {
    showLoading();
    $.post("/Convenio/RemoverCliente", { idCliente: idCliente })
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {

                }
                else {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }
            } else {
                $("#lista-convenioCliente-result").empty();
                $("#lista-convenioCliente-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => {
            hideLoading();
        });
}

function CarregarConvenioUnidades() {
    showLoading();
    $.post("/Convenio/ObterConvenioUnidades", { })
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {
                    
                }
                else {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }

            } else {
                $("#lista-convenioUnidade-result").empty();
                $("#lista-convenioUnidade-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => {
            hideLoading();
        });
}

function AdicionarConvenioUnidade() {
    if (!ValidarCampos()) {
        return true;
    }

    var idUnidade = $("#UnidadeID").val();
    var idTipoSelo = $("#TipoSeloId").val();
    var valor = $("#convenioUnidadeValor").val();
    
    showLoading();
    $.post("/Convenio/AdicionarConvenioUnidade", { idUnidade: idUnidade, idTipoSelo: idTipoSelo, valor: valor })
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {
                    //verificação adicionada a pedido da GTE-1778 - inicio
                    toastr.error(response.Mensagem + " - Unidade: " + response.Unidade + " Tipo Selo :" + response.TipoSelo);

                    hideLoading();
                    //verificação adicionada a pedido da GTE-1778 - fim               
                }
                else {
                    hideLoading();
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }

            } else {
                $("#lista-convenioUnidade-result").empty();
                $("#lista-convenioUnidade-result").append(response);
                $("#convenioUnidadeValor").val("0,00");
                hideLoading();
            }
        })
        .fail(() => { })
        .always(() => {
            hideLoading();
        });
}

function ValidarCampos() {
    if ($("#UnidadeID").val() === "") {
        toastr.error("O campo \"Unidade\" é obrigatório");
        return false;
    }
    else if ($("#TipoSeloId").val() === "") {
        toastr.error("O campo \"Tipo Selo\" é obrigatório");
        return false;
    }
    else if ($("#convenioUnidadeValor").val() === "") {
        toastr.error("O campo \"Valor\" é obrigatório. Verifique se o Valor foi Parametrizado.");
        return false;
    }
    return true;
}

function RemoverConvenioUnidade(idConvenioUnidade) {
    showLoading();
    $.post("/Convenio/RemoverConvenioUnidade", { idConvenioUnidade: idConvenioUnidade })
        .done((response) => {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#lista-convenioUnidade-result").empty();
                $("#lista-convenioUnidade-result").append(response);
                $("#convenioUnidadeValor").val("0,00");
                hideLoading();
            }
        })
        .fail(() => { })
        .always(() => { });
}

function buscarConvenios() {
    BuscarPartialSemFiltro("/Convenio/BuscarConvenios", "#lista-tabela-convenio")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function ValidarCamposPrincipal() {

    var valor = false;

    if ($("#Descricao").val() === "") {
        toastr.error("O campo \"Nome do Convênio\" é obrigatório");
        return false;
    }
    //aberto bug.. solicitado a verificação aqui no front - inicio
    $("#data_table_address").each(function (index) {
        valor = true;
    });

    if (!valor) {
        toastr.error("Adicione ao menos um item a Lista de Unidades x Tipos de Selo!");
        return false;
    }
    //aberto bug.. solicitado a verificação aqui no front - fim

    return true;
}