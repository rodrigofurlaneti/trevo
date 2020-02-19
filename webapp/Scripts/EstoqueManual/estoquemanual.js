let materialEhUmAtivo = false;

$(document).ready(function () {
    ConfigTabelaGridSemCampoFiltroPrincipal();
    FormatarNumerosInteiros(".numero-inteiro");
    FormatarNumerosDecimais("#valortotal");

    if (!isView())
        AtualizarCamposHabilitados();

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });

    $("#estoque-manual-form").submit(function (e) {
        if (!ValidarCampos())
            e.preventDefault();
    });

    $("#quantidade, #preco").blur(function () {
        CalcularValorTotal();
    });

    $("input[type=radio][name=Acao]").change(function () {
        AtualizarCamposHabilitados();
    });

    $("#estoque-manual-form")
        .on("change", "#pedido-compra", function () {
            BuscarPedidoCompraMaterialFornecedores(this.value);
        })
        .on("change", "#material", function () {
            let materialId = $(this).val();
            let pedidoCompraId = $("#pedido-compra").val();

            if (materialId) {
                ChecarSeEhUmAtivo(materialId)
                    .done((ehUmAtivo) => {
                        materialEhUmAtivo = ehUmAtivo;
                        if (!ehUmAtivo) {
                            toastr.warning("Material não é um ativo", "Alerta");
                        }

                        AtualizarCamposHabilitados();
                        AtualizarExibicao();

                        if (pedidoCompraId) {
                            BuscarQuantidade(pedidoCompraId, materialId);
                        }
                    });
            }
        });

    $("input[type=radio][name=Acao], #estoque").change(function () {
        AtualizarExibicao();
    });
});

const BuscarQuantidade = function BuscarQuantidade(pedidoCompraId, materialId) {
    if (!materialId)
        materialId = 0;

    return post("BuscarQuantidade", { pedidoCompraId, materialId })
        .done((response) => {
            $("#quantidade").val(response);
        });
}

const BuscarPedidoCompraMaterialFornecedores = function BuscarPedidoCompraMaterialFornecedores(pedidoCompraId) {
    if (!pedidoCompraId)
        pedidoCompraId = 0;

    return post("BuscarPedidoCompraMaterialFornecedores", { pedidoCompraId })
        .done((response) => {
            $("#lista-materiais").empty().append(response);
        });
}

function ValidarCampos() {
    let acao = $("input[type=radio][name=Acao]:checked").val();

    if (!acao) {
        toastr.error("Selecione uma Ação", "Erro");
        return false;
    }

    if (!$("#estoque").val()) {
        toastr.error("Selecione um Estoque", "Erro");
        return false;
    }

    if (!$("#material").val()) {
        toastr.error("Selecione um Material", "Erro");
        return false;
    }

    switch (acao) {
        case "Entrada":
            if (!$("#quantidade").val()) {
                toastr.error("Informe a Quantidade", "Erro");
                return false;
            }

            if (!$("#preco").val() || $("#preco").val() == "0,00") {
                toastr.error("Informe o Preço", "Erro");
                return false;
            }

            if (materialEhUmAtivo && !$(".item-gerado").length) {
                toastr.error("Não é possível Salvar uma entrada de um Ativo sem gerar os itens!", "Erro");
                return false;
            }
            break;
        case "Saida":
            if (!$("#unidade").val()) {
                toastr.error("Informe uma Unidade", "Erro");
                return false;
            }
            if (materialEhUmAtivo && !$(".item-gerado input[name=selecionalinha]:checked").length) {
                toastr.error("Não é possível Salvar uma saída de um Ativo sem selecionar um item!", "Erro");
                return false;
            } else if (!materialEhUmAtivo && !$("#quantidade").val()) {
                toastr.error("Informe a Quantidade", "Erro");
                return false;
            }
            break;
        case "Inventario":
            if (!$("#motivo").val()) {
                toastr.error("Informe o Motivo", "Erro");
                return false;
            }
            if (materialEhUmAtivo && !$(".item-gerado input[name=selecionalinha]:checked").length) {
                toastr.error("Não é possível Salvar um Inventário de um Ativo sem selecionar um item!", "Erro");
                return false;
            } else if (!materialEhUmAtivo && !$("#quantidade").val()) {
                toastr.error("Informe a Quantidade", "Erro");
                return false;
            }
            break;
        default:
            break;
    }

    return true;
}

