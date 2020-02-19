var listahorarioparametro = [];
var items = [];

$(document).ready(function () {
    FormatarCampoHora(".hora");
    FormatarCampos();

    if (isEdit()) {
        showLoading();
        var id = location.pathname.split('/').pop();

        $.post(`/TabelaPrecoMensalista/BuscarDadosDosGrids/${id}`)
            .done((response) => {
                items = response.items;
            })
            .always(() => { hideLoading(); })
    }

    $("#diascalculo").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $("#diasparacorte").keydown(function (e) {
        if (!SomenteNumero(e)) {
            e.preventDefault();
            return false;
        }
    });

    $("[type='time']").focus(function () {
        if ($(this).val() === "")
            $(this).val("00:00");
    });

    MetodoUtil();
});

function AjustaDropdowDinamicos() {
    $("#unidade").hide();

    var selMain = document.getElementById("unidade");

    var sel = document.getElementsByName("unidade")
    for (var i = 0; i < sel.length; i++) {
        var index = sel[i].getAttribute('data-value');

        sel[i].innerHTML = selMain.innerHTML;

        $("select[data-value=" + index + "]").val(index);
    }
}

function HabilitaDesabilitaCampo(x) {
    var valor = false;
    if ($(x).is(':checked'))
        valor = false;
    else
        valor = true;

    $('#quantidadehoras').prop("disabled", valor);
    $('#quantidadehoras').val('');
    $('#valorquantidade').prop("disabled", valor);
    $('#valorquantidade').val('');

}

function validarItens(nome, valor, diascalculo) {

    if (!nome) {
        toastr.error('Informe um Nome!', 'Informe um Nome!');
        return false;
    }

    if (!valor) {
        toastr.error('Informe um Valor Total do Mês!', 'Informe um Valor Total do Mês!');
        return false;
    }

    if (!diascalculo) {
        toastr.error('Informe Dias para Cálculo!', 'Informe Dias para Cálculo!');
        return false;
    }

    return true;
}

function validarSubItens(unidade) {

    if (unidade <= 0) {
        return false;
    }

    return true;
}

function SalvarDados() {

    var listahorarioparametro = [];
    var Id = $("#hdnTabelaPrecoMensalista").val();

    var nome = $("#nome").val();
    var valor = $("#valor").val();

    var diascalculo = $("#diascalculo").val();
    
    if (!validarItens(nome, valor, diascalculo))
        return;

    var model = {
        Id: Id,
        Nome: nome,
        Valor: valor,
        DiasCalculo: diascalculo
    }


    $.ajax({
        url: "/tabelaprecomensalista/SalvarDados",
        type: "POST",
        data: { model },
        success: function (response) {
            AtualizarAbrirModal(response);
        },
        beforeSend: function () {
            showLoading();
        },
        complete: function () {
            hideLoading();
        } 
    });
}

function AdicionaEquipamento() {

    var escopo = $('#teste tbody');

    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    var datetoday = day + "/" + month + "/" + year;

    escopo.append(`
           <tr>
               <td>Unidade:<select class="chosen-select" style="margin: 10px"  name="unidade"></select></td>
               <td>Horário Início:<input name="horarioinicio" type="time" id="0" class="horario" style="margin: 10px"  value="" placeholder="" /></td>
               <td>Horário Fim:<input name="horariofim" type="time" id="0" class="horario"  style="margin: 10px" value="" placeholder="" /></td>
               <td>Hora Adicional:<input name="horaadicional" type="checkbox" id="0" onclick="HabilitaDesabilitaCampo(this)" style="margin: 10px" value="" placeholder="" /></td>
               <td>Qtd Hora(s):<input name="quantidadehoras" type="number" id="0" disabled style="margin: 10px" value="" placeholder="" /></td>
               <td>Valor por Qtd:<input name="valorquantidade" class="form-control heightfield valmoney" type="text" id="0" disabled style="margin: 10px; width: auto" value="" placeholder="" /></td>

               <td><span class="btn btn-danger btn-circle" data-idequipamento="0"  style="margin: 10px" id="deleta-equipamento"  onclick="DeletaEquipamento(this)"><i class="fa" style="color: white">X</i>&nbsp;</span></td>
           </tr>
        `);

    var selMain = document.getElementById("unidade");

    var sel = document.getElementsByName("unidade")
    for (var i = 0; i < sel.length; i++) {
        if (sel[i].options.length === 0) {
            sel[i].innerHTML = sel[i].innerHTML + selMain.innerHTML;
        }
    }

    FormatarCampos();
}

function FormatarCampos() {

    MakeChosen("unidade");
    MakeChosen("TabelaPreco");
    MakeChosen("TipoPreco");
    MakeChosen("TipoNegociacao");
    MakeChosen("Perfil");

    $("input[class*=valmoney]").maskMoney({
        prefix: "",
        allowNegative: false,
        allowZero: true,
        thousands: ".",
        decimal: ",",
        affixesStay: false
    });
}

