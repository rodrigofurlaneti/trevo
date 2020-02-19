$(document).ready(function () {

    $('#empresaOrigem').change(function () {
        debugger;
        var IdEmpresa = $(this).find(':selected').val();
        showLoading();
        $.post("/ConsolidaAjusteFaturamento/BuscarUnidades", { IdEmpresa })

            .done((response) => {
                if (typeof (response) === "object") {
                    CarregaUnidades(response);
                } else {
                    alert("NAOIBJETO");
                }
            })
            .fail(() => { })
            .always(() => { hideLoading(); });
        //$.ajax({
        //    url: '/ConsolidaAjusteFaturamento/BuscarUnidades',
        //    type: 'POST',
        //    dataType: 'json',
        //    data: { IdEmpresa: IdEmpresa },
        //    success: function (response) {
        //        CarregaUnidades(response);
        //        hideLoading();
        //    }
        //});
    });

    $("#consolidaAjusteFaturamentoForm").submit(function (e) {

        if (!ValidarCamposAjusteFinalFaturamento()) return false;

        return true;


    });

    FormatarApresentacaoCampos();
    $("div#divconsolidafaturamento").hide();
    $("div#divconsolidaajustefinalfaturamento").hide();

});

function CarregaUnidades(empresas) {
    debugger;
    var equipeSelect = document.getElementById("unidadeOrigem");
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
    MakeChosen("unidadeOrigem");
    hideLoading();
}

function ValidarCamposAjusteFinalFaturamento() {

    if ($("#AjusteFinalFaturamento").val() === "") {

        toastr.error("Informe \"um valor de ajuste final de faturamento\"");
        return false;
    }

    return true;

}


function ResetarCampos() {

    //$("#unidadeOrigem").val('');
    //$("#equipeOrigem").val('');
    //$("#tipoEquipeOrigem").val('');
    //$("#horarioOrigem").val('');

    //$("#unidadeDestino").val('');
    //$("#equipeDestino").val('');
    //$("#tipoEsquipeDestino").val('');
    //$("#horarioDestino").val('');

    //$("#DataFinal").val('');

    //$("#equipeOrigem").html('<option value="">Selecione a Equipe</option>');
    //$("#tipoEquipeOrigem").html('<option value="">Selecione o Tipo Equipe</option>');
    //$("#horarioOrigem").html('<option value="">Selecione o Horário</option>');

    //$("#equipeDestino").html('<option value="">Selecione a Equipe</option>');
    //$("#tipoEsquipeDestino").html('<option value="">Selecione o Tipo Equipe</option>');
    //$("#horarioDestino").html('<option value="">Selecione o Horário</option>');


    //FormatarApresentacaoCampos();
    
  
}


function FormatarApresentacaoCampos() {

    MakeChosen("unidadeOrigem");
    MakeChosen("despesaMes");
    MakeChosen("despesaAno");
    MakeChosen("empresaOrigem");


    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

 
}


