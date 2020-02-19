$(document).ready(function () {
    FormatarCampoData("dtDataCompra");
});



function Pesquisar() {
    var sigla = $("#txtSigla").val();

    $.ajax({
        url: "/Carteira/PesquisarSigla",
        type: "POST",
        data: { sigla:sigla },
        success: function (result) {

        }
    });


}

