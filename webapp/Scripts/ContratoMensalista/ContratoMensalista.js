let temVeiculosParaSelecionar = false;
var veiculosCliente = [];
var indiceveiculosAdicionados = 0;

$(document).ready(function () {
    callbackPaginacao = FiltrarCampos;

    MakeChosen("cbContratoMensalista_TipoMensalista");
    MakeChosen("veiculos");
    MakeChosen("cbContratoMensalista_TabelaPrecoMensalista");

    var unidadeId = $("#UnidadeSelecionada").val();
    MakeChosen("unidade");
    if (unidadeId !== "" && unidadeId !== null && unidadeId !== undefined && parseInt(unidadeId) > 0) {
        $('#UnidadeSelecionada').val(unidadeId);
    }

    $("#txtValor, #vlrPago").maskMoney({
        prefix: "R$",
        allowNegative: false,
        allowZero: false,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
    $("#txtValor, #vlrPago").change(function () {
        var value = $(this).val();
        if (value !== "" && value !== null && value !== undefined) {
            $(this).val(value.replace(".", ""));
        }
    });

    FormatarCampoData('dtVencimento');
    FormatarCampoData('dtInicio');
    FormatarCampoData('dtFim');

    $("#ContratoMensalistaForm").submit(function (e) {
        showLoading();

        if (!ValidarCampos()) {
            hideLoading();
            e.preventDefault();
            return false;
        }
        if (!VerificacaoBloqueioReferencia()) {
            hideLoading();
            e.preventDefault();
            return false;
        }
    });

    $('#cbContratoMensalista_TabelaPrecoMensalista').change(function () {
        $('#TabelaPrecoMensalistaSelecionada').val($(this).val());
    });

    ClienteAutoComplete("clientes", "cliente", "clienteText", CarregarDadosPorCliente);

    $('#unidade').change(function () {
        CarregarTabelaPrecoMensalista(this.value);
    });

    $("#txtCodContrato").change(function () {
        var idContrato = $("#hdnContratoMensalista").val() !== null && $("#hdnContratoMensalista").val() !== "" ? $("#hdnContratoMensalista").val() : 0;
        var codigoContrato = $(this).val();
        if ($(this).val() !== "" && $(this).val() !== null && $(this).val() !== undefined)
            VerificarSeNumeroContratoExiste(idContrato, codigoContrato, "txtCodContrato");
    });

    if (isEdit() || isSave()) {
        BuscarClienteVeiculos()
            .done(() => {
                let unidadeId = $('#UnidadeSelecionada').val();
                CarregarTabelaPrecoMensalista(unidadeId);

                if ($('#cliente').val()) {
                    CarregarVeiculos();
                    CarregarUnidades($('#cliente').val());
                }
            });
    }
});

function VerificarSeNumeroContratoExiste(idContrato, codigoContrato, campoCodigoContrato) {
    $.ajax({
        url: "/ContratoMensalista/VerificarSeNumeroContratoExiste",
        type: "POST",
        dataType: "json",
        data: {
            idContrato,
            codigoContrato
        },
        success: function (result) {
            if (typeof result.resultado === "object"
                && result.resultado.Titulo !== null
                && result.resultado.Titulo !== "") {
                openCustomModal(null,
                    null,
                    result.resultado.TipoModal,
                    result.resultado.Titulo,
                    result.resultado.Mensagem,
                    false,
                    null,
                    function () { });
            }

            if (result.existe) {
                $("#" + campoCodigoContrato).val("");
                toastr.warning("O Código do Contrato Já Está em Uso!", "Atenção!");
            }
        },
        error: function (error) {
            console.log(error);
            toastr.error("Ocorreu um erro ao fazer a verificação, tente novamente.", "Erro");
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function CarregarDadosPorCliente() {
    CarregarVeiculos();
    CarregarUnidades($('#cliente').val());
}

function BuscarClienteVeiculos() {
    return get("BuscarClienteVeiculos")
        .done((response) => {
            veiculosCliente = response;
        });
}

function CarregaVeiculos(veiculos) {
    temVeiculosParaSelecionar = veiculosCliente && veiculosCliente.length ? true : false;
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
            url: '/ContratoMensalista/BuscarVeiculos',
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
        IdCliente: $("#cbContratoMensalista_Cliente").val()
    };

    veiculosCliente.push(veiculo);
    atualizarveiculos(veiculosCliente);
}
function adicionarVeiculoCadastroCliente() {
    var veiculo = veiculos.find(x => x.Placa === $("#veiculos option:selected").text());
    
    veiculosCliente.push(veiculo);

    atualizarveiculos(veiculosCliente);

    $("#veiculos").val("").trigger("chosen:updated");
}

function atualizarveiculos(veiculos) {
    showLoading();
    $.post("/ContratoMensalista/AtualizarVeiculos", { veiculos })
        .done((response) => {
            if (typeof response === "object") {
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
    veiculosCliente = veiculosCliente.filter(function (e) {
        return parseInt(e.Id) !== id;
    });

    atualizarveiculos(veiculosCliente);
}


function ValidarCampos() {
    if ($("#txtCodContrato").val() === "") {
        toastr.error("O campo \"Código de Contrato\" é obrigatório");
        return false;
    }
    if ($("#cliente").val() === "") {
        toastr.error("O campo \"Cliente\" é obrigatório");
        return false;
    }
    if ($("#cbContratoMensalista_TipoMensalista").val() === "") {
        toastr.error("O campo \"Tipo Mensalista\" é obrigatório");
        return false;
    }
    if (!$("#unidade").val()) {
        toastr.error("O campo \"Unidade\" é obrigatório");
        return false;
    }
    if ($("#dtInicio").val() === "") {
        toastr.error("O campo \"Data Inicio\" é obrigatório");
        return false;
    } else if (verificaData($("#dtInicio").val()) === false) {
        toastr.error("O campo \"Data Inicio\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }
    if ($("#dtFim").val() !== "" && verificaData($("#dtFim").val()) === false) {
        toastr.error("O campo \"Data Fim\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }
    if ($("#dtVencimento").val() === "") {
        toastr.error("O campo \"Data Vencimento\" é obrigatório");
        return false;
    } else if (verificaData($("#dtVencimento").val()) === false) {
        toastr.error("O campo \"Data Vencimento\" deve ser preenchido com uma data válida!");
        dadosValidos = false;
    }

    if ($("#txtValor").val() === "") {
        toastr.error("O campo \"Valor\" é obrigatório");
        return false;
    }
    if ($("#numero-vagas").val() === "" || $("#numero-vagas").val() === null || $("#numero-vagas").val() === undefined) {
        toastr.error("O campo \"Número de Vagas\" é obrigatório");
        return false;
    }
    else if (parseInt($("#numero-vagas").val()) <= 0) {
        toastr.error("O campo \"Número de Vagas\" deve receber um valor acima de zero");
        return false;
    }
    if ($('#TabelaPrecoMensalistaSelecionada').val() === "") {
        toastr.error("O campo \"Tabela de Preço\" é obrigatório");
        return false;
    }
    if (!temVeiculosParaSelecionar) {
        toastr.error("Necessário cadastro do veículo referente ao cliente em tela de Cliente", "Campo Obrigatório");
        return false;
    }
    if (!$(".item-veiculo").length) {
        toastr.error("Adicione pelo menos 1 Veículo!");
        return false;
    }

    return true;
}

function VerificacaoBloqueioReferencia() {
    var retorno = true;
    showLoading();
    $.ajax({
        url: "/ContratoMensalista/VerificacaoBloqueioReferencia",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            contratoMensalista: {
                Id: $("#hdnContratoMensalista").val(),
                DataVencimento: $("#dtVencimento").val()
            }
        },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
                retorno = false;
            }
            else if (typeof result === "object" && result.Sucesso !== undefined && !result.Sucesso) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
                retorno = false;
            }
            else {
                retorno = true;
            }
        },
        error: function (error) {
            console.log(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

    return retorno;
}

var veiculoEmEdicao = {};
function editarVeiculo(id) {
    if (Object.keys(veiculoEmEdicao).length)
        veiculosCliente.push(veiculoEmEdicao);

    veiculoEmEdicao = veiculosCliente.find(x => x.Id === id);
    removerVeiculo(id);

    $("#id").val(veiculoEmEdicao.Id);
    $("#veiculofull").val(veiculoEmEdicao.VeiculoFull);
    $("#cancel").show();
}

function ImprimirContrato(contratoMensalista) {
    var idContratoMensalista = $('#hdnContratoMensalista').val();

    $.ajax({
        url: '/ContratoMensalista/ImprimirContrato',
        dataType: 'json',
        data: { contratoMensalistaID: idContratoMensalista },
        success: function (html) {
            let printContents, popupWin;
            printContents = html;
            popupWin = window.open('', '_blank');
            popupWin.document.open();
            popupWin.document.write(`
          <html>
            <head>

            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

            <!-- Optional theme -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

            <!-- Latest compiled and minified JavaScript -->
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

            </head>
            <style>

            </style>

            <body onload="window.print(); window.close()">${printContents}</body>

          </html>`
            );
            popupWin.document.close();
        },
        error: function (xhr) {

            let printContents, popupWin;
            printContents = xhr.responseText;
            popupWin = window.open('', '_blank');
            popupWin.document.open();
            popupWin.document.write(`
          <html>
            <head>

            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">

            <!-- Optional theme -->
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">

            <!-- Latest compiled and minified JavaScript -->
            <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>

            </head>
            <style>

            </style>

            <body onload="window.print(); window.close()">${printContents}</body>

          </html>`
            );
            popupWin.document.close();
        }
    });
}

function CarregarUnidades(clienteId) {
    $("#unidade").empty();

    $.ajax({
        type: 'POST',
        url: '/ContratoMensalista/CarregarUnidades',
        data: { clienteId: clienteId !== undefined && clienteId !== null && clienteId !== "" ? clienteId : 0 },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    if (i === 0)
                        $("#unidade").append('<option value="">' + result.Text + '</option>');
                    else
                        $("#unidade").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            MakeChosen("unidade");

            if ($('#UnidadeSelecionada').val() !== '') {
                $('#unidade').val($('#UnidadeSelecionada').val()).trigger("chosen:updated");
            } else {
                $('#unidade').val("").trigger("chosen:updated");
            }
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

function CarregarTabelaPrecoMensalista(unidadeId, tabelaIdSelected) {
    if (!unidadeId)
        unidadeId = 0;
    
    $("#cbContratoMensalista_TabelaPrecoMensalista").empty();
    $.ajax({
        type: 'POST',
        url: '/ContratoMensalista/CarregarTabelaPrecoMensalista',
        data: { unidadeId },
        success: function (result) {
            $("#cbContratoMensalista_TabelaPrecoMensalista").empty()/
            $.each(result,
                function (i, result) {
                    if (i === 0)
                        $("#cbContratoMensalista_TabelaPrecoMensalista").append('<option value="">' + result.Text + '</option>');
                    else
                        $("#cbContratoMensalista_TabelaPrecoMensalista").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if ($('#TabelaPrecoMensalistaSelecionada').val() !== '') {
                $('#cbContratoMensalista_TabelaPrecoMensalista').val($('#TabelaPrecoMensalistaSelecionada').val());
            }

            if (tabelaIdSelected) {
                $("#cbContratoMensalista_TabelaPrecoMensalista").val(tabelaIdSelected);
            }

            MakeChosen("cbContratoMensalista_TabelaPrecoMensalista");            
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

function FiltrarCampos(pagina = 1) {
    var contrato = $("#contratoBusca").val();
    var nome = $("#nomeBusca").val();

    $.ajax({
        url: "/ContratoMensalista/FiltrarCampos",
        type: "POST",
        dataType: "json",
        data: {
            contrato,
            nome,
            pagina
        },
        success: function (result) {
            if (typeof (result.resultado) === "object" && result.resultado.Titulo !== null && result.resultado.Titulo !== "") {
                openCustomModal(null,
                    null,
                    result.resultado.TipoModal,
                    result.resultado.Titulo,
                    result.resultado.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#lista-contrato-mensalista").empty();
                $("#lista-contrato-mensalista").append(result.grid);
            }
        },
        error: function (error) {
            $("#lista-contrato-mensalista").empty();
            toastr.error("Ocorreu um erro ao fazer a pesquisa, tente novamente.", "Erro");
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function EfetuarPagamentoCadastro() {
    $("#hdnPagamentoCadastro").val("true");
    $("#hdnPagamentoCadastroCliente").val("true");
    $("#myModal").modal('hide');
    $("#salvar").click();
}