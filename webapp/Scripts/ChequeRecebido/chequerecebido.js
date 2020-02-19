$(document).ready(function () {
    MakeChosen("status");
    MakeChosen("lancamento");
    MakeChosen("bancoCheque");

    AlternarDataDevolucao();

    FormatarCampoData("data-devolucao");
    FormatarCampoData("datadeposito");
    FormatarCampoData("dataprotesto");

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

    $("#status").change(function () {
        var valor = $(this).val();

        AlternarDataDevolucao();

        if (valor === '4') {
            HabilitaProtesto(true);
        }
        else {
            HabilitaProtesto(false);
        }
    });

    $("#status").load(function () {
        var valor = $(this).val();

        if (valor) {
            if (valor === '4') {
                HabilitaProtesto(true);
            }
            else {
                HabilitaProtesto(false);
            }
        }
    });


    $('#Numero').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });

    FormataCampoCpf("cpf");

    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
    });
    
    ClienteAutoComplete("clientes", "cliente", "clienteText", callBackLancamentos);

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

        if ($("#cliente").val() === '') {
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

        if ($("#Conta").val() === '') {
            toastr.error("O campo conta deve ser preenchido!", "Agencia inválida!");
            dadosValidos = false;
        }

        if ($("#DigitoConta").val() === '') {
            toastr.error("O campo digito conta deve ser preenchido!", "Digito Conta inválida!");
            dadosValidos = false;
        }

        if (!CPFValido($("#cpf").val())) {
            toastr.error('Preencha um CPF válido!', 'CPF Inválido!');
            dadosValidos = false;
        }

        if ($("#Valor").val() === '') {
            toastr.error("O campo valor deve ser preenchido!", "Valor inválido!");
            dadosValidos = false;
        }

        if ($("#status").val() === '' || $("#status").val() === undefined || $("#status").val() === '0') {
            toastr.error("Selecione um status!", "Status!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    $(window).load(function () {
        var valor = $("#status").val();

        if (valor === '4') {
            HabilitaProtesto(true);
        }
        else {
            HabilitaProtesto(false);
        }
    });

    BuscarChequesRecebidos();
});

function callBackLancamentos() {
    CarregaLancamentos(true);
}

function BuscarChequesRecebidos() {
    BuscarPartialSemFiltro("/ChequeRecebido/BuscarChequesRecebidos", "#lista-cheques")
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

function AlternarDataDevolucao() {
    if ($("#status option:selected").text() == "Devolvido") {
        $("#container-data-devolucao").show();
    } else {
        $("#container-data-devolucao").hide();
        $("#data-devolucao").val("");
    }
}

function AdicionarLancamento() {

    if ($("#lancamento").val() === '' || $("#lancamento").val() === undefined || $("#lancamento").val() === null || $("#lancamento").val() === '0') {
        toastr.error("Selecione um Lançamento de Cobrança!", "Lançamento de Cobrança");
        return false;
    }

    var idLancamento = $("#lancamento").val();

    showLoading();
    $.post("/ChequeRecebido/AdicionarLancamento", { idLancamento: idLancamento })
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
        .fail(() => { })
        .always(() => {
            hideLoading();
        });
}

function RemoverLancamento(idLancamento) {
    showLoading();
    $.post("/ChequeRecebido/RemoverLancamento", { idLancamento: idLancamento })
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

    var id = 0;
    if ($('#idCheque').val() !== '' && $('#idCheque').val() !== undefined && $('#idCheque').val() !== '' && $('#idCheque').val() !== '0') {
        id = parseInt($('#idCheque').val());
    }

    let idCliente = $('#cliente').val();
    if (idCliente)
        populaCombo('#lancamento', 'ChequeRecebido', 'CarregarLancamento', { idCliente: parseInt(idCliente), limpaLancamentoSalvar: limparGrid, idCheque: id });

    if (limparGrid)
        $("#lista-lancamento-result").empty();
}

function HabilitaProtesto(habilita) {
    if (habilita) {
        $("div[name='protestoquadro']").show();
        $("#dataprotesto").val('');
        $("#cartorioprotestado").val('');
    }
    else {
        $("div[name='protestoquadro']").hide();
        $("#dataprotesto").val('');
        $("#cartorioprotestado").val('');
    }
}