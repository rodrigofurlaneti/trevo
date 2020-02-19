$(document).ready(function () {
    FormatarNumerosDecimais(".decimal");
    FormatarReal("real");
    BuscarPlanoCarreira();

    $(document).on("blur", ".decimal", function () {
        if (this.value.includes(",")) {
            let valorDecimal = parseInt(this.value.split(",").pop());
            if (valorDecimal > 11) {
                this.value = "";
                toastr.warning("Valor do mês não pode ser maior do que 11");
            }
        }
    });

    $("#plano-carreira-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
        if ($("#plano-carreira-valor").val() == "0,00") {
            toastr.warning("O campo Valor é obrigatório", "Alerta");
            return false;
        }
    });
});

function BuscarPlanoCarreira() {
    return get("BuscarPlanoCarreira", "PlanoCarreira")
        .done((response) => {
            $("#lista-plano-carreira").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}