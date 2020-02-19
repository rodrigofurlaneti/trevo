$(document).ready(function () {
    FormatarCampoData("DataDe");
    FormatarCampoData("DataAte");
});

function BuscaParametros() {
    $("#CodigoParametro").empty();
    $.ajax({
        type: 'POST',
        url: '/Boletagem/CarregarParametros',
        dataType: 'json',
        success: function (result) {
            $.each(result,
                function (i, result) {
                    $("#CodigoParametro").append('<option value="' + result.Value + '">' + result.Text + '</option>');
                });
            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function VisualizarContratos() {
    var paramCalculo = $("#CodigoParametro").val();
    var dataVencimento = $("#DataVencimento").val();
    var plano = $("#PlanoEscolhido").val();
    var agrupar = $("input[name='Agrupar']:checked").val().toLowerCase() === "true" ? 1 : 0;
    var valorMinAVista = $("#ValorMinAVista").val().replace('.', '');
    var valorMinParcela = $("#ValorMinParcela").val().replace('.', '');

    var msg = "";
    if (paramCalculo == "" || paramCalculo == "0" || paramCalculo == undefined)
        msg += "Selecione um Parâmetro de Cálculo<br />";
    if (dataVencimento == "" || dataVencimento == "0" || dataVencimento == undefined || dataVencimento == null || isValidDate(dataVencimento) == false)
        msg += "Informe uma Data de Vencimento<br />";
    if (plano == "" || plano == "0" || plano == undefined)
        msg += "Informe Número de Plano<br />";

    if (msg != "") {
        toastr.warning(msg, "Atenção!");
    }
    else {
        var dadosJson = {
            ParametroCalculo: paramCalculo,
            DataVencimentoLote: dataVencimento,
            AgrupaContratos: agrupar,
            Plano: plano,
            ValorMinimo: valorMinAVista,
            ValorMinimoParcela: valorMinParcela
        };

        $.ajax({
            type: 'POST',
            url: '/Boletagem/VisualizarDadosBoletagem',
            dataType: 'json',
            data: dadosJson,
            success: function (response) {
                if (typeof (response) === "object" && response.TipoModal != undefined) {
                    openCustomModal(null,
                        null,
                        response.TipoModal,
                        response.Titulo,
                        response.Mensagem,
                        false,
                        null,
                        function () { });
                } else {
                    $("#grid-result").empty();
                    $("#grid-result").html(response.divVisualizarBoletagem);
                    $("#myModalVisualizar").modal({ backdrop: 'static', keyboard: false });
                }

                hideLoading();
            },
            error: function (ex) {
                hideLoading();
            },
            beforeSend: function () {
                showLoading();
            }
        });
    }
}
function Pesquisar() {
    var credor = $("#CodigoCredor").val();
    var produto = $("#CodigoProduto").val();
    var carteira = $("#CodigoCarteira").val();
    var dataInicio = $("#DataDe").val();
    var dataFim = $("#DataAte").val();
    var tipoLote;

    if (dataInicio > dataFim) {
        toastr.error("Data de Início não pode ser maior que a Data Final!");
        return false;
    }

    if (credor === null || credor === 0) {
        toastr.error("Selecione um Credor", "Credor não selecionado!");
        return false;
    }

    if (produto === null || produto === 0) {
        toastr.error("Selecione um Produto", "Produto não selecionado!");
        return false;
    }

    if (carteira === null || carteira === 0) {
        toastr.error("Selecione um Carteira", "Carteira não selecionado!");
        return false;
    }

    var radios = document.getElementsByName('TipoLote');

    for (var i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {
            tipoLote = radios[i].value
            break;
        }
    }

    if (tipoLote === null || tipoLote === undefined) {
        toastr.error("Selecione um Tipo de Lote", "Tipo de Lote não selecionado!");
        return false;
    }

    $.ajax({
        url: "/Boletagem/Pesquisar",
        type: "POST",
        dataType: "json",
        data: {
            credor: credor,
            produto: produto,
            carteira: carteira,
            dataInicio: dataInicio,
            dataFim: dataFim,
            tipoLote: tipoLote
        },
        success: function (result) {
            if (result.message) {
                openCustomModal(null,
                    null,
                    result.tipo,
                    "Atenção",
                    result.message,
                    false,
                    null,
                    function () { });
            } else {
                $("#divGridPesquisa").empty();
                $("#divGridPesquisa").html(result.divGridPesquisaLote);
            }

            hideLoading();
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

function RemoveItemPesquisaContrato(idParcela) {
    $.ajax({
        url: "/Boletagem/RemoveItemPesquisaContrato",
        type: "POST",
        dataType: "json",
        data: {
            idParcela: idParcela
        },
        success: function (result) {
            if (result.message) {
                openCustomModal(null,
                    null,
                    result.tipo,
                    "Atenção",
                    result.menssage,
                    false,
                    null,
                    function () { });
            } else {
                $("#divGridPesquisa").empty();
                $("#divGridPesquisa").html(result.divGridPesquisaLote);
            }

            hideLoading();
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

function FecharModal() {
    $("#myModalVisualizar").modal("hide");
}
function FecharModalLote() {
    $("#myModalLote").modal("hide");
}

function AbrirModalLote() {
    $.ajax({
        type: 'POST',
        url: '/Boletagem/AbrirModalLote',
        dataType: 'json',
        success: function (response) {
            if (typeof (response) === "object" && response.TipoModal != undefined) {
                openCustomModal(null,
                    null,
                    response.TipoModal,
                    response.Titulo,
                    response.Mensagem,
                    false,
                    null,
                    function () { });
            } else {
                var tipoLote = response.TipoLote;

                $("#hdnTipoLote").val(response.TipoLote);
                $("#lblNumeroLote").text(response.Lote);
                $("#lblTotalLote").text(response.TotalLote);

                $("#ddlPlanoLote").empty();
                if (tipoLote == 3) {
                    $("#areaPlanoLote").css("display", "none");
                }
                else {
                    $("#areaPlanoLote").css("display", "");

                    $.each(response.ListaPlanos,
                        function (i, item) {
                            $("#ddlPlanoLote").append('<option value="' + item.Id + '">' + item.Descricao + '</option>');
                        });
                    $("#ddlPlanoLote").val(response.Plano);
                }

                $.each(response.ListaFaturamento,
                    function (i, item) {
                        $("#ddlFaturamento").append('<option value="' + item.Id + '">' + item.Descricao + '</option>');
                    });

                $("#txtDescricaoLote").val(response.Descricao);
                
                $("#myModalLote").modal({ backdrop: 'static', keyboard: false });
            }

            hideLoading();
        },
        error: function (ex) {
            hideLoading();
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function GerarArquivoConfirm() {
    var tipoLote = $("#hdnTipoLote").val();
    var plano = $("#ddlPlanoLote").val();
    var descricaoLote = $("#txtDescricaoLote").val();
    var faturamento = $("#ddlFaturamento").val();

    var msg = "";

    if (tipoLote != 3 && (plano == "" || plano == "0" || plano == undefined))
        msg += "Informe Número de Plano<br />";

    if (descricaoLote == "" || descricaoLote == undefined)
        msg += "Informe uma Descrição do Lote";

    if (faturamento == "" || faturamento == "0" || faturamento == undefined)
        msg += "Selecione um Parametro de Faturamento<br />";

    if (msg != "") {
        toastr.warning(msg, "Atenção!");
    }
    else {
        $.ajax({
            url: '/Boletagem/GerarArquivo',
			datatype: "JSON",
			type: "GET",
            data: {
                planoEscolhido: plano,
                descricaoLote: descricaoLote,
                parametroFaturamento: faturamento
            },
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
                } else {
                    window.open("data:application/force-download;filename=ArquivoRemessa.txt;base64," + btoa(response));

                    $("#myModalLote").modal("hide");
					$("#myModalVisualizar").modal("hide");

					openCustomModal(null,
						null,
						"success",
						"Campanha Boletagem",
						"Processo concluido!",
						false,
						null,
						function () { });
                }

                hideLoading();
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
}