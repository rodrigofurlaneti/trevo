let orcamentoSinistroFornecedores = [];

$(document).ready(function () {
    MakeChosen("fornecedor", null, "100%");

    if (isEdit() || isSave()) {
        post("BuscarOrcamentoSinistroFornecedores")
            .done((result) => {
                orcamentoSinistroFornecedores = result;
            })
    }
});

function atualizarFornecedor(orcamentoSinistroFornecedores) {
    return post("AtualizarOrcamentoSinistroFornecedores", { orcamentoSinistroFornecedores })
        .done((response) => {
            $("#lista-orcamento-sinistro-fornecedores").empty().append(response);
        });
}

function validarFornecedor({ Fornecedor }) {
    if (!Fornecedor.Id) {
        toastr.warning("Selecione um Fornecedor", "Alerta");
        return false;
    }
    if (orcamentoSinistroFornecedores.find(x => x.Fornecedor.Id == Fornecedor.Id)) {
        toastr.warning("Essa Fornecedor já foi adicionado", "Alerta");
        return false;
    }
    return true;
}

function adicionarFornecedor() {
    let orcamentoSinistroFornecedor = {
        Fornecedor: {
            Id: $("#fornecedor").val(),
            Nome: $("#fornecedor option:selected").text()
        }
    }

    if (!validarFornecedor(orcamentoSinistroFornecedor))
        return;

    orcamentoSinistroFornecedores.push(orcamentoSinistroFornecedor);

    atualizarFornecedor(orcamentoSinistroFornecedores);

    clearFieldsFornecedor();
}

function removerFornecedor(id) {
    orcamentoSinistroFornecedores = orcamentoSinistroFornecedores.filter(x => x.Fornecedor.Id != id);

    return atualizarFornecedor(orcamentoSinistroFornecedores);
}

function clearFieldsFornecedor() {
    $('#fornecedor').val("");
    MakeChosen("fornecedor", null, "100%");
}