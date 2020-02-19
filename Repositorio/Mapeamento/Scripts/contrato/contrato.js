
$(document).ready(function () {
    MakeChosen("carteira", true);
    MakeChosen("credor", true);
    ConfigTabelaGridSemCampoFiltroPrincipal();

    FormatarCampoData("DataVencimento");
    FormatarCampoData("DataAtraso");

    $("#Valor").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#Taxa").maskMoney({
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#ValorParcela").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#pesquisar").click(function () {
        //window.location = "/contrato/pesquisar/?codCarteira=" + $("#carteira option:selected").val();
        window.location = "/contrato/pesquisar/" + $("#carteira option:selected").val();
        //showLoading();
        //setInterval(hideLoading(), 3000);
    });
});

function formData() {
    var form = new FormData();

    form.Id = $("#hdnId").val();
    form.CodigoCarteira = [$("#carteira :selected").val()];
    form.CodigoCredor = [$("#credor :selected").val()];
    form.CodContrato = $("#CodContrato").val();
    form.Plano = $("#Plano").val();
    form.Taxa = $("#Taxa").maskMoney("unmasked")[0];
    form.DataVencimento = $("#DataVencimento").val();
    form.ValorParcela = $("#ValorParcela").maskMoney("unmasked")[0];
    form.DataAtraso = $("#DataAtraso").val();
    form.Filial = $("#Filial").val();
    form.Fase = $("#Fase").val();
    form.Devolvido = $("#Devolvido").val();
    form.CodigoStatusContrato = [$("#statusContrato :selected").val()];

    return form;
}



function MakeChosen(id, single, disableSearch, maxSelectedOptions) {
    if (single === undefined || single === null)
        single = false;

    if (disableSearch === undefined || disableSearch === null)
        disableSearch = false;

    if (maxSelectedOptions === undefined || maxSelectedOptions === null)
        maxSelectedOptions = $("select#" + id + " option").length;

    $("select#" + id).chosen("destroy");

    if (single)
        $("#" + id).removeAttr("multiple");

    $("#" + id).chosen({
        allow_single_deselect: true,
        disable_search: disableSearch,
        no_results_text: "Oops, item não encontrado!",
        max_selected_options: maxSelectedOptions
    }).trigger("chosen:updated");
}