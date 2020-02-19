$(document).ready(function () {

    MetodoUtil();

    $("#Form").submit(function (e) {

        let dadosValidos = true;
        let nome = $("#Nome").val();

        if (nome === '' || $.trim(nome) === "") {
            toastr.error("O campo \"Nome Marca\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});