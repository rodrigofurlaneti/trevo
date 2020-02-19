var listahorarioparametro = [];

$(document).ready(function () {
	if ($("#UnidadeSelecionada").val()) {
		LoadTipoEquipe();
	}

	if ($("#TipoEquipeSelecionada").val()) {
		LoadEquipe();
	}

	$("#unidade").change(function () {
        $("#UnidadeSelecionada").val($("#unidade").val());
        LoadTipoEquipe();
    });

    $("#tipoEquipe").change(function () {
        $("#TipoEquipeSelecionada").val($("#tipoEquipe").val());
        LoadEquipe();
    });

    $(window).load(function () {

        AjustaDropdowDinamicos();
    });
});

function LoadEquipe(){
    $("#equipe").html("");

	var tipoequipeid = $("#TipoEquipeSelecionada").val();
	var unidadeid = $("#UnidadeSelecionada").val();

    $.ajax({
        url: '/parametroequipe/AtualizaEquipes',
        data: { unidadeid,tipoequipeid },
        dataType: "json",
        type: "POST",
        success: function (data) {
            var equipeSelect = document.getElementById("equipe");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione uma equipe";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(data, function (i, item) {
                option = document.createElement("option");
                option.text = item.Nome;
                option.value = item.Id;
                equipeSelect.options.add(option);
			});

			if ($("#EquipeSelecionada").val()) {
				$("#equipe").val($("#EquipeSelecionada").val());
			}
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    }); 
}

