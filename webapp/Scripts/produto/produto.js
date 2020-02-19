$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();

    $.ajax({
        type: 'POST',
        url: '/Produto/CarregarCredores',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#Credor").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
            var identify = $('#IdCredor').val();
            if (identify !== '') {
                $('#Credor').val($('#IdCredor').val());
            }
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });

    $("#Credor").change(function () {
        $('#IdCredor').val($('#Credor').val());
    });
    
    $("#ProdutoForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#Descricao").val() === "") {
            toastr.error("Preencha a descrição!", "Descrição Inválida!");
            dadosValidos = false;
        }

        if ($("#Sigla").val() === "") {
            toastr.error("Preencha a Sigla!", "Sigla Inválida!");
            dadosValidos = false;
        }

        if ($("#Credor").val() === "0") {
            toastr.error("Selecione um Credor!", "Credor Inválido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }

        return true;
    });
});
