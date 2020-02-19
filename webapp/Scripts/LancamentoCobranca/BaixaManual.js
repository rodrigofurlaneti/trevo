$(document).ready(function () {
    MakeChosen("contaFinanceira");
    MakeChosen("tipoServico");
    FormatarCampoData("dtVencimentoInicio");
    FormatarCampoData("dtVencimentoFim");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
    
    ClienteAutoComplete();
});

function CalcularValoresTotais() {
    var lancamentoTotal = BuscarQtdeLinhas();
    var valorTotal = BuscarColunasTotal(6);
    var valorMulta = BuscarColunasTotal(7);
    var valorJuros = BuscarColunasTotal(8);

    valorTotal = valorTotal.toFixed(2);
    valorMulta = valorMulta.toFixed(2);
    valorJuros = valorJuros.toFixed(2);

    var valorTotalF = "R$ " + valorTotal.toString().replace('.', ',');
    var valorMultaF = "R$ " + valorMulta.toString().replace('.', ',');
    var valorJurosF = "R$ " + valorJuros.toString().replace('.', ',');

    $("#TotalLancamento").val(lancamentoTotal);
    $("#ValorTotalContrato").val(valorTotalF);
    $("#ValorTotalMulta").val(valorMultaF);
    $("#ValorTotalJuros").val(valorJurosF);
}


function Pesquisar() {
    let documento = $("#documento").val();
    if (documento && !CPFValido(documento) && !CnpjValido(documento)) {
        toastr.error("CPF/CNPJ Inválido!");
        return;
    }

    var filtro = {
        ContaFinanceira: {
            Id: $("#contaFinanceira").val()
        },
        TipoServico: $("#tipoServico").val(),
        DataVencimentoInicio: $("#dtVencimentoInicio").val(),
        DataVencimentoFim: $("#dtVencimentoFim").val(),
        Cliente: {
            Id: $("#cliente").val()
        },
        Documento: documento
    };


    post("Pesquisar", filtro)
        .done((response) => {
            $("#lista-lancamentoCobrancas").empty().html(response.lancamentosGrid);

            $("#TotalLancamento").val(response.totallancamentos);
            $("#ValorTotalContrato").val(response.totalcontratosF);
            $("#ValorTotalMulta").val(response.totalmultasF);
            $("#ValorTotalJuros").val(response.totaljurosF);
        });
}

function BuscarLancamentoCobrancas() {

    $.ajax({
        url: "/baixaManual/BuscarLancamentoCobrancas",
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (result.tipo === "danger") {
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
                $("#lista-lancamentoCobrancas").html(result.lancamentosGrid);

                $("#TotalLancamento").val(result.totallancamentos);
                $("#ValorTotalContrato").val(result.totalcontratosF);
                $("#ValorTotalMulta").val(result.totalmultasF);
                $("#ValorTotalJuros").val(result.totaljurosF);

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
            MetodoUtil();
        }
    });
}

function PagamentoParcial() {
    if ($("[id*='chkItem-']:checked").length <= 0) {
        toastr.warning("Selecione um ou mais lancamentos!", "Pagamento Parcial");
        return;
    }

    var lancamentos = [];
    $("[id*='chkItem-']:checked").each(function () {
        lancamentos.push($(this).val());
    });

    $.ajax({
        url: "/baixaManual/PagamentoParcial",
        type: "POST",
        dataType: "json",
        data: {
            idsLancamentosCobranca: lancamentos
        },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
            }
            else if (typeof result === "object") {
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
                $("#modalBodyPagamento").append(result);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });

                $('#modalPagamento').on('shown.bs.modal', function (e) {
                    $('.bootstrap-datetimepicker-widget').css('z-index', 1500);
                });
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
}

function PagamentoTotal() {
    if ($("[id*='chkItem-']:checked").length <= 0) {
        toastr.warning("Selecione um ou mais lancamentos!", "Pagamento Total");
        return;
    }

    var lancamentos = [];
    $("[id*='chkItem-']:checked").each(function () {
        lancamentos.push($(this).val());
    });

    $.ajax({
        url: "/baixaManual/PagamentoTotal",
        type: "POST",
        dataType: "json",
        data: {
            idsLancamentosCobranca: lancamentos
        },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
            }
            else if (typeof result === "object") {
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
                $("#modalBodyPagamento").append(result);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });

                $('#modalPagamento').on('shown.bs.modal', function (e) {
                    $('.bootstrap-datetimepicker-widget').css('z-index', 1500);
                });
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
}

