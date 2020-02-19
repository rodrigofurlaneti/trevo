$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();

    $.ajax({
        type: 'POST',
        url: '/Modelo/CarregarMarcaes',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#marca").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
            var identify = $('#IdMarca').val();
            if (identify !== '') {
                $('#marca').val($('#IdMarca').val());
            }
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });

    $("#marca").change(function () {
        $('#IdMarca').val($('#marca').val());
    });
    
    $("#ModeloForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#Descricao").val() === "") {
            toastr.error("Preencha a descrição!", "Descrição Inválida!");
            dadosValidos = false;
        }

        if ($("#Sigla").val() === "") {
            toastr.error("Preencha a Sigla!", "Sigla Inválida!");
            dadosValidos = false;
        }

        if ($("#marca").val() === "0") {
            toastr.error("Selecione um Marca!", "Marca Inválido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

        return true;
    });
});
