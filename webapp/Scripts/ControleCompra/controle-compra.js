$(document).ready(function () {
    FormatarCampoData("filtro-data");
    FormatarCampoData("nova-data");

    $("#controle-compra-form").submit(function () {
        if (!$("#nova-data").val()) {
            toastr.warning("Informe a nova data");
            event.preventDefault();
        }
    });
});

function BuscarCotacoes() {
    let data = $("#filtro-data").val();
    let statusCompraServico = $("#filtro-status").val();

    return post("BuscarCotacoes", { data, statusCompraServico })
        .done((response) => {
            $("#lista-controle-compra").empty().append(response);
            ConfigTabelaGridSemCampoFiltroPrincipal();
        })
}