function PossuiDesconto(ele) {
    var id = $(ele).attr("Id").split('-')[1];

    if ($(ele).prop("checked") === true) {
        $("#vlrPago-" + id).removeAttr("readonly");
        $("#vlrPago-" + id).removeAttr("disabled");
    }
    else {
        $("#vlrPago-" + id).attr("readonly", "readonly");
        $("#vlrPago-" + id).attr("disabled", "disabled");

        $("#vlrPago-" + id).val("");
    }
    //$(".valor-justificativa").each(function () {
    //    if ($(ele).prop("checked") === true) {
    //        $(this).css("display", "");
    //    }
    //    else {
    //        $(this).css("display", "none");
    //    }
    //});
    $("#lblDescontoAcrescimo-" + id).val("");
    $("#valorDescontoAcrescimo-" + id).val("");
    $("#tipoDescontoAcrescimo-" + id).val("");
    $("#txtJustificaDesconto-" + id).val("");
}

function EfetuarPagamentoTotal() {
    
    let valido = true;

    valido = validaCamposObrigatorios(valido, "[id*='nroRecibo-']");
    valido = validaCamposObrigatorios(valido, "[id*='dtPag-']");
    valido = validaCamposObrigatorios(valido, "[id*='dtCompetencia-']");
    valido = validaCamposObrigatorios(valido, "[id*='vlrPago-']");
    valido = validaCamposObrigatorios(valido, "[id*='formapagamento-']");
    valido = validaCamposObrigatorios(valido, "[id*='contacontabil-']");
    

    if (!valido) {
        toastr.warning("Todos os campos são obrigatórios, menos a justificativa", "Pagamento Total");
        return;
    }

    var dados = [];

    $("[id*='vlrPago-']").each(function () {
        var id = $(this).attr("id").split('-')[1];

        var nroRecibo = $("#nroRecibo-" + id).val();
        var dtPagamento = $("#dtPag-" + id).val();
        var dtCompetencia = $("#dtCompetencia-" + id).val();
        var vlrPago = $(this).val();
        var formaPagamento = $("#formapagamento-" + id).val();
        var contacontabil = $("#contacontabil-" + id).val();

        var vlrDescontoAcrescimo = $("#valorDescontoAcrescimo-" + id).val();
        var tipoDescontoAcrescimo = $("#tipoDescontoAcrescimo-" + id).val();
        var txtJustificativaDesconto = $("#txtJustificaDesconto-" + id).val();

        dados.push({
            Id: id,
            NumeroRecibo: nroRecibo,
            DataPagamento: dtPagamento,
            DataCompetencia: dtCompetencia,
            ValorPago: vlrPago,
            FormaPagamento: formaPagamento,
            ContaContabil: {
                Id: contacontabil
            },
            ValorDescontoAcrescimo: vlrDescontoAcrescimo,
            TipoDescontoAcrescimo: tipoDescontoAcrescimo,
            JustificativaDesconto: txtJustificativaDesconto
        });
    });

    $.ajax({
        url: "/baixaManual/EfetuarPagamentoTotal",
        type: "POST",
        dataType: "json",
        data: { dados: dados },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
            }
            else if (typeof result === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });

                $("#modalBodyPagamento").empty();
                $("#modalPagamento").modal('hide');

                window.location.href = "/baixamanual/index/";
            }
            else {
                $("#modalBodyPagamento").empty();
                $("#modalPagamento").modal('hide');

                window.location.href = "/baixamanual/index/";
            }
        },
        error: function (error) {
            $("#modalBodyPagamento").empty();
            $("#modalPagamento").modal('hide');

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
}

