var listahorarioparametro = [];

$(document).ready(function () {

    MakeChosen("unidade", null, "100%");
    MakeChosen("convenio", null, "100%");
    MakeChosen("tiposelo", null, "100%");

    FormatarCampoData("data-validade");
    FormatarCampoData("data-entrega");

    if (!$("#hdnEmissaoSelo").val() || !$("#data-validade").val())
        $("#validade-filtro").hide();

    $('input[name="checado"]').change(function () {

        var checado = $(this).is(':checked');

        $("#tabela-pedido tbody").find('tr').each(function (rowIndex, r) {
            $(this).find('input[name="checado"]').prop("checked", false);
        });

        if (checado)
            $(this).prop("checked", false);
        else
            $(this).prop("checked", true);
    });
    
    $("#unidade").change(function () {
        CarregarComboUnidade();
    });

    $("#convenio").change(function () {
        //LoadResponsavel();
        ReiniciaSelos();

        limpaCombo('#tiposelo');
        
        let idUnidade = $('#unidade').val();
        if (idUnidade) {
            let idConvenio = $('#convenio').val();
            if (idConvenio)
                populaCombo('#tiposelo', 'EmissaoSelo', 'CarregarTipoSelo', { idConvenio: parseInt(idConvenio), idUnidade: parseInt(idUnidade) });
        }
    });

    $("#tiposelo").change(function () {
        ReiniciaSelos();
    });

    $("#quantidade").change(function () {
        ReiniciaSelos();
    });

    $("#data-validade").change(function () {
        ReiniciaSelos();
    });

    var tiposeloid = $("#tiposelo").val();
    if (tiposeloid) {
        VerificaComValidade(tiposeloid);
    }

    $("#clientes").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/emissaoselo/BuscarCliente',
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

            populaCombo('#unidade', 'EmissaoSelo', 'CarregarUnidade', { idCliente: parseInt(i.item.id) }, 0, CarregarComboUnidade);
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
            limpaCombo('#tiposelo');
        }
    });
});

function CarregarComboUnidade() {
    limpaCombo('#convenio');
    limpaCombo('#tiposelo');

    let id = $('#unidade').val();
    if (id)
        populaCombo('#convenio', 'EmissaoSelo', 'CarregarConvenio', { idUnidade: parseInt(id) }, 0, CarregarComboConvenio);
}

function CarregarComboConvenio() {
    //  limpaCombo('#unidade');
    limpaCombo('#proposta');
    limpaCombo('#tiposelo');

    let id = $('#unidade').val();
    //if (id)
    //    populaCombo('#unidade', 'PedidoSelo', 'CarregarUnidade', { idConvenio: parseInt(id) });

    let idCliente = $('#cliente').val();
    if (idCliente) {
        populaCombo('#proposta', 'PedidoSelo', 'CarregarProposta', { idUnidade: parseInt(id), idCliente: parseInt(idCliente) }, 0);
        get(`BuscarNomeConvenio?idCliente=${idCliente}`)
            .done((response) => {
                $("#nomeImpressaoSelo").val(response);
            });
    }

    let idConvenio = $('#convenio').val();
    if (idConvenio)
        populaCombo('#tiposelo', 'PedidoSelo', 'CarregarTipoSelo', { idUnidade: parseInt(id), idConvenio: parseInt(idConvenio) });
}

function ValidarLote() {

    if ($("#hdnEmissaoSelo").val() <= 0 || $("#hdnEmissaoSelo").val() === "") {
        toastr.error('Selecione um lote válido!');
        return false;
    }

    return true;
}

function SelecionaPedido(ele) {

    var checado = $(ele).is(':checked');
    var possuiValidade = $(ele).attr('data-selo-com-validade');

    $("#tabela-pedido tbody").find('tr').each(function (rowIndex, r) {
        $(this).find('input[name="checado"]').prop("checked", false);
    });

    if (!checado) {
        $(ele).prop("checked", false);
        $("#validade-filtro").hide();
    }
    else {
        $(ele).prop("checked", true);
        if (possuiValidade === "True")
            $("#validade-filtro").show();
        else
            $("#validade-filtro").hide();
    }
}

