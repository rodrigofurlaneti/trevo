$(document).ready(function () {
    FormatarCampoData("PeriodoInicio");
    FormatarCampoData("PeriodoFim");

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

    $("#Leiaute").change(function () {
        pesquisar();
    });

    $("#LeiauteRelatorio").change(function () {
        BuscaAssessorias($("#LeiauteRelatorio").val());
    });

    $("#ddlAssessoria").change(function () {
        BuscaLotes($("#LeiauteRelatorio").val(), $("#ddlAssessoria").val());
    });

    
    $("#IntegracaoForm").submit(function (e) {
    });

});

function pesquisar() {
    $.ajax({
        url: "/integracao/Pesquisar",
        datatype: "JSON",
        type: "POST",
        data: {
            leiaute: $("#Leiaute").val()
        },
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
                if (response.length > 3903) { $("#GerarExel").val(1); } else { $("#GerarExel").val(0); }
                $("#iteracoes-grid").empty();
                $("#iteracoes-grid").append(response);
            }
            hideLoading();
        },
        error: function (error) {
            debugger;
            hideLoading();
            console.log(error);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function exportar() {

    var dadosValidos = true;

     if ($("#Leiaute").val() === "0") {
        toastr.error('É obrigatório informar o Leiaute!', 'Leiaute!');
        dadosValidos = false;
    }

    if ($("#GerarExel").val() === "0") {
        toastr.error('Não existem linhas para serem exportadas para esse Leiaute!', 'Exportação!');
        dadosValidos = false;
    }

    if (!dadosValidos) {
        e.preventDefault();
        hideLoading();
        return false;
    }
    
    showLoading();
    window.open("/integracao/ExportToExcel/?leiaute=" + $("#Leiaute").val());
    hideLoading();
}

function Relatorio() {
    var dadosValidos = true;

    if ($("#PeriodoInicio").val() === "") {
        toastr.error('É obrigatório informar o Período Inicial!', 'Período!');
        dadosValidos = false;
    }

    if ($("#PeriodoFim").val() === "") {
        toastr.error('É obrigatório informar o Período Final!', 'Período!');
        dadosValidos = false;
    }

    if ($("#PeriodoInicio").val() !== "" && $("#PeriodoFim").val() !== "") {
        if ($("#PeriodoInicio").val() > $("#PeriodoFim").val()) {
            toastr.error('Período Inicial não pode ser maior que Período Final!', 'Período!');
            dadosValidos = false;
        }
    }

    if ($("#LeiauteRelatorio").val() === "0") {
        toastr.error('É obrigatório informar o Leiaute!', 'Leiaute!');
        dadosValidos = false;
    }

    if ($("#ddlAssessoria").val() === "0") {
        toastr.error('É obrigatório informar a Assessoria!', 'Assessoria!');
        dadosValidos = false;
    }

    if (!dadosValidos) {
        e.preventDefault();
        hideLoading();
        return false;
    }

    var periodoInicio = AjustaData($("#PeriodoInicio").val());
    var periodoFim = AjustaData($("#PeriodoFim").val());
    var leiaute = $("#LeiauteRelatorio").val();
    var assessoria = $("#ddlAssessoria").val();
    var lote = $("#ddlLote").val();

    if (leiaute === "1") {
        showLoading();
        window.open("/integracao/ReportOcorrenciasToExcel/?periodoInicio=" + periodoInicio + "&periodoFim=" + periodoFim + "&leiaute=" + leiaute + "&assessoria=" + assessoria + "&lote=" + lote);
        hideLoading();
    }
}

function AjustaData(data) {
    var _data = data.split("/");
    var newDate = _data[1] + "/" + _data[0] + "/" + _data[2];
    return newDate;
}

function upload() {

    var dadosValidos = true;
    var extension = getExtensaoArquivo($("#file-upload").val());

    if ($("#file-upload").val() !== "") {
        if (extension.toUpperCase() !== "TXT") {
            toastr.error('Extensão de arquivo não permitida!', 'Arquivo!');
            dadosValidos = false;
        }
    }

    if ($("#file-upload").val() === "") {
        toastr.error('É obrigatório selecionar um arquivo!', 'Arquivo!');
        dadosValidos = false;
    }

    if ($("#Leiaute").val() === "0") {
        toastr.error('É obrigatório informar o Leiaute!', 'Leiaute!');
        dadosValidos = false;
    }

    if (!dadosValidos) {
        hideLoading();
        return false;
    }
    
    if (dadosValidos) {
        showLoading();
        var file = $("#file-upload");
        var url = "/Integracao/Importar/";

        var retorno = false;
        var form = new FormData();
        form.append("file", file[0].files[0]);
        form.append("leiaute", $("#Leiaute").val());

        $.ajax({
            url: url, 
            data: form,
            async: false,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (result) {
                if (result.Sucesso) {
                    retorno = true;
                    openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
                    $("[id*=file-upload]").val("");
                    hideLoading();
                } else {
                    openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
                    hideLoading();
                }
            }
        });

        pesquisar();
    }

    return retorno;
}

function BuscaAssessorias(leiaute) {
    $("#ddlAssessoria").empty();
    $("#ddlLote").empty();

    $.ajax({
        type: 'POST',
        url: '/Integracao/CarregarAssessorias',
        dataType: 'json',
        data: { leiaute: leiaute },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#ddlAssessoria").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function BuscaLotes(leiaute, assessoria) {
    $("#ddlLote").empty();
    $.ajax({
        type: 'POST',
        url: '/Integracao/CarregarLotes',
        dataType: 'json',
        data: { leiaute: leiaute, assessoria: assessoria },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#ddlLote").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}