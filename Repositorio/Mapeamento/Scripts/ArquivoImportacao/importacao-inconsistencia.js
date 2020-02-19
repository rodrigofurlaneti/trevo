$(document).ready(function () {

    ConfigTabelaGridSemCampoFiltroPrincipal();

    FormatarCampoData("DataDe");
    FormatarCampoData("DataAte");

    //$("#pesquisar").click(function () {
    //    window.location = "/contrato/pesquisar/" + $("#carteira option:selected").val();
    //    //showLoading();
    //    //setInterval(hideLoading(), 3000);
    //});
});

function editarStatus(idArquivo) {
    alert(idArquivo);
}