$(document).ready(function () {
    $("#ddlAssessoria").hide();

    $("input[name='ReservaDistribuicao']").each(function () {
        if (this.value === "Distribuir" && this.checked)
            $("#ddlAssessoria").show();
    });

    $("input[name='ReservaDistribuicao']").change(function () {
        $("#divfiltroNasc").hide();
        if (this.value === "Distribuir") {
            $("#ddlAssessoria").show();
        }
    });

});

