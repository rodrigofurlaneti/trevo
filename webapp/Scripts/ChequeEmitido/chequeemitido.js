$(document).ready(function () {
    BuscarChequesEmitidos();
    MakeChosen("fornecedor");
    MakeChosen("status");
    MakeChosen("lancamento");
    MakeChosen("bancoCheque");

    FormatarCampoData("dataemissao");
    FormatarCampoData("dataemissao");

    $('#Numero').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });

    $("#agencia").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $("#digitoagencia").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $("#conta").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $("#digitoconta").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });


    FormataCampoCpf("cpf");

    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
    });

    $("#fornecedor").change(function () {
        CarregaLancamentos(true);
    });

    $("#Valor").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#Numero").change(function () {
        var valor = $(this).val();

        if (valor < 0) {
            $(this).val(0);
            toastr.error("O campo \"Numero Cheque\" não pode ser negativo");
        }
    });

    $("#ChequeForm").submit(function (e) {
        var dadosValidos = true;

        if (!somenteLetra($("#Emitente").val())) {
            toastr.error("O campo Emitente não permite caracteres númericos!", "Emitente inválido!");
            dadosValidos = false;
        }

        if ($("#fornecedor").val() === '') {
            toastr.error("O campo Conta Financeira deve ser preenchido!", "Conta Financeira inválida!");
            dadosValidos = false;
        }

        if ($("#bancoCheque").val() === '') {
            toastr.error("O campo Banco deve ser preenchido!", "Banco inválido!");
            dadosValidos = false;
        }


        if ($("#Numero").val() === '') {
            toastr.error("O campo numero deve ser preenchido!", "Numero inválido!");
            dadosValidos = false;
        }

        if ($("#Agencia").val() === '') {
            toastr.error("O campo agencia deve ser preenchido!", "Agencia inválida!");
            dadosValidos = false;
        }

        if ($("#Conta").val() == '') {
            toastr.error("O campo conta deve ser preenchido!", "Agencia inválida!");
            dadosValidos = false;
        }

        if ($("#DigitoConta").val() == '') {
            toastr.error("O campo digito conta deve ser preenchido!", "Digito Conta inválida!");
            dadosValidos = false;
        }

        if (!CPFValido($("#cpf").val())) {
            toastr.error('Preencha um CPF válido!', 'CPF Inválido!');
            dadosValidos = false;
        }

        if ($("#Valor").val() == '') {
            toastr.error("O campo valor deve ser preenchido!", "Valor inválido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    if ($("#fornecedor").val() != '' && $("#fornecedor").val() != undefined && $("#fornecedor").val() != null && $("#fornecedor").val() != '0') {

        CarregaLancamentos(false);
    }
});

function BuscarChequesEmitidos() {
    BuscarPartialSemFiltro("/ChequeEmitido/BuscarChequesEmitidos", "#lista-cheques")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return true;
}


function AdicionarLancamento() {

    if ($("#lancamento").val() == '' || $("#lancamento").val() == undefined || $("#lancamento").val() == null || $("#lancamento").val() == '0') {
        toastr.error("Selecione uma Conta pagar!", "Conta Pagar");
        return false;
    }

    var idLancamento = $("#lancamento").val();

    showLoading();
    $.post("/ChequeEmitido/AdicionarLancamento", { idLancamento: idLancamento})
        .done((response) => {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {
                    toastr.error(response.Mensagem);
                    hideLoading();
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
                $("#lista-lancamento-result").empty();
                $("#lista-lancamento-result").append(response);
                $("#lancamento").val("0").trigger("chosen:updated");
                hideLoading();
            }
        })
        .fail((error) => {
            alert(error.response);
        })
        .always(() => {
            hideLoading();
        });
}

function RemoverLancamento(idLancamento) {
    showLoading();
    $.post("/ChequeEmitido/RemoverLancamento", { idLancamento: idLancamento })
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
                $("#lista-lancamento-result").empty();
                $("#lista-lancamento-result").append(response);
                $("#lancamento").val("0").trigger("chosen:updated");
                hideLoading();
            }
        })
        .fail(() => { })
        .always(() => { });
}


function CarregaLancamentos(limparGrid) {
    limpaCombo('#lancamento');
    
    let idFornecedor = $('#fornecedor').val();
    if (idFornecedor)
        populaCombo('#lancamento', 'ChequeEmitido', 'CarregarLancamento', { idFornecedor: parseInt(idFornecedor), limpaLancamentoSalvar: limparGrid });

    if (limparGrid)
        $("#lista-lancamento-result").empty();
}