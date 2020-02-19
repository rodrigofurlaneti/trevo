let solicitacaoPagamentoReembolso = [];

$(document).ready(function () {
    MakeChosen("conta-financeira", null, "100%");
    MakeChosen("departamento", null, "100%");
    MakeChosen("tipo-pagamento", null, "100%");
    FormatarCampoData("data-vencimento");
});

function removeItemDoGrid(componente) {
    var table = $('#datatable_fixed_column').DataTable();
    table.destroy();

    $(componente).closest('tr').remove();
    MetodoUtil();
    toastr.success("Item removido do grid temporariamente com sucesso", "Sucesso");
}

function AdicionarContaSelecionada(componente) {
    let contaSelecionada = {
        ContasAPagarId: componente.getAttribute("data-contaAPagarId")
    };

    if (componente.checked) {
        solicitacaoPagamentoReembolso.push(contaSelecionada);
    } else {
        let index = solicitacaoPagamentoReembolso.indexOf(contaSelecionada);
        solicitacaoPagamentoReembolso.splice(index, 1);
    }
}

function BuscarPartial(url, divId, obj) {
    showLoading();
    return $.post(url, obj)
        .done(function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $(divId).empty();
                $(divId).append(result);
            }
        })
        .fail(function (error) {
            $(divId).empty();
            $(divId).append(error.responseText);
        }).always(function () {
            hideLoading();
        });
}

function Buscar() {

    var obj = {
        ContaFinanceiraId: $("#conta-financeira").val(),
        DepartamentoId: $("#departamento").val(),
        TipoPagamento: $("#tipo-pagamento").val(),
        DataVencimento: $("#data-vencimento").val()
    };

    if (!obj.ContaFinanceiraId && !obj.DepartamentoId && !obj.TipoPagamento && !obj.DataVencimento) {
        toastr.error("Preencha pelo menos um campo do filtro", "Filtro Vazio");
    } else {
        BuscarPartial("/SolicitacaoPagamentoReembolso/Buscar", "#area-grid", obj)
            .done(function () {
                MetodoUtil();
            });
    }
}

function AbrirModalInformarRecibo() {
    if (solicitacaoPagamentoReembolso.length === 0) {
        toastr.error(error.statusText, "Ocorreu um erro ao informar recibo: Nenhuma conta Selecionada");
        return;
    }

    BuscarPartial("/SolicitacaoPagamentoReembolso/AbrirModalInformarRecibos", "#area-modal-informar-recibo", { solicitacaoPagamentoReembolso })
        .done(function () {
            $("#modalInformaRecibo").modal('show');
        });
}

function Solicitar() {
    let contasAPagar = [];
    showLoading();

    $("[data-numero-recibo]").each((index, element) => {
        let conta = {
            Id: element.getAttribute("data-conta-id"),
            NumeroRecibo: element.value
        };

        if (conta.NumeroRecibo !== "") {
            contasAPagar.push(conta);
        }
    });
    
    if ($("[data-numero-recibo]").length > contasAPagar.length) {
        toastr.error("Atenção", "É necessário preencher todos os campos com os números de recibo");
        hideLoading();
        return;
    }

    $.post("/SolicitacaoPagamentoReembolso/SalvarSolicitacoes", { contasAPagar })
        .done((result) => {
            toastr.success(result.status.StatusDescription, "Sucesso");
        })
        .fail((error) => {
            toastr.error(error.statusText, "Erro ao Salvar Recibos");
        }).always(() => {
            $("#modalInformaRecibo").modal('hide');
            Buscar();
            solicitacaoPagamentoReembolso.length = 0;
            hideLoading();
        });
}