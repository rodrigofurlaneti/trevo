$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    
    $('#cnpj').on('blur', function (ele) {
        VerificarCNPJ();
    });

});

function myFunction() {
    var x = document.getElementById("cnpj");
    x.value = x.value.toUpperCase();

    if (x.value !== '') {
        if (!CnpjValido(x.value)) {
            toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
            dadosValidos = false;
        }
    }

}

function VerificarCNPJ() {
    var cnpj = $("#cnpj").val();

    if (!CnpjValido(cnpj)) {
        toastr.error("Preencha um CNPJ válido!", "CNPJ Inválido!");
        return false;
    }

    return true;
}