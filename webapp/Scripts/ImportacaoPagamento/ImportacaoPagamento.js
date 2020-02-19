$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
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
    var formato = $('#FormatoArquivo').val();
    var formatoCarga = 0;
    var retornoLayout = false;

    if ($('#ParcSaldoUnico')[0].checked)
        formatoCarga = 1;

    if ($('#ParcParcela')[0].checked)
        formatoCarga = 2;

    if ($('#ParcIncremental')[0].checked)
        formatoCarga = 3;

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
                ProdutoSelecionado: $("#ProdutoSelecionado").val(),
                PossuiConfig: $("#PossuiConfig")[0].checked,
                Separador: $("#separador").val(),
                FormatoArquivo: formato,
                FormatoCarga: formatoCarga
            };

            $.ajax({
                url: "/ArquivoImportacao/Etapa1",
                type: "POST",
                data: { model: model },
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
                    } else {
                        window.location.href = "/ArquivoImportacao/AssociacaoArquivo/";
                    }
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

function BuscarArquivos() {
    var cedente = $("#ddlListaCedentes option:selected").val();
    
    if (typeof cedente == 'undefined') { cedente = 0; }
    
    $.ajax({
        url: "/ImportacaoPagamento/BuscarArquivos",
        type: "POST",
        dataType: "json",
        data: {
            cedente: cedente,
        },
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
                $("#lista-arquivos-pagamento").empty();
                $("#lista-arquivos-pagamento").append(result);
            }
        },
        error: function (error) {
            $("#lista-arquivos-pagamento").empty();
            $("#lista-arquivos-pagamento").append(error.responseText);
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
