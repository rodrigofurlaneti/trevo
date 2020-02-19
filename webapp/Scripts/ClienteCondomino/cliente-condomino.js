var veiculos = [];
var indiceveiculosAdicionados = 0;

$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    MakeChosen("cbClienteCondomino_Unidade");
    MakeChosen("veiculos");

    $("#cliente-condomino-form").submit(function () {
        if (!$("#unidade").val()) {
            toastr.warning("O campo unidade é obrigatório");
            return false;
        }
        if (!$(".item-veiculo").length) {
            toastr.warning("Adicione pelo menos 1 Veiculo!");
            return false;
        }
    });

    ClienteAutoComplete("clientes", "cliente", "clienteText", CarregarDadosPorCliente);
    
    if (isEdit() || isSave()) {
        BuscarClienteVeiculos()
            .done(() => {
                if ($('#cliente').val()) {
                    CarregarVeiculos();
                    CarregarUnidades($('#cliente').val());
                };
            });
    }
});

function CarregarDadosPorCliente() {
    CarregarVeiculos();
    CarregarUnidades($('#cliente').val());
}

function BuscarClienteVeiculos() {
    return get("BuscarClienteVeiculos")
        .done((response) => {
            veiculos = response;
        });
}

function CarregarUnidades(clienteId) {
    $("#unidade").empty();

    $.ajax({
        type: 'POST',
        url: '/ClienteCondomino/CarregarUnidades',
        data: { clienteId: clienteId !== undefined && clienteId !== null && clienteId !== "" ? clienteId : 0 },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    if (i === 0)
                        $("#unidade").append('<option value="">' + result.Text + '</option>');
                    else
                        $("#unidade").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if ($('#UnidadeSelecionada').val() !== '') {
                $('#unidade').val($('#UnidadeSelecionada').val());
            } else {
                $('#unidade').val("");
            }

            MakeChosen("unidade");
        },
        error: function (ex) {
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

function CarregaVeiculos(veiculos) {

    var veiculosSelect = document.getElementById("veiculos");
    veiculosSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione o veículo";
    option.value = 0;
    veiculosSelect.options.add(option);
    $.each(veiculos, function (i, item) {
        option = document.createElement("option");
        option.text = item.VeiculoFull;
        option.value = item.Id;
        veiculosSelect.options.add(option);
    });
    MakeChosen("veiculos");
}

function CarregarVeiculos() {
    var idCliente = $("#cliente").val();
    if (idCliente === undefined) {
        toastr.info('É necessário selecionar um cliente para atualizar a lista de veículos!');
    }
    else {
        showLoading();
        $.ajax({
            url: '/ClienteCondomino/BuscarVeiculos',
            dataType: 'json',
            data: { idCliente: idCliente === null || idCliente === "" ? 0 : idCliente },
            success: function (response) {
                CarregaVeiculos(response);
                hideLoading();
            },
            error: function (xhr) {
                toastr.error('BuscaClientes:' + xhr.responseText);
            }
        });
    }
}

function adicionarVeiculo() {
    var veiculo = {
        Id: $("#veiculos").val(),
        IdCliente: $("#cbClienteCondomino_Cliente").val(),
    }

    veiculos.push(veiculo);
    atualizarveiculos(veiculos);
}

function atualizarveiculos(veiculos) {
    showLoading();
    $.post("/ClienteCondomino/AtualizarVeiculos", { veiculos })
        .done((response) => {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#lista-contratoveiculo-result").empty();
                $("#lista-contratoveiculo-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}


function removerVeiculo(id) {
    veiculos = veiculos.filter(function (e) {
        return e.Id !== id;
    });
    atualizarveiculos(veiculos);
}