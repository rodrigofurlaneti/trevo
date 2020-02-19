var ocorrencias = [];
var indiceOcorrenciaAdicionados = 0;
var isEditOcorrencia = false;

$(document).ready(function () {
    ocorrencias = [];
    FormatarCampoData("data-ocorrencia");
    desabilitarCampo("ocorrencia-cliente-numero-protocolo");
    desabilitarCampo("ocorrencia-cliente-data-competencia");

    ClienteAutoComplete("clientes", "cliente", "clienteText", CarregarDadosPorCliente);

    callbackPaginacao = BuscarOcorrenciasCliente;

    if (isEdit() || isSave()) {
        if ($('#cliente').val()) {
            CarregarVeiculos();
            CarregarUnidades($('#cliente').val());
        }
    }

    $("#OcorrenciaClienteForm").submit(function (e) {       
        if (!ValidarCamposOcorrenciaCliente()) {
            e.preventDefault();
            return false;
        } 

        if (!ValidarCamposOcorrencia()) { 
            e.preventDefault();
            return false;
        }        

        showLoading();
    });

    $("#btnHideModal").click(function () {
        $("#modal-ocorrencia-cliente").modal('hide');
    }); 

    MakeChosen("ocorrencia-cliente-unidade");
    MakeChosen("ocorrencia-cliente-placa");

    buscarOcorrenciasPorClienteId();
    
});

function ValidarCamposOcorrenciaCliente() {
    var dadosValidos = true;

    if ($("#cliente").val() === "") {
        toastr.error('Informe um Cliente', 'Campo obrigatório');
        dadosValidos = false;
    }

    return dadosValidos;

}

function ValidarCamposOcorrencia() {
    var dadosValidos = true;

    if ($("#ocorencia-origem").val() === "") {
        toastr.error('Selecione uma Origem', 'Campo obrigatório');
        dadosValidos = false;
    }

    if ($("#funcionario").val() === "") {
        toastr.error('Informe a Pessoa Atribuida', 'Campo obrigatório');
        dadosValidos = false;
    }

    if ($("#data-ocorrencia").val() === "") {
        toastr.error('Informe a Data', 'Campo obrigatório');
        dadosValidos = false;
    }

    if ($("#ocorencia-natureza").val() === "") {
        toastr.error('Selecione a Natureza', 'Campo obrigatório');
        dadosValidos = false;
    }
    

    if ($("#ocorencia-prioridade").val() === "") {
        toastr.error('Selecione uma Prioridade', 'Campo obrigatório');
        dadosValidos = false;
    }

    if ($("#ocorencia-status").val() === "") {
        toastr.error('Selecione um Status', 'Campo obrigatório');
        dadosValidos = false;
    }

    if ($("#descricao-ocorrencia").val() === "") {
        toastr.error('Informe a Descrição', 'Campo obrigatório');
        dadosValidos = false;
    }

    return dadosValidos;
    
}

function BuscarClienteVeiculos() {
    return get("BuscarClienteVeiculos")
        .done((response) => {
            veiculos = response;
        });
}