function LoadTipoEquipe() {
    $("#equipe").html("");

    var unidadeid = $("#unidade").val();
    $.ajax({
        url: '/parametroequipe/AtualizaTipoEquipes',
        data: { unidadeid},
        dataType: "json",
        type: "POST",
        success: function (data) {
            var equipeSelect = document.getElementById("tipoEquipe");
            equipeSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione o tipo Equipe";
            option.value = 0;
            equipeSelect.options.add(option);
            $.each(data, function (i, item) {
                option = document.createElement("option");
                option.text = item.Descricao;
                option.value = item.Id;
                equipeSelect.options.add(option);
            });

			if ($("#TipoEquipeSelecionada").val()) {
				$("#tipoEquipe").val($("#TipoEquipeSelecionada").val());
			}

            //$("#tipoEquipe").append("<option value=0>Selecione...</option>");

            //$.each(data, function (index, equipe) {
            //    $("#tipoEquipe").append("<option value=" + equipe.Id + ">" + equipe.Descricao + "</option>");
            //});
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
}


function AjustaDropdowDinamicos() {

    $("input[name=datainicio]").datepicker();

    $("input[name=datafim]").datepicker();

    $("input[name=datainicio]").each(function (index) {
        var valor = $(this).val().replace('00:00:00', '');

        $(this).val(valor);
    });

    $("input[name=datafim]").each(function (index) {
        var valor = $(this).val().replace('00:00:00', '');

        $(this).val(valor);
    });

    var selMain = document.getElementById("unidade");

    var sel = document.getElementsByName("unidade");
    for (var i = 0; i < sel.length; i++) {

        var index = sel[i].getAttribute('data-value');

        sel[i].innerHTML = selMain.innerHTML;

        $("select[data-value=" + index + "]").val(index);
    }
}

function myFunction(td) {


    var rowIndex = $(x).closest('tr').index();
    alert("Row index is: " + rowIndex);

    alert("Cell index is: " + td.parentElement.rowIndex);

}

function DeletaEquipamento(x) {


    var listahorarioparametro = [];

    var index = $(x).closest('tr').index();

    $("#teste tbody").find('tr').each(function (rowIndex, r) {

        var periodohorario = {
            Id: $(this).find('span').attr('data-idhorario'),
            Unidade: {
                Id: $(this).find('select[name="unidade"]').val()
            },
            DataInicio: $(this).find('input[name="datainicio"]').val(),
            DataFim: $(this).find('input[name="datafim"]').val(),
            HorarioInicio: $(this).find('input[name="horarioinicio"]').val(),
            HorarioFim: $(this).find('input[name="horariofim"]').val()
        };

        listahorarioparametro.push(periodohorario);

    });


    $.ajax({
        url: "/parametroequipe/deletaequipamento",
        type: "POST",
        dataType: "json",
        data: {
            index, listahorarioparametro
        },
        success: function (result) {
            if (typeof(result) === "object") {

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

                $("#lista-funcionamentos-result").empty();
                $("#lista-funcionamentos-result").append(result);
            }
        },
        error: function (error) {

            $("#lista-funcionamentos-result").empty();
            $("#lista-funcionamentos-result").append(error.responseText);
            //hideLoading();
        },
        beforeSend: function () {
            //showLoading();
        },
        complete: function () {
            AjustaDropdowDinamicos();
        }
    });

}

function validarItens(equipe) {

    if (equipe.selectedIndex <= 0) {
        toastr.error('Selecione uma Equipe!', 'Informe a Equipe!');
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
    var Id = $("#hdnParametroEquipe").val();

    var equipeid = $("#equipe").val();
    var ativo = $('[name=Ativo]:checked').length === 1 ? 'true' : 'false';

    var equipe = document.getElementById("equipe");

    if (!validarItens(equipe))
        return;

    var model = {
        Id: Id,
        Equipe: {
            Id: equipeid
        },
        Ativo: ativo
    };

    listahorarioparametro.length = 0;


    var itemsValidados = true;
    $("#teste tbody").find('tr').each(function (rowIndex, r) {

        if (!validarSubItens($(this).find('select[name="unidade"]').val()))
            itemsValidados = false;

        var periodohorario = {
            Id: $(this).find('span').attr('data-idhorario'),
            Unidade: {
                Id: $(this).find('select[name="unidade"]').val()
            },
            DataInicio: $(this).find('input[name="datainicio"]').val(),
            DataFim: $(this).find('input[name="datafim"]').val(),
            HorarioInicio: $(this).find('input[name="horarioinicio"]').val(),
            HorarioFim: $(this).find('input[name="horariofim"]').val()
        };

        listahorarioparametro.push(periodohorario);

    });

    if (!itemsValidados) {
        toastr.error('Necessário informar as Unidades para os Horários e Unidades!', 'Informe a Unidade!');
        return;
    }

    $.ajax({
        url: "/parametroequipe/SalvarDados",
        type: "POST",
        data: { model, listahorarioparametro },
        success: function (result) {
            hideLoading();
            openCustomModal(null, null, result.TipoModal, result.Titulo, result.Mensagem, false, null, function () { });

            if (result.Sucesso)
                window.location.href = "/parametroequipe/Index/";

        },
        beforeSend: function () {
            showLoading();
        }
    });
}


function AdicionaEquipamento() {
    
    var equipe = document.getElementById("equipe");

    if (!validarItens(equipe))
        return;

    var escopo = $('#teste tbody'); 

    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    var datetoday = day + "/" + month + "/" + year;

    escopo.append(`
           <tr>
               <td>Unidade:<select class="chosen-select" style="margin: 10px"  name="unidade"></select></td>
               
               <td>Data Início:<input name="datainicio" type="text" id="0"  class="datepicker"  style="margin: 10px" data-date-format="dd/mm/yyyy" autocomplete="off" /></td>
               <td>Data Fim:<input name="datafim" type="text" id="0" class="datepicker"  style="margin: 10px" data-date-format="dd/mm/yyyy" autocomplete="off" /></td>
               <td>Horário Início:<input name="horarioinicio" type="time" id="0" class="horario" style="margin: 10px"  value="" placeholder="" /></td>
               <td>Horário Fim:<input name="horariofim" type="time" id="0" class="horario"  style="margin: 10px" value="" placeholder="" /></td>

               <td><span class="btn btn-danger btn-circle" data-idequipamento="0"  style="margin: 10px" id="deleta-equipamento"  onclick="DeletaEquipamento(this)"><i class="fa" style="color: white">X</i>&nbsp;</span></td>
           </tr>
        `);

    var selMain = document.getElementById("unidade"); 

    var sel = document.getElementsByName("unidade");
    for (var i = 0; i < sel.length; i++) {
        if (sel[i].options.length === 0) {
            sel[i].innerHTML = sel[i].innerHTML + selMain.innerHTML;
        }
    }

    $("input[name=datainicio]").datepicker({ pickTime: false });
    $("input[name=datafim]").datepicker({ pickTime: false });

}


function FormatarCampoDataByName(pCampoName) {
    //$("input[name=" + pCampoName + "]").mask("99/99/9999");
    //$("input[name=" + pCampoName + "]").datepicker({
    //    dateFormat: "dd/mm/yy",
    //    dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
    //    dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S", "D"],
    //    dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
    //    monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
    //    monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
    //    nextText: ">",
    //    prevText: "<"
    //});


    $("input[name=" + pCampoName + "]").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Proximo',
        prevText: 'Anterior',
        showOn: "button"
    }).css("display", "inline-block")
        .next("button").button({
            icons: { primary: "ui-icon-calendar" },
            label: "Selecione uma data",
            text: false
        });
}

