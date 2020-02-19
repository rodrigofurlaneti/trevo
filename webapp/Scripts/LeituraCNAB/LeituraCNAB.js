$(document).ready(function () {
    MakeChosen("conta-financeira");
    FormatarCampoData("dtVencimentoInicio");
    FormatarCampoData("dtVencimentoFim");

    $("#btn-upload").click(function () {
        let contaFinanceiraId = $("#conta-financeira").val();
        if (!contaFinanceiraId) {
            toastr.warning("Informe a conta financeira", "Campo Obrigatório");
            return false;
        }
    });

    $("#fileUpCNAB").on('change', function () {
        UploadCNAB(this);
    });
});

function BuscarPartial(url, divId, obj) {
    showLoading();
    return $.post(url, obj)
        .done(function (result) {
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
                $(divId).empty();
                $(divId).append(result);
            }
        })
        .fail(function (error) {
            $(divId).empty();
            $(divId).append(error.responseText);
        }).always(function () {
            hideLoading();
        });
}

function BuscarCNAB() {
    
    let dataInicial = $("#dtVencimentoInicio").val();
    let dataFinal = $("#dtVencimentoFim").val();

    if (validarPeriodo(dataInicial, dataFinal, true) === false) {
        toastr.warning("Os campos de data são obrigatórios e a data final deve ser maior ou igual à data inicial!", "Atenção");
        return;
    }

    var filtro = {
        DataDe: dataInicial,
        DataAte: dataFinal
    };

    BuscarPartial("/leituracnab/BuscarCNAB", "#lista-cnab", { filtro }).done(function () {
        ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_grid_cnab");
        $("#detalhes-cnab").empty();
    });
}

function DetalhesCNAB(id) {
    BuscarPartial("/leituracnab/DetalhesCNAB", "#detalhes-cnab", { idLeituraCNAB: id }).done(function () {
        ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_detalhes_cnab");
    });
}

function UploadCNAB(input) {
    let previa = $("#chkPrevia").prop("checked");
    let contaFinanceiraId = $("#conta-financeira").val();
    var url = $(input).val();
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext === "txt" || ext === "ret")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            SubmitUploadCNAB(e.target.result, input.files[0].name, input, contaFinanceiraId, previa);
        };
        reader.readAsDataURL(input.files[0]);
    }
    else {
        toastr.warning("O tipo de extensão do arquivo [" + ext + "] não é permitido!", "Upload CNAB");
    }
}

function SubmitUploadCNAB(file, filaName, ele, contaFinanceiraId, previa) {
    $.ajax({
        url: "/LeituraCNAB/UploadLeituraCNAB",
        type: "POST",
        dataType: 'json',
        data: { fileBase64: file, fileName: filaName, contaFinanceiraId, previa },
        success: function (result) {
            if (typeof result === "object" && result.TipoModal !== undefined) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#modalBodyRelatorio").empty();
                $("#modalBodyRelatorio").append(result.ModalPreviaLeitura);
                $("#modalRelatorio").modal({ backdrop: 'static', keyboard: false });
            }
            $(ele).val('');
        },
        error: function (error) {
            $(ele).val('');
            hideLoading();
            openCustomModal(null,
                null,
                "danger",
                "Erro",
                error.responseText,
                false,
                null,
                function () { });
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
            ObterNotificacoes();
        }
    });
}

function Cancelar() {
    $("#modalBodyRelatorio").empty();
    $("#modalRelatorio").modal('hide');
}

function Imprimir() {
    var elem = $(".imprimir");

    var mywindow = window.open('', 'PRINT', 'height=640,width=920');

    mywindow.document.write('<html><head><title>' + document.title + '</title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write(elem.html());
    mywindow.document.write('</body></html>');

    mywindow.document.close(); // necessary for IE >= 10
    mywindow.focus(); // necessary for IE >= 10*/

    mywindow.print();
    mywindow.close();

    return true;
}