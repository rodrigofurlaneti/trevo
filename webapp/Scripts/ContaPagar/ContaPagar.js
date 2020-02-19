$(document).ready(function () {
    MakeChosen("contaFinanceira");
    MakeChosen("tipoServico");
    MakeChosen("unidade");
    MakeChosen("dropTipoJuros");
    MakeChosen("dropTipoMulta");
    FormatarCampoData("Data");

    MetodoUtil();

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

});

function Pesquisar() {
    var contaFinanceira = $("#contaFinanceira").val();
    var Data = $("#Data").val();
    var departamento = $("#departamento").val();
    var fornecedor = $("#fornecedor").val();
    var tipopagamento = $("#tipopagamento").val();

    if (contaFinanceira == "" || contaFinanceira == null || contaFinanceira == undefined) {
        toastr.warning("Informe uma Conta Financeira!", "Campos");
        return true;
    }
    
    var filtro = {
        Data: Data,
        ContaFinanceira: {
            Id: contaFinanceira
        },
        Departamento: {
            Id: departamento
        },
        Fornecedor: {
            Id: fornecedor
        },
        TipoPagamento: {
            Id: tipopagamento
        }
        //DataVencimentoInicio: $("#dtVencimentoInicio").val(),
        //DataVencimentoFim: $("#dtVencimentoFim").val(),
        //Cliente: {
        //    Id: $("#cliente").val()
        //},
        //CPF: $("#cpf").val()
    };

    $.ajax({
        url: "/contaPagar/Pesquisar",
        type: "POST",
        dataType: "json",
        data: {
            filtro: filtro
        },
        success: function (result) {
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
                $("#lista-lancamentoCobrancas").empty();
                $("#lista-lancamentoCobrancas").append(result);
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
        url: "/contaPagar/BuscarLancamentoCobrancas",
        type: "POST",
        dataType: "json",
        success: function (result) {
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
                $("#lista-lancamentoCobrancas").empty();
                $("#lista-lancamentoCobrancas").append(result);
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

function ExecutarPagamentoModal(id) {

    $("#executarpagamento").val(id);
}

function ExecutarPagamento() {
    
    var id = $("#executarpagamento").val();

    $.ajax({
        url: "/contaPagar/ExecutarPagamento",
        type: "POST",
        dataType: "json",
        data: {
            id: id
        },
        success: function (result) {
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

                $("#lista-lancamentoCobrancas").empty();
                $("#lista-lancamentoCobrancas").append(result);
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
            $(".modal-backdrop").css("display", "none")
        }
    });
    
}




function Cancelar() {
    $("#modalBodyPagamento").empty();
    $("#modalPagamento").modal('hide');
}

