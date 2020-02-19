let orcamentoSinistroOficinas = [];

$(document).ready(function () {
    MakeChosen("oficina", null, "100%");

    if (isEdit() || isSave()) {
        post("BuscarOrcamentoSinistroOficinas")
            .done((result) => {
                orcamentoSinistroOficinas = result;
            })
    }
});

function atualizarOficina(orcamentoSinistroOficinas) {
    return post("AtualizarOrcamentoSinistroOficinas", { orcamentoSinistroOficinas })
        .done((response) => {
            $("#lista-orcamento-sinistro-oficinas").empty().append(response);
        });
}

function validarOficina({ Oficina }) {
    if (!Oficina.Id) {
        toastr.warning("Selecione uma Oficina", "Alerta");
        return false;
    }
    if (orcamentoSinistroOficinas.find(x => x.Oficina.Id == Oficina.Id)) {
        toastr.warning("Essa Oficina já foi adicionada", "Alerta");
        return false;
    }
    return true;
}

function adicionarOficina() {
    let oficina = {
        Oficina: {
            Id: $("#oficina").val(),
            Nome: $("#oficina option:selected").text()
        }
    }

    if (!validarOficina(oficina))
        return;

    orcamentoSinistroOficinas.push(oficina);

    atualizarOficina(orcamentoSinistroOficinas);

    clearFieldsOficina();
}

function removerOficina(id) {
    orcamentoSinistroOficinas = orcamentoSinistroOficinas.filter(x => x.Oficina.Id != id);

    return atualizarOficina(orcamentoSinistroOficinas);
}

function clearFieldsOficina() {
    $('#oficina').val("");
    MakeChosen("oficina", null, "100%");
}