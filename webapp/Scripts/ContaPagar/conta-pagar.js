$(document).ready(function () {
    FormatarCampos();

    if (!$("#juros").val() || $("#juros").val() === "0,00")
        $("#juros").readonly();
    else
        document.querySelector("#chkJuros").checked = true;

    if (!$("#multa").val() || $("#multa").val() === "0,00")
        $("#multa").readonly();
    else
        document.querySelector("#chkMulta").checked = true;

    if (location.search) {
        BuscarContaPagarPeloId(obterParametroDaUrl("contaPagarId"));
    }

    if (isEdit()) {
        disabledChosen("contaFinanceira");
        disabledChosen("tipoPagamento");
        disabledChosen("departamento");
        disabledChosen("fornecedor");
        disabledChosen("numeroParcelas");
        disabledChosen("unidade");
        disabledChosen("contacontabil");
    }

    MetodoUtil();

    $("#contaFinanceira").change(function () {
        BuscarEmpresaDaContaFinanceira();
    });

    $("#formaPagamento").change(function () {
        BuscarEmpresaDaContaFinanceira();

        $("#container-codigo-de-barras, #container-contribuinte").hide();
        $("#codigo-de-barras, #contribuinte").val("");

        let textoSelecionado = $(this).find(":selected").text();
        if (textoSelecionado === "Imposto sem código de barras (Tributos / Impostos)")
            $("#container-contribuinte").show();

        else if (textoSelecionado === "Imposto com código de barras (Tributos / Impostos) (CNAB)")
            $("#container-codigo-de-barras").show();

        else if (textoSelecionado === "Boleto (Título Bancário) (CNAB)")
            $("#container-codigo-de-barras").show();
    });

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#Form").submit(function (e) {
        if (!ValidarCampos()) {
            e.preventDefault();
            return false;
        }
        if (!VerificacaoBloqueioReferencia()) {
            e.preventDefault();
            return false;
        }
    });

    $('.valmoney').each(function () {
        $(this).maskMoney('mask', $(this).val());
    });
});

function ValidarCampos() {
    var dadosValidos = true;

    if ($("#contaFinanceira").val() === '') {
        toastr.error("O campo \"Conta Financeira\" deve ser preenchido!", "Conta Financeira Inválida!");
        dadosValidos = false;
    }

    if ($("#tipoPagamento").val() === '') {
        toastr.error("O campo \"Tipo de Pagamento\" deve ser preenchido!", "Tipo de Pagamento Inválido!");
        dadosValidos = false;
    }

    if ($("#formaPagamento").val() === '') {
        toastr.error("O campo \"Forma de Pagamento\" deve ser preenchido!", "Forma de Pagamento Inválido!");
        dadosValidos = false;
    }

    if ($("#unidade").val() === '') {
        toastr.error("O campo \"Unidade\" deve ser preenchido!", "Unidade Inválida!");
        dadosValidos = false;
    }

    if ($("#fornecedor").val() === '') {
        toastr.error("O campo \"Fornecedor\" deve ser preenchido!", "Fornecedor Inválido!");
        dadosValidos = false;
    }

    if (!verificaData($("#data").val())) {
        toastr.error('O campo \"Data\" deve ser preenchido com uma data válida!', 'Data Inválida!');
        dadosValidos = false;
    }

    if (!$("#valor-total").val() || $("#valor-total").val() === "0,00") {
        toastr.error("Adicione pelo menos um item ao grid", "Item obrigatório");
        dadosValidos = false;
    }

    if (!dadosValidos)
        return false;
    return true;
}

