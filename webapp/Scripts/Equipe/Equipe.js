var colaboradores = [];
var indiceColaboradoresAdicionados = 0;
var unidadeId = 0;
var horarioTrabalhoVar = 0;
$(document).ready(function () {

    $(function () {

        FormatarCampoData("date");

        $("#EquipeForm").submit(function (e) {
            if (ValidarCampos()) {
                return true;
            }

            return false;
        });

        $('#unidadeEquipe').change(function () {
            var idUnidade = $(this).find(':selected').val();
            unidadeId = idUnidade;
            showLoading();
            $.ajax({
                url: '/Equipe/BuscarHorarioTrabalho',
                type: 'POST',
                dataType: 'json',
                data: { idUnidade: idUnidade },
                success: function (response) {
                    CarregaHorarioTrabalho(response.typeItemList);
                    hideLoading();
                },
                complete: function () {
                    hideLoading();
                }
            });
        });
    });

    $.ajax({
        url: '/Equipe/BuscarColaboradores',
        type: 'POST',
        dataType: 'json',
        success: function (response) {
            colaboradores = response;
        }
    });

    buscarEquipes();

    formartarDropDowLists();

});

function formartarDropDowLists() {

    MakeChosen("unidadeEquipe");
    MakeChosen("tipoEquipe");
    MakeChosen("encarregadoEquipe");
    MakeChosen("supervisorEquipe");
    MakeChosen("horarioTrabalho");
    MakeChosen("TurnoId");
    MakeChosen("ColaboradorID");


}


function adicionarColaborador() {
    //debugger;
    if ($("#TurnoId").val() === "") {
        toastr.error("O campo \"Turno\" é obrigatório");
        return false;
    }

    if ($("#ColaboradorID").val() === ""){
        toastr.error("O campo \"Colaborador\" é obrigatório");
        return false;
    }
       

    indiceColaboradoresAdicionados--;
    var colaborador = {
        Id: indiceColaboradoresAdicionados,
        IdTurno: $("#TurnoId").val(),
        IdColaborador: $("#ColaboradorID").val()
    };

   

    colaboradores.push(colaborador);

    atualizarColaboradores(colaboradores);

}

function validarColaborador({ IdTurno }) {
    if (IdTurno === "") {
        toastr.warning("informe um Turno", "Alerta");
    }
    else {
        return true;
    }

    return false;
}

function atualizarColaboradores(colaboradores) {
    showLoading();
    $.post("/Equipe/AtualizarColaboradores", { colaboradores })
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
                $("#lista-colaboradores-result").empty();
                $("#lista-colaboradores-result").append(response);
            }
        })
        .fail(() => { })
        .always(() => {  });
    showLoading();
    $.ajax({
        url: '/Equipe/AtualizarHorarioTrabalho',
        type: 'POST',
        dataType: 'json',
        data: { valorHorarioTrabalho: horarioTrabalhoVar, unidadeId: unidadeId },
        success: function (horarios) {
            //debugger;
            var horarioSelect = document.getElementById("turnos");
            horarioSelect.innerHTML = "";

            var option = document.createElement("option");
            option.text = "Selecione o Turno";
            option.value = 0;
            horarioSelect.options.add(option);
            $.each(horarios, function (i, item) {
                option = document.createElement("option");
                //option.text = item.Nome;
                //option.value = item.Id;
                option.text = item.Text;
                option.value = item.Value;
                horarioSelect.options.add(option);
            });
            MakeChosen("turnos");
        }
    });
    hideLoading();
}


function removerColaborador(id) {
    colaboradores = colaboradores.filter(x => x.Id !== id);

    atualizarColaboradores(colaboradores);
}

var colaboradorEmEdicao = {};
function editarColaborador(id) {
    //debugger;
    if (Object.keys(colaboradorEmEdicao).length)
        colaboradores.push(colaboradorEmEdicao);

    colaboradorEmEdicao = colaboradores.find(x => x.Id === id);
    removercolaborador(id);

    $("#id").val(colaboradorEmEdicao.Id);
    $("#nomecolaborador").val(colaboradorEmEdicao.Descricao);
    $("#valorcolaborador").val(colaboradorEmEdicao.Valor);
    $("#cancel").show();
}



