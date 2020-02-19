var enderecos = [];

$(document).ready(function () {
    $("#zipcode").on("keyup", function (e) {
        $(this).val(
            $(this).val()
                .replace(/\D/g, "")
                .replace(/^(\d{5})?(\d{3})/, "$1-$2"));
    });

    $("#search-icon").on("click",
        function () {
            getInformationFromCEP();
        });

    $("#zipcode").blur(function () {
        getInformationFromCEP();
    });

    if (isEdit()) {
        showLoading();
        var id = location.pathname.split('/').pop();

        $.post(`/cliente/BuscarDadosDosGrids/${id}`)
            .done((response) => {
                enderecos = response.enderecos;
                contatos = response.contatos;
                veiculos = response.veiculos;

                if (typeof CarregaVeiculo !== "undefined") {
                    CarregaVeiculo();
                }
            })
            .always(() => { hideLoading(); });
    } else if (location.pathname.includes("salvardados")) {
        showLoading();
        $.post(`/cliente/BuscarDadosDosGridsEmSessao`)
            .done((response) => {
                enderecos = response.enderecos;
                contatos = response.contatos;
                veiculos = response.veiculos;

                if (typeof CarregaVeiculo !== "undefined") {
                    CarregaVeiculo();
                }
            })
            .always(() => { hideLoading(); });
    }
});

function clear_form() {
    $("#address, #district, #city, #state").val("");
}

function getInformationFromCEP() {
    var cep = $("#zipcode").val().replace(/\D/g, "");
    if (cep !== "") {
        var validacep = /^[0-9]{8}$/;
        if (validacep.test(cep)) {
            $("#invalid_zipcode").hide();
            $("#address, #district, #city, #state").val("...");

            $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                if (!("erro" in dados)) {
                    $("#address").val(dados.logradouro);
                    $("#district").val(dados.bairro);
                    $("#city").val(dados.localidade);
                    $("#state").val(dados.uf);
                }
                else {
                    clear_form();
                    toastr.error("CEP não encontrado.");
                }
            });
        }
        else {
            clear_form();
        }
    }
    else {
        $("#invalid_zipcode").hide();
        clear_form();
    }
}

var indiceEnderecosAdicionados = 0;
function adicionarEndereco() {
    indiceEnderecosAdicionados--;
    var endereco = {
        Id: indiceEnderecosAdicionados,
        Tipo: $("#type option:selected").val(),
        Cep: $("#zipcode").val(),
        Cidade: {
            Descricao: $("#city").val(),
            Estado: {
                Sigla: $("#state").val()
            }
        },
        CidadeDescricao: $("#city").val(),
        Estado: $("#state").val(),
        Logradouro: $("#address").val(),
        Numero: $("#number").val(),
        Bairro: $("#district").val(),
        Complemento: $("#complement").val(),
        Blacklist: $("#blacklist").val(),
        TipoMotivo: $("#tipoMotivo").val(),
        DescricaoMotivo: $("#descricaoMotivo").val()
    };

    if (!validarEndereco(endereco))
        return;

    enderecos.push(endereco);

    atualizarEnderecos(enderecos);

    clearFieldsAddress();
}

var enderecoEmEdicao = {};
function editarEndereco(id) {
    if (Object.keys(enderecoEmEdicao).length)
        enderecos.push(enderecoEmEdicao);

    enderecoEmEdicao = enderecos.find(x => x.Id === id);
    removerEndereco(id);

    $("#type").val(enderecoEmEdicao.Tipo);
    $("#id").val(enderecoEmEdicao.Id);
    $("#zipcode").val(enderecoEmEdicao.Cep);
    $("#city").val(enderecoEmEdicao.Cidade.Descricao);

    if (enderecoEmEdicao.Estado !== null)
        $("#state").val(enderecoEmEdicao.Estado);
    else if (enderecoEmEdicao.Cidade.Estado.Sigla !== null)
        $("#state").val(enderecoEmEdicao.Cidade.Estado.Sigla);

    $("#address").val(enderecoEmEdicao.Logradouro);
    $("#number").val(enderecoEmEdicao.Numero);
    $("#district").val(enderecoEmEdicao.Bairro);
    $("#complement").val(enderecoEmEdicao.Complemento);
    $("#blacklist").val(enderecoEmEdicao.Blacklist);
    $("#tipoMotivo").val(enderecoEmEdicao.TipoMotivo);
    $("#descricaoMotivo").val(enderecoEmEdicao.DescricaoMotivo);
    selecionarTipoDeEndereco(enderecoEmEdicao.Tipo);

    $("#cancel").show();

    VisibilidadeMsgSemEnderecos();
}

function removerEndereco(id) {
    enderecos = enderecos.filter(x => x.Id !== id);

    atualizarEnderecos(enderecos);
}

function cancelarEdicaoDeEndereco() {
    enderecos.push(enderecoEmEdicao);

    atualizarEnderecos(enderecos);

    clearFieldsAddress();
}

