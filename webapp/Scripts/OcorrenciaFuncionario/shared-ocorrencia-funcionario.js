let ocorrenciaFuncionarioDetalheEmEdicacao = false;

$(document).ready(function () {
    UnidadeAutoComplete("ocorrencia-funcionario-unidades", "ocorrencia-funcionario-unidade");
    MakeChosen("ocorrencia-funcionario-tipo-ocorrencia");
    FormatarReal("valmoney");
    FormatarCampoData("ocorrencia-funcionario-data");
    BuscarUsuarioLogado();

    if ($("#ocorrencia-funcionario-valor").length) {
        $("#ocorrencia-funcionario-tipo-ocorrencia").change(function () {
            if (this.value)
                BuscarPercentual(this.value);
            else
                $("#ocorrencia-funcionario-valor").val("0,00");
        });
    }
});

function BuscarPercentual(tipoOcorrenciaId) {
    return get(`BuscarPercentual?tipoOcorrenciaId=${tipoOcorrenciaId}`, "TipoOcorrencia")
        .done((response) => {
            $("#ocorrencia-funcionario-valor").val(response.Percentual);
        }); 
}

let idsAdicionados = 0;
function AdicionarOcorrenciaFuncionarioDetalhe() {
    if (location.pathname.toLowerCase().includes("ocorrenciafuncionario") && !$("#ocorrencia-funcionario-funcionario").val()) {
        toastr.warning("Informe o Funcionário antes de adicionar um registro", "Alerta");
        return false;
    }

    let dataOcorrencia = $("#ocorrencia-funcionario-data").val();
    let tipoOcorrenciaId = $("#ocorrencia-funcionario-tipo-ocorrencia").val();
    let unidadeId = $("#ocorrencia-funcionario-unidade").val();
    let justificativa = $("#ocorrencia-funcionario-justificativa").val();

    if (!dataOcorrencia) {
        toastr.warning("O campo Data Ocorrência é obrigatório", "Alerta");
        return false;
    }
    if (!tipoOcorrenciaId) {
        toastr.warning("O campo Tipo Ocorrência é obrigatório", "Alerta");
        return false;
    }
    if (!unidadeId) {
        toastr.warning("O Local é obrigatório", "Alerta");
        return false;
    }

    idsAdicionados--;
    let id = ocorrenciaFuncionarioDetalheEmEdicacao ? $("#ocorrencia-funcionario-id-em-edicao").val() : idsAdicionados;

    post("AdicionarOcorrenciaFuncionarioDetalhe", { id, dataOcorrencia, justificativa, tipoOcorrenciaId, unidadeId }, "OcorrenciaFuncionario")
        .done((response) => {
            $("#lista-ocorrencia-funcionario-detalhe").empty().append(response.Grid);
            $("#ocorrencia-funcionario-valor-total").text(response.ValorTotal);
            LimparCamposOcorrenciaFuncionario();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });

    ocorrenciaFuncionarioDetalheEmEdicacao = false;
}

function RemoverOcorrenciaFuncionarioDetalhe(id) {
    post("RemoverOcorrenciaFuncionarioDetalhe", { id }, "OcorrenciaFuncionario")
        .done((response) => {
            $("#lista-ocorrencia-funcionario-detalhe").empty().append(response.Grid);
            $("#ocorrencia-funcionario-valor-total").text(response.ValorTotal);
        });
}

function EditarOcorrenciaFuncionarioDetalhe(id) {
    if (ocorrenciaFuncionarioDetalheEmEdicacao) {
        toastr.warning("Já possui um item em edição", "Alerta");
        return;
    }

    post("EditarOcorrenciaFuncionarioDetalhe", { id }, "OcorrenciaFuncionario")
        .done((response) => {
            $("#lista-ocorrencia-funcionario-detalhe").empty().append(response.Grid);
            $("#ocorrencia-funcionario-id-em-edicao").val(response.Item.Id);
            $("#ocorrencia-funcionario-data").val(response.Data);
            $("#ocorrencia-funcionario-justificativa").val(response.Item.Justificativa);
            $("#ocorrencia-funcionario-tipo-ocorrencia").val(response.Item.TipoOcorrencia.Id);
            $("#ocorrencia-funcionario-unidades").val(response.Item.Unidade.Nome);
            $("#ocorrencia-funcionario-unidade").val(response.Item.Unidade.Id);
            $("#ocorrencia-funcionario-valor").val(response.Item.TipoOcorrencia.Percentual);
            $("#ocorrencia-funcionario-valor-total").text(response.ValorTotal);

            MakeChosen("ocorrencia-funcionario-tipo-ocorrencia");
            ocorrenciaFuncionarioDetalheEmEdicacao = true;
        });
}

function LimparCamposOcorrenciaFuncionario() {
    $("#ocorrencia-funcionario-id-em-edicao").val("");
    $("#ocorrencia-funcionario-data").val("");
    $("#ocorrencia-funcionario-justificativa").val("");
    $("#ocorrencia-funcionario-tipo-ocorrencia").val("");
    $("#ocorrencia-funcionario-unidades").val("");
    $("#ocorrencia-funcionario-unidade").val("");
    $("#ocorrencia-funcionario-valor").val("0,00");
    MakeChosen("ocorrencia-funcionario-tipo-ocorrencia");
}

function BuscarUsuarioLogado() {
    return $.get("/OcorrenciaFuncionario/BuscarUsuarioLogado")
        .done((response) => $("#ocorrencia-funcionario-usuario").val(response.Nome))
}