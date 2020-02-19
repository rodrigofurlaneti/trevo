$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
});

function salvarContrato() {
    var dt = $("#datatable_fixed_column tbody tr");
    var tipoLista = $("#hdnTipoLista").val();
    dt.splice(0,0);
    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        var data = dt[i].children[0] != undefined ? dt[i].children[0].innerText : "";
        var splitData = data.split("/");

        var dataVencimento = new Date(splitData[2], splitData[1], splitData[0]);
        
        var valorParcela = dt[i].children[6] != undefined ? dt[i].children[6].innerText : 0; 

        var valor = valorParcela.replace("R$","");

        obj.push({
            DataVencimento: dataVencimento,
            CodContrato: dt[i].children[5] != undefined ? dt[i].children[5].innerText ? parseInt(dt[i].children[5].innerText) : 0 : 0,
            ValorParcela: parseFloat(valor),
            Carteira: {
                Sigla: dt[i].children[2] != undefined ? dt[i].children[2].innerText : "",
                Descricao: dt[i].children[3] != undefined ? dt[i].children[3].innerText : "",
            },
            Devedor: {
                Cpf: dt[i].children[4] != undefined ? dt[i].children[4].innerText ? dt[i].children[4].innerText : "" : ""
            }
        });
    }

    $.ajax({
        url: "/spc/RemoverContrato",
        type: "POST",
        dataType: "json",
        data: { jsonContrato: JSON.stringify(obj), listaSessao: tipoLista },
        success: function (data) { }
    });

    //if (obj.length === 0)
    //    VisibilidadeMsgSemEnderecos();
}

function removerContrato(ele) {
    ele.parentElement.parentElement.remove();
    salvarContrato();
}
