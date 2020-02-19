
$(document).ready(function () {
    ObterNotificacoes();

    setInterval(function () { ObterNotificacoes(); }, 300000);
});

function ObterNotificacoes() {
    $.ajax({
        url: '/Notificacao/ObterNotificacoes',
        dataType: 'json',
        success: function (response) {
            var j = "";
            //var p = 0; var Q = "";
            $.each(response.ListaNotificacoes, function (k, v) {
                j += "<li class='menu-li'><a class='btnNotifyGroup' style='width: 235px !important;' title='Notificações'><i class='fa fa-bell fa-bell-notificacao'></i><span class='typeNotify' name='entidade'>" + v.Entidade + "</span>" + " <span class='text-right badge-item badge-red margin-left-10' style='float:right;'>" + v.Mensagem + "</span><span name='idTipoNotificacao' style='display:none;'>" + v.IdTipoNotificacao + "</span></a></li>";
            });
            $("[id*='myNotifyList']").html("");

            $("[id*='myNotifyList']").append(j);
            $("[id*='countNotify']").html("<span>" + response.TotalNot + "</span>");

            $('#myAllNotifications').css("display", "none");
            //$('#myAllNotifications').html("");
            //$('#myAllNotifications').append(Q);

            $("[id*='countNotify']").html(response.TotalNot);

            if (response.ListaNotificacoes.length > 0) {
                $("[id*='aNotify']").attr("data-toggle", "dropdown");
                $("[id*='iconNotify']").removeClass("faBlack").addClass("faWhite");

                $("[id*='myNotifyList']").off('click', 'li');
                $("[id*='myNotifyList']").on('click', 'li', function () {
                    var entity = $(this).find('span[name="idTipoNotificacao"]').text();
                    buscarNotificacoes(entity);
                });
            }
            else {
                $("[id*='aNotify']").removeAttr("data-toggle");
                $("[id*='iconNotify']").removeClass("faWhite").addClass("faBlack");
                $("[id*='countNotify']").html("");
            }
        },
        failure: function (response) {
            toastr.error("Erro ao buscar as notificações: " + response.d, "Notificações");
        }
    });
}

function Atualizar(id, entidade, status) {

    $.ajax({
        url: "/Notificacao/Atualizar",
        type: "POST",
        data: { id: id, entidade: entidade, statusAtualizacao: status },
        success: function (response) {
            hideLoading();
            if (status === "aprovar")
                toastr.success("Aprovação da notificação realizada com sucesso!");
            else
                toastr.success("Reprovação da notificação realizada com sucesso!");
            window.location.href = "/Home/Index/";
        },
        error: function (error) {
            hideLoading();
            console.log(error);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function RespostaNotificacao(id, tipoNotificacao, acao) {
    //debugger;
    $.ajax({
        url: "/Notificacao/RespostaNotificacao",
        type: "POST",
        data: { id: id, tipoNotificacao: tipoNotificacao, acao: acao },
        success: function (response) {
            hideLoading();

            if (response.Sucesso === false) {
                ObterNotificacoes();

                ModalNotifyClose(false);

                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });

                return false;
            }

            toastr.success("Realizada com sucesso!");

            buscarNotificacoes(tipoNotificacao);
        },
        error: function (error) {
            hideLoading();
            console.log(error);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}


function Informacoes(id, tipoNotificacao, acao) {
    //debugger;
    $.ajax({
        url: "/Notificacao/Informacoes",
        type: "POST",
        data: ({ id: id, tipoNotificacao: tipoNotificacao, acao: acao }),
        success: function (response) {
            hideLoading();

            if (response.Sucesso === false) {
                ModalNotifyClose(false);

                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });

                return false;
            }

            window.location.href = response.url;
            return false;
        },
        error: function (error) {
            hideLoading();
            console.log(error);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function buscarNotificacoes(idTipoNotificacao) {
        $.ajax({
            url: "/Notificacao/Principal",
            type: "POST",
            data: { idTipoNotificacao: idTipoNotificacao},
            success: function (result) {

                //comentada '$("#modalNot").modal("hide")'- ao aprovar, a tela ficava escura, com a modal na frente
                //parte do ajuste GTE-1793
                //$("#modalNot").modal("hide");
                
                $('#myNot').html("");
                $("#myNot").html(result);
                $("#modalNot").modal("show");
            },
            error: function (error) {
                hideLoading();
                console.log(error);
            },
            beforeSend: function () {
                showLoading();
            },
            complete: function () {
                hideLoading();
            }
        });
}

function ModalNotifyClose(reload = true) {
    $("#modalNot").attr("reload", reload);
    $("#modalNot").modal("hide");
}

$("#modalNot").on("hide.bs.modal", function () {
    if ($("#modalNot").attr("reload") === true
        || $("#modalNot").attr("reload") === null
        || $("#modalNot").attr("reload") === undefined
        || $("#modalNot").attr("reload") === "") {
        $("#modalNot").removeAttr("reload");
        showLoading();
        location.reload();
    }
});