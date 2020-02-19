$(document).ready(function () {
    FuncionarioAutoComplete("campo-funcionarios", "campo-funcionario");
    $("#campo-funcionarios").unReadonly();
    BuscarItemFuncionario();

    $("#item-funcionario-form").submit(function () {
        if (!$("#campo-funcionario").val()) {
            toastr.warning("Informe o Funcionário", "Campo Obrigatório");
            return false;
        }
    });

    $("#campo-funcionario").change(function () {
        EditarSeJaExisteItemFuncionario(this.value);
    });

    if (isEdit())
        $("#campo-funcionarios").readonly();
});

function BuscarItemFuncionario() {
    return get("BuscarItemFuncionario", "ItemFuncionario")
        .done((response) => {
            $("#lista-item-funcionario").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function EditarSeJaExisteItemFuncionario(funcionarioId) {
    return get(`EditarSeJaExiste?funcionarioId=${funcionarioId}`, "ItemFuncionario")
        .done((response) => {
            if (response.Existe) {
                $("#item-funcionario-id").val(response.Id);

                $("#data-entrega").val(response.DataEntrega);
                $("#responsaveis-entrega").val(response.ResponsavelEntregaNome);
                $("#responsavel-entrega").val(response.ResponsavelEntregaId);
                $("#data-devolucao").val(response.DataDevolucao);
                $("#responsaveis-devolucao").val(response.ResponsavelDevolucaoNome);
                $("#responsavel-devolucao").val(response.ResponsavelDevolucaoId);
            } else {
                $("#item-funcionario-id").val("");
                $("#data-entrega").val("");
                $("#responsaveis-entrega").val("");
                $("#responsavel-entrega").val("");
                $("#data-devolucao").val("");
                $("#responsaveis-devolucao").val("");
                $("#responsavel-devolucao").val("");
            }

            $("#lista-item").empty().append(response.Grid);
        });
}