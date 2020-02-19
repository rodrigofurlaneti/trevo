$(document).ready(function () {
    //ConfigTabelaGridSemCampoFiltroPrincipal();
    ResetCarteirasChosen();

    FormatarCampoData("dtNascimento");
    FormataCampoCpf("cpf");
    FormataCampoCpf("cpfBusca");
    $("#Nome").keyup(function () {
        if (!somenteLetra(this.value)) {
            if (!somenteLetra(this.value.substring(this.value.length - 1, this.value.length)))
                this.value = this.value.substring(0, this.value.length - 1);
        }
    });

    $('#cpf').on('blur', function () {
        VerificarCPF();
    });

    setTimeout(() => {
        $('#wid-id-1 .jarviswidget-toggle-btn').click();
    }, 600);

    $("#DevedorForm").submit(function (e) {
        var dadosValidos = true;

        if (!verificaData($("#dtNascimento").val())) {
            toastr.error('Preencha uma Data de Nascimento Inválida!', 'Data de Nascimento Inválida!');
            dadosValidos = false;
        }

        if (!VerificarCPF()) {
            dadosValidos = false;
        }

        if (!somenteLetra($("#Nome").val())) {
            toastr.error("O campo nome não permite caracteres númericos!", "Nome inválido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    MakeChosen("credores", null);
    MakeChosen("produtos", null);
    MakeChosen("carteirasBusca", null);

});

function GetCarteirasByFuncao(ele) {
    $("#carteiras").empty();
    $("#carteiras").trigger("chosen:update");
    $("#carteiras").val("").trigger("chosen:updated");

    $.ajax({
        url: "/devedor/Carteiras",
        type: "POST",
        dataType: "json",
        success: function (carteiras) {
            hideLoading();

            $("#lblMensagemCarteiraResultado").text("");

            if (carteiras !== undefined && carteiras !== null && carteiras.length > 0) {
                $(carteiras).each(function () {
                    $("<option />", {
                        val: this.Id,
                        text: this.RazaoSocial
                    }).appendTo("#carteiras");
                });
                ResetCarteirasChosen();

            } else {
                $("#lblMensagemCarteiraResultado").text("Não possui carteiras disponíveis!");
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

function ResetCarteirasChosen() {
    //Entidade.Uteis.Constantes.Funcao [Enum]
    var optionsCarteira = $("#carteiras > option").length;
    var lojaLength = optionsCarteira === undefined || optionsCarteira === null ? 0 : optionsCarteira;
    var maxSelectedOptions = lojaLength;
    MakeChosen("carteiras", maxSelectedOptions);

    $("#carteiras").off("change");
    $("#carteiras").change(function (event) {
        var obj = [];
        $("select#carteiras option:selected").each(function () {
            obj.push({ Id: $(this).val(), CarteiraProduto: $(this).text() });
        });

        $.ajax({
            url: "/devedor/carteirasSelecionadas",
            type: "POST",
            dataType: "json",
            data: { json: JSON.stringify(obj) },
            success: function (response) {
                hideLoading();
            },
            error: function (error) {
                hideLoading();
            },
            beforeSend: function () {
                showLoading();
            }
        });
    });
}

function AddBlacklist(tipo, parametro, tipoMotivoBusca) {
    $("#hdnParametro").val(parametro);
    $("#hdnTipo").val(tipo);

    $.ajax({
        url: "/devedor/PopularMotivos",
        datatype: "JSON",
        type: "GET",
        data: { tipoMotivoBusca: tipoMotivoBusca },
        success: function (response) {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                if (tipoMotivoBusca === "2") {
                    $("#msgBlacklist").innerHTML = "Deseja remover o registro da blacklist?";
                } else {
                    $("#msgBlacklist").innerHTML = "Deseja adicionar o registro na blacklist?";
                }

                $("#drop-result").empty();
                $("#drop-result").append(response);
            }
        },
        beforeSend: function () {
            showLoading();
        }
    });
    hideLoading();
    $("#myModalBlacklist").modal();
}

function ComfirmarBlacklist() {
    var parametro = $("#hdnParametro").val();
    var tipo = $("#hdnTipo").val();
    var idMotivo = $("#ddlMotivo").val();
    var descMotivo = $("#ddlMotivo option:selected").text();

    $("#myModalBlacklist").modal("hide");
    showLoading();

    $.ajax({
        url: "/devedor/ComfirmarBlacklist",
        type: "POST",
        //dataType: "json",
        data: {
            tipo: tipo, parametro: parametro, idMotivo: idMotivo, descMotivo: descMotivo
        },
        success: function (result) {
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
                if (tipo === "endereco") {
                    $("#lista-endereco-result").empty();
                    $("#lista-endereco-result").append(result);
                } else {
                    $("#lista-contato-result").empty();
                    $("#lista-contato-result").append(result);
                }
            }
        },
        error: function (xhr, status, errorThrown) {
            debugger;
            //Here the status code can be retrieved like;
            xhr.status;

            //The message added to Response object in Controller can be retrieved as following.
            xhr.responseText;
        }
    });
    hideLoading();
}

function ModalHistorico(idDevedor) {
    showLoading();
    $.ajax({
        url: "/devedor/ObterHistorico",
        datatype: "JSON",
        type: "GET",
        data: { idDevedor: JSON.stringify(idDevedor) },
        success: function (response) {
            if (typeof (response) === "object") {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            }
            else {
                $("#grid-result").empty();
                $("#grid-result").append(response);
                $("#grid-result table").dataTable();
            }
        },
        complete: function () {
            hideLoading();
            $("#myModalSituacao").modal();
        }
    });
}

function VerificarCPF() {
    var cpf = $("#cpf").val();

    if (!CPFValido(cpf)) {
        toastr.error("Preencha um CPF válido!", "CPF Inválido!");
        return false;
    }

    return true;
}

//function BuscarDevedores() {
//    var carteiras = [];
//    var credores = [];
//    var produtos = [];
//    var cpf = $("#cpfBusca").val();
//    $("select#carteirasBusca option:selected").each(function () {
//        var id = $(this).val();
//        carteiras.push({ Id: id });
//    });
//    $("select#credores option:selected").each(function () {
//        var id = $(this).val();
//        credores.push({ Id: id });
//    });
//    $("select#produtos option:selected").each(function () {
//        var id = $(this).val();
//        produtos.push({ Id: id });
//    });
//    $.ajax({
//        url: "/devedor/BuscarDevedores",
//        type: "POST",
//        dataType: "json",
//        data: {
//            carteiras: JSON.stringify(carteiras),
//            produtos: JSON.stringify(produtos),
//            credores: JSON.stringify(credores),
//            cpf: cpf
//        },
//        success: function (result) {
//            if (typeof (result) === "object") {
//                openCustomModal(null,
//                    null,
//                    result.TipoModal,
//                    result.Titulo,
//                    result.Mensagem,
//                    false,
//                    null,
//                    function () { });
//            }
//            else {
//                $("#lista-devedores").empty();
//                $("#lista-devedores").append(result);
//            }
//        },
//        error: function (error) {
//            //if (typeof (error) === "object") {

//                $("#lista-devedores").empty();
//                $("#lista-devedores").append(error.responseText);
//                hideLoading();
//            //    openCustomModal(null,
//            //        null,
//            //        result.TipoModal,
//            //        result.Titulo,
//            //        result.Mensagem,
//            //        false,
//            //        null,
//            //        function () { });
//            //}
//            //else {
//            //    $("#lista-devedores").empty();
//            //    $("#lista-devedores").append(error.responseText);
//            //}
//            //hideLoading();
//        },
//        beforeSend: function () {
//            showLoading();
//        },
//        complete: function () {
//            hideLoading();
//        }
//    });
//}

function BuscarDevedores() {
    var _credorSelecionado = $("#CodigoCredor option:selected").val();
    var _ProdutoSelecionado = $("#CodigoProduto option:selected").val();
    var _CarteiraSelecionado = $("#CodigoCarteira option:selected").val();
    var cpf = $("#Cpf").val();

    if (typeof _credorSelecionado == 'undefined') { _credorSelecionado = 0; }
    if (typeof _ProdutoSelecionado == 'undefined') { _ProdutoSelecionado = 0; }
    if (typeof _CarteiraSelecionado == 'undefined') { _CarteiraSelecionado = 0; }

    $.ajax({
        url: "/devedor/BuscarDevedores",
        type: "POST",
        dataType: "json",
        data: {
            credor: _credorSelecionado,
            produto: _ProdutoSelecionado,
            carteira: _CarteiraSelecionado,
            cpf: cpf
        },
        success: function (result) {
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
                $("#lista-devedores").empty();
                $("#lista-devedores").append(result);
            }
        },
        error: function (error) {
            $("#lista-devedores").empty();
            $("#lista-devedores").append(error.responseText);
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

function buscarDivida(id) {
    $.ajax({
        url: "/devedor/BuscarDivida",
        type: "GET",
        dataType: "json",
        data: { id: id },
        success: function (result) {
            debugger;
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-situacao").empty();
                $("#grid-divida").empty();
                $("#grid-divida").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }, error: function (error) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-situacao").empty();
                $("#grid-divida").empty();
                $("#grid-divida").append(error.responseText);
            }
            hideLoading();
        },
    });
}


function buscaSituacao(id) {
    $.ajax({
        url: "/devedor/ObterHistorico",
        type: "GET",
        dataType: "json",
        data: { idDevedor: JSON.stringify(id) },
        success: function (result) {
            debugger;
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-divida").empty();
                $("#grid-situacao").empty();
                $("#grid-situacao").append(result);
            }
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        }, error: function (error) {
            if (typeof (result) === "object") {
                openCustomModal(null,
                    null,
                    result.TipoModal,
                    result.Titulo,
                    result.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                $("#grid-divida").empty();
                $("#grid-situacao").empty();
                $("#grid-situacao").append(error.responseText);
            }
            hideLoading();
        },
    });
}