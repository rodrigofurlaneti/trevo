$(document).ready(function () {
    MakeChosen("conta-financeira");
    MakeChosen("forma-pagamento");
    MakeChosen("unidade");
    MakeChosen("conta-contabil");
    MakeChosen("fornecedor");
    MakeChosen("dropTipoJuros");
    MakeChosen("dropTipoMulta");
    MakeChosen("tipo-filtro");
    FormatarCampoData("data-vencimento");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("")
});

function RetirarConta(id) {
    post("RetirarConta", { id })
        .done((response) => {
            $("#lista-contas-pagar").empty();

            if (response.Sucesso === false) {
                AtualizarAbrirModal(response.DadosModal);
                return;
            }

            $("#lista-contas-pagar").append(response);
        });
}

function Pesquisar() {
    let contaFinanceiraId = $("#conta-financeira").val();
    if (!contaFinanceiraId) {
        toastr.warning("Informe uma Conta Financeira!", "Campos");
        return false;
    }

    let filtro = {
        ContaFinanceira: {
            Id: contaFinanceiraId
        },
        FormaPagamento: $("#forma-pagamento").val(),
        Unidade: {
            Id: $("#unidade").val()
        },
        ContaContabil: {
            Id: $("#conta-contabil").val()
        },
        Fornecedor: {
            Id: $("#fornecedor").val()
        },
        DataVencimento: $("#data-vencimento").val(),
        TipoFiltro: $("#tipo-filtro").val()
    };

    post("Pesquisar", { filtro })
        .done((response) => {
            $("#lista-contas-pagar").empty();

            if (response.Sucesso === false) {
                AtualizarAbrirModal(response.DadosModal);
                return;
            }

            $("#lista-contas-pagar").append(response);
        });
}

function GerarCNAB() {
    let contaFinanceiraId = $("#conta-financeira").val();
    if (!contaFinanceiraId) {
        toastr.warning("Informe uma Conta Financeira!", "Campos");
        return false;
    }

    let filtro = {
        ContaFinanceira: {
            Id: contaFinanceiraId
        },
        TipoPagamento: $("#forma-pagamento").val(),
        Unidade: {
            Id: $("#unidade").val()
        },
        ContaContabil: {
            Id: $("#conta-contabil").val()
        },
        Fornecedor: {
            Id: $("#fornecedor").val()
        },
        DataVencimento: $("#data-vencimento").val(),
        TipoFiltro: $("#tipo-filtro").val()
    };

    post("GerarPesquisados", { filtro })
        .done((response) => {
            if (response.Sucesso === true) {
                $("#gerar-arquivo-remessa").click();
                Pesquisar();
            }
            else {
                AtualizarAbrirModal(response.DadosModal);
            }
        });
}

function RetirarContasSelecionadas() {
    var lista = [];

    $("input[id*='chkuni']:checked").each(function () {
        lista.push($(this).attr("Id").split("-")[1]);
    });

    if ($("input[id*='chkuni']:checked").length <= 0) {
        toastr.error("Selecione um ou mais registros da listagem para retirar!", "Retirar Selecionados");
        return false;
    }

    $.ajax({
        url: "/GeracaoCNABContasPagar/RetirarContasSelecionadas",
        type: "POST",
        dataType: "json",
        data: {
            itens: lista
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
                $("#lista-contas-pagar").empty();
                $("#lista-contas-pagar").append(result);
            }
        },
        error: function (error) {
            $("#lista-contas-pagar").empty();
            $("#lista-contas-pagar").append(error.responseText);
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