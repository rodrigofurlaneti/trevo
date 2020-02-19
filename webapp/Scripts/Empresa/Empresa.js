$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();

    $('#cnpj').on('blur', function () {
        ValidarCnpj();
    });

    $("#empresa-form").submit(function () {
        return ValidarCampos()
    });
});

const ValidarCnpj = function () {
    if (!CnpjValido($("#cnpj").val())) {
        toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
        return false;
    }

    return true;
}

function ValidarCampos() {
    if (!ValidarCnpj())
        return false;

    if (!$("#razao_social").val()) {
        toastr.error("Preencha o campo Razão Social!", "Campo Obrigatório!");
        return false;
    }

    return true;
}