function CarregaHorarioTrabalho(horarios) {
    //debugger;
    var horarioSelect = document.getElementById("horarioTrabalho");
    horarioSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione o horário de trabalho";
    option.value = 0;
    horarioSelect.options.add(option);
    $.each(horarios, function (i, item) {
        option = document.createElement("option");
        //option.text = item.Nome;
        //option.value = item.Id;
        option.text = item.Text;
        option.value = item.Value;
        horarioSelect.options.add(option);
    });
    MakeChosen("horarioTrabalho");
}

function buscarEquipes() {
    BuscarPartialSemFiltro("/Equipe/BuscarEquipes", "#lista-tabela-equipe")
        .done(function () {
            ConfigTabelaGridSemCampoFiltroPrincipal();
        });

    if ($("#unidadeEquipe").val() !== "") {
        var idUnidade = $("#unidadeEquipe").val();
        showLoading();
        $.ajax({
            url: '/Equipe/BuscarHorarioTrabalho',
            type: 'POST',
            dataType: 'json',
            data: { idUnidade: idUnidade },
            success: function (response) {

                debugger;
                //CarregaHorarioTrabalho(response);
                CarregaHorarioTrabalho(response.typeItemList);

                $("#horarioTrabalho").val(response.valorHorarioTrabalho).trigger("chosen:updated");

                hideLoading();
            }
        });
    }

}

function selecionarHorarioTrabalho() {
    $.ajax({
        url: '/Equipe/SelecionarHorarioTrabalho',
        type: 'POST',
        dataType: 'json',
        success: function (idHorarioSelecionado) {
            ////debugger;
            $("#horarioTrabalho").val(idHorarioSelecionado);
            MakeChosen("horarioTrabalho");
        }
    });
}


function carregarTurno(turnos) {
    //debugger;
    var turnoSelect = document.getElementById("turnos");
    turnoSelect.innerHTML = "";

    var option = document.createElement("option");
    option.text = "Selecione o Turno";
    option.value = 0;
    turnoSelect.options.add(option);
    $.each(turnos, function (i, item) {
        option = document.createElement("option");
        option.text = item.Text;
        option.value = item.Value;
        turnoSelect.options.add(option);
    });
    MakeChosen("turnos");
}



function buscarTurnoColaborador() {

    var idColaborador = $("#ColaboradorID").val();
    showLoading();
    $.ajax({
        url: '/Equipe/BuscarTurnoColaborador',
        type: 'POST',
        dataType: 'json',
        data: { idColaborador: idColaborador },
        success: function (response) {

            debugger;
           
            carregarTurno(response.listaTurno);

            //$("#horarioTrabalho").val(response.valorHorarioTrabalho).trigger("chosen:updated");

            hideLoading();
        }
    });




}



function ValidarCampos() {
    if ($("#nomeEquipe").val() === "") {
        toastr.error("O campo \"Nome da Equipe\" é obrigatório");
        return false;
    }
    if ($("#unidadeEquipe").val() === "") {
        toastr.error("O campo \"Unidade\" é obrigatório");
        return false;
    }
    if ($("#tipoEquipe").val() === "") {
        toastr.error("O campo \"Tipo Equipe\" é obrigatório");
        return false;
    }
    if ($("#Datafim").val() === "") {
        toastr.error("O campo \"Data Fim\" é obrigatório");
        return false;
    }
    if ($("#horarioTrabalho").val() === "0") {
        toastr.error("O campo \"Horário de Trabalho\" é obrigatório");
        return false;
    }
    if ($("#encarregadoEquipe").val() === "") {
        toastr.error("O campo \"Encarregado Equipe\" é obrigatório");
        return false;
    }
    if ($("#supervisorEquipe").val() === "") {
        toastr.error("O campo \"Supervisor\" é obrigatório");
        return false;
    }
    return true;

}