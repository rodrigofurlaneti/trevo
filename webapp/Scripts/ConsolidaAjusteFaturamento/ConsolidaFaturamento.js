function enviarDadosFaturamento() {

    if (!ValidarCamposEnvioFaturamento()) return;

    showLoading();
    debugger;

    var unidade = {
        Id: $("#unidadeOrigem").val()
    };

    var empresaUnidade =
    {
        Id: $("#empresaOrigem").val()
    };

    var despesaValorFinal = document.getElementById('DespesaValorFinal').value;

    var consolidaDespesa = {

        DespesaTotal: $("#DespesaTotal").val(),
        DespesaFixa: $("#DespesaFixa").val(),
        DespesaEscolhida: $("#DespesaEscolhida").val(),
        DespesaEscolhidaFixa: $("#DespesaEscolhidaFixa").val(),
        DespesaValorFinal: despesaValorFinal.replace(".", "")

    };

    var faturamentoFinal = document.getElementById('FaturamentoFinal').value;

    var consolidaFaturamento =
    {
        FaturamentoMes: $("#FaturamentoMes").val(),
        FaturamentoCartao: $("#FaturamentoCartao").val(),
        Diferenca: $("#Diferenca").val(),
        FaturamentoFinal: faturamentoFinal.replace(".", "")
    };


    consolidaAjusteFaturamentoVM = {
        Id: $("#Id").val(),
        Mes: $('#despesaMes :selected').text(),
        Ano: $('#despesaAno :selected').text(),
        ConsolidaDespesa: consolidaDespesa,
        ConsolidaFaturamento: consolidaFaturamento,
        Unidade: unidade,
        Empresa: empresaUnidade
    };


    $.post("/ConsolidaAjusteFaturamento/EnviarDadosFaturamento", { consolidaAjusteFaturamentoVM })
        .done((response) => {
            if (response.tipo === "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {

                debugger;

                $('#unidadeOrigem').attr('disabled', true).trigger("chosen:updated");
                $('#despesaMes').attr('disabled', true).trigger("chosen:updated");
                $('#despesaAno').attr('disabled', true).trigger("chosen:updated");
                $('#empresaOrigem').attr('disabled', true).trigger("chosen:updated");
                $("div#divbotaoconsultar").hide();
                $("div#divconsolidadespesa").hide();
                $("div#divconsolidafaturamento").hide();
              
                var faturamentoFinal1 = faturamentoFinal.replace(".", "");
                var faturamentoFinal2 = faturamentoFinal1.replace(",", ".");

                var despesaValorFinal1 = despesaValorFinal.replace(".", "");
                var despesaValorFinal2 = despesaValorFinal1.replace(",", ".");
         
                var diferenca = Math.abs(faturamentoFinal2 - despesaValorFinal2);

                diferenca = diferenca.toFixed(2);
                diferenca = diferenca.replace(".", ",");

                $('#DespesaFinalAjuste').val(despesaValorFinal);
                $('#FaturamentoFinalAjuste').val(faturamentoFinal);
                $('#DiferencaAjuste').val(diferenca);

                $("div#divconsolidaajustefinalfaturamento").show();

            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });




}

function ValidarCamposEnvioFaturamento() {

    if ($("#FaturamentoFinal").val() === "" || $("#FaturamentoFinal").val() === "0,00") {

        toastr.error("Informe \"o faturamento Final\"");
        return false;
    }

    return true;
}

function RetornarWizard() {
    $('#unidadeOrigem').attr('disabled', false).trigger("chosen:updated");
    $('#despesaMes').attr('disabled', false).trigger("chosen:updated");
    $('#despesaAno').attr('disabled', false).trigger("chosen:updated");
    $('#empresaOrigem').attr('disabled', false).trigger("chosen:updated");  
    $("div#divbotaoconsultar").show();
    $("div#divconsolidadespesa").show();
    $("div#divconsolidafaturamento").hide();
    $("div#divconsolidaajustefinalfaturamento").hide();
}