$(document).ready(function () {
    BuscarTipoOcorrencia();

    $("#tipo-ocorrencia-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
    });
});

function BuscarTipoOcorrencia() {
    return get("BuscarTipoOcorrencia", "TipoOcorrencia")
        .done((response) => {
            $("#lista-tipo-ocorrencia").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}