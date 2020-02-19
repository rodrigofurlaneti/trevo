$(document).ready(function () {
    FuncionarioAutoComplete("beneficio-funcionario-funcionarios", "beneficio-funcionario-funcionario");
    $("#beneficio-funcionario-funcionarios").unReadonly();
    BuscarBeneficioFuncionario();

    $("#beneficio-funcionario-form").submit(function () {
        if (!$("#beneficio-funcionario-funcionario").val()) {
            toastr.warning("Informe o Funcionário", "Campo Obrigatório");
            return false;
        }
    });

    $("#beneficio-funcionario-funcionario").change(function () {
        EditarSeJaExisteBeneficioFuncionario(this.value);
    });

    if (isEdit())
        $("#beneficio-funcionario-funcionarios").readonly();
});

function BuscarBeneficioFuncionario() {
    return get("BuscarBeneficioFuncionario", "BeneficioFuncionario")
        .done((response) => {
            $("#lista-beneficio-funcionario").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function EditarSeJaExisteBeneficioFuncionario(funcionarioId) {
    return get(`EditarSeJaExiste?funcionarioId=${funcionarioId}`, "BeneficioFuncionario")
        .done((response) => {
            if (response.Existe) {
                $("#beneficio-funcionario-id").val(response.Id);
            } else {
                $("#beneficio-funcionario-id").val("");
            }

            $("#lista-beneficio-funcionario-detalhe").empty().append(response.Grid);
        });
}