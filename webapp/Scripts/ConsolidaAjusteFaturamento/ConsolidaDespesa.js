function ValidarCamposEnvioDespesa() {

    if ($("#DespesaTotal").val() === "") {

        toastr.error("Informe \"uma despesa total\"");
        return false;
    }


    if ($("#DespesaValorFinal").val() === "") {

        toastr.error("Informe \"um valor de despesa final\"");
        return false;
    }

    return true;
}


function enviarDadosDespesa() {

    if (!ValidarCamposEnvioDespesa()) return;

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
        DespesaValorFinal: despesaValorFinal.replace(".","")

    };


    consolidaAjusteFaturamentoVM = {
        Id: $("#Id").val(),
        Mes: $('#despesaMes :selected').text(),
        Ano: $('#despesaAno :selected').text(),
        ConsolidaDespesa : consolidaDespesa,
        Unidade: unidade,
        Empresa: empresaUnidade
    };


    $.post("/ConsolidaAjusteFaturamento/EnviarDadosDespesa", { consolidaAjusteFaturamentoVM })
        .done((response) => {
            if (response.tipo === "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {
                debugger;
              
                $('#unidadeOrigem').attr('disabled', true).trigger("chosen:updated");

                $.ajax({
                    url: '/ConsolidaAjusteFaturamento/BuscarDadosFaturamento',
                    type: 'POST',
                    data: { consolidaAjusteFaturamentoVM: consolidaAjusteFaturamentoVM },
                    dataType: 'json',
                    success: function (response) {
                        if (response.tipo === "danger") {
                            toastr.error(response.message);
                            hideLoading();
                            return;
                        } else {

                            $("#FaturamentoMes").val(response.faturamentoMeslF);
                            $("#FaturamentoCartao").val(response.faturamentoCartaoF);
                            $("#Diferenca").val(response.diferencaF);
                            $("#FaturamentoFinal").val(response.faturamentoFinalF);
                        }
                    }
                });

                $('#despesaMes').attr('disabled', true).trigger("chosen:updated");
                $('#despesaAno').attr('disabled', true).trigger("chosen:updated");
                $('#empresaOrigem').attr('disabled', true).trigger("chosen:updated");
                $("div#divbotaoconsultar").hide();
                $("div#divconsolidadespesa").hide();
                $("div#divconsolidafaturamento").show();
 
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });


    

}

function ValidarCamposDespesa() {



    if ($("#empresaOrigem").val() === "") {

        toastr.error("Selecione \"uma empresa\"");
        return false;
    }

    if ($("#despesaMes").val() === "") {

        toastr.error("Selecione \"um mês\"");
        return false;
    }

    if ($("#despesaAno").val() === "") {

        toastr.error("Selecione \"um ano\"");
        return false;
    }

    return true;

}





function buscarDadosDespesa() {

    debugger;

    $("#DespesaValorFinal").val('');

    if (!ValidarCamposDespesa()) return;

    showLoading();

    var unidade = {
        Id: $("#unidadeOrigem").val()
    };

    var empresaUnidade = {
        Id: $("#empresaOrigem").val()
    };


    consolidaAjusteFaturamentoVM = {
        Mes: $('#despesaMes :selected').text(),
        Ano: $('#despesaAno :selected').text(),
        Unidade: unidade,
        Empresa: empresaUnidade
    };


    $.post("/ConsolidaAjusteFaturamento/BuscarDadosDespesa", { consolidaAjusteFaturamentoVM })
        .done((response) => {
            if (response.tipo === "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {

                $("#DespesaTotal").val(response.despesaTotalF);
                $("#DespesaFixa").val(response.despesaFixaF);
                $("#DespesaEscolhida").val(response.despesaEscolhidaF);
                $("#DespesaEscolhidaFixa").val(response.despesaEscolhidaFixaF);
                $("#DespesaValorFinal").val(response.despesaValorFinalF);
                $("#Id").val(response.id);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}