"use strict";
let callbackPaginacao = function (pagina) {
    alert(`Defina qual sera a função de callback no script principal
           exemplo: callbackPaginacao = BuscarClientes`);
};

$(document).ready(function () {
    FormatarNumerosInteiros(".somente-numeros");
    FormatarNumerosDecimais(".somente-decimais");
});

(function ($) {
    $.fn.valDecimal = function () {
        return $(this).val().replace(/[.]/g, "");
    };

    $.fn.decimal = function () {
        let valor = parseFloat($(this).val().replace(/[.]/g, "").replace(",", "."));
        return (isNaN(valor) ? 0 : valor).toFixed(2);
    };

    $.fn.int = function () {
        let valor = parseInt($(this).val());

        return isNaN(valor) ? 0 : valor;
    };

    $.fn.readonly = function () {
        return $(this).attr("readonly", "readonly");
    };

    $.fn.unReadonly = function () {
        return $(this).removeAttr("readonly");
    };

    $.fn.disabled = function () {
        return $(this).attr("disabled", "disabled").addClass("disabled");
    };

    $.fn.unDisabled = function () {
        return $(this).removeAttr("disabled").removeClass("disabled");
    };

    $.fn.required = function () {
        return $(this).attr("required", "required");
    };

    $.fn.unRequired = function () {
        return $(this).removeAttr("required");
    };

    $.fn.checked = function () {
        return $(this).prop("checked", true);
    };

    $.fn.unChecked = function () {
        return $(this).prop("checked", false);
    };

    $.fn.isChecked = function () {
        return $(this).is(":checked");
    };
})(jQuery);

function showView(pUrl, pData, pDivId) {
    var imgLoading = applicationPath + "/Img/loading_spinner.gif";
    var pDivDialog = "#PopUpLoading";
    $.ajax({
        url: pUrl,
        data: pData,
        type: "POST",
        success: function (response) {
            $("#" + pDivId).html(response);
            $('[data-toggle="tooltip"]').tooltip({
                html: true
            });
            $(pDivDialog).dialog("close");
        },
        error: function (error) {
            alert(JSON.stringify(error));
            $(pDivDialog).dialog("close");
        },
        beforeSend: function () {
            $(pDivDialog).css("text-align", "center");
            $(pDivDialog).css("margin-top", "10px");
            $(pDivDialog).html("<img src='" + imgLoading + "'/>");
            $(pDivDialog).dialog({
                modal: true,
                width: 600,
                height: 300,
                resizable: false,
                closeOnEscape: false,
                dialogClass: "noclose"
            });
            $(".ui-dialog-titlebar-close").hide();
            $("span.ui-dialog-title").text("Aguarde...");
        }
    });
}

function showViewPopUp(pUrl, pData, pHeight, pWidth, pTitulo) {
    var imgLoading = applicationPath + "/Img/loading_spinner.gif";
    var pDivDialog = "#PopUpLoading";
    if (pHeight === null) pHeight = 400;
    if (pWidth === null) pWidth = 800;
    if (pTitulo === null) pTitulo = "";
    $.ajax({
        url: pUrl,
        data: pData,
        type: "POST",
        success: function (response) {
            $(".ui-dialog-titlebar-close").show();
            $(pDivDialog).html(response);
            $('[data-toggle="tooltip"]').tooltip({
                html: true
            });
        },
        error: function (error) {
            alert(JSON.stringify(error));
            $(pDivDialog).dialog("close");
        },
        beforeSend: function () {
            $(pDivDialog).css("text-align", "center");
            $(pDivDialog).css("margin-top", "10px");
            $(pDivDialog).html("<img src='" + imgLoading + "'/>");
            $(pDivDialog).dialog({
                modal: true,
                width: pWidth,
                height: pHeight,
                resizable: false,
                closeOnEscape: false,
                dialogClass: "noclose"
            });
            $(".ui-dialog-titlebar-close").hide();
            $("span.ui-dialog-title").text("Apollo: " + pTitulo);
        }
    });
}

function closeViewPopup() {
    var pDivDialog = "#PopUpLoading";
    $(pDivDialog).dialog("close");
}

function doRequest(pUrl, pData, pFunction, pRequestType, disableHideLoading, disableShowLoading) {
    if (pRequestType === null) {
        pRequestType = "POST";
    }

    return $.ajax({
        url: pUrl,
        data: pData,
        cache: false,
        type: pRequestType,
        success: function (response) {
            if (!disableHideLoading)
                hideLoading();
            pFunction(response);
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            if (!disableShowLoading)
                showLoading();
        }
    });
}

