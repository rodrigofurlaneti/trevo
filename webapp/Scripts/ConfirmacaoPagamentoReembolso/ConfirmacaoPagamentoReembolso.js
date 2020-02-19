let lancamentosReembolso = [];

$(document).ready(function () {
   
    FormatarCampoData("data");
    MakeChosen("unidade", null, "100%");
    MakeChosen("departamento", null, "100%");
});

function BuscaPagamentoReembolso() {

    $.ajax({
        url: '/confirmacaopagamentoreembolso/BuscaPagamentoReembolso',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            CarregaGridPagamentosReembolso(response);
        }
    });
}


function AdicionarRetiradaSelecionada(componente) {
    var PagamentoReembolsoViewModel = {
        Id: componente.getAttribute("data-lancamentoreembolsoid")
    };

    if (componente.checked) {
        lancamentosReembolso.push(PagamentoReembolsoViewModel);
    } else {
        let index = lancamentosReembolso.indexOf(PagamentoReembolsoViewModel);
        lancamentosReembolso.splice(index, 1);
    }
}

function Pesquisar() {
    lancamentosReembolso.length = 0;

    var unidade = $("#unidade").val();
    var Data = $("#data").val();
    var departamento = $("#departamento").val();
    var numerorecibo = $("#numerorecibo").val();
    
    if (!unidade && !Data && !departamento && !numerorecibo) {
        toastr.error("Preencha pelo menos um campo do filtro", "Filtro Vazio");
        return;
    } else if (Data && verificaData(Data) === false) {
        toastr.error("O campo \"Data de Solicitação\" deve ser preenchido com uma data válida!");
        return;
    }

    var filtro = {

        DataInsercao: Data,
        Numerorecibo: numerorecibo,
        ContasAPagarViewModel: {
            Departamento: { Id: departamento },
            Unidade: { Id: unidade }
        }
    };
    
    BuscarPartial("/confirmacaopagamentoreembolso/Pesquisar", "#lista-lancamentoCobrancas", filtro)
        .done(function () {
            MetodoUtil();
        });
}

function ConfirmaPagamento() {
    
    if (lancamentosReembolso.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para Confirmar Pagamento");
        return;
    }

    showLoading();

    $.post("/confirmacaopagamentoreembolso/ConfirmarPagamento", { lancamentosReembolso })
        .done(function (result) {
            toastr.success(result.status.StatusDescription, "Sucesso");
        })
        .fail(function (error) {
            toastr.error(error.statusText, "Erro ao Aprovar");
        }).always(function () {
            lancamentosReembolso.length = 0;
            BuscarLancamentosReembolso();
        });
}

function VerificaRecibo() {

    if (lancamentosReembolso.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para verificar recibos.");
        return;
    }

    BuscarPartial("/confirmacaopagamentoreembolso/VerificaRecibo", "#area-modal-verificarecibo", { lancamentosReembolso })
        .done(function () {
            $("#modalInformacoes").modal('show');
        });
}

function NegarPagamento() {

    if (lancamentosReembolso.length <= 0) {
        toastr.error(error.statusText, "Selecione ao menos 1 registro para Negar Pagamento");
        return;
    }

    showLoading();

    $.post("/confirmacaopagamentoreembolso/NegarPagamento", { lancamentosReembolso })
        .done(function (result) {
            toastr.success(result.status.StatusDescription, "Sucesso");
        })
        .fail(function (error) {
            toastr.error(error.statusText, "Erro ao Negar");
        }).always(function () {
            lancamentosReembolso.length = 0;
            BuscarLancamentosReembolso();
        });
}


function BuscarLancamentosReembolso() {

    showLoading();

    var unidade = $("#unidade").val();
    var Data = $("#data").val();
    var departamento = $("#departamento").val();
    var numerorecibo = $("#numerorecibo").val();
    
    var obj = {
        Unidade: unidade,
        DataInsercao: Data,

        ContasAPagarViewModel: {
            Departamento: {
                Id: departamento
            }
        },
        Numerorecibo: numerorecibo
    };
    
    BuscarPartial("/confirmacaopagamentoreembolso/BuscarLancamentosReembolso", "#lista-lancamentoCobrancas", obj)
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
    
}