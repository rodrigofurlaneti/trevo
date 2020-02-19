$(document).ready(function () {
    MakeChosen("ParametroSelo");

    $('#Ativo').prop('checked', true);
    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    BuscarTipoSelos();

    $('#ParametroSelo').change(function () {
        if ($("#ParametroSelo").val() == 1 || $("#ParametroSelo").val() == 5) {
            $("#Valor").attr('disabled', false);
        }
        else
            $("#Valor").attr('disabled', true);
    });

    $("#TipoSeloForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#Nome").val() === '') {
            toastr.error("O campo \"Nome do Selo\" deve ser preenchido!", "Nome do Selo Inválido!");
            dadosValidos = false;
        }

        if ($("#ParametroSelo").val() === '') {
            toastr.error("O campo \"Parâmetro Selo\" deve ser Selecionado!", "Parâmetro Selo Inválido!");
            dadosValidos = false;
        }

        if (!$("#Valor").val() || $("#Valor").val() === '0,00') {
            if ($("#ParametroSelo").val() == 1) {
                toastr.error("O campo \"Valor Cobrança Selo\" deve ser maior que 0!", "Valor Selo Inválido!");
                dadosValidos = false;
            }
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});

function BuscarTipoSelos() {
    BuscarPartialSemFiltro("/TipoSelo/BuscarTipoSelos", "#lista-tipo-selo")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}