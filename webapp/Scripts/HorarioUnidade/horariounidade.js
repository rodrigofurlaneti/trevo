var periodoitems = [];
var periodohorarioEmEdicao = {};
var indiceHorarioUnidadesAdicionados = 0;

$(document).ready(function () {
    MakeChosen("unidade", null, "100%");
    FormatarCampoHora(".hora");
    FormatarCampoData("data");

    EsconderGrids();

    $("#horariofixo").change(function () {
        var selecionado = $(this).is(':checked');

        if (selecionado == true) {
            $("#datafiltro").hide();
            $("#data").val('');
        }
        else {
            $("#datafiltro").show();
        }
    })

    $("#tipohorario").on("change", function () {

        var items = $(this).val();
        EsconderGrids();

        if (items != null) {
            for (i = 0; i < items.length; i++) {
                var idgrid = '#gridperiodo' + items[i];
                $(idgrid).show();
            }

            for (var p = 0; p < periodoitems.length; p++) {
                if (!items.includes(periodoitems[p].TipoHorario.toString())) {
                    periodoitems.splice(p, 1);
                    p--;
                }
            }
        }
        else {
            periodoitems.length = 0;
        }

        atualizaHorarioUnidades();
    })

    $(window).load(function () {
        // Run code
        var items = $("#tipohorario").val();

        EsconderGrids();

        if (items != null) {
            for (i = 0; i < items.length; i++) {
                var idgrid = '#gridperiodo' + items[i];
                $(idgrid).show();
            }
        }

        if (isEdit() || isSave())
            BuscarPeriodoHorarios();

        var selecionado = $("#horariofixo").is(':checked');

        if (selecionado == true) {
            $("#datafiltro").hide();
            $("#data").val('');
        }
        else {
            $("#datafiltro").show();
        }
    })

    MakeChosenMult("tipohorario");

    $("#Form").submit(function (e) {

        let dadosValidos = true;

        if (!$("#unidade").val()) {
            toastr.error("O campo \"Nome Horario\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!$("#nome").val()) {
            toastr.error("O campo \"Nome Horario\" deve ser preenchido!");
            dadosValidos = false;
        }

        if (!dadosValidos) {
            e.preventDefault();
            return false;
        }
    });

    //Todo: Refatorar isso depois PAULO
    if (!location.pathname.includes("cliente")) {
        ListaAtividades();
    }
});

function EsconderGrids() {
    $('#gridperiodo1, #gridperiodo2, #gridperiodo3, #gridperiodo4').hide();
}

function AdicionaHorarioUnidade(dia) {
    indiceHorarioUnidadesAdicionados--;

    var idGridDiaPeriodoadd = '#periodonovoadd' + dia;
    var idGridDiaInicioadd = '#datainicioadd' + dia;
    var idGridDiafinaladd = '#datafimadd' + dia;

    var periodo = $(idGridDiaPeriodoadd);

    var inicio = $(idGridDiaInicioadd).val();
    var final = $(idGridDiafinaladd).val();

    if (periodo.selectedIndex <= 0) {
        toastr.error('Informe um Período Válido!', 'Informe o Período!');
        return false;
    }

    if (inicio.trim() === '') {
        toastr.error('Informe hora Inicial Válida!', 'Informe hora Inicial!');
        return false;
    }


    if (final.trim() === '') {
        toastr.error('Informe hora Final Válida!', 'Informe hora Final!');
        return false;
    }

    var periodohorario = {
        Id: indiceHorarioUnidadesAdicionados,
        TipoHorario: dia,
        Periodo: $(idGridDiaPeriodoadd + " :selected").text(),
        Inicio: inicio,
        Fim: final
    }

    periodoitems.push(periodohorario);

    atualizaHorarioUnidades();

    $("input[name='periodo']").val('');

    periodohorarioEmEdicao = {};
}

function ListaAtividades() {
    var element = document.getElementById('hdnHorarioUnidade');
    if (typeof (element) != 'undefined' && element != null) {
        if (element.value != "") {

            var obj = [];

            var newObj = {
                Id: element.value
            };

            obj.push(newObj);

            $.ajax({
                url: '/horariounidade/ListaAtividades',
                type: 'POST',
                dataType: 'json',
                data: { horariounidade: JSON.stringify(obj) },
                success: function (response) {
                    if (response && response.length) {
                        periodoitems = response;
                        CarregaTipoAtividade();
                    }
                }
            })
        };

    }
}

function BuscarPeriodoHorarios() {
    return post("BuscarPeriodoHorarios")
        .done((response) => periodoitems = response);
}

function CarregaTipoAtividade() {

    $('#gridHorarioUnidades1 tbody').empty();
    $('#gridHorarioUnidades2 tbody').empty();
    $('#gridHorarioUnidades3 tbody').empty();
    $('#gridHorarioUnidades4 tbody').empty();

    $.each(periodoitems, function (i, periodohorario) {

        var idgrid = '#gridHorarioUnidades' + periodohorario.TipoHorario + ' tbody';

        $(idgrid)
            .append('<tr>'
            + '<td style="display:none">'
            + periodohorario.Id
            + '</td>'
            + '<td class="col-xs-10">'
            + periodohorario.Periodo
            + '</td>'
            + '<td class="col-xs-10">'
            + periodohorario.Inicio
            + '</td>'
            + '<td class="col-xs-10">'
            + periodohorario.Fim
            + '</td>'
            + '<td class="col-xs-2">'
            + '<span class="btn btn-primary" style="margin-right: 10px;" onclick="EditaHorarioUnidade(' + periodohorario.Id + ')")><i class="fa fa-edit"></i></span>'
            + '<span class="btn btn-danger" onclick="RemoveHorarioUnidade(' + periodohorario.Id + ')"><i class="fa fa-remove"></i></span>'
            + '</td>'
            + '</tr>'
            );
    });
}

function EditaHorarioUnidade(id) {
    if (Object.keys(periodohorarioEmEdicao).length) {
        periodoitems.push(periodohorarioEmEdicao);
    }


    periodohorarioEmEdicao = periodoitems.find(x => x.Id == id);

    periodoitems = periodoitems.filter(x => x.Id != periodohorarioEmEdicao.Id);

    console.table(periodohorarioEmEdicao);

    selecionaPeriodo("periodonovoadd" + periodohorarioEmEdicao.TipoHorario, periodohorarioEmEdicao.Periodo);


    $("#datainicioadd" + periodohorarioEmEdicao.TipoHorario).val(periodohorarioEmEdicao.Inicio);
    $("#datafimadd" + periodohorarioEmEdicao.TipoHorario).val(periodohorarioEmEdicao.Fim);

    atualizaHorarioUnidades();
}

function selecionaPeriodo(eID, text) {
    var ele = document.getElementById(eID);
    for (var ii = 0; ii < ele.length; ii++)
        if (ele.options[ii].text == text) {
            ele.options[ii].selected = true;
            return true;
        }
    return false;
}

function RemoveHorarioUnidade(id) {
    periodoitems = periodoitems.filter(x => x.Id != id);

    atualizaHorarioUnidades();
}

function atualizaHorarioUnidades() {
    post('AtualizaTipoAtividades', { jsonTipoAtividades: JSON.stringify(periodoitems) })
        .done(() => {
            CarregaTipoAtividade();
        })
}

function validarHorarioUnidade(marca) {
    if (marca.selectedIndex <= 0) {
        toastr.error('Selecione um item!', 'Informe o Tipo Atividade!');
        return false;
    }

    return true;
}