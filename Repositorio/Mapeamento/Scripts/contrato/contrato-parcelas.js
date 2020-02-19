$(document).ready(function () {
    FormatarCampoData("vencimentoParcela");
});

function clear_form() {
    $("#numParcela, #vencimentoParcela, #valorParcelaPrincipal").val("");
    //$("#statusParcela option[value='0']").attr({ selected: "selected" });
    $("#statusParcela").val("0");
}

//Function do Grid
var idParcelaInEdit;

$("#add-parcela").click(function (ele) {
    
    var numParcela = document.getElementById("numParcela").value;
    var vencimentoParcela = document.getElementById("vencimentoParcela").value;
    var valorParcelaPrincipal = document.getElementById("valorParcelaPrincipal").value;
    var status = $("#statusParcela option:selected").val();
    var statusText = $("#statusParcela option:selected").text();

    if (status === "0") {
        openCustomModal(null, null, "warning", "Atenção", "Selecione o status da parcela.", false, null, function () { });
        return;
    }
    showLoading();

    if ($("#data_table_parcelas tbody tr.odd").length > 0)
        $("#data_table_parcelas tbody tr.odd").remove();

    removeHiddenItemsParcela();

    var nextIndex = $("table#data_table_parcelas tbody tr").length;

    $("#data_table_parcelas tbody")
        .append("<tr>" +
        "<td style=\"display: none;\" id=\"id_parcela\">" + (idParcelaInEdit ? idParcelaInEdit : 0) + "</td>" +
        "<td>" +
        numParcela +
        "</td>" +
        "<td>" +
        vencimentoParcela +
        "</td>" +
        "<td>" +
        valorParcelaPrincipal +
        "</td>" +
        "<td>" +
        statusText +
        "</td>" +
        "<td>" +
        "<span class=\"btn btn-primary\" style=\"margin-right: 10px;\" onclick=\"editarParcela(this, " + nextIndex + ")\">" +
        "<i class=\"fa fa-edit\"></i>" +
        "</span>" +
        "<span class=\"btn btn-danger\" onclick=\"removerParcela(this)\"><i class=\"fa fa-remove\"></i></span>" +
        "</td>" +
        "<td style=\"display: none;\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "NumParcela\" value=\"" + numParcela + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "VencimentoParcela\" value=\"" + vencimentoParcela + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "ValorParcelaPrincipal\" value=\"" + ConvertToFloat(filtraNumerosComVirgula(valorParcelaPrincipal)) + "\">" +
        "<input type=\"hidden\" id=\"" + nextIndex + "StatusParcela\" value=\"" + status + "\">" +
        "</td>" +
        "</tr>");

    salvarParcela();
    clearFieldsParcela();
    hideLoading();
});

function salvarParcela() {
    if ($("#data_table_parcelas tbody tr.odd").length > 0)
        $("#data_table_parcelas tbody tr.odd").remove();

    var dt = $("#data_table_parcelas tbody tr");

    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        var x = 0;
        obj.push({
            Id: dt[i].children[0] != undefined ? dt[i].children[0].innerText ? parseInt(dt[i].children[0].innerText) : 0 : 0,
            NumParcela: $("#" + i + "NumParcela").val(),
            DataVencimento: $("#" + i + "VencimentoParcela").val(),
            ValorParcela: $("#" + i + "ValorParcelaPrincipal").val(),
            StatusParcela: $("#" + i + "StatusParcela").val()
        });
    }

    $.ajax({
        url: "/contrato/AdicionarParcela",
        type: "POST",
        dataType: "json",
        data: { json: JSON.stringify(obj) },
        success: function () {
        }
    });

    if (obj.length === 0)
        VisibilidadeMsgSemParcela();
}

function editarParcela(ele, index) {
    var hiddenItems = $("#data_table_parcelas tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsParcela();

    idParcelaInEdit = ele.parentElement.parentElement.children[0].innerText.trim();
    document.getElementById("numParcela").value = $("#" + index + "NumParcela").val();
    document.getElementById("vencimentoParcela").value = $("#" + index + "VencimentoParcela").val();
    document.getElementById("valorParcelaPrincipal").value = $("#" + index + "ValorParcelaPrincipal").val();
    $("#statusParcela").val($("#" + index + "StatusParcela").val());


    $("#cancel").show();
    $(ele.parentElement.parentElement).hide();

    VisibilidadeMsgSemParcela();
}

function removerParcela(ele) {
    ele.parentElement.parentElement.remove();
    salvarParcela();

    VisibilidadeMsgSemParcela();
}

function cancelEditParcela() {
    var hiddenItems = $("#data_table_parcelas tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsParcela();

    VisibilidadeMsgSemParcela();
}

function removeHiddenItemsParcela() {
    var hiddenItems = $("#data_table_parcelas tbody tr:hidden");
    if (hiddenItems) hiddenItems.remove();
}

function clearFieldsParcela() {
    idParcelaInEdit = 0;
    clear_form();
    $("#cancel").hide();
}

function VisibilidadeMsgSemParcela() {
    var total = $("#data_table_parcelas tbody tr").length;
    var totalInvisiveis = $("#data_table_parcelas tbody tr:hidden").length;
    if (total === 0 || total === totalInvisiveis) {
        $("#data_table_parcelas tbody")
            .append("<tr class=\"odd\">" +
            "<td valign=\"top\" colspan=\"5\" class=\"dataTables_empty\">" +
            "Nenhum registro encontrado" +
            "</td>" +
            "</tr>");
    } else {
        $("#data_table_parcelas tbody tr.odd").remove();
    }
}
