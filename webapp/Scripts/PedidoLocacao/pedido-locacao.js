let lancamentosAdicionais = [];

$(document).ready(function () {
    FormatarTodosCampos();

    ChecarSePossuiFiador();
    ChecarSePossuiCicloMensal();

    $("[name=PossuiFiador]").change(function () {
        ChecarSePossuiFiador();
    });

    $("[name=PossuiCicloMensal]").change(function () {
        ChecarSePossuiCicloMensal();
    });

    $("#unidade").change(function () {
        if (this.value) {
            BuscarClientesDaUnidade(this.value)
                .done(() => AtualizarTipoLocacao(this.value));
        } else {
            AtualizarTipoLocacao(0);
        }
    });

    $("#pedido-locacao-form").submit(function (e) {
        if (!ValidarCampos()) {
            e.preventDefault();
            return false;
        }
        if (!VerificacaoBloqueioReferencia())
            return false;
    });

    $("#valor").blur(function () {
        AtualizarValorTotal();
    });

    if (isEdit() || isSave()) {
        BuscarLancamentosAdicionais()
            .done(() => {
                BuscarPedidosLocacao()
                    .done(() => {
                        BuscarClientesDaUnidade($("#unidade").val())
                            .done(() => {
                                AtualizarTipoLocacao($("#unidade").val());
                            });
                    });
            });
    } else {
        BuscarPedidosLocacao();
    }
});

function AtualizarTipoLocacao(unidadeId) {
    return get(`AtualizarTipoLocacao?unidadeId=${unidadeId}`)
        .done((response) => {
            $("#tipo-locacoes").empty().append(response.Grid);

            let tipoLocacaoSelecionada = $("#tipo-locacao-selecionada").val();
            if (tipoLocacaoSelecionada)
                $("#tipo-locacao").val(tipoLocacaoSelecionada);

            MakeChosen("tipo-locacao", null, "100%");

            if (!response.TemLocacoes)
                toastr.warning("É necessário parametrizar o tipo de locação para essa unidade.", "Unidade sem parametrização");
        });
}

function ValidarCampos() {
    let dadosValidos = true;
    let mensagem = "";

    if (!$("#unidade").val()) {
        mensagem += "O campo Unidade deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#cliente").val()) {
        mensagem += "O campo Cliente deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#tipo-locacao").val()) {
        mensagem += "O campo Tipo Locação deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    let valor = parseFloat($("#valor").val().replace(",", "."));
    if (isNaN(valor) || valor <= 0) {
        mensagem += "O campo Valor deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#data-reajuste").val()) {
        mensagem += "O campo Reajuste deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#tipo-reajuste").val()) {
        mensagem += "O campo Tipo Reajuste deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#prazo-reajuste").val()) {
        mensagem += "O campo Prazo Reajuste deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    let valorReajuste = parseFloat($("#valor-reajuste").val().replace(",", "."));
    if (isNaN(valorReajuste) || valorReajuste <= 0) {
        mensagem += "O campo Valor Reajuste deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#forma-pagamento").val()) {
        mensagem += "O campo Forma de Pagamento deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#data-primeiro-pagamento").val()) {
        mensagem += "O campo Data do Primeiro Pagamento deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#data-demais-pagamentos").val()) {
        mensagem += "O campo Data demais Pagamento deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    let valorPrimeiroPagamento = parseFloat($("#valor-primeiro-pagamento").val().replace(",", "."));
    if (isNaN(valorPrimeiroPagamento) || valorPrimeiroPagamento <= 0) {
        mensagem += "O campo Valor do Primeiro Pagamento deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#ciclo-pagamentos").val() && $("[name=PossuiCicloMensal]:checked").val() === "false") {
        mensagem += "O campo Ciclo de Pagamentos em Dias deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    if (!$("#data-vigencia-inicio").val()) {
        mensagem += "O campo Data Início da Vigência deve ser preenchido!<br/><br/>";
        dadosValidos = false;
    }

    //A pedido do Felipe retirar a validação!
    //if (!$("#data-vigencia-fim").val()) {
    //    mensagem += "O campo Data Fim da Vigência deve ser preenchido!<br/><br/>";
    //    dadosValidos = false;
    //}

    if (!dadosValidos) {
        toastr.error(mensagem, "Validação de Campos");
        return dadosValidos;
    }

    return true;
}

function BuscarClientesDaUnidade(unidadeId) {
    $("#cliente").empty();

    return post("BuscarClientesDaUnidade", { unidadeId })
        .done((result) => {
            $("#cliente").append('<option value="">Selecione...</option>');

            $.each(result,
                function (i, result) {
                    $("#cliente").append('<option value="' + result.Id + '">' + result.Descricao + '</option>');
                });

            let clienteSelecionado = $("#cliente-selecionado").val();
            if (clienteSelecionado)
                $("#cliente").val(clienteSelecionado);

            MakeChosen("cliente", null, "100%");
        });
}

