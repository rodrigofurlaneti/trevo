$(document).ready(function () {
    FuncionarioAutoComplete("conta-corrente-cliente-clientes", "conta-corrente-cliente-cliente");
    $("#conta-corrente-cliente-clientes").unReadonly();
    BuscarContaCorrenteCliente();

    $("#conta-corrente-cliente-form").submit(function () {
        if (!$("#conta-corrente-cliente-cliente").val()) {
            toastr.warning("Informe o Funcionário", "Campo Obrigatório");
            return false;
        }
    });

    $("#conta-corrente-cliente-cliente").change(function () {
        EditarSeJaExisteContaCorrenteCliente(this.value);
    });

    if (isEdit())
        $("#conta-corrente-cliente-clientes").readonly();
});

function BuscarContaCorrenteCliente() {
    return get("BuscarContaCorrenteCliente", "ContaCorrenteCliente")
        .done((response) => {
            $("#lista-conta-corrente-cliente").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}

function EditarSeJaExisteContaCorrenteCliente(clienteId) {
    return get(`EditarSeJaExiste?clienteId=${clienteId}`, "ContaCorrenteCliente")
        .done((response) => {
            if (response.Existe) {
                $("#conta-corrente-cliente-id").val(response.Id);
            } else {
                $("#conta-corrente-cliente-id").val("");
            }

            $("#lista-conta-corrente-cliente-detalhe").empty().append(response.Grid);
        });
}