function AtualizarCamposHabilitados() {
    switch ($("input[type=radio][name=Acao]:checked").val()) {
        case "Entrada":
            $("#preco").unReadonly();
            $("#quantidade").unReadonly();
            $("#numero").unReadonly();
            $("#motivo").disabled().val("");
            $("#unidade").disabled().val("");
            $("#pedido-compra").unDisabled();

            if (materialEhUmAtivo)
                $("#gerar").unDisabled();

            break;
        case "Saida":
            $("#numero").readonly().val("");
            $("#preco").readonly().val("");
            $("#motivo").disabled().val("");
            $("#unidade").unDisabled();
            $("#pedido-compra").disabled().val("");

            materialEhUmAtivo ? $("#quantidade").readonly().val("") : $("#quantidade").unReadonly();

            break;
        case "Inventario":
            $("#numero").readonly().val("");
            $("#preco").readonly().val("");
            $("#motivo").unDisabled();
            $("#unidade").unDisabled();
            $("#pedido-compra").disabled().val("");

            materialEhUmAtivo ? $("#quantidade").readonly().val("") : $("#quantidade").unReadonly();

            break;
        default:
            break;
    }
}

function ChecarSeEhUmAtivo(materialId) {
    return post("ChecarSeEhUmAtivo", { materialId });
}

function AtualizarExibicao() {
    var material = document.getElementById("material");
    var estoque = document.getElementById("estoque");

    if (estoque.selectedIndex > 0) {
        if (material.selectedIndex > 0) {
            switch ($("input[type=radio][name=Acao]:checked").val()) {
                case "Entrada":
                    AtualizarExibicaoEntrada();
                    break;
                case "Saida":
                case "Inventario":
                    AtualizarExibicaoSaida();
                    break;
                default:
                    break;
            }
        }
    }
}

function AtualizarExibicaoEntrada() {
    return post("AtualizarExibicaoEntrada")
        .done((result) => {
            $("#lista-item-result").empty().append(result);
            $(".select-item").hide();
        });
}

function AtualizarExibicaoSaida() {
    var estoque = $("#estoque").val();
    var material = $("#material").val();

    var filtro = {
        Estoque: {
            Id: estoque
        },
        Material: {
            Id: material
        }
    }

    $.ajax({
        url: "/EstoqueManual/AtualizarExibicaoSaida",
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

                $("#lista-item-result").empty();
                $("#lista-item-result").append(result);
                $(".select-item").show();
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
        }
    });
}

function ArmazenarItensSelecionados(el, id) {
    let acao = $("input[type=radio][name=Acao]:checked").val();
    let estoqueId = $("#estoque").val();
    let unidadeId = $("#unidade").val();

    if (acao == "Saida") {
        if (unidadeId) {
            post("ArmazenarItensParaSaida", { id, estoqueId, unidadeId });
        } else {
            $(el).removeAttr("checked");
            toastr.warning("Selecione uma unidade", "Alerta");
        }
    } else if (acao == "Inventario") {
        post("ArmazenarItensParaInventario", { id });
    }
}

function GerarItens() {
    if (!$("#gerar").attr("disabled")) {
        let dto = {
            quantidade: $("#quantidade").val(),
            estoqueId: $("#estoque").val(),
            materialId: $("#material").val()
        }

        return post("GerarItens", dto)
            .done((result) => {
                $("#lista-item-result").empty();
                $("#lista-item-result").append(result);
            });
    }
}

function CalcularValorTotal() {
    let quantidade = $("#quantidade").int();
    let preco = $("#preco").decimal();
    let valorTotal = preco * quantidade;

    if (!isNaN(valorTotal)) {
        let valorComMascara = $("#valortotal").val(valorTotal.toFixed(2)).masked();
        $("#valortotal").val(valorComMascara);
    }
}