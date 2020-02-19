$(document).ready(function () {

    MakeChosen("contaFinanceira", null, "85%");
    MakeChosen("tipoServico", null, "85%");
    MakeChosen("unidade", null, "85%");
    MakeChosen("supervisor", null, "85%");
    MakeChosen("dropTipoJuros", null, "85%");
    MakeChosen("dropTipoMulta", null, "85%");

    FormatarCampoData("dtInicioFiltro");
    FormatarCampoData("dtFimFiltro");
    FormatarCampoData("dtVencimento");

    $("#juros").prop("readonly", "readonly");
    $("#multa").prop("readonly", "readonly");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });   

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/GeracaoCNABLancamentoCobranca/BuscarCliente',
                data: { descricao: request.term },
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
                    console.log(response.responseText);

                    $("#cliente").val(""); //hiddenId
                    $("#clienteText").val(""); //hiddenText
                },
                failure: function (response) {
                    console.log(response.responseText);

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
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            $("#clientes").val(""); //autocomplete
            $("#cliente").val(""); //hiddenId
            $("#clienteText").val(""); //hiddenText
        }
    });
});

function Pesquisar() {
    var contaFinanceira = $("#contaFinanceira").val();
    if (!contaFinanceira) {
        toastr.warning("Informe uma Conta Financeira!", "Campos");
        return true;
    }

    var dadosValidos = true;
    var dataInicio = $("#dtInicioFiltro").val();
    var dataFim = $("#dtFimFiltro").val();

    if (dataInicio !== "" && dataInicio !== undefined && verificaData($("#dtInicioFiltro").val()) === false) {
        toastr.error("O campo \"Data DE\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }
    if (dataFim !== "" && dataFim !== undefined && verificaData($("#dtFimFiltro").val()) === false) {
        toastr.error("O campo \"Data ATÉ\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }

    var dataParseInicio = dataInicio !== null && dataInicio !== "" ? new Date(dataInicio.split("/")[1] + '-' + dataInicio.split("/")[0] + '-' + dataInicio.split("/")[2]) : null;
    var dataParseFim = dataFim !== null && dataFim !== "" ? new Date(dataFim.split("/")[1] + '-' + dataFim.split("/")[0] + '-' + dataFim.split("/")[2]) : null;

    if (dataInicio !== "" && dataInicio !== undefined
        && dataFim !== "" && dataFim !== undefined
        && dataParseInicio > dataParseFim) {
        toastr.error("O campo \"Data DE\" deve ser menor que o campo \"Data ATÉ\"!");
        dadosValidos = false;
    }

    if (!dadosValidos) {
        e.preventDefault();
        return false;
    }

    var filtro = {
        ContaFinanceira: { Id: contaFinanceira },
        TipoServico: $("#tipoServico").val(),
        Unidade: {
            Id: $("#unidade").val()
        },
        StatusLancamentoCobranca: $("#statusLancamento").val(),
        TipoFiltroGeracaoCNAB: $("#tipoFiltroGeracaoCNAB").val(),
        Supervisor: {
            Id: $("#supervisor").val()
        },
        Cliente: {
            Id: $("#cliente").val()
        },
        DataDEFiltro: dataInicio,
        DataATEFiltro: dataFim
    };

    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/Pesquisar",
        type: "POST",
        dataType: "json",
        data: {
            filtro: filtro
        },
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
                hideLoading();
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

function BuscarLancamentoCobrancas() {
    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/BuscarLancamentoCobrancas",
        type: "POST",
        dataType: "json",
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
                hideLoading();
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

function RetirarLancamento(id) {
    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/RetirarLancamento",
        type: "POST",
        dataType: "json",
        data: {
            id: id
        },
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

function RetirarLancamentosSelecionados() {
    var lista = [];

    $("input[id*='chkuni']:checked").each(function () {
        lista.push($(this).attr("Id").split("-")[1]);
    });

    if ($("input[id*='chkuni']:checked").length <= 0) {
        toastr.error("Selecione um ou mais registros da listagem para retirar!", "Retirar Selecionados");
        return false;
    }

    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/RetirarLancamentosSelecionados",
        type: "POST",
        dataType: "json",
        data: {
            itens: lista
        },
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

function GerarCNAB() {
    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/GerarPesquisados",
        type: "POST",
        dataType: "json",
        data: {
            tipoServico: $("#tipoServico").val(),
            dtVencimento: $("#dtVencimento").val(),
            tipoValorJuros: $("#dropTipoJuros").val(),
            juros: $("#juros").val(),
            tipoValorMulta: $("#dropTipoMulta").val(),
            multa: $("#multa").val()
        },
        success: function (result) {
            if (typeof result === "object" && result.TipoModal !== undefined) {
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
                $("#modalBodyPagamento").empty();
                $("#modalBodyPagamento").append(result.Boletos);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            }
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalBodyPagamento").append(error.responseText);
            $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

    //toastr.success("Geracao de CNAB com os lancamentos exibidos", "Gerar CNAB");
}

function GerarPDF() {
    showLoading();
    window.open("/GeracaoCNABLancamentoCobranca/GerarPDF");
    hideLoading();
}

function GerarCNABModal() {
    $.ajax({
        url: "/GeracaoCNABLancamentoCobranca/GerarPesquisadosModal",
        type: "POST",
        dataType: "json",
        data: {
            dtVencimento: $("#dtVencimento").val(),
            tipoValorJuros: $("#dropTipoJuros").val(),
            juros: $("#juros").val(),
            tipoValorMulta: $("#dropTipoMulta").val(),
            multa: $("#multa").val()
        },
        success: function (result) {
            if (typeof result === "object" && result.TipoModal !== undefined) {
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
                $("#modalBodyPagamento").empty();
                $("#modalBodyPagamento").append(result.Boletos);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            }
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalBodyPagamento").append(error.responseText);
            $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

    //toastr.success("Geracao de CNAB com os lancamentos exibidos", "Gerar CNAB");
}


function Cancelar() {
    $("#modalBodyPagamento").empty();
    $("#modalPagamento").modal('hide');
}

function Imprimir() {
    var elem = $(".imprimir");

    var mywindow = window.open('', 'PRINT', 'height=640,width=920');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(elem.html());
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}

function HabilitarJuros() {
    $("#juros").val("");
    if ($("#chkJuros").prop("checked") === true)
        $("#juros").prop("readonly", "");
    else
        $("#juros").prop("readonly", "readonly");
}

function HabilitarMulta() {
    $("#multa").val("");
    if ($("#chkMulta").prop("checked") === true)
        $("#multa").prop("readonly", "");
    else
        $("#multa").prop("readonly", "readonly");
}

function CheckTodos() {
    $("input[id*='chkuni']").prop('checked', $("#chktodos").prop('checked'));
}

function CheckUni(elem) {
    if ($("#chktodos").prop('checked') === true
        && $(elem).prop('checked') === false)
        $("#chktodos").prop('checked', $(elem).prop('checked'));

    if ($("#chktodos").prop('checked') === false
        && $("input[id*='chkuni']:checked").length === $("input[id*='chkuni']").length)
        $("#chktodos").prop('checked', $(elem).prop('checked'));
}