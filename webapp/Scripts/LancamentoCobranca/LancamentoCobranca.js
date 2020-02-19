
var associados = [];

$(document).ready(function () {
    AlterarCamposSeSeguroReembolso();

    MakeChosen("contaFinanceira", null, "100%");
    MakeChosen("unidade", null, "100%");
    MakeChosen("unidadeFiltro", null, "90%");
    MakeChosen("statusLancamentoFiltro", null, "90%");
    MakeChosen("tipoServico", null, "100%");
    FormatarCampoData("dtVencimento");
    //FormatarCampoData("dtVencimentoFiltro");
    FormatarCampoDataMesAno("dtCompetencia");

    ValidaNumero("contratoBusca");

    $("#tipoServico").on("change", function () {
        AlterarCamposSeSeguroReembolso();

        ExibirAreaAssociado($(this).val(), $("#unidade").val(), $("#cliente").val());
    });

    $("#unidade").change(function () {
        BuscarEmpresa($("#contaFinanceira").val(), this.value);

        ExibirAreaAssociado($("#tipoServico").val(), $(this).val(), $("#cliente").val());
    });

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/lancamentoCobranca/BuscarCliente',
                data: { descricao: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#cliente").val("");
                        $("#clienteText").val("");

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
                        }));
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                    $("#clienteText").val("");
                    $("#cliente").val("");
                },
                failure: function (response) {
                    console.log(response.responseText);
                    $("#clienteText").val("");
                    $("#cliente").val("");
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
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            $("#clientes").val(""); //autocomplete
            $("#cliente").val(""); //hiddenId
            $("#clienteText").val(""); //hiddenText
        }

        ExibirAreaAssociado($("#tipoServico").val(), $(this).val(), $("#cliente").val());
    });

    $("#clientesFiltro").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/lancamentoCobranca/BuscarCliente',
                data: { descricao: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#clienteFiltro").val("");
                        $("#clienteTextFiltro").val("");

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
                        }));
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                    $("#clienteFiltro").val(""); //hiddenId
                    $("#clienteTextFiltro").val(""); //hiddenText
                },
                failure: function (response) {
                    console.log(response.responseText);
                    $("#clienteFiltro").val(""); //hiddenId
                    $("#clienteTextFiltro").val(""); //hiddenText
                }
            });
        },
        select: function (e, i) {
            $("#clienteTextFiltro").val(i.item.label);
            $("#clienteFiltro").val(i.item.id);
        },
        minLength: 3
    }).change(function () {
        var autoTexto = $("#clientesFiltro").val();
        var texto = $("#clienteTextFiltro").val();
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            $("#clientesFiltro").val(""); //autocomplete
            $("#clienteFiltro").val(""); //hiddenId
            $("#clienteTextFiltro").val(""); //hiddenText
        }
    });

    AutoCompleteAssociado();

    $("#valorContrato, #ValorMulta, #ValorJuros").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#LancamentoCobrancaForm").submit(function (e) {
        if (!ValidarCampos())
            return false;
        if (!VerificacaoBloqueioReferencia())
            return false;
    });

    if (isEdit() || isSave())
        BuscarEmpresa($("#contaFinanceira").val(), $("#unidade").val());
});

function ValidarCampos() {
    var dadosValidos = true;

    if (!$("#contaFinanceira").val()) {
        toastr.error("O campo \"Conta Financeira\" deve ser preenchido!");
        dadosValidos = false;
    }

    if ($("#tipoServico option:selected").text() !== "Seguro Reembolso" && !$("#cliente").val()) {
        toastr.error("O campo \"Cliente\" deve ser preenchido!");
        dadosValidos = false;
    }

    if (!$("#tipoServico").val()) {
        toastr.error("O campo \"Tipo de Serviço\" deve ser preenchido!");
        dadosValidos = false;
    }

    if (!$("#unidade").val()) {
        toastr.error("O campo \"Unidade\" deve ser preenchido!");
        dadosValidos = false;
    }
    
    if (!$("#dtVencimento").val()) {
        toastr.error("O campo \"Data de Vencimento\" deve ser preenchido!");
        dadosValidos = false;
    } else if (verificaData($("#dtVencimento").val()) === false) {
        toastr.error("O campo \"Data de Vencimento\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }

    if (!$("#dtCompetencia").val()) {
        toastr.error("O campo \"Data de Competência\" deve ser preenchido!");
        dadosValidos = false;
    } else if (verificaData("01/" + $("#dtCompetencia").val()) === false) {
        toastr.error("O campo \"Data de Competência\" deve ser preenchido com uma data no formato (Mês/Ano) válida!");
        dadosValidos = false;
    }

    if (!$("#valorContrato").val() || $("#valorContrato").val() === "0,00") {
        toastr.error("O campo \"Valor\" deve ser preenchido!");
        dadosValidos = false;
    }

    if (!dadosValidos)
        return false;
    return true;

}

function AutoCompleteAssociado() {
    $("#associados").autocomplete({
        source: function (request, response) {
            var tipoServico = $("#tipoServico").val();
            var unidade = $("#unidade").val();
            var cliente = $("#cliente").val();
            $.ajax({
                url: '/lancamentoCobranca/BuscarEntidadePorTipoServico',
                data: { descricao: request.term, tipoServico: tipoServico, unidade: unidade, cliente: cliente },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#associados").val("");
                        $("#associadoText").val("");

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
                        }));
                    }
                },
                error: function (response) {
                    console.log(response.responseText);
                    $("#associadoText").val("");
                    $("#associado").val("");
                },
                failure: function (response) {
                    console.log(response.responseText);
                    $("#associadoText").val("");
                    $("#associado").val("");
                }
            });
        },
        select: function (e, i) {
            $("#associadoText").val(i.item.label);
            $("#associado").val(i.item.id);
        },
        minLength: 3
    }).change(function () {
        var autoTexto = $("#associados").val();
        var texto = $("#associadoText").val();
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            $("#associados").val(""); //autocomplete
            $("#associado").val(""); //hiddenId
            $("#associadoText").val(""); //hiddenText
        }
    });
}

