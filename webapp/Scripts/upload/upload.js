$(document).ready(function () {
    $(document).on("click", ".browse", function () {
        var file = $(this).parent().parent().parent().find(".file");
        file.trigger("click");
    });

    $(document).on("change", ".file", function () {
        var filename = "";
        for (var i = 0; i < this.files.length; i++)
            filename += this.files[i].name + ", ";

        filename = filename.substring(0, filename.length - 2);
        $(this).parent().find(".form-control").val(filename);
    });

    $("#file-upload").change(function () {
        $("#btn-upload, #cancel").show();
        $("#arquivos").html("");
        $("#arquivos").append(
            "<div class=\"col-md-12\">" +
            "<div class=\"progress-bar progress-bar-striped active\" id=\"progressbar\" role=\"progressbar\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width:0%\"></div>" +
            "</div>" +
            "<div class=\"col-md-12\">" +
            "<p class=\"progress-status\" style=\"text-align: right;font-weight:bold;color:saddlebrown\" id=\"status\"></p>" +
            "</div>" +
            "<div class=\"col-md-12\">" +
            "<p id=\"notify\" style=\"text-align: right;\"></p>" +
            "</div>");
    });

    $("#cancel").on("click", function () {
        $("[id*=file-upload]").val("");
    });

    $("#processar").hide();

    $("#processar").click(function () {
        //var obj = {
        //    DataDe: $("#DataDe").val()
        //};

        showLoading();
        $.ajax({
            url: "/Upload/Processar",
            datatype: "JSON",
            type: "GET",
            //data: { json: JSON.stringify(obj) },
            success: function (response) {
                if (response.Sucesso === true) {
                    $("#processar").hide();
                }
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            },
            complete: function () {
                hideLoading();
            }
        });
    });
});

function uploadArquivo() {
    $("#arquivos").show();
    var file = document.getElementById("file-upload");
    for (var i = 0; i < file.files.length; i++) {
        upload(file.files[i], "/upload/uploadArquivo");
    }
}

function upload(file, url) {
    var fileSize = file.size;
    var ajax = new XMLHttpRequest();

    ajax.upload.addEventListener("progress", function (e) {
        var percent = (e.loaded / fileSize) * 100;
        $("#status").text(Math.round(percent) + "% carregando, por favor aguarde...");
        $("#progressbar").css("width", percent + "%");
        $("#notify").text("Enviado " + (e.loaded / 1048576).toFixed(2) + " MB de " + (e.total / 1048576).toFixed(2) + " MB - Enviando com sucesso!");
    }, false);

    ajax.addEventListener("load", function (e) {
        $("#status").text(event.target.responseText);
        $("#progressbar").css("width", "100%");

        var cancel = $("#cancel");
        cancel.hide();

        $("[id*=file-upload]").val("");
        $("#btn-upload").hide();
        $("#processar").show();


    }, false);

    ajax.addEventListener("error", function (e) {
        $("#status").text("Upload Falhou");
    }, false);

    ajax.addEventListener("abort", function (e) {
        $("#status").text("Upload Cancelado");
    }, false);

    ajax.open("POST", url);

    var uploaderForm = new FormData();
    uploaderForm.append("file", file);
    ajax.send(uploaderForm);

    var cancel = $("#cancel");
    cancel.show();

    cancel.on("click", function () { ajax.abort(); });
}