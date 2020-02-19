var veiculoEmEdicaoCadastroCliente = {};
var veiculos = [];
var indiceVeiculosAdicionados = 0;
var tpVeiculo = {};

function CarregaVeiculoCadastroCliente() {

    $('#gridVeiculosCadastroCliente tbody').empty();
    $.each(veiculos, function (i, veiculo) {

        $('#gridVeiculosCadastroCliente tbody')
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
                + '<td class="col-xs-3">'
                + veiculo.Modelo.Marca.Nome
                + '</td>'
                + '<td class="col-xs-3">'
                + veiculo.Modelo.Descricao
                + '</td>'
                + '<td class="col-xs-1" style="min-width:100px">'
                + veiculo.Placa
                + '</td>'
                + '<td class="col-xs-2">'
                + veiculo.Cor
                + '</td >'
                + '<td class="col-xs-1">'
                + veiculo.Ano
                + '</td>'
                + '<td class="col-xs-1">'
            + '<span class="btn btn-primary" style="margin-right: 10px;" onclick="EditaVeiculoCadatroCliente(' + veiculo.Id + ')"><i class="fa fa-edit"></i></span>'
            + '<span class="btn btn-danger" onclick="RemoveVeiculoCadatroCliente(' + veiculo.Id + ')"><i class="fa fa-remove"></i></span>'
                + '</td>'
                + '</tr>'
            );
    });
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

function CarregaVeiculoContratoMensalista() {
    $("#veiculos").empty();

    $.each(veiculos,
        function (i, result) {
            $("#veiculos").append('<option value="' + result.Id + '">' + result.Placa + '</option>');
        });

    if ($('#placa-selecionada').val() !== '') {
        $('#veiculos').val($('#placa-selecionada').val()).trigger("chosen:updated");
    } else {
        $('#veiculos').val("").trigger("chosen:updated");
    }
    
    MakeChosen("veiculos");
}

function AdicionaVeiculoCadastroCliente() {
    indiceVeiculosAdicionados--;
    var marca = document.getElementById("cliente-cadastro-marca");
    var marcaId = document.getElementById("marca-id");

    var modelo = document.getElementById("cliente-cadastro-modelo");
    var modeloId = document.getElementById("modelo-id");

    var placa = document.getElementById("placa");
    var cor = document.getElementById("cor");
    var ano = document.getElementById("ano");

    if (!validarVeiculoCadastroCliente(marca, modelo, placa, cor, ano))
        return;

    var veiculo = {
        Id: indiceVeiculosAdicionados,
        Placa: placa.value,
        Cor: cor.value,
        Ano: ano.value,
        Modelo: {
            Id: modeloId.value ? modeloId.value : -1,
            Descricao: modelo.value,
            Marca: {
                Id: marcaId.value ? marcaId.value : -1,
                Nome: marca.value
            }
        }
    };

    veiculos.push(veiculo);

    atualizaVeiculosCadastroCliente();

    marca.value = "";
    modelo.value = "";
    placa.value = '';
    cor.value = '';
    ano.value = '';
    veiculoEmEdicaoCadastroCliente = {};
}

function validarVeiculoCadastroCliente(marca, modelo, placa, cor, ano) {
    var valido = true;

    if (modelo === null || modelo.value === null || modelo.value === "") {
        toastr.error('Informe o Modelo!', 'Modelo Invalido!');
        valido = false;
    }

    if (marca === null || marca.value === null || marca.value === "") {
        toastr.error('Informe a Marca!', 'Marca Invalida!');
        valido = false;
    }

    if (!placa.value) {
        toastr.error('Informe a Placa!', 'Placa Invalida!');
        valido = false;
    }

    if (placa.value.match(/_/)) {
        toastr.error('Informe a Placa Completa!', 'Placa Invalida!');
        valido = false;
    }

    //if (!cor.value) {
    //    toastr.error('Informe a Cor!', 'Cor Invalida!');
    //    valido = false;
    //}

    //if (!ano.value) {
    //    toastr.error('Informe o Ano!', 'Ano Invalido!');
    //    valido = false;
    //}

    return valido;

    
}

function EditaVeiculoCadatroCliente(id) {
    if (Object.keys(veiculoEmEdicaoCadastroCliente).length)
        veiculos.push(veiculoEmEdicaoCadastroCliente);

    veiculoEmEdicaoCadastroCliente = veiculos.find(x => parseInt(x.Id) === id);

    veiculos = veiculos.filter(x => parseInt(x.Id) !== veiculoEmEdicaoCadastroCliente.Id);

    console.table(veiculoEmEdicaoCadastroCliente);

    $("#cliente-cadastro-modelo").val(veiculoEmEdicaoCadastroCliente.Modelo.Descricao);

    $("#cliente-cadastro-marca").val(veiculoEmEdicaoCadastroCliente.Modelo.Marca.Nome);

    var placa = document.getElementById("placa");
    placa.value = veiculoEmEdicaoCadastroCliente.Placa;
    var cor = document.getElementById("cor");
    cor.value = veiculoEmEdicaoCadastroCliente.Cor;
    var ano = document.getElementById("ano");
    ano.value = veiculoEmEdicaoCadastroCliente.Ano;
    atualizaVeiculosCadastroCliente();
}

function RemoveVeiculoCadatroCliente(id) {
    veiculos = veiculos.filter(x => parseInt(x.Id) !== id);

    atualizaVeiculosCadastroCliente();
}

function atualizaVeiculosCadastroCliente() {
    showLoading();
    $.post('/veiculo/AtualizaVeiculos', { jsonVeiculos: JSON.stringify(veiculos) })
        .done(() => {
            CarregaVeiculoCadastroCliente();
            CarregaVeiculoContratoMensalista();
            CarregaVeiculoOcorrenciaCliente();
        })
        .always(() => { hideLoading(); });
}

$(document).ready(function () {
    MakeChosen("marca");
    MakeChosen("modelo");
    FormatarPlaca();

    if (location.pathname.includes("veiculo")) {
        ListaVeiculo();
    }
});

function BuscarMarca() {
    var modeloId = document.getElementById("modelo-id");
    return post('ListaMarca', { jsonModeloId: modeloId.value }, 'Veiculo')
        .done((response) => {
            $("#cliente-cadastro-marca").val(response.Nome);  
            $("#marca-id").val(response.Id);
        });
}

function ListaVeiculo() {
    var element = document.getElementById('hdnCliente');
    if (typeof element !== 'undefined' && element !== null) {
        if (element.value !== "") {
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
                    }
                }
            });
        }
    }
}

