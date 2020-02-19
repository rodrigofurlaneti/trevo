$(document).ready(function () {
    callbackPaginacao = BuscarControleFerias;
    $(".mascara-data").mask("00/00/0000");

    FormatarCampoDataPelaClasse("campo-data");
    BuscarControleFerias();

    $("#controle-ferias-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
    });

    var timeout = null;

    $("#lista-controle-ferias").on("input", ".hasinput input", function (event) {
        let id = this.id;
        let length = this.value.length;
        clearTimeout(timeout);

        timeout = setTimeout(function () {
            if ((id != "coluna-busca-data-inicial" && id != "coluna-busca-data-final") || length == 0 || length == 10)
                BuscarControleFerias();
        }, 500);
    });

    $("#funcionario").change(function () {
        post("BuscarFuncionarioUnidade", { funcionarioId: this.value })
            .done((response) => {
                $("#unidade").val(response.NomeUnidade);
            });
    });
});

function BuscarControleFerias(pagina = 1) {
    let dataInicial = $("#coluna-busca-data-inicial").val();
    let dataFinal = $("#coluna-busca-data-final").val();

    let dto = {
        NomeFuncionario: $("#coluna-busca-funcionario").val(),
        NomeUnidade: $("#coluna-busca-unidade").val(),
        Mes: $("#coluna-busca-mes").val(),
        Ano: $("#coluna-busca-ano").val(),
        DataInicial: dataInicial.length == 10 ? dataInicial : null,
        DataFinal: dataFinal.length == 10 ? dataFinal : null,
        Trabalhada: $("#coluna-busca-trabalhada").val(),
    }

    return post("BuscarControleFerias", { pagina, dto })
        .done((response) => {
            $(".paginacao").empty().append(response.PartialPaginacao);
            $("#lista-controle-ferias tbody").empty().append(response.Grid);
            FormatarCampoDataPelaClasse("campo-data");
        });
}

function AutorizarTrabalhar(controleFeriasId, autorizado = true) {
    return post("AutorizarTrabalhar", { controleFeriasId, autorizado })
        .done(() => BuscarControleFerias($(".paginate_button.active a:first").text()));
}

function AtualizarTrabalhoDe(controleFeriasId, el) {
    if (!el.value) {
        el.value = el.defaultValue;
        toastr.warning("Informe a Data", "Campo Obrigatório");
        return;
    }

    return post("AtualizarTrabalhoDe", { controleFeriasId, data: el.value })
        .done(() => BuscarControleFerias($(".paginate_button.active a:first").text()));
}

function AtualizarTrabalhoAte(controleFeriasId, el) {
    if (!el.value) {
        el.value = el.defaultValue;
        toastr.warning("Informe a Data", "Campo Obrigatório");
        return;
    }

    return post("AtualizarTrabalhoAte", { controleFeriasId, data: el.value })
        .done(() => BuscarControleFerias($(".paginate_button.active a:first").text()));
}

let controleFeriasIdModal = 0;
function AbrirModalPeriodoPermitido(controleFeriasId) {
    controleFeriasIdModal = controleFeriasId;

    $("#modal-periodo-permitido").modal("show");
}