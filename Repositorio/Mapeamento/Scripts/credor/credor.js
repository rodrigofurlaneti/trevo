$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    ResetCarteirasChosen();

    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
});

function MakeChosen(id, maxSelectedOptions) {
    if (maxSelectedOptions === undefined || maxSelectedOptions === null)
        maxSelectedOptions = $("select#" + id + " option").length;

    $("select#" + id).chosen("destroy");
    $("#" + id).chosen({
        allow_single_deselect: true,
        no_results_text: "Oops, item não encontrado!",
        width: "700px",
        max_selected_options: maxSelectedOptions
    }).trigger("chosen:updated");
}

function GetCarteirasByFuncao(ele) {
    $("#carteiras").empty();
    $("#carteiras").trigger("chosen:update");
    $("#carteiras").val("").trigger("chosen:updated");

    $.ajax({
        url: "/credor/Carteiras",
        type: "POST",
        dataType: "json",
        success: function (carteiras) {
            hideLoading();

            $("#lblMensagemCarteiraResultado").text("");

            if (carteiras !== undefined && carteiras !== null && carteiras.length > 0) {
                $(carteiras).each(function () {
                    $("<option />", {
                        val: this.Id,
                        text: this.RazaoSocial
                    }).appendTo("#carteiras");
                });
                ResetCarteirasChosen();

            } else {
                $("#lblMensagemCarteiraResultado").text("Não possui carteiras disponíveis!");
            }
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ResetCarteirasChosen() {
    //Entidade.Uteis.Constantes.Funcao [Enum]
    var optionsCarteira = $("#carteiras > option").length;
    var lojaLength = optionsCarteira === undefined || optionsCarteira === null ? 0 : optionsCarteira;
    var maxSelectedOptions = (lojaLength);
    MakeChosen("carteiras", maxSelectedOptions);

    $("#carteiras").off("change");
    $("#carteiras").change(function (event) {
        var obj = [];
        $("select#carteiras option:selected").each(function () {
            obj.push({ Id: $(this).val(), CarteiraProduto: $(this).text() });
        });

        $.ajax({
            url: "/credor/carteirasSelecionadas",
            type: "POST",
            dataType: "json",
            data: { json: JSON.stringify(obj) },
            success: function (response) {
                hideLoading();
            },
            error: function (error) {
                hideLoading();
                //alert(JSON.stringify(error));
            },
            beforeSend: function () {
                showLoading();
            }
        });
    });
}