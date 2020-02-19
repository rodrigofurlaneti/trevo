var veiculos = [];
var veiculoEmEdicao = {};
var indiceVeiculosAdicionados = 0;
var tpVeiculo = {};

$(document).ready(function () {
    MakeChosen("marca");
    MakeChosen("modelo");
    MakeChosen("tipo");
    FormatarPlaca();

    $.ajax({
        url: '/veiculo/ListaMarca',
        type: 'POST',
        dataType: 'json',
        data: {},
        success: function (response) {
            CarregaMarca(response);
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseText);
            console.log(error.responseText);
        }
    });

    if (location.pathname.includes("veiculo")) {
        ListaVeiculo();
    }

    $.ajax({
        type: 'POST',
        url: '/veiculo/CarregarTipos',
        dataType: 'json',
        success: function (result) {
            CarregaTipos(result);
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
});

function MudaMarca() {
    var marcaSelect = document.getElementById("marca"); 
    var modeloSelect = document.getElementById("modelo");
    var obj = [];
    var newObj = {
        Id: marcaSelect.options[marcaSelect.selectedIndex].value,
        Nome: marcaSelect.options[marcaSelect.selectedIndex].text
    };
    obj.push(newObj);

    return post('ListaModelo', { marca: JSON.stringify(obj) }, 'Veiculo')
        .done((response) => {
            CarregaModelo(response)
        }); 
}

function CarregaMarca(marcas) {
    var marcaSelect = document.getElementById("marca");
    $.each(marcas, function (i, item) {
        var option = document.createElement("option");
        option.text = item.Nome;
        option.value = item.Id;
        marcaSelect.options.add(option);
    });
    MakeChosen("marca");
}

function CarregaTipos(tipos) {
    var tiposSelect = document.getElementById("tipo");
    $.each(tipos, function (i, item) {
        var option = document.createElement("option");
        option.text = item.Text;
        option.value = item.Value;
        tiposSelect.options.add(option);
    });
    MakeChosen("tipo");
}

function CarregaModelo(modelos) {
    var modeloSelect = document.getElementById("modelo");
    modeloSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione o Modelo";
    option.value = 0;
    modeloSelect.options.add(option);
    $.each(modelos, function (i, item) {
        option = document.createElement("option");
        option.text = item.Descricao;
        option.value = item.Id;
        modeloSelect.options.add(option);
    });
    MakeChosen("modelo");
}


function AdicionaVeiculo() {

    indiceVeiculosAdicionados--;
    var marca = document.getElementById("marca");
    var modelo = document.getElementById("modelo");
    var tipo = document.getElementById("tipo");
    var placa = document.getElementById("placa");
    var cor = document.getElementById("cor");
    var ano = document.getElementById("ano");

    if (!validarVeiculo(marca, modelo, tipo, placa, cor, ano))
        return;

    var veiculo = {
        Id: indiceVeiculosAdicionados,
        Placa: placa.value,
        Cor: cor.value,
        Ano: ano.value,
        Modelo: {
            Id: modelo.value,
            Descricao: modelo.options[modelo.selectedIndex].text,
            Marca: {
                Id: marca.value,
                Nome: marca.options[marca.selectedIndex].text
            }
        },
        TipoVeiculo: tipo.value
    }

    tpVeiculo = {
        Value: tipo.value,
        Text: tipo.options[tipo.selectedIndex].text
    }

    veiculos.push(veiculo);

    atualizaVeiculos();
   
    marca.selectedIndex = "0";
    modelo.selectedIndex = "0";
    tipo.selectedIndex = "0";
    MakeChosen("marca");
    MakeChosen("modelo");
    MakeChosen("tipo");
    placa.value = '';
    cor.value = '';
    ano.value = '';
    veiculoEmEdicao = {};
}

function ListaVeiculo() {
    var element = document.getElementById('hdnCliente');
    if (typeof (element) != 'undefined' && element != null) {
        if (element.value != "") {
            var obj = [];
            var newObj = {
                Id: element.value
            };
            obj.push(newObj);
            $.ajax({
                url: '/veiculo/ListaVeiculo',
                type: 'POST',
                dataType: 'json',
                data: { cliente: JSON.stringify(obj) },
                success: function (response) {
                    if (response && response.length) {
                        veiculos = response;
                        CarregaVeiculo();
                        CarregaVeiculoOcorrenciaCliente();
                    }
                }
            })
        };

    }
}

function CarregaVeiculo() {


    $('#gridVeiculos tbody').empty();
    $.each(veiculos, function (i, veiculo) {
        var tpId = 0;
        var tpDesc = "";
        if (tpVeiculo.Value == null) {
            tpId = veiculo.TipoVeiculo;
            tpDesc = veiculo.TipoVeiculoDesc;
        }
        else {
            tpId = tpVeiculo.Value;
            tpDesc = tpVeiculo.Text;
        }
        $('#gridVeiculos tbody')
            .append('<tr>'
                + '<td style="display:none">'
                + veiculo.Id
                + '</td>'
                + '<td style="display:none">'
                + veiculo.Modelo.Marca.Id
                + '</td>'
                + '<td style="display:none">'
                + veiculo.Modelo.Id
                + '</td>'
                + '<td style="display:none">'
                + tpId
                + '</td>'
                + '<td class="col-xs-10">'
                + veiculo.Modelo.Marca.Nome
                + '</td>'
                + '<td class="col-xs-10">'
                + veiculo.Modelo.Descricao
                + '</td>'
                + '<td class="col-xs-10">'
                + tpDesc
                + '</td>'
                + '<td class="col-xs-10" style="min-width:100px">'
                + veiculo.Placa
                + '</td>'
                + '<td class="col-xs-10">'
                + veiculo.Cor
                + '</td >'
                + '<td class="col-xs-10">'
                + veiculo.Ano
                + '</td>'
                + '<td class="col-xs-2">'
                + '<span class="btn btn-primary" style="margin-right: 10px;" onclick="EditaVeiculo(' + veiculo.Id + ')")><i class="fa fa-edit"></i></span>'
                + '<span class="btn btn-danger" onclick="RemoveVeiculo(' + veiculo.Id + ')"><i class="fa fa-remove"></i></span>' 
                + '</td>'
                + '</tr>'
        );
    });
}

function EditaVeiculo(id) {
    if (Object.keys(veiculoEmEdicao).length)
        veiculos.push(veiculoEmEdicao);

    veiculoEmEdicao = veiculos.find(x => x.Id == id);

    veiculos = veiculos.filter(x => x.Id != veiculoEmEdicao.Id);

    console.table(veiculoEmEdicao);

    $("#marca").val(veiculoEmEdicao.Modelo.Marca.Id);
    MakeChosen("marca");

    MudaMarca()
        .done(() => {
            $("#modelo").val(veiculoEmEdicao.Modelo.Id);
            MakeChosen("modelo");
        });

    $("#tipo").val(veiculoEmEdicao.TipoVeiculo);
    MakeChosen("tipo");

    var placa = document.getElementById("placa");
    placa.value = veiculoEmEdicao.Placa;
    var cor = document.getElementById("cor");
    cor.value = veiculoEmEdicao.Cor;
    var ano = document.getElementById("ano");
    ano.value = veiculoEmEdicao.Ano;
    atualizaVeiculos();
}

function RemoveVeiculo(id) {
    veiculos = veiculos.filter(x => x.Id != id);

    atualizaVeiculos();
}

function atualizaVeiculos() {
    showLoading();
    $.post('/veiculo/AtualizaVeiculos', { jsonVeiculos: JSON.stringify(veiculos) })
        .done(() => {
            CarregaVeiculo();
            CarregaVeiculoOcorrenciaCliente();
        })
        .always(() => { hideLoading(); });
}

function CarregaVeiculoOcorrenciaCliente() {

    $("#ocorrencia-cliente-placa").empty();

    $.each(veiculos,
        function (i, result) {
            $("#ocorrencia-cliente-placa").append('<option value="' + result.Id + '">' + result.Placa + '</option>');
        });

    if ($('#placa-selecionada').val() !== '') {
        $('#ocorrencia-cliente-placa').val($('#placa-selecionada').val());
    } else {
        $('#ocorrencia-cliente-placa').val("");
    }


    MakeChosen("ocorrencia-cliente-placa");
}

function validarVeiculo(marca, modelo, tipo, placa, cor, ano) {
    if (marca.selectedIndex <= 0) {
        toastr.error('Selecione a Marca!', 'Informe a Marca!');
        return false;
    }

    if (modelo.selectedIndex <= 0) {
        toastr.error('Selecione o Modelo!', 'Informe o Modelo!');
        return false;
    }

    if (tipo.selectedIndex <= 0) {
        toastr.error('Selecione o Tipo!', 'Informe o Tipo!');
        return false;
    }

    if (!placa.value) {
        toastr.error('Informe a Placa!', 'Placa Invalida!');
        return false;
    }

    if (placa.value.match(/_/)) {
        toastr.error('Informe a Placa Completa!', 'Placa Invalida!');
        return false;
    }


    //if (!cor.value) {
    //    toastr.error('Informe a Cor!', 'Cor Invalida!');
    //    return false;
    //}
    //if (!ano.value) {
    //    toastr.error('Informe o Ano!', 'Ano Invalido!');
    //    return false;
    //}

    return true;
}


