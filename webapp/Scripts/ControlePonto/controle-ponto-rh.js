enableMobileWidgets = true;

$(document).ready(function () {
    callbackPaginacao = BuscarFuncionarios;

    UnidadeAutoComplete("busca-unidades", "busca-unidade");
    FuncionarioAutoComplete("busca-funcionarios", "busca-funcionario");
    SupervisorAutoComplete("busca-supervisores", "busca-supervisor");


    let timeout = null;
    $("#lista-funcionario").on("input", ".hasinput input", function (event) {
        clearTimeout(timeout);

        timeout = setTimeout(function () {
            BuscarFuncionarios();
        }, 500);
    });

    if (location.pathname.toLowerCase().includes("editar")) {
        let ano = obterParametroDaUrl("ano");
        let mes = obterParametroDaUrl("mes");

        if (ano) {
            $(".ano").val(ano);
            MakeChosenPelaClasse("ano");
        }
        if (mes) {
            $(".mes").val(mes);
            MakeChosenPelaClasse("mes");
        }

        if (ano || mes) {
            AtualizarGridDiasGridControlePontoUnidadeApoio()
                .done(() => AtualizarDadosTotais());
        }
    }

    $(".ano").change(function () {
        $(".ano").val(this.value);
        MakeChosenPelaClasse("ano");
    });

    $(".mes").change(function () {
        $(".mes").val(this.value);
        MakeChosenPelaClasse("mes");
    });

    $(".ano, .mes").change(function () {
        AtualizarGridDiasGridControlePontoUnidadeApoio()
            .done(() => AtualizarDadosTotais());
    });
});

function BuscarFuncionarios(pagina = 1) {
    let busca = {
        UnidadeId: $("#busca-unidade").val(),
        SupervisorId: $("#busca-supervisor").val(),
        FuncionarioId: $("#busca-funcionario").val(),
        ColunaUnidade: $("#coluna-busca-unidade").val(),
        ColunaSupervisor: $("#coluna-busca-supervisor").val(),
        ColunaFuncionario: $("#coluna-busca-funcionario").val(),
        Ano: $("#busca-ano").val(),
        Mes: $("#busca-mes").val(),
    }

    post("BuscarFuncionarios", { pagina, busca })
        .done((response) => {
            $(".paginacao").empty().append(response.PartialPaginacao);
            $("#lista-funcionario tbody").empty().append(response.Grid);
        });
}

function AtualizarDadosTotais() {
    let funcionarioId = $("#campo-funcionario").val();

    return post("AtualizarDadosTotais", { funcionarioId })
        .done((response) => {
            $("#total-intervalo-pendente").val(response.IntervalosPendentes);
            $("#total-he-sessenta-cinco").val(response.TotalHoraExtraSessentaCinco);
            $("#total-asn").val(response.TotalAdicionalNoturno);
            $("#total-falta").val(response.TotalFalta);
            $("#total-atrasos").val(response.TotalAtraso);
            $("#total-he-cem").val(response.TotalHoraExtraCem);
            $("#total-feriado").val(response.TotalFeriadosTrabalhados);
        });
}