$("#dataVigencia").mask("99/99/9999");

var idDocumentacaoInEdit;

$(document).ready(function () {
    CarregaTipoDocumentacao();
    FormatarDataDocumentacao("dataVigencia");
});

function CarregaTipoDocumentacao() {
    $("#tipoDocumentacao").html("");

    $.getJSON("/Documentacao/SelecionarTipoDocumentacao", null, function (data) {
        $("#tipoDocumentacao").append("<option value=" + 0 + ">" + "Selecione Tipo Documentacao..." + "</option>");
        $.each(data.Result, function (index, tipoDocumentacao) {                   
            $("#tipoDocumentacao").append("<option value=" + tipoDocumentacao.Id + ">" + tipoDocumentacao.Nome + "</option>");

        });
    })
}

$('#add-documentacao').click(function (ele) {
    var t = document.getElementById("tipoDocumentacao");
    var tipoDocumentacao = t.options[t.selectedIndex].text;

    var dataVigencia = document.getElementById('dataVigencia').value;
    var dataSplit = dataVigencia.split("/");

    var dataFimDoc = new Date(dataSplit[2], dataSplit[1] - 1, dataSplit[0]);
    var dataAtual = new Date();
    var diferenca = Math.abs(dataFimDoc.getTime() - dataAtual.getTime());
    var dias = Math.ceil(diferenca / (1000 * 3600 * 24));
    var farol;

    if (dias <= 20) {
        farol = 'dotred';
    } else if (dias >= 21 && dias <= 60) {
        farol = 'dotyellow';
    } else {
        farol = 'dotgreen';
    }

    $('#data_table_documentacoes tbody')
        .append('<tr>' +
            '<td style="display:none;" id="id_documentacao">' + (idDocumentacaoInEdit ? idDocumentacaoInEdit : 0) + '</td>' +
            '<td class="col-xs-4">' + document.getElementById('descricao').value + '</td>' +
            '<td class="col-xs-2">' + dataVigencia + '</td>' +
            '<td class="col-xs-3">' + tipoDocumentacao + '</td>' +
            '<td class="col-xs-1">' + '<span class="'+farol+'"></span>' + '</td>' +
            '<td style="display:none;" id="idTipoDocumento">' + document.getElementById("tipoDocumentacao").value + '</td>' +
            '<td class="col-xs-2">' +
            '<span class="btn btn-primary" style="margin-right: 10px;" onclick="editDocumentacao(this)"><i class="fa fa-edit"></i></span>' +
            '<span class="btn btn-danger" onclick="removeDocumentacao(this)"><i class="fa fa-remove"></i></span>' +
            '</td>' +
            '</tr>');

    removeHiddenItemsDocumentacao();
    saveDocumentacoes();
    clearFieldsDocumentacao();
});

function saveDocumentacoes() {
    var dt = $("#data_table_documentacoes tbody tr");
    dt.splice(0, 0);
    debugger;
    var obj = [];
    
    for (var i = 0; i < dt.length; i++) {
        var dataVigencia = dt[i].children[2] != undefined ? dt[i].children[2].innerText : '';
        if (dataVigencia !== '') {
            var partDate = dataVigencia.split('/');
            dataVigencia = new Date(parseInt(partDate[2]), parseInt(partDate[1]) - 1, parseInt(partDate[0]));
        }

        obj.push({
            Id: dt[i].children[0] != undefined ? dt[i].children[0].innerText ? parseInt(dt[i].children[0].innerText) : 0 : 0,
            Descricao: dt[i].children[1] != undefined ? dt[i].children[1].innerText : '',
            DataVigencia: dataVigencia !== '' ? dataVigencia : Date.now,
            TipoDocumentacao: { Nome: dt[i].children[3] != undefined ? dt[i].children[3].innerText : '' },
            TipoDocumentacao: { Id: dt[i].children[4] != undefined ? dt[i].children[4].innerText ? parseInt(dt[i].children[4].innerText) : 0 : 0 }
        });
    }

    $.ajax({
        url: '/documentacao/AdicionarDocumentacao',
        type: 'POST',
        dataType: 'json',
        data: { json: JSON.stringify(obj) },
        success: function (data) { }
    });

    if (obj.length > 0 && $('#data_table_documentacoes tbody tr.odd').length > 0)
        $('#data_table_documentacoes tbody tr.odd').remove();
}

function editDocumentacao(ele) {
    var hiddenItems = $('#data_table_documentacoes tbody tr:hidden');
    if (hiddenItems) hiddenItems.show();
    clearFields();
    $('#cancelDocumentacao').hide();

    idDocumentacaoInEdit = ele.parentElement.parentElement.children[0].innerText.trim();
    var value = ele.parentElement.parentElement.children[1].textContent.trim();
    
    document.getElementById('descricao').value = ele.parentElement.parentElement.children[1].textContent.trim();
    document.getElementById('dataVigencia').value = ele.parentElement.parentElement.children[2].textContent.trim();
    document.getElementById('tipoDocumentacao').value = ele.parentElement.parentElement.children[4].textContent.trim();

    $('#cancelDocumentacao').show();
    $(ele.parentElement.parentElement).hide();

    VisibilidadeMsgSemContatos();
}

function removeDocumentacao(ele) {
    ele.parentElement.parentElement.remove();
    saveDocumentacoes();

    VisibilidadeMsgSemDocumentaoes();
}

function cancelEditDocumentacao() {
    var hiddenItems = $('#data_table_documentacoes tbody tr:hidden');
    if (hiddenItems) hiddenItems.show();
    clearFieldsDocumentacao();
    $('#cancelDocumentacao').hide();

    VisibilidadeMsgSemDocumentacoes();
}

function removeHiddenItemsDocumentacao() {
    var hiddenItems = $('#data_table_documentacoes tbody tr:hidden');
    if (hiddenItems) hiddenItems.remove();
}

function clearFieldsDocumentacao() {
    idContactInEdit = 0;
    document.getElementById('descricao').value = "";
    document.getElementById('dataVigencia').value = "";
    document.getElementById('tipoDocumentacao').value = "";
    $('#cancelDocumentacao').hide();
}

function VisibilidadeMsgSemDocumentacoes() {
    var totalDocumentacoes = $("#data_table_documentacoes tbody tr").length;
    var totalDocumentacoesInvisiveis = $("#data_table_documentacoes tbody tr:hidden").length;
    if (totalDocumentacoes == 0 || totalDocumentacoes == totalDocumentacoesInvisiveis)
        $('#data_table_documentacoes tbody')
            .append(
                "<tr class='odd'><td valign='top' colspan='2' class='dataTables_empty'>Nenhum registro encontrado</td></tr>");
    else
        $('#data_table_documentacoes tbody tr.odd').remove();
}

function FormatarDataDocumentacao(pCampoId) {
    $("#" + pCampoId).mask("99/99/9999");
    $("#" + pCampoId).datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: ">",
        prevText: "<",
        minDate: 1
    });
}

