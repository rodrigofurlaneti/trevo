let contaCorrenteClienteEmEdicacao = false;

$(document).ready(function () {
    MakeChosen("conta-corrente-cliente-tipo-operacao");
    FormatarCampoDataMesAno("conta-corrente-cliente-dtCompetencia");
    FormatarReal("valmoney");

    MakeChosen("conta-corrente-contratos");
});

function AdicionarContaCorrenteCliente() {
    let clienteId = $("#conta-corrente-cliente-cliente").val();

    if (location.pathname.toLowerCase().includes("contaCorrenteCliente") && !$("#conta-corrente-cliente-cliente").val()) {
        toastr.warning("Informe o Cliente antes de adicionar um registro", "Alerta");
        return false;
    }

    let tipoOperacaoId = $("#conta-corrente-cliente-tipo-operacao").val();
    let dataReferencia = $("#conta-corrente-cliente-dtCompetencia").val();
    let valor = $("#conta-corrente-cliente-valor").val();

    if (!tipoOperacaoId) {
        toastr.warning("Informe um Tipo de Operação", "Campo Obrigatório");
        return;
    }

    if (!$("#conta-corrente-cliente-dtCompetencia").val()) {
        toastr.error("O campo \"Data de Competência\" deve ser preenchido!");
        return;
    } else if (verificaData("01/" + $("#conta-corrente-cliente-dtCompetencia").val()) === false) {
        toastr.error("O campo \"Data de Competência\" deve ser preenchido com uma data no formato (Mês/Ano) válida!");
        return;
    }

    let contratoId = $("#conta-corrente-contratos").val();
    if (!contratoId) {
        toastr.error('Selecione o Contrato', 'Campo obrigatório');
        return;
    }

    let numeroContrato = $("#conta-corrente-contratos option:selected").text();

    post("AdicionarContaCorrenteCliente", { tipoOperacaoId, dataReferencia, valor, contratoId: contratoId, numeroContrato: numeroContrato }, "ContaCorrenteCliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
            LimparCamposContaCorrenteCliente();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    contaCorrenteClienteEmEdicacao = false;
}

function RemoverContaCorrenteCliente(tipoOperacaoId, dataReferencia) {
    var tipoBeneficioId = tipoOperacaoId;

    post("RemoverContaCorrenteCliente", { tipoBeneficioId, dataReferencia }, "ContaCorrenteCliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
        });
}

function EditarContaCorrenteCliente(tipoOperacaoId, dataReferencia) {
    var tipoBeneficioId = tipoOperacaoId;
    
    if (contaCorrenteClienteEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarContaCorrenteCliente", { tipoBeneficioId, dataReferencia }, "ContaCorrenteCliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
            $("#conta-corrente-cliente-tipo-operacao").val(response.Item.TipoOperacaoContaCorrente);
            $("#conta-corrente-cliente-dtCompetencia").val(moment(response.Item.DataCompetencia).format("MM/YYYY"));
            $("#conta-corrente-cliente-valor").val(response.Item.Valor);
            $("#conta-corrente-contratos").val(response.Item.ContratoMensalista.Id);
            MakeChosen("conta-corrente-cliente-tipo-operacao");
            MakeChosen("conta-corrente-contratos");
            contaCorrenteClienteEmEdicacao = true;
        });
}

function LimparCamposContaCorrenteCliente() {
    $("#conta-corrente-cliente-tipo-operacao").val("");
    $("#conta-corrente-cliente-dtCompetencia").val("");
    $("#conta-corrente-cliente-valor").val("0,00");
    $("#conta-corrente-contratos").val("");
    MakeChosen("conta-corrente-cliente-tipo-operacao");
    MakeChosen("conta-corrente-contratos");
}