$(document).ready(function () {

    $('#EmpresaFechamento').change(function () {
        var IdEmpresa = $(this).find(':selected').val();
        showLoading();
        $.post("/CalculoFechamento/BuscarUnidades", { IdEmpresa })

            .done((response) => {
                if (typeof (response) === "object") {
                    CarregaUnidades(response);
                }
            })
            .fail(() => { })
            .always(() => { hideLoading(); });
    });

    $("#calculoFechamentoForm").submit(function (e) {


    });


    FormatarApresentacaoCampos();
});
function FormatarApresentacaoCampos() {

    MakeChosen("EmpresaFechamento");
    MakeChosen("MesFechamento");
    MakeChosen("AnoFechamento");
    MakeChosen("UnidadeFechamento");


    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });


}

function CarregaUnidades(empresas) {
    var equipeSelect = document.getElementById("UnidadeFechamento");
    equipeSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione uma unidade";
    option.value = 0;
    equipeSelect.options.add(option);
    $.each(empresas, function (i, item) {
        option = document.createElement("option");
        option.text = item.Nome;
        option.value = item.Id;
        equipeSelect.options.add(option);
    });
    MakeChosen("UnidadeFechamento");
    hideLoading();
}


function InformarValorComplementar() {
    if ($("#ValorNotaEmissao").val() === "") {
        toastr.error("Insirá a Pesquisa de Fechamento!");
    }
    else {
        $('#ValorComplementarEmitido').attr('checked', true);


        $.post("/CalculoFechamento/AtualizarValorComplementar")
            .always(() => { hideLoading(); });

    }
    

}

function buscarDadosFechamento() {

    showLoading();

    var unidade = {
        Id: $("#UnidadeFechamento").val()
    };

    var empresaUnidade = {
        Id: $("#EmpresaFechamento").val()
    };


    var consolidaAjusteFaturamentoVM = {
        Mes: $('#MesFechamento :selected').text(),
        Ano: $('#AnoFechamento :selected').text(),
        Unidade: unidade,
        Empresa: empresaUnidade
    };


    $.post("/CalculoFechamento/BuscarAjusteFaturamento", { consolidaAjusteFaturamentoVM })
        .done((response) => {
            if (response.tipo === "danger") {
                toastr.error(response.message);
                hideLoading();
                $("#DespesaTotal").val("");
                $("#DespesaValorFinal").val("");
                $("#FaturamentoMes").val("");
                $("#FaturamentoFinal").val("");
                $("#ValorNotaEmissao").val("");
                $('#PrefeituraComplementarMaiorIgualDespesa').attr('checked', false);
                $('#ValorComplementarEmitido').attr('checked', false);
                $('#PrefeituraMaiorIgualCartao').attr('checked', false);
                return;
            } else {
                $("#DespesaTotal").val(response.despesaTotalF);
                $("#DespesaValorFinal").val(response.despesaValorFinalF);
                $("#FaturamentoMes").val(response.faturamentoMesF);
                $("#FaturamentoFinal").val(response.faturamentoFinalF);
                $("#ValorNotaEmissao").val(response.valorNotaEmissaoF);


                if (response.prefeituraMaiorIgualCartaoF === true) {
                    $('#PrefeituraMaiorIgualCartao').attr('checked', true);
                }

                if (response.prefeituraComplementarMaiorIgualDespesaF === true) {
                    $('#PrefeituraComplementarMaiorIgualDespesa').attr('checked', true);
                }
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}