function BuscarOcorrenciasCliente(pagina = 1) {
    var protocolo = $("#protocoloBusca").val();
    var nome = $("#nomeBusca").val();
    var status = $("#statusBusca").val() == 0 ? "" : $("#statusBusca").val();

    $.ajax({
        url: "/OcorrenciaCliente/BuscarOcorrenciasCliente",
        type: "POST",
        dataType: "json",
        data: {
            protocolo,
            nome,
            status,
            pagina
        },
        success: function (result) {
            $("#protocolo").val(result.documentoFormatado);

            if (typeof result.resultado === "object" && result.resultado.Titulo !== null && result.resultado.Titulo !== "") {
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
                $("#lista-ocorrencias").empty();
                $("#lista-ocorrencias").append(result.grid);
            }
        },
        error: function (error) {
            $("#lista-ocorrencias").empty();
            toastr.error("Ocorreu um erro ao fazer a busca das ocorrências, tente novamente.", "Atenção");
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

function CarregarDadosPorCliente() {
    CarregarVeiculos();
    CarregarUnidades($('#cliente').val());
}

function CarregarUnidades(clienteId) {
    $("#ocorrencia-cliente-unidade").empty();

    $.ajax({
        type: 'POST',
        url: '/ContratoMensalista/CarregarUnidades',
        data: { clienteId: clienteId !== undefined && clienteId !== null && clienteId !== "" ? clienteId : 0 },
        success: function (result) {
            $.each(result,
                function (i, result) {
                    if (i === 0)
                        $("#ocorrencia-cliente-unidade").append('<option value="">' + result.Text + '</option>');
                    else
                        $("#ocorrencia-cliente-unidade").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });

            if ($('#ocorrencia-cliente-unidade-selecionada').val() !== '') {
                $('#ocorrencia-cliente-unidade').val($('#ocorrencia-cliente-unidade-selecionada').val());
            } else {
                $('#ocorrencia-cliente-unidade').val("");
            }

            MakeChosen("ocorrencia-cliente-unidade");
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

function CarregaVeiculos(veiculos) {
    temVeiculosParaSelecionar = veiculos && veiculos.length ? true : false;
    var veiculosSelect = document.getElementById("ocorrencia-cliente-placa");
    veiculosSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione a placa";
    option.value = 0;
    veiculosSelect.options.add(option);
    $.each(veiculos, function (i, item) {
        option = document.createElement("option");
        option.text = item.Placa;
        option.value = item.Id;
        veiculosSelect.options.add(option);
    });

    if ($('#placa-selecionada').val() !== '') {
        $('#ocorrencia-cliente-placa').val($('#placa-selecionada').val());
    } else {
        $('#ocorrencia-cliente-placa').val("");
    }

    MakeChosen("ocorrencia-cliente-placa");
}

function AdicionarOcorrencia() {

    indiceOcorrenciaAdicionados--;

    if (!ValidarCamposOcorrencia()) {
        return;
    } 
    
    var ocorrencia = {
        Id: isEditOcorrencia ? $("#hdnOcorrenciaCliente").val() : indiceOcorrenciaAdicionados,
        NumeroProtocolo: $("#ocorrencia-cliente-numero-protocolo").val(),
        DataInsercao: isEditOcorrencia ? $("#hdn-ocorrencia-cliente-data-competencia").val() : moment(new Date()).format("DD/MM/YYYY"), 
        Origem: $("#ocorencia-origem").val(),        
        FuncionarioAtribuido: {
            Pessoa: { Id: $("#funcionario").val(), Nome: $("#funcionarioText").val() }
        },
        DataOcorrencia: $("#data-ocorrencia").val(),
        Natureza: $("#ocorencia-natureza").val(),
        Prioridade: $("#ocorencia-prioridade").val(),
        StatusOcorrencia: $("#ocorencia-status").val(),
        Descricao: $("#descricao-ocorrencia").val(),
        Solucao: $("#ocorrencia-solucao").val()

    }

    if ($("#unidade-ocorrencia").val() !== "" && $("#unidade-ocorrencia").val() !== null) {
        ocorrencia.Unidade = { Id: $("#unidade-ocorrencia").val(), Nome: $("#unidade-ocorrencia option:selected").text() };
    }

    if ($("#ocorrencia-cliente-placa").val() !== "" && $("#ocorrencia-cliente-placa").val() !== null) {
        ocorrencia.Veiculo = { Id: $("#ocorrencia-cliente-placa").val(), Placa: $("#ocorrencia-cliente-placa option:selected").text() };
    }

    debugger;
    var ocorrenciaEdit = ocorrencias.find(x => x.Id == ocorrencia.Id);
    if (ocorrenciaEdit == undefined || ocorrenciaEdit == null) {
        ocorrencias.push(ocorrencia);
    }
    else {
        ocorrencias = ocorrencias.filter(x => x.Id != ocorrencia.Id);
        ocorrencias.push(ocorrencia);
    }
    
    atualizaOcorrencia();

    LimparCamposOcorrencia();
    $("#modal-ocorrencia-cliente").modal('hide');
}


function SalvarOcorrencia() {
    debugger;
    if (!ValidarCamposOcorrenciaCliente()) {
        return;
    }

    if (!ValidarCamposOcorrencia()) {
        return;
    }   

    var ocorrencia = {
        Id: $("#hdnOcorrenciaCliente").val(),
        NumeroProtocolo: $("#ocorrencia-cliente-numero-protocolo").val(),
        DataInsercao: moment(new Date()).format("DD/MM/YYYY"),
        Origem: $("#ocorencia-origem").val(),
        FuncionarioAtribuido: {
            Pessoa: { Id: $("#funcionario").val(), Nome: $("#funcionarioText").val() }
        },
        DataOcorrencia: $("#data-ocorrencia").val(),
        Natureza: $("#ocorencia-natureza").val(),
        Prioridade: $("#ocorencia-prioridade").val(),
        StatusOcorrencia: $("#ocorencia-status").val(),
        Descricao: $("#descricao-ocorrencia").val(),
        Solucao: $("#ocorrencia-solucao").val(),
        Cliente: { Id: $("#cliente").val()}
    }

    if ($("#ocorrencia-cliente-unidade").val() !== "" && $("#ocorrencia-cliente-unidade").val() !== null) {
        ocorrencia.Unidade = { Id: $("#ocorrencia-cliente-unidade").val(), Nome: $("#ocorrencia-cliente-unidade option:selected").text() };
    }

    if ($("#ocorrencia-cliente-placa").val() !== "" && $("#ocorrencia-cliente-placa").val() !== null) {
        ocorrencia.Veiculo = { Id: $("#ocorrencia-cliente-placa").val(), Placa: $("#ocorrencia-cliente-placa option:selected").text() };
    }

    $.ajax({
        type: 'POST',
        url: '/OcorrenciaCliente/SalvarDadosOcorrencia',
        dataType: 'json',
        data: { jsonModel: JSON.stringify(ocorrencia) },
        success: function (result) {
            debugger;
            if (typeof result === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }

            LimparCamposOcorrencia();
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

function atualizaOcorrencia() {
    showLoading();
    $.post('/OcorrenciaCliente/AdicionarOcorrencia', { jsonOcorrencias: JSON.stringify(ocorrencias) })
        .done(() => {
            CarregaOcorrencias();
            isEditOcorrencia = false;
        })
        .always(() => { hideLoading(); });
}

function formatarDatas() {
    ocorrencias.forEach(function (ocorrencia) {
        ocorrencia.DataCompetencia = moment(ocorrencia.DataCompetencia).format("DD/MM/YYYY");
        ocorrencia.DataInsercao = moment(ocorrencia.DataInsercao).format("DD/MM/YYYY");
        ocorrencia.DataOcorrencia = moment(ocorrencia.DataOcorrencia).format("DD/MM/YYYY");
    })
}

function buscarOcorrenciasPorClienteId() {
    showLoading();  
    ocorrencias = [];
    $.ajax({
        type: 'GET',
        url: '/OcorrenciaCliente/BuscarOcorrenciasPorClienteId',
        //dataType: 'json',
        data: {},
        success: function (response) {
            if (response) {
                ocorrencias = response.ocorrenciaLista;
                formatarDatas();
                CarregaOcorrencias();                
                hideLoading();
            }
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function CarregaOcorrencias() {

    $('#gridOcorrencias tbody').empty();
    $.each(ocorrencias, function (i, ocorrencia) {

        $('#gridOcorrencias tbody')
            .append('<tr>'
                + '<td style="display:none">'
                + ocorrencia.Id
                + '</td>'
                //+ '<td style="display:none">'
                //+ ocorrencia.Unidade.Id
                //+ '</td>'
                + '<td col-xs-3">'
                + ocorrencia.DataOcorrencia
                + '</td>'
            + '<td col-xs-3">'
            + ocorrencia.FuncionarioAtribuido.Pessoa.Nome
                + '</td>'
                + '<td col-xs-3>'            
                + ocorrencia.Descricao
                + '</td>'                
                + '<td class="col-xs-1">'
                + '<span class="btn btn-primary" style="margin-right: 10px;" onclick="EditarOcorrencia(' + ocorrencia.Id + ')")><i class="fa fa-edit"></i></span>'
                + '<span class="btn btn-danger" onclick="RemoverOcorrencia(' + ocorrencia.Id + ')"><i class="fa fa-remove"></i></span>'
                + '</td>'
                + '</tr>'
            );
    });
}

function LimparCamposOcorrencia() {
    
    $("#ocorencia-origem").val("3"); //default 3
    $("#data-ocorrencia").val("");
    $("#ocorencia-natureza").val("3");//default 3
    $("#ocorencia-prioridade").val("1");//default 1
    $("#ocorencia-status").val("1");//default 1
    $("#descricao-ocorrencia").val("");
    $("#ocorrencia-solucao").val("");
    $("#unidade-ocorrencia").val("");
    $("#ocorrencia-cliente-placa").val("");
    $("#funcionarios").val("");
    $("#funcionario").val("");
    $("#funcionarioText").val("");
    $("#clientes").val("");
    $('#ocorrencia-cliente-data-competencia').val("");
    $('#hdn-ocorrencia-cliente-data-competencia').val("");
    $('#hdnOcorrenciaCliente').val("");
    

    $("#ocorrencia-cliente-unidade").val("");
    MakeChosen("unidade-ocorrencia");
    MakeChosen("ocorrencia-cliente-unidade");
    MakeChosen("ocorrencia-cliente-placa");
}

function EditarOcorrencia(ocorrenciaId) {

    isEditOcorrencia = true;
    LimparCamposOcorrencia();

    var ocorrencia = ocorrencias.find(x => x.Id == ocorrenciaId);
    $("#id-ocorrencia").val(ocorrencia.Id);

    if (ocorrencia.Unidade !== null && ocorrencia.Unidade !== undefined) {
        $("#unidade-ocorrencia").val(ocorrencia.Unidade.Id);
    }
    else {
        $("#unidade-ocorrencia").val("");
    }    
    MakeChosen("unidade-ocorrencia");

    if (ocorrencia.Veiculo !== null && ocorrencia.Veiculo !== undefined) {
        $("#ocorrencia-cliente-placa").val(ocorrencia.Veiculo.Id);
    }
    else {
        $("#ocorrencia-cliente-placa").val("");
    }
    MakeChosen("ocorrencia-cliente-placa");

    debugger;

    $("#ocorrencia-cliente-numero-protocolo").val(ocorrencia.NumeroProtocolo);
    $("#ocorrencia-cliente-data-competencia").val(ocorrencia.DataInsercao);
    $("#hdn-ocorrencia-cliente-data-competencia").val(ocorrencia.DataInsercao);
    $("#ocorencia-origem").val(ocorrencia.Origem);
    $("#data-ocorrencia").val(ocorrencia.DataOcorrencia);
    $("#ocorencia-natureza").val(ocorrencia.Natureza);
    $("#ocorencia-prioridade").val(ocorrencia.Prioridade);
    $("#ocorencia-status").val(ocorrencia.StatusOcorrencia);
    $("#descricao-ocorrencia").val(ocorrencia.Descricao);
    $("#ocorrencia-solucao").val(ocorrencia.Solucao);
    $("#funcionarios").val(ocorrencia.FuncionarioAtribuido.Pessoa.Nome); 
    $("#funcionarioText").val(ocorrencia.FuncionarioAtribuido.Pessoa.Nome); 
    $("#funcionario").val(ocorrencia.FuncionarioAtribuido.Pessoa.Id); 
    $("#hdnOcorrenciaCliente").val(ocorrenciaId);    

    ExecutarModalEditarOcorrencia();    
}

function RemoverOcorrencia(id) {
    ocorrencias = ocorrencias.filter(x => x.Id != id);
    atualizaOcorrencia();
}

function desabilitarCampo(campo) {
    $("#" + campo).prop("disabled", true);
}

function RemoverOcorrenciaCliente(id) {
    post("RemoverOcorrenciaCliente", { id }, "OcorrenciaCliente")
        .done((response) => {
            $("#lista-ocorrencias").empty().append(response.Grid);
        });
}

function ExecutarModalAdicionarOcorrencia() {
    isEditOcorrencia = false;
    LimparCamposOcorrencia();
    gerarProtocolo();
    $("#modal-ocorrencia-cliente").modal({ backdrop: 'static', keyboard: false });    
}

function ExecutarModalEditarOcorrencia() {
    $("#modal-ocorrencia-cliente").modal({ backdrop: 'static', keyboard: false });
}

function gerarProtocolo() {
    var today = new Date();
    var day = today.getDate() + "";
    var month = (today.getMonth() + 1) + "";
    var year = today.getFullYear() + "";
    var hour = today.getHours() + "";
    var minutes = today.getMinutes() + "";
    var seconds = today.getSeconds() + "";
    var milliseconds = today.getMilliseconds();

    var protocolo = year + month + day + hour + minutes + seconds + milliseconds;
    $("#ocorrencia-cliente-numero-protocolo").val(protocolo);
    $("#hdn-ocorrencia-cliente-data-competencia").val(day + '/' + month + '/' + year);
    $("#ocorrencia-cliente-data-competencia").val(month + '/' + year);
} 








