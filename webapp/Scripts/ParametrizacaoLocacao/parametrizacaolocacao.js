
$(document).ready(function () {
    
    MakeChosen("tipolocacao");
    MakeChosenMult("unidade");

    $("#selecionar").change(function () {
        var valor = "false";
        if ($(this).is(':checked'))
            valor = true;
        else
            valor = false;

        LoadUnidades(valor);
    });

    $("#tipolocacao").change(function () {
        var valor = "false";
        if ($("#selecionar").is(':checked'))
            valor = true;
        else
            valor = false;

        LoadUnidades(valor);
    });

});

function LoadUnidades(valor) {

    $("#unidade").html("").trigger("chosen:updated");

    var tipolocacaoid = $("#tipolocacao").val();

    $.ajax({
        url: '/parametrizacaolocacao/AtualizaUnidades',
        data: { tipolocacaoid },
        dataType: "json",
        type: "POST",
        success: function (data) {
            var equipeSelect = document.getElementById("unidade");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione...";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(data, function (i, item) {
                option = document.createElement("option");
                option.text = item.Text;
                option.value = item.Value;

                if (valor)
                    option.setAttribute("selected", valor);

                equipeSelect.options.add(option);
                equipeSelect.options.add(option);
            });

            $("#unidade").trigger("chosen:updated");

            hideLoading();

        },
        beforeSend: function () {
            showLoading();
        }
    });
}