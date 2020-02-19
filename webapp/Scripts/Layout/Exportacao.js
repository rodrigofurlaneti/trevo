$(document).ready(function () {
    MakeChosen("exportFields");

    $(".formato").on("click", function () {
        ExibeCampoDelimitador($(this).attr("id"));
    });

    AutoCompleteField("nome-layout", "hdnLayoutId", "lblMessageResult", "/Layout/SuggestionLayout");

    //ConfigTabelaGridSemCampoFiltroPrincipal();
});

function AutoCompleteField(idField, idHidden, idMessageResult, url) {
    $("#" + idField).autocomplete({
        source: function (request, response) {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                data: { param: request.term, exact: false },
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            pessoa = item;
                            return { label: item.Nome, value: item.Nome };
                        }));
                }
            });
        },
        noResults: function () { },
        results: function () { },
        select: function (elem, obj) {
            $("#" + idMessageResult).text("");
            $("#" + idHidden).val(pessoa.Id);
        },
        change: function (event, ui) {
            ConfirmSearchPersonByName(url, idField, idHidden, idMessageResult, event.currentTarget.value);
        }
    });
}
function ConfirmSearchPersonByName(url, idField, idHidden, idMessageResult, param) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: { param: param, exact: true },
        success: function (person) {
            var idLayout = 0;
            $("#" + idHidden).val("");
            if (person !== undefined && person !== null && person.length > 0) {
                idLayout = person[0].Id;
                $("#" + idHidden).val(person[0].Id);
                $("#" + idField).val(person[0].Nome);
            }
            CarregarLayout(idLayout);
        }
    });
}
function CarregarLayout(id) {
    if (id <= 0)
        return;

    $.ajax({
        url: "/Layout/CarregarLayout",
        type: "POST",
        dataType: "json",
        data: {
            Id: parseInt(id)
        },
        success: function (response) {

            if (response.tipo == "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {
                LimparCampos();

                $("#nome-layout").attr("disabled", "disabled");

                $("#btnRemoveField").css("display", "none");
                $("#btnAtualizar").css("display", "none");
                $("#btnIncluir").css("display", "");

                $("#div-linhas-exportacao").html(response.divGridLinhas);
                $("#div-formato").html(response.divGridFormato);

                BloquearCamposParaVisualizacao(false);
                //ConfigTabelaGridSemCampoFiltroPrincipal();

                toastr.warning(response.message);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ExibeCampoDelimitador(id) {
    if (id == "Delimitador") {
        $("#txtDelimitador").css("display", "");
    } else {
        $("#txtDelimitador").css("display", "none");
        $("#txtDelimitador").val("");
    }
}

function SelecionarTodos(checked) {
    $("input[id*='chk_']").each(function () {
        this.checked = checked;
    });
}

function AdicionarCampo(codLinha) {
    var campo = $("#exportFields_" + codLinha).val();
    var conteudo = $("#txtConteudo").val();

    if (conteudo != "" && campo == "" || campo == null) {
        campo = "Fixo";
    }

    if (campo == "" || campo == null) {
        toastr.warning("Selecione um campo para adicionar a lista");
        return;
    }

    $.ajax({
        url: "/Layout/AddCampoExportacao",
        type: "POST",
        dataType: "json",
        data: { campo: campo, conteudo: conteudo, codLinha: codLinha },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                debugger;
                $("#div-campos-exportacao_" + codLinha).html(response.divGrid);

            }

            $("#exportFields_" + codLinha).val("").trigger("chosen:updated");
            $("#txtConteudo").val("");

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function DeletarSelecionados(ele, codLinha) {
    var listaCampo = [];

    var idEle = $(ele).attr("id").split("-")[1];
    var id = idEle.split("_")[1];
    var campo = idEle.split("_")[0];
    var conteudo = $("#hdnConteudo").val();
    listaCampo.push({ Id: id, Campo: campo, Conteudo: conteudo });


    if (listaCampo.length == 0) {
        toastr.warning("Selecione um ou mais itens para remover da listagem!");
        return;
    }

    $.ajax({
        url: "/Layout/DeleteCampoExportacao",
        type: "POST",
        dataType: "json",
        data: { listaCampo: JSON.stringify(listaCampo), codLinha: codLinha },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-campos-exportacao_" + codLinha).html(response.divGrid);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function ChangeData(ele, codLinha) {
    var field = $(ele).attr("id").split("-")[0];
    var idEle = $(ele).attr("id").split("-")[1];
    var conteudo = $("#hdnConteudo").val();
    var id = idEle.split("_")[1];
    var campo = idEle.split("_")[0];

    var posicaoIni = $("input[id='posicaoIni-" + idEle + "']").val();
    var posicaoFim = $("input[id='posicaoFim-" + idEle + "']").val();
    var tamanho = $("input[id='tamanho-" + idEle + "']").val();
    var preenchimento = $("input[id='preenchimento-" + idEle + "']").val();
    var formatacao = $("select[id='formatacao-" + idEle + "']").val();
    var direcao = $("select[id='direcao-" + idEle + "']").val();

    $.ajax({
        url: "/Layout/ChangeCampoExportacao",
        type: "POST",
        dataType: "json",
        data: {
            id: id,
            campo: campo,
            inicio: posicaoIni,
            final: posicaoFim,
            tamanho: tamanho,
            preenchimento: preenchimento,
            formatacao: formatacao,
            direcao: direcao,
            conteudo: conteudo,
            codLinha: codLinha
        },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-campos-exportacao_" + codLinha).html(response.divGrid);
                //ConfigTabelaGridSemCampoFiltroPrincipal();
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function IncluirFormato() {
    SalvarFormato("/Layout/IncluirFormato");
}
function AtualizarFormato() {
    SalvarFormato("/Layout/AtualizarFormato");
}
function SalvarFormato(url) {
    var valido = true;

    if ($(".item-campo-exportar").length <= 0) {
        valido = false;
        toastr.warning("Adicione um ou mais campos para o arquivo de exportação!");
    }

    if ($(".formato:checked").length <= 0) {
        valido = false;
        toastr.warning("Selecione um formato de arquivo!");
    } else {
        if ($(".formato:checked").first().val() == "Delimitador" &&
            $("#txtDelimitador").val() === "" || $("#txtDelimitador").val() == undefined) {
            valido = false;
            toastr.warning("Informe um caractere delimitador!");
        }
    }

    if ($("#descricao-formato").val() === "" || $("#descricao-formato").val() == undefined) {
        valido = false;
        toastr.warning("Informe uma descrição para o formato de arquivo!");
    }

    if (valido) {
        var idFormato = $("#hdnIdFormato").val() == "" ? 0 : parseInt($("#hdnIdFormato").val());
        var descricaoFormato = $("#descricao-formato").val();
        var formato = $(".formato:checked").first().val();
        var delimitador = $("#txtDelimitador").val();

        $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: {
                formato: {
                    Id: idFormato,
                    Descricao: descricaoFormato,
                    Formato: formato,
                    Delimitador: delimitador
                }
            },
            success: function (response) {

                if (response.message) {
                    toastr.warning(response.message);
                } else {

                    $("#btnRemoveField").css("display", "none");
                    $("#btnAtualizar").css("display", "none");
                    $("#btnIncluir").css("display", "");

                    LimparCampos();

                    $("#div-linhas-exportacao").html(response.divGridCampos);
                    $("#div-formato").html(response.divGridFormato);

                    //ConfigTabelaGridSemCampoFiltroPrincipal();

                    toastr.success("Formato de arquivo armazenado e pronto para Salvar os Dados!");
                }

                hideLoading();
            },
            error: function (error) {
                hideLoading();
                //alert(JSON.stringify(error));
            },
            beforeSend: function () {
                showLoading();
            }
        });
    }
}

function VisualizarFormato(id, descricao) {
    ExibirDadosFormato("/Layout/VisualizarFormato", id, descricao, true);
}
function EditarFormato(id, descricao) {
    ExibirDadosFormato("/Layout/EditarFormato", id, descricao, false);
}
function ExibirDadosFormato(url, id, descricao, bloquear) {
    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: {
            id: id, descricao: descricao
        },
        success: function (response) {
            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#btnRemoveField").css("display", "none");
                $("#btnAtualizar").css("display", bloquear ? "none" : "");
                $("#btnIncluir").css("display", "none");

                LimparCampos();

                //popular
                $("#hdnIdFormato").val(response.Formato.Id);
                $("#descricao-formato").val(response.Formato.Descricao);
                $(".formato").each(function () {
                    var formatoEnum = response.Formato.Formato == 1
                        ? "Excel"
                        : response.Formato.Formato == 2
                            ? "Delimitador"
                            : "Posicional (TXT)";

                    if ($(this).attr("id") == formatoEnum) {
                        $(this).prop("checked", "checked");
                    } else {
                        $(this).prop("checked", "");
                    }

                    if (response.Formato.Formato == 2) {
                        $("#txtDelimitador").css("display", "");
                    }
                });
                $("#txtDelimitador").val(response.Formato.Delimitador);

                $("#div-linhas-exportacao").html(response.divGridCampos);

                //ConfigTabelaGridSemCampoFiltroPrincipal();

                BloquearCamposParaVisualizacao(bloquear);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function DeletarFormato(id, descricao) {
    var limparGridCampos = $("#descricao-formato").val() == descricao;

    $.ajax({
        url: "/Layout/DeletarFormato",
        type: "POST",
        dataType: "json",
        data: {
            id: id, descricao: descricao, limparGridCampos: limparGridCampos
        },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {

                $("#btnRemoveField").css("display", "none");
                $("#btnAtualizar").css("display", "none");
                $("#btnIncluir").css("display", "");

                $("#div-formato").html(response.divGridFormato);
                if (limparGridCampos) {
                    LimparCampos();

                    $("#div-campos-exportacao").html(response.divGridCampos);

                    BloquearCamposParaVisualizacao(false);
                }

                //try {
                //    ConfigTabelaGridSemCampoFiltroPrincipal();
                //}
                //catch(e){};

                toastr.success("Formato de arquivo retirado da listagem!<br><br>Salve os dados para exclu-í-lo definitivamente.");
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function DeletarExportacao(id) {
    var funcao = function () {
        ConfirmarExclusaoLayout(id);
        $('#modalToHtml').modal('toggle');
    };
    ModalHtml("danger",
        "Atenção",
        "Deseja realmente excluir este layout de exportação?",
        "Sim, Desejo!",
        funcao);
}
function ConfirmarExclusaoLayout(id) {
    $.ajax({
        url: "/Layout/DeletarExportacao",
        type: "POST",
        dataType: "json",
        data: {
            id: id
        },
        success: function (response) {

            if (response.tipo == "danger") {
                toastr.error(response.message);
            } else {
                $("#div-layouts").html(response.divGridExportacao);

                //ConfigTabelaGridSemCampoFiltroPrincipal();

                toastr.success(response.message);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function SalvarDadosExportacao() {
    var idLayout = $("#hdnLayoutId").val();
    var nomeLayout = $("#nome-layout").val();

    $.ajax({
        url: "/Layout/SalvarDadosExportacao",
        type: "POST",
        dataType: "json",
        data: {
            viewModel: {
                Id: idLayout,
                Nome: nomeLayout
            }
        },
        success: function (response) {

            if (response.tipo == "danger") {
                toastr.error(response.message);
                hideLoading();
                return;
            } else {

                $("#btnRemoveField").css("display", "none");
                $("#btnAtualizar").css("display", "none");
                $("#btnIncluir").css("display", "");

                $("#hdnLayoutId").val("");
                $("#nome-layout").val("");
                LimparCampos();
                $("#div-campos-exportacao").html(response.divGridCampos);
                $("#div-formato").html(response.divGridFormato);
                $("#div-layouts").html(response.divGridExportacao);
                BloquearCamposParaVisualizacao(false);

                toastr.success(response.message);
            }

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}

function LimparCampos() {
    $("#hdnIdFormato").val("");
    $("#descricao-formato").val("");
    $(".formato").each(function () {
        $(this).prop("checked", "");
    });
    $("#txtDelimitador").val("");
    $("#txtDelimitador").css("display", "none");
    $("#exportFields").val("").trigger("chosen:updated");
}
function BloquearCamposParaVisualizacao(bloquear) {
    if (bloquear) {
        $("select").prop("disabled", "disabled").trigger("chosen:updated");
        $("#btnAddField").css("display", "none");
        $("#btnRemoveField").css("display", "none");

        $("#descricao-formato").attr("disabled", "disabled");
        $("#txtDelimitador").attr("disabled", "disabled");
        $("[type*=radio]").prop("disabled", "disabled");
        $("[type*=checkbox]").prop("disabled", "disabled");
        $(".grid-field").each(function () { $(this).prop("disabled", "disabled"); });
    } else {
        $("select").removeAttr("disabled").trigger("chosen:updated");
        $("#btnAddField").css("display", "");
        $("#btnRemoveField").css("display", "");

        $("#descricao-formato").removeAttr("disabled");
        $("#txtDelimitador").removeAttr("disabled");
        $("[type*=radio]").removeAttr("disabled");
        $("[type*=checkbox]").removeAttr("disabled");
        $(".grid-field").each(function () { $(this).removeAttr("disabled"); });
    }
}

function ModalHtml(tipo, titulo, mensagem, textoBotao, acaoBotao) {
    $("#modal-content").removeClass("panel-success");
    $("#modal-content").removeClass("panel-danger");
    $("#modal-content").removeClass("panel-warning");
    $("#modal-content").removeClass("panel-info");

    $("#modal-content").addClass("panel-" + tipo);
    $("#modalToHtmlTitle").html(titulo);
    $("#modalToHtmlBody").html(mensagem);
    $("#btnModalToHtmlAction").html(textoBotao);
    $("#btnModalToHtmlAction").val(textoBotao);
    $("#btnModalToHtmlAction").on("click", acaoBotao);

    $("#modalToHtml").modal({
        backdrop: "static",
        keyboard: false
    });
}

function AdicionarLinha() {
    var codLinha = $("#txtCodLinha").val();
    var tipoLinha = $("#txtLinha").val();
    if (tipoLinha == "" || tipoLinha == null) {
        toastr.warning("Informe a linha para cadastro");
        return;
    }

    if (codLinha == "" || codLinha == null) {
        toastr.warning("Informe o código da linha a ser cadastrado");
        return;
    }

    $.ajax({
        url: "/Layout/AddLinhaExportacao",
        type: "POST",
        dataType: "json",
        data: { codLinha: codLinha, tipoLinha: tipoLinha },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-linhas-exportacao").html(response.divGrid);
            }

            $("#btnRemoveLine").css("display", response.ExibirBotaoExcluir ? "" : "none");

            $("#txtCodLinha").val("");
            $("#txtLinha").val("");

            hideLoading();
        },
        error: function (error) {
            hideLoading();
            //alert(JSON.stringify(error));
        },
        beforeSend: function () {
            showLoading();
        }
    });
}
