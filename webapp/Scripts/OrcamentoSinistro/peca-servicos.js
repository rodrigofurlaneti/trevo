let orcamentoSinistroPecaServicos = [];

$(document).ready(function () {
    if (isEdit() || isSave()) {
        post("BuscarOrcamentoSinistroPecaServicos")
            .done((result) => {
                orcamentoSinistroPecaServicos = result;
            })
    }
});

function atualizarPecaServico(orcamentoSinistroPecaServicos) {
    return post("AtualizarOrcamentoSinistroPecaServicos", { orcamentoSinistroPecaServicos })
        .done((response) => {
            $("#lista-orcamento-sinistro-peca-servicos").empty().append(response);
        });
}

function validarPecaServico({ PecaServico }) {
    if (!PecaServico.Nome) {
        toastr.warning("Informe o nome da Peça", "Alerta");
        return false;
    }
    if (orcamentoSinistroPecaServicos.find(x => x.PecaServico.Nome.toLowerCase() == PecaServico.Nome.toLowerCase())) {
        toastr.warning("Essa Peça já foi adicionada", "Alerta");
        return false;
    }
    return true;
}

function adicionarPecaServico() {
    let orcamentoSinistroPecaServico = {
        PecaServico: {
            Nome: $("#peca-servico").val()
        }
    }

    if (!validarPecaServico(orcamentoSinistroPecaServico))
        return;

    orcamentoSinistroPecaServicos.push(orcamentoSinistroPecaServico);

    atualizarPecaServico(orcamentoSinistroPecaServicos);

    clearFieldsPecaServico();
}

function removerPecaServico(nome) {
    orcamentoSinistroPecaServicos = orcamentoSinistroPecaServicos.filter(x => x.PecaServico.Nome.toLowerCase() != nome.toLowerCase());

    return atualizarPecaServico(orcamentoSinistroPecaServicos);
}

function clearFieldsPecaServico() {
    $('#peca-servico').val("");
}