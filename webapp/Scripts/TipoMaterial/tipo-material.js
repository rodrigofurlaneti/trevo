$(document).ready(function () {
    BuscarTiposMateriais();
});

function BuscarTiposMateriais() {
    post("BuscarTiposMateriais")
        .done((result) => {
            $("#lista-tipo-material")
                .empty()
                .append(result);
        });
}