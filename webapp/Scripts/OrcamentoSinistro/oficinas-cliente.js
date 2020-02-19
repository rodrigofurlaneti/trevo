let orcamentoSinistroOficinasCliente = [];

$(document).ready(function () {
    MakeChosen("oficina-cliente", null, "100%");

    if (isEdit() || isSave()) {
        post("BuscarOrcamentoSinistroOficinasCliente")
            .done((result) => {
                orcamentoSinistroOficinasCliente = result;
            })
    }
});

function atualizarOficinaCliente(orcamentoSinistroOficinasCliente) {
    return post("AtualizarOrcamentoSinistroOficinasCliente", { orcamentoSinistroOficinasCliente })
        .done((response) => {
            $("#lista-orcamento-sinistro-oficinas-cliente").empty().append(response);
        });
}

function validarOficinaCliente({ Oficina }) {
    if (!Oficina.Id) {
        toastr.warning("Selecione uma Oficina", "Alerta");
        return false;
    }
    if (orcamentoSinistroOficinasCliente.find(x => x.Oficina.Id == Oficina.Id)) {
        toastr.warning("Essa Oficina já foi adicionada", "Alerta");
        return false;
    }
    return true;
}

function adicionarOficinaCliente() {
    let oficinaCliente = {
        Oficina: {
            Id: $("#oficina-cliente").val(),
            Nome: $("#oficina-cliente option:selected").text()
        }
    }

    if (!validarOficinaCliente(oficinaCliente))
        return;

    orcamentoSinistroOficinasCliente.push(oficinaCliente);

    atualizarOficinaCliente(orcamentoSinistroOficinasCliente);

    clearFieldsOficinaCliente();
}

function removerOficinaCliente(id) {
    orcamentoSinistroOficinasCliente = orcamentoSinistroOficinasCliente.filter(x => x.Oficina.Id != id);

    return atualizarOficinaCliente(orcamentoSinistroOficinasCliente);
}

function clearFieldsOficinaCliente() {
    $('#oficina-cliente').val("");
    MakeChosen("oficina-cliente", null, "100%");
}