function ExibirAreaAssociado(tipoServico, unidade, cliente) {
    if (tipoServico === "" || unidade === "" || cliente === "") {
        $("#tipoServicoLabel").text("");
        $("#divCobrancaAssociada").css("display", "none");
        $("#associados").val(""); //autocomplete
        $("#associado").val(""); //hiddenId
        $("#associadoText").val(""); //hiddenText
        return;
    }
    else if (tipoServico === "1"
        //|| tipoServico === "2" || tipoServico === "6"
    ) {
        $("#tipoServicoLabel").text($("#tipoServico option:selected").text());
        $("#divCobrancaAssociada").css("display", "inline");
        $("#associados").val(""); //autocomplete
        $("#associado").val(""); //hiddenId
        $("#associadoText").val(""); //hiddenText
        return;
    }
    $("#tipoServicoLabel").text("");
    $("#divCobrancaAssociada").css("display", "none");
}

function BuscarEmpresa(contaFinanceiraId, unidadeId) {
    if (contaFinanceiraId && unidadeId)
        return get(`BuscarEmpresa?contaFinanceiraId=${contaFinanceiraId}&unidadeId=${unidadeId}`)
            .done((response) => {
                if (response) {
                    $("#container-empresa").show();
                    $("#empresa").val(response);
                } else {
                    $("#container-empresa").hide();
                }
            });
}

function AlterarCamposSeSeguroReembolso() {
    if ($("#tipoServico option:selected").text() === "Seguro Reembolso") {
        $("#container-cliente").hide();
        $("#container-cia-seguro").show();
    } else {
        $("#container-cliente").show();
        $("#container-cia-seguro").hide();
    }
}

function BuscarLancamentoCobrancas() {
    var status = $("#statusLancamentoFiltro").val() === "" ? 0 : parseInt($("#statusLancamentoFiltro").val());
    var unidade = $("#unidadeFiltro").val();
    var cliente = $("#clienteFiltro").val();
    var contrato = $("#contratoBusca").val();
    //var dataVencimento = $("#dtVencimentoFiltro").val();

    //if (validarPeriodo(dataInicial, dataFinal, true) === false) {
    //    toastr.warning("Os campos de data são obrigatórios e a data final deve ser maior ou igual à data inicial!", "Atenção");
    //    return;
    //}

    var filtro = {
        status: status,
        unidade: unidade,
        cliente: cliente,
        //dataVencimento: dataVencimento
        contrato: contrato
    };

    $.ajax({
        url: "/lancamentoCobranca/BuscarLancamentoCobrancas",
        type: "POST",
        dataType: "json",
        data: filtro,
        success: function (result) {
            if (typeof result === "object" && (result.Grid === undefined || result.Grid === null)) {
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
                $("#lista-lancamentoCobrancas").empty();
                $("#lista-lancamentoCobrancas").append(result.Grid);
                if (result.Grid.indexOf("Nenhum Lançamento de Cobrança") < 0)
                    ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_fixed_column");
            }
        },
        error: function (error) {
            $("#lista-lancamentoCobrancas").empty();
            $("#lista-lancamentoCobrancas").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function adicionarAssociado() {
    if ($("#associado").val() === "" || $("#associado").val() === null || $("#associado").val() === undefined)
        return false;

    var obj = {
        Id: $("#associado").val(),
        Descricao: $("#associados").val(),
        Categoria: $("#tipoServico").val()
    };

    associados.push(obj);
    atualizarAssociado(associados);
    preencheCamposContrato(obj.Id);
}

function atualizarAssociado(associados) {
    showLoading();
    $.post("/LancamentoCobranca/AtualizarAssociados", { associados })
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
                $("#lista-associados-result").empty();
                $("#lista-associados-result").append(response);

                $("#associados").val(""); //autocomplete
                $("#associado").val(""); //hiddenId
                $("#associadoText").val(""); //hiddenText
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function removerAssociado(id) {
    showLoading();
    $.post("/LancamentoCobranca/RemoverAssociado", { id })
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
                $("#lista-associados-result").empty();
                $("#lista-associados-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function VerificacaoBloqueioReferencia() {
    var retorno = true;
    showLoading();
    $.ajax({
        url: "/LancamentoCobranca/VerificacaoBloqueioReferencia",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            model: {
                Id: $("#Id").val(),
                DataCompetencia: $("#dtCompetencia").val(),
                DataVencimento: $("#dtVencimento").val()
            }
        },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
                retorno = false;
            }
            else if (typeof result === "object" && result.Sucesso !== undefined && !result.Sucesso) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
                retorno = false;
            }
            else {
                retorno = true;
            }
        },
        error: function (error) {
            console.log(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

    return retorno;
}

function preencheCamposContrato(id) {
    showLoading();
    $.post("/LancamentoCobranca/PreencheCamposContrato", { id })
        .done((response) => {
            $("#dtVencimento").val(response.DataVencimento);
            $("#valorContrato").val(response.ValorContrato);
            $("#unidade").val(response.Unidade); 
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}