function VerificacaoBloqueioReferencia() {
    var retorno = true;
    showLoading();
    $.ajax({
        url: "/ContaPagar/VerificacaoBloqueioReferencia",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            model: {
                Id: $("#Id").val(),
                DataVencimento: $("#data").val(),
                DataCompetencia: $("#competencia").val()
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

function BuscarEmpresaDaContaFinanceira() {
    let contaFinanceiraId = $("#contaFinanceira").val();
    let textoSelecionado = $("#formaPagamento").find(":selected").text();
    if (textoSelecionado === "Imposto sem código de barras (Tributos / Impostos)" && contaFinanceiraId) {
        get(`BuscarEmpresaDaContaFinanceira?contaFinanceiraId=${contaFinanceiraId}`)
            .done((response) => $("#contribuinte").val(response.Descricao));
    } else {
        $("#contribuinte").val("");
    }
}

function FormatarCampos() {
    MakeChosen("contaFinanceira");
    MakeChosen("tipoPagamento");
    MakeChosen("formaPagamento");
    MakeChosen("departamento");
    MakeChosen("fornecedor");
    MakeChosen("numeroParcelas");
    MakeChosen("unidade");
    MakeChosen("contacontabil");
    FormatarCampoData("data");
    FormatarCampoData("data-vencimento");
    FormatarCampoDataMesAno("competencia");
    MakeChosen("dropTipoJuros");
    MakeChosen("dropTipoMulta");
    MakeChosen("grid-contacontabil");
    MakeChosen("grid-unidade");
    FormatarNumerosDecimais("#valor-total");
}

function AdicionarItem() {
    let contaContabilId = $("#grid-contacontabil").val();
    let unidadeId = $("#grid-unidade").val();
    let valor = $("#grid-valor").val();

    if (!contaContabilId) {
        toastr.warning("Informe a conta contábil para adicionar", "Campo Obrigatório");
        return false;
    }
    if (!unidadeId) {
        toastr.warning("Informe a unidade para adicionar", "Campo Obrigatório");
        return false;
    }
    if (!valor || valor === "0,00") {
        toastr.warning("Informe o valor para adicionar", "Campo Obrigatório");
        return false;
    }

    post("AdicionarItem", { contaContabilId, unidadeId, valor })
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            let valorComMascara = $("#valor-total").val(response.ValorTotal.toFixed(2)).masked();
            $("#valor-total").val(valorComMascara).masked();

            $("#grid-valor").val("0,00");
            $("#grid-contacontabil").val(0).trigger('chosen:updated');
            $("#grid-unidade").val(0).trigger('chosen:updated');
        });
}

function RemoverItem(contaContabilId, unidadeId, valor) {
    post("RemoverItem", { contaContabilId, unidadeId, valor })
        .done((response) => {
            $("#lista-item").empty().append(response.Grid);
            let valorComMascara = $("#valor-total").val(response.ValorTotal.toFixed(2)).masked();
            $("#valor-total").val(valorComMascara).masked();
        });
}

function BuscarContaPagarPeloId (id) {
    return post("BuscarContaPagarPeloId", { id })
        .done((response) => {
            $("#lista-lancamentoCobrancas").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function Pesquisar() {
    //debugger;
    var contaFinanceira = $("#contaFinanceira").val();
    var Data = $("#data").val();
    var competencia = $("#competencia").val();
    var departamento = $("#departamento").val();
    var fornecedor = $("#fornecedor").val();
    var tipopagamento = $("#tipoPagamento").val();
    var unidade = $("#unidade").val();
    var contacontabil = $("#contacontabil").val();

    if (!contaFinanceira) {
        toastr.warning("Informe uma Conta Financeira!", "Campos");
        return true;
    } else if (Data && !verificaData(Data)) {
        toastr.warning("O campo \"Data Vencimento\" deve ser preenchido com uma data válida!", "Campos");
        return true;
    }

    var filtro = {
        DataVencimento: Data,
        DataCompetencia: competencia,
        ContaFinanceira: { Id: contaFinanceira },
        Departamento: { Id: departamento },
        Fornecedor: { Id: fornecedor },
        Unidade: { Id: unidade },
        ContaContabil: { Id: contacontabil },
        TipoPagamento: tipopagamento
    };

    return post("Pesquisar", { filtro })
        .done((response) => {
            if (typeof result === "object") {
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
                $("#lista-lancamentoCobrancas").append(response);
                ConfigTabelaGridSemCampoFiltroPrincipal();
            }
        });
}

function ConfirmarPagamento() {
    var id = $("#idcontapagar").val();
    var tipodocumento = $("#tipodocumento").val();
    var numerodocumento = $("#numerodocumento").val();

    if (tipodocumento === null || tipodocumento === 'undefined' || tipodocumento === 0) {
        toastr.warning("Informe um Tipo de Documento!", "Tipo Documento");
        return true;
    }

    if (numerodocumento === null || numerodocumento === 'undefined' || numerodocumento === '') {
        toastr.warning("Informe um Numero de Documento!", "Numero de Documento");
        return true;
    }

    var contapagar = {
        Id: id,
        TipoDocumentoConta: tipodocumento,
        NumeroDocumento: numerodocumento
    };

    $.ajax({
        url: "/contapagar/ConfirmarPagamento",
        type: "POST",
        dataType: "json",
        data: { contapagar },
        success: function (result) {
            $("#modalPagamento").modal('hide');
            $("#lista-lancamentoCobrancas").empty();
            $("#lista-lancamentoCobrancas").append(result);

            if (typeof result === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () {
                        if (location.search) {
                            showLoading();
                            location.href = location.pathname;
                        } else {
                            Pesquisar();
                        }
                    });
            }
        },
        error: function (error) {
            //$("#modalBodyPagamento").empty();
            //$("#modalPagamento").modal('hide');
            $("#modalPagamento").modal('hide');
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

let contaParaNegarId = 0;
function AbrirModalNegar(id) {
    contaParaNegarId = id;
    $("#observacao").val("");
    $("#modal-negar").modal("show");
}

function NegarConta() {
    let observacao = $("#observacao").val();

    if (!observacao) {
        toastr.warning("Informe a observação");
        return null;
    }

    return post("NegarConta", { id: contaParaNegarId, observacao })
        .done(() => {
            $("#modal-negar").modal("hide");
            Pesquisar();
            ObterNotificacoes();
        });
}

function ExecutarPagamentoModal(id) {
    var idsLancamentosCobranca = id;

    $.ajax({
        url: "/contapagar/ExecutarPagamentoModal",
        type: "POST",
        dataType: "json",
        data: { idsLancamentosCobranca },
        success: function (result) {
            if (typeof result === "object") {
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

function ExecutarPagamento() {
    var id = $("#executarpagamento").val();
    
    $.ajax({
        url: "/contaPagar/ExecutarPagamento",
        type: "POST",
        async: true,
        dataType: "json",
        data: {
            id: id
        },
        success: function (result) {
            if (typeof result === "object") {
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
            hideLoading();
            $("#lista-lancamentoCobrancas").empty();
            $("#lista-lancamentoCobrancas").append(error.responseText);
        },
        beforeSend: function () {
        },
        complete: function () {
            MetodoUtil();
        }
    });

}

function disabledChosen(id) {
    $("#" + id).prop('disabled', true).trigger('chosen:updated').prop('disabled', false);
}

function makeReadonly(id) {
    $("#" + id).prop('readonly', true);
}

function HabilitarJuros() {
    $("#juros").val("0,00");
    if ($("#chkJuros").prop("checked") === true)
        $("#juros").unReadonly();
    else
        $("#juros").readonly();
}

function HabilitarMulta() {
    $("#multa").val("0,00");
    if ($("#chkMulta").prop("checked") === true)
        $("#multa").unReadonly();
    else
        $("#multa").readonly();
}