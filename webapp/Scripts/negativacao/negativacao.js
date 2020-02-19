$(document).ready(function () {
    FormatarCampoData("DataDe");
    FormatarCampoData("DataAte");

    $("#pesquisar").click(function () {
        var obj = {
            DataDe: $("#DataDe").val(),
            DataAte: $("#DataAte").val(),
            CodigoDestino: $("#CodigoDestino").val(),
            CodigoSituacao: $("#CodigoSituacao").val()
        };

        showLoading();
        $.ajax({
            url: "/Negativacao/Pesquisar",
            datatype: "JSON",
            type: "GET",
            data: { json: JSON.stringify(obj) },
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
            },
            complete: function () {
                hideLoading();
            }
        });
    });
});

function showModal(id) {
    $("#hId").val(id);
    $("#modalCartorio").modal();
}

function alterarStatusCartorio() {
    var id = $("#hId").val();
    var status = $("#ddlSituacao").val();
    var statusText = $("#ddlSituacao :selected").text();

    $("#modalCartorio").modal("hide");
    showLoading();
    $.ajax({
        url: "/Negativacao/AlterarStatus",
        datatype: "JSON",
        type: "GET",
        data: { idArquivo: id, status: status },
        success: function (response) {
            if (typeof (response) === "object") {
                if (response.Sucesso === true) {
                    $("#situacao" + id).text(statusText);
                }
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
        },
        complete: function () {
            hideLoading();
        }
    });
}