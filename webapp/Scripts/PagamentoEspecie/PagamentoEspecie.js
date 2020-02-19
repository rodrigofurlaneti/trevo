let pagamentoEspecie = [];

$(document).ready(function () {
    FormatarCampoData("data-vencimento");
    MakeChosen("departamento", null, "100%");
    MakeChosen("tipo-pagamento", null, "100%");
    MakeChosen("unidade", null, "100%");
    MakeChosen("conta-financeira", null, "100%");

    if (location.search) {
        post("BuscarPagamentoEspeciePeloIdDaConta", { contaPagarId: obterParametroDaUrl("contaPagarId") })
            .done((response) => {
                $("#pagamento-especie").empty().append(response);
                ConfigTabelaGridSemCampoFiltroPrincipal();
            });
    }
});

function AdicionarContaSelecionada(componente) {
    var contaSelecionada = {
        ContasAPagarId: componente.getAttribute("data-contaAPagarId"),
        DepartamentoId: componente.getAttribute("data-departamentoId"),
        PendentePagamento: componente.getAttribute("data-pendente-pagamento")
    };

    if (componente.checked) {
        pagamentoEspecie.push(contaSelecionada);
    } else {
        pagamentoEspecie.splice(pagamentoEspecie.findIndex(v => v.ContasAPagarId === contaSelecionada.ContasAPagarId), 1);
    }

    if (pagamentoEspecie.some(x => x.PendentePagamento == "true")) {
        $("#solicitar-valor").hide();
        $("#solicitar-valor-disabled").show();
    }
    else {
        $("#solicitar-valor-disabled").hide();
        $("#solicitar-valor").show();
    }
}


function BuscarPagamentoEspecie() {
    pagamentoEspecie = [];

    var obj = {
        DepartamentoId: $("#departamento").val(),
        TipoPagamento: $("#tipo-pagamento").val(),
        UnidadeId: $("#unidade").val(),
        DataVencimento: $("#data-vencimento").val(),
        ContaFinanceiraId: $("#conta-financeira").val()
    };

    if (!obj.DepartamentoId && !obj.TipoPagamento && !obj.UnidadeId && !obj.DataVencimento && !obj.ContaFinanceiraId) {
        if (location.search)
            location.href = "/pagamentoespecie/index";
        else
        toastr.error("Preencha pelo menos um campo do filtro", "Filtro Vazio");
    } else if (obj.DataVencimento && verificaData(obj.DataVencimento) === false) {
        toastr.error("O campo \"Data de Vencimento\" deve ser preenchido com uma data válida!");
    } else {
        BuscarPartial("/pagamentoespecie/BuscarPagamentoEspecie", "#pagamento-especie", obj)
            .done(function () {
                ConfigTabelaGridSemCampoFiltroPrincipal();
            });
    }
}


function InformarObservacoes() {

    if (pagamentoEspecie.length === 0) {
        toastr.warning("Selecione pelo menos uma conta", "Problema ao Solitar o Valor");
        return;
    }

    BuscarPartial("/pagamentoespecie/InformarObservacoes", "#area-modal-observacoes")
        .done(function () {
            $("#modalObservacoes").modal('show');
        });
}


function SolicitarRetirada() {
    if (pagamentoEspecie.length === 0) {
        toastr.warning("Selecione pelo menos uma conta", "Problema ao Solitar o Valor");
        return;
    }

    var observacoes = document.getElementById("#observacoes").value;

    return post("SolicitarRetirada", { pagamentoEspecie, observacoes})
        .done((response) => {
            toastr.success(response.status.StatusDescription, "Sucesso");
        })
        .fail((error) => toastr.error(error.statusText, "Erro ao Solitar o Valor"))
        .always(() => {
            BuscarPagamentoEspecie();
            ObterNotificacoes();
        });
}

function InformarRecibo() {
    if (pagamentoEspecie.length === 0) {
        toastr.warning("Selecione pelo menos uma conta", "Campos");
        return;
    }
        
    BuscarPartial("/pagamentoespecie/InformarValor", "#area-modal-informar-recibo", { pagamentoEspecie })
        .done(function () {
            $("#modalInformaRecibo").modal('show');
        });
}


function SalvarRecibos() {
    let contasAPagar = [];
    showLoading();

    $("[data-numero-recibo]").each((index, element) => {
        let conta = {
            Id: element.getAttribute("data-conta-id"),
            NumeroRecibo: element.value
        };

        contasAPagar.push(conta);
    });

    $.post("/pagamentoespecie/SalvarRecibos", { contasAPagar })
        .done((result) => {
            toastr.success(result.status.StatusDescription, "Sucesso");
        })
        .fail((error) => {
            toastr.error(error.statusText, "Erro ao Salvar Recibos");
        }).always(() => {
            $("#modalInformaRecibo").modal('hide');
            BuscarPagamentoEspecie();
            ObterNotificacoes();
            hideLoading();
        });
}