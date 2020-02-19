$(document).ready(function () {

    ConfigTabelaGridSemCampoFiltroPrincipal();

    FormatarCampoData("DataDe");
    FormatarCampoData("DataAte");

    $("#alterarStatus").click(function () {

        var idArquivo = $("#hdnId").val();
        var novoStatus = $("#novoStatus option:selected").val();

        $.ajax({
            url: "/ArquivoImportacao/AlterarStatus",
            type: "POST",
            data: { id: idArquivo, status: novoStatus },
            success: function (result) {
                if (result.Sucesso) {
                    var newclass = "fa fa-circle";
                    var idNameStatus = "status" + idArquivo;
                    var idNameSinal = "sinal" + idArquivo;
                    if (novoStatus == "1") {
                        $("#" + idNameStatus).text("Processando");
                        $("#" + idNameSinal).removeClass();
                        $("#" + idNameSinal).addClass(newclass + " black");
                    }

                    if (novoStatus == "2") {
                        $("#" + idNameStatus).text("Ativo");
                        $("#" + idNameSinal).removeClass();
                        $("#" + idNameSinal).addClass(newclass + " green");
                    }

                    if (novoStatus == "3") {
                        $("#" + idNameStatus).text("Inativo");
                        $("#" + idNameSinal).removeClass();
                        $("#" + idNameSinal).addClass(newclass + " yellow");
                    }

                    if (novoStatus == "4") {
                        $("#" + idNameStatus).text("Cancelado");
                        $("#" + idNameSinal).removeClass();
                        $("#" + idNameSinal).addClass(newclass + " red");
                    }

                }
                $("#myModalStatus").modal("hide");
                openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
            }
        });
    });
});

function editarStatus(idArquivo, status) {
    $("#hdnId").val(idArquivo);
    $("#novoStatus").val(status);
    $("#myModalStatus").modal();
}