function EfetuarPagamentoParcial() {

    let valido = true;

    valido = validaCamposObrigatorios(valido, "[id*='nroRecibo-']");
    valido = validaCamposObrigatorios(valido, "[id*='vlrPago-']");
    valido = validaCamposObrigatorios(valido, "[id*='dtVenc-']");
    valido = validaCamposObrigatorios(valido, "[id*='dtCompetencia-']");
    valido = validaCamposObrigatorios(valido, "[id*='dtPag-']");
    valido = validaCamposObrigatorios(valido, "[id*='formapagamento-']");
    valido = validaCamposObrigatorios(valido, "[id*='contacontabil-']");
    
    if (!valido) {
        toastr.warning("Todos os campos são obrigatórios a justificativa", "Pagamento Parcial");
        return;
    }

    var dados = [];

    $("[id*='vlrPago-']").each(function () {
        var id = $(this).attr("id").split('-')[1];

        var nroRecibo = $("#nroRecibo-" + id).val();
        var vlrPago = $(this).val();
        var dtPagamento = $("#dtPag-" + id).val();
        var dtCompetencia = $("#dtCompetencia-" + id).val();
        var dtVencimento = $("#dtVenc-" + id).val();
        var formaPagamento = $("#formapagamento-" + id).val();
        var contacontabil = $("#contacontabil-" + id).val();

        var vlrDesconto = $("#valorDescontoAcrescimo-" + id).val();
        var txtJustificativaDesconto = $("#txtJustificaDesconto-" + id).val();

        //let vencido = false;
        //if (Date.parse(dtPagamento) > Date.parse(dtVencimento)) {
        //    vencido = true;
        //}

        //var valorcontrato = ConvertToFloat($(this).data("valorcontrato"));
        //var valorparcial = ConvertToFloat($(this).val());

        ////if (valorparcial > valorcontrato && !vencido) {
        ////    valido = false;
        ////}

        dados.push({
            Id: id,
            NumeroRecibo: nroRecibo,
            DataPagamento: dtPagamento,
            ValorPago: vlrPago,
            DataVencimento: dtVencimento,
            DataCompetencia: dtCompetencia,
            FormaPagamento: formaPagamento,
            ContaContabil: {
                Id: contacontabil
            },
            ValorDesconto: vlrDesconto,
            JustificativaDesconto: txtJustificativaDesconto
        });
    });

    //if (!valido) {
    //    toastr.error("O valor não pode ser maior que o valor Total");
    //    return;
    //}

    post("EfetuarPagamentoParcial", { dados })
        .done((response) => {
            if (response.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(response.Modal);
            }
            else if (typeof response === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });

                $("#modalBodyPagamento").empty().append(response);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
                $("#modalPagamento").modal('hide');
            }
            else {
                $("#modalBodyPagamento").empty().append(response);
                $("#modalPagamento").modal({ backdrop: 'static', keyboard: false });
                $("#modalPagamento").modal('hide');
            }
        });
}

function Cancelar() {
    $("#modalBodyPagamento").empty();
    $("#modalPagamento").modal('hide');
}

function SelecionarTodos() {
    var checked = $("#SelecionarTodos").prop("checked");

    $("[id*='chkItem-']").each(function () {
        if (checked)
            $(this).prop("checked", "true");
        else
            $(this).prop("checked", "");
    });
}

function SelecionaItem() {
    if ($("[id*='chkItem-']").length === $("[id*='chkItem-']:checked").length) {
        $("#SelecionarTodos").prop("checked", "true");
        return;
    }

    $("[id*='chkItem-']").each(function () {
        if ($(this).prop("checked") === false) {
            $("#SelecionarTodos").prop("checked", "");
            return;
        }
    });
}

function marcaCampo(campo) {
    $(campo).css("background-color", "rgb(255, 193, 7, 40%)");
}

function desmarcaCampo(campo) {
    $(campo).css("background-color", "");
}

function validaCamposObrigatorios(valido, seletor) {

    $(seletor).each(function () {
        var preenchido = $(this).val();
        if (!preenchido || preenchido === "0,00") {
            //marcaCampo(this);
            valido = false;
        }
        //else {
        //    desmarcaCampo(this);
        //}
    });

    return valido;
}