$(document).ready(function () {


    $("#parametroNumeroNotaFiscalForm").submit(function (e) {

        debugger;

        if (!ValidaCampos()) {

            return false;
        }

        return true;

    });

    FormatarCampos();

});


function FormatarCampos() {

    MakeChosen("Unidade");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });


}

function buscarParametroNFUnidade() {

    debugger;

    showLoading();

    var idUnidade = $("#Unidade").val();
    

    $.post("/ParametroNumeroNotaFiscal/BuscarParametroNFUnidade", { idUnidade })
        .done((response) => {
            if (response.tipo === "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {
                debugger;
                $("#ValorMaximoNota").val(response.valorMaximoNota);
                $("#ValorMaximoNotaDia").val(response.valorMaximoNotaDia);
                $("#hdnParamentroNF").val(response.id);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });

}


function ValidaCampos() {

    if ($("#Unidade").val() === "") {
        toastr.error("Informe a \"Unidade\"");
        return false;
    }


    if ($("#ValorMaximoNota").val() === "") {
        toastr.error("Informe o \"Valor Máximo de Nota Fiscal\"");
        return false;
    }


    if ($("#ValorMaximoNotaDia").val() === "") {
        toastr.error("Informe o \"Valor Máximo de Nota Fiscal do Dia\"");
        return false;
    }

    return true;

}