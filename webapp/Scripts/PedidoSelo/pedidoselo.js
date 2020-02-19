var periodoitems = [];
var periodohorarioEmEdicao = {};
var indiceVeiculosAdicionados = 0;

$(document).ready(function () {

    //$("#diasvencimento").change(function () {
    //    var tipo = $("#tipopagamento").val();
    //    var valor = $(this).val();

    //    if (tipo) {
    //        if (valor) {
    //            if (tipo === 1) {
    //                if (valor < 1) {
    //                    $(this).val(1);
    //                }
    //            }
    //            else {
    //                if (valor < 5) {
    //                    $(this).val(5);
    //                }
    //            }
    //        }
    //    }
    //});

    $("#datavencimento").change(function () {
        var partesData = $(this).val().split("/");
        var data = new Date(partesData[2], partesData[1], partesData[0]);

        if (data <= new Date()) {
            $(this).val("");
            toastr.error("Informe uma Data para Vencimento, maior que a data atual!", "Data para Vencimento");
        }
    });


    FormatarCampoData("validadepedido");
    FormatarCampoData("datavencimento");
    
    MakeChosen("unidade", null, "100%");
    MakeChosen("convenio", null, "100%");
    MakeChosen("tipopagamento", null, "100%");
    MakeChosen("negociacaodesconto", null, "100%");
    MakeChosen("tiposelo", null, "100%");
    MakeChosen("proposta", null, "100%");

    $("#tipopagamento").prop('disabled', true).trigger("chosen:updated");
    $("#validadepedido").prop('disabled', true);
    $("#datavencimento").prop('disabled', true);

    MetodoUtil();

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/PedidoSelo/BuscarCliente',
                data: { descricao: request.term },
                type: "POST",
                success: function (data) {
                    if (!data.length) {
                        $("#clienteText").val("");
                        $("#cliente").val("");

                        var result = [
                            {
                                label: 'Nenhum resultado encontrado',
                                value: response.term
                            }
                        ];
                        response(result);
                    }
                    else {
                        response($.map(data, function (item) {
                            return { label: item.Descricao, value: item.Descricao, id: item.Id };
                        }));
                    }
                },
                error: function (response) {
                    console.log(response.responseText);

                    $("#cliente").val(""); //hiddenId
                    $("#clienteText").val(""); //hiddenText
                },
                failure: function (response) {
                    console.log(response.responseText);

                    $("#cliente").val(""); //hiddenId
                    $("#clienteText").val(""); //hiddenText
                }
            });
        },
        select: function (e, i) {
            $("#clienteText").val(i.item.label);
            $("#cliente").val(i.item.id);

            populaCombo('#unidade', 'PedidoSelo', 'CarregarUnidade', { idCliente: parseInt(i.item.id) }, 0, CarregarComboUnidade);
            buscarClienteSelo(parseInt(i.item.id));
        },
        minLength: 3
    }).change(function () {
        var autoTexto = $("#clientes").val();
        var texto = $("#clienteText").val();
        if (autoTexto !== texto
            || (autoTexto === "" || autoTexto === null || autoTexto === undefined)) {

            $("#clientes").val(""); //autocomplete
            $("#cliente").val(""); //hiddenId
            $("#clienteText").val(""); //hiddenText

            limpaCombo('#unidade');
            limpaCombo('#convenio');
            limpaCombo('#proposta');
            limpaCombo('#tiposelo');
        }
    });

    
    $("#unidade").change(function () {
        CarregarComboUnidade();
    });

    $("#convenio").change(function () {
        CarregarComboConvenio();
    });

    $("#buscastatus").change(function() {
        Pesquisa();
    });

    $("#clonar").click(function () {
        clonar();
    });

    $("#cancelar").click(function () {
        cancelar();
    });

    $("#bloquear").click(function () {
        bloquear();
    });

    $("#desbloquear").click(function () {
        desbloquear();
    });

    //$("#tipopagamento").change(function () {

    //    var tipo = $(this).val();

    //    if (tipo === "1") {
    //        $("#diasvencimento").val(1);
    //        $("#diasvencimento").attr("min", 1);

    //    }
    //    else if (tipo === "2") {
    //        $("#diasvencimento").val(5);
    //        $("#diasvencimento").attr("min", 5);
    //    }
    //});
    
    $("#quantidade").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });
    
    $(window).load(function () {

        var id = $("#hdnPedidoSelo").val();

        if (!id || id === "0") {
            $("#clonar").attr('disabled', 'disabled');
            $("#cancelar").attr('disabled', 'disabled');
            $("#bloquear").attr('disabled', 'disabled');
            $("#desbloquear").attr('disabled', 'disabled');
        }

        var status = $("#hdnStatusPedido").val();
        $("#emissaoselo").prop('disabled', true);
        if (status && status !== "Rascunho") {
            BloqueiaTodosCampos();
        }

        Pesquisa();
    });

    $("#form").submit(function (e) {
        if (!ValidarCampos()) {
            e.preventDefault();
            return false;
        }
        if (!VerificacaoBloqueioReferencia()) {
            e.preventDefault();
            return false;
        }
    });
});

