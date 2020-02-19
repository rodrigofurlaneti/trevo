$(document).ready(function () {
    MudaEstiloSelect();
    BuscarParametroNegociacao();
    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
});

function MudaEstiloSelect(){
    var selects = document.getElementsByTagName("select");
    for (var contador = 0; contador < selects.length; contador++) {
        MakeChosen(selects[contador].id, null, "100%");
    }
}

$("#ParametroNegociacaoForm").submit(function (e) {
    if (ValidaParametroNegociacao()) {
        GravaLimiteDesconto();
    }
    else{
        e.preventDefault();
        return false;
    }

    
   
});

function ValidaParametroNegociacao() {
    if ((document.getElementById("unidade").value > 0
            && document.getElementById("perfil").value <= 0)
        || (document.getElementById("unidade").value <= 0
            && document.getElementById("perfil").value > 0)
        || (document.getElementById("unidade").value <= 0
            && document.getElementById("perfil").value <=  0)) {
        toastr.error('Preencha a Unidade e o Perfil', 'Unidade e Perfil são Obrigatórios');
        return false;
    }

    return true;
}

function GravaLimiteDesconto() {
    var tiposValor = document.getElementsByTagName("select");
    limitesDesconto = [];
    for (contador = 0; contador < tiposValor.length; contador++) {
        if (tiposValor[contador].value > 0) {
            var nomes = tiposValor[contador].id.split("_");
            if (nomes[2] == "TipoServico") {
                var limiteDesconto = {
                    Id: document.getElementById(nomes[0] + "_" + nomes[1] + "_Hidden").value
                    , TipoServico: nomes[0]
                    , TipoValor: tiposValor[contador].value
                    , Valor: document.getElementById(nomes[0] + "_" + nomes[1] + "_TextBox").value
                };
                limitesDesconto.push(limiteDesconto);
            }
            
        }
    }

    $.ajax({
        url: "/ParametroNegociacao/SalvaLimiteDesconto",
        type: "POST",
        dataType: "json",
        data: { json: JSON.stringify(limitesDesconto) }
    });

}

function BuscarParametroNegociacao() {
    var unidade = document.getElementById("unidadeBusca");
   
   
    $.ajax({
        url: "/ParametroNegociacao/BuscarParametroNegociacao",
        type: "POST",
        dataType: "json",
        data: { unidade: unidade.value },
        success: function (response) {
            if (response.Status == "Success") {
                $('#divParametroNegociacao').empty();
                $('#divParametroNegociacao').append(response.Html);
            }
        },
        error: function (error) {
            window.alert('Error');
        }
    });
}

function ExcluiParametroNegociacao(id) {
    $.ajax({
        url: "/ParametroNegociacao/Delete",
        type: "POST",
        dataType: "json",
        data: { id: id },
        success: function (response) {
            if (response.Status == "Success") {
                RemoveItemNaGridParametroNegociacao(id);
            }
        },
        error: function (error) {
            window.alert('Error');
        }
    });
}

function RemoveItemNaGridParametroNegociacao(id) {
    var index = RetornaIndex(id);
    document.getElementById("gridParametroNegociacao").deleteRow(index);

}

function RetornaIndex(id) {
    var gridParametroNegociacao = document.getElementById("gridParametroNegociacao");
    for (var rowIndex = 0; rowIndex < gridParametroNegociacao.rows.length; rowIndex++) {
        if (gridParametroNegociacao.rows[rowIndex].cells[0].innerHTML == id) {
            return rowIndex;
        }
    }
}