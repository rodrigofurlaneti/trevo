$(document).ready(function () {
    FormatarCampoData("dtInicioFiltro");
    FormatarCampoData("dtFimFiltro");

    MakeChosen("tipoRelatorioFinanceiroFiltro", null, "95%");
    MakeChosen("unidadeFiltro", null, "95%");
    MakeChosen("tipoServicoFiltro", null, "95%");

    $("#tipoRelatorioFinanceiroFiltro").change(function () {
        limpaCombo('#tipoServicoFiltro');
        populaCombo('#tipoServicoFiltro', 'relatoriosFinanceiros', 'FiltroTipoServico', { tipoRelatorio: parseInt($(this).val()) }, 0);
    });

    $("#tipoServicoFiltro").change(function () {
        if ($(this).val() === 3
            || $(this).val() === 4
            || $(this).val() === 5
            || $(this).val() === 6
            || $(this).val() === 10) {
            $("#unidadeFiltro").val("");
            $("#unidadeFiltro").css("disabled", "disabled");
        }
        else {
            $("#unidadeFiltro").removeAttr("disabled");
        }
    });

});

function GerarRelatorio() {
    var tipoRelatorioFinanceiroFiltro = $("#tipoRelatorioFinanceiroFiltro").val();

    var unidadeFiltro = $("#unidadeFiltro").val();

    var tipoServicoFiltro = $("#tipoServicoFiltro").val();

    var dadosValidos = true;

    var dataInicio = $("#dtInicioFiltro").val();
    var dataFim = $("#dtFimFiltro").val();

    if (!$("#dtInicioFiltro").val()) {
        toastr.error("O campo \"Data DE\" deve ser preenchido!");
        dadosValidos = false;
    } else if (verificaData($("#dtInicioFiltro").val()) === false) {
        toastr.error("O campo \"Data DE\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }

    if (!$("#dtFimFiltro").val()) {
        toastr.error("O campo \"Data ATÉ\" deve ser preenchido!");
        dadosValidos = false;
    } else if (verificaData($("#dtFimFiltro").val()) === false) {
        toastr.error("O campo \"Data ATÉ\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }

    var dataParseInicio = new Date(dataInicio.split("/")[1] + '-' + dataInicio.split("/")[0] + '-' + dataInicio.split("/")[2]);
    var dataParseFim = new Date(dataFim.split("/")[1] + '-' + dataFim.split("/")[0] + '-' + dataFim.split("/")[2]);

    if (dataParseInicio > dataParseFim) {
        toastr.error("O campo \"Data DE\" deve ser menor que o campo \"Data ATÉ\"!");
        dadosValidos = false;
    }

    if (!dadosValidos) {
        e.preventDefault();
        return false;
    }

    if (tipoRelatorioFinanceiroFiltro === "1"
        || tipoRelatorioFinanceiroFiltro === "2"
        || tipoRelatorioFinanceiroFiltro === "7"
        || tipoRelatorioFinanceiroFiltro === "8"
        || tipoRelatorioFinanceiroFiltro === "9"
        || tipoRelatorioFinanceiroFiltro === "10")
        RelatorioModal(tipoRelatorioFinanceiroFiltro, dataInicio, dataFim, unidadeFiltro, tipoServicoFiltro);
    else
        RelatorioDownload(tipoRelatorioFinanceiroFiltro, dataInicio, dataFim, unidadeFiltro, tipoServicoFiltro);
}

function RelatorioModal(tipoRelatorio, dataInicio, dataFim, unidade, tipoServico) {
    var filtro = {
        dataInicio: dataInicio,
        dataFim: dataFim,
        tipoRelatorio: tipoRelatorio,
        tipoServico: tipoServico,
        unidade: unidade
    };

    $.ajax({
        url: "/relatoriosFinanceiros/RelatorioModal",
        type: "POST",
        dataType: "json",
        data: filtro,
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
            }
            else {
                $("#modalBodyRelatorio").empty();
                $("#modalBodyRelatorio").append(result.Dados);
                $("#modalRelatorio").modal({ backdrop: 'static', keyboard: false });
            }
        },
        error: function (error) {
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
        }
    });
}

function RelatorioDownload(tipoRelatorio, dataInicio, dataFim, unidade, tipoServico) {
    var filtro = {
        dataInicio: dataInicio,
        dataFim: dataFim,
        tipoRelatorio: tipoRelatorio,
        tipoServico: tipoServico,
        unidade: unidade
    };

    $.ajax({
        cache: false,
        url: '/RelatoriosFinanceiros/RelatorioDownload',
        type: "POST",
        dataType: "json",
        data: filtro,
        success: function (data) {
            window.location = '/RelatoriosFinanceiros/Download?fileGuid=' + data.FileGuid + '&filename=' + data.FileName;
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