function CancelarLote() {
    if ($('#btnTransferir').attr("disabled"))
        return;

    if (!ValidarLote()) return;

    var id = $("#hdnEmissaoSelo").val();

    $.ajax({
        url: "/emissaoselo/CancelarLote",
        type: "POST",
        data: { id },
        success: function (result) {
            hideLoading();
            if (result.tipo === "danger") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.message,
                    false,
                    null,
                    function () { });
            }
            else {
                hideLoading();

                openCustomModal(null,
                    null,
                    "success",
                    "Geração Selos",
                    "Selos Cancelados com Sucesso!",
                    false,
                    null,
                    function () {
                        window.location.href = '/EmissaoSelo/Edit/' + id;
                        $('#btnImprimir').attr('disabled', 'disabled');
                    });
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function Imprimir() {
    if ($('#btnImprimir').attr("disabled"))
        return;

    var idemissao = $("#hdnEmissaoSelo").val();
    var nomeImpressaoSelo = $("#nomeImpressaoSelo").val();

    if (!idemissao) {
        toastr.error('Nenhuma emissão para imprimir!', 'Informe uma Emissão!');
        return false;
    }

    if (!nomeImpressaoSelo) {
        toastr.error('Necessário preencher o nome para imprimir!', 'Informe um Nome para Impressão no Selo!');
        return false;
    }

    get(`Imprimir?idemissao=${idemissao}&nomeImpressaoSelo=${nomeImpressaoSelo}`)
        .done((response) => {
            let popupWin = window.open('', '_blank');
            popupWin.document.write(response);
            popupWin.document.close();
        })
        .fail((error) => {
            openCustomModal(null,
                null,
                //result.TipoModal,
                //result.Titulo,
                //result.Mensagem,
                "warning",
                "Geração Selos",
                "Não foi possível imprimir os selos!",
                false,
                null,
                function () { });
        });
}
function ImprimirProtocolo() {
    var idemissao = $("#hdnEmissaoSelo").val();

    if (!idemissao) {
        toastr.error('Nenhuma emissão para imprimir o protocolo!', 'Informe uma Emissão!');
        return false;
    }

    get(`ImprimirProtocolo?idEmissao=${idemissao}`)
        .done((response) => {
            let popupWin = window.open('', '_blank');
            popupWin.document.write(response);
            popupWin.document.close();
        })
        .fail((error) => {
            openCustomModal(null,
                null,
                //result.TipoModal,
                //result.Titulo,
                //result.Mensagem,
                "warning",
                "Protocolo",
                "Não foi possível gerar o protocolo!",
                false,
                null,
                function () { });
        });
}
function ImprimirEnvelope() {
    var idemissao = $("#hdnEmissaoSelo").val();

    if (!idemissao) {
        toastr.error('Nenhuma emissão para imprimir o envelope!', 'Informe uma Emissão!');
        return false;
    }

    get(`ImprimirEnvelope?idEmissao=${idemissao}`)
        .done((response) => {
            let popupWin = window.open('', '_blank');
            popupWin.document.write(response);
            popupWin.document.close();
        })
        .fail((error) => {
            openCustomModal(null,
                null,
                //result.TipoModal,
                //result.Titulo,
                //result.Mensagem,
                "warning",
                "Envelope",
                "Não foi possível gerar o envelope!",
                false,
                null,
                function () { });
        });
}

function ReiniciaSelos() {

    $.ajax({
        url: "/emissaoselo/ReiniciaSelos",
        type: "POST",
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

                $("#lista-selo-result").empty();
                $("#lista-selo-result").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function VerificaComValidade(idTipoSelo) {

    $.ajax({
        url: "/emissaoselo/VerificaComValidade",
        type: "POST",
        data: { idTipoSelo },
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
                if (result === false)
                    $("#validade-filtro").hide();
                else
                    $("#validade-filtro").show();
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}

function FiltrarPedidos() {

    var unidade = document.getElementById("unidade");
    //var cliente = document.getElementById("cliente");
    var convenio = document.getElementById("convenio");

    var clienteid = $("#cliente").val();
    var convenioId = $("#convenio").val();
    var unidadeid = $("#unidade").val();
    var tiposeloid = $("#tiposelo").val();
    var validade = $("#data-validade").val();

    let valido = true;

    if (unidadeid === "" || unidadeid === undefined || unidadeid <= 0) {
        toastr.error('Selecione uma Unidade!', 'Informe uma Unidade!');
        valido = false;
    }

    if (convenioId === "" || convenioId === undefined || convenioId <= 0) {
        toastr.error('Selecione um Convênio!', 'Informe um Convênio!');
        valido = false;
    }

    if (clienteid === "" || clienteid === undefined || clienteid <= 0) {
        toastr.error('Selecione um Cliente!', 'Informe um Cliente!');
        valido = false;
    }

    if (!valido)
        return false;

    var filtro = {
        Unidade: { Id: unidadeid },
        TipoSelo: { Id: tiposeloid },
        ValidadePedido: validade,
        Cliente: { Id: clienteid },
        Convenio: { Id: convenioId }
    };

    $.ajax({
        url: "/emissaoselo/FiltrarPedidoselos",
        type: "POST",
        data: { filtro },
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

                $("#lista-pedido-result").empty();
                $("#lista-pedido-result").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }
    });
}


function GerarSelos() {

    var dataValidade = $("#data-validade");
    if (dataValidade.is(":visible")) {
        if (!dataValidade.val()) {
            toastr.error('Preencha o campo \"Validade do Selo\"!', 'Informe uma Validade de Selo!');
            return false;
        }

        if (dataMenorQueDataAtual(dataValidade.val(), 1)) {
            toastr.error("O campo \"Validade do Selo\" deve ser preenchido com uma data superior a data atual!", 'Informe uma Validade de Selo válida!');
            return false;
        }
    }

    var entregarealizada = $("#entregarealizada").is(':checked');
    var data_entrega = $("#data-entrega").val();
    var clienteRemetente = $("#clienteRemetente").val();
    var responsavel = $("#responsavel").val();
    var quantidade = $("#quantidade").val();
    var unidade = document.getElementById("unidade");
    var tiposelo = document.getElementById("tiposelo");
    var idLote = $("#hdnEmissaoSelo").val();
    var unidadeid = $("#unidade").val();
    var tiposeloid = $("#tiposelo").val();
    var validade = $("#data-validade").val();

    var filtro = {
        EntregaRealizada: entregarealizada,
        DataEntrega: data_entrega,
        ClienteRemetente: clienteRemetente,
        Responsavel: responsavel
    };

    var objPedido;

    $("#tabela-pedido tbody").find('tr').each(function (rowIndex, r) {

        var checado = $(this).find('input[name="checado"]').is(':checked');

        var pedido = {
            Id: $(this).find('input[name="checado"]').attr('data-pedidoid')
        };

        if (checado)
            objPedido = pedido;

    });

    dataValidade = dataValidade.val();

    $.ajax({
        url: "/emissaoselo/GerarSelos",
        type: "POST",
        data: { filtro, objPedido, dataValidade },
        success: function (result) {
            hideLoading();
            if (result.Sucesso == false) {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () {
                    });
            }
            else {
                hideLoading();

                $("#lista-selo-result").empty();
                $("#lista-selo-result").append(result);
                $('#btnTransferir').attr("disabled", true);
                $('#btnImprimir').attr("disabled", true)
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();

        }
    });

}

function MensagemSelos() {
    toastr.error('Não é possível regerar os selos!', 'Atenção!');
}

//método foi desabilitado... o campo passou a ser digitavel pelo usuário
function LoadResponsavel() {

    var idresponsavel = $("#responsavel").val();

    $("#responsavel").html("");

    var unidadeid = $("#unidade").val();
    $.ajax({
        url: '/emissaoselo/AtualizaReponsaveis',
        data: { unidadeid },
        dataType: "json",
        type: "POST",
        success: function (data) {
            var equipeSelect = document.getElementById("responsavel");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione o responsável";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(data, function (i, item) {
                option = document.createElement("option");

                //if (item.Pessoa.Nome != null)
                option.text = item.Pessoa.Nome;
                //else
                option.text = '';

                option.value = item.Id;
                equipeSelect.options.add(option);
            });

            if (idresponsavel !== 0) {
                $("#responsavel").val(idresponsavel).change();
            }

            //$("#tipoEquipe").append("<option value=0>Selecione...</option>");

            //$.each(data, function (index, equipe) {
            //    $("#tipoEquipe").append("<option value=" + equipe.Id + ">" + equipe.Descricao + "</option>");
            //});
        },
        error: function (response) {
            console.log(response.responseText);
        },
        failure: function (response) {
            console.log(response.responseText);
        },
        complete: function () {


            //hideLoading();
        }
    });
}