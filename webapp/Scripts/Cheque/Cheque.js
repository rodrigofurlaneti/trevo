$(document).ready(function () {
    BuscarCheques();
    MakeChosen("contaFinanceira");
    MakeChosen("bancoCheque");

    
    $('#Numero').keyup(function () {
        $(this).val(this.value.replace(/\D/g, ''));
    });

    FormataCampoCpf("cpf");

    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
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

        if ($("#contaFinanceira").val() === '') {
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

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});

function BuscarCheques() {
    $.ajax({
        url: "/cheque/BuscarCheques",
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
                $("#lista-cheques").empty();
                $("#lista-cheques").append(result);
            }
        },
        error: function (error) {
            $("#lista-cheques").empty();
            $("#lista-cheques").append(error.responseText);
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

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return true;
}