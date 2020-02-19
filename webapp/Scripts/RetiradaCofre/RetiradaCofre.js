let retiradasCofre = [];

let acao = {
    "Aprovar": 1,
    "Negar": 2
};

$(document).ready(function () {
    FormatarCampoData("dtInicio");
    FormatarCampoData("dtFim");
    
    if (location.search) {
        post("BuscarRetiradaCofrePeloId", { retiradaCofreId: obterParametroDaUrl("retiradaCofreId") })
            .done((response) => {
                $("#retirada-cofre").empty().append(response);
                ConfigTabelaGridSemCampoFiltroPrincipal();
            });
    }

    MakeChosen("departamento", null, "100%");
    MakeChosen("usuario", null, "100%");
    MakeChosen("contaFinanceira", null, "100%");
});

function AdicionarRetiradaSelecionada(componente) {
    var retirada = {
        Id: componente.getAttribute("data-retiradaCofreId")
    };

    if (componente.checked) {
        retiradasCofre.push(retirada);
    } else {
        let index = retiradasCofre.indexOf(retirada);
        retiradasCofre.splice(index, 1);
    }
}

function BuscarRetiradaCofre() {
    var obj = {
        ContaFinanceiraId: $("#contaFinanceira").val(),
        DepartamentoId: $("#departamento").val(),
        UsuarioId: $("#usuario").val(),
        DataInicio: $("#dtInicio").val(),
        DataFim: $("#dtFim").val()
    };

    if (!obj.DepartamentoId && !obj.ContaFinanceiraId && !obj.UsuarioId && !obj.DataInicio && !obj.DataFim) {
        if (location.search)
            location.href = "/retiradacofre/index";
        else
            toastr.error("Preencha pelo menos um campo do filtro", "Filtro Vazio");
    } else if ((obj.DataInicio && !verificaData(obj.DataInicio)) || (obj.DataFim && !verificaData(obj.DataFim))) {
        toastr.error('Os campos de \"Data de Solicitação\" devem ser preenchidos com datas válidas!', 'Data Inválida!');
    } else {
        BuscarPartial("/retiradacofre/BuscarRetiradaCofre", "#retirada-cofre", obj)
            .done(function () {
                ConfigTabelaGridSemCampoFiltroPrincipal();
            })
            .always(() => retiradasCofre = []);
    }
}

function Aprovar() {
    return post("AtualizarStatus", { retiradasCofre, acao: acao["Aprovar"] })
        .done((response) => {
            toastr.success(response.status.StatusDescription, "Sucesso");
            BuscarRetiradaCofre();
            ObterNotificacoes();
        });
}

function Negar() {
    return post("AtualizarStatus", { retiradasCofre, acao: acao["Negar"] })
        .done((response) => {
            toastr.success(response.status.StatusDescription, "Sucesso");
            BuscarRetiradaCofre();
            ObterNotificacoes();
        });
}

function InformacoesRetirada(retiradaCofreId) {
    BuscarPartial("/retiradacofre/Informacoes", "#area-modal-informacoes", { retiradaCofreId })
        .done(() =>
            $("#modalInformacoes").modal('show')
        );
}