$(document).ready(function () {
    BuscarContaFinanceiras();
    MakeChosen("banco");
    MakeChosen("empresa", null, "100%");

    FormataCampoCpf("cpf");
    FormataCampoCnpj("cnpj");

    $('#cpf').on('blur', function (ele) {
        VerificarCPF();
    });
    $('#cnpj').on('blur', function (ele) {
        VerificarCNPJ();
    });

    $("#ContaFinanceiraForm").submit(function (e) {

        if (!somenteLetra($("#Descricao").val())) {
            toastr.error("O campo descricao não permite caracteres númericos!", "Descricao inválida!");
            return false;
        }

        if ($("#banco").val() === '') {
            toastr.error("O campo banco deve ser preenchido!", "Banco inválido!");
            return false;
        }

        if (!$("#empresa").val()) {
            toastr.error("O campo Empresa deve ser preenchido!", "Empresa inválida!");
            return false;
        }

        if ($("#Agencia").val() === '') {
            toastr.error("O campo agencia deve ser preenchido!", "Agencia inválida!");
            return false;
        }

        if ($("#Conta").val() === '') {
            toastr.error("O campo conta deve ser preenchido!", "Agencia inválida!");
            return false;
        }

        if ($("#DigitoConta").val() === '') {
            toastr.error("O campo digito conta deve ser preenchido!", "Digito Conta inválida!");
            return false;
        }

        if ($("#cpf").val() === '' && $("#cnpj").val() === '') {
            toastr.error("Preencha CPF ou CNPJ!", "CPF ou CNPJ Inválido!");
            return false;
        }

        if ($("#cpf").val().length > 0) {
            return VerificarCPF();
        }

        return true;
    });
});

function BuscarContaFinanceiras() {
    $.ajax({
        url: "/contaFinanceira/BuscarContaFinanceiras",
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
                $("#lista-contaFinanceiras").empty();
                $("#lista-contaFinanceiras").append(result);
            }
        },
        error: function (error) {
            $("#lista-contaFinanceiras").empty();
            $("#lista-contaFinanceiras").append(error.responseText);
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
function VerificarCNPJ() {
    var cnpj = $("#cnpj").val();

    if (!CnpjValido(cnpj)) {
        toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
        return false;
    }

    return true;
}