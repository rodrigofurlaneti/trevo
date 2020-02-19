$(document).ready(function () {
    
    BuscarDepartamentos();
    $("#responsaveisIds").chosen();
    

    $("#Form").submit(function (e) {
        var dadosValidos = true;

        if (!$("#Nome").val()) {
            toastr.error("O campo \"Nome Departamento\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!$("#Sigla").val()) {
            toastr.error("O campo \"Abreviatura:\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!$("#responsaveisIds").val()) {
            toastr.error("O campo \"Nome do Responsável\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});

function BuscarDepartamentos() {

    BuscarPartialSemFiltro("/departamento/BuscarDepartamentos", "#lista-departamentos")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });
}