function buscarClienteSelo(idCliente) {

    $.ajax({
        url: '/PedidoSelo/BuscarSeloCliente',
        type: "POST",
        dataType: "json",
        data: { clienteId: idCliente },
        success: function (result) {

            $('#tipopagamento').val(result.TipoPagamentoSelo);
            $('#tipopagamento').trigger("chosen:updated");

            debugger;
            var dataAtual = new moment();
            dataAtual.add(result.ValidadeSelo, "d");
            $('#validadepedido').val(dataAtual.format('DD/MM/YYYY'));

            dataAtual = new moment();
            dataAtual.add(result.PrazoPagamentoSelo, "d");
            $('#datavencimento').val(dataAtual.format('DD/MM/YYYY'));

        },
        error: function (error) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}


function ValidarCampos() {
    let dadosValidos = true;

    var cliente = document.getElementById("cliente");
    var convenio = $("#convenio").val();//document.getElementById("convenio");
    var unidade = $("#unidade").val();//document.getElementById("unidade");
    var tipopagamento = $("#tipopagamento").val();//document.getElementById("tipopagamento");
    var negociacaodesconto = document.getElementById("negociacaodesconto");
    var validadepedido = $("#validadepedido").val();
    var tiposelo = $("#tiposelo").val();//document.getElementById("tiposelo");
    var quantidade = $("#quantidade").val();
    //var diasvencimento = $("#diasvencimento").val();
    var datavencimento = $("#datavencimento").val();
    var proposta = $("#proposta").val();//document.getElementById("proposta");

    if (!datavencimento) {
        toastr.error("O campo \"Data para Vencimento\" deve ser preenchido!");
        dadosValidos = false;
    }

    if (!quantidade) {
        toastr.error("O campo \"Quantidade\" deve ser preenchido!");
        dadosValidos = false;
    }

    if (tiposelo <= 0 || tiposelo === "" || tiposelo === undefined) {
        toastr.error("O campo \"Tipo Selo\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (!validadepedido) {
        toastr.error("O campo \"Validade Pedido\" deve ser preenchido!");
        dadosValidos = false;
    } else if (dataMenorQueDataAtual(validadepedido)) {
        toastr.error("O campo \"Validade Pedido\" deve ser preenchido com a data atual ou superior!");
        dadosValidos = false;
    }

    if (proposta <= 0 || proposta === "" || proposta === undefined) {
        toastr.error("O campo \"Proposta\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (tipopagamento <= 0 || tipopagamento === "" || tipopagamento === undefined) {
        toastr.error("O campo \"Tipo Pagamento\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (unidade <= 0 || unidade === "" || unidade === undefined) {
        toastr.error("O campo \"Unidade\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (convenio <= 0 || convenio === "" || convenio === undefined) {
        toastr.error("O campo \"Convênio\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (cliente <= 0 || cliente === "" || cliente === undefined) {
        toastr.error("O campo \"Cliente\" deve ser selecionado!");
        dadosValidos = false;
    }

    if (!dadosValidos) {
        return false;
    }
    return true;
}

function CarregarComboUnidade() {
    limpaCombo('#convenio');
    limpaCombo('#proposta');
    limpaCombo('#tiposelo');

    let id = $('#unidade').val();
    if (id) {
        populaCombo('#convenio', 'PedidoSelo', 'CarregarConvenio', { idUnidade: parseInt(id) }, 0, CarregarComboConvenio);
    }
}

function CarregarComboConvenio() {
    //  limpaCombo('#unidade');
    limpaCombo('#proposta');
    limpaCombo('#tiposelo');

    let id = $('#unidade').val();
    //if (id)
    //    populaCombo('#unidade', 'PedidoSelo', 'CarregarUnidade', { idConvenio: parseInt(id) });

    let idCliente = $('#cliente').val();
    if (idCliente)
        populaCombo('#proposta', 'PedidoSelo', 'CarregarProposta', { idUnidade: parseInt(id), idCliente: parseInt(idCliente) }, 0);

    let idConvenio = $('#convenio').val();
    if (idConvenio)
        populaCombo('#tiposelo', 'PedidoSelo', 'CarregarTipoSelo', { idUnidade: parseInt(id), idConvenio: parseInt(idConvenio) });
}

function BloqueiaPorTipoPedido(valor) {
    
    if (valor === "1") { //bloqueio
        $("#clientes").prop('disabled', true).trigger("chosen:updated");
        $("#cliente").prop('disabled', true).trigger("chosen:updated");
        $("#convenio").prop('disabled', true).trigger("chosen:updated");
        $("#unidade").prop('disabled', true).trigger("chosen:updated");
        //$("#tipopagamento").prop('disabled', true).trigger("chosen:updated");
        $("#negociacaodesconto").prop('disabled', true).trigger("chosen:updated");
        //$("#validadepedido").prop('disabled', true).trigger("chosen:updated");
        $("#tiposelo").prop('disabled', true).trigger("chosen:updated");
        $("#quantidade").prop('disabled', true);
        $("#diasvencimento").prop('disabled', true);
        //$("#datavencimento").prop('disabled', true);
        $("#proposta").prop('disabled', true).trigger("chosen:updated");
        $("#salvar").text("Realizar Bloqueio");

    }
    else if (valor === "2") { //desbloqueio
        $("#clientes").prop('disabled', true).trigger("chosen:updated");
        $("#cliente").prop('disabled', true).trigger("chosen:updated");
        $("#convenio").prop('disabled', true).trigger("chosen:updated");
        $("#unidade").prop('disabled', true).trigger("chosen:updated");
        //$("#tipopagamento").prop('disabled', true).trigger("chosen:updated");
        $("#negociacaodesconto").prop('disabled', true).trigger("chosen:updated");
        //$("#validadepedido").prop('disabled', true).trigger("chosen:updated");
        $("#tiposelo").prop('disabled', true).trigger("chosen:updated");
        $("#quantidade").prop('disabled', true);
        $("#diasvencimento").prop('disabled', true);
        //$("#datavencimento").prop('disabled', true);
        $("#proposta").prop('disabled', true).trigger("chosen:updated");
        $("#salvar").text("Realizar Desbloqueio");
    }
    else if (valor === "3") { //nova emissao
        $("#clientes").prop('disabled', true).trigger("chosen:updated");
        $("#cliente").prop('disabled', false).trigger("chosen:updated");
        $("#convenio").prop('disabled', false).trigger("chosen:updated");
        $("#unidade").prop('disabled', false).trigger("chosen:updated");
        //$("#tipopagamento").prop('disabled', false).trigger("chosen:updated");
        $("#negociacaodesconto").prop('disabled', false).trigger("chosen:updated");
        //$("#validadepedido").prop('disabled', false).trigger("chosen:updated");
        $("#tiposelo").prop('disabled', false).trigger("chosen:updated");
        $("#quantidade").prop('disabled', false);
        $("#diasvencimento").prop('disabled', true);
        //$("#datavencimento").prop('disabled', false);
        $("#proposta").prop('disabled', false).trigger("chosen:updated");
        $("#salvar").text("Solicitar Aprovação");
    }
}

function AtualizaParcial(url, divId, obj) {
    showLoading();
    return $.post(url, obj)
        .done(function (result) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $(divId).empty();
                $(divId).append(result);
            }
        })
        .fail(function (error) {
            $(divId).empty();
            $(divId).append(error.responseText);
        }).always(function () {
            hideLoading();
        });
}

function Pesquisa() {
    var valor = document.getElementById("buscastatus");
    var statusid = valor.selectedIndex;
    
    $.ajax({
        url: "/pedidoselo/AtualizaGridPedidos",
        type: "POST",
        data: { statusid },
        success: function (result) {
            hideLoading();
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                hideLoading();

                $("#lista-pedido").empty();
                $("#lista-pedido").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        error: function (err) {
            showLoading();
        },
        complete: function () {
            hideLoading();
            MetodoUtil();
        }
    });
}

function BloqueiaTodosCampos() {
    $("#clientes").prop('disabled', true).trigger("chosen:updated");
    $("#cliente").prop('disabled', true).trigger("chosen:updated");
    $("#convenio").prop('disabled', true).trigger("chosen:updated");
    $("#unidade").prop('disabled', true).trigger("chosen:updated");
    //$("#tipopagamento").prop('disabled', true).trigger("chosen:updated");
    $("#negociacaodesconto").prop('disabled', true).trigger("chosen:updated");
    $("#proposta").prop('disabled', true).trigger("chosen:updated");
    $("#tiposelo").prop('disabled', true).trigger("chosen:updated");
    $("#quantidade").prop('disabled', true);
    $("#diasvencimento").prop('disabled', true);
    //$("#validadepedido").prop('disabled', true).trigger("chosen:updated");
    //$("#datavencimento").prop('disabled', true);
    $("#salvar").prop('disabled', true);
    $("#limpar").attr('disabled', 'disabled');
    $("#bloquear").attr('disabled', 'disabled');
    $("#desbloquear").attr('disabled', 'disabled');
    $("#emissaoselo").prop('disabled', true);

    var status = $("#hdnStatusPedido").val();
    var statusEmissao = $("#hdnStatusEmissaoSelo").val();
    if (status) {
        if (status === "Cancelado") {
            $("#cancelar").attr('disabled', 'disabled');
        } if (status === "AprovadoPeloCliente") {
            if (!statusEmissao || statusEmissao === "Ativo")
                $("#bloquear").removeAttr('disabled');
            else if (statusEmissao === "Bloqueado")
                $("#desbloquear").removeAttr('disabled');
        }
    }
}

function clonar() {
    var id = $("#hdnPedidoSelo").val();
    var action = 'ClonarPedido';

    ChamarAjax(action, id);
}

function cancelar() {
    var id = $("#hdnPedidoSelo").val();
    var action = 'CancelarPedido';

    $('#alert-modal').one("success", function () {
        location.reload();
    })

    var statusCobranca = $("#hdnStatusLancamentoCobrancaPedidoSelo").val();
    if (statusCobranca == "Pago") {
        //$("#cancelar").attr('disabled', 'disabled');
        openCustomModal(id, "'/PedidoSelo/" + action + "'", 'warning', 'Atenção!', 'O pedido será cancelado, mas existem cobranças pagas! <br />Confirma a operação?', true, "Sim", function cancelarModalCallback() {
            //ChamarAjax(action, id);
            $('#alert-modal').off("success");
        }, "Não");
    } else {
        ChamarAjax(action, id);
    }
}

function bloquear() {
    var id = $("#hdnPedidoSelo").val();
    var action = 'BloquearLote';

    ChamarAjax(action, id);
}

function desbloquear() {
    var id = $("#hdnPedidoSelo").val();
    var action = 'DesbloquearLote';

    ChamarAjax(action, id);
}

function ChamarAjax(action, id) {

    if (!id)
        id = 0;

    $.ajax({
        url: "/PedidoSelo/" + action,
        type: "POST",
        data: { id },
        success: function (result) {
            hideLoading();

            let funcaoPosAcao = function () { };
            if (result.tipo === "success")
                funcaoPosAcao = function () { window.location.href = '/PedidoSelo/Index'; };
            
            openCustomModal(null, null, result.tipo, result.titulo, result.mensagem, false, null, funcaoPosAcao);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function VerificacaoBloqueioReferencia() {
    var retorno = true;
    showLoading();
    $.ajax({
        url: "/PedidoSelo/VerificacaoBloqueioReferencia",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            pedidoSelo: {
                Id: $("#hdnPedidoSelo").val(),
                DataVencimento: $("#datavencimento").val()
            }
        },
        success: function (result) {
            if (result.Bloqueio) {
                $("#modalDivBloqueio").empty();
                $("#modalDivBloqueio").append(result.Modal);
                retorno = false;
            }
            else if (typeof result === "object" && result.Sucesso !== undefined && !result.Sucesso) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
                retorno = false;
            }
            else {
                retorno = true;
            }
        },
        error: function (error) {
            console.log(error.responseText);
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });

    return retorno;
}