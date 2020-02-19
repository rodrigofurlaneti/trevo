$(document).ready(function () {
    FormatarCampoDataMesAno("dtReferencia");
    
    $("#BloqueioReferenciaForm").submit(function (e) {
        if (!$("#dtReferencia").val()) {
            toastr.error("O campo \"Mês/Ano\" deve ser preenchido!");
            dadosValidos = false;
        } else if (verificaData("01/" + $("#dtReferencia").val()) === false) {
            toastr.error("O campo \"Mês/Ano\" deve ser preenchido com uma data no formato (Mês/Ano) válida!");
            dadosValidos = false;
        }

        return true;
    });

    BuscarDados();
});

function BuscarDados() {
    $.ajax({
        url: "/BloqueioReferencia/BuscarDados",
        type: "POST",
        dataType: "json",
        success: function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#lista-registros").empty();
                $("#lista-registros").append(result);
                if (result.indexOf("Nenhum") < 0)
                    ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_fixed_column");
            }
        },
        error: function (error) {
            $("#lista-registros").empty();
            $("#lista-registros").append(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}