function BuscarPedidosLocacao() {
    return post("BuscarPedidosLocacao")
        .done((result) => {
            $("#lista-pedido-locacao").empty().append(result);
        })
        .then(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal("#datatable_fixed_column_locacao");
        });
}

function AdicionarLancamentoAdicional() {
    let lancamentoAdicional = {
        Id: 0,
        Descricao: $("#lancamento-adicional-descricao").val(),
        Valor: $("#lancamento-adicional-valor").val()
    };
    lancamentosAdicionais.push(lancamentoAdicional);

    AtualizarLancamentosAdicionais();
}

function RemoverLancamentoAdicional(indice) {
    lancamentosAdicionais.splice(indice, 1);
    AtualizarLancamentosAdicionais();
}

function EditarLancamentoAdicional(indice) {
    var lancamentoAdicional = lancamentosAdicionais.splice(indice, 1);

    AtualizarLancamentosAdicionais();

    $("#lancamento-adicional-descricao").val(lancamentoAdicional[0].Descricao);
    $("#lancamento-adicional-valor").val(lancamentoAdicional[0].Valor);
}

function AtualizarLancamentosAdicionais() {
    limparCampos();
    AtualizarValorTotal();

    return post("AtualizarLancamentosAdicionais", { lancamentosAdicionais })
        .done((result) => {
            $("#lista-pedido-locacao-lancamentos-adicionais").empty();
            $("#lista-pedido-locacao-lancamentos-adicionais").append(result);
        });
}

function BuscarLancamentosAdicionais() {
    return post("BuscarLancamentosAdicionais")
        .done((result) => {
            lancamentosAdicionais = result;
        });
}

function limparCampos() {
    $("#lancamento-adicional-descricao").val("");
    $("#lancamento-adicional-valor").val("");
}

function FormatarTodosCampos() {
    MakeChosen("unidade", null, "100%");
    MakeChosen("cliente", null, "100%");
    MakeChosen("tipo-locacao", null, "100%");
    MakeChosen("desconto", null, "100%");
    MakeChosen("tipo-reajuste", null, "100%");
    MakeChosen("prazo-reajuste", null, "100%");
    MakeChosen("forma-pagamento", null, "100%");
    FormatarCampoData("data-reajuste");
    FormatarCampoData("data-primeiro-pagamento");
    FormatarCampoData("data-demais-pagamentos");
    FormatarCampoData("data-vigencia-inicio");
    FormatarCampoData("data-vigencia-fim");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
}

function ChecarSePossuiFiador() {
    if ($("[name=PossuiFiador]:checked").val() === "true") {
        $("#nome-fiador").removeAttr("readonly");
        $("#forma-garantia").val("");
        $("#forma-garantia").attr("readonly", "readonly");
    } else {
        $("#nome-fiador").val("");
        $("#nome-fiador").attr("readonly", "readonly");
        $("#forma-garantia").removeAttr("readonly");
    }
}

function ChecarSePossuiCicloMensal() {
    if ($("[name=PossuiCicloMensal]:checked").val() === "true") {
        $("#ciclo-pagamentos").attr("readonly", "readonly");
        $("#ciclo-pagamentos").val("");
    } else {
        $("#ciclo-pagamentos").removeAttr("readonly");
    }
}

function AtualizarValorTotal() {
    let valor = parseFloat($("#valor").val().replace(".", "").replace(",", "."));
    valor = !isNaN(valor) ? valor : 0;

    let valorTotalLancamentos = 0;
    for (var i = 0; i < lancamentosAdicionais.length; i++) {
        let valorLancamento = parseFloat(lancamentosAdicionais[i].Valor.replace(".", "").replace(",", "."));
        valorLancamento = !isNaN(valorLancamento) ? valorLancamento : 0;

        valorTotalLancamentos += valorLancamento;
    }

    $("#valor-total").val((valor + valorTotalLancamentos).toFixed(2).toString().replace(".", ","));

    $("#valor-total").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
}

function VerificacaoBloqueioReferencia() {
    var retorno = true;
    showLoading();
    $.ajax({
        url: "/PedidoLocacao/VerificacaoBloqueioReferencia",
        type: "POST",
        dataType: "json",
        async: false,
        data: {
            pedidoLocacaoViewModel: {
                Id: $("#Id").val(),
                DataPrimeiroPagamento: $("#data-primeiro-pagamento").val()
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