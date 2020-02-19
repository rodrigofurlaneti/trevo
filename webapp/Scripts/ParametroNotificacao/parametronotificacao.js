$(document).ready(function () {

    MakeChosen("aprovadores");

    $("#ParametroNotificacaoForm").submit(function (e) {
        var dadosValidos = true;

        if ($("#cbTipoNotificacao").val() === "") {
            toastr.error('Preencha o Tipo de Notificação', 'Tipo de Notificação Obrigatório');
            dadosValidos = false;
        }

        if ($("#aprovadores").val() === "") {
            toastr.error('Defina os usuários aprovadores!', 'Usuários Aprovadores');
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });
});