var indiceitemsAdicionados = 0;

function adicionarItem() {

    let valido = true;

    var item = {
        Id: indiceitemsAdicionados,
        Unidade: {
            Id: $("#unidade option:selected").val(),
            Nome: $("#unidade option:selected").text()
        },
        QuantidadeHoras: $("#quantidadehoras").val(),
        ValorQuantidade: $("#valorquantidade").val(),
        DiasParaCorte: $("#diasparacorte").val(),
        HorarioInicio: $("#horarioinicio").val(),
        HorarioFim: $("#horariofim").val(),
        HoraAdicional: $("#horaadicional").is(':checked'),
    };

    if (item.Unidade.Id === "0") {
        toastr.warning("Selecione uma Unidade", "Alerta");
        valido = false;
    }

    let horaInicio = $("#horarioinicio").val();
    let horaFim = $("#horariofim").val();
    let diasparacorte = $("#diasparacorte").val();

    if (!horaFim) {
        toastr.error('O campo \"Horário Fim\" deve ser preenchido no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    } else if (horaFim === '0' || parseInt(horaFim) > 24) {
        toastr.error('O campo \"Horário Fim\" deve ser preenchido com horário no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    }

    if (!horaInicio) {
        toastr.error('O campo \"Horário Início\" deve ser preenchido no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    } else if (horaInicio === '0' || parseInt(horaInicio) > 24) {
        toastr.error('O campo \"Horário Início\" deve ser preenchido com horário no formato \"HH:mm\" do tipo 24hs!');
        valido = false;
    }

    if (!diasparacorte) {
        toastr.error('Informe Dias para Corte do pagamento da mensalidade integral!', 'Informe Dias para Corte!');
        return false;
    }

    if ($("#horaadicional").is(':checked')) {
        if (!item.QuantidadeHoras) {
            toastr.warning("Informe uma quantidade horas", "Alerta");
            valido = false;
        }
        else if (!item.ValorQuantidade) {
            toastr.warning("Informe um valor quantidade", "Alerta");
            valido = false;
        }
    }

    if (!valido)
        return false;

    indiceitemsAdicionados--;

    items.push(item);

    atualizarItems(items);



    $("#unidade").val("0");
    $("#quantidadehoras").val('');
    $("#valorquantidade").val('');
    $("#horarioinicio").val('');
    $("#horariofim").val('');
    $("#horaadicional").val('');
    $("#diasparacorte").val('');
    itemEmEdicao = {};
    $("#cancel").hide();
    //clearFieldsAddress();
}

function cancelarEdicaoDeitem() {
    items.push(itemEmEdicao);

    atualizarItems(items);

    $("#unidade").val("0");
    $("#quantidadehoras").val('');
    $("#valorquantidade").val('');
    $("#horarioinicio").val('');
    $("#horariofim").val('');
    $("#horaadicional").val('');
    $("#diasparacorte").val('');
    itemEmEdicao = {};
    $("#cancel").hide();
}


function atualizarItems(items) {
    showLoading();
    $.post("/TabelaPrecoMensalista/atualizarItems", { items })
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
                $("#lista-item-result").empty();
                $("#lista-item-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => { hideLoading(); });
}

function removerItem(id) {
    items = items.filter(x => x.Id !== id);

    atualizarItems(items);
}

var itemEmEdicao = {};
function editarItem(id) {
    if (Object.keys(itemEmEdicao).length)
        items.push(itemEmEdicao);

    itemEmEdicao = items.find(x => x.Id === id);
    removerItem(id);
    
    $("#id").val(itemEmEdicao.Id);
    $("#unidade").val(itemEmEdicao.Unidade.Id);

    $("#horarioinicio").val(itemEmEdicao.HorarioInicio);
    $("#horariofim").val(itemEmEdicao.HorarioFim);
    $("#diasparacorte").val(itemEmEdicacao.DiasParaCorte);
    $('#horaadicional').prop('checked', itemEmEdicao.HoraAdicional);

    $('#quantidadehoras').prop("disabled", itemEmEdicao.HoraAdicional ? false : true);
    $('#quantidadehoras').val('');
    $('#valorquantidade').prop("disabled", itemEmEdicao.HoraAdicional ? false : true);
    $('#valorquantidade').val('');

    if (itemEmEdicao.HoraAdicional) {
        $("#quantidadehoras").val(itemEmEdicao.QuantidadeHoras);
        $("#valorquantidade").val(itemEmEdicao.ValorQuantidade);
    }

    selecionarTipoDeitem(itemEmEdicao.Unidade.Id);

    $("#cancel").show();
}

function selecionarTipoDeitem(indice) {
    $("#unidade_chosen .chosen-results .result-selected").removeClass("result-selected");
    $(`#unidade_chosen .chosen-results .active-result[data-option-array-index='${indice}']`).addClass("result-selected");
    $("#unidade_chosen .chosen-single span").text($(`#unidade option[value='${indice}']`).text());
    $("#unidade").val(indice);
}
