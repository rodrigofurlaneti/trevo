enableMobileWidgets = true;

$(document).ready(function () {
    $("#campo-supervisor").change(function () {
        $("#data-admissao").val("");
    });

    let timeout = null;
    $("#lista-funcionario").on("input", ".hasinput input", function (event) {
        clearTimeout(timeout);

        timeout = setTimeout(function () {
            BuscarFuncionarios();
        }, 500);
    });

    $("#combo-controle-ponto-supervisor").on("change", "#campo-funcionario", function () {
        BuscarDataAdmissao(this.value).done(() => {
            AtualizarGridDiasGridControlePontoUnidadeApoio();
        });
    });

    $("#combo-controle-ponto-supervisor").on("change", "#ano, #mes", function () {
        AtualizarGridDiasGridControlePontoUnidadeApoio();
    });
});