$(document).ready(function () {
    //$("select[id*='importFields_']").each(function () {
    //    MakeChosen($(this).attr("id"));
    //});
    //$("select[id*='ddlTipoValidacao_']").each(function () {
    //    MakeChosen($(this).attr("id"));
    //});

    $(".formato").on("click", function() {
        ExibeCampoDelimitador($(this).attr("id"));
    });

    AutoCompleteField("nome-layout", "hdnLayoutId", "lblMessageResult", "/Layout/SuggestionLayout");

    ConfigTabelaGridSemCampoFiltroPrincipal();
});

function AutoCompleteField(idField, idHidden, idMessageResult, url) {
    $("#"+idField).autocomplete({
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

                $("#div-campos-exportacao").html(response.divGridCampos);
                $("#div-formato").html(response.divGridFormato);
                
                BloquearCamposParaVisualizacao(false);
                ConfigTabelaGridSemCampoFiltroPrincipal();

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

function MakeChosen(id, maxSelectedOptions) {
    if (maxSelectedOptions === undefined || maxSelectedOptions === null)
        maxSelectedOptions = $("select#" + id + " option").length;

    $("select#" + id).chosen("destroy");
    $("#" + id).chosen({
        allow_single_deselect: true,
        no_results_text: "Oops, item não encontrado!",
        width: "100%",
        max_selected_options: maxSelectedOptions
    }).trigger("chosen:updated");
}

function SelecionarTodos(checked) {
    $("input[id*='chk_']").each(function () {
        this.checked = checked;
    });
}

function AdicionarCampo() {
    var campo = $("#importFields").val();
    if (campo == "" || campo == null) {
        toastr.warning("Selecione um campo para adicionar a lista");
        return;
    }

    $.ajax({
        url: "/Layout/AddCampoExportacao",
        type: "POST",
        dataType: "json",
        data: { campo: campo },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-campos-exportacao").html(response.divGrid);
                ConfigTabelaGridSemCampoFiltroPrincipal();
            }

            $("#btnRemoveField").css("display", response.ExibirBotaoExcluir ? "" : "none");

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

function DeletarSelecionados() {
    var listaCampo = [];
    $("input[id*='chk_']:checked").each(function () {
        var idEle = $(this).attr("Id");
        var id = idEle.toString().substring(4).split("_")[1];
        var campo = idEle.toString().substring(4).split("_")[0];
        listaCampo.push({ Id: id, Campo: campo });
    });

    if (listaCampo.length == 0) {
        toastr.warning("Selecione um ou mais itens para remover da listagem!");
        return;
    }

    $.ajax({
        url: "/Layout/DeleteCampoExportacao",
        type: "POST",
        dataType: "json",
        data: { listaCampo: JSON.stringify(listaCampo) },
        success: function (response) {

            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-campos-exportacao").html(response.divGrid);
                ConfigTabelaGridSemCampoFiltroPrincipal();
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

function ChangeData(ele) {
    var idEle = $(ele).attr("id").split("-")[1];

    var id = idEle.split("_")[1];
    var campo = idEle.split("_")[0];
    var posicao = $("input[id='posicao-" + idEle + "']").val();
    var formatacao = $("input[id='formatacao-" + idEle + "']").val();

    $.ajax({
        url: "/Layout/ChangeCampoExportacao",
        type: "POST",
        dataType: "json",
        data: { id: id, campo: campo, posicao: posicao, formatacao: formatacao},
        success: function (response) {
            
            if (response.message) {
                toastr.warning(response.message);
            } else {
                $("#div-campos-exportacao").html(response.divGrid);
                ConfigTabelaGridSemCampoFiltroPrincipal();
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

                    $("#div-campos-exportacao").html(response.divGridCampos);
                    $("#div-formato").html(response.divGridFormato);

                    ConfigTabelaGridSemCampoFiltroPrincipal();

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

                $("#div-campos-exportacao").html(response.divGridCampos);

                ConfigTabelaGridSemCampoFiltroPrincipal();

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
    var funcao = function() {
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
                ConfigTabelaGridSemCampoFiltroPrincipal();

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
    $("#importFields").val("").trigger("chosen:updated");
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
        $(".grid-field").each(function() { $(this).prop("disabled", "disabled"); });
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


function Etapa2() {
    var model = [];
    $("select[id*='importFields_']").each(function () {
        var campos = {};
        var destino = $(this).val();
        var id = $(this).attr("id").split("_");
        if (destino !== "") {
            var origem = $("#lblOrigem_" + id[1]).text();
            var validador = $("#ddlTipoValidacao_" + id[1]).val();
            campos["CampoDestino"] = destino;
            campos["CampoOrigem"] = origem;
            campos["Validador"] = validador;
            model.push(campos);
        }
            
    });

    $.ajax({
        url: "/ArquivoImportacao/Etapa2",
        type: "POST",
        data: { listaOrigens: model },
        success: function (result) {
            if (!result.Sucesso)
                openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });
            window.location.href = "/ArquivoImportacao/ArquivoConfiguracao/";
        }
    });

}




