$(document).ready(function () {
    BuscarParametroBoletoBancario();

    $("#parametro-boleto-bancario-form").submit(function () {
        if (!ValidarForm(this.id)) {
            return false;
        }
        if ($("#valor-desconto").val() == '0,00') {
            toastr.warning("Informe o valor", "Alerta");
            return false;
        }
    });
});

function AdicionarDescritivo() {
    let descritivo = $("#descritivo").val();

    if (!descritivo) {
        toastr.warning("Digite a informação antes de adicionar", "Campo Obrigatório");
        return false;
    }

    post("AdicionarDescritivo", { descritivo })
        .done((response) => {
            $("#lista-descritivo").empty().append(response);
            $("#descritivo").val("");
        });
}

function RemoverDescritivo(descritivo) {
    post("RemoverDescritivo", { descritivo })
        .done((response) => {
            $("#lista-descritivo").empty().append(response);
        });
}

function BuscarParametroBoletoBancario() {
    return get("BuscarParametroBoletoBancario")
        .done((response) => {
            $("#lista-parametro-boleto-bancario").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}