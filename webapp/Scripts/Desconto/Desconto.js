$(document).ready(function () {

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    BuscarDescontos();

    $("#DescontoForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#Descricao").val() === '') {
            toastr.error("O campo \"Descrição\" deve ser preenchido!", "Descrição Inválida!");
            dadosValidos = false;
        }

        if ($("#tipodesconto").val() === '') {
            toastr.error("O campo \"Tipo Desconto\" deve ser Selecionado!", "Tipo Desconto Inválido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

    });
});

function BuscarDescontos() {
    BuscarPartialSemFiltro("/Desconto/BuscarDescontos", "#lista-desconto")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}