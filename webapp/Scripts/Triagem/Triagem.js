$(document).ready(function () {
    $("#divfiltroNasc").hide();
    $("#divfiltroEnd").hide();

    $("input[name='FiltroNasc']").each(function () {
        if (this.value === "ComFiltroNasc" && this.checked)
            $("#divfiltroNasc").show();
    });

    $("input[name='FiltroEnd']").each(function () {
        if (this.value === "ComFiltroEnd" && this.checked)
            $("#divfiltroEnd").show();
    });


    $("input[name='FiltroNasc']").change(function () {
        $("#divfiltroNasc").hide();
        if (this.value === "ComFiltroNasc") {
            $("#divfiltroNasc").show();
        }
    });

    $("input[name='FiltroEnd']").change(function () {
        $("#divfiltroEnd").hide();
        if (this.value === "ComFiltroEnd") {
            $("#divfiltroEnd").show();
        }
    });
});

function Divida() {
    window.location.href = "/Triagem/Divida/";
}

function GridDevedor() {
    var url = "/Triagem/GridDevedores/";
    var win = window.open(url, '_blank');
    win.focus();
}

