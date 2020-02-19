$(document).ready(function () {

    $("#Separador").prop("disabled", true);
    $("#uploadConfig").hide();
    $("#layoutPosicional").hide();


    $("input[name='FormatoArquivo']").change(function () {
        $("#Separador").prop("disabled", true);
        $("#layoutPosicional").hide();
        if (this.value === "2") {
            $("#Separador").prop("disabled", false);
        }
        if (this.value === "3") {
            $("#layoutPosicional").show();
        }
    });

    $("#PossuiConfig").click(function () {
        $("#uploadConfig").hide();
        if ($(this)[0].checked)
            $("#uploadConfig").show();
    });


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
});

function Etapa1() {
    var retorno = false;
    var formato = $("input[name='FormatoArquivo']:checked").val();
    var retornoLayout = false;
    if ($("#PossuiConfig")[0].checked) {
        retorno = upload("config", $("#file-upload-config"), "/ArquivoImportacao/RecuperarConfig/");
    } else {
        retorno = true;
    }

    if (formato === "3") {
        retornoLayout = upload("layout", $("#file-upload-layout"), "/ArquivoImportacao/LerLayoutPosicional/");
    }

    if (retorno) {
        retorno = upload("arquivo", $("#file-upload-importacao"), "/ArquivoImportacao/RecuperarArquivo/?formato=" + formato);

        if (retorno) {
            var model = {
                CarteiraSelecionada: $("#SiglaProduto").val(),
                FormatoArquivo: formato,
                PossuiConfig: $("#PossuiConfig")[0].checked,
                Separador: $("#Separador").val()
        };

            $.ajax({
                url: "/ArquivoImportacao/Etapa1",
                type: "POST",
                data: { model: model },
                success: function (result) {
                    if (!result.Sucesso)
                        openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
                    window.location.href = "/ArquivoImportacao/AssociacaoArquivo/";
                }
            });
        }
    }
}


function upload(tipo, file, url) {
    var retorno = false;
    var form = new FormData();
    form.append("file", file[0].files[0]); // para apenas 1 arquivo
    //var name = file.files[0].content.name; // para capturar o nome do arquivo com sua extenção

    $.ajax({
        url: url, // Url do lado server que vai receber o arquivo
        data: form,
        async: false,
        processData: false,
        contentType: false,
        type: "POST",
        success: function (result) {
            if (result.Sucesso) {
                retorno = true;
            } else {
                openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
            }
        }
    });

    return retorno;
}