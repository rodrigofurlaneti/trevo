$(document).ready(function () {

    FormatarCampos();

    $("#PrecoParametroSeloForm").submit(function (e) {

        if (!ValidarCampos()) {

            return false;
        }

        return true;

    });

	buscarLista();

	$("#TodasUnidades").click(function (e) {
		if($(this).is(":checked"))
		{
			$("#Unidade").prop("disabled", "disabled").trigger("chosen:updated");;
		}
		else
		{
			$("#Unidade").prop("disabled", false).trigger("chosen:updated");;
		}
	});

});

function buscarLista() {
    BuscarPartialSemFiltro("/PrecoParametroSelo/BuscarLista", "#lista-preco-parametro-selo")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}


function ValidarCampos() {


	if ($("#Unidade").val() === "" && !$("#TodasUnidades").is(":checked")) {
        toastr.error("Informe a \"Unidade\"");
        return false;
    }
    

    if ($("#TipoPreco").val() === "") {
        toastr.error("Informe o \"Tipo de selo\"");
        return false;
    }


    if ($("#DescontoTabelaPreco").val() === "") {
        toastr.error("Informe o \"Desconto aplicado a tabela de preço\"");
        return false;
    }


    if ($("#TipoNegociacao").val() === "") {
        toastr.error("Informe o \"Tipo de desconto de negociação\"");
        return false;
    }


    if ($("#DescontoMaximoValor").val() === "") {
        toastr.error("Informe o \"Desconto máximo do valor final\"");
        return false;
    }

    if ($("#Perfil").val() === "") {
        toastr.error("Informe o \"Perfil\"");
        return false;
    }



    return true;
}



function FormatarCampos() {

    MakeChosen("Unidade");
    MakeChosen("TabelaPreco");
    MakeChosen("TipoPreco");
    MakeChosen("TipoNegociacao");
    MakeChosen("Perfil");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });


}