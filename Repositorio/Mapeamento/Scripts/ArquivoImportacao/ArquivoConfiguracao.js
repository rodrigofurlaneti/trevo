$(document).ready(function () {
    $("#btnGravar").hide();
    $("#divProcessando").hide();
});

function ModalHtml(tipo, titulo, mensagem, textoBotao, acaoBotao) {
    $("#modal-content").removeClass("panel-success");
    $("#modal-content").removeClass("panel-danger");
    $("#modal-content").removeClass("panel-warning");
    $("#modal-content").removeClass("panel-info");

    $("#modal-content").addClass("panel-" + tipo);
    $("#modalToHtmlTitle").html(titulo);
    $("#modalToHtmlBody").html(mensagem);
    $("#btnModalToHtmlAction").html(textoBotao);
    $("#btnModalToHtmlAction").val(textoBotao);
    $("#btnModalToHtmlAction").on("click", acaoBotao);

    $("#modalToHtml").modal({
        backdrop: "static",
        keyboard: false
    });
}


function Etapa3() {
    $("#btnImportar").hide();
    $("#btnDownLoad").hide();
    $("#btnInconsistencia").hide();

    $("#divProcessando").show();
    $("#iProcessOk").hide();
    $("#iProcessErro").hide();

    $("#iLeituraOk").hide();
    $("#iLeituraErro").hide();

    $("#iDadosOk").hide();
    $("#iDadosErro").hide();

    $("#iRegOk").hide();
    $("#iRegErro").hide();

    $("#iConcOk").hide();
    $("#iConcErro").hide();
    
    $.ajax({
        url: "/ArquivoImportacao/Etapa3",
        type: "POST",
        data: {  },
        success: function (result) {
            if (!result.Sucesso) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
                $("#iProcessErro").show();
                $("#iLeituraErro").show();
                $("#iDadosErro").show();
            } else {
                $("#iLeituraOk").show();
                $("#iDadosOk").show();
                $.ajax({
                    url: "/ArquivoImportacao/ValidarCampos",
                    type: "POST",
                    data: {},
                    success: function (result) {
                        if (!result.Sucesso) {
                            openCustomModal(null,
                                null,
                                result.TipoModal,
                                result.Titulo,
                                result.Mensagem,
                                false,
                                null,
                                function () { });
                            $("#iProcessErro").show();
                            $("#iRegErro").show();
                            $("#iConcErro").show();
                            $("#btnGravar").hide();
                            $("#iProcess").hide();
                        } else {
                            $("#iRegOk").show();
                            $("#iConcOk").show();
                            $("#btnGravar").show();
                            $("#iProcess").hide();
                            $("#iProcessOk").show();
                            if (result.Mensagem !== "" && result.Mensagem !== "0") {
                                $("#lblInconsistente")
                                    .html("<label id='lblInconsistente'><i class='glyphicon glyphicon-alert'></i> " +
                                        result.Mensagem +
                                        " Registros inconsistentes</label>");
                                $("#btnInconsistencia").show();
                            } else {
                                $("#lblInconsistente")
                                    .html("<label id='lblInconsistente'><i class='glyphicon glyphicon-ok'></i> Nenhum Registro inconsistente</label>");
                            }
                        }
                    }
                });
            }
        }
    });
}

function SalvarDados() {
    $.ajax({
        url: "/ArquivoImportacao/SalvarDados",
        type: "POST",
        data: {  },
        success: function (result) {
            if (!result.Sucesso)
                openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
            //window.location.href = "/ArquivoImportacao/DownLoadArquivoConfig/";
        }
    });

}







