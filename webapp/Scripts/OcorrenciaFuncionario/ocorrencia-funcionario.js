$(document).ready(function () {
    FuncionarioAutoComplete("ocorrencia-funcionario-funcionarios", "ocorrencia-funcionario-funcionario");
    $("#ocorrencia-funcionario-funcionarios").unReadonly();
    BuscarOcorrenciaFuncionario();

    $("#ocorrencia-funcionario-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
    });

    $("#ocorrencia-funcionario-funcionario").change(function () {
        EditarSeJaExisteOcorrenciaFuncionario(this.value);
    });

    if (isEdit())
        $("#ocorrencia-funcionario-funcionarios").readonly();
});

function BuscarOcorrenciaFuncionario() {
    return get("BuscarOcorrenciaFuncionario", "OcorrenciaFuncionario")
        .done((response) => {
            $("#lista-ocorrencia-funcionario").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function EditarSeJaExisteOcorrenciaFuncionario(funcionarioId) {
    return get(`EditarSeJaExiste?funcionarioId=${funcionarioId}`, "OcorrenciaFuncionario")
        .done((response) => {
            if (response.Existe) {
                $("#ocorrencia-funcionario-id").val(response.Id);
                $("#ocorrencia-funcionario-valor-total").text(response.ValorTotal);
            } else {
                $("#ocorrencia-funcionario-id").val("");
                $("#ocorrencia-funcionario-valor-total").text("0,00");
            }

            $("#lista-ocorrencia-funcionario-detalhe").empty().append(response.Grid);
        });
}