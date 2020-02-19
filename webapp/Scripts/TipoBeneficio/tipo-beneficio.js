$(document).ready(function () {
    BuscarTipoBeneficio();

    $("#tipo-beneficio-form").submit(function () {
        if (!ValidarForm(this.id))
            return false;
    });
});

function BuscarTipoBeneficio() {
    return get("BuscarTipoBeneficio")
        .done((response) => {
            $("#lista-tipo-beneficio").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}