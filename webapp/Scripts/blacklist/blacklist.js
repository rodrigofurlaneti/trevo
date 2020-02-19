$(document).ready(function () {
    FormatarCampoData("DataDe");
    FormatarCampoData("DataAte");

    ConfigTabelaGridSemCampoFiltroPrincipal();

    $("#pesquisar").click(function () {
        var obj = {
            DataDe: $("#DataDe").val(),
            DataAte: $("#DataAte").val(),
            CodigoCarteira: $("#CodigoCarteira").val(),
            CodigoAssessoria: $("#CodigoAssessoria").val(),
            CodigoMotivo: $("#CodigoMotivo").val(),
            Cpf: $("#Cpf").val(),
            Inclusao: $("#Inclusao").is(":checked"),
            Exclusao: $("#Exclusao").is(":checked")
        };

        showLoading();
        $.ajax({
            url: "/Blacklist/Pesquisar",
            datatype: "JSON",
            type: "GET",
            data: { json: JSON.stringify(obj) },
            //data: { json: obj },
            success: function (response) {
                if (typeof (response) === "object") {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                }
                else {
                    $("#grid-result").empty();
                    $("#grid-result").append(response);
                    $("#grid-result table").dataTable();
                }
            }
        });
        hideLoading();
    });
});

function exportar() {
    showLoading();

    var link = document.createElement("a");
    link.href = "/Blacklist/Exportar";
    link.click();

    hideLoading();
}