function doRequestMethods(pUrl, pData, pFunction, pRequestType, pBeforeSend, pError) {
    if (pRequestType === null) {
        pRequestType = "POST";
    }

    return $.ajax({
        url: pUrl,
        data: pData,
        type: pRequestType,
        beforeSend: function () {
            pBeforeSend();
        },
        success: function (response) {
            pFunction(response);
        },
        error: function (xhr, status, error) {
            pError(xhr, status, error);
        }
    });
}

function doRequestNoMethods(pUrl, pData, pRequestType) {
    if (pRequestType === null) {
        pRequestType = "POST";
    }

    $.ajax({
        url: pUrl,
        data: pData,
        type: pRequestType
    });
}

function doJsonRequest(pUrl, pData, pFunction, pRequestType, disableHideLoading) {
    if (pRequestType === null) {
        pRequestType = "POST";
    }

    return $.ajax({
        url: pUrl,
        data: pData,
        type: pRequestType,
        contentType: "application/json",
        success: function (response) {
            if (!disableHideLoading)
                hideLoading();
            pFunction(response);
        },
        error: function (error) {
            hideLoading();
            alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function showPopUpMessage(html, tituloPopUp) {
    var pDivDialog = "#PopUpLoading";
    $(pDivDialog).css("text-align", "center");
    $(pDivDialog).css("margin-top", "10px");
    $(pDivDialog).html(html);
    $(pDivDialog).dialog({
        modal: true,
        width: 600,
        height: 300,
        resizable: false,
        closeOnEscape: false,
        dialogClass: "noclose"
    });
    $(".ui-dialog-titlebar-close").show();
    $("span.ui-dialog-title").text(tituloPopUp);
}

function isNullOrEmpty(value) {
    return value === "" || value === null;
}

function showLoading() {
    $("#background-loading").addClass("background-shadow");
    $(".LoadPosition").removeClass("hide");
}

function hideLoading() {
    $(".LoadPosition").addClass("hide");
    $("#background-loading").removeClass("background-shadow");
}

var TipoMensagem = {
    Sucesso: 0,
    Atencao: 1,
    Error: 2
};

function ShowModalLiberacaoBloqueioReferencia() {
    $('#myModalBloqueioReferencia').modal({ backdrop: 'static', keyboard: false });
}
function FecharModalBloqueio() {
    $("#txtPassLiberacao").val("");
    $("#myModalBloqueioReferencia").modal("hide");
}

function showMessage(mensagem, tipo, callback) {
    var urlImg = "";
    switch (tipo) {
        case 0:
            urlImg = applicationPath + "/Img/msg_ok.gif";
            break;
        case 1:
            urlImg = applicationPath + "/Img/msg_atencao.gif";
            break;
        default:
            urlImg = applicationPath + "/Img/msg_erro.gif";
            break;
    }
    mostrarPopUp(0, "", mensagem, urlImg);

    $(document).off("click", "#btOk_PopUp");
    if (callback) {
        $(document).on("click", "#btOk_PopUp", function () {
            callback($.colorbox.close);
        });
    }
}

function showMessageValidate(obj) {
    showMessage(obj.Mensagens, obj.Tipo);
}

function showMessageComBotao(mensagem, tipo, sim, nao) {
    var urlImg = "";
    switch (tipo) {
        case 0:
            urlImg = applicationPath + "/Img/msg_ok.gif";
            break;
        case 1:
            urlImg = applicationPath + "/Img/msg_atencao.gif";
            break;
        default:
            urlImg = applicationPath + "/Img/msg_erro.gif";
            break;
    }
    mostrarPopUpSimNao(mensagem, urlImg, sim, nao);
}
// Foco no botão "OK"
var dotCounter = 0;
var intId = 0;
var tentarFocar = function () {
    $("#cboxClose").remove();
    intId = setInterval(verificaFechamento, 400);
};

var verificaFechamento = function () {
    if (dotCounter < 10) {
        dotCounter++;
        $("#btOk_PopUp").focus();
        if ($("#btOk_PopUp").is(":focus")) {
            clearInterval(intId);
        }
    } else {
        clearInterval(intId);
    }
};
// guarda a lista de PopUps
var popups = [];
var mostrarPopUp = function (mostrarCancelar, redirecUrl, message, imageUrl) {
    var popup;

    // Cria novo PopUp e Inclui na lista de popups
    popup = {
        mostrarCancelar: mostrarCancelar,
        redirecUrl: redirecUrl,
        message: message,
        imageUrl: imageUrl
    };
    popups.unshift(popup);

    // verifica se tem popup pra exibir
    if (popups.length === 0)
        return;

    popup = popups.pop();
    //verifica se precisa redirecionar e já cria um redirecionamento

    $("#div_popup")[0].redirecionar = popup.redirecUrl;
    $("#div_popup_mensagem").html(popup.message);
    $("#div_popup_image").attr("src", popup.imageUrl);

    if (popup.mostrarCancelar) {
        $("#btCancelar_PopUp").show();
    } else {
        $("#btCancelar_PopUp").hide();
    }

    $.colorbox({
        inline: true,
        close: false,
        href: "#div_popup",
        onLoad: function () {
            $("#cboxClose").remove();
        },
        onClosed: function () {
            var divPopUp = $("#div_popup")[0];
            // redireciona só se clica no OK!
            if (popup.redirecUrl.length > 0 && divPopUp.ativarRedirecionamento === 1) {
                document.location = popup.redirecUrl;
            }
            //                        else {
            //                            mostrarPopUp();
            //                        }
        },
        OnComplete: tentarFocar
    });

};

var mostrarPopUpSimNao = function (message, imageUrl, callbackSim, callbackNao) {
    var $elem = $("#div_popup_sim_nao");

    $elem.find("#div_popup_mensagem").html(message);
    $elem.find("#div_popup_image").attr("src", imageUrl);

    $elem.find("#btSim_PopUp").unbind("click");
    $elem.find("#btNao_PopUp").unbind("click");

    $elem.find("#btSim_PopUp").bind("click", function () {
        if (callbackSim)
            callbackSim($.colorbox.close());
        else
            $.colorbox.close();
    });
    $elem.find("#btNao_PopUp").bind("click", function () {
        if (callbackNao)
            callbackNao($.colorbox.close());
        else
            $.colorbox.close();
    });

    $.colorbox({
        inline: true,
        close: false,
        href: "#div_popup_sim_nao",
        onLoad: function () {
            $("#cboxClose").remove();
        },
        OnComplete: tentarFocar
    });
}

var mostrarPopUpSimNaoManterAtivo = function (message, tipo, callbackSim, callbackNao, manterAtivo) {
    var imageUrl = '';
    switch (tipo) {
        case 0:
            imageUrl = applicationPath + '/Img/msg_ok.gif';
            break;
        case 1:
            imageUrl = applicationPath + '/Img/msg_atencao.gif';
            break;
        default:
            imageUrl = applicationPath + '/Img/msg_erro.gif';
            break;
    }

    var $elem = $('#div_popup_sim_nao');

    $elem.find('#div_popup_mensagem').html(message);
    $elem.find('#div_popup_image').attr('src', imageUrl);

    $elem.find('#btSim_PopUp').unbind('click');
    $elem.find('#btNao_PopUp').unbind('click');

    $elem.find('#btSim_PopUp').bind('click', function () {
        if (callbackSim)
            callbackSim();

        if (!manterAtivo)
            $.colorbox.close();
    });
    $elem.find('#btNao_PopUp').bind('click', function () {
        if (callbackNao)
            callbackNao();

        if (!manterAtivo)
            $.colorbox.close();
    });

    $.colorbox({
        inline: true,
        close: false,
        href: '#div_popup_sim_nao',
        onLoad: function () {
            $('#cboxClose').remove();
        },
        OnComplete: tentarFocar
    });
}

// ** Extende jQuery para que possa verificar foco **
$.extend(jQuery.expr[":"], {
    focus: function (element) {
        return element === document.activeElement;
    }
});
// esconde o popup. Este chama (auto-chama) o próximo popup
var esconderPopUp = function (ativarRedirecionamento) {
    var divPopUp = $("#div_popup")[0];
    divPopUp.ativarRedirecionamento = ativarRedirecionamento;
    $.colorbox.close();
};

function tratarRetornoValidacao(obj) {
    var validacao = obj.ValidacaoResultado;

    if (!validacao)
        return;

    obj = {
        Valido: validacao.Valido,
        Mensagens: "",
        Tipo: 0
    }

    if (validacao.Sucessos !== null && validacao.Sucessos.length > 0) {
        obj.Mensagens = retornarMensagens(validacao.Sucessos);
        obj.Tipo = TipoMensagem.Sucesso;
    } else if (validacao.Informacoes !== null && validacao.Informacoes.length > 0) {
        obj.Mensagens = retornarMensagens(validacao.Informacoes);
        obj.Tipo = TipoMensagem.Atencao;
    } else {
        obj.Mensagens = retornarMensagens(validacao.Erros);
        obj.Tipo = TipoMensagem.Error;
    }

    return obj;
}

function retornarMensagens(objMensagens) {
    var mensagens = $.map(objMensagens, function (msg, i) {
        return msg.Mensagem;
    });

    return mensagens.join("<br/>");
}

function ValidaNumero(pCampoId) {
    $("#" + pCampoId).keyup(function () {
        var $this = $(this);
        $this.val(er_replace(/[^0-9]+/g, '', $this.val()));
    });
}

function ValidaTexto(pCampoId) {
    $("#" + pCampoId).keyup(function () {
        var $this = $(this);
        $this.val(er_replace(/^[A-Za-z ]/g, '', $this.val()));
    });
}

function MakeChosen(id, maxSelectedOptions = null, width = "100%", single = false, disableSearch = false) {
    if (maxSelectedOptions === undefined || maxSelectedOptions === null)
        maxSelectedOptions = $("select#" + id + " option").length;

    $("select#" + id).chosen("destroy");

    if (single)
        $("#" + id).removeAttr("multiple");

    $("#" + id).chosen({
        allow_single_deselect: true,
        disable_search: disableSearch,
        no_results_text: "Oops, item não encontrado!",
        width: width,
        max_selected_options: maxSelectedOptions
    }).trigger("chosen:updated");
}

function MakeChosenPelaClasse(classe, maxSelectedOptions = null, width = "100%", single = false, disableSearch = false) {
    if (maxSelectedOptions === undefined || maxSelectedOptions === null)
        maxSelectedOptions = $("select." + classe + " option").length;

    $("select." + classe).chosen("destroy");

    if (single)
        $("." + classe).removeAttr("multiple");

    $("." + classe).chosen({
        allow_single_deselect: true,
        disable_search: disableSearch,
        no_results_text: "Oops, item não encontrado!",
        width: width,
        max_selected_options: maxSelectedOptions
    }).trigger("chosen:updated");
}

function MakeChosenMult(id) {
    $("#" + id).chosen({
        allow_single_deselect: true,
        no_results_text: "Oops, item não encontrado!",
        width: "95%"
    }).trigger("chosen:updated");
}

function FormatarCampoDataPelaClasse(classe) {
    $("." + classe).each(function () {
        $(this).mask("99/99/9999");
        $(this).datepicker({
            dateFormat: "dd/mm/yy",
            dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
            dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S", "D"],
            dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
            monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
            monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
            nextText: ">",
            prevText: "<"
        });
    });
}

function FormatarCampoData(pCampoId) {
    $("#" + pCampoId).mask("99/99/9999");
    $("#" + pCampoId).datepicker({
            dateFormat: "dd/mm/yy",
            dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
        dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S", "D"],
        dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
        monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        nextText: ">",
        prevText: "<"
    }).focus(function () {
        $(".ui-datepicker-calendar").css("display", "inline-table");
    });
}

function FormatarCampoDataMesAno(pCampoId) {
    $("#" + pCampoId).mask("99/9999");
    $("#" + pCampoId).datepicker({
        dateFormat: "mm/yy",
        changeMonth: true,
        changeYear: true,
        showButtonPanel: true,
        nextText: ">",
        prevText: "<",
        closeText: "OK",
        currentText: "Hoje",
        onClose: function (dateText, inst) {
            function isDonePressed() {
                return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
            }
            if (isDonePressed()) {
                var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                $('.date-picker').focusout();
            }
        },
        beforeShow: function (input, inst) {
            inst.dpDiv.addClass('month_year_datepicker');
            var datestr = $(this).val();
            if (datestr.length > 0) {
                var year = datestr.substring(datestr.length - 4, datestr.length);
                var month = datestr.substring(0, 2);
                $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                $(this).datepicker('setDate', new Date(year, month - 1, 1));
                $(".ui-datepicker table").css("display", "none");
            }
        }
    });
    $("#" + pCampoId).focus(function () {
        setTimeout(function () {
            $(".ui-datepicker-calendar").hide();
            $(".ui-datepicker-calendar").css("display", "none");
        }, 10);
    });
}

function FormatarCampoHora(selector) {
    var spOptions = {
        placeholder: "00:00"
    };

    $(document).on("blur", selector, function () {
        let val = this.value.replace(":", "");
        if (val.length === 1) {
            this.value = `0${val}:00`;
        }
        else if (val.length === 2) {
            if (val > 24) {
                toastr.error("Hora deve ser menor do que 24", "Erro");
                this.value = "";
            }
            else if (val === 24) {
                this.value = "00:00";
            }
            else {
                this.value = `${val}:00`;
            }
        }
        else if (val.length === 3) {
            toastr.error("Hora inválida.", "Erro");
            this.value = "";
        }
        else if (val.length === 4) {
            let valorHora = val.substr(0, 2);
            let valorMinuto = val.substr(2);

            if (valorHora > 24 || (valorHora === 24 && valorMinuto > 0)) {
                toastr.error("Hora deve ser menor do que 24", "Erro");
                this.value = "";
            }
            else if (valorHora === 24) {
                this.value = "00:00";
            }
        }
    });

    $(selector).mask("00:00", spOptions);
}

function FormatarReal(classe) {
    $(`input[class*=${classe}]`).maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
}

function FormatarNumerosInteiros(seletor) {
    var options = {
        placeholder: "0",
        onInvalid: function (val, e, f, invalid, options) {
            toastr.error("Digite apenas números.", "Digito Inválido", { timeOut: 1000 });
        }
    }

    $(seletor).mask("0#", options);
}

function FormatarNumerosDecimais(seletor, casasDecimais = 2) {
    let zerosDecimais = "";
    for (var i = 0; i < casasDecimais; i++) {
        zerosDecimais += "0";
    }

    var options = {
        placeholder: `00,${zerosDecimais}`,
        reverse: true,
        onInvalid: function (val, e, f, invalid, options) {
            toastr.error("Digite apenas números.", "Digito Inválido", { timeOut: 1000 });
        }
    }
    $(seletor).mask(`#.##0,${zerosDecimais}`, options);
}

function FormatarPlaca(seletor = "#placa") {
    //$(seletor).mask("SSS0A00", {
    //    'translation': {
    //        S: { pattern: /[A-Za-z]/ },
    //        0: { pattern: /[0-9]/ }
    //    },
    //    onKeyPress: function (value, event) {
    //        event.currentTarget.value = value.toUpperCase();
    //    }
    //});
}

function FormatarTelefoneFixo(seletor) {
    var options = {
        placeholder: "(99)9999-9999",
        onInvalid: function (val, e, f, invalid, options) {
            toastr.error("Digite apenas números.", "Digito Inválido", { timeOut: 1000 });
        }
    }
    $(seletor).mask("(00)0000-0000", options);
}

function FormatarCelular(seletor) {
    var options = {
        placeholder: "(99)99999-9999",
        onInvalid: function (val, e, f, invalid, options) {
            toastr.error("Digite apenas números.", "Digito Inválido", { timeOut: 1000 });
        }
    }
    $(seletor).mask("(00)00000-0000", options);
}

function FormatarCampoAno(pCampoId) {
    $("#" + pCampoId).mask("9999");
    $("#" + pCampoId).datepicker({
        changeMonth: false,
        changeYear: true,
        showButtonPanel: true,
        dateFormat: "yy",
        dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
        dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S", "D"],
        dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
        monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        nextText: ">",
        prevText: "<",
        onClose: function (dateText, inst) {
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 0, 1));
        }
    });
    $("#" + pCampoId).focus(function () {
        $(".ui-datepicker-year").hide();
        $(".ui-datepicker-calendar").hide();
    });
}

function ConvertToFloat(value) {
    if (value === undefined || value === null || value === "")
        return 0;

    return parseFloat(value.replace(".", "").replace(".", "").replace(",", "."));
}

function AcaoFecharModalExplicito() {
    var pDivDialog = "#PopUpLoading";
    $(pDivDialog).dialog("close");
}

function AvisoMensagemModalExplicito(mensagem) {
    var img = applicationPath + "/Img/msg_atencao.gif";
    var pDivDialog = "#PopUpLoading";

    $(pDivDialog).css("margin-top", "10px");
    $(pDivDialog).html("<div style='margin-top: 30px;'><img src='" + img + "' style='float: left; width: 70px;'><span style='font-size: 14px;'>" + mensagem + "</span><div><input type='button' value=' OK ' style='margin-top: 20px !important;margin-left: 350px;' onclick='AcaoFecharModalExplicito()'></div></div>");
    $(pDivDialog).dialog({
        modal: true,
        width: 930,
        resizable: false,
        closeOnEscape: false,
        dialogClass: "noclose"
    });
    $(pDivDialog).css("height", "");
    $(".ui-dialog-titlebar").hide();
}

function doRequestExplicito(pUrl, pData, pFunction, pRequestType, disableHideLoading) {
    if (pRequestType === null) {
        pRequestType = "POST";
    }

    $.ajax({
        url: pUrl,
        data: pData,
        type: pRequestType,
        success: function (response) {
            if (!disableHideLoading)
                hideLoading();
            pFunction(response);
        },
        error: function (xhr, exception) {
            hideLoading();
            AvisoMensagemModalExplicito(xhr.responseText);
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function getNomeArquivo(filename) {
    var lastPoint = filename.lastIndexOf(".");
    return filename.substring(0, lastPoint);
}

function getExtensaoArquivo(filename) {
    var lastPoint = filename.lastIndexOf(".");
    return filename.substring(lastPoint + 1);
}

function doDataTable(table, scrollY) {
    if (scrollY === '') {
        scrollY = '400px';
    }

    $(table).DataTable({
        "scrollX": true,
        "scrollY": scrollY,
        "paging": true,
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
        // intermitentemente, o arquivo json abaixo poderia demorar segundos para ser carregado, afetando toda personalizacao da tabela.
        // pego o conteudo e inserido diretamente na criadao do componente.
        //"language": {
        //    "url": applicationPath + "/Content/datatables/lang/Portuguese-Brasil.json"
        //}
    });
}

function doDataTableWithOptions(table, options) {
    if (options === null || options.scrollY === null || options.scrollY === '') {
        options.scrollY = '400px'
    }

    $(table).DataTable({
        "scrollX": true,
        "scrollY": options.scrollY,
        "order": [
            [options.order.columnIndex, options.order.direction]
        ],
        "paging": true,
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
        // intermitentemente, o arquivo json abaixo poderia demorar segundos para ser carregado, afetando toda personalizacao da tabela.
        // pego o conteudo e inserido diretamente na criadao do componente.
        //"language": {
        //    "url": applicationPath + "/Content/datatables/lang/Portuguese-Brasil.json"
        //}
    });
}

function endsWithImpl(text, searchText) {
    // implementacao realizada devido a str.endsWith nao ser aceita em IE 11 ou inferior
    // https://www.w3schools.com/Jsref/jsref_endswith.asp
    // Note: The endsWith() method is not supported in IE 11 (and earlier versions).
    if (text === null ||
        searchText === null ||
        text.trim() === '' ||
        searchText.trim() === '' ||
        text.length < searchText.length) {
        return false;
    }

    var sLen = searchText.length;
    var sufixInit = text.length - sLen;
    var sufix = text.substr(sufixInit);

    return sufix === searchText;
}

function MakeCombo(pCampo) {
    $("#" + pCampo)
        .chosen({
            //disable_search_threshold: 10,
            no_results_text: "Nenhuma opção encontrada",
            width: "100%"
        });
}

function Calendario(inputText) {
    FormatarCampoData(inputText);
}

function FormataCampoCpf(pCampoId) {
    $("#" + pCampoId).mask("999.999.999-99", {
        reverse: true
    });
}

function FormataCampoCnpj(pCampoId) {
    $("#" + pCampoId).mask("99.999.999/9999-99", {
        reverse: true
    });
}

function FormataCampoRg(pCampoId) {
    $("#" + pCampoId).mask("99.999.999-9", {
        reverse: true
    });
}


function validaCPF(cpf) {
    var strCPF = cpf.value;
    strCPF = strCPF.replace(".", "");
    strCPF = strCPF.replace(".", "");
    strCPF = strCPF.replace(".", "");
    strCPF = strCPF.replace("-", "");
    strCPF = strCPF.replace("/", "");
    strCPF = strCPF.replace("/", "");

    let soma;
    let resto;
    soma = 0;
    if (strCPF === "00000000000") {
        cpf.value = "";
        return false;
    }

    for (let i = 1; i <= 9; i++) soma = soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    resto = (soma * 10) % 11;

    if ((resto === 10) || (resto === 11)) resto = 0;
    if (resto !== parseInt(strCPF.substring(9, 10))) {
        cpf.value = "";
        return false;
    }

    soma = 0;
    for (let i = 1; i <= 10; i++) soma = soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    resto = (soma * 10) % 11;

    if ((resto === 10) || (resto === 11)) resto = 0;
    if (resto !== parseInt(strCPF.substring(10, 11))) {
        cpf.value = "";
        return false;
    }
    return true;
}

function validaCNPJ(Campocnpj) {

    var cnpj = Campocnpj.value;

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj === '') return false;

    if (cnpj.length !== 14) {
        Campocnpj.value = "";
        return false;
    }

    // Elimina CNPJs invalidos conhecidos
    if (cnpj === "00000000000000" ||
        cnpj === "11111111111111" ||
        cnpj === "22222222222222" ||
        cnpj === "33333333333333" ||
        cnpj === "44444444444444" ||
        cnpj === "55555555555555" ||
        cnpj === "66666666666666" ||
        cnpj === "77777777777777" ||
        cnpj === "88888888888888" ||
        cnpj === "99999999999999")
    {
        Campocnpj.value = "";
        return false;
    }

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado !== digitos.charAt(0)) {
        Campocnpj.value = "";
        return false;
    }

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado !== digitos.charAt(1)) {
        Campocnpj.value = "";
        return false;
    }

    return true;
}

function visualizarLog(nomeEntidade, codigoEntidade) {
    $('#modal-log .modal-body').empty();

    if (nomeEntidade && codigoEntidade) {
		let controllerAtivo = window.location.pathname.split("/")[1];
		let url = "/" + controllerAtivo + "/VisualizarLogEntidade"

        $.ajax({
            url: url,
            data: { nomeEntidade: nomeEntidade, codigoEntidade: codigoEntidade },
            cache: false,
            type: 'GET',
            success: function (response) {
                if (response) {
                    var items = JSON.parse(response);
                    var html = "";

					$.map(items, function (obj, i) {
						html += "<p><i> " + obj.UsuarioNome + " alterou <strong>" + obj.Atributo + "</strong> de " + obj.ValorAntigo + " para <strong>" + obj.ValorNovo + "</strong> em " + obj.DataEHora + ".</i></p>";
                    });

                    if (html) {
                        $('#modal-log .modal-body').append(html);
                    } else {
                        $('#modal-log .modal-body').append("Ainda não há alterações para o registro selecionado!");
                    }

					$("#modal-log").modal('show');
					hideLoading();
                }
            },
            error: function (error) {
                hideLoading();
            },
            beforeSend: function () {
                showLoading();
            }
        });
    }
}

function er_replace(pattern, replacement, subject) {
    return subject.replace(pattern, replacement);
}


function verificaData(Data) {
    Data = Data.substring(0, 10);

    var dma = -1;
    var data = Array(3);
    var ch = Data.charAt(0);
    for (let i = 0; i < Data.length && ((ch >= '0' && ch <= '9') || (ch === '/' && i !== 0));) {
        data[++dma] = '';
        if (ch !== '/' && i !== 0) return false;
        if (i !== 0) ch = Data.charAt(++i);
        if (ch === '0') ch = Data.charAt(++i);
        while (ch >= '0' && ch <= '9') {
            data[dma] += ch;
            ch = Data.charAt(++i);
        }
    }
    if (ch !== '') return false;
    if (data[0] === '' || isNaN(data[0]) || parseInt(data[0]) < 1) return false;
    if (data[1] === '' || isNaN(data[1]) || parseInt(data[1]) < 1 || parseInt(data[1]) > 12) return false;
    if (data[2] === '' || isNaN(data[2]) || ((parseInt(data[2]) < 0 || parseInt(data[2]) > 99) && (parseInt(data[2]) < 1900 || parseInt(data[2]) > 9999))) return false;
    if (data[2] < 50) data[2] = parseInt(data[2]) + 2000;
    else if (data[2] < 100) data[2] = parseInt(data[2]) + 1900;
    switch (parseInt(data[1])) {
        case 2:
            {
                if (((parseInt(data[2]) % 4 !== 0 || (parseInt(data[2]) % 100 === 0 && parseInt(data[2]) % 400 !== 0)) && parseInt(data[0]) > 28) || parseInt(data[0]) > 29) return false;
                break;
            }
        case 4:
        case 6:
        case 9:
        case 11:
            {
                if (parseInt(data[0]) > 30) return false;
                break;
            }
        default:
            {
                if (parseInt(data[0]) > 31) return false;
            }
    }
    return true;
}

function submit(action, method, values) {
    var form = $("<form/>", {
        action: action,
        method: method
    });
    $.each(values, function () {
        form.append($("<input/>", {
            type: "hidden",
            name: this.name,
            value: this.value
        }));
    });
    form.appendTo("body").submit();
}

function somenteLetra(value) {
    var re = /^[a-záàâãéèêíïóôõöúçñ ]+$/ig;
    return re.test(value);
}

function ValidaTelefone(pValor) {
    pValor = pValor.replace("(", "");
    pValor = pValor.replace(")", "");
    pValor = pValor.replace("-", "");
    pValor = pValor.replace(" ", "").trim();
      
    if (pValor.length !== 10) {
        return false;
    }

    if (pValor === '0000000000') {
        return false;
    }


    return true;
}

function ValidaCelular(pValor) {
    pValor = pValor.replace("(", "");
    pValor = pValor.replace(")", "");
    pValor = pValor.replace("-", "");
    pValor = pValor.replace(" ", "").trim();

    if (pValor.length !== 11) {
        return false;
    }

    if (pValor === '00000000000') {
        return false;
    }

    return true;
}

function generateNewGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
}

function runBackgroundFunction(method) {
    if (method)
        method();
}

function checkIfImportationIsDone(hashParam) {
    var hashImportacao = hashParam ? hashParam : localStorage.getItem('importacao');
    var tentativa = localStorage.getItem('importacao-verificacao');
    var emProcesso = localStorage.getItem('verificacao-em-processo');

    if (!hashImportacao) {
        limparSessionImportacao();
        return false;
    }

    if (!tentativa) {
        localStorage.setItem('importacao-verificacao', '1');
        tentativa = 1;
    }

    if (tentativa > 5) {
        limparSessionImportacao();
        return false;
    }

    if (emProcesso) {
        zeraContadorDeTentativas();
        buscarProgressoImportacaoNovamente();
        return false;
    }

    localStorage.setItem('verificacao-em-processo', '1');

    var xhr = $.ajax({
        type: 'GET',
        contentType: 'application/json',
        data: {
            hash: hashImportacao
        },
        url: '/ArquivoImportacao/RetornaProgressoImportacao'
    }).done(function (response) {
        response = JSON.parse(response);

        if (response.Concluido) {
            $('#modal-process-import').modal('hide');
            openCustomModal(null, null, response.TipoModal, response.Titulo, response.Mensagem, false, null, function () { window.location.href = '/ArquivoImportacao/Index/';});
            limparSessionImportacao();
            return true;
        } else {
            if ($('#progress-importacao').length > 0) {
                var percentage = ((parseInt(response.Qtdprocessado) / parseInt(response.QtdRegistros)) * 100).toFixed(2) + "%";
                $('.bg-color-orange').css('width', percentage);
                $('.bg-color-orange')[0].attributes['aria-valuenow'] = percentage;
                $('#progress-importacao')[0].innerText = percentage;
                $('#mensagem-importacao')[0].innerText = "Processando " + response.Qtdprocessado + " de " + response.QtdRegistros + "...";
            }
        }

        buscarProgressoImportacaoNovamente();
    }).fail(function (response) {
        buscarProgressoImportacaoNovamente();
    });

    setTimeout(function () {
        xhr.abort();
        zeraContadorDeTentativas();
    }, 20000);
}

function zeraContadorDeTentativas() {
    localStorage.setItem('importacao-verificacao', 1);
    localStorage.removeItem('verificacao-em-processo');
}

function buscarProgressoImportacaoNovamente() {
    let numTentativa = localStorage.getItem('importacao-verificacao');
    localStorage.setItem('importacao-verificacao', parseInt(numTentativa) + 1);
    setTimeout(checkIfImportationIsDone, 10000);
}

function limparSessionImportacao() {
    localStorage.removeItem('importacao');
    localStorage.removeItem('importacao-verificacao');
    localStorage.removeItem('verificacao-em-processo');
}

(function () {
    setTimeout(checkIfImportationIsDone, 10000);
})();


function mudarPagina(event, pagina, disabled = false) {
    event.preventDefault();

    if (!disabled) {
        // Defina qual sera a função de callback no script principal
        // exemplo: callbackPaginacao = BuscarClientes
        callbackPaginacao(pagina);
    }
}

function isEdit() {
    return location.pathname.toLowerCase().includes("edit");
}

function isView() {
    return location.pathname.toLowerCase().includes("view");
}

function isSave() {
    return location.pathname.toLowerCase().includes("salvardados");
}

function ValidarForm(formId) {
    let valido = true;

    $(`#${formId} [required=required]`).each(function (index, element) {
        if (!element.value) {
            let label = $(element).parent().find("label").text().replace(":", "")
            toastr.warning(`O campo ${label} é obrigatório`, "Alerta");
            valido = false;
            return valido;
        }
    });
    return valido;
}

function obterParametroDaUrl(parametro) {
    let params = new URLSearchParams(location.search.toLowerCase());

    return params.get(parametro.toLowerCase());
}

String.prototype.convertToDate = function () {
    if (typeof this !== "string")
        return this;

    let data = new Date(parseInt(this.replace("/Date(", "").replace(")/", "")));
    let dia = data.getDate().toString().length === 1 ? "0" + data.getDate() : data.getDate();
    let mes = data.getMonth().toString().length === 1 ? "0" + data.getMonth() : data.getMonth();

    return `${dia}/${mes}/${data.getFullYear()}`;
}

function generateUUID() { // Public Domain/MIT
    var d = new Date().getTime();//Timestamp
    var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16;//random number between 0 and 16
        if (d > 0) {//Use timestamp until depleted
            r = (d + r) % 16 | 0;
            d = Math.floor(d / 16);
        } else {//Use microseconds since page-load if supported
            r = (d2 + r) % 16 | 0;
            d2 = Math.floor(d2 / 16);
        }
        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}