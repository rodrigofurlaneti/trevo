$(document).ready(function () {
    callbackPaginacao = BuscarFolhaPresenca;

    MakeChosen("ano");
    MakeChosen("mes");
});

function BuscarFolhaPresenca(pagina = 1, novabusca = false) {
    let supervisorId = $("#supervisor").val();
    let funcionarioId = $("#funcionario").val();

    return post("BuscarFolhaPresenca", { supervisorId, funcionarioId, pagina, novabusca})
        .done((response) => {
            $("#lista-funcionarios").empty().append(response);
        });
}

function Imprimir() {
    return ArmazenarDadosImpressao()
        .done(() => {
            document.getElementById("botao-impressao").click();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
        });
}

function ArmazenarDadosImpressao() {
    let ano = $("#ano").val();
    let mes = $("#mes").val();
    let observacao = $("#observacao").val();

    return post("ArmazenarDadosImpressao", { ano, mes, observacao });
}

function AltenarSelecionarTudo(el) {
    let selecionado = el.checked;

    return post("AltenarSelecionarTudo", { selecionado })
        .done(() => {
            if (selecionado)
                $(".item-checkbox").checked();
            else
                $(".item-checkbox").unChecked();
        })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
            $(el).unChecked();
        });
}

function AlternarItemSelecionado(funcionarioId, el) {
    return post("AlternarItemSelecionado", { funcionarioId })
        .fail((error) => {
            toastr.warning(error.statusText, "Alerta");
            $(el).unChecked();
        });
}