function atualizarEnderecos(enderecos) {
    showLoading();
    $.post("/Cliente/AtualizarEnderecos", { enderecos })
        .done((response) => {
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
                $("#lista-endereco-result").empty();
                $("#lista-endereco-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function validarEndereco({ Tipo, Cep }) {
    if (Tipo === 0) {
        toastr.warning("Selecione um Tipo de Locação", "Alerta");
    }
    else if (!Cep) {
        toastr.warning("Informe o cep", "Alerta");
    }else {
        return true;
    }

    return false;
}

function removeHiddenItemsAddress() {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.remove();
}

function clearFieldsAddress() {
    enderecoEmEdicao = {};
    clear_form();
    selecionarTipoDeEndereco("0");
    $("#zipcode, #number, #complement").val("");
    $("#cancel").hide();
}

function selecionarTipoDeEndereco(indice) {
    $("#type_chosen .chosen-results .result-selected").removeClass("result-selected");
    $(`#type_chosen .chosen-results .active-result[data-option-array-index='${indice}']`).addClass("result-selected");
    $("#type_chosen .chosen-single span").text($(`#type option[value='${indice}']`).text());
    $("#type").val(indice);
}

function VisibilidadeMsgSemEnderecos() {
    var totalEnderecos = $("#data_table_address tbody tr").length;
    var totalEnderecosInvisiveis = $("#data_table_address tbody tr:hidden").length;
    if (totalEnderecos === 0 || totalEnderecos === totalEnderecosInvisiveis) {
        $("#data_table_address tbody")
            .append(
                "<tr class=\"odd\"><td valign=\"top\" colspan=\"2\" class=\"dataTables_empty\">Nenhum registro encontrado</td></tr>");
    } else {
        $("#data_table_address tbody tr.odd").remove();
    }
}

// Obsoleto
function saveAddress() {
    //if ($("#data_table_address tbody tr.odd").length > 0)
    //    $("#data_table_address tbody tr.odd").remove();

    var dt = $("#data_table_address tbody tr");
    dt.splice(0, 0);
    var obj = [];

    for (var i = 0; i < dt.length; i++) {
        var id = $("#" + i + "Id").val() !== "" ? $("#" + i + "Id").val() : 0;
        var blacklist = $("#" + i + "Blacklist").val() !== "" ? $("#" + i + "Blacklist").val() : false;
        var tipoMotivo = $("#" + i + "TipoMotivo").val() !== "" ? $("#" + i + "TipoMotivo").val() : 1;
        var descricaoMotivo = $("#" + i + "DescricaoMotivo").val();

        obj.push({
            Id: id,
            Tipo: $("#" + i + "Tipo").val(),
            Cep: $("#" + i + "Cep").val(),
            Logradouro: $("#" + i + "Logradouro").val(),
            Numero: $("#" + i + "Numero").val(),
            Bairro: $("#" + i + "Bairro").val(),
            Cidade: {
                Descricao: $("#" + i + "Cidade").val(),
                Estado: {
                    Sigla: $("#" + i + "Estado").val()
                }
            },
            CidadeDescricao: $("#city").val(),
            Estado: $("#" + i + "Estado").val(),
            Complemento: $("#" + i + "Complemento").val(),
            Blacklist: blacklist,
            TipoMotivo: tipoMotivo,
            DescricaoMotivo: descricaoMotivo
        });
    }

    showLoading();
    $.ajax({
        url: "/Cliente/AdicionarEndereco",
        datatype: "JSON",
        type: "GET",
        data: { json: JSON.stringify(obj) },
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
                $("#lista-endereco-result").empty();
                $("#lista-endereco-result").append(response);
                $("#lista-endereco-result table").dataTable();
            }
        }
    });
    hideLoading();

    if (obj.length === 0)
        VisibilidadeMsgSemEnderecos();
}

function editAddress(ele, index) {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsAddress();

    $("#type").val($("#" + index + "Tipo").val());
    $("#id").val($("#" + index + "Cep").val());
    $("#zipcode").val($("#" + index + "Cep").val());
    $("#city").val($("#" + index + "Cidade").val());
    $("#state").val($("#" + index + "Estado").val());
    $("#address").val($("#" + index + "Logradouro").val());
    $("#number").val($("#" + index + "Numero").val());
    $("#district").val($("#" + index + "Bairro").val());
    $("#complement").val($("#" + index + "Complemento").val());
    $("#blacklist").val($("#" + index + "Blacklist").val());
    $("#tipoMotivo").val($("#" + index + "TipoMotivo").val());
    $("#descricaoMotivo").val($("#" + index + "DescricaoMotivo").val());

    $("#cancel").show();
    $(ele.parentElement.parentElement).hide();

    VisibilidadeMsgSemEnderecos();
}

function removeAddress(ele) {
    ele.parentElement.parentElement.remove();
    saveAddress();

    VisibilidadeMsgSemEnderecos();
}

function cancelEditAddress() {
    var hiddenItems = $("#data_table_address tbody tr:hidden");
    if (hiddenItems) hiddenItems.show();
    clearFieldsAddress();

    VisibilidadeMsgSemEnderecos();
}
// Fim Obsoleto

